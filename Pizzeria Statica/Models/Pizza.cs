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

        [Column("nome")]
        [Required(ErrorMessage = "Il campo è obbligatorio")]
        [StringLength(50, ErrorMessage = "Il nome non può avere più di 50 caratteri")]
        public string Nome { get; set; }

        [Column("descrizione")]
        [DefaultValue("")]
        [Required(ErrorMessage = "Il campo è obbligatorio")]
        [StringLength(150, ErrorMessage = "Il nome non può avere più di 150 caratteri")]
        public string? Descrizione {  get; set; }

        [Column("foto")]
        [DefaultValue("default.png")]
        [Required(ErrorMessage = "Il campo è obbligatorio")]
        public string? Foto {  get; set; }

        [Column("prezzo")]
        [Required(ErrorMessage = "Il campo è obbligatorio")]
        public float? Prezzo { get; set; }

        //relazione Categoria
        public int? categoriaId { get; set; }
        public Categoria? categoria { get; set; }

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
