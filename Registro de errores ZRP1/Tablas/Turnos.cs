﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConsultaCore;
using System.Reflection;

namespace Registro_de_errores_ZRP1.Tablas
{
    class Turnos:IdateableObject
    {
        public string Turno { get; set; }

        public override string ToString()
        {
            return Turno;
        }
    }
}
