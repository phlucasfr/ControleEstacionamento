using ControleEstacionamento.Services;
using System;
using System.Windows.Forms;

namespace ControleEstacionamento.Controller
{
    public class TabelaPrecosController
    {
        private readonly TabelaPrecosService _tabelaPrecosService;

        public TabelaPrecosController()
        {
            _tabelaPrecosService = new TabelaPrecosService();
        }

        public void RegistrarNovoPreco(DateTimePicker dateTimePickerDatiniTpr, DateTimePicker dateTimePickerDatfimTpr, TextBox textBoxValhoriniTpr, Action atualizarDataGridView)
        {
            if (!string.IsNullOrWhiteSpace(textBoxValhoriniTpr.Text))
            {
                var resultado = _tabelaPrecosService.CriarPreco(dateTimePickerDatiniTpr.Value, dateTimePickerDatfimTpr.Value, textBoxValhoriniTpr.Text);

                MessageBox.Show(resultado.Mensagem, resultado.Sucesso ? "Informação" : "Erro", MessageBoxButtons.OK, resultado.Sucesso ? MessageBoxIcon.Information : MessageBoxIcon.Error);

                if (resultado.Sucesso && atualizarDataGridView != null)
                {
                    atualizarDataGridView();
                }
            }
            else
            {
                MessageBox.Show("Por favor, preencha todos os campos.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void ExcluirPreco(DataGridView dataGridViewTabelaPrecos, Action atualizarDataGridView)
        {
            if (dataGridViewTabelaPrecos.SelectedRows.Count > 0)
            {
                var selectedRow = dataGridViewTabelaPrecos.SelectedRows[0];
                var codigoTpr = selectedRow.Cells["CodigoTpr"].Value != null ? (int)selectedRow.Cells["CodigoTpr"].Value : 0;
                var datfimTpr = selectedRow.Cells["DatfimTpr"].Value != null ? (DateTime)selectedRow.Cells["DatfimTpr"].Value : DateTime.MinValue;

                var resultado = _tabelaPrecosService.ExcluirPreco(codigoTpr, datfimTpr);

                MessageBox.Show(resultado.Mensagem, resultado.Sucesso ? "Informação" : "Erro", MessageBoxButtons.OK, resultado.Sucesso ? MessageBoxIcon.Information : MessageBoxIcon.Error);

                if (resultado.Sucesso && atualizarDataGridView != null)
                {
                    atualizarDataGridView();
                }
            }
            else
            {
                MessageBox.Show("Por favor, selecione um preço na tabela.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

    }
}
