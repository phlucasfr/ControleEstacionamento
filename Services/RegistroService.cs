using ControleEstacionamento.Data;
using ControleEstacionamento.Data.Repositories;
using ControleEstacionamento.Models;
using System;
using System.Linq;

namespace ControleEstacionamento.Services
{
    public class RegistroService
    {
        private readonly CrudRepository<Registro> _registroRepository;
        private readonly CrudRepository<Veiculo> _veiculoRepository;
        private readonly TabelaPrecosService _tabelaPrecosService;

        public RegistroService()
        {
            var dbContext = new ApplicationDbContext();
            _registroRepository = new CrudRepository<Registro>(dbContext);
            _veiculoRepository = new CrudRepository<Veiculo>(dbContext);
            _tabelaPrecosService = new TabelaPrecosService();
        }

        public ResultadoRegistro RegistrarEntrada(string placa)
        {
            try
            {
                var dataEntrada = DateTime.Now;
                var precoAtual = _tabelaPrecosService.ObterPrecoAtual(dataEntrada);

                if (precoAtual == null)
                {
                    return new ResultadoRegistro(false, "Por favor, adicione um preço na tabela de preços.");
                }

                if (string.IsNullOrEmpty(placa))
                {
                    return new ResultadoRegistro(false, "Placa do veículo não fornecida.");
                }

                var veiculoExistente = _veiculoRepository.Find(v => v.PlacaVei == placa.ToUpper()).FirstOrDefault();

                if (veiculoExistente == null)
                {
                    Veiculo veiculo = new Veiculo
                    {
                        PlacaVei = placa.ToUpper()
                    };

                    _veiculoRepository.Add(veiculo);
                    _veiculoRepository.SaveChanges();

                    veiculoExistente = veiculo;
                }

                var registroAberto = _registroRepository.Find(r => r.CodveiReg == veiculoExistente.CodigoVei && r.DatsaiReg == null).FirstOrDefault();

                if (registroAberto != null)
                {
                    throw new InvalidOperationException("Já existe um registro em aberto para essa placa.");
                }
                else
                {
                    Registro registro = new Registro
                    {
                        CodveiReg = veiculoExistente.CodigoVei,
                        CodtprReg = (int)precoAtual,
                        DatentReg = dataEntrada
                    };

                    _registroRepository.Add(registro);
                    _registroRepository.SaveChanges();
                }

                return new ResultadoRegistro(true, "Entrada registrada com sucesso.");
            }
            catch (Exception ex)
            {
                return new ResultadoRegistro(false, $"Ocorreu um erro ao registrar a entrada do veículo: {ex.Message}");
            }
        }

        public ResultadoRegistro RegistrarSaida(int registroId)
        {
            try
            {
                DateTime horarioSaida = DateTime.Now;

                Registro registro = _registroRepository.GetById(registroId);

                if (registro != null && registro.DatsaiReg != null)
                {
                    return new ResultadoRegistro(false, "Já foi realizada a saída para esse veículo.");
                }

                if (registro != null && registro.DatsaiReg == null)
                {
                    registro.DatsaiReg = horarioSaida;
                    _registroRepository.SaveChanges();
                    return new ResultadoRegistro(true, "Saída registrada com sucesso.");
                }

                return new ResultadoRegistro(false, "Por favor, selecione um registro.");
            }
            catch (Exception ex)
            {
                return new ResultadoRegistro(false, $"Ocorreu um erro ao registrar a saída do veículo: {ex.Message}");
            }
        }
    }

    public struct ResultadoRegistro
    {
        public bool Sucesso { get; }
        public string Mensagem { get; }

        public ResultadoRegistro(bool sucesso, string mensagem)
        {
            Sucesso = sucesso;
            Mensagem = mensagem;
        }
    }

}
