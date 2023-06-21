using ControleEstacionamento.Data.Repositories;
using ControleEstacionamento.Data;
using System;
using System.Drawing;
using System.Windows.Forms;
using ControleEstacionamento.Models;
using System.Globalization;

namespace ControleEstacionamento
{
    public partial class TabelaPrecosForm : Form
    {
        private TabelaPrecosRepository _tabelaPrecosRepository;

        private Label labelDatiniTpr;
        private Label labelDatfimTpr;
        private Label labelValhoriniTpr;
        private Label labelValhoradiTpr;

        private DateTimePicker dateTimePickerDatiniTpr;
        private DateTimePicker dateTimePickerDatfimTpr;
        private TextBox textBoxValhoriniTpr;
        private TextBox textBoxValhoradiTpr;

        private Button buttonCriar;
        private Button buttonExcluir;

        private DataGridView dataGridViewTabelaPrecos;

        public TabelaPrecosForm()
        {
            InitializeComponent();
            InitializeControls();

            var context = new ApplicationDbContext();
            _tabelaPrecosRepository = new TabelaPrecosRepository(context);
        }

        private void TabelaPrecosForm_Load(object sender, EventArgs e)
        {
            AtualizarDataGridViewTabelaPrecos();
        }

        private void InitializeControls()
        {
            Size = new Size(800, 600);

            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            MinimizeBox = false;

            labelDatiniTpr = new Label();
            labelDatfimTpr = new Label();
            labelValhoriniTpr = new Label();
            labelValhoradiTpr = new Label();

            dateTimePickerDatiniTpr = new DateTimePicker();
            dateTimePickerDatfimTpr = new DateTimePicker();
            textBoxValhoriniTpr = new TextBox();
            textBoxValhoradiTpr = new TextBox();

            buttonCriar = new Button();
            buttonExcluir = new Button();

            dataGridViewTabelaPrecos = new DataGridView();

            labelDatiniTpr.Text = "Data Inicial:";
            labelDatfimTpr.Text = "Data Final:";
            labelValhoriniTpr.Text = "Valor Hora Inicial:";
            labelValhoradiTpr.Text = "Valor Hora Adicional:";

            dateTimePickerDatiniTpr.Name = "dateTimePickerDatiniTpr";
            dateTimePickerDatfimTpr.Name = "dateTimePickerDatfimTpr";
            textBoxValhoriniTpr.Name = "textBoxValhoriniTpr";
            textBoxValhoradiTpr.Name = "textBoxValhoradiTpr";

            buttonCriar.Text = "Criar";
            buttonExcluir.Text = "Excluir";

            dataGridViewTabelaPrecos.Name = "dataGridViewTabelaPrecos";
            dataGridViewTabelaPrecos.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill; // Ajusta as colunas para preencher o espaço disponível
            dataGridViewTabelaPrecos.ReadOnly = true;
            dataGridViewTabelaPrecos.DefaultCellStyle.BackColor = Color.LightGray;

            Controls.Add(labelDatiniTpr);
            Controls.Add(dateTimePickerDatiniTpr);
            Controls.Add(labelDatfimTpr);
            Controls.Add(dateTimePickerDatfimTpr);
            Controls.Add(labelValhoriniTpr);
            Controls.Add(textBoxValhoriniTpr);
            Controls.Add(labelValhoradiTpr);
            Controls.Add(textBoxValhoradiTpr);

            Controls.Add(buttonCriar);
            Controls.Add(buttonExcluir);

            Controls.Add(dataGridViewTabelaPrecos);

            labelDatiniTpr.Location = new Point(20, 20);
            dateTimePickerDatiniTpr.Location = new Point(150, 20);
            labelDatfimTpr.Location = new Point(20, 60);
            dateTimePickerDatfimTpr.Location = new Point(150, 60);
            labelValhoriniTpr.Location = new Point(20, 100);
            textBoxValhoriniTpr.Location = new Point(150, 100);
            labelValhoradiTpr.Location = new Point(20, 140);
            labelValhoradiTpr.AutoSize = true;
            textBoxValhoradiTpr.Location = new Point(150, 140);

            buttonCriar.Size = new Size(80, 30);
            buttonExcluir.Size = new Size(80, 30);

            int buttonTopMargin = Math.Max(textBoxValhoradiTpr.Bottom, dataGridViewTabelaPrecos.Bottom) + 8;
            buttonCriar.Location = new Point(ClientSize.Width - buttonCriar.Width - 18, buttonTopMargin);
            buttonExcluir.Location = new Point(ClientSize.Width - buttonCriar.Width - buttonExcluir.Width - 40, buttonTopMargin);

            dataGridViewTabelaPrecos.Location = new Point(20, 200);
            dataGridViewTabelaPrecos.Size = new Size(ClientSize.Width - 40, ClientSize.Height - dataGridViewTabelaPrecos.Top - 30);
            dataGridViewTabelaPrecos.Columns.Add("CodigoTpr", "Código");
            dataGridViewTabelaPrecos.Columns["CodigoTpr"].Width = dataGridViewTabelaPrecos.Columns["CodigoTpr"].Width / 2;
            dataGridViewTabelaPrecos.Columns.Add("DatiniTpr", "Data Inicial");
            dataGridViewTabelaPrecos.Columns.Add("DatfimTpr", "Data Final");
            dataGridViewTabelaPrecos.Columns.Add("ValhoriniTpr", "Valor Hora Inicial");
            dataGridViewTabelaPrecos.Columns.Add("ValhoradiTpr", "Valor Hora Adicional");

            dataGridViewTabelaPrecos.DefaultCellStyle.Font = new Font("Arial", 8);
            dataGridViewTabelaPrecos.ColumnHeadersDefaultCellStyle.Font = new Font("Arial", 8, FontStyle.Bold);
            dataGridViewTabelaPrecos.ColumnHeadersDefaultCellStyle.BackColor = Color.LightGray;
            dataGridViewTabelaPrecos.ColumnHeadersDefaultCellStyle.WrapMode = DataGridViewTriState.True;
            dataGridViewTabelaPrecos.EnableHeadersVisualStyles = false;

            buttonCriar.Click += buttonCriar_Click;
            buttonExcluir.Click += buttonExcluir_Click;

            dataGridViewTabelaPrecos.CellClick += dataGridViewTabelaPrecos_CellClick;
        }

