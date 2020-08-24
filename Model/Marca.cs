using System;
using System.ComponentModel.DataAnnotations;

namespace PatrimoniosV2.Model
{
    public class Marca
    {
        [Key]
        public int MarcaId { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [StringLength(100, ErrorMessage = "O campo {0} precisa ter entre {2} e {1} caracteres", MinimumLength = 2)]
        public String Nome { get; set; }
    }
}