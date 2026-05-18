namespace VeterinariaAPI.Entidades
{
    public class Atencion
    {
        public int Id { get; set; }
        public int AnimalId { get; set; }
        public string Motivo { get; set; }
        public int TratamientoId { get; set; }
        public List<int> MedicamentosIds { get; set; }
        public DateTime Fecha { get; set; }
    }
}
