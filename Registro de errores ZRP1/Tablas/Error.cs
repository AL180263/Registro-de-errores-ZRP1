using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using ConsultaCore;

namespace Registro_de_errores_ZRP1.Tablas
{
  public  class Error : IDateable
    {
        private readonly List<PropertyInfo> propiedades = typeof(Error).GetProperties().ToList();

        #region implementacion de IDateable
        [Excluible]
        public int Id { get; set; }
        [Excluible]
        public string Tabla { get => "Problemas"; }
        [Excluible]
        public List<PropertyInfo> Propiedades
        {
            get
            {
                propiedades.RemoveAll(x => x.Name == "Propiedades" || x.Name == "Tabla" || x.Name == "EstatusString");

                return propiedades;
            }
        }

        #endregion

        public string EstatusString
        {
            get
            {
                if (Estatus)
                {
                    return "Resuelto";
                }
                else
                {
                    return "Sin Resolver";
                }
            }
            set
            {
                if (value == "Resuelto")
                {
                    Estatus = true;
                }
                else if (value == "Sin resolver")
                {
                    Estatus = false;
                }
                else
                {
                    Estatus = false;
                }
               
            }
        }

        public string Orden { get; set; }

        public string Problema { get; set; }
        [Excluible]
        public bool Estatus { get; set; }

        public DateTime Fecha { get; set; }

        public string Usuario { get; set; }

        public string Material { get; set; }

        public string HU { get; set; }

        public string Turno { get; set; }

        public string Departamento { get; set; }

        public string Informacion_Extra { get; set; }

        public double HashCode { get; set; }


        public override int GetHashCode()
        {
            var hashCode = 230498;
          
            hashCode = hashCode * -1521135 + EqualityComparer<string>.Default.GetHashCode(Problema);
            hashCode = hashCode * 15211345 + EqualityComparer<string>.Default.GetHashCode(Usuario);
            hashCode = hashCode * 15211345 + EqualityComparer<string>.Default.GetHashCode(Departamento);
            hashCode = hashCode * 15211345 + EqualityComparer<string>.Default.GetHashCode(Fecha.ToString());
            hashCode = hashCode * 15211345 + EqualityComparer<string>.Default.GetHashCode(Turno);
            if (hashCode > 0)
            {
                return hashCode;
            }
            else
            {
                return -hashCode;
            }
        }

        public override string ToString()
        {
            return Problema;
        }


    }
}
