# Práctica PP4 - Programación Avanzada
**Estudiante:** Mariano Mora Arrieta
**Carné:** FI23032359  

---

## 🧩 Descripción del Proyecto
Este proyecto consiste en una aplicación de consola en **C# (.NET 8)** utilizando **Entity Framework Core 9.0** y una base de datos **SQLite 3**, que carga un archivo CSV con información de libros y genera archivos TSV organizados por la inicial del autor.

El flujo principal es:
1. Verifica si la base de datos está vacía.
2. Si lo está, carga la información desde `books.csv`.
3. Si ya contiene datos, genera los archivos `.tsv` (uno por cada inicial del autor).

---

## ⚙️ Comandos de CLI utilizados

```bash
# Crear solución y proyecto
dotnet new sln -n PP4
dotnet new console -n BooksApp
dotnet sln add BooksApp/BooksApp.csproj

# Agregar Entity Framework Core
dotnet add package Microsoft.EntityFrameworkCore
dotnet add package Microsoft.EntityFrameworkCore.Sqlite
dotnet add package Microsoft.EntityFrameworkCore.Design
dotnet tool install --global dotnet-ef

# Crear migración y base de datos
dotnet ef migrations add InitialCreate
dotnet ef database update

# Ejecutar el programa
dotnet run


🌐 Fuentes de apoyo y Snippets usados

Documentación oficial de Microsoft: EF Core con SQLite

Tutorial CSV Reader en C#

StackOverflow - GroupBy con Entity Framework

ChatGPT (consulta y ajuste de código y lógica para generar archivos TSV)

Vínculo de la conversación
https://chatgpt.com/share/6910f7b0-415c-8011-a2c8-6ff6de593b34


Prompts utilizados con ChatGPT
🟣 Prompt 1

“¿Por qué mi programa no genera el archivo B.tsv aunque tengo autores con ‘B’?”

Respuesta resumida:
ChatGPT identificó que los nombres de los autores estaban entre comillas ("Borges, Jorge Luis") y por eso el programa agrupaba por " en lugar de la letra B.
Se resolvió usando Trim('"') para limpiar las comillas antes de agrupar los autores por inicial.

Primera ejecución:
dotnet run
La base de datos está vacía, por lo que será llenada a partir de los datos del archivo CSV.
Procesando...
Listo.

Segunda ejecución:
dotnet run
La base de datos se está leyendo para crear los archivos TSV.
Procesando...
Listo.

Preguntas teóricas
1.Uso de Code First con bases NoSQL

El enfoque Code First se basa en modelos relacionales y migraciones que definen tablas, columnas y relaciones.
En bases NoSQL como MongoDB, no existe un esquema rígido ni claves foráneas, por lo que este enfoque no sería viable sin un ORM especializado.

Con Database First, podría mapearse parcialmente la estructura existente, pero las relaciones se perderían y sería necesario mantenerlas manualmente desde el código.

Conclusión:

Sí habría complicaciones con las Foreign Keys y el enfoque Code First no es adecuado para bases NoSQL, ya que estas son no relacionales.

Otros caracteres para separar valores tabulares

Además de la coma (,) y el tabulador (\t), se pueden usar:

Pipe (|) → recomendada extensión: .pipe

Punto y coma (;)

Dos puntos (:)

El Pipe (|) es el más adecuado porque:

No suele aparecer dentro de los textos o nombres.

Permite una lectura clara y estructurada.

Es compatible con herramientas de análisis como Python, Excel y Power BI.

Por ejemplo:

Author|Title|Tags
Acosta, Soledad|La mujer en la sociedad moderna|Ensayo|Autoayuda