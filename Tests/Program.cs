using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Registro_de_errores_ZRP1;
using Registro_de_errores_ZRP1.Tablas;
using System.Windows.Forms;

namespace Tests
{
    class Program
    {
        static readonly Problemas ObjetoDePrueba = new Problemas { Departamento = "Calidad", Estatus = false, Fecha = DateTime.Now, HU = "18570569", Id = 1, Material = "49598A", Orden = "6003121868", Turno = "D", Problema = "Orden no se encuentra en zrp1", Informacion_Extra = "Mucha gente me ha preguntado recientemente como añadir o insertar imágenes en los documentos PDF que están creando con iTextSharp. Así que decidí publicar este pequeño artículo para demostrar los sencillo que es insertar imágenes en nuestros documentos PDF." };

        static void Main(string[] args)
        {

            GeneradorDocument.GenerarPdf(ObjetoDePrueba, new ConsultaCore.CoreDataBaseAccess(string.Format("Provider={0};Data Source =\"{1}\"", "Microsoft.ACE.OLEDB.12.0", @"D:\INFINITO HERNANDEZ GAMEZ\Escritorio\pdf prueba")));
            


        }
    }
}
