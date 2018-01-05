using System;
using System.Drawing;
using System.Windows.Forms;

namespace MGMPDV.Formularios
{
    public partial class FInputDataPicker : Form
    {
        public DateTime data { get; set; }
        public bool ok { get; set; }
        public string valor { get; set; }
        public bool somenteleitura = false;
        string valorgeral = "";

        public FInputDataPicker(string datalabel, DateTime data, string valorlabel,string valor)
        {
            InitializeComponent();
            pnltitulo.BackColor = Color.FromArgb(50, Color.Black);
            ok = false;
            lblvalor.Text = valorlabel;
            lbldata.Text = datalabel;
            dtp.Value = data;
            this.data = data;
            ttbvalor.Text = valor;
            this.valor = valor;
        }

        public FInputDataPicker(string datalabel, DateTime data, string valorlabel, string valor, bool somenteleitura)
        {
            InitializeComponent();
            pnltitulo.BackColor = Color.FromArgb(50, Color.Black);
            ok = false;
            lblvalor.Text = valorlabel;
            lbldata.Text = datalabel;
            dtp.Value = data;
            this.data = data;
            ttbvalor.Text = valor;
            this.valor = valor;
            if(somenteleitura)
            {
                ttbvalor.ReadOnly = true;
            }
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Escape)
            {
                btncancelar.PerformClick();
                return true;   // indicate that you handled this keystroke
            }

            if (keyData == Keys.Enter)
            {
                btnconfirmar.PerformClick();
                return true;   // indicate that you handled this keystroke
            }




            // Call the base class
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void btnconfirmar_Click(object sender, EventArgs e)
        {
            ok = true;
            data = dtp.Value;
            try
            {
                decimal.Parse(ttbvalor.Text);
            }
            catch { FMessageOk f = new FMessageOk(); f.Mostrar("Valor inválido!"); return; }
            valor = ttbvalor.Text;
            Close();
        }

        private void btncancelar_Click(object sender, EventArgs e)
        {
            ok = false;
            data = dtp.Value;
            valor = ttbvalor.Text;
            Close();
        }

        private void ttbagencia_MouseClick(object sender, MouseEventArgs e)
        {
            valorgeral = "agencia";
        }

        private void ttbconta_MouseClick(object sender, MouseEventArgs e)
        {
            valorgeral = "conta";
        }

        private void ttbcpf_MouseClick(object sender, MouseEventArgs e)
        {
            valorgeral = "cpf";
        }

        private void ttbrg_MouseClick(object sender, MouseEventArgs e)
        {
            valorgeral = "rg";
        }

        private void ttbbanco_MouseClick(object sender, MouseEventArgs e)
        {
            valorgeral = "banco";
        }

