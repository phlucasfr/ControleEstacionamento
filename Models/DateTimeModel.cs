using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ControleEstacionamento.Model
{
    public class DateTimeModel
    {
        public event Action<DateTime> DateTimeUpdated;

        public void StartUpdating()
        {
            // Inicia uma tarefa em segundo plano para atualizar a data e hora continuamente
            Task.Run(() =>
            {
                while (true)
                {
                    DateTime currentDateTime = DateTime.Now;

                    // Dispara o evento informando a data e hora atualizada
                    DateTimeUpdated?.Invoke(currentDateTime);

                    Thread.Sleep(1000);
                }
            });
        }
    }
}
