using ControleEstacionamento.Data.Repositories;
using ControleEstacionamento.Models;
using ControleEstacionamento.Services;
using System.Collections.Generic;

namespace ControleEstacionamento.Controllers
{
    public class DataGridController
    {
        private readonly DataGridService _dataGridService;

        public DataGridController(CrudRepository<Registro> registroRepository, CrudRepository<TabelaPrecos> tabelaPrecosRepository, CrudRepository<Veiculo> veiculoRepository)
        {
            _dataGridService = new DataGridService(registroRepository, tabelaPrecosRepository, veiculoRepository);
        }

        public List<ControleEstacionamento.Models.DataGrid> GetGridData(string placa = null)
        {
            return _dataGridService.GetGridData(placa);
        }

        public void LimparTabelas()
        {
            _dataGridService.LimparTabelas();
        }
    }
}
