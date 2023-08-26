using ControleEstacionamento.Data;
using ControleEstacionamento.Data.Repositories;
using ControleEstacionamento.Models;
using System;
using System.Globalization;
using System.Linq;

namespace ControleEstacionamento.Services
{
    public class TabelaPrecosService
    {
        private readonly CrudRepository<TabelaPrecos> _tabelaPrecosRepository;

        public TabelaPrecosService()
        {
            var dbContext = new ApplicationDbContext();
            _tabelaPrecosRepository = new CrudRepository<TabelaPrecos>(dbContext);
        }

        public int? ObterPrecoAtual(DateTime dataEntrada)
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

        public ResultadoRegistro CriarPreco(DateTime inicio, DateTime fim, string valorInicialStr)
        {
            var culture = new CultureInfo("pt-BR");

            valorInicialStr = valorInicialStr.Replace("R$", "").Trim().Replace(".", ",");

            if (!decimal.TryParse(valorInicialStr, NumberStyles.Currency, culture, out decimal valhorTpr))
            {
                return new ResultadoRegistro(false, "O valor hora inicial está em um formato inválido.");
            }

            DateTime agora = DateTime.Now;

            if (inicio <= agora)
            {
                return new ResultadoRegistro(false, "Por favor, selecione uma data e hora iniciais que sejam posteriores ao momento atual.");
            }

            if (fim <= inicio)
            {
                return new ResultadoRegistro(false, "Por favor, selecione uma data e hora de término que sejam posteriores à data e hora de início.");
            }

            // Verifica no banco se já existe um preço atual para as datas especificadas
            var precoAtualInicio = ObterPrecoAtual(inicio);
            var precoAtualFim = ObterPrecoAtual(fim);

            if (precoAtualInicio.HasValue || precoAtualFim.HasValue)
            {
                return new ResultadoRegistro(false, "Já existe um preço registrado para o período especificado.");
            }

            var novaTabelaPrecos = new TabelaPrecos
            {
                DatiniTpr = inicio,
                DatfimTpr = fim,
                ValhorTpr = valhorTpr,
            };

            _tabelaPrecosRepository.Add(novaTabelaPrecos);
            _tabelaPrecosRepository.SaveChanges();

            return new ResultadoRegistro(true, "Novo preço criado com sucesso.");
        }

        public ResultadoRegistro ExcluirPreco(int codigoTpr, DateTime datfimTpr)
        {
            var tabelaPrecos = _tabelaPrecosRepository.GetById(codigoTpr);
            if (tabelaPrecos != null)
            {
                using (var context = new ApplicationDbContext())
                {
                    var registrosRepository = new CrudRepository<Registro>(context);
                    var registrosComPreco = registrosRepository.GetAll().Where(r => r.CodtprReg == codigoTpr);

                    if (registrosComPreco.Any() && datfimTpr < DateTime.Now)
                    {
                        _tabelaPrecosRepository.Delete(tabelaPrecos);
                        _tabelaPrecosRepository.SaveChanges();
                        return new ResultadoRegistro(true, "Preço deletado com sucesso.");
                    }

                    if (registrosComPreco.Any() && datfimTpr > DateTime.Now)
                    {
                        return new ResultadoRegistro(false, "O preço está sendo usado em um ou mais registros e não pode ser deletado.");
                    }
                    else
                    {
                        _tabelaPrecosRepository.Delete(tabelaPrecos);
                        _tabelaPrecosRepository.SaveChanges();
                        return new ResultadoRegistro(true, "Preço deletado com sucesso.");
                    }
                }
            }
            else
            {
                return new ResultadoRegistro(false, "Preço não encontrado.");
            }
        }
    }
}
