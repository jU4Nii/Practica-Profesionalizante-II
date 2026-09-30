using System.Text;

namespace BarberManager.Web.Infrastructure;

public static class CsvExportador
{
    public static byte[] Crear(IEnumerable<string> encabezados, IEnumerable<IEnumerable<string?>> filas)
    {
        var contenido = new StringBuilder();
        contenido.AppendLine(string.Join(";", encabezados.Select(Escapar)));

        foreach (var fila in filas)
            contenido.AppendLine(string.Join(";", fila.Select(valor => Escapar(valor ?? string.Empty))));

        return Encoding.UTF8.GetBytes("\uFEFF" + contenido);
    }

    private static string Escapar(string valor)
        => $"\"{valor.Replace("\"", "\"\"")}\"";
}
