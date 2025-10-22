using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Xml.Serialization;

//https://chatgpt.com/share/68f83cf1-3f3c-8011-97fd-ce782bf40911

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

var list = new List<object>();

// GET — redirige a Swagger
app.MapGet("/", () => Results.Redirect("/swagger"));

// POST — devuelve la lista (JSON o XML según header)
app.MapPost("/", ([FromHeader(Name = "xml")] bool xml = false) =>
{
    if (xml)
    {
        var serializer = new XmlSerializer(typeof(List<object>));
        using var stream = new MemoryStream();
        serializer.Serialize(stream, list);
        var xmlString = Encoding.UTF8.GetString(stream.ToArray());
        return Results.Content(xmlString, "application/xml");
    }

    return Results.Ok(list);
});

// PUT — agrega elementos con validación
app.MapPut("/", ([FromForm] int quantity, [FromForm] string type) =>
{
    // Validaciones según la rúbrica
    if (quantity <= 0)
        return Results.BadRequest(new { error = "'quantity' must be higher than zero" });

    if (type != "int" && type != "float")
        return Results.BadRequest(new { error = "'type' must be 'int' or 'float'" });

    var random = new Random();

    // Agregar los elementos según el tipo
    if (type == "int")
    {
        for (int i = 0; i < quantity; i++)
        {
            list.Add(random.Next()); // entero aleatorio
        }
    }
    else // type == "float"
    {
        for (int i = 0; i < quantity; i++)
        {
            list.Add(random.NextDouble()); // flotante aleatorio
        }
    }

    // Retornar lista actualizada
    return Results.Ok(list);
}).DisableAntiforgery();


// DELETE — elimina elementos con validación
app.MapDelete("/", ([FromForm] int quantity) =>
{
    // Verificar cantidad válida
    if (quantity <= 0)
        return Results.BadRequest(new { error = "'quantity' must be higher than zero" });

    // Verificar que existan suficientes elementos
    if (list.Count < quantity)
        return Results.BadRequest(new { error = "Not enough elements in list to delete" });

    // Eliminar desde el inicio de la lista
    for (int i = 0; i < quantity; i++)
    {
        list.RemoveAt(0);
    }

    // Devolver lista actualizada
    return Results.Ok(list);
}).DisableAntiforgery();


//  PATCH — limpia completamente la lista
app.MapPatch("/", () =>
{
    list.Clear(); // Limpia todos los elementos
    return Results.Ok(list); // Devuelve la lista vacía
});
