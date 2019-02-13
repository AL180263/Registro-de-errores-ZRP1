using Registro_de_errores_ZRP1.Tablas;
using ConsultaCore;
using System.Windows.Forms;
using iTextSharp.text.pdf;
using iTextSharp.text;
using System.Globalization;
using Registro_de_errores_ZRP1.Properties;
using Excel = Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;

namespace Registro_de_errores_ZRP1
{
    public static  class GeneradorDocument
    {
        private static Document document;

        private static PdfWriter gen;

        private static Excel._Application ExcelBook = new Excel.Application();

       

        public static void GenerarPdf(Problemas Reporte, CoreDataBaseAccess Database)
        {
            FolderBrowserDialog folder = new FolderBrowserDialog();

            if ( DialogResult.Cancel != folder.ShowDialog())
            {
                string path = string.Format("{0}\\{1}", folder.SelectedPath, string.Format("{0}_{1}_{2}.pdf", Reporte.HashCode, Reporte.Departamento, "REPORTE_DE_ERROR_ZRP1"));

                document = new Document(PageSize.LETTER);

                gen = PdfWriter.GetInstance(document, new System.IO.FileStream(path, System.IO.FileMode.Create));
                Font FuenteTitulos = new Font(Font.FontFamily.HELVETICA, 18, Font.BOLD);
                Font FuenteNormal = new Font(Font.FontFamily.HELVETICA, 16, Font.NORMAL);
                if (Database.ReadAsync<Usuarios>(string.Format("UserName=\"{0}\"", Reporte.Usuario)).Result.Count > 1)
                {
                    document.AddAuthor(Database.ReadAsync<Usuarios>(string.Format("UserName=\"{0}\"", Reporte.Usuario)).Result[0].ToString());
                }
                else
                {
                    document.AddAuthor("Desconocido");
                }
               
                document.AddTitle(Reporte.Problema);

                document.Open();

                Image logo = Image.GetInstance(Resources.Commscope, (BaseColor)null);
                logo.BorderWidth = 0;
                logo.Alignment = Element.ALIGN_LEFT;
                float porcentaje = 150 / logo.Width;
                logo.ScalePercent(porcentaje * 100);

                Image code = GenerarBarcode(Reporte.HashCode.ToString(),true);
                code.Alignment = Element.ALIGN_RIGHT;
                code.BorderWidth = 0;
                float scalado = (135 / code.Width) * 100;
                code.ScalePercent(scalado);
               
                   
                document.Add(logo);
                document.Add(new Chunk(code,380,-20));
                document.Add(new Paragraph(string.Format("Reporte de problema ZRP1 {0}{1}", "".PadRight(25), Reporte.Fecha.ToString("d / MMMM / yyyy", CultureInfo.CreateSpecificCulture("es-MX"))), new Font(Font.FontFamily.HELVETICA, 18, Font.BOLD)));
                document.Add(Chunk.NEWLINE);
                var d = new Paragraph("Clerck  : ", FuenteTitulos);
                if (Database.ReadAsync<Usuarios>(string.Format("UserName=\"{0}\"", Reporte.Usuario)).Result.Count > 0)
                {
                    d.AddSpecial(new Phrase(string.Format("({0}) {1}", Reporte.Usuario, Database.ReadAsync<Usuarios>(string.Format("UserName=\"{0}\"", Reporte.Usuario)).Result[0]), FuenteNormal));
                }
                else
                {
                    d.AddSpecial(new Phrase("Desconocido",FuenteNormal));
                }
                
                document.Add(d);
                document.Add(Chunk.NEWLINE);
                Paragraph a = new Paragraph("Estado : ", FuenteTitulos);
                a.AddSpecial(new Phrase(Reporte.EstatusString, FuenteNormal));
                document.Add(a);
                document.Add(Chunk.NEWLINE);
                var i = new Paragraph("Hora : ", FuenteTitulos);
                i.AddSpecial(new Phrase(Reporte.Fecha.ToLongTimeString(), FuenteNormal));
                document.Add(i);
                document.Add(Chunk.NEWLINE);
                var j = new Paragraph("Turno : ", FuenteTitulos);
                j.AddSpecial(new Phrase(Reporte.Turno, FuenteNormal));
                document.Add(j);
               
                document.Add(Chunk.NEWLINE);


                var b = new Paragraph("Problema : ", FuenteTitulos);
                b.AddSpecial(new Phrase(Reporte.Problema, FuenteNormal));
                document.Add(b);
                document.Add(Chunk.NEWLINE);


                var c = new Paragraph("Departamento del problema : ", FuenteTitulos);
                c.AddSpecial(new Phrase(Reporte.Departamento, FuenteNormal));
                document.Add(c);
                document.Add(Chunk.NEWLINE);
               
                var e = new Paragraph("Orden : ", FuenteTitulos);
                e.AddSpecial(new Phrase(Reporte.Orden, FuenteNormal));
                if (!string.IsNullOrEmpty(Reporte.Orden))
                {
                    e.Add(new Chunk(GenerarBarcode(Reporte.Orden,false), 10, -7));
                }
                document.Add(e);
                document.Add(Chunk.NEWLINE);

                var f = new Paragraph("Material : ", FuenteTitulos);
                f.AddSpecial(new Phrase(Reporte.Material, FuenteNormal));

                if (!string.IsNullOrEmpty(Reporte.Material))
                {
                    f.Add(new Chunk(GenerarBarcode(Reporte.Material,false),10,-7));
                }
                document.Add(f);
                document.Add(Chunk.NEWLINE);
                var g = new Paragraph("HU : ", FuenteTitulos);
                g.AddSpecial( new Phrase(Reporte.HU, FuenteNormal));
                if (!string.IsNullOrEmpty(Reporte.HU))
                {
                    g.Add(new Chunk(GenerarBarcode(Reporte.HU,false), 10, -7));
                }
                document.Add(g);
                document.Add(Chunk.NEWLINE);
              
                var h = new Paragraph("Informacion adicional : ", FuenteTitulos);
                h.Add(Chunk.NEWLINE);
                h.AddSpecial(new Phrase(Reporte.Informacion_Extra, FuenteNormal));
                document.Add(h);
                document.Add(Chunk.NEWLINE);





                document.Close();

                gen.Close();

                System.Diagnostics.Process.Start(path);
            }
            

           

        }

        private static Image GenerarBarcode(string code, bool conTitulo)
        {

            BarcodeLib.Barcode.Linear codigo = new BarcodeLib.Barcode.Linear();
            codigo.Type = BarcodeLib.Barcode.BarcodeType.CODE128;
            codigo.Data = code;
            codigo.ShowText = conTitulo;
            codigo.BarHeight = 25;


            System.Drawing.Bitmap BarcodeBmp = codigo.drawBarcode();
            Image image = Image.GetInstance(BarcodeBmp, (BaseColor)null);
            image.Alignment = Element.ALIGN_MIDDLE;
            image.BorderWidth = 0;
            float escalado = (150/ image.Width) * 100;
            image.ScalePercent(escalado);
            return image;
        }

        public static void GenerarExcel(List<Problemas>Errores)
        {
         
            Excel._Workbook workbook = ExcelBook.Workbooks.Add(Type.Missing);
            Excel._Worksheet worksheet = null;
            try
            {
                worksheet = workbook.ActiveSheet;
                worksheet.Name = string.Format("Reporte_{0}",DateTime.Now.ToShortDateString());


            }
            catch (Exception Error)
            {

               
            }
            finally
            {

            }
        }
     
       

    }

    
}