        private void dataGridViewTabelaPrecos_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridViewTabelaPrecos.Rows[e.RowIndex];
                row.Selected = true;
            }
        }

        private void buttonCriar_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(textBoxValhoriniTpr.Text) && !string.IsNullOrWhiteSpace(textBoxValhoradiTpr.Text))
            {
                var culture = new CultureInfo("pt-BR");

                var novaTabelaPrecos = new TabelaPrecos
                {
                    DatiniTpr = dateTimePickerDatiniTpr.Value,
                    DatfimTpr = dateTimePickerDatfimTpr.Value
                };

                string valhoriniTprString = textBoxValhoriniTpr.Text.Replace(".", ",");
                string valhoradiTprString = textBoxValhoradiTpr.Text.Replace(".", ",");

                if (decimal.TryParse(valhoriniTprString, NumberStyles.AllowDecimalPoint, culture, out decimal valhoriniTpr) &&
                    decimal.TryParse(valhoradiTprString, NumberStyles.AllowDecimalPoint, culture, out decimal valhoradiTpr))
                {
                    novaTabelaPrecos.ValhoriniTpr = valhoriniTpr;
                    novaTabelaPrecos.ValhoradiTpr = valhoradiTpr;

                    if (novaTabelaPrecos.DatiniTpr.Date == novaTabelaPrecos.DatfimTpr.Date)
                    {
                        MessageBox.Show("Por favor, selecione uma data final diferente da inicial.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                    {
                        _tabelaPrecosRepository.CreateTabelaPrecos(novaTabelaPrecos);
                        AtualizarDataGridViewTabelaPrecos();
                    }
                }
                else
                {
                    MessageBox.Show("Os valores de valor hora inicial e valor hora adicional estão em um formato inválido.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Por favor, preencha todos os campos.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void buttonExcluir_Click(object sender, EventArgs e)
        {
            if (dataGridViewTabelaPrecos.SelectedRows.Count > 0)
            {
                var selectedRow = dataGridViewTabelaPrecos.SelectedRows[0];
                var codigoTpr = selectedRow.Cells["CodigoTpr"].Value != null ? (int)selectedRow.Cells["CodigoTpr"].Value : 0;
                var datfimTpr = selectedRow.Cells["DatfimTpr"].Value != null ? (DateTime)selectedRow.Cells["DatfimTpr"].Value : DateTime.MinValue;

                if (codigoTpr > 0)
                {
                    _tabelaPrecosRepository.DeleteTabelaPrecos(codigoTpr, datfimTpr);
                    AtualizarDataGridViewTabelaPrecos();
                }
            }
            else
            {
                MessageBox.Show("Por favor, selecione um preço na tabela.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void AtualizarDataGridViewTabelaPrecos()
        {
            dataGridViewTabelaPrecos.Rows.Clear();
            dataGridViewTabelaPrecos.ReadOnly = true;

            var tabelaPrecosList = _tabelaPrecosRepository.GetAllTabelaPrecos();

            foreach (var tabelaPrecos in tabelaPrecosList)
            {
                int rowIndex = dataGridViewTabelaPrecos.Rows.Add();
                DataGridViewRow row = dataGridViewTabelaPrecos.Rows[rowIndex];

                row.Cells["CodigoTpr"].Value = tabelaPrecos.CodigoTpr;
                row.Cells["DatiniTpr"].Value = tabelaPrecos.DatiniTpr;
                row.Cells["DatfimTpr"].Value = tabelaPrecos.DatfimTpr;
                row.Cells["ValhoriniTpr"].Value = tabelaPrecos.ValhoriniTpr;
                row.Cells["ValhoradiTpr"].Value = tabelaPrecos.ValhoradiTpr;
            }
        }
    }
}
