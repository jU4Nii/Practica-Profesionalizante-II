using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiblioClases
{
    public class Paciente
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public DateOnly FechaNacimiento { get; set; }
        public string Dni { get; set; }
        public string Email { get; set; }
        public string Telefono { get; set; }
        public string HistorialMedico { get; set; }

        public string ObraSocial { get; set; }
    }
}
