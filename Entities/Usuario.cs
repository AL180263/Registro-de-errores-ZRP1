using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace Entities
{

    public enum Access
    {
        Invitado,
        Normal,
        Administrador,
        Programador


    }

    public partial class Usuario
    {

        public Usuario()
        {
            this.Reportes = new HashSet<Reporte>();
        }

        public Usuario(string UserName,string Nombre, string Reloj,Access Acceso)
        {
            this.Reportes = new HashSet<Reporte>();
            this.UserName = UserName;
            this.Nombre = Nombre;
            this.Reloj = Reloj;
            this.Acceso = Acceso;
        }

        [Key]
        [StringLength(6)]
        [Required]
        public string UserName { get; set; }

        [Required]
        [StringLength(6)]
        public string Reloj { get; set; }

        public string Contraseña { get; set; }

        public string Nombre { get; set; }

        public Access Acceso { get; set; }

        public byte[] Imagen { get; set; }

        public virtual ICollection<Reporte> Reportes { get; set; }

        [NotMapped]
        public static Usuario UsuarioActual { get; set; } = new Usuario("AA0000", "Invitado", "0000", Access.Invitado);

        public override string ToString()
        {
            return Nombre;
        }


    }
}
