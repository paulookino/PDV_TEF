using ACBrFramework.ECF;
using ACBrFramework.LCB;
using MGMPDV.Classes;
using System;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Media;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Speech.Synthesis;
using WMPLib;
using System.Drawing.Printing;
using System.Collections.Generic;
using System.Threading;
using System.Xml;
using System.Collections;
using System.Text;
using MGMPDV.TEF;

namespace MGMPDV.Formularios
{
    public partial class FPDV : Form
    {
        SpeechSynthesizer reader;

        int X = 0;
        int Y = 0;
        public bool login { get; set; }
        bool ttbvalorfoco = false;
        int ItemCupom = 1;
        int LinhaCupom = 6;

        WindowsMediaPlayer sonido;

        bool mover = false;
        string componente = "";

        // Variaveis INI
        CIniFile Ini = new CIniFile();
        string Versao = "1.0019";
        string PortaBalanca = "0";
        string PortaImpressora = "0";
        string Mensagem1 = string.Empty;
        string Mensagem2 = string.Empty;
        string Mensagem3 = string.Empty;
        string Mensagem4 = string.Empty;
        string Mensagem5 = string.Empty;
        string Mensagem6 = string.Empty;
        string Mensagem7 = string.Empty;
        string produto1 = string.Empty;
        string msgsat1 = string.Empty;
        string msgsat2 = string.Empty;
        string satmon = string.Empty;
        string LimitarItens = string.Empty;
        string LimitarNumItens = string.Empty;
        string HabilitaSenha = string.Empty;
        string CupomAtual = string.Empty;
        string Caminhoxmlenv = string.Empty;
        string Caminhoxmlret = string.Empty;
        public string CaminhoDadosRep = string.Empty;
        public string CaminhoDadosResp = string.Empty;
        public string NomeArquivo = string.Empty;
        public string LinhaC = string.Empty;
        public string LinhaD = string.Empty;
        public string LinhaE = string.Empty;
        public string LinhaF = string.Empty;
        public string LinhaG = string.Empty;
        string PortaTorre = "Com1";
        string UtilizarTorre = "nao";
        string UtilizarImpre = "nao";
        string UtilizarBalan = "nao";
        string UtilizarLeitor = "nao";
        string IPTef = "";
        string LojaTef = "";
        string TerminalTef = "";
        string TEFDLL = "nao";
        public string SomenteCartao = "sim";
        string PortaLeitor = "COM1";
        string tipopag = "01";
        public string Caminhoreq = string.Empty;
        public string Caminhoresp = string.Empty;
        public string ItemCupom1 = string.Empty;
        public string ItemCupom2 = string.Empty;

        private CProduto produto;
        private CProduto cproduto = new CProduto();
        private CCliente cliente = new CCliente();
        private CVenda venda = new CVenda();
        private CEmpresa empresa = new CEmpresa();
        private DataTable dtitemvenda = new DataTable();
        private DataTable dtitemcupom = new DataTable();
        private CConfiguracao configuracao = new CConfiguracao();
        ACBrLCB ACBrBarra = new ACBrLCB();
        List<Pagamento> listaPagamento;
        DataTable fonteDados;

        Label l = new Label();
        int idfuncionario = 0;
        string nomefuncionario = "";
        string codigoDeRetorno = "0";
        string numeroSessao = "0";
        string xmlpuro = "";
        int acesoVermelho = 0;
        int ContaItem = 1;

        string status = string.Empty;
        string FormPag = string.Empty;
        decimal peso = 0;
        decimal pesototal = 0;
        decimal produtonovo = 0;
        string media = string.Empty;
        string DadosReducaoStr = string.Empty;
        int itemdecupom = 0;
        string numerocupom = "0";
        string INTPOS = "";
        string parcelas = "";
        string nivel = "";
        string CPFSAT = "";
        string telefone, ddd;
        bool okb;
        decimal desconto = 0;
        int idcaixa = 0;
        int caixanumero = 0;
        decimal pagamentos = 0;
        int ultimoitem = 0;
        int FlagSudacao = 0;
        int NI = 0;
        string Parcelas = "";

        #region Cadastrar Arquivos Siad

        public void GravaArquivoVen(string TextoLinha)
        {
            StreamReader rdr = null;
            NomeArquivo = "Arquivo.temp";
            NomeArquivo = CaminhoDadosRep + NomeArquivo;

            if (!File.Exists(NomeArquivo))
            {
                using (StreamWriter sw = File.CreateText(NomeArquivo)) { }
            }

            int numeroLinha = System.IO.File.ReadAllLines(NomeArquivo).Length;

            string textoInserir = TextoLinha;
            numeroLinha++;

            ArrayList linhas = new ArrayList();



            if (File.Exists(NomeArquivo))
            {
                try
                {
                    rdr = new StreamReader(NomeArquivo);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erro ao acessar o arquivo : " + ex.Message);
                    return;
                }
            }
            else
            {
                MessageBox.Show("O arquivo : " + NomeArquivo + " não existe...");
                return;
            }

            string linha;

            while ((linha = rdr.ReadLine()) != null)
            {
                try
                {
                    linhas.Add(linha);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erro ao acessar o arquivo : " + ex.Message);
                    return;
                }
            }
            rdr.Close();

            if (linhas.Count > numeroLinha)
                linhas.Insert(numeroLinha, textoInserir);
            else
                linhas.Add(textoInserir);

            StreamWriter wrtr = new StreamWriter(NomeArquivo);

            foreach (string strNewLine in linhas)
            {
                wrtr.WriteLine(strNewLine);
            }
            wrtr.Close();
        }
        public void ApagaLinhaArquivoVen(string nomeArquivo, string TextoLinha)
        {
            string tempFile = Path.GetTempFileName();

            using (var sr = new StreamReader(nomeArquivo))
            {
                using (var sw = new StreamWriter(tempFile))
                {
                    string line;

                    while ((line = sr.ReadLine()) != null)
                    {
                        if (line != TextoLinha)
                            sw.WriteLine(line);
                    }
                }
            }

            File.Delete(nomeArquivo);
            File.Move(tempFile, nomeArquivo);
        }

        public void GerarCabecaArquivo(string destino, string nomearq, string uad, string lot, string ope, string ger, string maq,
string dat, string hor, string cpm, string vap, string ctr, string cts)
        {
            string a = "A|" + lot + "|" + uad + "|" + ope + "|" + ger + "|" + maq + "|" + dat + "|" + hor + "|" + vap + "|";
            GravaArquivoVen(a);

            string b = "C|" + cpm + "|" + dat + "|" + hor + "||" + dat + "|" + ctr + "|" + cts + "|";
            GravaArquivoVen(b);
        }

        public void GerarRodapeArquivo(string destino, string nomearq, string tpg, string vlt, string vsq,
            string frc, string vtr, string cac, string vlr, string cav, string dpg, string nsh, string nsu, string aut, string vtp,
            string ntp, string ope)
        {
            string h = "F|" + tpg + "|" + vlt + "|" + vsq + "|" + frc + "|" + vtr + "|";
            GravaArquivoVen(h);

            string i = "F04|" + cac + "|" + vlr + "|" + cav + "|" + dpg + "|" + aut + "|" + nsh + "|" + nsu + "|" + vtp + "|" + ntp + "|" + ope + "| ";
            GravaArquivoVen(i);
        }

        public void GerarItemArquivo(string destino, string nomearq, string seq, string prd, string qtd, string cmp,
string vlr, string ean, string vtr, string crm, string dtr, string tcs, string ufc, string cls, string tal, string ctr,
string bal, string pbo, string tbo, string ucu, string cum, string tpg, string vlt, string vsq, string frc, string cac, string cav, string dpg, string aut, string nsh, string nsu)
        {
            string c = "I|" + seq + "|";
            LinhaC = c;
            GravaArquivoVen(c);

            string d = "I01|" + prd + "|" + qtd + "|" + vlr + "|" + ean + "|" + vtr + "|";
            LinhaD = d;
            GravaArquivoVen(d);

            //string e = "I03|" + crm + "|" + dtr + "|" + tcs + "|" + ufc + "|" + cls + "|" + ctr + "|" + tal + "|";
            //LinhaE = e;
            //GravaArquivoVen(e);

            //string f = "I04|" + bal + "|" + pbo + "|" + tbo + "|";
            //LinhaF = f;
            //GravaArquivoVen(f);

            string g = "I06|" + ucu + "|" + cum + "|";
            LinhaG = g;
            GravaArquivoVen(g);
        }

        public void GerarFechamentoParcial(string destino, string nomearq, string uad, string lot, string ope, string ger, string maq,
string dat, string hor, string cpm, string vap, string ctr, string cts, string qcp, string dgt, string fpg, string qtd, string vlr, string vdg, string tpg)
        {
            string a = "A|" + lot + "|" + uad + "|" + ope + "|" + ger + "|" + maq + "|" + dat + "|" + hor + "|" + vap + "|";
            GravaArquivoVen(a);

            string c = "C|" + cpm + "|" + dat + "|" + hor + "||" + dat + "|" + ctr + "|" + cts + "|";
            GravaArquivoVen(c);

            string x = "X|" + ope + "|" + dat + "|" + hor + "||" + qcp + "|" + dgt + "|";
            GravaArquivoVen(x);

            string x03 = "X03|" + fpg + "|" + qtd + "|" + vlr + "|" + vdg + "|";
            GravaArquivoVen(x03);

            string x04 = "X04|" + tpg + "|" + qtd + "|" + vlr + "|" + vdg + "|";
            GravaArquivoVen(x04);

        }

        public void GerarFechamentoDiario(string destino, string nomearq, string uad, string lot, string ope, string ger, string maq,
        string dat, string hor, string cpm, string vap, string ctr, string cts, string fpg, string qtd, string vlr, string vdg, string trb,
        string alq, string bic, string ttp, string nrz, string ico, string fco, string vgt, string vmv, string vds, string dsb,
         string vcn, string dmv, string seq, string cro, string mfd, string tip, string mar, string mod, string vsb, string hsb, string nse, string nou, string nef, string tpg)
        {
            string a = "A|" + lot + "|" + uad + "|" + ope + "|" + ger + "|" + maq + "|" + dat + "|" + hor + "|" + vap + "|";
            GravaArquivoVen(a);

            string c = "C|" + cpm + "|" + dat + "|" + hor + "||" + dat + "|" + ctr + "|" + cts + "|";
            GravaArquivoVen(c);

            string z01 = "Z01|" + fpg + "|" + qtd + "|" + vlr + "|" + vdg + "|";
            GravaArquivoVen(z01);

            string z02 = "Z02|" + trb + "|" + alq + "|" + bic + "|" + ttp + "|";
            GravaArquivoVen(z02);

            string z03 = "Z03|" + nrz + "|" + ico + "|" + fco + "|" + vgt + "|" + vmv + "|" + vds + "|" + vcn + "|" + dmv + "|" + seq + "|" + "|" + cro + "|" + "|" + mfd + "|" + "|" + tip + "|" + mar + "|" + mod + "|" + vsb + "|" + dsb + "|" + hsb + "|" + nse + "|" + nou + "|" + nef + "|";
            GravaArquivoVen(z03);

            string z04 = "Z04|" + tpg + "|" + qtd + "|" + vlr + "|" + vdg + "|";
            GravaArquivoVen(z04);
        }
        #endregion

        public void ImprimeCabecalhoBobina()
        {
            try
            {
                Bobina2.Items.Add(new string('-', 48));
                Bobina2.Items.Add("               **     CUPOM  FISCAL   **               ");
                Bobina2.Items.Add(new string('-', 48));
                Bobina2.Items.Add("ITEM Codigo         Descricao                   ");
                Bobina2.Items.Add("QTD.     UN      VL.UNIT.(R$)       VL.ITEM(R$)");
                Bobina2.Items.Add(new string('-', 48));
            }
            catch (Exception eError)
            {
                //Log.write(eError.ToString());
            }
        }

        public void CancelaVendaAtual()
        {
            CItemVenda citemvenda = new CItemVenda();
            int iditem = 0;
            decimal quantidade = 0;
            string descricao = "";
            decimal valorproduto = 0;
            int idproduto = 0;
            CProduto cproduto = new CProduto();
            CCupom cupom = new CCupom();
            ContaItem = 0;
            ultimoitem = gridproduto.Rows.Count;
            while (ultimoitem > 0)
            {
                try
                {
                    int numeroinput = ultimoitem;// int.Parse(f.valor);
                    if (numeroinput > 0)
                    {
                        iditem = int.Parse(gridproduto.Rows[numeroinput - 1].Cells["ite_id"].Value.ToString());
                        quantidade = decimal.Parse(gridproduto.Rows[numeroinput - 1].Cells["ite_quantidade"].Value.ToString());
                        valorproduto = decimal.Parse(gridproduto.Rows[numeroinput - 1].Cells["ite_valor"].Value.ToString());
                        iditem = int.Parse(gridproduto.Rows[numeroinput - 1].Cells["ite_id"].Value.ToString());
                        idproduto = int.Parse(gridproduto.Rows[numeroinput - 1].Cells["pro_id"].Value.ToString());
                        descricao = gridproduto.Rows[numeroinput - 1].Cells["pro_nome"].Value.ToString();

                        citemvenda.remover(iditem);
                        cupom.remover(itemdecupom);
                        venda.adicionarvalor(venda.ven_id, ((quantidade * valorproduto)) * -1);

                        //cproduto.baixarestoque(idproduto, quantidade * -1);
                        cproduto.RetornarEstoque(idproduto, quantidade);
                        label16.Text = descricao;
                        ttbproduto.Focus();
                        ultimoitem--;
                    }
                    else
                    {
                        fmok.Mostrar("NÃO HA ITENS A SEREM EXCLUIDOS!!");
                    }
                }
                catch { fmok.Mostrar("ERRO AO REMOVER PRODUTO!!"); };
            }
            cupom.removerTodos();
            carregar();


            Bobina2.Items.Clear();
            //  ImprimeCabecalhoBobina();

            label16.Text = "Passe o produto pelo Leitor";
        }

        public void CriarPasta(string diretorio, string arquivo, string conteudo)
        {
            DirectoryInfo dic = new DirectoryInfo(@diretorio);
            dic.Create();
            StreamWriter sw = new StreamWriter(diretorio + "\\" + arquivo);
            sw.WriteLine(conteudo);
            sw.Close();
        }

        public string RetornaLinhaIntPos(string CaminhoArquivo)
        {
            string Saida = "";
            string f = CaminhoArquivo;

            List<string> myList = new List<string>();

            using (StreamReader r = new StreamReader(f))
            {
                string line;
                while ((line = r.ReadLine()) != null)
                {
                    myList.Add(line);
                }
            }

            var listString = new StringBuilder();
            foreach (string s in myList)
            {
                if (s.Substring(0, 3) == "029")
                {
                    listString.Append(Environment.NewLine);

                    listString.Append(s);
                }

            }
            Saida = listString.ToString();
            return Saida;
        }
        public string RetornaChaveIntPos(string CaminhoArquivo, string Chave)
        {
            string Saida = "";
            string f = CaminhoArquivo;

            List<string> myList = new List<string>();

            using (StreamReader r = new StreamReader(f))
            {
                string line;
                while ((line = r.ReadLine()) != null)
                {
                    myList.Add(line);
                }
            }

            var listString = new StringBuilder();
            foreach (string s in myList)
            {
                if (s.Substring(0, 7) == Chave)
                {
                    listString.Append(Environment.NewLine);

                    listString.Append(s);
                }

            }
            Saida = listString.ToString();
            return Saida;
        }

        public bool ImprimeArqIntpos(string caminho)
        {
            EscPos esc = new EscPos();
            string valor = string.Empty;
            int counter = 0;
            string line;
            bool aceito = false;
            try
            {
                List<string> Plines = new List<string>();
                string texto = "";
                textBox1.Lines = Plines.ToArray();
                System.IO.StreamReader file = new System.IO.StreamReader(caminho);
                while ((line = file.ReadLine()) != null)
                {
                    System.Console.WriteLine(line);

                    int tamanho = line.Length;
                    tamanho = tamanho - 10;
                    if (line.Substring(0, 3) == "029")
                    {

                        texto = line.Substring(11, tamanho - 1).ToString();
                        texto = texto.Replace("'", "");
                        Plines.Add(texto.Trim().Replace("'", ""));

                    }
                    counter++;
                }
                file.Close();
                textBoxCupom.Text.Insert(textBoxCupom.SelectionStart, texto);
                textBoxCupom.Lines = Plines.ToArray();
                string s = textBoxCupom.Text;

                // Allow the user to select a printer.
                PrintDialog pd = new PrintDialog();
                s = s.Replace("\"", "");
                pd.PrinterSettings = new PrinterSettings();
                RawPrinterHelper.SendStringToPrinter(pd.PrinterSettings.PrinterName, s);

                esc.printText(pd.PrinterSettings.PrinterName, s);

                feedAndCutter(pd.PrinterSettings.PrinterName, 5);

                FileStream fileStream = new FileStream(caminho, FileMode.Open);
                fileStream.Seek(0, SeekOrigin.Begin);

                aceito = true;
            }
            catch (Exception ex)
            {
                aceito = false;
            }
            return aceito;
        }

        public class ExButton : Button
        {
            public ExButton()
            {
                SetStyle(ControlStyles.Selectable, false);
            }
        }

        public void falar(string texto)
        {
            textBox1.Text = texto;
            //     reader.Dispose();
            if (textBox1.Text != "")
            {

                reader = new SpeechSynthesizer();
                reader.SpeakAsync(textBox1.Text);
                label2.Text = "FALANDO";
                button2.Enabled = true;
                button4.Enabled = true;
                reader.SpeakCompleted += new EventHandler<SpeakCompletedEventArgs>(reader_SpeakCompleted);
            }
            else
            {
                MessageBox.Show("Digite alguma frase", "Message", MessageBoxButtons.OK);
            }
        }

        void reader_SpeakCompleted(object sender, SpeakCompletedEventArgs e)
        {
            label2.Text = "IDLE";
        }
        public void LerBalanca(string porta)
        {
            serialPort.PortName = porta;
            serialPort.Open();
            while (true)
            {
                string valor = serialPort.ReadLine();//ST,GS,+000.000kg\r
                valor = valor.Substring(7, 7);
                peso = Convert.ToDecimal(valor);
                serialPort.Close();
                return;
            }
        }
        private void timer2_Tick(object sender, EventArgs e)
        {
            serialPort.PortName = PortaBalanca;
            try
            {
                serialPort.Open();
                while (true)
                {
                    string valor = serialPort.ReadLine();//ST,GS,+000.000kg\r
                    try
                    {
                        valor = valor.Substring(7, 7);
                        peso = Convert.ToDecimal(valor);
                        lblpeso.Text = peso.ToString();
                        serialPort.Close();
                        return;
                    }
                    catch { }

                }

            }
            catch (Exception ex)
            {

            }
        }
        private void timervalidasacola_Tick(object sender, EventArgs e)
        {
        }

