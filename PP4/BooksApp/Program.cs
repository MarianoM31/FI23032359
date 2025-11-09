using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using BooksApp.Data;

class Program
{
    static void Main()
    {
        using var db = new AppDbContext();

        if (!db.Authors.Any())
        {
            Console.WriteLine("La base de datos está vacía, por lo que será llenada a partir de los datos del archivo CSV.");
            Console.WriteLine("Procesando...");

            LoadDataFromCSV(db);

            Console.WriteLine("Listo.");
        }
        else
        {
            Console.WriteLine("La base de datos se está leyendo para crear los archivos TSV.");
            Console.WriteLine("Procesando...");

            ExportDataToTSV(db);

            Console.WriteLine("Listo.");
        }
    }

    // =======================================================
    // LECTURA DEL CSV Y LLENADO DE BASE DE DATOS
    // =======================================================
    static void LoadDataFromCSV(AppDbContext db)
    {
        var csvPath = Path.Combine(Directory.GetCurrentDirectory(), "Data", "data", "books.csv");
        if (!File.Exists(csvPath))
        {
            Console.WriteLine("❌ No se encontró el archivo books.csv en la carpeta Data/data.");
            return;
        }

        var lines = File.ReadAllLines(csvPath).Skip(1); // Omitir encabezado

        foreach (var line in lines)
        {
            var parts = SplitCsvLine(line);
            if (parts.Length < 3) continue;

            var authorName = parts[0].Trim('"');
            var titleName = parts[1].Trim();
            var tagNames = parts[2].Split('|', StringSplitOptions.RemoveEmptyEntries).Select(t => t.Trim()).ToList();

            // Buscar o crear autor
            var author = db.Authors.FirstOrDefault(a => a.AuthorName == authorName);
            if (author == null)
            {
                author = new Author { AuthorName = authorName };
                db.Authors.Add(author);
                db.SaveChanges();
            }

            // Crear título
            var title = new Title { TitleName = titleName, AuthorId = author.AuthorId };
            db.Titles.Add(title);
            db.SaveChanges();

            // Procesar etiquetas
            foreach (var tagName in tagNames)
            {
                var tag = db.Tags.FirstOrDefault(t => t.TagName == tagName);
                if (tag == null)
                {
                    tag = new Tag { TagName = tagName };
                    db.Tags.Add(tag);
                    db.SaveChanges();
                }

                db.TitlesTags.Add(new TitleTag
                {
                    TitleId = title.TitleId,
                    TagId = tag.TagId
                });
                db.SaveChanges();
            }
        }
    }

    // =======================================================
    // LECTURA DE BASE DE DATOS Y GENERACIÓN DE TSVs
    // =======================================================
    static void ExportDataToTSV(AppDbContext db)
    {
        var outputDir = Path.Combine(Directory.GetCurrentDirectory(), "Data", "data");

        var titles = db.Titles
            .Include(t => t.Author)
            .Include(t => t.TitleTags)
            .ThenInclude(tt => tt.Tag)
            .ToList();

        var groupedByInitial = titles.GroupBy(t =>
            {
                var name = t.Author.AuthorName.Trim('"'); // quita comillas
                return char.ToUpper(name[0]);             // obtiene la primera letra limpia
            });


        foreach (var group in groupedByInitial)
        {
            var filePath = Path.Combine(outputDir, $"{group.Key}.tsv");

            using (var writer = new StreamWriter(filePath))
            {
                writer.WriteLine("AuthorName\tTitleName\tTagName");

                // Orden descendente por autor, título y etiqueta
                var ordered = group
                    .OrderByDescending(t => t.Author.AuthorName)
                    .ThenByDescending(t => t.TitleName);

                foreach (var title in ordered)
                {
                    foreach (var tag in title.TitleTags.Select(tt => tt.Tag).OrderByDescending(t => t.TagName))
                    {
                        writer.WriteLine($"{title.Author.AuthorName}\t{title.TitleName}\t{tag.TagName}");
                    }
                }
            }
        }
    }

    // =======================================================
    // FUNCIÓN AUXILIAR PARA DIVIDIR LÍNEAS DE CSV
    // =======================================================
    static string[] SplitCsvLine(string line)
    {
        var parts = new List<string>();
        bool inQuotes = false;
        var value = "";

        foreach (var c in line)
        {
            if (c == '"' && !inQuotes)
            {
                inQuotes = true;
            }
            else if (c == '"' && inQuotes)
            {
                inQuotes = false;
            }
            else if (c == ',' && !inQuotes)
            {
                parts.Add(value);
                value = "";
            }
            else
            {
                value += c;
            }
        }
        parts.Add(value);
        return parts.ToArray();
    }
}
