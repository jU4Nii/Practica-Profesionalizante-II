using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiblioClases
{
    public class Doctor
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Matricula { get; set; }
        public int EspecialidadMedicaId { get; set; }

        public int Telefono { get; set; }

        public string Email { get; set; }

        public HorarioAtencion HorarioAtencion { get; set; }

        public string Consultorio { get; set; }
    }
}
