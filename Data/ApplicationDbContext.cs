using ControleEstacionamento.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;

namespace ControleEstacionamento.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext() : base(new SQLiteConnection()
        {
            ConnectionString = new SQLiteConnectionStringBuilder()
            {
                DataSource = "estacionamento.db",
                ForeignKeys = true
            }.ConnectionString
        }, true)
        {
            Database.SetInitializer<ApplicationDbContext>(null);
        }

        public DbSet<Veiculo> Veiculo { get; set; }
        public DbSet<Registro> Registro { get; set; }
        public DbSet<TabelaPrecos> TabelaPrecos { get; set; }
    }

}
