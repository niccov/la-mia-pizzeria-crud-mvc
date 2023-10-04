using Microsoft.AspNetCore.Mvc;
using Pizzeria_Statica.Database;
using Pizzeria_Statica.Models;
using System.Diagnostics;

namespace Pizzeria_Statica.Controllers
{
    public class PizzaController : Controller
    {
        private PizzeriaContext _myDatabase;

        public PizzaController(PizzeriaContext db)
        {
            _myDatabase = db;
        }

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
                List<Categoria> categorie = data.Categorie.ToList();
                data.Categorie = categorie;

                return View("Create", data);
            }
            //using (PizzeriaContext context = new PizzeriaContext())
            //{
            ////    Pizza PizzaToCreate = new Pizza();
            ////    PizzaToCreate.Nome = newPizza.Nome;
            ////    PizzaToCreate.Descrizione = newPizza.Descrizione;
            ////    PizzaToCreate.Prezzo = newPizza.Prezzo;
            ////    PizzaToCreate.Foto = newPizza.Foto;
            ////    context.Pizze.Add(PizzaToCreate);
            ////    context.SaveChanges();
            _myDatabase.Pizze.Add(data.Pizza);
            _myDatabase.SaveChanges();
           
                return RedirectToAction("Index");
            //}
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
