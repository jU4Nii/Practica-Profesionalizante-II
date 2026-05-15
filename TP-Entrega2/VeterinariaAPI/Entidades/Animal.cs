namespace VeterinariaAPI.Entidades
{
    public class Animal
    {

        public int Id { get; set; }
        public string Nombre { get; set; }
        public int Edad { get; set; }
        public string Sexo { get; set; }

        public int RazaId { get; set; }
        public int? DuenoId { get; set; }

    }
}
