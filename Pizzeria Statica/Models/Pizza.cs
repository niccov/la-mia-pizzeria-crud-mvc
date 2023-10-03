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

        public Pizza() { }
        public Pizza(int id, string nome, string? descrizione, float? prezzo, string? foto)
        {   
            Id = id;
            Nome = nome;
            Descrizione = descrizione;
            Prezzo = prezzo;
            Foto = Foto;
        }

    }
}
