using Microsoft.AspNetCore.Mvc;
using Transportation.Interfaces;
using Transportation.Models;

namespace Transportation.Controllers; // para IAirplanes, Airbus, Boeing

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    // ChatGPT (OpenAI)
    // Recibimos TODAS las implementaciones de IAirplanes
    public IActionResult Index([FromServices] IEnumerable<IAirplanes> airplanesServices)
    {
        using var db = new CarsContext();

        // ---------- Minnie Mouse + Dealer ----------
        var customer = db.Customers.First(c => c.LastName == "Mouse");
        var ownership = db.CustomerOwnerships.First(o => o.CustomerId == customer.CustomerId);
        var vin = db.CarVins.First(v => v.Vin == ownership.Vin);
        var model = db.Models.First(m => m.ModelId == vin.ModelId);
        var brand = db.Brands.First(b => b.BrandId == model.BrandId);

        ViewData["BrandModel"] = $"{brand.BrandName} - {model.ModelName}";

        var dealer = db.Dealers.First(d => d.DealerId == ownership.DealerId);
        ViewData["Dealer"] = $"{dealer.DealerName} - {dealer.DealerAddress}";

        // ---------- Aviones: Airbus y Boeing ----------
        var airbus = airplanesServices.FirstOrDefault(a => a.GetBrand == "Airbus");
        var boeing = airplanesServices.FirstOrDefault(a => a.GetBrand == "Boeing");

        if (airbus != null)
        {
            ViewData["Airbus"] = $"{airbus.GetBrand}: {string.Join(" - ", airbus.GetModels)}";
        }

        if (boeing != null)
        {
            ViewData["Boeing"] = $"{boeing.GetBrand}: {string.Join(" - ", boeing.GetModels)}";
        }

        return View();
    }
}
