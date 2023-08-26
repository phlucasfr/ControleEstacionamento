using ControleEstacionamento.Models;
using System.Collections.Generic;
using System.Linq;

namespace ControleEstacionamento.Data.Repositories
{
    public class TabelaPrecosRepository : CrudRepository<TabelaPrecos>
    {
        public TabelaPrecosRepository(ApplicationDbContext context) : base(context)
        {
        }

        public IEnumerable<TabelaPrecos> GetAllTabelaPrecos()
        {
            return GetAll().ToList();
        }

        public void CreateTabelaPrecos(TabelaPrecos tabelaPrecos)
        {
            Add(tabelaPrecos);
            SaveChanges();
        }

        public void UpdateTabelaPrecos(TabelaPrecos tabelaPrecos)
        {
            Update(tabelaPrecos);
            SaveChanges();
        }
    }
}
