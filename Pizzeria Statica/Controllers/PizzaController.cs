using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
                    pizze = db.Pizze.Include(pizza => pizza.Categoria).ToList<Pizza>();
                    return View("Index", pizze);

                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return View("Error", new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
            }
        }

        [HttpGet]
        public IActionResult Details(int id)
        {
            using (PizzeriaContext db = new PizzeriaContext())
            {
                Pizza? pizzaTrovata = db.Pizze.Where(pizza => pizza.Id == id).Include(pizza => pizza.Categoria).FirstOrDefault();

                if (pizzaTrovata == null)
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
            using (PizzeriaContext context = new PizzeriaContext())
            {
                List<Categoria> categorie = context.Categorie.ToList();

                PizzaFormModel model = new PizzaFormModel();
                model.Pizza = new Pizza();
                model.Categorie = categorie;

                return View("Create", model);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(PizzaFormModel data)
        {
            if (!ModelState.IsValid)
            {
                using (PizzeriaContext context = new PizzeriaContext())
                {
                    List<Categoria> categorie = context.Categorie.ToList();
                    data.Categorie = categorie;

                    return View("Create", data);
                }
                
            }
            using (PizzeriaContext context = new PizzeriaContext())
            {
                Pizza PizzaToCreate = new Pizza();
                PizzaToCreate.Nome = data.Pizza.Nome;
                PizzaToCreate.Descrizione = data.Pizza.Descrizione;
                PizzaToCreate.Prezzo = data.Pizza.Prezzo;
                PizzaToCreate.Foto = data.Pizza.Foto;
                PizzaToCreate.categoriaId = data.Pizza.categoriaId;
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
                    List<Categoria> categorie = db.Categorie.ToList();

                    PizzaFormModel model = new PizzaFormModel();
                    model.Pizza = pizzaDaModificare;
                    model.Categorie = categorie;    
                    return View(model);
                }
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Update(int id,  PizzaFormModel data)
        {
            if(!ModelState.IsValid)
            {
                using (PizzeriaContext context = new PizzeriaContext())
                {
                    List<Categoria> categorie = context.Categorie.ToList();
                    data.Categorie = categorie;
                    
                    return View("Update", data);
                }
            }

            using (PizzeriaContext db = new PizzeriaContext())
            {
                Pizza? pizzaDaModificare = db.Pizze.Where(pizza => pizza.Id == id).FirstOrDefault();
                if(pizzaDaModificare != null)
                {
                    pizzaDaModificare.Nome = data.Pizza.Nome;
                    pizzaDaModificare.Descrizione = data.Pizza.Descrizione;
                    pizzaDaModificare.Prezzo = data.Pizza.Prezzo;
                    pizzaDaModificare.Foto = data.Pizza.Foto;
                    pizzaDaModificare.categoriaId = data.Pizza.categoriaId;

                    db.SaveChanges();

                    return RedirectToAction("Index");
                }
                else
                {
                    return NotFound();
                }
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            using (PizzeriaContext db = new PizzeriaContext())
            {
                Pizza pizzaDaEliminare = db.Pizze.Where(pizza => pizza.Id == id).FirstOrDefault();
                if (pizzaDaEliminare != null)
                {
                    db.Pizze.Remove(pizzaDaEliminare);

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
