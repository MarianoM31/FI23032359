using Microsoft.AspNetCore.Mvc;
using MVC.Models;
 
namespace MVC.Controllers;
 
public class HomeController : Controller
{
    [HttpGet]
    public IActionResult Index()
    {
        return View();
    }
 
    [HttpPost]
    public IActionResult Index(TheModel model)
    {
        ViewBag.Valid = ModelState.IsValid;
 
        if (ViewBag.Valid)
        {
            var chars = model.Phrase!
                .Where(c => c != ' ')   //OpenAi
                .ToList();
 
            foreach (var c in chars)
            {
                if (!model.Counts!.ContainsKey(c))
                    model.Counts[c] = 0;
 
                model.Counts[c]++;
            }
 
            model.Lower = new string(chars.Select(char.ToLower).ToArray());
            model.Upper = new string(chars.Select(char.ToUpper).ToArray());
        }
 
        return View(model);
    }
}
 