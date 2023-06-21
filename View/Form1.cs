using ControleEstacionamento.Controller;
using ControleEstacionamento.Data.Repositories;
using ControleEstacionamento.Data;
using ControleEstacionamento.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ControleEstacionamento.Controllers;
using System.Xml.Schema;
using ControleEstacionamento.View;

namespace ControleEstacionamento
{
    public partial class Form1 : Form
    {
        private readonly DataGridController _dataGridController;
        private readonly RegistrarEntradaController _registrarEntradaController;
        private readonly RegistrarSaidaController _registrarSaidaController;


        public Form1()
        {
            InitializeComponent();

            var dbContext = new ApplicationDbContext();
            var registroRepository = new CrudRepository<Registro>(dbContext);
            var tabelaPrecosRepository = new CrudRepository<TabelaPrecos>(dbContext);
            var veiculoRepository = new CrudRepository<Veiculo>(dbContext);

            _dataGridController = new DataGridController(registroRepository, tabelaPrecosRepository, veiculoRepository);
            _registrarEntradaController = new RegistrarEntradaController();
            _registrarSaidaController = new RegistrarSaidaController();

            DataGridViewHelper.MontarGrid(this.dataGridView1);
            this.dataGridView1.CellClick += dataGridView1_CellClick;
            this.placaTextBox.TextChanged += PlacaTextBox_TextChanged;


            this.MinimumSize = new Size(this.Width, this.Height);
        }

        private void Form1_Load(object sender, EventArgs e)
        {                        
            var dateTimeController = new DateTimeController(this.dateTimeLbl);
            dateTimeController.StartUpdating();    

            DataGridViewHelper.AtualizarGrid(this.dataGridView1);
        }

        private void PlacaTextBox_TextChanged(object sender, EventArgs e)
        {
            // Obtém o texto da placa digitado pelo usuário
            string placa = placaTextBox.Text;                     

            DataGridViewHelper.AtualizarGrid(this.dataGridView1, placa);
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void dateTimeLbl_Click(object sender, EventArgs e)
        {

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
            _registrarEntradaController.RegistrarEntrada();
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
                    _registrarSaidaController.RegistrarSaida(codigoReg);
                    DataGridViewHelper.AtualizarGrid(this.dataGridView1);
                }
            }
            else
            {
                MessageBox.Show("Por favor, selecione um registro.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }
    }
}
