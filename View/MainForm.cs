using ControleEstacionamento.Controllers;
using ControleEstacionamento.View;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace ControleEstacionamento
{
    public partial class MainForm : Form
    {
        private readonly RegistroController _registroController;

        public MainForm()
        {
            InitializeComponent();

            _registroController = new RegistroController();

            DataGridViewHelper.MontarGrid(this.dataGridView1);
            this.dataGridView1.CellClick += dataGridView1_CellClick;
            this.placaTextBox.TextChanged += PlacaTextBox_TextChanged;

            this.MinimumSize = new Size(this.Width, this.Height);
            this.MaximumSize = new Size(this.Width, this.Height);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.timer1.Start();

            DataGridViewHelper.AtualizarGrid(this.dataGridView1);
        }

        private void PlacaTextBox_TextChanged(object sender, EventArgs e)
        {
            string placa = placaTextBox.Text;
            DataGridViewHelper.AtualizarGrid(this.dataGridView1, placa);
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = this.dataGridView1.Rows[e.RowIndex];
                row.Selected = true;
            }
        }

        private void btnRegistrarEntrada_Click(object sender, EventArgs e)
        {
            _registroController.RegistrarEntrada();
            DataGridViewHelper.AtualizarGrid(this.dataGridView1);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            TabelaPrecosForm tabelaPrecosForm = new TabelaPrecosForm();
            tabelaPrecosForm.ShowDialog();
        }

        private void button2_Click(object sender, EventArgs e)
        {

            if (this.dataGridView1.SelectedRows.Count > 0)
            {
                var selectedRow = this.dataGridView1.SelectedRows[0];
                var codigoReg = selectedRow.Cells["Codigo"].Value != null ? (int)selectedRow.Cells["Codigo"].Value : 0;

                if (codigoReg > 0)
                {
                    _registroController.RegistrarSaida(codigoReg);
                    DataGridViewHelper.AtualizarGrid(this.dataGridView1);
                }
            }
            else
            {
                MessageBox.Show("Por favor, selecione um registro.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            this.dateTimeLbl.Text = DateTime.Now.ToString("dddd, dd MMMM yyyy HH:mm:ss");
        }

    }
}
