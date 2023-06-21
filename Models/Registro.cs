using ControleEstacionamento.Data;
using ControleEstacionamento.Data.Repositories;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ControleEstacionamento.Models
{
    [Table("Registro")]
    public class Registro: RegistroModel
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

        // Propriedades de navegação para o Veiculo e TabelaPrecos
        public virtual Veiculo Veiculo { get; set; }
        public virtual TabelaPrecos TabelaPrecos { get; set; }
    }

    public class RegistroModel
    {
        private static readonly CrudRepository<Registro> _registroRepository;

        static RegistroModel()
        {
            var dbContext = new ApplicationDbContext();
            _registroRepository = new CrudRepository<Registro>(dbContext);
        }

        public static void RegistrarSaida(int registroId)
        {
            try
            {
                DateTime horarioSaida = DateTime.Now;

                Registro registro = _registroRepository.GetById(registroId);

                if (registro != null && registro.DatsaiReg != null) {
                    MessageBox.Show("Já foi realizada a saída para esse veículo.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                
                if (registro != null && registro.DatsaiReg == null)
                {
                    registro.DatsaiReg = horarioSaida;

                    _registroRepository.SaveChanges();
                }
                else
                {
                    MessageBox.Show("Por favor, selecione um registro.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ocorreu um erro ao registrar a saída do veículo: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
