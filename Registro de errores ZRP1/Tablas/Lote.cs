using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using ConsultaCore;

namespace Registro_de_errores_ZRP1.Tablas
{
    class Lote : IDateable
    {
        private int id;

        

        public string Tabla => "Lote";

        public List<PropertyInfo> Propiedades => typeof(Lote).GetProperties().ToList();

        public int Id { get => id; set => id = value; }

        public string NumeroDeLote { get; set; }

        public string Orden { get; set; }

        public override string ToString()
        {
            return NumeroDeLote;
        }

    }
}
