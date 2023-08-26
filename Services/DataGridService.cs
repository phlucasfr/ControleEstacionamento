using ControleEstacionamento.Data.Repositories;
using ControleEstacionamento.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ControleEstacionamento.Services
{
    public class DataGridService
    {
        private readonly CrudRepository<Registro> _registros;
        private readonly CrudRepository<TabelaPrecos> _tabelaPrecos;
        private readonly CrudRepository<Veiculo> _veiculos;

        public DataGridService(CrudRepository<Registro> registroRepository, CrudRepository<TabelaPrecos> tabelaPrecosRepository, CrudRepository<Veiculo> veiculoRepository)
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
                veiculos = veiculos.Where(v => v.PlacaVei.Contains(placa.ToUpper()));
            }

            var dataGridList = new List<DataGrid>();

            // Ordena pelo mais recente
            foreach (var registro in registros.OrderByDescending(r => r.DatentReg))
            {
                var veiculo = veiculos.FirstOrDefault(v => v.CodigoVei == registro.CodveiReg);
                var preco = tabelasPrecos.FirstOrDefault(p => p.CodigoTpr == registro.CodtprReg);
                var tempoCobrado = Math.Ceiling((registro.DatsaiReg - registro.DatentReg)?.TotalHours ?? 0);

                if (veiculo != null && preco != null)
                {
                    var duracao = (registro.DatsaiReg - registro.DatentReg)?.TotalMinutes;
                    decimal valorAPagar = 0.00m;

                    if (registro.DatsaiReg.HasValue)
                    {
                        if (duracao <= 30)
                        {
                            // Cobrança de metade do valor inicial
                            valorAPagar = preco.ValhorTpr / 2;
                            tempoCobrado = 0.5;
                        }
                        else
                        {
                            // Cobrança da primeira hora completa
                            valorAPagar = preco.ValhorTpr;

                            // Calcula o tempo adicional
                            double tempoAdicional = duracao.GetValueOrDefault() - 60;

                            // Calcula horas completas e minutos excedentes
                            int horasAdicionais = (int)tempoAdicional / 60;
                            double minutosRestantes = tempoAdicional % 60;

                            tempoCobrado = horasAdicionais + 1;

                            // Adiciona o valor das horas completas
                            valorAPagar += (decimal)horasAdicionais * preco.ValhorTpr;

                            // Verifica a tolerância dos minutos restantes
                            if (minutosRestantes > 10)
                            {
                                valorAPagar += preco.ValhorTpr;
                                tempoCobrado += 1;
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
                        Preco = preco.ValhorTpr,
                        ValorAPagar = valorAPagar
                    };

                    dataGridList.Add(dataGrid);
                }
            }

            return dataGridList;
        }

        public void LimparTabelas()
        {
            var registros = _registros.GetAll();
            foreach (var registro in registros)
            {
                _registros.Delete(registro);
            }
            _registros.SaveChanges();

            var veiculos = _veiculos.GetAll();
            foreach (var veiculo in veiculos)
            {
                _veiculos.Delete(veiculo);
            }
            _veiculos.SaveChanges();

            var tabelasPrecos = _tabelaPrecos.GetAll();
            foreach (var tabelaPreco in tabelasPrecos)
            {
                _tabelaPrecos.Delete(tabelaPreco);
            }
            _tabelaPrecos.SaveChanges();
        }
    }

}
