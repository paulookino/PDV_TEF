using MGMPDV.Classes;
using System;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace MGMPDV.Formularios
{
    public partial class FAbrirCaixa : Form
    {
        int X = 0;
        int Y = 0;
        int NumeroLote = 0;

        CIniFile Ini = new CIniFile();
        string CaminhoDadosRep = "";
        string CaminhoDadosResp = "";
        public string ArqAbertura = "";
        public string CaixaAberto = "";

        string caixanum = "";
        int idfuncionario;
        DataTable dtcaixa = new DataTable();
        bool cai_status = false;
        DataTable dtfuncionario = new DataTable();
        public int numerocaixa { get; set; }
        public int idcaixa {get; set;}
        public bool ok { get; set; }


        public FAbrirCaixa()
        {
            Ini.IniFile("checkout");
            CaminhoDadosRep = Ini.IniReadString("dados", "Caminhorep", "");
            CaminhoDadosResp = Ini.IniReadString("dados", "Caminhoresp", "");
            
        }

        public void GerarAberturaArquivo(string destino, string nomearq, string uad, string lot, string ope, string ger, string maq,
string dat, string hor, string vap)
        {
            try
            {
                ArqAbertura = "A|" + lot + "|" + uad + "|" + ope + "|" + ger + "|" + maq + "|" + dat + "|" + hor + "|" + vap + "| <b />" +
                    "C| 950102 | 2017 - 01 - 01 | 13:55:02 | 900003 | 2017 - 01 - 01 || 00000 |";
                          File.WriteAllText(destino + nomearq, ArqAbertura, Encoding.UTF8);
            }
            catch (Exception ex)
            {

            };
        }

        public FAbrirCaixa(int idfuncionario)
        {
            InitializeComponent();
            Ini.IniFile("checkout");
            caixanum = Ini.IniReadString("Caixa", "Numero", "");

            lbltitulo.MouseDown += new MouseEventHandler(Form3_MouseDown);
            lbltitulo.MouseMove += new MouseEventHandler(Form3_MouseMove);
            pnltitulo.BackColor = Color.FromArgb(50, Color.Black);
            pnlbot.BackColor = Color.FromArgb(50, Color.Black);


            this.idfuncionario = idfuncionario;
            ok = false;
            idcaixa = 0;
            numerocaixa = 0;
            CFuncionario c = new CFuncionario();
            dtfuncionario = c.pesquisarID(idfuncionario);
            try {
                ttbusuario.Text = dtfuncionario.Rows[0]["fun_usuario"].ToString();
            }
            catch { }
            carregarcaixa();
            selecionarCaixa();
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            // Call the base class
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void Form3_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left) return;
            X = this.Left - MousePosition.X;
            Y = this.Top - MousePosition.Y;
        }

        private void Form3_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left) return;
            this.Left = X + MousePosition.X;
            this.Top = Y + MousePosition.Y;
        }
        private void btnfechar_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void carregarcaixa()
        {

            ttbcaixanumero.Text = caixanum;// caixastring;
            int numerocaixa = 0;
            int.TryParse(ttbcaixanumero.Text, out numerocaixa);
            this.numerocaixa = numerocaixa;
            CCaixa Caixa = new CCaixa();
            CaixaAberto = Caixa.cai_id.ToString();
            
        }

        private void gridcaixa_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {

                selecionarCaixa();

            }
            catch { }
        }


        private void timer1_Tick(object sender, EventArgs e)
        {
            if (lblstatus.Visible)
                ttbvalor.ReadOnly = true;
            else
                ttbvalor.ReadOnly = false;
            lblhora.Text = DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString();
        }

        private void selecionarCaixa()
        {

            if (numerocaixa == 0)
                return;


            CCaixa caixa = new CCaixa();
            DataTable dt = caixa.pegaultimo(numerocaixa);
            try
            {
                cai_status = (dt.Rows[0]["cai_status"].ToString() == "1");
            }
            catch { cai_status = false; }

            decimal valor = 0;
            int cai_id = 0;
            if (cai_status)
            {
                lblstatus.Visible = true;
                try
                {
                    int.TryParse(dt.Rows[0]["cai_id"].ToString(), out cai_id);
                    this.idcaixa = cai_id;
                    valor = caixa.totalCaixa(cai_id);
                }
                catch { }
            }
            else
                lblstatus.Visible = false;

            ttbvalor.Text = valor.ToString("00.00");

            string Hora = DateTime.Now.ToShortTimeString()+":00";
            string Dia = DateTime.Now.ToShortDateString();
            Dia = Dia.Substring(6, 4) + "-" + Dia.Substring(3, 2) + "-" + Dia.Substring(0, 2);
            FEntrar TelaEntrar = new FEntrar();
            if (!File.Exists(TelaEntrar.CaminhoDadosRep + "Abertura.ven"))
            {
                GerarAberturaArquivo(TelaEntrar.CaminhoDadosRep, "Abertura.ven", "8", CaixaAberto, TelaEntrar.idusuario.ToString(), TelaEntrar.idusuario.ToString(), numerocaixa.ToString(), Dia, Hora, "1.0");
            }
            ttbvalor.Focus();
        }

        private void btncancelar_Click(object sender, EventArgs e)
        {

            Close();
        }

        private void btnsair_Click(object sender, EventArgs e)
        {
            FMessageSimNao f = new FMessageSimNao();
            FMessageOk fm = new FMessageOk();
            
            if (cai_status)
            {
              
                if(f.Mostrar("Caixa Aberto!","O Caixa está aberto, deseja fechar?"))
                {
                    FFecharCaixa1 fecharcaixa = new FFecharCaixa1(idfuncionario,idcaixa);
                    fecharcaixa.ShowDialog();
                }
                carregarcaixa();
                selecionarCaixa();

            }


            MGM mgm = new MGM();
            decimal valor = 0;
            if(!mgm.isDecimal(ttbvalor.Text, false, out valor))
            {
                fm.Mostrar("Valor inválido!");
                return;
            }
            f = new FMessageSimNao();
            //if(valor>0)
            //{
            if (cai_status)
            {
               
                if (f.Mostrar("Abrir Caixa", "Deseja reabrir o caixa com o valor R$: " + ttbvalor.Text + " ? " ))
                {

                    if (idcaixa > 0)
                        ok = true;
                    else
                        fm.Mostrar("Falha ao abrir caixa!");

                    Close();
                }
            }
            else
                if (f.Mostrar("Abrir Caixa", "Deseja abrir o caixa com o valor R$: " + ttbvalor.Text + "?"))
                {
                    CCaixa c = new CCaixa();
                
                    idcaixa = c.abrir(numerocaixa, idfuncionario, valor, DateTime.Now.Date, DateTime.Now);
                    if (idcaixa > 0)
                        ok = true;
                    else
                        fm.Mostrar("Falha ao abrir caixa!");

                    Close();
                }
                      

        }



        private void FAbrirCaixa_KeyUp(object sender, KeyEventArgs e)
        {


            if (e.KeyCode == Keys.Escape)
                btncancelar.PerformClick();

        }


        private void ttbvalor_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
               
                 btnsair.PerformClick();

            }
        }
    }
}
