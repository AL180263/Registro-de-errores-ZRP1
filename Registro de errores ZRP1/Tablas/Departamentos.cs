using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConsultaCore;
using System.Reflection;

namespace Registro_de_errores_ZRP1.Tablas
{
    class Departamento:IDateable
    {
       
       
        [Excluible]
        public int Id { get; set; }
        [Excluible]
        public List<PropertyInfo> Propiedades { get { return typeof(Departamento).GetProperties().ToList();  } } 
        [Excluible]
        public string Tabla { get { return "Departamentos"; } }

        public string DepartamentosResponsables { get; set; }

        public override string ToString()
        {
            return DepartamentosResponsables;
        }


    }
}
