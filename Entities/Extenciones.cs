using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Media.Imaging;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.Office.Interop.Excel;
using System.Drawing;

namespace Entities
{

   
  public static class Extenciones
    {
        public static BitmapImage ToImage(this byte[] imageData)
        {
            if (imageData == null || imageData.Length == 0) return null;
            var image = new BitmapImage();
            using (var mem = new MemoryStream(imageData))
            {
                mem.Position = 0;
                image.BeginInit();
                image.CreateOptions = BitmapCreateOptions.PreservePixelFormat;
                image.CacheOption = BitmapCacheOption.OnLoad;
                image.UriSource = null;
                image.StreamSource = mem;
                image.EndInit();
            }
            image.Freeze();
            return image;
        }

        public static byte[] BitmapSourceToByteArray(this BitmapImage bitmap)
        {
         
            var encoder = new PngBitmapEncoder(); // or any other BitmapEncoder

            encoder.Frames.Add(BitmapFrame.Create(bitmap));

            using (var stream = new MemoryStream())
            {
                encoder.Save(stream);
                return stream.ToArray();
            }
        }

        public static System.Data.DataTable ToDataTable(this IEnumerable<Reporte> datos)
        {

            System.Data.DataTable table = new System.Data.DataTable();
            List<Reporte> temp = datos.ToList();

            table.Columns.Add("Hash");
            table.Columns.Add("UserName");
            table.Columns.Add("Fecha");
            table.Columns.Add("Incidente");
            table.Columns.Add("Turno");
            table.Columns.Add("HU");
            table.Columns.Add("Material");
            table.Columns.Add("Orden");
            table.Columns.Add("Departamento");
            table.Columns.Add("Estatus");
            table.Columns.Add("Descripcion");


            for (int i = 0; i < temp.Count; i++)
            {
               System.Data.DataRow row = table.NewRow();
                row["Hash"] = temp[i].Hash.ToString();
                row["UserName"] = temp[i].UserName;
                row["Fecha"] = temp[i].Fecha.ToString();
                row["Incidente"] = temp[i].Incidente;
                row["Turno"] = temp[i].Turno.ToString();
                row["HU"] = temp[i].Hu;
                row["Material"] = temp[i].Material;
                row["Orden"] = temp[i].Orden;
                row["Departamento"] = temp[i].Departamento.ToString();
                row["Estatus"] = temp[i].Estatus.ToString().Replace('_', ' ');
                row["Descripcion"] = temp[i].Descripcion;
                table.Rows.Add(row);

            }

            return table;

        }

        public static async Task ToExcelDocumentAsync(this System.Data.DataTable dataTable, string path = "")
        {
            await Task.Run(() =>
            {
                try
                {


                    Application excelFile = new Application();



                    excelFile.Workbooks.Add();
                    _Worksheet worksheet = (_Worksheet)excelFile.ActiveSheet;

                    var datatable = dataTable;
                    string[] columns = new string[datatable.Columns.Count];

                    for (int i = 0; i < columns.Length; i++)
                    {
                        columns[i] = datatable.Columns[i].ColumnName;
                    }
                    Range columnas = worksheet.Range[worksheet.Cells[1, 1], worksheet.Cells[1, datatable.Columns.Count]];

                    columnas.Value = columns;



                    columnas.Interior.Color = System.Drawing.Color.DimGray;
                    columnas.Font.Color = Color.White;
                    columnas.Font.Bold = true;




                    string[,] buf = new string[datatable.Rows.Count, datatable.Columns.Count];

                    for (int i = 0; i < datatable.Rows.Count; i++)
                    {
                        for (int j = 0; j < datatable.Columns.Count; j++)
                        {
                            buf[i, j] = datatable.Rows[i][j].ToString();
                        }
                    }

                    Range seleccion = worksheet.Range[worksheet.Cells[2, 1], worksheet.Cells[datatable.Rows.Count + 1, datatable.Columns.Count]];
                    seleccion.Value = buf;
                    seleccion.NumberFormat = "# ?/?"; // Formato de numero de fraccion.

                    if (!string.IsNullOrEmpty(path))
                    {
                        try
                        {
                            worksheet.SaveAs(path);
                            excelFile.Visible = true;

                        }
                        catch (Exception Error)
                        {
                            throw new Exception("Exportacion a excel: archivo de excel podo no haberse aguardado.", Error
                            );
                        }
                    }
                    else    // no filepath is given
                    {
                        excelFile.Visible = true;
                    }

                }
                catch (Exception Error)
                {

                    new LogManager(Error);

                }
            });
           
        }

        public static async Task  ToExcelDocumentAsync(this IEnumerable<Reporte> datos, string path = "")
        {

            await datos.ToDataTable().ToExcelDocumentAsync();

        }



    }
}
