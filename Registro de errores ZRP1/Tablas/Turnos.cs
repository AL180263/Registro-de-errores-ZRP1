using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConsultaCore;
using System.Reflection;

namespace Registro_de_errores_ZRP1.Tablas
{
    class Turnos:IDateable
    {

        public int Id { get; set; }

        public string Tabla { get; set; } = "Turnos";

        public List<PropertyInfo> Propiedades => typeof(Turnos).GetProperties().ToList();

        public string Turno { get; set; }

        public override string ToString()
        {
            return Turno;
        }
    }
}