        public void ativarBalanca()
        {
            serialPort.PortName = PortaBalanca;
            try
            {
                serialPort.Open();
                while (true)
                {
                    string valor = serialPort.ReadLine();//ST,GS,+000.000kg\r
                    try
                    {
                        valor = valor.Substring(7, 7);
                        peso = Convert.ToDecimal(valor);
                        lblpeso.Text = peso.ToString();
                        serialPort.Close();
                        return;
                    }
                    catch { }
                }
            }
            catch (Exception ex)
            {
                falar("BALANÇA NÃO CONECTADA OU PORTA DE COMUNICAÇÃO INEXISTENTE, A APLICAÇÃO SERÁ FINALIZADA !");
                fmok.Mostrar("BALANÇA NÃO CONECTADA OU PORTA DE COMUNICAÇÃO INEXISTENTE !");
                this.Close();
            }
        }
        public void ativarTorre()
        {
            serialPortTorre.PortName = PortaTorre;
            try
            {
                serialPortTorre.Open();
            }
            catch (Exception ex)
            {
                falar("Porta da Torre não encontrada.");
                fmok.Mostrar("Porta da Torre não encontrada: " + ex);
                return;
            }
        }
        private void ativarECF()
        {
            try
            {

                acBrECF1.Modelo = (ModeloECF)1;
                acBrECF1.Device.Porta = PortaImpressora;
                acBrECF1.Ativar();
            }
            catch (Exception ex)
            {
                falar("Erro ao Ativar impressora.");
                fmok.Mostrar("Ativando Impressora: " + ex.Message);
                return;
            }
        }
        private void DesativarECF()
        {
            try
            {

                acBrECF1.Modelo = (ModeloECF)1;
                acBrECF1.Desativar();
            }
            catch (Exception ex)
            {
                falar("Erro ao desativar impressora.");
                fmok.Mostrar("Desativando Impressora: " + ex.Message);
            }
        }
        public void ativarLeitor()
        {
            SerialLeitor.PortName = PortaLeitor;
            try
            {
                SerialLeitor.Open();
            }
            catch (Exception ex)
            {
                falar("Porta do leitor de codigo de barras não encontrada.");
                fmok.Mostrar("Porta do Leitor não encontrada: " + ex);
                return;
            }
        }

        public void saudacao()
        {
            if (DateTime.Now.Hour < 12)
            {
                falar(Mensagem1);
            }
            else if (DateTime.Now.Hour < 17)
            {
                falar(Mensagem2);
            }
            else
            {
                falar(Mensagem3);
            }

        }

        DataTable dtpesquisarproduto = new DataTable();
        FMessageOk fmok = new FMessageOk();
        private string lot;
        private string ope;

        public FPDV(int idfuncionario, int idcaixa, int numerocaixa)
        {
            InitializeComponent();

            CFuncionario fun = new CFuncionario();
            CCupom Cupom = new CCupom();
            int nc = Cupom.RetornaNumeroTotalDeCupons();
            if (nc > 0)
            {
                Cupom.removerTodos();
                CancelaVendaAtual();
            }


            DataTable dt = fun.pesquisarID(idfuncionario);
            this.idfuncionario = idfuncionario;
            try
            {
                nomefuncionario = dt.Rows[0]["fun_nome"].ToString();
                nivel = dt.Rows[0]["fun_nivel"].ToString();
                lblfuncionario.Text = nomefuncionario;
            }
            catch { }

            if (nivel == "ADMINISTRADOR")
            {
                BtnConfig.Visible = true;
                tileItem4.Visible = true;
            }

            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(1024, 768);
            this.KeyPreview = true;
            this.Name = "Form1";
            this.Text = "Form1";

            Ini.IniFile("checkout");
            PortaImpressora = Ini.IniReadString("Impressora", "Porta", "");
            PortaBalanca = Ini.IniReadString("balanca", "Porta", "");
            LimitarItens = Ini.IniReadString("NumItens", "Limitar", "");
            LimitarNumItens = Ini.IniReadString("NumItens", "TotalItens", "");
            SomenteCartao = Ini.IniReadString("SoCartao", "Habilitar", "");
            TEFDLL = Ini.IniReadString("TipoTef", "Dll", "");

            IPTef = Ini.IniReadString("ConfigTef", "IP", "");
            LojaTef = Ini.IniReadString("ConfigTef", "Loja", "");
            TerminalTef = Ini.IniReadString("ConfigTef", "Terminal", "");
            HabilitaSenha = Ini.IniReadString("SoCartao", "HabilitarSenha", "");
            Mensagem1 = Ini.IniReadString("voz", "mensagem1", "");
            Mensagem2 = Ini.IniReadString("voz", "mensagem2", "");
            Mensagem3 = Ini.IniReadString("voz", "mensagem3", "");
            Mensagem4 = Ini.IniReadString("voz", "mensagem4", "");
            Mensagem5 = Ini.IniReadString("voz", "mensagem5", "");
            Mensagem6 = Ini.IniReadString("voz", "mensagem6", "");
            Mensagem7 = Ini.IniReadString("voz", "mensagem7", "");
            produto1 = Ini.IniReadString("voz", "produto1", "");
            satmon = Ini.IniReadString("satmon", "Habilita", "");
            Caminhoxmlenv = Ini.IniReadString("xml", "Caminhoenv", "");
            Caminhoxmlret = Ini.IniReadString("xml", "Caminhoret", "");
            PortaTorre = Ini.IniReadString("torre", "Porta", "");
            UtilizarBalan = Ini.IniReadString("Utiliza", "Balanca", "");
            UtilizarImpre = Ini.IniReadString("Utiliza", "Impressora", "");
            UtilizarTorre = Ini.IniReadString("Utiliza", "Torre", "");
            UtilizarLeitor = Ini.IniReadString("Utiliza", "Leitor", "");
            PortaLeitor = Ini.IniReadString("leitorBarra", "Porta", "");
            Caminhoreq = Ini.IniReadString("tef", "Caminhoreq", "");
            Caminhoresp = Ini.IniReadString("tef", "Caminhoresp", "");
            CaminhoDadosRep = Ini.IniReadString("dados", "Caminhorep", "");
            CaminhoDadosResp = Ini.IniReadString("dados", "Caminhoresp", "");

            if (UtilizarTorre == "sim") ativarTorre();
            if (UtilizarImpre == "sim") ativarECF();
            if (UtilizarBalan == "sim") ativarBalanca();
            if (UtilizarLeitor == "sim") ativarLeitor();

            //verificar se balança

            status = acBrECF1.Estado.ToString();
            if (status == "Pagamento") acBrECF1.CancelaCupom();
            status = acBrECF1.Estado.ToString();
            if (status == "RequerZ") acBrECF1.ReducaoZ();
            status = acBrECF1.Estado.ToString();
            if (status == "estRequerX") acBrECF1.LeituraX();

            gridproduto.AutoGenerateColumns = false;
            gridpesquisarproduto.AutoGenerateColumns = false;
            pnltitulo.BackColor = Color.FromArgb(50, Color.Black);
            //   pnlttproduto.BackColor = Color.FromArgb(50, Color.Black);
            pnlprodutonome.BackColor = Color.FromArgb(50, Color.Black);
            pnlprodutoestoque.BackColor = Color.FromArgb(50, Color.Black);
            pnlinfproduto.BackColor = Color.FromArgb(50, Color.Black);
            //   pnlentrega.BackColor = Color.FromArgb(50, Color.Black);
            //   pnlentregatitulo.BackColor = Color.FromArgb(50, Color.Black);
            //   pnlentregainformacao.BackColor = Color.FromArgb(50, Color.Black);
            pnlvendadetalhe.BackColor = Color.FromArgb(50, Color.Black);
            pnlbot.BackColor = Color.FromArgb(50, Color.Black);
            pnlpesquisarproduto.Left = 13;
            pnlpesquisarproduto.Visible = true;
            pnlpesquisarproduto.BringToFront();

            //if (SomenteCartao == "sim")
            //{
            //    tileItem6.Visible = false;
            //}
            //else
            //{
            //    tileItem1.Visible = true;
            //    tileItem6.Visible = true;
            //}

            tileItem1.Visible = false;
            tileItem6.Visible = false;
            ImprimeCabecalhoBobina();

            login = false;

            limpar();

            empresa.pesquisar();

            try
            {
                CConfiguracao c = new CConfiguracao();

                this.idcaixa = idcaixa;
                caixanumero = numerocaixa;
                string textoini = "SAT.Inicializar";
                System.IO.File.WriteAllText(@"C:\ACBrMonitorPLUS\ENT.txt", textoini);

                //System.IO.File.WriteAllText(@"C:\ACBrMonitorPLUS\SAI.txt", "");
            }
            catch
            {
                CriarPasta(@"C:\ACBrMonitorPLUS", "ENT.txt", "Informações");
            }

            venda.pesquisarvendaaberta(idcaixa);
            carregar();

            //      mostrarimagemproduto("9999999999999");

            try
            {
                PainelMerchan PN = new PainelMerchan();
                Application.Run(PN);
            }
            catch (Exception ex)
            {

            }

            // saudacao();

        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Escape)
            {
                if (pnlpesquisarproduto.Visible)
                    pnlpesquisarproduto.Visible = false;
                else
                    if (ttbproduto.Focused)
                {
                    limpar();
                }

                return true;
            }

            if (keyData == Keys.F1)
            {
                if (pnlpesquisar.Visible)
                    btnproduto.PerformClick();
                else
                    btniniciarcupom.PerformClick();
                return true;
            }

            if (keyData == Keys.F2)
            {
                if (pnlpesquisar.Visible)
                    btncupomcancelado.PerformClick();
                else
                    btnpesquisarcliente.PerformClick();
                return true;
            }

            if (keyData == Keys.F3)
            {
                if (pnlpesquisar.Visible)
                    btnmovimentacao.PerformClick();
                else
                    //   btnfecharcupom.PerformClick();
                    return true;
            }

            if (keyData == Keys.F4)
            {
                if (pnlpesquisar.Visible)
                    btnrelatorio.PerformClick();
                else
                    btncancelarcupom.PerformClick();
                return true;
            }

            if (keyData == Keys.F5)
            {
                if (pnlpesquisar.Visible)
                    btnfuncionario.PerformClick();
                else
                    //   btnexcluiritem.PerformClick();
                    return true;
            }

            if (keyData == Keys.F6)
            {
                if (pnlpesquisar.Visible)
                    btnconfiguracoes.PerformClick();
                else
                    btnprevenda.PerformClick();
                return true;
            }

            if (keyData == Keys.F7)
            {
                btnfecharcaixa.PerformClick();
                return true;
            }
            if (keyData == Keys.F8)
            {
                btnsair.PerformClick();
                return true;
            }
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
        private void limpar()
        {
            produto = new CProduto();
            produto.pro_id = 0;
            ttbproduto.Clear();

            //limparcliente();
            ttbquantidade.Clear();
            ttbvalor.Clear();
            ttbtotalproduto.Clear();
            ttbproduto.Focus();
            lblprodutoestoque.Text = "";
            lblprodutonome.Text = "";
            pnlpesquisarproduto.Visible = false;
        }
        public void carregar()
        {
            carregarvenda(venda.ven_id);
            carregarcliente();
            carregaritemvenda();
            mostrar();
        }
        public void mostrar()
        {
            if (cliente.id > 0)
                mostrarcliente(cliente);
            else
                limparcliente();
            mostraritemvenda(dtitemvenda);

        }
        public void carregarvenda(int idvenda)
        {
            venda.pesquisarvendaIdVenda(idvenda);
            desconto = venda.ven_desconto;
            ttbsubtotal.Text = venda.ven_total.ToString("00.00");
            ttbdesconto.Text = venda.ven_desconto.ToString("00.00");
            ttbtotal.Text = (venda.ven_total - venda.ven_desconto).ToString("00.00");
            CContaReceber cc = new CContaReceber();
            pagamentos = cc.totalPacelaContaReceberVenda(venda.ven_id);
        }
        public void mostraritemvenda(DataTable dtitemvenda)
        {
            gridproduto.DataSource = dtitemvenda;

        }
        public void carregarcliente()
        {
            cliente.pesquisarID(venda.cli_id);
        }
        private void mostrarcliente(CCliente cliente)
        {
            if (cliente.id > 0)
            {
                //lblclientenome.Text = "";
                //lblclienteendereco.Text = "";
                //lblclientenumero.Text = "";
                //lblclientebairro.Text = "";
                //lblclientetelefone1.Text = "";
                //lblclientetelefone2.Text = "";
                //lblclientenome.Text = cliente.nome;
                //lblclienteendereco.Text = cliente.endereco;
                //lblclientebairro.Text = cliente.bairro;
                //lblclientenumero.Text = "N. " + cliente.numero + " " + cliente.bairro;
                //string telefone = "";
                //if (cliente.telefone1.Trim() != "")
                //    lblclientetelefone1.Text = "(" + cliente.ddd1 + ") " + cliente.telefone1;
                //if (cliente.telefone2.Trim() != "")
                //{
                //    lblclientetelefone2.Text = "(" + cliente.ddd2 + ")" + cliente.telefone2;
                //}

                venda.atualizarClienteFuncionario(venda.ven_id, cliente.id, idfuncionario);

                ttbproduto.Focus();


            }
            else
            {
                /* if (pnlcadastrarcliente.Visible)
                {
                    if (btncadastrarnao.Focused)
                        btncadastrarnao.PerformClick();
                    else
                        btncadastrarsim.PerformClick();
                }
                else
                {
                    pnlcadastrarcliente.Visible = true;
                    btncadastrarsim.Focus();
                }*/


            }
        }
        private void limparcliente()
        {
            //lblclientenome.Text = "";
            //lblclienteendereco.Text = "";
            //lblclientenumero.Text = "";
            //lblclientebairro.Text = "";
            //lblclientetelefone1.Text = "";
            //lblclientetelefone2.Text = "";
            //ckbcpf.Text = "CPF";
            ////ckbcpf.Checked = false;
            //ckbentrega.Checked = false;
            cliente = new CCliente();
            cliente.id = 0;
            //pnlcadastrarcliente.Visible = false;
        }
        public void carregaritemvenda()
        {
            CItemVenda c = new CItemVenda();
            dtitemvenda = c.pesquisar(venda.ven_id);
        }
        public void carregarcupom()
        {
            CCupom c = new CCupom();
            dtitemcupom = c.pesquisar(0);
        }

        private void btniniciarcupom_Click(object sender, EventArgs e)
        {
            // ativarECF();

            if (venda.ven_id > 0)
            {
                falar("cupom já iniciado.");
                fmok.Mostrar("O Cupom já foi iniciado!");
                return;
            }
            else
            {

            }
            venda = new CVenda();
            venda.inserir(cliente.id, idfuncionario, 0, "", idcaixa);
            carregar();
            ttbproduto.Focus();
        }

        private void ttbproduto_Enter(object sender, EventArgs e)
        {
            ttbproduto.Clear();
            limpar();
        }

        private void ttbproduto_Leave(object sender, EventArgs e)
        {
            int NumLinha = 0;

            if (ttbproduto.Text != "")
            {

                pnlpesquisarproduto.Visible = false;

                bool barras = false;
                try
                {
                    long.Parse(ttbproduto.Text);
                    barras = true;
                }
                catch { }


                string quantidade = "1";
                produto.pesquisarid(int.Parse(gridpesquisarproduto.CurrentRow.Cells[0].Value.ToString()));
                mostrarproduto();

                // GerarItemArquivo(string destino, string nomearq, string seq, string prd, string qtd, string cmp,
                // string vlr, string ean, string vtr, string crm, string dtr, string tcs, string ufc, string cls, string tal, string ctr,
                //string bal, string pbo, string tbo, string ucu, string cum, string tpg, string vlt, string vsq, string frc, string cac, string cav, string dpg, string aut, string nsh, string nsu);

                ttbquantidade.Focus();
                if (barras)
                {
                    ttbquantidade.Text = quantidade;
                    vender();
                }
            }

        }

        private void mostrarimagemproduto(string TxtEan)
        {
            ttbproduto.Clear();
            int tt = produto.pesquisarCodigoBarra(TxtEan).Rows.Count;
            if (tt == 0)
            {
                falar(produto1);
                fmok.Mostrar("Produto com imagem padrão não encontrado!");

                ttbproduto.Clear();
                return;
            }
            ttbvalor.Text = produto.pro_valor.ToString("99.00");
            ttbquantidade.Text = "99,00";
            lblprodutonome.Text = produto.pro_nome;
            lblprodutoestoque.Text = produto.pro_quantidade.ToString("99");
            try
            {
                byte[] data = (byte[])produto.pro_imagem;
                MemoryStream ms = new MemoryStream(data);
                btnImagem.BackgroundImage = Image.FromStream(ms);
            }
            catch (Exception ex)
            {
                //    fmok.Mostrar("Erro ao pesquisar imagem do produto: " + ex);
            }
        }

        private void mostrarproduto()
        {
            string ope = "0";
            string ger = "0";
            string uad = "0";
            string lot = "0";
            string maq = "0";
            string dat = "0";
            string hor = "0";
            string cpm = "0";
            string vap = "0";
            string ctr = "0";
            string cts = "0";

            ultimoitem++;
            string TxtEan = ttbproduto.Text;
            ttbproduto.Text = string.Empty;
            ttbvalor.Text = produto.pro_valor.ToString("00.00");
            ttbquantidade.Text = "1,00";
            lblprodutonome.Text = produto.pro_nome;
            lblprodutoestoque.Text = produto.pro_quantidade.ToString("00");
            try
            {
                byte[] data = (byte[])produto.pro_imagem;
                MemoryStream ms = new MemoryStream(data);
                btnImagem.BackgroundImage = Image.FromStream(ms);
            }
            catch (Exception ex)
            {
                //    fmok.Mostrar("Erro ao pesquisar imagem do produto: " + ex);
            }

            ItemCupom = ultimoitem;
            string VendaDetalhe = produto.pro_codigo;//  "********";
            string Unidade = produto.pro_unidade;
            string Quantidade, Unitario, Total;
            Quantidade = ttbquantidade.Text;
            Unitario = ttbvalor.Text;
            Total = Convert.ToString((Convert.ToDecimal(ttbvalor.Text) * Convert.ToDecimal(ttbquantidade.Text)));// VendaDetalhe.ValorTotal.Value.ToString("###,##0.00");
            string DescricaoProduto = lblprodutonome.Text;

            ItemCupom1 = new string('0', 3 - Convert.ToString(ItemCupom).Length) + Convert.ToString(ItemCupom) + "  " + VendaDetalhe + new string(' ', 14 - VendaDetalhe.Length) + " " + DescricaoProduto;
            //Bobina2.Items.Add(ItemCupom1);
            //LinhaCupom++;

            ItemCupom2 = new string(' ', 8 - Quantidade.Length) + Quantidade + " " + new string(' ', 3 - Unidade.Length) + Unidade + " x " + new string(' ', 13 - Unitario.Length) + Unitario + " " + new string(' ', 5) + new string(' ', 14 - Total.Length) + Total;
            //Bobina2.Items.Add(ItemCupom2);
            //LinhaCupom++;

            int NumLinha = gridproduto.RowCount;

            CVenda venda = new CVenda();
            CResumoFiscal Resumo = new CResumoFiscal();
            CMeioPagamento FormaPagamento = new CMeioPagamento();
            CEmpresa Empresa = new CEmpresa();
            CFuncionario Funcionario = new CFuncionario();
            CVenda Venda = new CVenda();
            CCupom Cupom = new CCupom();
            CCaixa Caixa = new CCaixa();
            CProduto Produto = new CProduto();

            if (gridproduto.Rows.Count == 0)
            {
                CupomAtual = Convert.ToString(Cupom.pesquisaTotalCupom());
            }
            Produto.pesquisarProduto(DescricaoProduto);
            venda.pesquisarvendaaberta(idcaixa);
            Funcionario.pesquisar(lblfuncionario.Text, "nome");
            var NCaixa = venda.pesquisarvendaaberta(idcaixa);
            DateTime D1 = DateTime.Now;
            DateTime D2 = DateTime.Now;
            var TotCupm = venda.pesquisarTotalCupons(D1, D2);
            var VTotal = venda.pesquisarTotalVenda(D1, D2);
            DataTable LResumo = Resumo.pesquisarResuno(D1);

            string DataHoje = DateTime.Now.ToString().Replace("/", "-").Substring(0, 10);
            string HoraHoje = DateTime.Now.ToString();
            DataHoje = DataHoje.Substring(6, 4) + "-" + DataHoje.Substring(3, 2) + "-" + DataHoje.Substring(0, 2);
            HoraHoje = HoraHoje.Substring(11, 8);


            int CaixaNum = venda.pesquisarIdVendaAtual();// cai_id;
            //         Caixa.carregarcaixa(D2, D2);
            Caixa.pegacaixa(CaixaNum);

            if (NumLinha == 0)
            {
                CupomAtual = CaixaNum.ToString();// Convert.ToString(Cupom.pesquisaTotalCupom());
                ope = Convert.ToString(Funcionario.fun_id);
                ger = Convert.ToString(Funcionario.fun_id);
                uad = Convert.ToString(empresa.emp_id);
                lot = CupomAtual;// numerocupom Cupom.cup_codigo;
                maq = Convert.ToString(Caixa.cai_id);
                dat = DateTime.Now.ToString();// Convert.ToString(Caixa.cai_datainicial);
                dat = DataHoje;
                hor = DateTime.Now.ToString();//Convert.ToString(Caixa.cai_horafinal);
                hor = HoraHoje;
                cpm = CupomAtual;
                vap = Versao;
                ctr = venda.ven_id.ToString();
                cts = "0";


                GerarCabecaArquivo(CaminhoDadosRep, NomeArquivo, uad, lot, ope, ger, maq, dat, hor, cpm, vap, ctr, cts);
            }


            string destino = "";
            string nomearq = "";
            ope = Convert.ToString(Funcionario.fun_id);
            ger = Convert.ToString(Funcionario.fun_id);
            uad = Convert.ToString(empresa.emp_id);
            lot = Cupom.cup_codigo;
            maq = Convert.ToString(Caixa.cai_id);
            dat = DataHoje;
            hor = HoraHoje;
            cpm = CupomAtual;
            vap = Versao;

            ctr = venda.ven_id.ToString();
            int sequencia = Convert.ToInt32(gridproduto.Rows.Count.ToString());
            sequencia++;
            string seq = sequencia.ToString();
            int IntemGrid = Convert.ToInt32(seq);
            string prd = Produto.pro_id.ToString();// "0";//
            string qtd = "1";//
            string cmp = CupomAtual;//

            string vlr = ttbvalor.Text.Replace(",", ".");//valor
            string ean = TxtEan; // gridproduto.Rows[IntemGrid].Cells["pro_id"].Value.ToString();//
            string vtr = "0";//valor uniotario dos tributos
            string crm = "0";// numero do crm
            string dtr = DataHoje;//
            string tcs = "M";// Tipo de Conselho
            string ufv = "0";// Unidade Federacao de Venda
            string cls = "0";// Classificacao
            string tal = "0";//

            string bal = "0";//
            string pbo = "0";//
            string tbo = "0";//
            DataTable dtProduto = Produto.pesquisarid(Convert.ToInt32(prd));
            string ucu = dtProduto.Rows[0]["pro_ultimo_cust"].ToString();
            string cum = dtProduto.Rows[0]["pro_cust_medio"].ToString();
            string tpg = tipopag;// "0";//
            string vlt = vlr;// "0";//
            string vsq = "0";//
            string frc = "0";//
            string cac = "0";//
            string cav = "0";//
            string dpg = "0";//
            string aut = "0";//
            string nsh = "0";//
            string nsu = "0";//

            GerarItemArquivo(destino, nomearq, seq, prd, qtd, cmp,
             vlr, ean, vtr, crm, dtr, tcs, ufv, cls, tal, ctr,
             bal, pbo, tbo, ucu, cum, tpg, vlt, vsq, frc, cac, cav, dpg, aut, nsh, nsu);

        }

