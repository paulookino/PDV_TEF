using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using MGMPDV.Formularios;
using System.IO;
using MGMPDV.Classes;

namespace MGMPDV
{
    public partial class FEntrar : Form
    {
        public bool ok { get; set; }
        public int idusuario { get; set; }
        public string usuario { get; set; }
        public string nivel { get; set; }
        public int totalDias { get; set; }

        CIniFile Ini = new CIniFile();
        public string CaminhoDadosRep = "";
        public string CaminhoDadosResp = "";

        public void CriarPasta(string diretorio, string arquivo, string conteudo)
        {
            DirectoryInfo dic = new DirectoryInfo(@diretorio);
            dic.Create();
            StreamWriter sw = new StreamWriter(diretorio + "\\" + arquivo);
            sw.WriteLine(conteudo);
            sw.Close();
        }


        public FEntrar()
        {
            InitializeComponent();

            Ini.IniFile("checkout");
            CaminhoDadosRep = Ini.IniReadString("dados", "Caminhorep", "");
            CaminhoDadosResp = Ini.IniReadString("dados", "Caminhoresp", "");

            ok = false;

            CConfiguracao c = new CConfiguracao();
            // c.carregar();
            TimeSpan date = c.retornaData() - DateTime.Now.Date;
            // totalDias = 0;
            //if (DateTime.Now.Date < c.retornaData())
            //     totalDias = date.Days;               
            //if (totalDias < 6)
            //    lbllicenca.ForeColor = Color.Red;
            totalDias = 180;
            lbllicenca.Text = "Licença válida por " + totalDias.ToString("00") + " dias";
            CriarPasta(CaminhoDadosRep, "pasta.txt", "Informações");
         
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Enter)
            {

                btnentrar.PerformClick();
                return true;    // indicate that you handled this keystroke
            }
            if (keyData == Keys.Escape)
            {

                btnsair.PerformClick();
                return true;    // indicate that you handled this keystroke
            }
            // Call the base class
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void btnsair_Click(object sender, EventArgs e)
        {
            ok = false;
            Application.Exit();
        }

        private void btnentrar_Click(object sender, EventArgs e)
        {
            totalDias = 100;
            FMessageOk f = new FMessageOk();
            if (totalDias<=0)
            {
                
                f.Mostrar("Licença Expirada!");
                return;
            }
            CFuncionario cf = new CFuncionario();

            DataTable dt = cf.pesquisar("", "nome");
            if (dt.Rows.Count <= 0)
            {
                ok = true;
                f.Mostrar("Cadastre um Funcionário");
                nivel = "";
                Close();
            }
            else
            {
                dt = cf.entrar(ttbusuario.Text, ttbsenha.Text);
                if (dt.Rows.Count > 0)
                {
                    ok = true;
                    idusuario = int.Parse(dt.Rows[0]["fun_id"].ToString());
                    usuario = dt.Rows[0]["fun_usuario"].ToString();
                    nivel = dt.Rows[0]["fun_nivel"].ToString();
                    Close();
                }
                else
                    f.Mostrar("Usuário ou Senha inválido!");

            }
            
        }

    }
}
