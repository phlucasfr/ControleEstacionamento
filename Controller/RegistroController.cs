using ControleEstacionamento.Services;
using ControleEstacionamento.Utils;
using System.Windows.Forms;

namespace ControleEstacionamento.Controllers
{
    public class RegistroController
    {
        private readonly RegistroService _registroService;

        public RegistroController()
        {
            _registroService = new RegistroService();
        }

        public void RegistrarEntrada()
        {
            string placa = Prompt.ShowDialog("Digite a placa do veículo:", "Registrar Entrada");

            var resultado = _registroService.RegistrarEntrada(placa);

            MessageBox.Show(resultado.Mensagem, resultado.Sucesso ? "Informação" : "Erro", MessageBoxButtons.OK, resultado.Sucesso ? MessageBoxIcon.Information : MessageBoxIcon.Error);
        }

        public void RegistrarSaida(int registroId)
        {
            var resultado = _registroService.RegistrarSaida(registroId);
            MessageBox.Show(resultado.Mensagem, resultado.Sucesso ? "Informação" : "Erro", MessageBoxButtons.OK, resultado.Sucesso ? MessageBoxIcon.Information : MessageBoxIcon.Error);
        }
    }
}