        private void btnpesquisarcliente_Click(object sender, EventArgs e)
        {
            FConsultaCliente f = new FConsultaCliente();
            f.ShowDialog();
            // pnlclientecadastrar.Visible = false;
            if (f.ok)
            {
                cliente.pesquisarID(int.Parse(f.dt.Rows[f.index]["cli_id"].ToString()));
                venda.atualizarClienteFuncionario(venda.ven_id, cliente.id, idfuncionario);
            }
            else
            {
                cliente = new CCliente();
                //  pnlclientecadastrar.BringToFront();
                // pnlclientecadastrar.Visible = true;
            }
            carregar();
        }

        private void ttbproduto_KeyPress(object sender, KeyPressEventArgs e)
        {

            ultimoitem = 0;
            if (idcaixa == 0)
                return;
            else
            //if(ttbproduto.Text.Length == 13)

            if (e.KeyChar == 13)
            {
                //VENDA QUANTIDADE + CODIGO
                string quantidade = "1";
                if (ttbproduto.Text.Contains("="))
                {
                    if (ttbproduto.Text[0] == '=')
                    {
                        return;
                    }
                    int index = ttbproduto.Text.IndexOf('=');
                    quantidade = ttbproduto.Text.Substring(0, index);
                    ttbproduto.Text = ttbproduto.Text.Substring(index + 1);

                }
                //FIM CODIGO + QUANTIDADE
                bool barras = false;
                try
                {
                    long.Parse(ttbproduto.Text);
                    barras = true;
                }
                catch { }


                try
                {
                    //  produto.pesquisarid(int.Parse(gridpesquisarproduto.CurrentRow.Cells[0].Value.ToString()));
                    int tt = produto.pesquisarCodigoBarra(ttbproduto.Text).Rows.Count;
                    if (tt == 0)
                    {
                        falar(produto1);
                        fmok.Mostrar("Produto não encontrado!");

                        ttbproduto.Clear();
                        return;
                    }
                    mostrarproduto();

                    #region LENDO BALANÇA
                    if (UtilizarBalan == "nao")
                    {
                        try
                        {
                            fmok.Close();
                            //LerBalanca("COM11");
                            timer2.Enabled = true;
                            timervalidasacola.Enabled = true;

                            produtonovo = 1;
                            pesototal = peso;
                            fmok.EsconderBotao();
                            fmok.Mostrar(Mensagem4);
                        }
                        catch
                        {

                        }
                    }
                    #endregion


                    ttbquantidade.Focus();
                    if (barras)
                    {
                        ttbquantidade.Text = quantidade;
                        vender();
                    }

                    //}

                }
                catch (Exception ex)
                {
                    fmok.Mostrar(ex.Message);
                }
            }


        }

        private void ttbquantidade_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
                vender();
        }

        public void vender()
        {

            decimal valor = 0;
            decimal quantidade = 0;

            if (produto.pro_id < 1)
            {
                label16.Text = "Passe outro Produto..";
                falar("Passe outro Produto..");
                fmok.Mostrar("Produto não encontrado, passe outro Produto!"); return;

            }
            try
            {
                quantidade = decimal.Parse(ttbquantidade.Text);
            }
            catch { fmok.Mostrar("Quantidade inválido!"); falar("Quantidade inválido!"); return; }

            try
            {
                valor = decimal.Parse(ttbvalor.Text);
            }
            catch { fmok.Mostrar("Valor inválido!"); return; }

            if (produto.pro_estoque > 0)
                if (produto.pro_quantidade < quantidade)
                {
                    falar("Estoque deste produto não é suficiente.");
                    fmok.Mostrar("Estoque insuficiente");
                    return;
                }

            if (venda.ven_id <= 0)
            {
                venda = new CVenda();
                venda.inserir(cliente.id, idfuncionario, 0, "", idcaixa);

            }

            CItemVenda citemvenda = new CItemVenda();
            CCupom cupom = new CCupom();

            if (gridproduto.Rows.Count == 0)
            {
                CupomAtual = Convert.ToString(venda.ven_id);
            }
            else
            {
                CupomAtual = Convert.ToString(venda.pesquisarIdVendaAtual());
            }

            int iteng = gridproduto.Rows.Count;
            int itens = citemvenda.pesquisarTotalItensVenda(venda.ven_id);

            if (iteng == itens)
            {
                citemvenda.inserir(venda.ven_id, produto.pro_id, quantidade, valor);
                produto.baixarestoque(produto.pro_id, quantidade);
                venda.adicionarvalor(venda.ven_id, valor * quantidade);
                label16.Text = produto.pro_descricao;
                falar(label16.Text);
                //http://platin-001-site4.atempurl.com/index.asp = Convert.ToString(venda.ven_id);

                // acBrECF1.VendeItem(produto.pro_codigo, produto.pro_descricao, produto.pro_aliquota.ToString(), quantidade, valor * quantidade);
                itemdecupom = cupom.inserir(CupomAtual, produto.pro_codigo, produto.pro_descricao, produto.pro_aliquota.ToString(), quantidade.ToString(), valor * quantidade);

                carregar();

                limpar();
                ttbproduto.Focus();
            }
            else
            {
                CancelaVendaAtual();
                limpar();
                ttbproduto.Focus();
            }
            //venda
        }

        private void btncancelarcupom_Click(object sender, EventArgs e)
        {
            if (venda.ven_id < 1)
            {
                falar("Não há produto a ser cancelado.");
                fmok.Mostrar("Não há cupom para ser cancelado!");
                return;
            }
            //if (MessageBox.Show("Deseja cancelar o cupom atual?", "Cancelar Venda", MessageBoxButtons.YesNo) != DialogResult.Yes)

            FMessageSimNao f = new FMessageSimNao();
            if (!f.Mostrar("Cancelar Venda", "Deseja cancelar o cupom atual?"))
                return;

            //INICIO ESTOQUE
            CItemVenda citemvenda = new CItemVenda();
            for (int i = 0; i < dtitemvenda.Rows.Count; i++)
            {
                int iditem = 0;
                decimal quantidade = 0;
                decimal valorproduto = 0;
                int idproduto = 0;
                CProduto cproduto = new CProduto();

                try
                {
                    quantidade = decimal.Parse(dtitemvenda.Rows[i]["ite_quantidade"].ToString());
                    valorproduto = decimal.Parse(dtitemvenda.Rows[i]["ite_valor"].ToString());
                    iditem = int.Parse(dtitemvenda.Rows[i]["ite_id"].ToString());
                    idproduto = int.Parse(dtitemvenda.Rows[i]["pro_id"].ToString());

                    citemvenda.remover(iditem);
                    venda.adicionarvalor(venda.ven_id, ((quantidade * valorproduto)) * -1);

                    cproduto.baixarestoque(idproduto, quantidade * -1);
                }
                catch { fmok.Mostrar("ERRO AO REMOVER PRODUTO!!"); };




            }
            //FIM REMOVER ITEM
            venda.excluir(venda.ven_id);
            venda = new CVenda();
            venda.ven_id = 0;
            limpar();
            limparcliente();
            carregar();
        }

        private void btnprevenda_Click(object sender, EventArgs e)
        {
            Formularios.FPreVenda f = new Formularios.FPreVenda(venda.ven_id, cliente.id);
            f.ShowDialog();
            venda = new CVenda();
            venda.ven_id = f.idvenda;
            limpar();
            limparcliente();
            carregar();

        }

        private void ttbquantidade_TextChanged(object sender, EventArgs e)
        {
            try
            {
                decimal quantidade = decimal.Parse(ttbquantidade.Text);
                decimal valor = decimal.Parse(ttbvalor.Text);
                ttbtotalproduto.Text = (quantidade * valor).ToString("00.00");
            }
            catch { ttbtotalproduto.Text = "00,00"; }
        }

