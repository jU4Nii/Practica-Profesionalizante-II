using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiblioClases
{
    public class TurnoMedico
    {

        public int Id { get; set; }

        public int DoctorId { get; set; }

        public int PacienteId { get; set; }

        public DateTime FechaHora { get; set; }

        public string Estado { get; set; }

        public string Duracion { get; set; }

        public string Motivo { get; set; }

        public Recepcionista Recepcionista { get; set; }

    }
}
