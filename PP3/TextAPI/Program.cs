using System.Text;
using System.Xml.Serialization;

var builder = WebApplication.CreateBuilder(args);

// ✅ Registrar Swagger y API Explorer
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// --- Endpoint raíz (Swagger redirect) ---
app.MapGet("/", () => Results.Redirect("/swagger"));

// --- Endpoint /include ---
app.MapPost("/include/{position:int}", (int position, string value, string text, bool? xml) =>
{
    if (position < 0)
        return Results.BadRequest(new { error = "'position' must be 0 or higher" });
    if (string.IsNullOrWhiteSpace(value))
        return Results.BadRequest(new { error = "'value' cannot be empty" });
    if (string.IsNullOrWhiteSpace(text))
        return Results.BadRequest(new { error = "'text' cannot be empty" });

    var words = text.Split(' ').ToList();

    if (position >= words.Count)
        words.Add(value);
    else
        words.Insert(position, value);

    var result = new ResultModel
    {
        Ori = text,
        New = string.Join(" ", words)
    };

    return xml == true
        ? Results.Text(ToXml(result), "application/xml")
        : Results.Json(result);
});

// --- Endpoint /replace ---
app.MapPut("/replace/{length:int}", (int length, string value, string text, bool? xml) =>
{
    if (length <= 0)
        return Results.BadRequest(new { error = "'length' must be greater than 0" });
    if (string.IsNullOrWhiteSpace(value))
        return Results.BadRequest(new { error = "'value' cannot be empty" });
    if (string.IsNullOrWhiteSpace(text))
        return Results.BadRequest(new { error = "'text' cannot be empty" });

    var words = text.Split(' ');
    for (int i = 0; i < words.Length; i++)
        if (words[i].Length == length)
            words[i] = value;

    var result = new ResultModel
    {
        Ori = text,
        New = string.Join(" ", words)
    };

    return xml == true
        ? Results.Text(ToXml(result), "application/xml")
        : Results.Json(result);
});

// --- Endpoint /erase ---
app.MapDelete("/erase/{length:int}", (int length, string text, bool? xml) =>
{
    if (length <= 0)
        return Results.BadRequest(new { error = "'length' must be greater than 0" });
    if (string.IsNullOrWhiteSpace(text))
        return Results.BadRequest(new { error = "'text' cannot be empty" });

    var newWords = text.Split(' ').Where(w => w.Length != length);
    var result = new ResultModel
    {
        Ori = text,
        New = string.Join(" ", newWords)
    };

    return xml == true
        ? Results.Text(ToXml(result), "application/xml")
        : Results.Json(result);
});

// --- Swagger (interfaz gráfica) ---
app.UseSwagger();
app.UseSwaggerUI();

app.Run();

// --- Función auxiliar para convertir a XML ---
static string ToXml(ResultModel obj)
{
    var xmlSerializer = new XmlSerializer(typeof(ResultModel));
    using var stringWriter = new StringWriter(new StringBuilder());
    xmlSerializer.Serialize(stringWriter, obj);
    return stringWriter.ToString();
}

// --- Clase auxiliar para resultados ---
public class ResultModel
{
    public string Ori { get; set; } = string.Empty;
    public string New { get; set; } = string.Empty;

    public ResultModel() { }
}
