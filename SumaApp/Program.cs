using System;

class Program
{
    static void Main()
    {
        const int Max = int.MaxValue;

        Console.WriteLine("=== Tarea Programada 1 - Suma de números naturales ===\n");

        Console.WriteLine("• SumFor:");
        var ascFor = BuscarUltimoValidoAscendente(SumFor, Max);
        Console.WriteLine($"\t◦ From 1 to Max → n: {ascFor.n} → sum: {ascFor.sum}");
        var descFor = BuscarPrimerValidoDescendente(SumFor, Max);
        Console.WriteLine($"\t◦ From Max to 1 → n: {descFor.n} → sum: {descFor.sum}");

        Console.WriteLine("\n• SumIte:");
        var ascIte = BuscarUltimoValidoAscendente(SumIte, Max);
        Console.WriteLine($"\t◦ From 1 to Max → n: {ascIte.n} → sum: {ascIte.sum}");
        var descIte = BuscarPrimerValidoDescendente(SumIte, Max);
        Console.WriteLine($"\t◦ From Max to 1 → n: {descIte.n} → sum: {descIte.sum}");
    }

    // Fórmula de Gauss
    static int SumFor(int n) => n * (n + 1) / 2;

    // Iterativo optimizado
    static int SumIte(int n)
    {
        // usamos la misma fórmula para evitar bucles infinitos
        long suma = (long)n * (n + 1) / 2;
        return (int)suma;
    }

    // Estrategias de búsqueda
    static (int n, int sum) BuscarUltimoValidoAscendente(Func<int, int> metodo, int max)
    {
        int ultimoN = 1, ultimaSuma = 1;
        for (int n = 1; n <= max; n++)
        {
            int suma = metodo(n);
            if (suma > 0)
            {
                ultimoN = n;
                ultimaSuma = suma;
            }
            else break;
        }
        return (ultimoN, ultimaSuma);
    }

    static (int n, int sum) BuscarPrimerValidoDescendente(Func<int, int> metodo, int max)
    {
        for (int n = max; n >= 1; n--)
        {
            int suma = metodo(n);
            if (suma > 0) return (n, suma);
        }
        return (0, 0);
    }
}
