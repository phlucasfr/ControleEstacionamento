using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ControleEstacionamento.Models
{
    [Table("Registro")]
    public class Registro
    {
        [Key]
        [Column("codigo_reg")]
        public int CodigoReg { get; set; }

        [ForeignKey("Veiculo")]
        [Column("codvei_reg")]
        public int CodveiReg { get; set; }

        [ForeignKey("TabelaPrecos")]
        [Column("codtpr_reg")]
        public int CodtprReg { get; set; }

        [Column("datent_reg")]
        public DateTime DatentReg { get; set; }

        [Column("datsai_reg")]
        public DateTime? DatsaiReg { get; set; }

        public virtual Veiculo Veiculo { get; set; }
        public virtual TabelaPrecos TabelaPrecos { get; set; }
    }
}
