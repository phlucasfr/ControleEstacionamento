using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ControleEstacionamento.Models
{
    [Table("Veiculo")]
    public class Veiculo
    {
        [Key]
        [Column("codigo_vei")]
        public int CodigoVei { get; set; }

        [Column("placa_vei")]
        public string PlacaVei { get; set; }
    }
}
