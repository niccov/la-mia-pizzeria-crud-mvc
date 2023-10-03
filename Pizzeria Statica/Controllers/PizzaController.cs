using Microsoft.AspNetCore.Mvc;
using Pizzeria_Statica.Database;
using Pizzeria_Statica.Models;
using System.Diagnostics;

namespace Pizzeria_Statica.Controllers
{
    public class PizzaController : Controller
    {
        public IActionResult Index()
        {
            List<Pizza> pizze = new List<Pizza>();
            try
            {
                using (PizzeriaContext db = new PizzeriaContext())
                {
                    pizze = db.Pizze.ToList<Pizza>();
                    return View("Index", pizze);

                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return View("Error", new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
            }
        }

        public IActionResult Details(int id)
        {
            using (PizzeriaContext db = new PizzeriaContext())
            {
                Pizza? pizzaTrovata = db.Pizze.Where(pizza => pizza.Id == id).FirstOrDefault();

                if(pizzaTrovata == null)
                {
                    return NotFound($"La pizza non è stata trovata");
                }
                else
                {
                    return View("Details", pizzaTrovata);
                }
            }
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View("Create");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Pizza newPizza)
        {
            if (!ModelState.IsValid)
            {
                return View("Create", newPizza);
            }
            using (PizzeriaContext context = new PizzeriaContext())
            {
                Pizza PizzaToCreate = new Pizza();
                PizzaToCreate.Nome = newPizza.Nome;
                PizzaToCreate.Descrizione = newPizza.Descrizione;
                PizzaToCreate.Prezzo = newPizza.Prezzo;
                PizzaToCreate.Foto = newPizza.Foto;
                context.Pizze.Add(PizzaToCreate);
                context.SaveChanges();
                return RedirectToAction("Index");
            }
        }

        [HttpGet]
        public IActionResult Update(int id)
        {

            using (PizzeriaContext db =  new PizzeriaContext())
            {
                Pizza? pizzaDaModificare = db.Pizze.Where(pizza => pizza.Id == id).FirstOrDefault();
                if (pizzaDaModificare == null)
                {
                    return NotFound();
                }
                else
                {
                    return View(pizzaDaModificare);
                }
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Update(int id,  Pizza pizza)
        {
            if(!ModelState.IsValid)
            {
                return View("Update", pizza);
            }

            using (PizzeriaContext db = new PizzeriaContext())
            {
                Pizza? pizzaDaModificare = db.Pizze.Where(pizza => pizza.Id == id).FirstOrDefault();
                if(pizzaDaModificare != null)
                {
                    pizzaDaModificare.Nome = pizza.Nome;
                    pizzaDaModificare.Descrizione = pizza.Descrizione;
                    pizzaDaModificare.Prezzo = pizza.Prezzo;
                    pizzaDaModificare.Foto = pizza.Foto;

                    db.SaveChanges();

                    return RedirectToAction("Index");
                }
                else
                {
                    return NotFound();
                }
            }
        }


    }
}
