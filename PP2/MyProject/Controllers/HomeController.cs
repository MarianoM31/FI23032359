using Microsoft.AspNetCore.Mvc;
using MyProject.Models;
using System;
using System.Text;

namespace MyProject.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View(new BinaryModel());
        }

        [HttpPost]
        public IActionResult Index(BinaryModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // Normalizar a 8 bits
            string aBin = model.A.PadLeft(8, '0');
            string bBin = model.B.PadLeft(8, '0');

            // Convertir a decimal
            int aDec = Convert.ToInt32(aBin, 2);
            int bDec = Convert.ToInt32(bBin, 2);

            // Operaciones binarias sobre strings
            string andBin = OperacionBinaria(aBin, bBin, (x, y) => x & y);
            string orBin  = OperacionBinaria(aBin, bBin, (x, y) => x | y);
            string xorBin = OperacionBinaria(aBin, bBin, (x, y) => x ^ y);

            // Operaciones aritméticas
            int suma = aDec + bDec;
            int mult = aDec * bDec;

            // Pasar resultados a ViewBag
            ViewBag.Resultados = new []
            {
                new { Nombre="a",      Bin=aBin, Dec=aDec, Oct=Convert.ToString(aDec,8), Hex=Convert.ToString(aDec,16).ToUpper() },
                new { Nombre="b",      Bin=bBin, Dec=bDec, Oct=Convert.ToString(bDec,8), Hex=Convert.ToString(bDec,16).ToUpper() },
                new { Nombre="a AND b", Bin=andBin, Dec=Convert.ToInt32(andBin,2), Oct=Convert.ToString(Convert.ToInt32(andBin,2),8), Hex=Convert.ToString(Convert.ToInt32(andBin,2),16).ToUpper() },
                new { Nombre="a OR b", Bin=orBin, Dec=Convert.ToInt32(orBin,2), Oct=Convert.ToString(Convert.ToInt32(orBin,2),8), Hex=Convert.ToString(Convert.ToInt32(orBin,2),16).ToUpper() },
                new { Nombre="a XOR b", Bin=xorBin, Dec=Convert.ToInt32(xorBin,2), Oct=Convert.ToString(Convert.ToInt32(xorBin,2),8), Hex=Convert.ToString(Convert.ToInt32(xorBin,2),16).ToUpper() },
                new { Nombre="a + b", Bin=Convert.ToString(suma,2), Dec=suma, Oct=Convert.ToString(suma,8), Hex=Convert.ToString(suma,16).ToUpper() },
                new { Nombre="a • b", Bin=Convert.ToString(mult,2), Dec=mult, Oct=Convert.ToString(mult,8), Hex=Convert.ToString(mult,16).ToUpper() }
            };

            return View(model);
        }

        private string OperacionBinaria(string a, string b, Func<int,int,int> op)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < a.Length; i++)
            {
                int bitA = a[i] - '0';
                int bitB = b[i] - '0';
                sb.Append(op(bitA, bitB));
            }
            return sb.ToString();
        }
    }
}
