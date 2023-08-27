using ControleEstacionamento.Controller;
using ControleEstacionamento.Data;
using ControleEstacionamento.Data.Repositories;
using System;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;

namespace ControleEstacionamento
{
    public partial class TabelaPrecosForm : Form
    {
        private TabelaPrecosRepository _tabelaPrecosRepository;
        private TabelaPrecosController _tabelaPrecosController = new TabelaPrecosController();

        private Label labelDatiniTpr;
        private Label labelDatfimTpr;
        private Label labelValhorTpr;

        private DateTimePicker dateTimePickerDatiniTpr;
        private DateTimePicker dateTimePickerDatfimTpr;
        private TextBox textBoxValhorTpr;

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
            labelValhorTpr = new Label();
            dateTimePickerDatiniTpr = new DateTimePicker();
            dateTimePickerDatfimTpr = new DateTimePicker();
            textBoxValhorTpr = new TextBox();
            buttonCriar = new Button();
            buttonExcluir = new Button();
            dataGridViewTabelaPrecos = new DataGridView();

            labelDatiniTpr.Text = "Data Inicial:";
            labelDatfimTpr.Text = "Data Final:";
            labelValhorTpr.Text = "Valor Hora:";
            dateTimePickerDatiniTpr.Name = "dateTimePickerDatiniTpr";
            dateTimePickerDatfimTpr.Name = "dateTimePickerDatfimTpr";
            textBoxValhorTpr.Name = "textBoxValhorTpr";
            buttonCriar.Text = "Criar";
            buttonExcluir.Text = "Excluir";
            dataGridViewTabelaPrecos.Name = "dataGridViewTabelaPrecos";

            dataGridViewTabelaPrecos.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridViewTabelaPrecos.ReadOnly = true;
            dataGridViewTabelaPrecos.DefaultCellStyle.BackColor = Color.LightGray;

            Controls.Add(labelDatiniTpr);
            Controls.Add(dateTimePickerDatiniTpr);
            Controls.Add(labelDatfimTpr);
            Controls.Add(dateTimePickerDatfimTpr);
            Controls.Add(labelValhorTpr);
            Controls.Add(textBoxValhorTpr);
            Controls.Add(buttonCriar);
            Controls.Add(buttonExcluir);
            Controls.Add(dataGridViewTabelaPrecos);

            int padding = 28;
            labelDatiniTpr.Location = new Point(padding, padding);
            labelDatiniTpr.Width = 70;
            dateTimePickerDatiniTpr.Location = new Point(100, labelDatiniTpr.Top);
            dateTimePickerDatiniTpr.Width = 125;

            labelDatfimTpr.Location = new Point(padding, dateTimePickerDatiniTpr.Bottom + padding);
            labelDatfimTpr.Width = 70;
            dateTimePickerDatfimTpr.Location = new Point(100, labelDatfimTpr.Top);
            dateTimePickerDatfimTpr.Width = 125;

            labelValhorTpr.Location = new Point(padding, dateTimePickerDatfimTpr.Bottom + padding);
            labelValhorTpr.Width = 70;
            textBoxValhorTpr.Location = new Point(100, labelValhorTpr.Top);
            textBoxValhorTpr.Width = 125;
            textBoxValhorTpr.TextAlign = HorizontalAlignment.Right;

            buttonCriar.Size = new Size(90, 35);
            buttonExcluir.Size = new Size(90, 35);

            // Ajusta a posição vertical dos botões para alinhar com a parte de baixo do textBoxValhoradiTpr.
            int buttonVerticalPosition = textBoxValhorTpr.Bottom - buttonCriar.Height;
            buttonCriar.Location = new Point(ClientSize.Width - buttonCriar.Width - padding, buttonVerticalPosition);
            buttonExcluir.Location = new Point(buttonCriar.Left - buttonExcluir.Width - padding, buttonVerticalPosition);

            dataGridViewTabelaPrecos.Location = new Point(20, 160);
            dataGridViewTabelaPrecos.Size = new Size(ClientSize.Width - 40, ClientSize.Height - dataGridViewTabelaPrecos.Top - 30);

            dataGridViewTabelaPrecos.Columns.Add("CodigoTpr", "Código");
            dataGridViewTabelaPrecos.Columns["CodigoTpr"].Width = dataGridViewTabelaPrecos.Columns["CodigoTpr"].Width / 2;
            dataGridViewTabelaPrecos.Columns.Add("DatiniTpr", "Data Inicial");
            dataGridViewTabelaPrecos.Columns.Add("DatfimTpr", "Data Final");
            dataGridViewTabelaPrecos.Columns.Add("ValhorTpr", "Valor Hora");

            dataGridViewTabelaPrecos.DefaultCellStyle.Font = new Font("Arial", 8);
            dataGridViewTabelaPrecos.ColumnHeadersDefaultCellStyle.Font = new Font("Arial", 8, FontStyle.Bold);
            dataGridViewTabelaPrecos.ColumnHeadersDefaultCellStyle.BackColor = Color.LightGray;
            dataGridViewTabelaPrecos.ColumnHeadersDefaultCellStyle.WrapMode = DataGridViewTriState.True;
            dataGridViewTabelaPrecos.EnableHeadersVisualStyles = false;

            dateTimePickerDatiniTpr.Format = DateTimePickerFormat.Custom;
            dateTimePickerDatiniTpr.CustomFormat = "dd/MM/yyyy HH:mm";
            dateTimePickerDatiniTpr.ShowUpDown = true;
            dateTimePickerDatfimTpr.Format = DateTimePickerFormat.Custom;
            dateTimePickerDatfimTpr.CustomFormat = "dd/MM/yyyy HH:mm";
            dateTimePickerDatfimTpr.ShowUpDown = true;

            Font fontPicker = new Font("Arial", 10);
            dateTimePickerDatiniTpr.Font = fontPicker;
            dateTimePickerDatfimTpr.Font = fontPicker;
            dateTimePickerDatiniTpr.BackColor = Color.LightGray;
            dateTimePickerDatfimTpr.BackColor = Color.LightGray;

            buttonCriar.Click += buttonCriar_Click;
            buttonExcluir.Click += buttonExcluir_Click;
            dataGridViewTabelaPrecos.CellClick += dataGridViewTabelaPrecos_CellClick;
            textBoxValhorTpr.LostFocus += TextBoxValhorTpr_LostFocus;
            this.MouseClick += Form1_MouseClick;
        }

        private void Form1_MouseClick(object sender, MouseEventArgs e)
        {
            // Remove o foco do controle ativo.
            ActiveControl = null;
        }

        private void TextBoxValhorTpr_LostFocus(object sender, EventArgs e)
        {
            if (decimal.TryParse(textBoxValhorTpr.Text, out decimal value))
            {
                textBoxValhorTpr.Text = string.Format(CultureInfo.GetCultureInfo("pt-BR"), "{0:C}", value);
            }
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
            _tabelaPrecosController.RegistrarNovoPreco(dateTimePickerDatiniTpr, dateTimePickerDatfimTpr, textBoxValhorTpr, AtualizarDataGridViewTabelaPrecos);

            textBoxValhorTpr.Text = "";
            dateTimePickerDatiniTpr.Value = DateTime.Now;
            dateTimePickerDatfimTpr.Value = DateTime.Now;
        }

        private void buttonExcluir_Click(object sender, EventArgs e)
        {
            _tabelaPrecosController.ExcluirPreco(dataGridViewTabelaPrecos, AtualizarDataGridViewTabelaPrecos);
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
                row.Cells["ValhorTpr"].Value = tabelaPrecos.ValhorTpr.ToString("C2");
            }
        }
    }
}
