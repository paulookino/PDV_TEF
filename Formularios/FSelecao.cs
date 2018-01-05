using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MGMPDV.Formularios
{
    public partial class FSelecao : Form
    {
        public FSelecao(string menuSelec)
        {
            InitializeComponent();

            string[] m1 = menuSelec.TrimEnd(';').Split(';');

            for (int i = 0; i < m1.Length; i++)
            {
                string[] m2 = m1[i].Split(':');
                menu.Add(int.Parse(m2[0]), m2[1]);

                listBox1.Items.Add(menu.ElementAt(menu.Count - 1).Value);
            }
        }
        Dictionary<int, string> menu = new Dictionary<int, string>();
        public string result;

        private void button1_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex > -1)
            {
                result = menu.Keys.ElementAt(listBox1.SelectedIndex).ToString();
                this.Dispose();
            }
            else
            {
                MessageBox.Show("Selecione uma opção");
            }
        }

        private void frmAuxSelecao_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                button1_Click(sender, e);
            }
        }

        private void button1_Click_1(object sender, EventArgs e)
        {

        }

        private void btnconfirmar_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex > -1)
            {
                result = menu.Keys.ElementAt(listBox1.SelectedIndex).ToString();
                this.Dispose();
            }
            else
            {
                MessageBox.Show("Selecione uma opção");
            }
        }

        private void label4_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex > -1)
            {
                result = menu.Keys.ElementAt(listBox1.SelectedIndex).ToString();
                this.Dispose();
            }
            else
            {
                MessageBox.Show("Selecione uma opção");
            }
        }

        private void btnvoltar_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
