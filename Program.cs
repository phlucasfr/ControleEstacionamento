using ControleEstacionamento.Data;
using ControleEstacionamento.Data.Repositories;
using ControleEstacionamento.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ControleEstacionamento
{
    internal static class Program
    {
        /// <summary>
        /// Ponto de entrada principal para o aplicativo.
        /// </summary>
        [STAThread]
        static void Main()
        {
            //using (var context = new ApplicationDbContext())

            //if (context.Database.Exists())
            //{
            //}

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
            
        }
    }
}
