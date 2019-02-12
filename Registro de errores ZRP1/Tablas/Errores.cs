using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConsultaCore;
using System.Reflection;

namespace Registro_de_errores_ZRP1.Tablas
{
    class Errores: IDateable
    {
        public int Id { get; set; }

        public string Tabla { get; } = "Errores";

        public List<PropertyInfo> Propiedades { get; } = typeof(Errores).GetProperties().ToList();

        public string Error { get; set; }


        public override string ToString()
        {
            return Error;
        }

      
    }
}
