namespace Pizzeria_Statica.Models
{
    public class Categoria
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public List<Pizza> Pizze { get; set; }

        public Categoria()
        {

        }
    }
}
