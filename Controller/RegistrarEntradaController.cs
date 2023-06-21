using ControleEstacionamento.Data;
using ControleEstacionamento.Data.Repositories;
using ControleEstacionamento.Models;
using ControleEstacionamento.Utils;
using System;
using System.Data.Entity;
using System.Windows.Forms;

namespace ControleEstacionamento.Controllers
{
    public class RegistrarEntradaController
    {
        private readonly CrudRepository<Veiculo> _veiculoRepository;
        private readonly CrudRepository<Registro> _registroRepository;

        public RegistrarEntradaController()
        {
            var dbContext = new ApplicationDbContext();
            _veiculoRepository = new CrudRepository<Veiculo>(dbContext);
            _registroRepository = new CrudRepository<Registro>(dbContext);
        }

        public void RegistrarEntrada()
        {
            try
            {
                var dataEntrada = DateTime.Now;
                var precoAtual = TabelaPrecosModel.ObterPrecoAtual(dataEntrada);

                if (precoAtual == null)
                {
                    MessageBox.Show("Por favor, adicione um preço na tabela de preços.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                string placa = Prompt.ShowDialog("Digite a placa do veículo:", "Registrar Entrada");

                if (!string.IsNullOrEmpty(placa))
                {
                    
                    DateTime horarioChegada = DateTime.Now;

                    Veiculo veiculo = new Veiculo
                    {
                        PlacaVei = placa                        
                    };
                    _veiculoRepository.Add(veiculo);
                    _veiculoRepository.SaveChanges();

                    Registro registro = new Registro
                    {
                        CodveiReg = veiculo.CodigoVei,
                        CodtprReg = (int)precoAtual,
                        DatentReg = horarioChegada
                    };
                    _registroRepository.Add(registro);
                    _registroRepository.SaveChanges();           
          
                }
    
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ocorreu um erro ao registrar a entrada do veículo: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }        
    }
}
