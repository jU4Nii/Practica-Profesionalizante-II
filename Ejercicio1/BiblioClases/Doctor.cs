using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiblioClases
{
    public class Doctor
    {

        public Doctor(int id, string nombre, string apellido, string matricula, int especialidadMedicaId, int telefono, string email, HorarioAtencion horarioAtencion, string consultorio)
        {
            Id = id;
            Nombre = nombre;
            Apellido = apellido;
            Matricula = matricula;
            EspecialidadMedicaId = especialidadMedicaId;
            Telefono = telefono;
            Email = email;
            HorarioAtencion = horarioAtencion;
            Consultorio = consultorio;
        }

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
