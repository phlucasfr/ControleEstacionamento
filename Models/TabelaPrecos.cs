using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ControleEstacionamento.Models
{
    [Table("TabelaPrecos")]
    public class TabelaPrecos
    {
        [Key]
        [Column("codigo_tpr")]
        public int CodigoTpr { get; set; }

        [Column("datini_tpr")]
        public DateTime DatiniTpr { get; set; }

        [Column("datfim_tpr")]
        public DateTime DatfimTpr { get; set; }

        [Column("valhor_tpr")]
        public decimal ValhorTpr{ get; set; }
    }
}
