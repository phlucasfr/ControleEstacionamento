using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ControleEstacionamento.Data.Repositories;
using ControleEstacionamento.Data;

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

        [Column("valhorini_tpr")]
        public decimal ValhoriniTpr { get; set; }

        [Column("valhoradi_tpr")]
        public decimal ValhoradiTpr { get; set; }
    }

    public class TabelaPrecosModel
    {
        private static readonly CrudRepository<TabelaPrecos> _tabelaPrecosRepository;

        static TabelaPrecosModel()
        {
            var dbContext = new ApplicationDbContext();
            _tabelaPrecosRepository = new CrudRepository<TabelaPrecos>(dbContext);
        }

        public static int? ObterPrecoAtual(DateTime dataEntrada)
        {
            var tabelaPrecos = _tabelaPrecosRepository.GetAll()
                .Where(tp => tp.DatiniTpr <= dataEntrada && tp.DatfimTpr >= dataEntrada)
                .FirstOrDefault();

            if (tabelaPrecos != null)
            {
                return tabelaPrecos.CodigoTpr;
            }

            return null;
        }
    }

}
