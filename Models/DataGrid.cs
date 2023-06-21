using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControleEstacionamento.Models
{
    public class DataGrid
    {
        public int Codigo { get; set; }
        public string Placa { get; set; }
        public DateTime HorarioChegada { get; set; }
        public DateTime? HorarioSaida { get; set; }
        public TimeSpan Duracao { get; set; }
        public decimal TempoCobrado { get; set; }
        public decimal Preco { get; set; }
        public decimal ValorAPagar { get; set; }
    }
}