        private void gridproduto_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            ultimoitem = gridproduto.Rows.Count;
            this.gridproduto.Rows[e.RowIndex].Cells[0].Value = (e.RowIndex + 1).ToString();
        }

        private void btnproduto_Click(object sender, EventArgs e)
        {
            FConsultaProduto f = new FConsultaProduto();
            f.ShowDialog();
            if (f.ok)
            {
                produto.pesquisarid(int.Parse(f.dt.Rows[f.index]["pro_id"].ToString()));
                mostrarproduto();
                btnpesquisarvoltar.PerformClick();
                ttbquantidade.Focus();
            }
        }

        private void btnpesquisarvoltar_Click(object sender, EventArgs e)
        {
            pnlpesquisar.Visible = false;
        }

        private void btnpnlpesquisar_Click(object sender, EventArgs e)
        {
            // playSimpleSound();
            //PainelMerchan frm = new PainelMerchan();

            //frm.Show();

            pnlpesquisar.BringToFront();

            if (!pnlpesquisar.Visible)
                pnlpesquisar.Visible = true;
            else
                pnlpesquisar.Visible = false;

        }

        private void playSimpleSound()
        {

            if (sonido == null)
            {
                sonido = new WindowsMediaPlayer();
                sonido.URL = Application.StartupPath + @"\mp3\musica.mp3";
                sonido.controls.play();
            }
            //SoundPlayer simpleSound = new SoundPlayer(@"c:\intel\dragon.wav");
            //simpleSound.Play();
        }

        private void timerfechar_Tick(object sender, EventArgs e)
        {
            Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            FCancelarCupom f = new FCancelarCupom(idfuncionario, caixanumero);
            f.ShowDialog();
        }

        private void btncadastrarcliente_Click(object sender, EventArgs e)
        {
            FCliente f = new FCliente();
            f.ShowDialog();
        }

        private void btnvoltarcadastrarcliente_Click(object sender, EventArgs e)
        {
            //pnlclientecadastrar.Visible = false;
        }

        private int criarSAT()
        {

            int sleep = 3000;
            try
            {
                sleep = int.Parse(parametroSat("sleep"));
            }
            catch { MessageBox.Show("Sleep padrao = 3000, favor configurar!"); }



            decimal aliquota = 0;
            decimal aliquotatotal = 0;
            int sessao = 0;
            string texto = "";
            // texto = "SAT.Inicializar";
            // System.IO.File.WriteAllText(parametroSat("caminhocupomtxt"), texto);
            // System.Threading.Thread.Sleep(500);
            string[] lines;// File.ReadAllLines(@"C:\ACBrMonitorPLUS\SAI.txt");
            empresa.pesquisar();
            //     empresa.emp_ie = "111111111111";
            //    empresa.emp_cnpj = "13889840000107";
            texto = @"
 SAT.CriarEnviarCfe(""[infCFe]
  versao = " + parametroSat("versao") + @"
  [Identificacao]
  CNPJ = " + parametroSat("cnpjsh") + @"
  signAC = " + parametroSat("assinaturaac") + @"
  numeroCaixa = " + caixanumero + @"
  [Emitente]
  CNPJ = " + empresa.emp_cnpj + @"
  IE = " + empresa.emp_ie + @" 
  IM = " + empresa.emp_im + @"
  indRatISSQN = S
  [Destinatario]
  CNPJCPF = " + CPFSAT + "";
            //if (ckbcpf.Checked)
            //{
            //    texto += cliente.cpf;
            //    /*if (cliente.id > 0)       **************  IE = " + empresa.emp_ie + @"
            //    {
            //        texto += cliente.cpf;
            //    }
            //    else
            //        texto += lblclienteendereco.Text;*/
            //}
            texto += @"
  xNome = ";
            //if (ckbcpf.Checked)
            //{
            //    //texto += cliente.nome;
            //    if (cliente.id > 0)
            //    {
            //        texto += cliente.nome;
            //    }
            //    else
            //        texto += "Cliente";
            //}

            //if (ckbentrega.Checked)
            //{
            //    texto += @"
            //         [Entrega]
            //      xLgr = " + cliente.endereco + @"
            //       nro = " + cliente.numero + @"
            //      xCpl =
            //      xBairro = " + cliente.bairro + @"
            //      xMun = " + cliente.cid_nome + @"
            //      UF = " + cliente.cid_uf;

            //}

            for (int i = 0; i < dtitemvenda.Rows.Count; i++)
            {
                CItemVenda item = new CItemVenda();
                item.pesquisarIditemvenda(int.Parse(dtitemvenda.Rows[i]["ite_id"].ToString()));
                CProduto produto = new CProduto();
                produto.pesquisarid(item.pro_id);

                if (produto.pro_nome.Length > 30)
                    produto.pro_nome = produto.pro_nome.Substring(0, 29);

                aliquota = (item.ite_quantidade * item.ite_valor) * (produto.pro_porcentagemtributo / 100);
                aliquotatotal += aliquota;
                texto += @"
[Produto" + ((i + 1).ToString("000")) + @"]
cProd = " + produto.pro_id + @"
infAdProd = 
cEAN =
xProd = " + produto.pro_nome + @"
NCM = " + produto.pro_ncm.Replace(".", "") + @"
CFOP = " + produto.pro_cfop + @"
uCom = " + produto.pro_unidade + @"
Combustivel = 0
qCom = " + item.ite_quantidade + @"
vUnCom = " + item.ite_valor + @"
indRegra = A
vDesc = 0
vOutro = 0
vItem12741 = " + aliquota + @"
[ObsFiscoDet" + ((i + 1).ToString("000")) + @"]
xCampoDet = Cod. CEST
xTextoDet = " + produto.pro_cest;


                texto += @"
[ICMS" + ((i + 1).ToString("000")) + @"]
Orig = " + produto.pro_origem.ToString();
                if (configuracao.regime == "S")
                {
                    texto += @"
CSOSN=" + produto.pro_csosn;
                    if (produto.pro_csosn == "900")
                    {
                        texto += @"
pICMS=" + produto.pro_aliquota;
                    }
                    texto += @"
[PIS" + ((i + 1).ToString("000")) + @"]
CST = 49
[COFINS" + ((i + 1).ToString("000")) + @"]
CST = 49";
                }
                else
                {
                    /* string cst = "";
                             pis = pisempresa / 100
                             pis = if (wpPIS = 0, 0.0065, wpPIS)
                             if (cst = "00"|| cst = "20" || cst = "90")
                               "CST=01"
                               "vBC=" + (quant * valor)
                               "pPIS=" + pis
                             else
                               if (cst = "40" || cst = "41" || cst = "50")
                               "CST=07"
                               else
                               if (cst = "60")
                               "CST=05"
                               "vBC=" 0
                               "pPIS=" pis

                               "[PISST" + strzero(wseq, 3) + "]"
                               "vBC=" + ltrim(transf(quant * valor, "@E 999999.99"))
                               "pPIS=" + ltrim(transf(wpPIS, "@E 999999.9999"))*/


                    texto += @"[ICMS" + ((i + 1).ToString("000")) + @"]
Orig = 0
CST = 40
[PIS" + ((i + 1).ToString("000")) + @"]
CST = 01
[COFINS" + ((i + 1).ToString("000")) + @"]
CST = 01";
                }
            }

            texto += @"
[Total]
vCFeLei12741 = " + aliquotatotal + @"
[DescAcrEntr]
vDescSubtot = " + venda.ven_desconto.ToString();

            CContaReceber c = new CContaReceber();
            DataTable dtparcelas = c.carregarParcelasMeioPagamentoAgrupado(venda.ven_id);

            for (int i = 0; i < dtparcelas.Rows.Count; i++)
            {
                FormPag = dtparcelas.Rows[i]["PAR_DESCRICAO"].ToString();
                texto += @"
[Pagto" + ((i + 1).ToString("000")) + @"]
cMP = " + int.Parse(dtparcelas.Rows[i]["mei_id"].ToString()).ToString("00") + @"
vMP = " + dtparcelas.Rows[i]["par_total"].ToString();
            }


            texto += @"
  [DadosAdicionais]
  infCpl = " + configuracao.mensagem + @"
    [ObsFisco001]
  xCampo = 
  xTexto = "")";

            string mesarquivo = DateTime.Now.ToString().Substring(0, 2) + DateTime.Now.ToString().Substring(3, 2) + DateTime.Now.ToString().Substring(6, 4) + DateTime.Now.ToString().Substring(11, 2) + DateTime.Now.ToString().Substring(14, 2) + DateTime.Now.ToString().Substring(17, 2);
            if (satmon == "nao")// utilizan€do o Aprelho SAT
            {
                File.Delete(parametroSat("caminhosaitxt"));
                System.IO.File.WriteAllText(parametroSat("caminhocupomtxt"), texto);
            }
            else
            {//19/06/2015 10:03:06
                try
                {
                    //  string mesarquivo = DateTime.Now.ToString().Substring(0, 2) + DateTime.Now.ToString().Substring(3, 2) + DateTime.Now.ToString().Substring(6, 4) + DateTime.Now.ToString().Substring(11, 2) + DateTime.Now.ToString().Substring(14, 2) + DateTime.Now.ToString().Substring(17, 2);
                    System.IO.File.WriteAllText(Caminhoxmlenv + "AD" + mesarquivo + ".xml", texto);
                }
                catch
                {
                    CriarPasta(Caminhoxmlenv, "AD" + mesarquivo + ".xml", texto);
                }
            }


            System.Threading.Thread.Sleep(sleep);

            pnlcarregando.Visible = false;


            if (satmon == "nao")
            {
                #region Validar APARELHO SAT
                string xml = retornoSAT("XML");
                if (xml.Contains("Erro Monitor"))
                {
                    File.Delete(parametroSat("caminhocupomtxt"));
                    falar(msgsat1);
                    fmok.MostrarBotao();
                    fmok.Mostrar("Erro!. Monitor não encontrado!");
                    return 0;
                }

                string xmlpuro = xml;
                xml = "SAT.ImprimirExtratoVenda(\"" + xml + "\");";
                codigoDeRetorno = retornoSAT("codigoDeRetorno");
                numeroSessao = retornoSAT("numeroSessao");
                System.IO.File.WriteAllText(@"C:\ACBrMonitorPLUS\ENT.txt", xml);

                bool via2 = false;
                for (int i = 0; i < dtparcelas.Rows.Count; i++)
                {
                    if (dtparcelas.Rows[i]["par_descricao"].ToString().ToUpper().Contains("MARCAR"))
                    {
                        via2 = true;
                    }
                }

                if (via2)
                {
                    System.Threading.Thread.Sleep(sleep);
                    System.IO.File.WriteAllText(@"C:\ACBrMonitorPLUS\ENT.txt", xml);
                }
                #endregion
            }
            else
            {
                codigoDeRetorno = "6000";
            }


            if (codigoDeRetorno != "6000")
            {
                //// enviar codigo NCN para TEF

                sessao = 0;
                int retorno = 0;
                try
                {
                    retorno = int.Parse(codigoDeRetorno);
                }
                catch { }
                fmok.MostrarBotao();
                fmok.Mostrar(retornaMSGSat(retorno));
            }
            else
                try
                {
                    /// evviar codigo CNF para TEF

                    CCupom citemcupom = new CCupom();
                    sessao = int.Parse(numeroSessao);
                    CSat sat = new CSat();
                    sat.inserir(venda.ven_id, sessao, xmlpuro);

                    /////////////////// IMPRESSAO DE CUPOM

                    if (UtilizarImpre == "sim")
                    {
                        status = acBrECF1.Estado.ToString();

                        if (status == "Livre" || status == "Bloqueada")
                        {
                            try
                            {
                                acBrECF1.AbreCupom();
                            }
                            catch (Exception ex)
                            {
                                fmok.Mostrar("Não possivel abrir o cupom!");
                            }
                        }
                        else
                        {
                            falar("NÃO É POSSIVEL PROCESSAR O ITEM DO CUPOM COM STATUS DA IMPRESSORA EM " + status.ToUpper() + ".");
                            fmok.Mostrar(" NAO É POSSIVEL PROCESSAR O ITEM DO CUPOM COM STATUS DA IMPRESSORA EM " + status.ToUpper() + " !!");

                        }
                    }

                    carregarcupom();

                    for (int i = 0; i < dtitemcupom.Rows.Count; i++)
                    {
                        int iditem = 0;
                        decimal quantidade = 0;
                        decimal valorproduto = 0;
                        string produto = "";
                        string codigo = "";
                        string aliq = "";
                        CProduto cproduto = new CProduto();

                        try
                        {
                            quantidade = decimal.Parse(dtitemcupom.Rows[i]["CUP_QUANTIDADE"].ToString());
                            valorproduto = decimal.Parse(dtitemcupom.Rows[i]["CUP_TOTAL"].ToString());
                            iditem = int.Parse(dtitemcupom.Rows[i]["CUP_ID"].ToString());
                            produto = dtitemcupom.Rows[i]["CUP_PRODUTO"].ToString();
                            codigo = dtitemcupom.Rows[i]["CUP_CODIGO"].ToString();
                            aliq = dtitemcupom.Rows[i]["CUP_ALIQUOTA"].ToString();

                            if (UtilizarImpre == "sim") acBrECF1.VendeItem(codigo, produto, aliq, quantidade, valorproduto * quantidade);
                        }
                        catch (Exception ex) { fmok.Mostrar(ex.Message + " = NAO FOI POSSIVEL PROCESSAR O ITEM DO CUPOM!!"); };
                    }

                    if (UtilizarImpre == "sim")
                    {
                        // tipopag = "01";
                        acBrECF1.SubtotalizaCupom();
                        acBrECF1.EfetuaPagamento(tipopag, decimal.Parse(ttbsubtotal.Text));
                        acBrECF1.FechaCupom(@"Self Checkout Platin|http://platin.com.br");
                    }
                    citemcupom.removertodos(0);

                }
                catch (Exception ex)
                {
                    sessao = 0; fmok.Mostrar(ex.Message); /*acBrECF1.CancelaCupom(); */
                    System.Threading.Thread.CurrentThread.Abort();
                    this.Close();
                }
            return sessao;
        }

        private string parametroSat(string chave)
        {

            string retorno = "";
            string[] lines = File.ReadAllLines(Application.StartupPath + @"\SAT.ini");
            for (int i = 0; i < lines.Length; i++)
            {
                if (lines[i].Contains(chave + "="))
                {
                    retorno = lines[i].Substring(chave.Length + 1);
                }
            }
            return retorno;
        }

        private string retornaMSGSat(int codigodeRetorno)
        {
            string msg = ""; // colocar aliquota de pis e confins em centesimal 
            string[] msgSat = new string[20000];

            msgSat[100] = "CF-e-SAT processado com sucesso";
            msgSat[101] = "CF-e-SAT de cancelamento processado com sucesso";
            msgSat[102] = "CF-e-SAT processado – verificar inconsistências";
            msgSat[103] = "CF-e-SAT de cancelamento processado – verificar inconsistências";
            msgSat[104] = "Não Existe Atualização do Software";
            msgSat[105] = "Lote recebido com sucesso";
            msgSat[106] = "Lote Processado";
            msgSat[107] = "Lote em Processamento";
            msgSat[108] = "Lote não localizado";
            msgSat[109] = "Serviço em Operação";
            msgSat[110] = "Status SAT recebido com sucesso";
            msgSat[112] = "Assinatura do AC Registrada";
            msgSat[113] = "Consulta cadastro com uma ocorrência";
            msgSat[114] = "Consulta cadastro com mais de uma ocorrência";
            msgSat[115] = "Solicitação de dados efetuada com sucesso";
            msgSat[116] = "Atualização do SB pendente";
            msgSat[117] = "Solicitação de Arquivo de Parametrização efetuada com sucesso";
            msgSat[118] = "Logs extraídos com sucesso";
            msgSat[119] = "Comandos da SEFAZ pendentes";
            msgSat[120] = "Não existem comandos da SEFAZ pendentes";
            msgSat[121] = "Certificado Digital criado com sucesso";
            msgSat[122] = "CRT recebido com sucesso";
            msgSat[123] = "Adiar transmissão do lote";
            msgSat[124] = "Adiar transmissão do CF-e";
            msgSat[125] = "CF-e de teste de produção emitido com sucesso";
            msgSat[126] = "CF-e de teste de ativação emitido com sucesso";
            msgSat[127] = "Erro na emissão de CF-e de teste de produção";
            msgSat[128] = "Erro na emissão de CF-e de teste de ativação";
            msgSat[129] = "Solicitações de emissão de certificados excedidas. (Somente ocorrerá no ambiente de testes)";
            msgSat[200] = "Rejeição: Status do equipamento SAT difere do esperado";
            msgSat[201] = "Rejeição: Falha na Verificação da Assinatura do Número de segurança";
            msgSat[202] = "Rejeição: Falha no reconhecimento da autoria ou integridade do arquivo digital";
            msgSat[203] = "Rejeição: Emissor não Autorizado para emissão da CF-e-SAT";
            msgSat[204] = "Rejeição: Duplicidade de CF-e-SAT";
            msgSat[205] = "Rejeição: Equipamento SAT encontra-se Ativo";
            msgSat[206] = "Rejeição: Hora de Emissão do CF-e-SAT posterior à hora de recebimento.";
            msgSat[207] = "Rejeição: CNPJ do emitente inválido";
            msgSat[208] = "Rejeição: Equipamento SAT encontra-se Desativado";
            msgSat[209] = "Rejeição: IE do emitente inválida";
            msgSat[210] = "Rejeição: Intervalo de tempo entre o CF-e-SAT emitido e a emissão do respectivo CF-e-SAT de cancelamento é maior que 30 (trinta) minutos.";
            msgSat[211] = "Rejeição: CNPJ não corresponde ao informado no processo de transferência.";
            msgSat[212] = "Rejeição: Data de Emissão do CF-e-SAT posterior à data de recebimento.";
            msgSat[213] = "Rejeição: CNPJ-Base do Emitente difere do CNPJ-Base do Certificado Digital";
            msgSat[214] = "Rejeição: Tamanho da mensagem excedeu o limite estabelecido";
            msgSat[215] = "Rejeição: Falha no schema XML";
            msgSat[216] = "Rejeição: Chave de Acesso difere da cadastrada";
            msgSat[217] = "Rejeição: CF-e-SAT não consta na base de dados da SEFAZ";
            msgSat[218] = "Rejeição: CF-e-SAT já esta cancelado na base de dados da SEFAZ";
            msgSat[219] = "Rejeição: CNPJ não corresponde ao informado no processo de declaração de posse.";
            msgSat[220] = "Rejeição: Valor do rateio do desconto sobre subtotal do item (N) inválido.";
            msgSat[221] = "Rejeição: Aplicativo Comercial não vinculado ao SAT";
            msgSat[222] = "Rejeição: Assinatura do Aplicativo Comercial inválida";
            msgSat[223] = "Rejeição: CNPJ do transmissor do lote difere do CNPJ do transmissor da consulta";
            msgSat[224] = "Rejeição: CNPJ da Software House inválido";
            msgSat[225] = "Rejeição: Falha no Schema XML do lote de CFe";
            msgSat[226] = "Rejeição: Código da UF do Emitente diverge da UF receptora";
            msgSat[227] = "Rejeição: Erro na Chave de Acesso - Campo Id – falta a literal CFe";
            msgSat[228] = "Rejeição: Valor do rateio do acréscimo sobre subtotal do item (N) inválido.";
            msgSat[229] = "Rejeição: IE do emitente não informada";
            msgSat[230] = "Rejeição: IE do emitente não autorizada para uso do SAT";
            msgSat[231] = "Rejeição: IE do emitente não vinculada ao CNPJ";
            msgSat[232] = "Rejeição: CNPJ do destinatário do CF-e-SAT de cancelamento diferente daquele do CF-e-SAT a ser cancelado.";
            msgSat[233] = "Rejeição: CPF do destinatário do CF-e-SAT de cancelamento diferente daquele do CF-e-SAT a ser cancelado.";
            msgSat[234] = "Alerta: Razão Social/Nome do destinatário em branco";
            msgSat[235] = "Rejeição: CNPJ do destinatario Invalido";
            msgSat[236] = "Rejeição: Chave de Acesso com dígito verificador inválido";
            msgSat[237] = "Rejeição: CPF do destinatario Invalido";
            msgSat[238] = "Rejeição: CNPJ do emitente do CF-e-SAT de cancelamento diferente do CNPJ do CF-e-SAT a ser cancelado.";
            msgSat[239] = "Rejeição: Versão do arquivo XML não suportada";
            msgSat[240] = "Rejeição: Valor total do CF-e-SAT de cancelamento diferente do Valor total do CF-e-SAT a ser cancelado.";
            msgSat[241] = "Rejeição: diferença de transmissão e recebimento da mensagem superior a 5 minutos.";
            msgSat[242] = "Alerta: CFe dentro do lote estão fora de ordem.";
            msgSat[243] = "Rejeição: XML Mal Formado";
            msgSat[244] = "Rejeição: CNPJ do Certificado Digital difere do CNPJ da Matriz e do CNPJ do Emitente";
            msgSat[245] = "Rejeição: CNPJ Emitente não autorizado para uso do SAT";
            msgSat[246] = "Rejeição: Campo cUF inexistente no elemento cfeCabecMsg do SOAP Header";
            msgSat[247] = "Rejeição: Sigla da UF do Emitente diverge da UF receptora";
            msgSat[248] = "Rejeição: UF do Recibo diverge da UF autorizadora";
            msgSat[249] = "Rejeição: UF da Chave de Acesso diverge da UF receptora";
            msgSat[250] = "Rejeição: UF informada pelo SAT, não é atendida pelo Web Service";
            msgSat[251] = "Rejeição: Certificado enviado não confere com o escolhido na declaração de posse";
            msgSat[252] = "Rejeição: Ambiente informado diverge do Ambiente de recebimento";
            msgSat[253] = "Rejeição: Digito Verificador da chave de acesso composta inválida";
            msgSat[254] = "Rejeição: Elemento cfeCabecMsg inexistente no SOAP Header";
            msgSat[255] = "Rejeição: CSR enviado inválido";
            msgSat[256] = "Rejeição: CRT enviado inválido";
            msgSat[257] = "Rejeição: Número do série do equipamento inválido";
            msgSat[258] = "Rejeição: Data e/ou hora do envio inválida";
            msgSat[259] = "Rejeição: Versão do leiaute inválida";
            msgSat[260] = "Rejeição: UF inexistente";
            msgSat[261] = "Rejeição: Assinatura digital não encontrada";
            msgSat[262] = "Rejeição: CNPJ da software house não está ativo";
            msgSat[263] = "Rejeição: CNPJ do contribuinte não está ativo";
            msgSat[264] = "Rejeição: Base da receita federal está indisponível";
            msgSat[265] = "Rejeição: Número de série inexistente no cadastro do equipamento";
            msgSat[266] = "Falha na comunicação com a AC-SAT";
            msgSat[267] = "Erro desconhecido na geração do certificado pela AC-SAT";
            msgSat[268] = "Rejeição: Certificado está fora da data de validade.";
            msgSat[269] = "Rejeição: Tipo de atividade inválida";
            msgSat[270] = "Rejeição: Chave de acesso do CFe a ser cancelado inválido.";
            msgSat[271] = "Rejeição: Ambiente informado no CF-e difere do Ambiente de recebimento cadastrado.";
            msgSat[272] = "Rejeição: Valor do troco negativo.";
            msgSat[273] = "Rejeição: Serviço Solicitado Inválido";
            msgSat[274] = "Rejeição: Equipamento não possui declaração de posse";
            msgSat[275] = "Rejeição: Status do equipamento diferente de Fabricado";
            msgSat[276] = "Rejeição: Diferença de dias entre a data de emissão e de recepção maior que o prazo legal";
            msgSat[277] = "Rejeição: CNPJ do emitente não está ativo junto à Sefaz na data de emissão";
            msgSat[278] = "Rejeição: IE do emitente não está ativa junto à Sefaz na data de emissão";
            msgSat[280] = "Rejeição: Certificado Transmissor Inválido";
            msgSat[281] = "Rejeição: Certificado Transmissor Data Validade";
            msgSat[282] = "Rejeição: Certificado Transmissor sem CNPJ";
            msgSat[283] = "Rejeição: Certificado Transmissor - erro Cadeia de Certificação";
            msgSat[284] = "Rejeição: Certificado Transmissor revogado";
            msgSat[285] = "Rejeição: Certificado Transmissor difere ICP-Brasil";
            msgSat[286] = "Rejeição: Certificado Transmissor erro no acesso a LCR";
            msgSat[287] = "Rejeição: Código Município do FG - ISSQN: dígito inválido. Exceto os códigos descritos no Anexo 2 que apresentam dígito inválido.";
            msgSat[288] = "Rejeição: Data de emissão do CF-e-SAT a ser cancelado inválida";
            msgSat[289] = "Rejeição: Código da UF informada diverge da UF solicitada";
            msgSat[290] = "Rejeição: Certificado Assinatura inválido";
            msgSat[291] = "Rejeição: Certificado Assinatura Data Validade";
            msgSat[292] = "Rejeição: Certificado Assinatura sem CNPJ";
            msgSat[293] = "Rejeição: Certificado Assinatura - erro Cadeia de Certificação";
            msgSat[294] = "Rejeição: Certificado Assinatura revogado";
            msgSat[295] = "Rejeição: Certificado Raiz difere dos Válidos";
            msgSat[296] = "Rejeição: Certificado Assinatura erro no acesso a LCR";
            msgSat[297] = "Rejeição: Assinatura difere do calculado";
            msgSat[298] = "Rejeição: Assinatura difere do padrão do Projeto";
            msgSat[299] = "Rejeição: Hora de emissão do CF-e-SAT a ser cancelado inválida";
            msgSat[402] = "Rejeição: XML da área de dados com codificação diferente de UTF-8";
            msgSat[403] = "Rejeição: Versão do leiaute do CF-e-SAT não é válida";
            msgSat[404] = "Rejeição: Uso de prefixo de namespace não permitido";
            msgSat[405] = "Alerta: Versão do leiaute do CF-e-SAT não é a mais atual";
            msgSat[406] = "Rejeição: Versão do Software Básico do SAT não é valida.";
            msgSat[407] = "Rejeição: Indicador de CF-e-SAT cancelamento inválido (diferente de „C? e „?)";
            msgSat[408] = "Rejeição: Valor total do CF-e-SAT maior que o somatório dos valores de Meio de Pagamento empregados em seu pagamento.";
            msgSat[409] = "Rejeição: Valor total do CF-e-SAT supera o máximo permitido no arquivo de Parametrização de Uso";
            msgSat[410] = "Rejeição: UF informada no campo cUF não é atendida pelo Web Service";
            msgSat[411] = "Rejeição: Campo versaoDados inexistente no elemento cfeCabecMsg do SOAP Header";
            msgSat[412] = "Rejeição: CFe de cancelamento não corresponde ao CFe anteriormente gerado";
            msgSat[420] = "Rejeição: Cancelamento para CF-e-SAT já cancelado";
            msgSat[450] = "Rejeição: Modelo da CF-e-SAT diferente de 59";
            msgSat[452] = "Rejeição: número de série do SAT inválido ou não autorizado.";
            msgSat[453] = "Rejeição: Ambiente de processamento inválido (diferente de 1 e 2)";
            msgSat[454] = "Rejeição: CNPJ da Software House inválido";
            msgSat[455] = "Rejeição: Assinatura do Aplicativo Comercial não é válida.";
            msgSat[456] = "Rejeição: Código de Regime tributário invalido";
            msgSat[457] = "Rejeição: Código de Natureza da Operação para ISSQN inválido";
            msgSat[458] = "Rejeição: Razão Social/Nome do destinatário em branco";
            msgSat[459] = "Rejeição: Código do produto ou serviço em branco";
            msgSat[460] = "Rejeição: GTIN do item (N) inválido";
            msgSat[461] = "Rejeição: Descrição do produto ou serviço em branco";
            msgSat[462] = "Rejeição: CFOP não é de operação de saída prevista para CF-e-SAT";
            msgSat[463] = "Rejeição: Unidade comercial do produto ou serviço em branco";
            msgSat[464] = "Rejeição: Quantidade Comercial do item (N) inválido";
            msgSat[465] = "Rejeição: Valor unitário do item (N) inválido";
            msgSat[466] = "Rejeição: Valor bruto do item (N) difere de quantidade * Valor Unitário, considerando regra de arred/trunc.";
            msgSat[467] = "Rejeição: Regra de calculo do item (N) inválida";
            msgSat[468] = "Rejeição: Valor do desconto do item (N) inválido";
            msgSat[469] = "Rejeição: Valor de outras despesas acessórias do item (N) inválido.";
            msgSat[470] = "Rejeição: Valor líquido do Item do CF-e difere de Valor Bruto de Produtos e Serviços - desconto + Outras Despesas Acessórias – rateio do desconto sobre subtotal + rateio do acréscimo sobre subtotal ";
            msgSat[471] = "Rejeição: origem da mercadoria do item (N) inválido (difere de 0, 1, 2, 3, 4, 5, 6 e 7)";
            msgSat[472] = "Rejeição: CST do Item (N) inválido (diferente de 00, 20, 90)";
            msgSat[473] = "Rejeição: Alíquota efetiva do ICMS do item (N) inválido.";
            msgSat[474] = "Rejeição: Valor líquido do ICMS do Item (N) difere de Valor do Item * Aliquota Efetiva";
            msgSat[475] = "Rejeição: CST do Item (N) inválido (diferente de 40 e 41 e 50 e 60)";
            msgSat[476] = "Rejeição: Código de situação da operação - Simples Nacional - do Item (N) inválido (diferente de 102, 300 e 500)";
            msgSat[477] = "Rejeição: Código de situação da operação - Simples Nacional - do Item (N) inválido (diferente de 900)";
            msgSat[478] = "Rejeição: Código de Situação Tributária do PIS Inválido (diferente de 01 e 02)";
            msgSat[479] = "Rejeição: Base de cálculo do PIS do item (N) inválido.";
            msgSat[480] = "Rejeição: Alíquota do PIS do item (N) inválido.";
            msgSat[481] = "Rejeição: Valor do PIS do Item (N) difere de Base de Calculo * Aliquota do PIS";
            msgSat[482] = "Rejeição: Código de Situação Tributária do PIS Inválido (diferente de 03)";
            msgSat[483] = "Rejeição: Qtde Vendida do item (N) inválido.";
            msgSat[484] = "Rejeição: Alíquota do PIS em R$ do item (N) inválido.";
            msgSat[485] = "Rejeição: Valor do PIS do Item (N) difere de Qtde Vendida* Aliquota do PIS em R$";
            msgSat[486] = "Rejeição: Código de Situação Tributária do PIS Inválido (diferente de 04, 06, 07, 08 e 09)";
            msgSat[487] = "Rejeição: Código de Situação Tributária do PIS inválido (diferente de 49)";
            msgSat[488] = "Rejeição: Código de Situação Tributária do PIS Inválido (diferente de 99)";
            msgSat[489] = "Rejeição: Valor do PIS do Item (N) difere de Qtde Vendida* Aliquota do PIS em R$ e difere de Base de Calculo * Aliquota do PIS";
            msgSat[490] = "Rejeição: Código de Situação Tributária da COFINS Inválido (diferente de 01 e 02)";
            msgSat[491] = "Rejeição: Base de cálculo do COFINS do item (N) inválido.";
            msgSat[492] = "Rejeição: Alíquota da COFINS do item (N) inválido.";
            msgSat[493] = "Rejeição: Valor da COFINS do Item (N) difere de Base de Calculo * Aliquota da COFINS";
            msgSat[494] = "Rejeição: Código de Situação Tributária da COFINS Inválido (diferente de 03)";
            msgSat[495] = "Rejeição: Valor do COFINS do Item (N) difere de Qtde Vendida* Aliquota do COFINS em R$ e difere de Base de Calculo * Aliquota do COFINS";
            msgSat[496] = "Rejeição: Alíquota da COFINS em R$ do item (N) inválido.";
            msgSat[497] = "Rejeição: Valor da COFINS do Item (N) difere de Qtde Vendida* Aliquota da COFINS em R$";
            msgSat[498] = "Rejeição: Código de Situação Tributária da COFINS Inválido (diferente de 04, 06, 07, 08 e 09)";
            msgSat[499] = "Rejeição: Código de Situação Tributária da COFINS Inválido (diferente de 49)";
            msgSat[500] = "Rejeição: Código de Situação Tributária da COFINS Inválido (diferente de 99)";
            msgSat[501] = "Rejeição: Operação com tributação de ISSQN sem informar a Inscrição Municipal";
            msgSat[502] = "Rejeição: Erro na Chave de Acesso - Campo Id não corresponde à concatenação dos campos correspondentes";
            msgSat[503] = "Rejeição: Valor das deduções para o ISSQN do item (N) inválido.";
            msgSat[504] = "Rejeição: Valor da Base de Calculo do ISSQN do Item (N) difere de Valor do Item - Valor das deduções";
            msgSat[505] = "Rejeição: Alíquota efetiva do ISSQN do item (N) não é maior ou igual a 2,00 (2%) e menor ou igual a 5,00 (5%).";
            msgSat[506] = "Valor do ISSQN do Item (N) difere de Valor da Base de Calculo do ISSQN * Alíquota Efetiva do ISSQN";
            msgSat[507] = "Rejeição: Indicador de rateio para ISSQN inválido";
            msgSat[508] = "Rejeição: Item da lista de Serviços do ISSQN do item (N) inválido.";
            msgSat[509] = "Rejeição: Código municipal de Tributação do ISSQN do Item (N) em branco.";
            msgSat[510] = "Rejeição: Código de Natureza da Operação para ISSQN inválido";
            msgSat[511] = "Rejeição: Indicador de Incentivo Fiscal do ISSQN do item (N) inválido (diferente de 1 e 2)";
            msgSat[512] = "Rejeição: Total do PIS difere do somatório do PIS dos itens";
            msgSat[513] = "Rejeição: Total do COFINS difere do somatório do COFINS dos itens";
            msgSat[514] = "Rejeição: Total do PIS-ST difere do somatório do PIS-ST dos itens";
            msgSat[515] = "Rejeição: Total do COFINS-STdifere do somatório do COFINS-ST dos itens";
            msgSat[516] = "Rejeição: Total de Outras Despesas Acessórias difere do somatório de Outras Despesas Acessórias (acréscimo) dos itens";
            msgSat[517] = "Rejeição: Total dos Itens difere do somatório do valor líquido dos itens";
            msgSat[518] = "Rejeição: Informado grupo de totais do ISSQN sem informar grupo de valores de ISSQN";
            msgSat[519] = "Rejeição: Total da BC do ISSQN difere do somatório da BC do ISSQN dos itens";
            msgSat[520] = "Rejeição: Total do ISSQN difere do somatório do ISSQN dos itens";
            msgSat[521] = "Rejeição: Total do PIS sobre serviços difere do somatório do PIS dos itens de serviços";
            msgSat[522] = "Rejeição: Total do COFINS sobre serviços difere do somatório do COFINS dos itens de serviços";
            msgSat[523] = "Rejeição: Total do PIS-ST sobre serviços difere do somatório do PIS-ST dos itens de serviços";
            msgSat[524] = "Rejeição: Total do COFINS-ST sobre serviços difere do somatório do COFINS-ST dos itens de serviços";
            msgSat[525] = "Rejeição: Valor de Desconto sobre total inválido.";
            msgSat[526] = "Rejeição: Valor de Acréscimo sobre total inválido.";
            msgSat[527] = "Rejeição: Código do Meio de Pagamento inválido";
            msgSat[528] = "Rejeição: Valor do Meio de Pagamento inválido.";
            msgSat[529] = "Rejeição: Valor de desconto sobre subtotal difere do somatório dos seus rateios nos itens.";
            msgSat[530] = "Rejeição: Operação com tributação de ISSQN sem informar a Inscrição Municipal";
            msgSat[531] = "Rejeição: Valor de acréscimo sobre subtotal difere do somatório dos seus rateios nos itens.";
            msgSat[532] = "Rejeição: Total do ICMS difere do somatório dos itens";
            msgSat[533] = "Rejeição: Valor aproximado dos tributos do CF-e-SAT – Lei 12741/12 inválido";
            msgSat[534] = "Rejeição: Valor aproximado dos tributos do Produto ou serviço – Lei 12741/12 inválido.";
            msgSat[535] = "Rejeição: código da credenciadora de cartão de débito ou crédito inválido";
            msgSat[537] = "Rejeição: Total do Desconto difere do somatório dos itens";
            msgSat[539] = "Rejeição: Duplicidade de CF-e-SAT, com diferença na Chave de Acesso [99999999999999999999999999999999999999999]";
            msgSat[540] = "Rejeição: CNPJ da Software House + CNPJ do emitente assinado no campo “signAC” difere do informado no campo “CNPJvalue” ";
            msgSat[555] = "Rejeição: Tipo autorizador do protocolo diverge do Órgão Autorizador";
            msgSat[564] = "Rejeição: Total dos Produtos ou Serviços difere do somatório do valor dos Produtos ou Serviços dos itens";
            msgSat[600] = "Serviço Temporariamente Indisponível";
            msgSat[601] = "CF-e-SAT inidôneo por recepção fora do prazo";
            msgSat[602] = "Rejeição: Status do equipamento não permite ativação";
            msgSat[603] = "Arquivo inválido";
            msgSat[604] = "Erro desconhecido na verificação de comandos";
            msgSat[605] = "Tamanho do arquivo inválido";
            msgSat[999] = "Rejeição: Erro não catalogado";

            msgSat[04000] = "Ativado corretamente SAT Ativado com Sucesso.";
            msgSat[04001] = "Erro na criação do certificado processo de ativação foi interrompido.";
            msgSat[04002] = "SEFAZ não reconhece este SAT (CNPJ inválido) Verificar junto a SEFAZ o CNPJ cadastrado.";
            msgSat[04003] = "SAT já ativado SAT disponível para uso.";
            msgSat[04004] = "SAT com uso cessado SAT bloqueado por cessação de uso.";
            msgSat[04005] = "Erro de comunicação com a SEFAZ Tentar novamente.";
            msgSat[04006] = "CSR ICP-BRASIL criado com sucesso Processo de criação do CSR para certificação ICP-BRASIL com sucesso";
            msgSat[04007] = "Erro na criação do CSR ICP-BRASIL Processo de criação do CSR para certificação ICP-BRASIL com erro";
            msgSat[04098] = "SAT em processamento. Tente novamente.";
            msgSat[04099] = "Erro desconhecido na ativação Informar ao administrador.";
            msgSat[05000] = "Certificado transmitido com Sucesso ";
            msgSat[05001] = "Código de ativação inválido.";
            msgSat[05002] = "Erro de comunicação com a SEFAZ. Tentar novamente.";
            msgSat[05003] = "Certificado Inválido ";
            msgSat[05098] = "SAT em processamento.";
            msgSat[05099] = "Erro desconhecido Informar o administrador.";
            msgSat[06000] = "Emitido com sucesso + conteúdo notas. Retorno CF-e-SAT ao AC para contingência.";
            msgSat[06001] = "Código de ativação inválido.";
            msgSat[06002] = "SAT ainda não ativado. Efetuar ativação.";
            msgSat[06003] = "SAT não vinculado ao AC Efetuar vinculação";
            msgSat[06004] = "Vinculação do AC não confere Efetuar vinculação";
            msgSat[06005] = "Tamanho do CF-e-SAT superior a 1.500KB";
            msgSat[06006] = "SAT bloqueado pelo contribuinte";
            msgSat[06007] = "SAT bloqueado pela SEFAZ";
            msgSat[06008] = "SAT bloqueado por falta de comunicação";
            msgSat[06009] = "SAT bloqueado, código de ativação incorreto";
            msgSat[06010] = "Erro de validação do conteúdo.";
            msgSat[06098] = "SAT em processamento.";
            msgSat[06099] = "Erro desconhecido na emissão. Informar o administrador.";
            msgSat[07000] = "Cupom cancelado com sucesso + conteúdo CF-eSAT cancelado.";
            msgSat[07001] = "Código ativação inválido Verificar o código e tentar mais uma vez.";
            msgSat[07002] = "Cupom inválido Informar o administrador.";
            msgSat[07003] = "SAT bloqueado pelo contribuinte";
            msgSat[07004] = "SAT bloqueado pela SEFAZ";
            msgSat[07005] = "SAT bloqueado por falta de comunicação";
            msgSat[07006] = "SAT bloqueado, código de ativação incorreto";
            msgSat[07007] = "Erro de validação do conteúdo";
            msgSat[07098] = "SAT em processamento.";
            msgSat[07099] = "Erro desconhecido no cancelamento.";
            msgSat[08000] = "SAT em operação. Verifica se o SAT está ativo.";
            msgSat[08098] = "SAT em processamento.";
            msgSat[08099] = "Erro desconhecido. Informar o administrador.";
            msgSat[09000] = "Emitido com sucesso Gera e envia um cupom de teste para SEFAZ, para verificar a comunicação.";
            msgSat[09001] = "código ativação inválido Verificar o código e tentar mais uma vez.";
            msgSat[09002] = "SAT ainda não ativado. Efetuar ativação ";
            msgSat[09098] = "SAT em processamento.";
            msgSat[09099] = "Erro desconhecido Informar o ";
            msgSat[10000] = "Resposta com Sucesso. Informações de status do SAT.";
            msgSat[10001] = "Código de ativação inválido";
            msgSat[10098] = "SAT em processamento.";
            msgSat[10099] = "Erro desconhecido Informar o administrador.";
            msgSat[11000] = "Emitido com sucesso Retorna o conteúdo do CF-ao AC.";
            msgSat[11001] = "código ativação inválido Verificar o código e tentar mais uma vez.";
            msgSat[11002] = "SAT ainda não ativado. Efetuar ativação.";
            msgSat[11003] = "Sessão não existe. AC deve executar a sessão novamente.";
            msgSat[11098] = "SAT em processamento.";
            msgSat[11099] = "Erro desconhecido. Informar o administrador.";
            msgSat[12000] = "Rede Configurada com Sucesso";
            msgSat[12001] = "código ativação inválido Verificar o código e tentar mais uma vez.";
            msgSat[12002] = "Dados fora do padrão a ser informado Corrigir dados";
            msgSat[12098] = "SAT em processamento.";
            msgSat[12099] = "Erro desconhecido Informar o administrador.";
            msgSat[13000] = "Assinatura do AC";
            msgSat[13001] = "código ativação inválido Verificar o código e tentar mais uma vez.";
            msgSat[13002] = "Erro de comunicação com a SEFAZ";
            msgSat[13003] = "Assinatura fora do padrão informado Corrigir dados";
            msgSat[13004] = "CNPJ da Software House + CNPJ do emitente assinado no campo “signAC” difere do informado no campo “CNPJvalue” Corrigir dados";
            msgSat[13098] = "SAT em processamento.";
            msgSat[13099] = "Erro desconhecido Informar o administrador.";
            msgSat[14000] = "Software Atualizado com Sucesso ";
            msgSat[14001] = "Código de ativação inválido.";
            msgSat[14002] = "Atualização em Andamento";
            msgSat[14003] = "Erro na atualização Não foi possível Atualizar o SAT.";
            msgSat[14004] = "Arquivo de atualização inválido";
            msgSat[14098] = "SAT em processamento.";
            msgSat[14099] = "Erro desconhecido Informar o administrador.";
            msgSat[15000] = "Transferência completa Arquivos de Logs extraídos";
            msgSat[15001] = "Código de ativação inválido.";
            msgSat[15002] = "Transferência em andamento";
            msgSat[15098] = "SAT em processamento.";
            msgSat[15099] = "Erro desconhecido Informar o administrador.";
            msgSat[16000] = "Equipamento SAT bloqueado com sucesso.";
            msgSat[16001] = "Código de ativação inválido.";
            msgSat[16002] = "Equipamento SAT já está bloqueado.";
            msgSat[16003] = "Erro de comunicação com a SEFAZ";
            msgSat[16004] = "Não existe parametrização de bloqueio disponível.";
            msgSat[16098] = "SAT em processamento.";
            msgSat[16099] = "Erro desconhecido Informar o administrador.";
            msgSat[17000] = "Equipamento SAT desbloqueado com sucesso.";
            msgSat[17001] = "Código de ativação inválido.";
            msgSat[17002] = "SAT bloqueado pelo contribuinte. Verifique configurações na SEFAZ";
            msgSat[17003] = "SAT bloqueado pela SEFAZ";
            msgSat[17004] = "Erro de comunicação com a SEFAZ";
            msgSat[17098] = "SAT em processamento.";
            msgSat[17099] = "Erro desconhecido Informar o administrador.";
            msgSat[18000] = "Código de ativação alterado com sucesso.";
            msgSat[18001] = "Código de ativação inválido.";
            msgSat[18002] = "Código de ativação de emergência Incorreto.";
            msgSat[18098] = "SAT em processamento.";
            msgSat[18099] = "Erro desconhecido Informar o administrador.";

            try
            {
                msg = msgSat[codigodeRetorno];
            }
            catch { }

            return msg;

        }

        private string retornoSAT(string chave)
        {
            string retorno = "";
            try
            {
                string[] lines = File.ReadAllLines(parametroSat("caminhosaitxt"));
                for (int i = 0; i < lines.Length; i++)
                {
                    if (lines[i].Contains(chave + "="))
                    {
                        retorno = lines[i].Substring(chave.Length + 1);
                    }
                }
            }
            catch (Exception ex)
            {
                retorno = "Erro SAT: " + ex;
                fmok.Mostrar(retorno);
            }

            return retorno;
        }
        private void btnfecharcupom_Click(object sender, EventArgs e)
        {
            if (venda.ven_id < 1)
            {
                fmok.Mostrar("Não há cupom para ser fechado!");
                return;
            }


            if (dtitemvenda.Rows.Count < 1)
            {
                falar("não é possivel fechar venda sem itens.");
                fmok.Mostrar("Não é possivel fechar uma venda sem itens!");
                return;
            }

            //if (ckbcpf.Text == "")
            //{
            //    FInput fi = new FInput();
            //    if (fi.Mostrar("CPF", "Informe o CPF do cliente!"))
            //    {
            //        ckbcpf.Text = fi.valor;
            //        cliente.cpf = ckbcpf.Text;
            //        // cliente.nome = "Cliente";
            //    }
            //    else
            //    {
            //        ckbcpf.Text = "";
            //        cliente.cpf = "";
            //        //cliente.nome = "";
            //    }

            //}

            //if (ckbentrega.Checked)
            //{
            //    if (cliente.id <= 0)
            //        btnpesquisarcliente.PerformClick();

            //    if (cliente.id <= 0)
            //    {
            //        fmok.Mostrar("Selecione um cliente para   fechar a venda!");
            //        return;
            //    }
            //}
            //if (ckbcpf.Checked)
            //{
            //    if (ckbcpf.Text == "CPF")
            //    {
            //        if (cliente.id < 1)
            //        {
            //            btnpesquisarcliente.PerformClick();

            //            if (cliente.id <= 0)
            //            {
            //                fmok.Mostrar("Selecione um cliente para fechar a venda!");
            //                return;
            //            }
            //        }
            //    }
            //    else
            //    {
            //        if (ckbcpf.Text == "")
            //        {
            //            FInput fi = new FInput();
            //            if (fi.Mostrar("CPF", "Informe o CPF do cliente!"))
            //            {
            //                ckbcpf.Text = fi.valor;
            //                cliente.cpf = ckbcpf.Text;
            //                // cliente.nome = "Cliente";
            //            }
            //            else
            //            {
            //                ckbcpf.Text = "";
            //                cliente.cpf = "";
            //                //cliente.nome = "";
            //            }

            //        }
            //    }
            //    //   btnpesquisarcliente.PerformClick();
            //}


            pnlcarregando.Visible = true;
            pnlcarregando.BringToFront();
            //Formularios.FPagamento f = new Formularios.FPagamento(venda.ven_id, 0, venda.ven_total, venda.ven_desconto);
            //f.ShowDialog();

            //if (f.ok)
            //{

            int sessao = criarSAT();
            //venda.atualizarCaixa(venda.ven_id, idcaixa);


            if (sessao == 0)
            {
                fmok.MostrarBotao();
                falar(msgsat2);
                fmok.Mostrar("Erro ao enviar venda ao SAT!");
                return;
            }


            venda.atualizarClienteFuncionario(venda.ven_id, cliente.id, idfuncionario);
            venda.fechar(venda.ven_id);
            /*
            PrintDialog pd = new PrintDialog();
            if (pd.ShowDialog() == DialogResult.OK)
            {

                PrintDocument printDocument = new PrintDocument();
                printDocument.PrintPage += PrintDocumentOnPrintPage;
                printDocument.PrinterSettings = pd.PrinterSettings;
                printDocument.Print();
            }*/
            // return;


            limparvenda();
            limpar();
            limparcliente();

            carregar();



            fmok.Mostrar("Venda fechada com sucesso!");
            //}
            //else
            carregar();

            pnlcarregando.Visible = false;
        }

        private void limparvenda()
        {
            venda = new CVenda();
            venda.ven_id = 0;
        }

        private void btnmovimentacao_Click(object sender, EventArgs e)
        {
            FSangriaSuprimento f = new FSangriaSuprimento(idfuncionario, idcaixa);
            f.ShowDialog();
        }

        private void btnrelatorio_Click(object sender, EventArgs e)
        {
            FRelatorio f = new FRelatorio();
            f.ShowDialog();
        }
        public static class NativeMethods
        {
            [DllImport("kernel32.dll", SetLastError = true, CallingConvention = CallingConvention.Winapi)]
            [return: MarshalAs(UnmanagedType.Bool)]
            internal static extern bool IsWow64Process([In] IntPtr process, [Out] out bool wow64Process);
        }
        public static bool IsWin64Emulator(Process process)
        {
            if ((Environment.OSVersion.Version.Major > 5)
                || ((Environment.OSVersion.Version.Major == 5) && (Environment.OSVersion.Version.Minor >= 1)))
            {
                bool retVal;

                return NativeMethods.IsWow64Process(process.Handle, out retVal) && retVal;
            }

            return false; // not on 64-bit Windows Emulator
        }

        public static string getOskPath(string dir)
        {
            string path = Path.Combine(dir, "osk.exe");
            if (File.Exists(path))
            {
                Process p = System.Diagnostics.Process.Start(path);
                //if (p.IsWin64Emulator())
                //{
                //    path = string.Empty;
                //}
                p.Kill();
                return path;
            }
            DirectoryInfo di = new DirectoryInfo(dir);
            foreach (DirectoryInfo subDir in di.GetDirectories().Reverse())
            {
                path = getOskPath(Path.Combine(dir, subDir.Name));
                if (!string.IsNullOrWhiteSpace(path))
                {
                    return path;
                }
            }
            return string.Empty;
        }

        private void btnexcluiritem_Click(object sender, EventArgs e)
        {
            FInput f = new FInput();
            //  f.Mostrar("Remover Item!", "Informe o número do item que deseja remover?");

            CItemVenda citemvenda = new CItemVenda();
            int iditem = 0;
            decimal quantidade = 0;
            decimal valorproduto = 0;
            int idproduto = 0;
            CProduto cproduto = new CProduto();

            try
            {
                int numeroinput = ultimoitem;// int.Parse(f.valor);

                if (ultimoitem > 0)
                {
                    iditem = int.Parse(gridproduto.Rows[numeroinput - 1].Cells["ite_id"].Value.ToString());
                    quantidade = decimal.Parse(gridproduto.Rows[numeroinput - 1].Cells["ite_quantidade"].Value.ToString());
                    valorproduto = decimal.Parse(gridproduto.Rows[numeroinput - 1].Cells["ite_valor"].Value.ToString());
                    iditem = int.Parse(gridproduto.Rows[numeroinput - 1].Cells["ite_id"].Value.ToString());
                    idproduto = int.Parse(gridproduto.Rows[numeroinput - 1].Cells["pro_id"].Value.ToString());

                    citemvenda.remover(iditem);
                    venda.adicionarvalor(venda.ven_id, ((quantidade * valorproduto)) * -1);

                    cproduto.baixarestoque(idproduto, quantidade * -1);
                }
                else
                {
                    fmok.Mostrar("NÃO HÁ ITENS A SEREM REMOVIDOS!!");
                }
            }
            catch { fmok.Mostrar("ERRO AO REMOVER PRODUTO!!"); };
            carregar();
        }

        private void btncancelarcliente_Click(object sender, EventArgs e)
        {
            limparcliente();
            venda.atualizarClienteFuncionario(venda.ven_id, 0, idfuncionario);
            carregar();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            timerSaudacao.Enabled = true;
            lblcaixanumero.Text = "Caixa: " + caixanumero.ToString("00") + " - " + DateTime.Now.ToShortTimeString();
            if (venda.cli_id > 0)
            {
                //  btncancelarcliente.Enabled = true;
                btnpesquisarcliente.Enabled = false;

            }
            else
            {

                btnpesquisarcliente.Enabled = true;
                //btncancelarcliente.Enabled = false;
            }

            if (caixanumero > 0 && idcaixa > 0)
                btnpesquisarcliente.Enabled = true;

            if (idcaixa < 1)
                btnpesquisarcliente.Enabled = false;

        }

        private void button1_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {

        }

        private void btnconfiguracoes_Click(object sender, EventArgs e)
        {
            FConfiguracao f = new FConfiguracao();
            f.ShowDialog();
        }

        private void btnfuncionario_Click(object sender, EventArgs e)
        {
            FFuncionario f = new FFuncionario();
            f.ShowDialog();
        }

        private void btnfecharcaixa_Click(object sender, EventArgs e)
        {
            CVenda venda = new CVenda();
            venda.pesquisarvendaaberta(idcaixa);
            if (venda.ven_status == 1)
            {
                fmok.Mostrar("Não é possível fechar o caixa com uma venda aberta!");
                return;
            }

            if (idcaixa == 0)
            {
                FAbrirCaixa abrircaixa = new FAbrirCaixa(idfuncionario);
                abrircaixa.ShowDialog();
                if (abrircaixa.ok)
                {
                    idcaixa = abrircaixa.idcaixa;
                }
            }
            else
            {
                FFecharCaixa1 f = new FFecharCaixa1(idfuncionario, idcaixa);
                f.ShowDialog();

                if (f.ok)
                {
                    idcaixa = 0;
                }

            }

            if (idcaixa == 0)
            {
                btniniciarcupom.Enabled = false;
                btnpesquisarcliente.Enabled = false;
                //   btnfecharcupom.Enabled = false;
                btncancelarcupom.Enabled = false;
                //  btnexcluiritem.Enabled = false;
                btnprevenda.Enabled = false;
                btnfecharcaixa.Text = "Abrir Caixa";
                pnlpesquisar.Visible = false;
                btnpnlpesquisar.Enabled = false;

            }
            else
            {
                btniniciarcupom.Enabled = true;
                btnpesquisarcliente.Enabled = true;
                // btnfecharcupom.Enabled = true;
                btncancelarcupom.Enabled = true;
                //  btnexcluiritem.Enabled = true;
                btnprevenda.Enabled = true;
                btnfecharcaixa.Text = "Fechar Caixa";
                btnpnlpesquisar.Enabled = true;
            }







        }

        private void ckbcpf_CheckedChanged(object sender, EventArgs e)
        {
            //if (ckbcpf.Checked)
            //{
            //    //if (cliente.id <= 0)
            //    //  btnpesquisarcliente.PerformClick();


            //    if (cliente.id < 1)
            //    {
            //        FInput f = new FInput();
            //        if (f.Mostrar("CPF", "Informe o CPF do cliente!"))
            //        {
            //            //ckbcpf.Text = f.valor;
            //            //cliente.cpf = ckbcpf.Text;
            //            //ckbcpf.Checked = true;
            //            //cliente.nome = "Cliente";
            //        }
            //        else
            //        {
            //          //  ckbcpf.Text = "CPF";
            //            cliente.cpf = "";
            //            //cliente.nome = "";
            //        }

            //    }
            //    //if (cliente.id <= 0)
            //    //  btnpesquisarcliente.PerformClick();
            //}
            //else
            //{
            //   // ckbcpf.Text = "CPF";
            //}
        }

        private void ckbentrega_CheckedChanged(object sender, EventArgs e)
        {
            //if (ckbentrega.Checked)
            //{
            //    if (cliente.id <= 0)
            //        btnpesquisarcliente.PerformClick();
            //}
        }

        private void btncontasreceber_Click(object sender, EventArgs e)
        {
            FContaReceber f = new FContaReceber(idcaixa);
            f.ShowDialog();
        }

        private void timercarregando_Tick(object sender, EventArgs e)
        {

        }

        private void ttbproduto_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            { limpar(); }

            if (e.KeyCode == Keys.Up)
            {
                if (pnlpesquisarproduto.Visible)
                {
                    try
                    {
                        if (gridpesquisarproduto.CurrentRow != null)
                            gridpesquisarproduto.CurrentCell =
                                gridpesquisarproduto
                                .Rows[Math.Min(gridpesquisarproduto.CurrentRow.Index - 1, gridpesquisarproduto.Rows.Count - 1)]
                                .Cells[1];
                    }
                    catch
                    {

                    }
                }
            }
            if (e.KeyCode == Keys.Down)
            {
                if (pnlpesquisarproduto.Visible)
                {
                    try
                    {
                        if (gridpesquisarproduto.CurrentRow != null)
                            gridpesquisarproduto.CurrentCell =
                                gridpesquisarproduto
                                .Rows[Math.Min(gridpesquisarproduto.CurrentRow.Index + 1, gridpesquisarproduto.Rows.Count - 1)]
                                .Cells[1];

                        //MessageBox.Show(dtpesquisarproduto.Rows[gridpesquisarproduto.CurrentRow.Index][1].ToString());
                    }
                    catch
                    {

                    }
                }
            }
        }

        private void ttbproduto_TextChanged(object sender, EventArgs e)
        {
            if (ttbproduto.Text != "")
            {
                NI = 0;
                CCupom cupom = new CCupom();
                decimal total = cupom.RetornaNumeroTotalDeCupons();
                if (total == 0) ContaItem = 0;

                if (LimitarItens == "sim" && ttbproduto.Text != "")
                {
                    NI = Convert.ToInt32(LimitarNumItens);
                }
                else
                {
                    NI = 1000;
                }


                if (ContaItem < NI)
                {
                    if (idcaixa == 0)
                        return;
                    if (ttbproduto.Text.Trim() != "")
                    {
                        dtpesquisarproduto = produto.pesquisarCodigo(ttbproduto.Text, "");

                        gridpesquisarproduto.DataSource = dtpesquisarproduto;

                        int teste = gridpesquisarproduto.Rows.Count;

                        try
                        {
                            long.Parse(ttbproduto.Text);
                            pnlpesquisarproduto.Visible = true;
                            pnlpesquisarproduto.BringToFront();
                        }
                        catch (Exception ex)
                        {
                            pnlpesquisarproduto.Visible = true;
                            pnlpesquisarproduto.BringToFront();

                        }

                        if (ttbproduto.Text.Length == 13)
                        {
                            pnlpesquisarproduto.Visible = false;

                            bool barras = false;
                            try
                            {
                                long.Parse(ttbproduto.Text);
                                barras = true;
                            }
                            catch { }

                            string quantidade = "1";
                            try
                            {
                                produto.pesquisarid(int.Parse(gridpesquisarproduto.CurrentRow.Cells[0].Value.ToString()));
                                mostrarproduto();
                            }
                            catch (Exception ex)
                            {
                                fmok.Mostrar("Erro ao incluir Produdo " + ex);
                                limpar();
                            }

                            #region LENDO BALANÇA
                            if (UtilizarBalan == "nao")
                            {
                                Bobina2.Items.Add(ItemCupom1);
                                LinhaCupom++;

                                Bobina2.Items.Add(ItemCupom2);
                                LinhaCupom++;
                            }

                            if (UtilizarBalan == "sim")
                            {
                                timer2.Enabled = true;
                                //  timervalidasacola.Enabled = true;
                                falar(Mensagem4);
                                fmok.EsconderBotao();
                                fmok.Mostrar(Mensagem4);

                                try
                                {
                                    if (peso != 0)
                                    {
                                        produtonovo = 1;
                                        pesototal = peso;
                                        Bobina2.Items.Add(ItemCupom1);
                                        LinhaCupom++;

                                        Bobina2.Items.Add(ItemCupom2);
                                        LinhaCupom++;
                                    }

                                }
                                catch
                                {

                                }
                            }
                            #endregion

                            ttbquantidade.Focus();
                            if (barras)
                            {
                                //   lblpeso.Text = ContaItem.ToString();
                                ttbquantidade.Text = quantidade;
                                vender();
                            }

                        }

                    }
                    else
                    {
                        pnlpesquisarproduto.Visible = false;
                    }

                    if (SomenteCartao == "sim")
                    {
                        tileItem1.Visible = true;
                    }
                    else
                    {
                        tileItem1.Visible = false;
                        tileItem6.Visible = true;
                    }

                    ContaItem++;
                }
                else
                {
                    ttbproduto.Text = string.Empty;
                    fmok.Mostrar("Limite de Itens a serem vendidos foram excedidos! - Limite: " + LimitarNumItens);
                }
            }
        }

        private void label9_Click(object sender, EventArgs e)
        {

        }

        private void btnbackup_Click(object sender, EventArgs e)
        {
            // Close();
            // }
            // else return;// Close();
            //if (MessageBox.Show("O Procedimento finalizará o sistema para continuação do Backup, Confirma?", "Backup", MessageBoxButtons.YesNo) == DialogResult.Yes)
            //{
            //    System.Diagnostics.Process.Start(Application.StartupPath + "\\Backup.exe", "mgm");
            //    Close();
            //}
        }

        private void lblprodutoestoque_Click(object sender, EventArgs e)
        {

        }

        public static bool validarCPF(string CPF)
        {
            int[] mt1 = new int[9] { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] mt2 = new int[10] { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            string TempCPF;
            string Digito;
            int soma;
            int resto;

            CPF = CPF.Trim();
            CPF = CPF.Replace(".", "").Replace("-", "");

            if (CPF.Length != 11)
                return false;

            TempCPF = CPF.Substring(0, 9);
            soma = 0;
            for (int i = 0; i < 9; i++)
                soma += int.Parse(TempCPF[i].ToString()) * mt1[i];

            resto = soma % 11;
            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;

            Digito = resto.ToString();
            TempCPF = TempCPF + Digito;
            soma = 0;

            for (int i = 0; i < 10; i++)
                soma += int.Parse(TempCPF[i].ToString()) * mt2[i];

            resto = soma % 11;
            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;

            Digito = Digito + resto.ToString();

            return CPF.EndsWith(Digito);
        }

        private void tileItem6_ItemPress(object sender, DevExpress.XtraEditors.TileItemEventArgs e)
        {

            if (dtitemvenda.Rows.Count < 1)
            {
                fmok.Mostrar("Não é possivel fechar uma venda sem itens!");
                return;
            }

            if (cliente.id <= 0)
                btnpesquisarcliente.PerformClick();

            falar("Confirme o valor e Informe sua forma de pagamento.");

            pnlcarregando.Visible = true;
            pnlcarregando.BringToFront();
            Formularios.FPagamento f = new Formularios.FPagamento(venda.ven_id, 0, venda.ven_total, venda.ven_desconto);
            f.ShowDialog();// return;

            tipopag = f.Ptipopag;

            CVenda Vvenda = new CVenda();
            parcelas = f.Parcelas;
            numerocupom = Vvenda.pesquisarIdVendaAtual().ToString();// acBrECF1.NumCupom; //numerocupom do cupom atual

            if (TEFDLL == "nao")
            {
                RequisitaTEF(venda.ven_id.ToString(), numerocupom, venda.ven_total.ToString(), parcelas);
            }

            if (!f.ok)
            {
                return;
            }





            if (TEFDLL == "nao")
            {
                while (!File.Exists(Caminhoresp + "INTPOS.001"))
                {
                    string a = "0";
                }

                if (File.Exists(Caminhoresp + "INTPOS.001"))
                {
                    var chave = RetornaChaveIntPos(Caminhoresp + "INTPOS.001", "009-000");
                    chave = chave.Trim();
                    chave = chave.Substring(10, chave.Length - 10).Trim();//009-000 = 0

                    if (chave == "0")
                    {
                        int sessao = criarSAT();

                        if (sessao == 0 && satmon == "nao")
                        {
                            fmok.Mostrar("Erro ao enviar venda ao SAT!");
                            return;
                        }


                        venda.atualizarClienteFuncionario(venda.ven_id, cliente.id, idfuncionario);
                        venda.fechar(venda.ven_id);

                        limparvenda();
                        limpar();
                        limparcliente();

                        carregar();

                        textBoxCupom.Text = string.Empty;
                        string DataHoje = DateTime.Now.ToString().Replace("/", "-").Substring(0, 10);
                        string HoraHoje = DateTime.Now.ToString();
                        DataHoje = DataHoje.Substring(6, 4) + "-" + DataHoje.Substring(3, 2) + "-" + DataHoje.Substring(0, 2);
                        HoraHoje = HoraHoje.Substring(11, 8);

                        Bobina2.Items.Clear();

                        CItemVenda itemVenda = new CItemVenda();
                        CMeioPagamento Pagamento = new CMeioPagamento();
                        CVenda Venda = new CVenda();
                        CFuncionario Funcionario = new CFuncionario();

                        ImprimeCabecalhoBobina();

                        string FormaPag = RetornaChaveIntPos(Caminhoresp + "Intpos.001", "018-000");
                        FormaPag = FormaPag.Trim();
                        FormaPag = FormaPag.Substring(10, FormaPag.Length - 10);
                        if (FormaPag == "01")
                        {
                            FormaPag = "Cartão de Débito";
                        }
                        else
                        {
                            FormaPag = "Cartão de Crédito";
                        }

                        string VendaTotal = RetornaChaveIntPos(Caminhoresp + "Intpos.001", "003-000");
                        VendaTotal = VendaTotal.Trim();
                        VendaTotal = VendaTotal.Substring(10, VendaTotal.Length - 10);

                        Pagamento.pesquisaIdCartao(FormaPag);
                        int IdVenda = Venda.pesquisarIdVendaAtual();
                        Venda.pesquisarvendaIdVenda(IdVenda);

                        string vlt = VendaTotal.Replace(",", "."); // Convert.ToString(venda.ven_total);
                        string vlr = VendaTotal.Replace(",", ".");
                        string dpg = DataHoje;// Convert.ToString(Venda.ven_data);
                        string tpg = Pagamento.mei_tpg;
                        string vsq = "0.00";
                        string frc = "S";
                        string vtr = "0.00";
                        string cac = Pagamento.mei_cac;
                        string cav = "S";
                        string aut = "0";
                        string nsh = "0";
                        string nsu = "0";
                        string vtp = "0";
                        string ntp = "0";
                        string ope = idfuncionario.ToString();

                        if (File.Exists(Caminhoresp + "Intpos.001"))
                        {
                            string Parcelas = RetornaChaveIntPos(Caminhoresp + "Intpos.001", "018-000");
                            Parcelas = Parcelas.Trim();
                            Parcelas = Parcelas.Substring(10, Parcelas.Length - 10);
                            if (Parcelas == "01") cav = "S";
                            if (Parcelas != "01") cav = "N";
                        }

                        //  ImprimeArqIntpos(Caminhoresp + "Intpos.001");
                        if (UtilizarImpre == "sim")
                        {
                            DesativarECF(); // desativa impressora liberando a porta

                            PrintDialog pd = new PrintDialog();
                            feedAndCutter(pd.PrinterSettings.PrinterName, 5);

                            ImprimeArqIntpos(Caminhoresp + "Intpos.001");

                            Thread.Sleep(3000);
                            ativarECF(); // ativa novamente a impressora
                        }

                        GerarRodapeArquivo(CaminhoDadosRep, NomeArquivo, tpg, vlt, vsq, frc, vtr, cac, vlr, cav, dpg, aut, nsh, nsu, vtp, ntp, ope);

                        if (FlagSudacao == 1)
                        {
                            falar(Mensagem6);
                        }

                        ContaItem = 1;
                        FlagSudacao = 0;

                        tileItem1.Visible = false;
                        tileItem6.Visible = false;

                        mostrarimagemproduto("9999999999999");
                        label16.Text = "Passe o produto pelo Leitor";

                        PainelMerchan merchan = new PainelMerchan();
                        merchan.Show();



                        //  fmok.Mostrar("Venda fechada com sucesso!");
                    }
                    else
                    {
                        chave = RetornaChaveIntPos(Caminhoresp + "INTPOS.001", "030-000");
                        chave = chave.Trim().Substring(9, chave.Length - 12).Trim();//030-000 = Valor fora do limite
                        fmok.Mostrar(chave);

                        carregar();

                        pnlcarregando.Visible = false;
                    }
                }
            }

            if (TEFDLL == "sim")
            {
                int retorno = clsTEF.Configura(IPTef, LojaTef, TerminalTef);
                if (sender == null)
                {
                    fmok.Mostrar("Problema de Conexão  com o TEF: [" + retorno.ToString() + "]");
                    return;
                }

                int sessao = criarSAT();

                if (sessao == 0 && satmon == "nao")
                {
                    fmok.Mostrar("Erro ao enviar venda ao SAT!");
                    return;
                }


                venda.atualizarClienteFuncionario(venda.ven_id, cliente.id, idfuncionario);
                venda.fechar(venda.ven_id);
                int idv = venda.pesquisarIdVendaAtual();
                decimal valor = venda.pesquisarvendaTotal(idv);// ven_total.ToString();
         //       string valor2 = valor.
                limparvenda();
                limpar();
                limparcliente();

                carregar();

                textBoxCupom.Text = string.Empty;
                string DataHoje = DateTime.Now.ToString().Replace("/", "-").Substring(0, 10);
                string HoraHoje = DateTime.Now.ToString();
                DataHoje = DataHoje.Substring(6, 4) + "-" + DataHoje.Substring(3, 2) + "-" + DataHoje.Substring(0, 2);
                HoraHoje = HoraHoje.Substring(11, 8);

                Bobina2.Items.Clear();

                CItemVenda itemVenda = new CItemVenda();
                CMeioPagamento Pagamento = new CMeioPagamento();
                CVenda Venda = new CVenda();
                CFuncionario Funcionario = new CFuncionario();

                ImprimeCabecalhoBobina();

                string VendaTotal = valor.ToString().Replace(",",".");
                int IdVenda = 0;

                if (TEFDLL == "nao")
                {
                    string FormaPag = RetornaChaveIntPos(Caminhoresp + "Intpos.001", "018-000");
                    FormaPag = FormaPag.Trim();
                    FormaPag = FormaPag.Substring(10, FormaPag.Length - 10);
                    if (FormaPag == "01")
                    {
                        FormaPag = "Cartão de Débito";
                    }
                    else
                    {
                        FormaPag = "Cartão de Crédito";
                    }

                    VendaTotal = RetornaChaveIntPos(Caminhoresp + "Intpos.001", "003-000");
                    VendaTotal = VendaTotal.Trim();
                    VendaTotal = VendaTotal.Substring(10, VendaTotal.Length - 10);

                    Pagamento.pesquisaIdCartao(FormaPag);
                    IdVenda = Venda.pesquisarIdVendaAtual();
                    Venda.pesquisarvendaIdVenda(IdVenda);
                }

                string  vlt = VendaTotal;// Convert.ToString(venda.ven_total);
                string vlr =  VendaTotal;
                string dpg = DataHoje;// Convert.ToString(Venda.ven_data);
                string tpg = tipopag;// Pagamento.mei_tpg;
                string vsq = "0.00";
                string frc = "S";
                string vtr = "0.00";
                string cac = tipopag;// Pagamento.mei_cac;
                string cav = "S";
                string aut = "0";
                string nsh = "0";
                string nsu = "0";
                string vtp = "0";
                string ntp = "0";
                string ope = idfuncionario.ToString();

                if (TEFDLL == "nao")
                {
                    if (File.Exists(Caminhoresp + "Intpos.001"))
                    {
                        Parcelas = RetornaChaveIntPos(Caminhoresp + "Intpos.001", "018-000");
                        Parcelas = Parcelas.Trim();
                        Parcelas = Parcelas.Substring(10, Parcelas.Length - 10);
                        if (Parcelas == "01") cav = "S";
                        if (Parcelas != "01") cav = "N";
                    }
                }

                //  ImprimeArqIntpos(Caminhoresp + "Intpos.001");
                if (UtilizarImpre == "sim")
                {
                    DesativarECF(); // desativa impressora liberando a porta

                    PrintDialog pd = new PrintDialog();
                    feedAndCutter(pd.PrinterSettings.PrinterName, 5);

                    if (TEFDLL == "nao")
                    {
                        ImprimeArqIntpos(Caminhoresp + "Intpos.001");
                    }
                    else
                    {
                        ImprimeArqIntpos("COLOCAR A IMPRESSAO AQUI DA DLL"); // formato personalizado para termicas
                    }

                    Thread.Sleep(3000);
                    ativarECF(); // ativa novamente a impressora
                }

                GerarRodapeArquivo(CaminhoDadosRep, NomeArquivo, tpg, vlt, vsq, frc, vtr, cac, vlr, cav, dpg, aut, nsh, nsu, vtp, ntp, ope);

                if (FlagSudacao == 1)
                {
                    falar(Mensagem6);
                }

                ContaItem = 1;
                FlagSudacao = 0;

                tileItem1.Visible = false;
                tileItem6.Visible = false;

                mostrarimagemproduto("9999999999999");
                label16.Text = "Passe o produto pelo Leitor";

                PainelMerchan merchan = new PainelMerchan();
                merchan.Show();



                //  fmok.Mostrar("Venda fechada com sucesso!");
            }






            //int sessao = criarSAT();

            //if (sessao == 0 && satmon == "nao")
            //{
            //    fmok.Mostrar("Erro ao enviar venda ao SAT!");
            //    return;
            //}


            //venda.atualizarClienteFuncionario(venda.ven_id, cliente.id, idfuncionario);
            //venda.fechar(venda.ven_id);

            //limparvenda();
            //limpar();
            //limparcliente();

            //carregar();

            //if (UtilizarImpre == "sim")
            //{
            //    DesativarECF(); // desativa impressora liberando a porta

            //    PrintDialog pd = new PrintDialog();
            //    feedAndCutter(pd.PrinterSettings.PrinterName, 5);

            //    Thread.Sleep(3000);
            //    ativarECF(); // ativa novamente a impressora
            //}
            //textBoxCupom.Text = string.Empty;


            //Bobina2.Items.Clear();
            //ImprimeCabecalhoBobina();

            //falar(Mensagem6);

            //tileItem1.Visible = false;
            //tileItem6.Visible = false;

            //mostrarimagemproduto("9999999999999");
            //label16.Text = "Passe o produto pelo Leitor";

            //PainelMerchan merchan = new PainelMerchan();
            //merchan.Show();

            //pnlcarregando.Visible = false;
        }

        private void tileItem1_ItemPress(object sender, EventArgs e)
        {
            if (TEFDLL == "nao")
            {
                if (File.Exists(Caminhoresp + "INTPOS.001"))
                {
                    File.Delete(Caminhoresp + "INTPOS.001");
                    File.Delete(Caminhoresp + "INTPOS.STS");
                }
            }

            pnlcarregando.Visible = true;
            pnlcarregando.BringToFront();

            bool validado = false;
            //ckbcpf.Text = "";
            if (venda.ven_id < 1)
            {
                fmok.Mostrar("Não há cupom para ser fechado!");
                return;
            }

            if (gridproduto.RowCount < 1)
            {
                fmok.Mostrar("Não há cupom para ser fechado!");
                return;
            }

            FMessageSimNao CP = new FMessageSimNao();
            if (CP.Mostrar("CPF", "Deseja informar o CPF ?"))
            {
                FInput fi = new FInput();
                if (fi.Mostrar("CPF", "Informe o CPF do cliente!"))
                {
                    CPFSAT = fi.valor;
                };
                ////if (validado == false)
                ////{
                ////    falar("CPF inválido. digite novamente");
                ////    FInput fi = new FInput();
                ////    if (fi.Mostrar("CPF", "Informe o CPF do cliente!"))
                ////    {
                ////        CPFSAT = fi.valor;
                ////    }
                ////    else
                ////    {
                ////        //ckbcpf.Text = "";
                ////        cliente.cpf = "";
                ////        //cliente.nome = "";
                ////    }
                ////    if (validado == false)
                ////    {
                ////        falar("CPF inválido. digite novamente");
                ////        FInput fi2 = new FInput();
                ////        if (fi2.Mostrar("CPF", "Informe o CPF do cliente!"))
                ////        {
                ////            CPFSAT = fi.valor;
                ////        }
                ////        else
                ////        {
                ////            //ckbcpf.Text = "";
                ////            return;
                ////        }
                ////    }

                ////}
                //else
                //{

                //}

            }

            if (dtitemvenda.Rows.Count < 1)
            {
                fmok.Mostrar("Não é possivel fechar uma venda sem itens!");
                return;
            }


            parcelas = "1";
            CItemVenda ItemVenda = new CItemVenda();
            CupomAtual = ItemVenda.pesquisarproxcupom().ToString();
            numerocupom = CupomAtual;// numerocupom do cupom atual

            if (TEFDLL == "nao")
            {
                RequisitaTEF(venda.ven_id.ToString(), numerocupom, venda.ven_total.ToString(), parcelas);
            }

            falar("Escolha seu cartão de crédito ou débito.");

            validado = true;

            if (TEFDLL == "nao")
            {
                while (!File.Exists(Caminhoresp + "INTPOS.001"))
                {
                    string a = "0";
                }

                if (File.Exists(Caminhoresp + "INTPOS.001"))
                {
                    var chave = RetornaChaveIntPos(Caminhoresp + "INTPOS.001", "009-000");
                    chave = chave.Trim();
                    chave = chave.Substring(10, chave.Length - 10).Trim();//009-000 = 0

                    if (chave == "0")
                    {
                        int sessao = criarSAT();

                        if (sessao == 0 && satmon == "nao")
                        {
                            fmok.Mostrar("Erro ao enviar venda ao SAT!");
                            return;
                        }


                        venda.atualizarClienteFuncionario(venda.ven_id, cliente.id, idfuncionario);
                        venda.fechar(venda.ven_id);

                        limparvenda();
                        limpar();
                        limparcliente();

                        carregar();

                        textBoxCupom.Text = string.Empty;
                        string DataHoje = DateTime.Now.ToString().Replace("/", "-").Substring(0, 10);
                        string HoraHoje = DateTime.Now.ToString();
                        DataHoje = DataHoje.Substring(6, 4) + "-" + DataHoje.Substring(3, 2) + "-" + DataHoje.Substring(0, 2);
                        HoraHoje = HoraHoje.Substring(11, 8);

                        Bobina2.Items.Clear();

                        CItemVenda itemVenda = new CItemVenda();
                        CMeioPagamento Pagamento = new CMeioPagamento();
                        CVenda Venda = new CVenda();
                        CFuncionario Funcionario = new CFuncionario();

                        ImprimeCabecalhoBobina();

                        string FormaPag = RetornaChaveIntPos(Caminhoresp + "Intpos.001", "018-000");
                        FormaPag = FormaPag.Trim();
                        FormaPag = FormaPag.Substring(10, FormaPag.Length - 10);
                        if (FormaPag == "01")
                        {
                            FormaPag = "Cartão de Débito";
                        }
                        else
                        {
                            FormaPag = "Cartão de Crédito";
                        }

                        string VendaTotal = RetornaChaveIntPos(Caminhoresp + "Intpos.001", "003-000");
                        VendaTotal = VendaTotal.Trim();
                        VendaTotal = VendaTotal.Substring(10, VendaTotal.Length - 10);

                        Pagamento.pesquisaIdCartao(FormaPag);
                        int IdVenda = Venda.pesquisarIdVendaAtual();
                        Venda.pesquisarvendaIdVenda(IdVenda);

                        string vlt = VendaTotal.Replace(",", "."); // Convert.ToString(venda.ven_total);
                        string vlr = VendaTotal.Replace(",", ".");
                        string dpg = DataHoje;// Convert.ToString(Venda.ven_data);
                        string tpg = Pagamento.mei_tpg;
                        string vsq = "0.00";
                        string frc = "S";
                        string vtr = "0.00";
                        string cac = Pagamento.mei_cac;
                        string cav = "S";
                        string aut = "0";
                        string nsh = "0";
                        string nsu = "0";
                        string vtp = "0";
                        string ntp = "0";
                        string ope = idfuncionario.ToString();

                        if (File.Exists(Caminhoresp + "Intpos.001"))
                        {
                            string Parcelas = RetornaChaveIntPos(Caminhoresp + "Intpos.001", "018-000");
                            Parcelas = Parcelas.Trim();
                            Parcelas = Parcelas.Substring(10, Parcelas.Length - 10);
                            if (Parcelas == "01") cav = "S";
                            if (Parcelas != "01") cav = "N";
                        }

                        //  ImprimeArqIntpos(Caminhoresp + "Intpos.001");
                        if (UtilizarImpre == "sim")
                        {
                            DesativarECF(); // desativa impressora liberando a porta

                            PrintDialog pd = new PrintDialog();
                            feedAndCutter(pd.PrinterSettings.PrinterName, 5);

                            ImprimeArqIntpos(Caminhoresp + "Intpos.001");

                            Thread.Sleep(3000);
                            ativarECF(); // ativa novamente a impressora
                        }

                        GerarRodapeArquivo(CaminhoDadosRep, NomeArquivo, tpg, vlt, vsq, frc, vtr, cac, vlr, cav, dpg, aut, nsh, nsu, vtp, ntp, ope);

                        if (FlagSudacao == 1)
                        {
                            falar(Mensagem6);
                        }

                        ContaItem = 1;
                        FlagSudacao = 0;

                        tileItem1.Visible = false;
                        tileItem6.Visible = false;

                        mostrarimagemproduto("9999999999999");
                        label16.Text = "Passe o produto pelo Leitor";

                        PainelMerchan merchan = new PainelMerchan();
                        merchan.Show();



                        //  fmok.Mostrar("Venda fechada com sucesso!");
                    }
                    else
                    {
                        chave = RetornaChaveIntPos(Caminhoresp + "INTPOS.001", "030-000");
                        chave = chave.Trim().Substring(9, chave.Length - 12).Trim();//030-000 = Valor fora do limite
                        fmok.Mostrar(chave);

                        carregar();

                        pnlcarregando.Visible = false;
                    }
                }
            }

            if (TEFDLL == "sim")
            {
                int retorno = clsTEF.Configura(IPTef, LojaTef, TerminalTef);
                if (sender == null)
                {
                    fmok.Mostrar("Problema de Conexão  com o TEF: [" + retorno.ToString() + "]");
                    return;
                }

                #region Pagamento Simulado

                Formularios.FPagamento f = new Formularios.FPagamento(venda.ven_id, 0, venda.ven_total, venda.ven_desconto);
                f.ShowDialog();// return;

                tipopag = f.Ptipopag;

                label16.Text = "Passe o produto pelo Leitor";
                #endregion

                if (!f.ok)
                {
                    return;
                }

                int sessao = criarSAT();

                if (sessao == 0 && satmon == "nao")
                {
                    fmok.Mostrar("Erro ao enviar venda ao SAT!");
                    return;
                }


                venda.atualizarClienteFuncionario(venda.ven_id, cliente.id, idfuncionario);
                venda.fechar(venda.ven_id);

                //limparvenda();
                //limpar();
                //limparcliente();

                //carregar();

                textBoxCupom.Text = string.Empty;
                string DataHoje = DateTime.Now.ToString().Replace("/", "-").Substring(0, 10);
                string HoraHoje = DateTime.Now.ToString();
                DataHoje = DataHoje.Substring(6, 4) + "-" + DataHoje.Substring(3, 2) + "-" + DataHoje.Substring(0, 2);
                HoraHoje = HoraHoje.Substring(11, 8);

                Bobina2.Items.Clear();

                CItemVenda itemVenda = new CItemVenda();
                CMeioPagamento Pagamento = new CMeioPagamento();
                CVenda Venda = new CVenda();
                CFuncionario Funcionario = new CFuncionario();

                ImprimeCabecalhoBobina();

                string VendaTotal = "";
                int IdVenda = 0;

                if (TEFDLL == "nao")
                {
                    string FormaPag = RetornaChaveIntPos(Caminhoresp + "Intpos.001", "018-000");
                    FormaPag = FormaPag.Trim();
                    FormaPag = FormaPag.Substring(10, FormaPag.Length - 10);
                    if (FormaPag == "01")
                    {
                        FormaPag = "Cartão de Débito";
                    }
                    else
                    {
                        FormaPag = "Cartão de Crédito";
                    }

                    VendaTotal = RetornaChaveIntPos(Caminhoresp + "Intpos.001", "003-000");
                    VendaTotal = VendaTotal.Trim();
                    VendaTotal = VendaTotal.Substring(10, VendaTotal.Length - 10);

                    Pagamento.pesquisaIdCartao(FormaPag);
                    IdVenda = Venda.pesquisarIdVendaAtual();
                    Venda.pesquisarvendaIdVenda(IdVenda);
                }

                string vlt = VendaTotal;// Convert.ToString(venda.ven_total);
                string vlr = VendaTotal;
                string dpg = DataHoje;// Convert.ToString(Venda.ven_data);
                string tpg = Pagamento.mei_tpg;
                string vsq = "0.00";
                string frc = "S";
                string vtr = "0.00";
                string cac = Pagamento.mei_cac;
                string cav = "S";
                string aut = "0";
                string nsh = "0";
                string nsu = "0";
                string vtp = "0";
                string ntp = "0";
                string ope = idfuncionario.ToString();

                if (TEFDLL == "nao")
                {
                    if (File.Exists(Caminhoresp + "Intpos.001"))
                    {
                        Parcelas = RetornaChaveIntPos(Caminhoresp + "Intpos.001", "018-000");
                        Parcelas = Parcelas.Trim();
                        Parcelas = Parcelas.Substring(10, Parcelas.Length - 10);
                        if (Parcelas == "01") cav = "S";
                        if (Parcelas != "01") cav = "N";
                    }
                }

                //  ImprimeArqIntpos(Caminhoresp + "Intpos.001");
                if (UtilizarImpre == "sim")
                {
                    DesativarECF(); // desativa impressora liberando a porta

                    PrintDialog pd = new PrintDialog();
                    feedAndCutter(pd.PrinterSettings.PrinterName, 5);

                    if (TEFDLL == "nao")
                    {
                        ImprimeArqIntpos(Caminhoresp + "Intpos.001");
                    }
                    else
                    {
                        ImprimeArqIntpos("COLOCAR A IMPRESSAO AQUI DA DLL"); // formato personalizado para termicas
                    }

                    Thread.Sleep(3000);
                    ativarECF(); // ativa novamente a impressora
                }

                GerarRodapeArquivo(CaminhoDadosRep, NomeArquivo, tpg, vlt, vsq, frc, vtr, cac, vlr, cav, dpg, aut, nsh, nsu, vtp, ntp, ope);

                if (FlagSudacao == 1)
                {
                    falar(Mensagem6);
                }

                ContaItem = 1;
                FlagSudacao = 0;

                tileItem1.Visible = false;
                tileItem6.Visible = false;

                mostrarimagemproduto("9999999999999");
                label16.Text = "Passe o produto pelo Leitor";

                PainelMerchan merchan = new PainelMerchan();
                merchan.Show();



                //  fmok.Mostrar("Venda fechada com sucesso!");
            }


            limparvenda();
            limpar();
            limparcliente();

            carregar();

            pnlcarregando.Visible = false;
        }

        public void RequisitaTEF(string vendaID, string numCupom, string valor, string parcela)
        {
            if (!File.Exists(Caminhoreq + "INTPOS.001"))
            {
                CriarPasta(Caminhoreq, "INTPOS.001", INTPOS);
            }
            valor = Convert.ToString(Convert.ToDecimal(valor) * 100);
            valor = valor.Replace(",", ".");
            INTPOS = "000-000 = CRT" + @"
001-000 = " + vendaID + @"
002-000 = " + numCupom + @"
003-000 = " + valor + @"
017-000 = 1" + @"
018-000 = " + parcela + @" 
999-999 = 0";
            //  Caminhoreq = Caminhoreq + "INTPOS.001";
            try
            {
                System.IO.File.WriteAllText(Caminhoreq + "INTPOS.001", INTPOS);
            }
            catch
            {

            }
        }
        public void feedAndCutter(string printerName, int numLines)
        {
            EscPos esc = new EscPos();
            System.Threading.Thread.Sleep(500);
            esc.lineFeed(printerName, numLines);
            esc.CutPaper(printerName);
        }

        private void tileItem2_ItemPress(object sender, EventArgs e)
        {

        }

        private void CALLBUTTON(Button BTN)
        {
            ttbvalor.Text = ttbvalor.Text + BTN.Text;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            CALLBUTTON(button1);
        }

        private void tileItem2_ItemPress(object sender, DevExpress.XtraEditors.TileItemEventArgs e)
        {

            FInputValor f = new FInputValor();
            f.Mostrar("Identificação!", "Informe o código de identificação!");
            string codigo = f.valor;
            if (f.valor == "") return;

            CFuncionario funcionario = new CFuncionario();
            DataTable Retorno = funcionario.pesquisar(codigo, "informacao");
            int retornados = Retorno.Rows.Count;

            if (retornados > 0)
            {
                CItemVenda citemvenda = new CItemVenda();
                int iditem = 0;
                int itematual = 0;
                decimal quantidade = 0;
                string descricao = "";
                decimal valorproduto = 0;
                int idproduto = 0;
                CProduto cproduto = new CProduto();
                CCupom cupom = new CCupom();
                itematual = cupom.pesquisaTotalCupom();
                int TT = gridproduto.Rows.Count;
                ContaItem--;
                try
                {
                    int numeroinput = TT;// ultimoitem;// int.Parse(f.valor);
                    if (numeroinput > 0)
                    {

                        iditem = int.Parse(gridproduto.Rows[numeroinput - 1].Cells["ite_id"].Value.ToString());
                        quantidade = decimal.Parse(gridproduto.Rows[numeroinput - 1].Cells["ite_quantidade"].Value.ToString());
                        valorproduto = decimal.Parse(gridproduto.Rows[numeroinput - 1].Cells["ite_valor"].Value.ToString());
                        iditem = int.Parse(gridproduto.Rows[numeroinput - 1].Cells["ite_id"].Value.ToString());
                        idproduto = int.Parse(gridproduto.Rows[numeroinput - 1].Cells["pro_id"].Value.ToString());
                        descricao = gridproduto.Rows[numeroinput - 1].Cells["pro_nome"].Value.ToString();

                        citemvenda.remover(iditem);
                        cupom.remover(itematual);

                        LinhaCupom = Bobina2.Items.Count;

                        LinhaCupom = LinhaCupom - 1;
                        Bobina2.Items.RemoveAt(LinhaCupom);
                        LinhaCupom = LinhaCupom - 1;
                        Bobina2.Items.RemoveAt(LinhaCupom);
                        Bobina2.Refresh();
                        venda.adicionarvalor(venda.ven_id, ((quantidade * valorproduto)) * -1);

                        if (NomeArquivo != "" && LinhaC != "")
                        {
                            ApagaLinhaArquivoVen(NomeArquivo, LinhaC);
                            ApagaLinhaArquivoVen(NomeArquivo, LinhaD);
                            ApagaLinhaArquivoVen(NomeArquivo, LinhaE);
                            ApagaLinhaArquivoVen(NomeArquivo, LinhaF);
                            ApagaLinhaArquivoVen(NomeArquivo, LinhaG);
                        }

                        //cproduto.baixarestoque(idproduto, quantidade * -1);
                        cproduto.RetornarEstoque(idproduto, quantidade);
                        label16.Text = descricao;
                        ttbproduto.Focus();
                    }
                    else
                    {
                        fmok.Mostrar("NÃO HA ITENS A SEREM EXCLUIDOS!!");
                    }
                }
                catch (Exception ex)
                { fmok.Mostrar("ERRO AO REMOVER PRODUTO!! " + ex.Message); };
                carregar();
            }
            else
            {
                fmok.Mostrar("FUNCIONARIO NÃO AUTORIZADO!! ");
            }
        }

        private void tileItem8_ItemPress(object sender, DevExpress.XtraEditors.TileItemEventArgs e)
        {
            if (acesoVermelho == 0)
            {
                serialPortTorre.Write("#5");
                acesoVermelho = 1;
            }
            else
            {
                serialPortTorre.Write("#6");
                acesoVermelho = 0;
            }
        }

        private void SerialLeitor_DataReceived(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {
            char[] result = new char[14];
            for (int len = 0; len < result.Length;)
            {
                len += SerialLeitor.Read(result, len, result.Length - len);
            }
            SerialLeitor.Read(result, 0, result.Length);
            string s = new string(result);
            //  memoEdit1.Text = s;
            s = s.Substring(1, 13).ToString();

            if (ttbproduto.InvokeRequired)
                ttbproduto.BeginInvoke((MethodInvoker)delegate
                {
                    ttbproduto.Text = s;
                });
        }

        private void btnpnlpesquisar_Click(object sender, DevExpress.XtraEditors.TileItemEventArgs e)
        {

        }

        private void tileItem3_ItemPress(object sender, DevExpress.XtraEditors.TileItemEventArgs e)
        {
            pnlpesquisar.BringToFront();

            if (!pnlpesquisar.Visible)
                pnlpesquisar.Visible = true;
            else
                pnlpesquisar.Visible = false;
        }

        private void btnmovimentacao_Click_1(object sender, EventArgs e)
        {
            if (UtilizarImpre == "sim") acBrECF1.ReducaoZ();
        }

        private void btncontasreceber_Click_1(object sender, EventArgs e)
        {
            if (UtilizarImpre == "sim") acBrECF1.LeituraX();
        }



        private void btnfecharcupom_Click(object sender, DevExpress.XtraEditors.TileItemEventArgs e)
        {
            if (venda.ven_id < 1)
            {
                fmok.Mostrar("Não há cupom para ser fechado!");
                return;
            }
            //  if (venda.ven_total - pagamentos - venda.ven_desconto < 0)
            //  {
            //      fmok.Mostrar("Para poder fechar a venda é necessário um valor final não negativo");
            //     return;
            // }

            if (dtitemvenda.Rows.Count < 1)
            {
                fmok.Mostrar("Não é possivel fechar uma venda sem itens!");
                return;
            }

            //if (ckbentrega.Checked)
            //{
            //    if (cliente.id <= 0)
            //        btnpesquisarcliente.PerformClick();

            //    if (cliente.id <= 0)
            //    {
            //        fmok.Mostrar("Selecione um cliente para fechar a venda!");
            //        return;
            //    }
            //}
            //if (ckbcpf.Checked)
            //{
            //    if (ckbcpf.Text == "CPF")
            //    {
            //        if (cliente.id < 1)
            //        {
            //            btnpesquisarcliente.PerformClick();

            //            if (cliente.id <= 0)
            //            {
            //                fmok.Mostrar("Selecione um cliente para fechar a venda!");
            //                return;
            //            }
            //        }
            //    }
            //    else
            //    {
            //        if (ckbcpf.Text == "")
            //        {
            //            FInput fi = new FInput();
            //            if (fi.Mostrar("CPF", "Informe o CPF do cliente!"))
            //            {
            //                ckbcpf.Text = fi.valor;
            //                cliente.cpf = ckbcpf.Text;
            //                // cliente.nome = "Cliente";
            //            }
            //            else
            //            {
            //                ckbcpf.Text = "";
            //                cliente.cpf = "";
            //                //cliente.nome = "";
            //            }

            //        }
            //    }
            //    //   btnpesquisarcliente.PerformClick();
            //}


            pnlcarregando.Visible = true;
            pnlcarregando.BringToFront();
            Formularios.FPagamento f = new Formularios.FPagamento(venda.ven_id, 0, venda.ven_total, venda.ven_desconto);
            f.ShowDialog();// return;
                           // MGMLib.FPagamento fp = new MGMLib.FPagamento(venda.ven_id, 0, venda.ven_total, venda.ven_desconto);
                           //fp.ShowDialog();
            if (f.ok)
            {

                int sessao = criarSAT();
                //venda.atualizarCaixa(venda.ven_id, idcaixa);

                sessao = 1; ////TESTE PARA VERIFICAR SESSAO SAT

                if (sessao == 0)
                {
                    fmok.MostrarBotao();
                    falar("Erro ao enviar venda ao SAT!");
                    fmok.Mostrar("Erro ao enviar venda ao SAT!");
                    return;
                }


                venda.atualizarClienteFuncionario(venda.ven_id, cliente.id, idfuncionario);
                venda.fechar(venda.ven_id);
                /*
                PrintDialog pd = new PrintDialog();
                if (pd.ShowDialog() == DialogResult.OK)
                {

                    PrintDocument printDocument = new PrintDocument();
                    printDocument.PrintPage += PrintDocumentOnPrintPage;
                    printDocument.PrinterSettings = pd.PrinterSettings;
                    printDocument.Print();
                }*/
                // return;


                limparvenda();
                limpar();
                limparcliente();

                carregar();



                fmok.Mostrar("Venda fechada com sucesso!");
            }
            else
                carregar();

            pnlcarregando.Visible = false;
        }

        private void FPDV_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.F11)
            {
                // Form em tela cheia
                this.Height = Screen.PrimaryScreen.Bounds.Height;
                this.Width = Screen.PrimaryScreen.Bounds.Width;
                this.TopMost = true;
            }
            if (e.KeyData == Keys.F12)
            {
                this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
                this.ClientSize = new System.Drawing.Size(1024, 768);
                this.KeyPreview = true;
                this.Name = "Form1";
                this.Text = "Form1";
            }
            if (e.KeyData == Keys.F10)
            {
                Close();
            }
        }

        private void tileItem7_ItemClick(object sender, DevExpress.XtraEditors.TileItemEventArgs e)
        {
            pesototal = 0;
            peso = 0;
            lblpeso.Text = "0";
            lblPesoTotal.Text = "0";
            CancelaVendaAtual();
            ImprimeCabecalhoBobina();
            falar("operação de venda atual cancelada.");
        }

        private void pnlvendadetalhe_Paint(object sender, PaintEventArgs e)
        {

        }

        private void tileItem4_ItemClick(object sender, DevExpress.XtraEditors.TileItemEventArgs e)
        {
            FProduto FormProduto = new FProduto();
            FormProduto.Show();
        }

        private void timerSaudacao_Tick(object sender, EventArgs e)
        {
            if (FlagSudacao == 0)
            {
                System.Threading.Thread.Sleep(1000);
                // saudacao();
                FlagSudacao = 1;
            }
            timerSaudacao.Enabled = false;
        }

        private void button14_Click(object sender, EventArgs e)
        {
            FAbrirCaixa AbreCaixa = new FAbrirCaixa();
            FMessageSimNao fm = new FMessageSimNao();
            if (idcaixa > 0)
            {
                if (fm.Mostrar("Caixa", "Deseja Fechar o Caixa atual ?"))
                {
                    string DataHoje = DateTime.Now.ToString().Replace("/", "-").Substring(0, 10);
                    string HoraHoje = DateTime.Now.ToString();
                    DataHoje = DataHoje.Substring(6, 4) + "-" + DataHoje.Substring(3, 2) + "-" + DataHoje.Substring(0, 2);
                    HoraHoje = HoraHoje.Substring(11, 8);

                    CVenda venda = new CVenda();
                    CResumoFiscal Resumo = new CResumoFiscal();
                    CMeioPagamento FormaPagamento = new CMeioPagamento();
                    CEmpresa Empresa = new CEmpresa();
                    CFuncionario Funcionario = new CFuncionario();
                    CVenda Venda = new CVenda();
                    CCupom Cupom = new CCupom();
                    CCaixa Caixa = new CCaixa();

                    var NCaixa = venda.pesquisarvendaaberta(idcaixa);
                    DateTime D1 = DateTime.Now;
                    DateTime D2 = DateTime.Now;
                    var TotCupm = venda.pesquisarTotalCupons(D1, D2);
                    var VTotal = venda.pesquisarTotalVenda(D1, D2);
                    DataTable LResumo = Resumo.pesquisarResuno(D1);

                    venda.pesquisarvendaaberta(idcaixa);

                    if (venda.ven_status == 1)
                    {
                        fmok.Mostrar("Não é possível fechar o caixa com uma venda aberta!");
                        return;
                    }

                    string CupomAtual = venda.pesquisarIdVendaAtual().ToString();
                    /////////// fazer fechamento automatico
                    FFecharCaixa1 f = new FFecharCaixa1(idfuncionario, idcaixa);
                    f.ShowDialog();
                    if (f.ok)
                    {

                        string ope = Convert.ToString(idfuncionario);
                        string ger = Convert.ToString(idfuncionario);
                        string uad = Convert.ToString(empresa.emp_id);
                        string lot = CupomAtual;
                        string maq = Convert.ToString(venda.cai_id);
                        string dat = DataHoje;// Convert.ToString(Caixa.cai_datainicial);
                        string hor = HoraHoje;// Convert.ToString(Caixa.cai_horafinal);
                        string cpm = CupomAtual;// CupomAtual;
                        string vap = Versao;
                        string ctr = venda.ven_id.ToString();
                        string cts = "0";

                        string qcp = TotCupm.ToString();
                        string dgt = "N";
                        string fpg = "00006"; //Debito - Form de pagamento
                        string qtd = TotCupm.ToString(); // total de operacoes
                        string vlr = VTotal.ToString();
                        string vdg = "0"; // valor digitado
                        string tpg = Convert.ToInt32(FormaPagamento.mei_cac).ToString(); // tipo de venda (Se é visa, master, dinners..)
                        string trb = "0";//Cod.tributacao
                        string alq = "0";//Aliquota
                        string bic = "0";//Valor da Base de ICMS
                        string ttp = "0";//totalizador parcial

                        string nrz = "0";//Numero da reducao Z
                        string ico = "0";//COO inicial
                        string fco = "0";//COO Final
                        string vgt = VTotal.ToString();//Valor do GT
                        string vmv = VTotal.ToString();//Valor do Movimento (bruto)
                        string vds = "0";//Valor dos descontos
                        string dsb = "0";//Data Software Basico
                        string vcn = "0";//Valor dos Cancelamentos
                        string dmv = DataHoje; //Data do movimento
                        string seq = "0000";//Numero de serie do ECF
                        string cro = "0";//Contador de reinicio de operacao
                        string mfd = "0";// Mfd adicional
                        string tip = "ELGIN";//Tipo de emissor
                        string mar = "ELGIN";//Marca do ECF
                        string mod = "SS-100";//Modelo do ECF
                        string vsb = Versao; // Versao Software Basico
                        string hsb = HoraHoje;// Hora Software Basico
                        string nse = "001";//Sequencial 
                        string nou = idfuncionario.ToString(); //	Numero do usuario
                        string nef = "0";//Numero do ECF

                        GerarFechamentoParcial(CaminhoDadosRep, NomeArquivo, uad, lot, ope, ger, maq, dat, hor, cpm, vap, ctr, cts, qcp, dgt, fpg, qtd, vlr, vdg, tpg);

                        GerarFechamentoDiario(CaminhoDadosRep, NomeArquivo, uad, lot, ope, ger, maq, dat, hor, cpm, vap, ctr, cts, fpg, qtd, vlr, vdg, trb,
    alq, bic, ttp, nrz, ico, fco, vgt, vmv, vds, dsb, vcn, dmv, seq, cro, mfd, tip, mar, mod, vsb, hsb, nse, nou, nef, tpg);

                        //AbreCaixa.CaixaAberto
                        ///////////////////////////// FECHAMENTO PARCIAL ///////////////////////////////////
                        /*
                         * A|3189|47|47100|99999|01|2017-01-01|14:10:47|5.05.O.161205|
                        C|971195|2017-01-01|19:46:35|900018|2017-01-01||00000|
                        X|47100|2017-01-01|19:46:35|46|N|
                        X03|DN|24|657.39|0.00|
                        X03|DC|19|36.43|0.00|
                        X04|01|24|657.39|0.00|
                        X04|62|1|36.43|0.00|
                        */

                        ///////////////////////////// FECHAMENTO DIARIO ///////////////////////////////////
                        /*
                         *  A|3190|47|47100|99999|01|2017-01-01|14:10:47|5.05.O.161205|
                            C|999999|2017-01-01|19:49:24|900021|2017-01-01||00000|
                            Z|47100|2017-01-01|19:49:24|46|N|
                            Z01|DN|24|607.39|0.00|
                            Z01|CC|3|202.73|0.00|
                            Z01|DC|19|539.78|0.00|
                            Z02|4|0.00|30.58|II|
                            Z02|5|0.00|1117.44|FF|
                            Z02|1|18.00|2.88|01T1800|
                            Z03|503|97459|97552|1823271.89|1436.63|0.00|285.73|2017-01-01|000020157|2||SAT|SWEDA|SS-100|01.00.05|2009-06-25|10:12:12|1500000000020157|1|003|
                            Z04|01|24|607.39|0.00|
                            Z04|08|6|125.57|0.00|
                            Z04|14|6|147.81|0.00|
                            Z04|18|2|169.32|0.00|
                            Z04|42|5|110.99|0.00|
                            Z04|45|1|33.41|0.00|
                            Z04|61|1|59.77|0.00|
                            Z04|62|1|95.64|0.00
                            */
                    }

                    return;
                }

            }
            else
                return;
        }

        private void lblpeso_TextChanged(object sender, EventArgs e)
        {
            Thread.Sleep(3000);
            if (UtilizarBalan == "sim")
            {
                if (lblpeso.Text != "0") fmok.Fechar(); // if(lblpeso.Text != "0") 

                timer2.Enabled = false;
            }

        }

        private void btnfechar_Click(object sender, EventArgs e)
        {
            FMessageSimNao fm = new FMessageSimNao();
            if (idcaixa > 0)
            {
                if (fm.Mostrar("Caixa", "Deseja Fechar o caixa antes de encerrar o sistema?"))
                {
                    CVenda venda = new CVenda();
                    venda.pesquisarvendaaberta(idcaixa);
                    if (venda.ven_status == 1)
                    {
                        fmok.Mostrar("Não é possível fechar o caixa com uma venda aberta!");
                        return;
                    }
                    FFecharCaixa1 f = new FFecharCaixa1(idfuncionario, idcaixa);
                    f.ShowDialog();
                    if (f.ok)
                        Close();
                }
                else
                    Close();
            }
            else Close();
        }
    }
}