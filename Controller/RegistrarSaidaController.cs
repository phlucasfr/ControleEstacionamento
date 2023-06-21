using ControleEstacionamento.Data;
using ControleEstacionamento.Data.Repositories;
using ControleEstacionamento.Models;
using ControleEstacionamento.Utils;
using System;
using System.Data.Entity;
using System.Windows.Forms;

namespace ControleEstacionamento.Controllers
{
    public class RegistrarSaidaController
    {

        public void RegistrarSaida(int registroId)
        {
            Registro.RegistrarSaida(registroId);
        }

    }
}