        private void ttbcheque_MouseClick(object sender, MouseEventArgs e)
        {
            valorgeral = "cheque";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (valorgeral == "agencia")
            {
                ttbagencia.Text += button1.Text;
            }
            if (valorgeral == "cpf")
            {
                ttbcpf.Text += button1.Text;
            }
            if (valorgeral == "conta")
            {
                ttbconta.Text += button1.Text;
            }
            if (valorgeral == "rg")
            {
                ttbrg.Text += button1.Text;
            }
            if (valorgeral == "banco")
            {
                ttbbanco.Text += button1.Text;
            }
            if (valorgeral == "cheque")
            {
                ttbcheque.Text += button1.Text;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (valorgeral == "agencia")
            {
                ttbagencia.Text += button2.Text;
            }
            if (valorgeral == "cpf")
            {
                ttbcpf.Text += button2.Text;
            }
            if (valorgeral == "conta")
            {
                ttbconta.Text += button2.Text;
            }
            if (valorgeral == "rg")
            {
                ttbrg.Text += button2.Text;
            }
            if (valorgeral == "banco")
            {
                ttbbanco.Text += button2.Text;
            }
            if (valorgeral == "cheque")
            {
                ttbcheque.Text += button2.Text;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (valorgeral == "agencia")
            {
                ttbagencia.Text += button3.Text;
            }
            if (valorgeral == "cpf")
            {
                ttbcpf.Text += button3.Text;
            }
            if (valorgeral == "conta")
            {
                ttbconta.Text += button3.Text;
            }
            if (valorgeral == "rg")
            {
                ttbrg.Text += button3.Text;
            }
            if (valorgeral == "banco")
            {
                ttbbanco.Text += button3.Text;
            }
            if (valorgeral == "cheque")
            {
                ttbcheque.Text += button3.Text;
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (valorgeral == "agencia")
            {
                ttbagencia.Text += button4.Text;
            }
            if (valorgeral == "cpf")
            {
                ttbcpf.Text += button4.Text;
            }
            if (valorgeral == "conta")
            {
                ttbconta.Text += button4.Text;
            }
            if (valorgeral == "rg")
            {
                ttbrg.Text += button4.Text;
            }
            if (valorgeral == "banco")
            {
                ttbbanco.Text += button4.Text;
            }
            if (valorgeral == "cheque")
            {
                ttbcheque.Text += button4.Text;
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (valorgeral == "agencia")
            {
                ttbagencia.Text += button5.Text;
            }
            if (valorgeral == "cpf")
            {
                ttbcpf.Text += button5.Text;
            }
            if (valorgeral == "conta")
            {
                ttbconta.Text += button5.Text;
            }
            if (valorgeral == "rg")
            {
                ttbrg.Text += button5.Text;
            }
            if (valorgeral == "banco")
            {
                ttbbanco.Text += button5.Text;
            }
            if (valorgeral == "cheque")
            {
                ttbcheque.Text += button5.Text;
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (valorgeral == "agencia")
            {
                ttbagencia.Text += button4.Text;
            }
            if (valorgeral == "cpf")
            {
                ttbcpf.Text += button4.Text;
            }
            if (valorgeral == "conta")
            {
                ttbconta.Text += button4.Text;
            }
            if (valorgeral == "rg")
            {
                ttbrg.Text += button4.Text;
            }
            if (valorgeral == "banco")
            {
                ttbbanco.Text += button4.Text;
            }
            if (valorgeral == "cheque")
            {
                ttbcheque.Text += button4.Text;
            }
        }

        private void button10_Click(object sender, EventArgs e)
        {
            if (valorgeral == "agencia")
            {
                ttbagencia.Text += button10.Text;
            }
            if (valorgeral == "cpf")
            {
                ttbcpf.Text += button10.Text;
            }
            if (valorgeral == "conta")
            {
                ttbconta.Text += button10.Text;
            }
            if (valorgeral == "rg")
            {
                ttbrg.Text += button10.Text;
            }
            if (valorgeral == "banco")
            {
                ttbbanco.Text += button10.Text;
            }
            if (valorgeral == "cheque")
            {
                ttbcheque.Text += button10.Text;
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            if (valorgeral == "agencia")
            {
                ttbagencia.Text += button8.Text;
            }
            if (valorgeral == "cpf")
            {
                ttbcpf.Text += button8.Text;
            }
            if (valorgeral == "conta")
            {
                ttbconta.Text += button8.Text;
            }
            if (valorgeral == "rg")
            {
                ttbrg.Text += button8.Text;
            }
            if (valorgeral == "banco")
            {
                ttbbanco.Text += button8.Text;
            }
            if (valorgeral == "cheque")
            {
                ttbcheque.Text += button8.Text;
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            if (valorgeral == "agencia")
            {
                ttbagencia.Text += button7.Text;
            }
            if (valorgeral == "cpf")
            {
                ttbcpf.Text += button7.Text;
            }
            if (valorgeral == "conta")
            {
                ttbconta.Text += button7.Text;
            }
            if (valorgeral == "rg")
            {
                ttbrg.Text += button7.Text;
            }
            if (valorgeral == "banco")
            {
                ttbbanco.Text += button7.Text;
            }
            if (valorgeral == "cheque")
            {
                ttbcheque.Text += button7.Text;
            }
        }

        private void button13_Click(object sender, EventArgs e)
        {
            if (valorgeral == "agencia")
            {
                ttbagencia.Text += button13.Text;
            }
            if (valorgeral == "cpf")
            {
                ttbcpf.Text += button13.Text;
            }
            if (valorgeral == "conta")
            {
                ttbconta.Text += button13.Text;
            }
            if (valorgeral == "rg")
            {
                ttbrg.Text += button13.Text;
            }
            if (valorgeral == "banco")
            {
                ttbbanco.Text += button13.Text;
            }
            if (valorgeral == "cheque")
            {
                ttbcheque.Text += button13.Text;
            }
        }

        private void button12_Click(object sender, EventArgs e)
        {
            if (valorgeral == "agencia")
            {
                ttbagencia.Text += button12.Text;
            }
            if (valorgeral == "cpf")
            {
                ttbcpf.Text += button12.Text;
            }
            if (valorgeral == "conta")
            {
                ttbconta.Text += button12.Text;
            }
            if (valorgeral == "rg")
            {
                ttbrg.Text += button12.Text;
            }
            if (valorgeral == "banco")
            {
                ttbbanco.Text += button12.Text;
            }
            if (valorgeral == "cheque")
            {
                ttbcheque.Text += button12.Text;
            }
        }

        private void button11_Click(object sender, EventArgs e)
        {
            if (ttbvalor.Text != "")
            {
                ttbvalor.Text = ttbvalor.Text.Remove(ttbvalor.TextLength - 1);
            }
            if (valorgeral == "agencia")
            {
                ttbagencia.Text = ttbagencia.Text.Remove(ttbagencia.TextLength - 1);
            }
            if (valorgeral == "cpf")
            {
                ttbcpf.Text = ttbcpf.Text.Remove(ttbcpf.TextLength - 1); 
            }
            if (valorgeral == "conta")
            {
               // ttbconta.Text += button12.Text;
                ttbconta.Text = ttbconta.Text.Remove(ttbconta.TextLength - 1);
            }
            if (valorgeral == "rg")
            {
               // ttbrg.Text += button12.Text;
                ttbrg.Text = ttbrg.Text.Remove(ttbrg.TextLength - 1);
            }
            if (valorgeral == "banco")
            {
               // ttbbanco.Text += button12.Text;
                ttbbanco.Text = ttbbanco.Text.Remove(ttbbanco.TextLength - 1);
            }
            if (valorgeral == "cheque")
            {
               // ttbcheque.Text += button12.Text;
                ttbcheque.Text = ttbcheque.Text.Remove(ttbcheque.TextLength - 1);
            }
        }
    }
}
