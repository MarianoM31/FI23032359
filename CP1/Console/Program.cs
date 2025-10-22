//https://chatgpt.com/share/68f83cf1-3f3c-8011-97fd-ce782bf40911
public class Numbers


{
    private static readonly double N = 25;

    public static double Formula(double z)
    {
        return Round((z + Math.Sqrt(4 + Math.Pow(z, 2))) / 2);
    }

    public static double Recursive(double z)
    {
        return Round(Recursive(z, N) / Recursive(z, N - 1));
    }

    public static double Iterative(double z)
    {
        return Round(Iterative(z, N) / Iterative(z, N - 1));
    }

    private static double Recursive(double z, double n)
    {
        // Caso base: f(z, 0) = 1
        if (n == 0)
            return 1;

        // Caso base: f(z, 1) = 1
        if (n == 1)
            return 1;

        // Caso general: f(z, n) = z * f(z, n - 1) + f(z, n - 2)
        return z * Recursive(z, n - 1) + Recursive(z, n - 2);
    }

    private static double Iterative(double z, double n)
    {
        double a = 1; // f(z, 0)
        double b = 1; // f(z, 1)
        double temp;

        for (int i = 2; i <= n; i++)
        {
            temp = z * b + a;
            a = b;
            b = temp;
        }

        return b; // Devuelve f(z, n)
    }
    private static double Round(double value)
    {
        return Math.Round(value, 10);
    }

    public static void Main(String[] args)
    {
        String[] metallics = [
            "Platinum", // [0]
            "Golden", // [1]
            "Silver", // [2]
            "Bronze", // [3]
            "Copper", // [4]
            "Nickel", // [5]
            "Aluminum", // [6]
            "Iron", // [7]
            "Tin", // [8]
            "Lead", // [9]
        ];
        for (var z = 0; z < metallics.Length; z++)
        {
            Console.WriteLine("\n[" + z + "] " + metallics[z]);
            Console.WriteLine(" ↳ formula(" + z + ")   ≈ " + Formula(z));
            Console.WriteLine(" ↳ recursive(" + z + ") ≈ " + Recursive(z));
            Console.WriteLine(" ↳ iterative(" + z + ") ≈ " + Iterative(z));
        }
    }
}
