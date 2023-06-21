using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ControleEstacionamento.Utils
{
    public static class Prompt
    {
        public static string ShowDialog(string text, string caption)
        {
            Form prompt = new Form()
            {
                Width = 300,
                Height = 180,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                Text = caption,
                StartPosition = FormStartPosition.CenterScreen
            };
            Label textLabel = new Label() { Left = 20, Top = 20, Text = text, AutoSize = true };
            TextBox textBox = new TextBox() { Left = 20, Top = 50, Width = 250 };
            Button confirmation = new Button() { Text = "OK", Width = 100, Top = 100, DialogResult = DialogResult.OK };
            Button cancelation = new Button() { Text = "Cancelar", Width = 100, Top = 100, DialogResult = DialogResult.Cancel };
            confirmation.Left = textBox.Left + textBox.Width - confirmation.Width;
            cancelation.Left = textBox.Left;
            confirmation.Click += (sender, e) => { prompt.Close(); };
            cancelation.Click += (sender, e) => { prompt.Close(); };
            prompt.Controls.Add(confirmation);
            prompt.Controls.Add(cancelation);
            prompt.Controls.Add(textLabel);
            prompt.Controls.Add(textBox);
            prompt.AcceptButton = confirmation;

            return prompt.ShowDialog() == DialogResult.OK ? textBox.Text : "";
        }
    }

}
