using Microsoft.AspNetCore.Mvc.Rendering;

namespace Pizzeria_Statica.Models
{
    public class PizzaFormModel
    {
        public Pizza Pizza { get; set; }
        public List<Categoria>? Categorie { get; set; }

        //proprietà per select multipla
        public List<SelectListItem>? Ingredienti {  get; set; }
        public List<string>? IngredientiSelezionatiId { get; set; }

    }
}
