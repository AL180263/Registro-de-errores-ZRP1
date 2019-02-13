using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConsultaCore;
using System.Reflection;

namespace Registro_de_errores_ZRP1.Tablas
{
    public enum PERMISO 
    {
        Usuario,
        Intermedio,
        Administrador,
        Programador

    }

    class Usuarios:IdateableObject
    {
      


        private string userName;

        private string nombre;

        private string permiso;

             
        public Usuarios()
        {

        }
        public Usuarios(string userName, string nombre)
        {
            this.userName = userName;
            this.nombre = nombre;
        }

      
        public string UserName { get => userName; set => userName = value; }
        public string Nombre { get => nombre; set => nombre = value; }
        public string Permiso { get => permiso.ToUpperInvariant(); set => permiso = value.ToUpperInvariant(); }
        [Excluible]
        public PERMISO PermisoEnum
        {
            get
            {
                switch (permiso.ToLowerInvariant())
                {
                    case "administrador":
                        return PERMISO.Administrador;
                    case "intermedio":
                        return PERMISO.Intermedio;
                    case "usuario":
                        return PERMISO.Usuario;
                    case "programador":
                        return PERMISO.Programador;
                    default:
                        return PERMISO.Usuario;
                      
                }
            }
            set
            {
                switch (value)
                {
                    case PERMISO.Usuario:
                        permiso = "USUARIO";
                        break;
                    case PERMISO.Intermedio:
                        permiso = "INTERMEDIO";
                        break;
                    case PERMISO.Administrador:
                        permiso = "ADMINISTRADOR";
                        break;
                    case PERMISO.Programador:
                        permiso = "PROGRAMADOR";
                        break;
                    default:
                        permiso = "USUARIO";
                        break;
                }

            }
        }

        public override string ToString()
        {
            if (!string.IsNullOrEmpty(nombre))
            {
                return nombre;
            }
            else
            {
                return userName;
            }
            
        }

        public override bool Equals(object obj)
        {
            var usuario = obj as Usuarios;
            return usuario != null &&
                   userName == usuario.userName &&
                   nombre == usuario.nombre;
        }

        public override int GetHashCode()
        {
            var hashCode = 1420585889;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(userName);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(nombre);
            return hashCode;
        }
    }
}
