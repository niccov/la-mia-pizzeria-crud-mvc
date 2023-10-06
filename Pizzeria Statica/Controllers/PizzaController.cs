using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Pizzeria_Statica.Database;
using Pizzeria_Statica.Models;
using System.Diagnostics;

namespace Pizzeria_Statica.Controllers
{
    [Authorize(Roles = "USER,ADMIN")]
    public class PizzaController : Controller
    {
        [Authorize(Roles = "USER,ADMIN")]
        public IActionResult Index()
        {
            List<Pizza> pizze = new List<Pizza>();
            try
            {
                using (PizzeriaContext db = new PizzeriaContext())
                {
                    pizze = db.Pizze.Include(pizza => pizza.Categoria).Include(pizza => pizza.Ingredienti).ToList<Pizza>();
                    return View("Index", pizze);

                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return View("Error", new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
            }
        }

        [Authorize(Roles = "USER,ADMIN")]
        [HttpGet]
        public IActionResult Details(int id)
        {
            using (PizzeriaContext db = new PizzeriaContext())
            {
                Pizza? pizzaTrovata = db.Pizze.Where(pizza => pizza.Id == id).Include(pizza => pizza.Categoria).Include(pizza => pizza.Ingredienti).FirstOrDefault();

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

        [Authorize(Roles = "ADMIN")]
        [HttpGet]
        public IActionResult Create()
        {
            using (PizzeriaContext context = new PizzeriaContext())
            {
                List<Categoria> categorie = context.Categorie.ToList();

                List<SelectListItem> allTagsSelectList = new List<SelectListItem>();
                List<Ingrediente> databaseAllIngredients = context.Ingredienti.ToList();

                foreach(Ingrediente ingrediente in databaseAllIngredients)
                {
                    allTagsSelectList.Add(new SelectListItem { Text = ingrediente.Name, Value = ingrediente.Id.ToString() });
                }

                PizzaFormModel model = new PizzaFormModel();
                model.Pizza = new Pizza();
                model.Categorie = categorie;
                model.Ingredienti = allTagsSelectList;

                return View("Create", model);
            }
        }

        [Authorize(Roles = "ADMIN")]
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

                    List<SelectListItem> allTagsSelectList = new List<SelectListItem>();
                    List<Ingrediente> databaseAllIngredients = context.Ingredienti.ToList();

                    foreach (Ingrediente ingrediente in databaseAllIngredients)
                    {
                        allTagsSelectList.Add(new SelectListItem { Text = ingrediente.Name, Value = ingrediente.Id.ToString() });
                    }

                    data.Ingredienti = allTagsSelectList;

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

                PizzaToCreate.Ingredienti = new List<Ingrediente>();

                if (data.IngredientiSelezionatiId != null)
                {
                    foreach (string selectedIngredientsId in data.IngredientiSelezionatiId)
                    {
                        int selectedIntIngredientsId = int.Parse(selectedIngredientsId);

                        Ingrediente? ingrediente = context.Ingredienti.Where(m => m.Id == selectedIntIngredientsId).FirstOrDefault();

                        if(ingrediente != null)
                        {
                            PizzaToCreate.Ingredienti.Add(ingrediente);
                        }
                    }
                }

                context.Pizze.Add(PizzaToCreate);
                context.SaveChanges();
           
                return RedirectToAction("Index");
            }
        }

        [Authorize(Roles = "ADMIN")]
        [HttpGet]
        public IActionResult Update(int id)
        {

            using (PizzeriaContext db =  new PizzeriaContext())
            {
                Pizza? pizzaDaModificare = db.Pizze.Where(pizza => pizza.Id == id).Include(pizza => pizza.Ingredienti).FirstOrDefault();

                if (pizzaDaModificare == null)
                {
                    return NotFound("La pizza che vuoi modificare non è stata trovata");
                }
                else
                {
                    List<Categoria> categorie = db.Categorie.ToList();

                    List<Ingrediente> dbIngredientList = db.Ingredienti.ToList();
                    List<SelectListItem> selectListItem = new List<SelectListItem>();

                    foreach (Ingrediente ingrediente in dbIngredientList)
                    {
                        selectListItem.Add(new SelectListItem
                        {
                            Value = ingrediente.Id.ToString(),
                            Text = ingrediente.Name,
                            Selected = pizzaDaModificare.Ingredienti.Any(ingredientAssociated => ingredientAssociated.Id == ingrediente.Id)
                        });
                    }
                    PizzaFormModel model = new PizzaFormModel();
                    model.Pizza = pizzaDaModificare;
                    model.Categorie = categorie;
                    model.Ingredienti = selectListItem;
                    return View(model);
                }
            }
        }

        [Authorize(Roles = "ADMIN")]
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

                    List<Ingrediente> dbIngredientList = context.Ingredienti.ToList();
                    List<SelectListItem> selectListItem = new List<SelectListItem>();

                    foreach (Ingrediente ingrediente in dbIngredientList)
                    {
                        selectListItem.Add(new SelectListItem
                        {
                            Value = ingrediente.Id.ToString(),
                            Text = ingrediente.Name
                        });
                    }

                    data.Ingredienti = selectListItem;

                    return View("Update", data);
                }
            }

            using (PizzeriaContext db = new PizzeriaContext())
            {
                Pizza? pizzaDaModificare = db.Pizze.Where(pizza => pizza.Id == id). Include(pizza => pizza.Ingredienti).FirstOrDefault();

                if(pizzaDaModificare != null)
                {
                    pizzaDaModificare.Ingredienti.Clear();

                    if (data.IngredientiSelezionatiId != null)
                    {
                        foreach (string selectedIngredientsId in data.IngredientiSelezionatiId)
                        {
                            int selectedIntIngredientsId = int.Parse(selectedIngredientsId);

                            Ingrediente? ingrediente = db.Ingredienti.Where(m => m.Id == selectedIntIngredientsId).FirstOrDefault();

                            if (ingrediente != null)
                            {
                                pizzaDaModificare.Ingredienti.Add(ingrediente);
                            }
                        }
                    }

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

        [Authorize(Roles = "ADMIN")]
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
