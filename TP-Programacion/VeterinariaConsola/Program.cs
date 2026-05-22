using System.Net.Http.Json;
using System.Threading.Tasks;
namespace VeterinariaConsola

{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            bool salir = false;

            HttpClient client = new HttpClient();

            client.BaseAddress = new Uri("https://localhost:7073/");

            while (!salir)
            {
                Console.WriteLine("1 - Crear dueño");
                Console.WriteLine("2 - Consultar animales");
                Console.WriteLine("3 - Crear animal");
                Console.WriteLine("4 - Modificar animal");
                Console.WriteLine("5 - Eliminar animal");
                Console.WriteLine("6 - Crear atención");
                Console.WriteLine("7 - Consultar medicamentos");
                Console.WriteLine("0 - Salir");

                string opcion = Console.ReadLine();

                switch (opcion)
                {
                    case "1":
                        Console.Write("DNI: ");
                        string dni = Console.ReadLine();

                        Console.Write("Nombre: ");
                        string nombre = Console.ReadLine();

                        Console.Write("Apellido: ");
                        string apellido = Console.ReadLine();

                        DuenoDTO dto = new DuenoDTO
                        {
                            Dni = dni,
                            Nombre = nombre,
                            Apellido = apellido
                        };

                        var response = await client.PostAsJsonAsync("duenos", dto);

                        if (response.IsSuccessStatusCode)
                        {
                            Console.WriteLine("Dueño creado correctamente");
                        }
                        else
                        {
                            Console.WriteLine("Error al crear dueño");
                        }
                        break;

                    case "2":
                        var animales = await client.GetFromJsonAsync<List<Animal>>("animales");

                        foreach (var animal in animales)
                        {
                            Console.WriteLine($"{animal.Id} - {animal.Nombre}");
                        }
                        break;

                    case "3":
                        Console.Write("Nombre: ");
                        string nombreAnimal = Console.ReadLine();

                        Console.Write("Edad: ");
                        int edad = int.Parse(Console.ReadLine());

                        Console.Write("Dueño Id: ");
                        int duenoId = int.Parse(Console.ReadLine());

                        Console.Write("Raza Id: ");
                        int razaId = int.Parse(Console.ReadLine());

                        AnimalDTO dtoAnimal = new AnimalDTO
                        {
                            Nombre = nombreAnimal,
                            Edad = edad,
                            DuenoId = duenoId,
                            RazaId = razaId
                        };

                        var responseAnimal = await client.PostAsJsonAsync("animales", dtoAnimal);

                        if (responseAnimal.IsSuccessStatusCode)
                        {
                            Console.WriteLine("Animal creado correctamente");
                        }
                        else
                        {
                            Console.WriteLine("Error al crear animal");
                        }
                        break;

                    case "4":
                        Console.Write("Id del animal: ");
                        int idModificar = int.Parse(Console.ReadLine());

                        Console.Write("Nuevo nombre: ");
                        string nuevoNombre = Console.ReadLine();

                        Console.Write("Nueva edad: ");
                        int nuevaEdad = int.Parse(Console.ReadLine());

                        Console.Write("Nuevo Dueño Id: ");
                        int nuevoDuenoId = int.Parse(Console.ReadLine());

                        Console.Write("Nueva Raza Id: ");
                        int nuevaRazaId = int.Parse(Console.ReadLine());

                        AnimalDTO animalEditado = new AnimalDTO
                        {
                            Nombre = nuevoNombre,
                            Edad = nuevaEdad,
                            DuenoId = nuevoDuenoId,
                            RazaId = nuevaRazaId
                        };

                        var responsePut = await client.PutAsJsonAsync($"animales/{idModificar}", animalEditado);

                        if (responsePut.IsSuccessStatusCode)
                        {
                            Console.WriteLine("Animal modificado correctamente");
                        }
                        else
                        {
                            Console.WriteLine("Error al modificar animal");
                        }
                        break;

                    case "5":
                        Console.Write("Id del animal a eliminar: ");
                        int idEliminar = int.Parse(Console.ReadLine());

                        var responseDelete = await client.DeleteAsync($"animales/{idEliminar}");

                        if (responseDelete.IsSuccessStatusCode)
                        {
                            Console.WriteLine("Animal eliminado correctamente");
                        }
                        else
                        {
                            Console.WriteLine("Error al eliminar animal");
                        }
                        break;

                    case "6":
                        Console.Write("Animal Id: ");
                        int animalId = int.Parse(Console.ReadLine());

                        Console.Write("Motivo: ");
                        string motivo = Console.ReadLine();

                        Console.Write("Tratamiento Id: ");
                        int tratamientoId = int.Parse(Console.ReadLine());

                        Console.Write("Medicamentos Ids (ej: 1,2): ");
                        string medicamentosIds = Console.ReadLine();

                        AtencionDTO dtoAtencion = new AtencionDTO
                        {
                            AnimalId = animalId,
                            Motivo = motivo,
                            TratamientoId = tratamientoId,
                            MedicamentosIds = medicamentosIds,
                            Fecha = DateTime.Now
                        };

                        var responseAtencion = await client.PostAsJsonAsync("atenciones", dtoAtencion);

                        if (responseAtencion.IsSuccessStatusCode)
                        {
                            Console.WriteLine("Atención creada correctamente");
                        }
                        else
                        {
                            Console.WriteLine("Error al crear atención");
                        }
                        break;

                    case "7":
                        var medicamentos = await client.GetFromJsonAsync<List<Medicamento>>("medicamentos");

                        foreach (var medicamento in medicamentos)
                        {
                            Console.WriteLine($"{medicamento.Id} - {medicamento.Nombre}");
                        }
                        break;

                    case "0":
                        salir = true;
                        break;

                    default:
                        Console.WriteLine("Opción inválida");
                        break;
                }

                Console.WriteLine();
            }
        }

        public class Animal
        {
            public int Id { get; set; }
            public string Nombre { get; set; }
        }

        public class DuenoDTO
        {
            public string Dni { get; set; }
            public string Nombre { get; set; }
            public string Apellido { get; set; }
        }

        public class Medicamento
        {
            public int Id { get; set; }
            public string Nombre { get; set; }
        }

        public class AnimalDTO
        {
            public string Nombre { get; set; }

            public int Edad { get; set; }

            public int DuenoId { get; set; }

            public int RazaId { get; set; }
        }

        public class AtencionDTO
        {
            public int AnimalId { get; set; }

            public string Motivo { get; set; }

            public int TratamientoId { get; set; }

            public string MedicamentosIds { get; set; }

            public DateTime Fecha { get; set; }
        }

    }
}
