using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Checador_FXE.Plantillas
{
    public class Empleado
    {
        public static readonly string TABLE_NAME = "empleados_sind";
        public static readonly string DATABASE_NAME = "checador_fxe_db";

        public string NoEmp { get; set; }
        public string Nombres { get; set; }
        public string Apellidos { get; set; }
        public string Puesto { get; set; }
        public string Region { get; set; }
        public string Division { get; set; }
        public string Localidad { get; set; }


        public Empleado() { }
    }
}
