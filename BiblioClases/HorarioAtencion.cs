using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiblioClases
{
    public class HorarioAtencion
    {
        public int Id { get; set; }
        public int EspecialidadMedicaId { get; set; }
        public DateTime Dia { get; set; }
        public TimeSpan HoraInicio { get; set; }
        public TimeSpan HoraFin { get; set; }
    }
}
