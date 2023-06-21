using ControleEstacionamento.Data.Repositories;
using ControleEstacionamento.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ControleEstacionamento.Controllers
{
    public class DataGridController
    {
        private readonly CrudRepository<Registro> _registros;
        private readonly CrudRepository<TabelaPrecos> _tabelaPrecos;
        private readonly CrudRepository<Veiculo> _veiculos;

        public DataGridController(CrudRepository<Registro> registroRepository, CrudRepository<TabelaPrecos> tabelaPrecosRepository, CrudRepository<Veiculo> veiculoRepository)
        {
            _registros = registroRepository;
            _tabelaPrecos = tabelaPrecosRepository;
            _veiculos = veiculoRepository;
        }

        public List<DataGrid> GetGridData(string placa = null)
        {
            var registros = _registros.GetAll();
            var tabelasPrecos = _tabelaPrecos.GetAll();
            var veiculos = _veiculos.GetAll();

            if (placa != null)
            {
                veiculos = veiculos.Where(v => v.PlacaVei.StartsWith(placa, StringComparison.OrdinalIgnoreCase));
            }

            var dataGridList = new List<DataGrid>();

            // Ordena pelo mais recente
            foreach (var registro in registros.OrderByDescending(r => r.DatentReg))
            {
                var veiculo = veiculos.FirstOrDefault(v => v.CodigoVei == registro.CodveiReg);
                var preco = tabelasPrecos.FirstOrDefault(p => p.CodigoTpr == registro.CodtprReg);

                if (veiculo != null && preco != null)
                {
                    var duracao = (registro.DatsaiReg - registro.DatentReg)?.TotalMinutes;
                    var tempoCobrado = Math.Ceiling((registro.DatsaiReg - registro.DatentReg)?.TotalHours ?? 0);
                    decimal valorAPagar = 0.00m;

                    if (registro.DatsaiReg.HasValue)
                    {
                        if (duracao <= 30)
                        {
                            // Cobrança de metade do valor inicial
                            valorAPagar = preco.ValhoriniTpr / 2;
                        }
                        else if (duracao <= 60)
                        {
                            // Cobrança do valor inicial
                            valorAPagar = preco.ValhoriniTpr;
                        }
                        else
                        {
                            decimal valorHoraInicial = preco.ValhoriniTpr;
                            decimal valorHoraAdicional = preco.ValhoradiTpr;

                            // Obtém o número de horas completas
                            int horasCompletas = (int)Math.Floor(duracao.GetValueOrDefault() / 60.0);

                            // Verifica se a duração ultrapassa a tolerância
                            if (duracao > 70) // 60 minutos + tolerância de 10 minutos
                            {
                                // Cobrança do valor inicial + valor adicional
                                valorAPagar = valorHoraInicial + valorHoraAdicional;
                            }
                            else
                            {
                                // Cobrança do valor inicial
                                valorAPagar = valorHoraInicial;
                            }

                            // Adiciona o valor das horas adicionais
                            if (horasCompletas > 0)
                            {
                                valorAPagar += (horasCompletas * valorHoraAdicional);
                            }
                        }
                    }

                    var dataGrid = new DataGrid
                    {
                        Codigo = registro.CodigoReg,
                        Placa = veiculo.PlacaVei,
                        HorarioChegada = registro.DatentReg,
                        HorarioSaida = registro.DatsaiReg,
                        Duracao = TimeSpan.FromMinutes(duracao ?? 0),
                        TempoCobrado = (decimal)tempoCobrado,
                        Preco = preco.ValhoriniTpr,
                        ValorAPagar = valorAPagar
                    };

                    dataGridList.Add(dataGrid);
                }
            }

            return dataGridList;
        }

        public void LimparTabelas()
        {
            // Exclui todos os registros da tabela Registro
            var registros = _registros.GetAll();
            foreach (var registro in registros)
            {
                _registros.Delete(registro);
            }
            _registros.SaveChanges();

            // Exclui todos os registros da tabela Veiculo
            var veiculos = _veiculos.GetAll();
            foreach (var veiculo in veiculos)
            {
                _veiculos.Delete(veiculo);
            }
            _veiculos.SaveChanges();

            // Exclui todos os registros da tabela TabelaPrecos
            var tabelasPrecos = _tabelaPrecos.GetAll();
            foreach (var tabelaPreco in tabelasPrecos)
            {
                _tabelaPrecos.Delete(tabelaPreco);
            }
            _tabelaPrecos.SaveChanges();
        }
    }
}
