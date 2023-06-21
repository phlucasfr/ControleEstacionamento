using ControleEstacionamento.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControleEstacionamento.Data.Repositories
{
    public class MyRepository
    {
        private readonly CrudRepository<Registro> _registroRepository;
        private readonly CrudRepository<TabelaPrecos> _tabelaPrecosRepository;
        private readonly CrudRepository<Veiculo> _veiculoRepository;

        public MyRepository(ApplicationDbContext context)
        {
            _registroRepository = new CrudRepository<Registro>(context);
            _tabelaPrecosRepository = new CrudRepository<TabelaPrecos>(context);
            _veiculoRepository = new CrudRepository<Veiculo>(context);
        }
    }
}
