using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pizzeria_Statica.Models
{
    [Table("pizzas")]
    public class Pizza
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Required]
        [Column("nome")]
        public string Nome { get; set; }

        [Column("descrizione")]
        [DefaultValue("")]
        public string? Descrizione {  get; set; }

        [Column("foto")]
        [DefaultValue("default.png")]
        public string? Foto {  get; set; }

        [Column("prezzo")]
        public float? Prezzo { get; set; }
        


    }
}
