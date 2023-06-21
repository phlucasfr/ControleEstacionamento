using ControleEstacionamento.Controllers;
using ControleEstacionamento.Data.Repositories;
using ControleEstacionamento.Data;
using ControleEstacionamento.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.CompilerServices;

namespace ControleEstacionamento.View
{
    public class DataGridViewHelper
    {

        public static void AtualizarGrid(DataGridView dataGridView, string placa = null)
        {
            var dbContext = new ApplicationDbContext();
            var registroRepository = new CrudRepository<Registro>(dbContext);
            var tabelaPrecosRepository = new CrudRepository<TabelaPrecos>(dbContext);
            var veiculoRepository = new CrudRepository<Veiculo>(dbContext);

            var _dataGridController = new DataGridController(registroRepository, tabelaPrecosRepository, veiculoRepository);
            var dataGridData = _dataGridController.GetGridData(placa);
            //_dataGridController.LimparTabelas();

            dataGridView.Rows.Clear();

            foreach (var data in dataGridData)
            {
                int rowIndex = dataGridView.Rows.Add();
                DataGridViewRow row = dataGridView.Rows[rowIndex];

                row.Cells["Codigo"].Value = data.Codigo;
                row.Cells["Placa"].Value = data.Placa;
                row.Cells["HorarioChegada"].Value = data.HorarioChegada;
                row.Cells["HorarioSaida"].Value = data.HorarioSaida;

                string duracaoFormatted = data.Duracao.ToString(@"hh\:mm\:ss");
                row.Cells["Duracao"].Value = duracaoFormatted;

                row.Cells["TempoCobrado"].Value = data.TempoCobrado;
                row.Cells["Preco"].Value = data.Preco.ToString("C2");
                row.Cells["ValorAPagar"].Value = data.ValorAPagar.ToString("C2");
            }

            dataGridView.Refresh();
        }

        public static void MontarGrid(DataGridView dataGridView)
        {
            dataGridView.Columns.Clear();

            dataGridView.Columns.Add("Codigo", "Código");
            dataGridView.Columns.Add("Placa", "Placa");
            dataGridView.Columns.Add("HorarioChegada", "Horário de Chegada");
            dataGridView.Columns.Add("HorarioSaida", "Horário de Saída");
            dataGridView.Columns.Add("Duracao", "Duração");
            dataGridView.Columns.Add("TempoCobrado", "Tempo Cobrado (hora)");
            dataGridView.Columns.Add("Preco", "Preço");
            dataGridView.Columns.Add("ValorAPagar", "Valor a Pagar");

            dataGridView.Columns["Placa"].DataPropertyName = "Placa";
            dataGridView.Columns["HorarioChegada"].DataPropertyName = "HorarioChegada";
            dataGridView.Columns["HorarioSaida"].DataPropertyName = "HorarioSaida";
            dataGridView.Columns["Duracao"].DataPropertyName = "Duracao";
            dataGridView.Columns["TempoCobrado"].DataPropertyName = "TempoCobrado";
            dataGridView.Columns["Preco"].DataPropertyName = "Preco";
            dataGridView.Columns["ValorAPagar"].DataPropertyName = "ValorAPagar";
            dataGridView.ReadOnly = true;
        }
    }
}
