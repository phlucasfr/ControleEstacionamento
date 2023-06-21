using ControleEstacionamento.Data.Repositories;
using ControleEstacionamento.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

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

        public void DeleteTabelaPrecos(int codigoTpr, DateTime datfimTpr)
        {
            var tabelaPrecos = GetById(codigoTpr);
            if (tabelaPrecos != null)
            {
                using (var context = new ApplicationDbContext())
                {
                    var registrosRepository = new CrudRepository<Registro>(context);
                    var registrosComPreco = registrosRepository.GetAll().Where(r => r.CodtprReg == codigoTpr);

                    if (registrosComPreco.Any() && datfimTpr < DateTime.Now)
                    {
                        Delete(tabelaPrecos);
                        SaveChanges();
                    }

                    if (registrosComPreco.Any() && datfimTpr > DateTime.Now)
                    {
                        MessageBox.Show("O preço está sendo usado em um ou mais registros e não pode ser deletado.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                    {
                        Delete(tabelaPrecos);
                        SaveChanges();
                    }                    
                }
            }
        }

    }
}
