using MGMPDV.Classes;
using MGMPDV.TEF;
using System;
using System.Collections.Generic;
using Microsoft.VisualBasic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Speech.Synthesis;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Printing;

namespace MGMPDV.Formularios
{
    public partial class FPagamento : Form
    {
        private int nVezes = 0;

        clsTEF TEF;


        double TOTAL;
        List<Pagamento> lstPag;

        delegate void dSetMensOp(string msg);
        delegate void dSetMensCli(string msg);
        delegate void dSetMensDoisVisores(string msg);
        delegate void dSetMensFormaPagamento(string msg);
        delegate void dFechaJanela();

        dSetMensOp setMensOp;
        dSetMensCli setMensCli;
        dSetMensDoisVisores setMensDoisVisores;
        dSetMensFormaPagamento setMensFormaPagamento;
        dFechaJanela fechaJanela;

        public bool ok { get; set; }
        public string titulo { get; set; }
        public decimal valortotal { get; set; }
        public decimal valorpago { get; set; }
        public int quantidadeparcela { get; set; }
        public int idcompra { get; set; }
        public int idvenda { get; set; }
        public int idcliente { get; set; }
        public int idcaixa { get; set; }
        public DateTime dtvencimento { get; set; }
        public bool avista { get; set; }
        public decimal desconto { get; set; }
        public decimal troco { get; set; }
        decimal valordepagamento { get; set; }
        private DateTime datahora { get; set; }
        private CContaReceber contareceber = new CContaReceber();
        private CContaPagar contapagar = new CContaPagar();
        private CVenda venda = new CVenda();
        private int[] vetidparcela;
        public string Parcelas = "0";
        public string VerDinheiro;// = "nao";
        public string VerCheque;// = "nao";
        public string TotalPago = "0";
        public bool Cancelado = false;
        public string Ptipopag = "01";
        int X = 0;
        int Y = 0;
        DataTable dtmeiopagamento = new DataTable();
        int idmeiopagamento = -1;
        string INTPOS = "";
        SpeechSynthesizer reader;
        string Caminhoreq = "";
        DataTable dt = new DataTable();
        FMessageOk fmok = new FMessageOk();

        public void GravaListBox()
        {
            string caminhoArquivo = "ListBox.txt";

            using (StreamWriter arquivo = new StreamWriter(caminhoArquivo, true))
            {
                for (int i = 0; i < listBox1.Items.Count; i++)
                {
                    arquivo.Write(listBox1.Items[i].ToString());
                    arquivo.Write(Environment.NewLine);
                }

            }
        }

        void setMsgDoisVisores(string msg)
        {
            if (!string.IsNullOrEmpty(msg))
            {
                fmok.Mostrar(msg);
                //  lblMensCli.Text = msg;
                //  lblMensOp.Text = msg;
            }
        }

        void setMsgOp(string msg)
        {
            if (!string.IsNullOrEmpty(msg))
            {
                fmok.Mostrar(msg);
                //  lblMensOp.Text = msg;
                //  lblMensCli.Text = "";
            }
        }

        void setMsgCli(string msg)
        {
            if (!string.IsNullOrEmpty(msg))
            {
                fmok.Mostrar(msg);
                //  lblMensCli.Text = msg;
                // lblMensOp.Text = "";
            }
        }

        void setMsgFormaPag(string msg)
        {
            if (!string.IsNullOrEmpty(msg))
                label2.Text = msg;
        }

        public int RotinaColeta(int comando, int tipoCampo, ref short pTamanhoMinimo, ref short pTamanhoMaximo, byte[] pDadosComando, byte[] pCampo, ref byte[] retornoDadosUsuario)
        {
            FInput f = new FInput();
            f.HabilitaSenha = "sim";
            char c;
            string mensagem = Encoding.UTF8.GetString(pDadosComando);

            mensagem = mensagem.Substring(0, mensagem.IndexOf('\x0'));

            if (comando != 23)
            {
                nVezes = 0;
            }

            switch (comando)
            {
                case 1:
                    //System.Windows.Forms.MessageBox.Show("Mensagem Visor Operador: [" + mensagem.ToString() + "]", "RotinaColeta");
                    // this.Invoke(setMensOp,new object[] {mensagem});
                    this.Invoke((MethodInvoker)delegate ()
                    {
                        listBox1.Items.Add("Mensagem 1: Visor Operador:[" + mensagem.ToString() + "]");
                        label2.Text = mensagem.ToString();

                        label27.Text = mensagem.ToString();
                        pnlcarregando.Visible = true;
                        pnlcarregando.BringToFront();
                    });
                    return 0;
                case 2:
                    //System.Windows.Forms.MessageBox.Show("Mensagem Visor Cliente: [" + mensagem.ToString() + "]", "RotinaColeta");
                    // this.Invoke(setMensCli,new object[]{mensagem});
                    this.Invoke((MethodInvoker)delegate ()
                    {
                        listBox1.Items.Add("Mensagem 2: Visor Cliente:[" + mensagem.ToString() + "]");
                    });
                    return 0;
                case 3:
                    //System.Windows.Forms.MessageBox.Show("Mensagem para os dois visores: [" + mensagem.ToString() + "]", "RotinaColeta");
                    // this.Invoke(setMensOp, new object[] { mensagem });
                    this.Invoke((MethodInvoker)delegate ()
                    {
                        listBox1.Items.Add("Mensagem 3: dois visores [" + mensagem.ToString() + "]");
                    });
                    //  this.Invoke(setMensCli, new object[] { mensagem });
                    this.Invoke((MethodInvoker)delegate ()
                    {
                        listBox1.Items.Add("Mensagem 4: [" + mensagem.ToString() + "]");
                        label2.Text = mensagem.ToString();

                        label27.Text = mensagem.ToString();
                        pnlcarregando.Visible = true;
                        pnlcarregando.BringToFront();
                    });
                    return 0;
                case 4:
                    // System.Windows.Forms.MessageBox.Show("Mensagem Visor: [" + mensagem.ToString() + "]", "RotinaColeta");
                    //  this.Invoke(setMensCli, new object[] { mensagem });
                    this.Invoke((MethodInvoker)delegate ()
                    {
                        listBox1.Items.Add("Mensagem 5: [" + mensagem.ToString() + "]");
                        label2.Text = mensagem.ToString();

                        label27.Text = mensagem.ToString();
                        pnlcarregando.Visible = true;
                        pnlcarregando.BringToFront();
                    });
                    return 0;
                case 11:

                    break;
                case 12:
                    break;
                case 13:
                    break;
                case 14:
                    //LIMPA O TEXTO UTILIZADO COMO TÍTULO NA APRESENTAÇÃO DO MENU
                    break;
                case 37:
                    // System.Windows.Forms.MessageBox.Show("Coleta confirmacao no PinPad: [" + mensagem.ToString() + "]", "RotinaColeta", System.Windows.Forms.MessageBoxButtons.YesNo);
                    this.Invoke((MethodInvoker)delegate ()
                    {
                        listBox1.Items.Add("Mensagem 6: Coleta confirmacao no PinPad[" + mensagem.ToString() + "]");
                    });
                    return 0;

                case 20:
                    //System.Windows.Forms.MessageBox.Show("Coleta Sim/Nao: [" + mensagem.ToString() + "]", "RotinaColeta", System.Windows.Forms.MessageBoxButtons.YesNo);
                    this.Invoke((MethodInvoker)delegate ()
                    {
                        listBox1.Items.Add("Mensagem 7: Coleta Sim/Nao:[" + mensagem.ToString() + "]");
                    });
                    return 0;

                case 21:
                    //21 -> MENU DE OPÇÕES E RETORNO DE ESCOLHA   
                    this.Invoke((MethodInvoker)delegate ()
                    {
                        pnlcarregando.Visible = false;
                    });
                    FSelecao form = new FSelecao(mensagem);
                    form.ShowDialog();
                    retornoDadosUsuario = Encoding.ASCII.GetBytes(String.Format("{0}\0", form.result));
                    form = null;
                    return 0;

                case 22:
                    //    System.Windows.Forms.MessageBox.Show("Obtem qualquer tecla: [" + mensagem.ToString() + "]", "RotinaColeta");
                    this.Invoke((MethodInvoker)delegate ()
                    {
                        fmok.Mostrar("22 - " + mensagem);
                        listBox1.Items.Add("22 - Obtem qualquer tecla: [" + mensagem + "]- RotinaColeta");
                    });
                    return 0;

                case 23:
                    System.Threading.Thread.Sleep(1000);

                    if (nVezes++ > 30)
                    {
                        return -1;
                    }

                    return 0;

                case 30:
                    //LEITURA DE DADOS COM TAM MIN E MÁX 



                    switch (tipoCampo)
                    {

                        //ENTRADA DE 
                        case 140:
                            // retornoDadosUsuario = Encoding.ASCII.GetBytes(EntradaDadosUsuarioStr("DIGITE A DATA DA PRIMEIRA PARCELA NO FORMATO ddmmaaaa"));
                            f.HabilitaSenha = "sim";
                            f.Mostrar("CARTÃO", "DATA DA PRIMEIRA PARCELA!");
                            retornoDadosUsuario = Encoding.ASCII.GetBytes(f.valor);
                            return 0;

                        //ENTRADA DE NÚMERO DE PARCELAS
                        case 505:
                            f.HabilitaSenha = "sim";
                            // retornoDadosUsuario = Encoding.ASCII.GetBytes(EntradaDadosUsuarioStr("DIGITE O NÚMERO DE PARCELAS").Replace(" ", ""));
                            f.Mostrar("CARTÃO", "DIGITE O NÚMERO DE PARCELAS!");
                            retornoDadosUsuario = Encoding.ASCII.GetBytes(f.valor);
                            return 0;

                        case 506:
                            f.HabilitaSenha = "sim";
                            /// retornoDadosUsuario = Encoding.ASCII.GetBytes(EntradaDadosUsuarioStr("DIGITE A DATA DO PRÉ-DATADO NO FORMATO ddMMaaaa"));
                            f.Mostrar("CHEQUE", "DIGITE A DATA DO PRÉ-DATADO!");
                            retornoDadosUsuario = Encoding.ASCII.GetBytes(f.valor);
                            return 0;

                        //ENTRADA DO NÚMERO DE PARCELAS CDC
                        case 511:
                            f.HabilitaSenha = "sim";
                            //retornoDadosUsuario = Encoding.ASCII.GetBytes(EntradaDadosUsuarioStr("DIGITE O NÚMERO DE PARCELAS CDC"));
                            f.Mostrar("CARTÃO", "DIGITE O NÚMERO DE PARCELAS CDC!");
                            retornoDadosUsuario = Encoding.ASCII.GetBytes(f.valor);
                            return 0;

                        //ENTRADA DE NÚMERO DO CARTÃO
                        case 512:
                            f.HabilitaSenha = "sim";
                            // retornoDadosUsuario = Encoding.ASCII.GetBytes(EntradaDadosUsuarioStr("DIGITE O NÚMERO DO CARTÃO").Replace(" ", ""));
                            f.Mostrar("CARTÃO", "DIGITE O NÚMERO DO CARTÃO!");
                            retornoDadosUsuario = Encoding.ASCII.GetBytes(f.valor);
                            return 0;
                        //ENTRADA DE DATA DO VENCIMENTO DO CARTÃO
                        case 513:
                            f.HabilitaSenha = "sim";
                            // retornoDadosUsuario = Encoding.ASCII.GetBytes(EntradaDadosUsuarioStr("DIGITE A DATA DE VENCIMENTO DO CARTÃO").Replace(" ", ""));
                            f.Mostrar("CARTÃO", "DIGITE A DATA DE VENCIMENTO DO CARTÃO!");
                            retornoDadosUsuario = Encoding.ASCII.GetBytes(f.valor);
                            return 0;
                        //ENTRADA DE CÓDIGO DE SEGURANÇA DO CARTÃO
                        case 514:
                            f.HabilitaSenha = "sim";
                            //retornoDadosUsuario = Encoding.ASCII.GetBytes(EntradaDadosUsuarioStr("DIGITE O CÓDIGO DE SEGURANÇA DO CARTÃO"));
                            f.Mostrar("CARTÃO", "DIGITE O CÓDIGO DE SEGURANÇA DO CARTÃO!");
                            retornoDadosUsuario = Encoding.ASCII.GetBytes(f.valor);
                            return 0;
                        //ENTRADA DE DATA DE CANCELAMENTO
                        case 515:
                            f.HabilitaSenha = "sim";
                            // retornoDadosUsuario = Encoding.ASCII.GetBytes(EntradaDadosUsuarioStr("DIGITE A DATA DA TRANSAÇÃO A SER CANCELADA (DDMMAAAA)"));
                            f.Mostrar("CARTÃO", "DIGITE A DATA DA TRANSAÇÃO A SER CANCELADA!");
                            retornoDadosUsuario = Encoding.ASCII.GetBytes(f.valor);
                            return 0;
                    }


                    break;
                case 31:
                    break;
                case 32:
                    break;
                case 33:
                    break;
                case 34:

                    switch (tipoCampo)
                    {
                        case 130:
                            return 0;

                        case 504:

                            // retornoDadosUsuario = Encoding.ASCII.GetBytes(EntradaDadosUsuarioStr("DIGITE O VALOR DA TAXA DE SERVIÇO"));
                            // f.Mostrar("CARTÃO", "DIGITE O VALOR DA TAXA DE SERVIÇO!");
                            retornoDadosUsuario = Encoding.ASCII.GetBytes("0");
                            return 0;
                    }


                    break;
                case 35:
                    break;
                case 38:
                    // System.Windows.Forms.MessageBox.Show("nComando: [" + comando.ToString() + "]\nTipoCampo: [" + tipoCampo.ToString() + "]", "RotinaColeta");
                    this.Invoke((MethodInvoker)delegate ()
                    {
                        listBox1.Items.Add("38 - nComando: [" + comando.ToString() + "]\nTipoCampo: [" + tipoCampo.ToString() + "]-RotinaColeta");
                    });
                    return 0;
            }

            retornoDadosUsuario = null;


            return -1;
        }

        public int RotinaColetaCartDig(int comando, int tipoCampo, ref short pTamanhoMinimo, ref short pTamanhoMaximo, byte[] pDadosComando, byte[] pCampo, ref byte[] retornoDadosUsuario)
        {
            char c;
            string mensagem = Encoding.UTF8.GetString(pDadosComando);

            mensagem = mensagem.Substring(0, mensagem.IndexOf('\x0'));

            if (comando != 23)
            {
                nVezes = 0;
            }

            switch (comando)
            {
                case 1:
                    //System.Windows.Forms.MessageBox.Show("Mensagem Visor Operador: [" + mensagem.ToString() + "]", "RotinaColeta");
                    // this.Invoke(setMensOp, new object[] { mensagem });
                    this.Invoke((MethodInvoker)delegate ()
                    {
                        listBox1.Items.Add("Mensagem Visor Operador: [" + mensagem.ToString() + "]");
                    });
                    return 0;
                case 2:
                    //System.Windows.Forms.MessageBox.Show("Mensagem Visor Cliente: [" + mensagem.ToString() + "]", "RotinaColeta");
                    // this.Invoke(setMensCli, new object[] { mensagem });
                    this.Invoke((MethodInvoker)delegate ()
                    {
                        listBox1.Items.Add("Mensagem Visor Cliente: [" + mensagem.ToString() + "]");
                    });
                    return 0;
                case 3:
                    //System.Windows.Forms.MessageBox.Show("Mensagem para os dois visores: [" + mensagem.ToString() + "]", "RotinaColeta");
                    // this.Invoke(setMensOp, new object[] { mensagem });
                    this.Invoke((MethodInvoker)delegate ()
                    {
                        listBox1.Items.Add("Mensagem dois visores: [" + mensagem.ToString() + "]");
                    });
                    // this.Invoke(setMensCli, new object[] { mensagem });
                    this.Invoke((MethodInvoker)delegate ()
                    {
                        listBox1.Items.Add("99 - Mensagem : [" + mensagem.ToString() + "]");
                    });
                    return 0;
                case 4:
                    // System.Windows.Forms.MessageBox.Show("Mensagem Visor: [" + mensagem.ToString() + "]", "RotinaColeta");
                    //  this.Invoke(setMensCli, new object[] { mensagem });
                    this.Invoke((MethodInvoker)delegate ()
                    {
                        listBox1.Items.Add("99B - Mensagem Visor: [" + mensagem.ToString() + "]");
                    });
                    return 0;
                case 11:

                    break;
                case 12:
                    break;
                case 13:
                    break;
                case 14:
                    //LIMPA O TEXTO UTILIZADO COMO TÍTULO NA APRESENTAÇÃO DO MENU
                    break;
                case 37:
                    // System.Windows.Forms.MessageBox.Show("Coleta confirmacao no PinPad: [" + mensagem.ToString() + "]", "RotinaColeta", System.Windows.Forms.MessageBoxButtons.YesNo);
                    this.Invoke((MethodInvoker)delegate ()
                    {
                        listBox1.Items.Add("Coleta confirmacao no PinPad: [" + mensagem.ToString() + "]-RotinaColeta");
                    });
                    return 0;

                case 20:
                    //   System.Windows.Forms.MessageBox.Show("Coleta Sim/Nao: [" + mensagem.ToString() + "]", "RotinaColeta", System.Windows.Forms.MessageBoxButtons.YesNo);
                    this.Invoke((MethodInvoker)delegate ()
                    {
                        listBox1.Items.Add("Coleta Sim/Nao: [" + mensagem.ToString() + "]-RotinaColeta");
                    });
                    return 0;

                case 21:
                    //21 -> MENU DE OPÇÕES E RETORNO DE ESCOLHA   
                    //if(mensagem.Equals(""))
                    pnlcarregando.Visible = false;
                    FSelecao form = new FSelecao(mensagem);
                    form.ShowDialog();
                    retornoDadosUsuario = Encoding.ASCII.GetBytes(String.Format("{0}\0", form.result));
                    form = null;
                    return 0;

                case 22:
                    //  System.Windows.Forms.MessageBox.Show("Obtem qualquer tecla: [" + mensagem.ToString() + "]", "RotinaColeta");
                    this.Invoke((MethodInvoker)delegate ()
                    {
                        fmok.Mostrar("22b - " + mensagem.ToString());
                        listBox1.Items.Add("Obtem qualquer tecla: [" + mensagem.ToString() + "]-RotinaColeta");
                    });
                    return 0;

                case 23:
                    System.Threading.Thread.Sleep(1000);

                    if (nVezes++ > 30)
                    {
                        return -1;
                    }

                    return 0;

                case 30:
                    //LEITURA DE DADOS COM TAM MIN E MÁX 



                    switch (tipoCampo)
                    {

                        //ENTRADA DE 
                        case 140:
                            pnlcarregando.Visible = false;
                            retornoDadosUsuario = Encoding.ASCII.GetBytes(EntradaDadosUsuarioStr("DIGITE A DATA DA PRIMEIRA PARCELA NO FORMATO ddmmaaaa"));
                            return 0;

                        //ENTRADA DE NÚMERO DE PARCELAS
                        case 505:
                            pnlcarregando.Visible = false;
                            retornoDadosUsuario = Encoding.ASCII.GetBytes(EntradaDadosUsuarioStr("DIGITE O NÚMERO DE PARCELAS").Replace(" ", ""));
                            return 0;

                        case 506:
                            pnlcarregando.Visible = false;
                            retornoDadosUsuario = Encoding.ASCII.GetBytes(EntradaDadosUsuarioStr("DIGITE A DATA DO PRÉ-DATADO NO FORMATO ddMMaaaa"));
                            return 0;

                        //ENTRADA DO NÚMERO DE PARCELAS CDC
                        case 511:
                            pnlcarregando.Visible = false;
                            retornoDadosUsuario = Encoding.ASCII.GetBytes(EntradaDadosUsuarioStr("DIGITE O NÚMERO DE PARCELAS CDC"));
                            return 0;

                        //ENTRADA DE NÚMERO DO CARTÃO
                        case 512:
                            pnlcarregando.Visible = false;
                            retornoDadosUsuario = Encoding.ASCII.GetBytes(EntradaDadosUsuarioStr("DIGITE O NÚMERO DO CARTÃO").Replace(" ", ""));
                            return 0;
                        //ENTRADA DE DATA DO VENCIMENTO DO CARTÃO
                        case 513:
                            pnlcarregando.Visible = false;
                            retornoDadosUsuario = Encoding.ASCII.GetBytes(EntradaDadosUsuarioStr("DIGITE A DATA DE VENCIMENTO DO CARTÃO").Replace(" ", ""));
                            return 0;
                        //ENTRADA DE CÓDIGO DE SEGURANÇA DO CARTÃO
                        case 514:
                            pnlcarregando.Visible = false;
                            retornoDadosUsuario = Encoding.ASCII.GetBytes(EntradaDadosUsuarioStr("DIGITE O CÓDIGO DE SEGURANÇA DO CARTÃO"));
                            return 0;
                        //ENTRADA DE DATA DE CANCELAMENTO
                        case 515:
                            pnlcarregando.Visible = false;
                            retornoDadosUsuario = Encoding.ASCII.GetBytes(EntradaDadosUsuarioStr("DIGITE A DATA DA TRANSAÇÃO A SER CANCELADA (DDMMAAAA)"));
                            return 0;
                    }


                    break;
                case 31:
                    break;
                case 32:
                    break;
                case 33:
                    break;
                case 34:

                    switch (tipoCampo)
                    {
                        case 130:
                            return 0;

                        case 504:
                            ///  retornoDadosUsuario = Encoding.ASCII.GetBytes(EntradaDadosUsuarioStr("DIGITE O VALOR DA TAXA DE SERVIÇO"));
                            retornoDadosUsuario = Encoding.ASCII.GetBytes("0"); // EntradaDadosUsuarioStr("DIGITE O VALOR DA TAXA DE SERVIÇO"));
                            return 0;
                    }


                    break;
                case 35:
                    break;
                case 38:
                    //  System.Windows.Forms.MessageBox.Show("nComando: [" + comando.ToString() + "]\nTipoCampo: [" + tipoCampo.ToString() + "]", "RotinaColeta");
                    this.Invoke((MethodInvoker)delegate ()
                    {
                        listBox1.Items.Add("nComando: [" + comando.ToString() + "]\nTipoCampo: [" + tipoCampo.ToString() + "]-RotinaColeta");
                    });
                    return 0;
            }

            retornoDadosUsuario = null;


            return -1;
        }

        public void feedAndCutter(string printerName, int numLines)
        {
            EscPos esc = new EscPos();
            System.Threading.Thread.Sleep(500);
            esc.lineFeed(printerName, numLines);
            esc.CutPaper(printerName);
        }

        public int RotinaResultadoCartDig(int tipoCampo, byte[] buffer)
        {
            string mensagem = Encoding.UTF8.GetString(buffer);

            mensagem = mensagem.Substring(0, mensagem.IndexOf('\x0'));

            switch (tipoCampo)
            {
                case 1:
                    //  System.Windows.Forms.MessageBox.Show("Finalizacao: [" + mensagem.ToString() + "]", "RotinaResultado");
                    this.Invoke((MethodInvoker)delegate ()
                    {
                        listBox1.Items.Add("1 - Finalizacao: [" + mensagem.ToString() + "]");
                    });
                    break;

                case 105:
                    //   System.Windows.Forms.MessageBox.Show("nTipoCampo - 105 (data e hora da transação no formato aaaaMMddhhmmss)\n\nConteúdo: [" + mensagem + "]");
                    this.Invoke((MethodInvoker)delegate ()
                    {
                        listBox1.Items.Add("105 - (data e hora da transação no formato aaaaMMddhhmmss)\n\nConteúdo: [" + mensagem + "]");
                    });
                    break;

                case 121:
                    //  System.Windows.Forms.MessageBox.Show("Comprovante Cliente: \n" + mensagem.ToString(), "RotinaResultado");
                    this.Invoke((MethodInvoker)delegate ()
                    {
                        listBox1.Items.Add("121 - Comprovante Cliente: \n" + mensagem.ToString());

                        // Allow the user to select a printer.
                        EscPos esc = new EscPos();
                        string s = mensagem.ToString();
                        PrintDialog pd = new PrintDialog();
                        s = s.ToString().Replace("\"", "");
                        pd.PrinterSettings = new PrinterSettings();
                        RawPrinterHelper.SendStringToPrinter(pd.PrinterSettings.PrinterName, s);

                        esc.printText(pd.PrinterSettings.PrinterName, s);

                        feedAndCutter(pd.PrinterSettings.PrinterName, 5);

                    });
                    break;

                case 122:
                    // System.Windows.Forms.MessageBox.Show("Comprovante Estabelecimento: \n" + mensagem.ToString(), "RotinaResultado");
                    this.Invoke((MethodInvoker)delegate ()
                    {
                        listBox1.Items.Add("122 - Comprovante Estabelecimento: \n" + mensagem.ToString());

                        // Allow the user to select a printer.
                        EscPos esc = new EscPos();
                        string s = mensagem.ToString();
                        PrintDialog pd = new PrintDialog();
                        s = s.ToString().Replace("\"", "");
                        pd.PrinterSettings = new PrinterSettings();
                        RawPrinterHelper.SendStringToPrinter(pd.PrinterSettings.PrinterName, s);

                        esc.printText(pd.PrinterSettings.PrinterName, s);

                        feedAndCutter(pd.PrinterSettings.PrinterName, 5);
                    });
                    break;

                case 131:
                    //  System.Windows.Forms.MessageBox.Show("Rede Destino: [" + mensagem.ToString() + "]", "RotinaResultado");
                    this.Invoke((MethodInvoker)delegate ()
                    {
                        listBox1.Items.Add("131 - Rede Destino: [" + mensagem.ToString() + "]");
                    });
                    break;

                case 132:
                    // System.Windows.Forms.MessageBox.Show("Tipo Cartao: [" + mensagem.ToString() + "]", "RotinaResultado");
                    this.Invoke((MethodInvoker)delegate ()
                    {
                        listBox1.Items.Add("132 - Tipo Cartao: [" + mensagem.ToString() + "]");
                    });
                    break;

                case 136:
                    //  System.Windows.Forms.MessageBox.Show("nTipoCampo - 136 (6 primeiras posições do cartão)\n\nConteúdo:[" + mensagem + "]", "RotinaResultado");
                    this.Invoke((MethodInvoker)delegate ()
                    {
                        listBox1.Items.Add("136 - (6 primeiras posições do cartão)\n\nConteúdo:[" + mensagem + "]");
                    });
                    break;

                case 170:
                    //  System.Windows.Forms.MessageBox.Show("nTipoCampo - 170 (Venda Parcelada Estabelecimento Habilitada)\n\nConteúdo: [" + mensagem + "]", "RotinaResultado");
                    this.Invoke((MethodInvoker)delegate ()
                    {
                        listBox1.Items.Add("170 - (Venda Parcelada Estabelecimento Habilitada)\n\nConteúdo: [" + mensagem + "]");
                    });
                    break;

                case 171:
                    // System.Windows.Forms.MessageBox.Show("nTipoCampo - 171 (Número mínimo de parcelas - parcelada estabelecimento)\n\nConteúdo: [" + mensagem + "]", "RotinaResultado");
                    this.Invoke((MethodInvoker)delegate ()
                    {
                        listBox1.Items.Add("171 - (Número mínimo de parcelas - parcelada estabelecimento)\n\nConteúdo: [" + mensagem + "]");
                    });
                    break;

                case 172:
                    //  System.Windows.Forms.MessageBox.Show("nTipoCampo - 172 (Número máximo de parcelas - parcelada estabelecimento)\n\nConteúdo: [" + mensagem + "]", "RotinaResultado");
                    this.Invoke((MethodInvoker)delegate ()
                    {
                        listBox1.Items.Add("172 - (Número máximo de parcelas - parcelada estabelecimento)\n\nConteúdo: [" + mensagem + "]");
                    });
                    break;

                case 173:
                    // System.Windows.Forms.MessageBox.Show("nTipoCampo - 173 (Valor mínimo por parcela - parcelada estabelecimento)\n\nConteúdo: [" + mensagem + "]", "RotinaResultado");
                    this.Invoke((MethodInvoker)delegate ()
                    {
                        listBox1.Items.Add("173 - (Valor mínimo por parcela - parcelada estabelecimento)\n\nConteúdo: [" + mensagem + "]");
                    });
                    break;

                case 174:
                    //  System.Windows.Forms.MessageBox.Show("nTipoCampo - 174 (Venda parcela administradora habilitada)\n\nConteúdo: [" + mensagem + "]", "RotinaResultado");
                    this.Invoke((MethodInvoker)delegate ()
                    {
                        listBox1.Items.Add("174 - (Venda parcela administradora habilitada)\n\nConteúdo: [" + mensagem + "]");
                    });
                    break;

                case 175:
                    // System.Windows.Forms.MessageBox.Show("nTipoCampo - 175 (Número Mínimo de Parcelas - parcelada administradora)\n\nConteúdo: [" + mensagem + "]", "RotinaResultado");
                    this.Invoke((MethodInvoker)delegate ()
                    {
                        listBox1.Items.Add("175 - (Número Mínimo de Parcelas - parcelada administradora)\n\nConteúdo: [" + mensagem + "]");
                    });
                    break;

                case 176:
                    // System.Windows.Forms.MessageBox.Show("nTipoCampo - 176 (Número Máximo de Parcelas - parcelada administradora)\n\nConteúdo: [" + mensagem + "]", "RotinaResultado");
                    this.Invoke((MethodInvoker)delegate ()
                    {
                        listBox1.Items.Add("176 - (Número Máximo de Parcelas - parcelada administradora)\n\nConteúdo: [" + mensagem + "]");
                    });
                    break;

                case 2421:
                    //System.Windows.Forms.MessageBox.Show("nTipoCampo - 2421 (Função de coleta de dados adicionais do cliente habilitada)\nConteúdo:[" + mensagem + "]", "RotinaResultado");
                    this.Invoke((MethodInvoker)delegate ()
                    {
                        listBox1.Items.Add("2421 - (Função de coleta de dados adicionais do cliente habilitada)\nConteúdo:[" + mensagem + "]");
                    });
                    break;

                case 2090:
                    //  System.Windows.Forms.MessageBox.Show("nTipoCampo - 2090 (Tipo do cartão lido)\n\nConteúdo[" + mensagem + "]", "RotinaResultado");
                    this.Invoke((MethodInvoker)delegate ()
                    {
                        listBox1.Items.Add("2090 - (Tipo do cartão lido)\n\nConteúdo[" + mensagem + "]");
                    });
                    break;

                default:
                    // System.Windows.Forms.MessageBox.Show("nTipoCampo: [" + tipoCampo.ToString() + "]\nConteudo: [" + mensagem.ToString() + "]", "RotinaResultado");
                    this.Invoke((MethodInvoker)delegate ()
                    {
                        listBox1.Items.Add("[" + tipoCampo.ToString() + "]\nConteudo: [" + mensagem.ToString() + "]");
                    });
                    break;
            }

            return 0;
        }

        public int RotinaResultado(int tipoCampo, byte[] buffer)
        {
            string mensagem = Encoding.UTF8.GetString(buffer);

            mensagem = mensagem.Substring(0, mensagem.IndexOf('\x0'));

            switch (tipoCampo)
            {
                case 1:
                    // System.Windows.Forms.MessageBox.Show("Finalizacao: [" + mensagem.ToString() + "]", "RotinaResultado");
                    this.Invoke((MethodInvoker)delegate ()
                    {
                        listBox1.Items.Add("1 - Finalizacao: [" + mensagem.ToString() + "]");
                    });
                    break;

                case 105:
                    // System.Windows.Forms.MessageBox.Show("nTipoCampo - 105 (data e hora da transação no formato aaaaMMddhhmmss)\n\nConteúdo: [" + mensagem + "]");
                    this.Invoke((MethodInvoker)delegate ()
                    {
                        listBox1.Items.Add("105 - (data e hora da transação no formato aaaaMMddhhmmss)\n\nConteúdo: [" + mensagem + "]");
                    });
                    break;

                case 121:
                    // System.Windows.Forms.MessageBox.Show("Comprovante Cliente: \n" + mensagem.ToString(), "RotinaResultado");
                    this.Invoke((MethodInvoker)delegate ()
                    {
                        listBox1.Items.Add("121 - Comprovante Cliente: \n" + mensagem.ToString());
                    });
                    break;

                case 122:
                    //  System.Windows.Forms.MessageBox.Show("Comprovante Estabelecimento: \n" + mensagem.ToString(), "RotinaResultado");
                    this.Invoke((MethodInvoker)delegate ()
                    {
                        listBox1.Items.Add("122 - Comprovante Estabelecimento: \n" + mensagem.ToString());
                    });
                    break;

                case 131:
                    //  System.Windows.Forms.MessageBox.Show("Rede Destino: [" + mensagem.ToString() + "]", "RotinaResultado");
                    this.Invoke((MethodInvoker)delegate ()
                    {
                        listBox1.Items.Add("131 - Rede Destino: [" + mensagem.ToString() + "]");
                    });
                    break;

                case 132:
                    //System.Windows.Forms.MessageBox.Show("Tipo Cartao: [" + mensagem.ToString() + "]", "RotinaResultado");
                    this.Invoke((MethodInvoker)delegate ()
                    {
                        listBox1.Items.Add("132 - Tipo Cartao: [" + mensagem.ToString() + "]");
                    });
                    break;

                case 136:
                    // System.Windows.Forms.MessageBox.Show("nTipoCampo - 136 (6 primeiras posições do cartão)\n\nConteúdo:[" + mensagem + "]", "RotinaResultado");
                    this.Invoke((MethodInvoker)delegate ()
                    {
                        listBox1.Items.Add("136 - (6 primeiras posições do cartão)\n\nConteúdo:[" + mensagem + "]");
                    });
                    break;

                case 170:
                    // System.Windows.Forms.MessageBox.Show("nTipoCampo - 170 (Venda Parcelada Estabelecimento Habilitada)\n\nConteúdo: [" + mensagem + "]", "RotinaResultado");
                    this.Invoke((MethodInvoker)delegate ()
                    {
                        listBox1.Items.Add("170 - (Venda Parcelada Estabelecimento Habilitada)\n\nConteúdo: [" + mensagem + "]");
                    });
                    break;

                case 171:
                    // System.Windows.Forms.MessageBox.Show("nTipoCampo - 171 (Número mínimo de parcelas - parcelada estabelecimento)\n\nConteúdo: [" + mensagem + "]", "RotinaResultado");
                    this.Invoke((MethodInvoker)delegate ()
                    {
                        listBox1.Items.Add("171 - (Número mínimo de parcelas - parcelada estabelecimento)\n\nConteúdo: [" + mensagem + "]");
                    });
                    break;

                case 172:
                    // System.Windows.Forms.MessageBox.Show("nTipoCampo - 172 (Número máximo de parcelas - parcelada estabelecimento)\n\nConteúdo: [" + mensagem + "]", "RotinaResultado");
                    this.Invoke((MethodInvoker)delegate ()
                    {
                        listBox1.Items.Add("172 - (Número máximo de parcelas - parcelada estabelecimento)\n\nConteúdo: [" + mensagem + "]");
                    });
                    break;

                case 173:
                    // System.Windows.Forms.MessageBox.Show("nTipoCampo - 173 (Valor mínimo por parcela - parcelada estabelecimento)\n\nConteúdo: [" + mensagem + "]", "RotinaResultado");
                    this.Invoke((MethodInvoker)delegate ()
                    {
                        listBox1.Items.Add("173 - (Valor mínimo por parcela - parcelada estabelecimento)\n\nConteúdo: [" + mensagem + "]");
                    });
                    break;

                case 174:
                    // System.Windows.Forms.MessageBox.Show("nTipoCampo - 174 (Venda parcela administradora habilitada)\n\nConteúdo: [" + mensagem + "]", "RotinaResultado");
                    this.Invoke((MethodInvoker)delegate ()
                    {
                        listBox1.Items.Add("174 - (Venda parcela administradora habilitada)\n\nConteúdo: [" + mensagem + "]");
                    });
                    break;

                case 175:
                    //System.Windows.Forms.MessageBox.Show("nTipoCampo - 175 (Número Mínimo de Parcelas - parcelada administradora)\n\nConteúdo: [" + mensagem + "]", "RotinaResultado");
                    this.Invoke((MethodInvoker)delegate ()
                    {
                        listBox1.Items.Add("175 - (Número Mínimo de Parcelas - parcelada administradora)\n\nConteúdo: [" + mensagem + "]");
                    });
                    break;

                case 176:
                    //System.Windows.Forms.MessageBox.Show("nTipoCampo - 176 (Número Máximo de Parcelas - parcelada administradora)\n\nConteúdo: [" + mensagem + "]", "RotinaResultado");
                    this.Invoke((MethodInvoker)delegate ()
                    {
                        listBox1.Items.Add("176 - (Número Máximo de Parcelas - parcelada administradora)\n\nConteúdo: [" + mensagem + "]");
                    });
                    break;

                case 2421:
                    //System.Windows.Forms.MessageBox.Show("nTipoCampo - 2421 (Função de coleta de dados adicionais do cliente habilitada)\nConteúdo:[" + mensagem + "]", "RotinaResultado");
                    this.Invoke((MethodInvoker)delegate ()
                    {
                        listBox1.Items.Add("2421 - (Função de coleta de dados adicionais do cliente habilitada)\nConteúdo:[" + mensagem + "]");
                    });
                    break;

                case 2090:
                    // System.Windows.Forms.MessageBox.Show("nTipoCampo - 2090 (Tipo do cartão lido)\n\nConteúdo[" + mensagem + "]", "RotinaResultado");
                    this.Invoke((MethodInvoker)delegate ()
                    {
                        listBox1.Items.Add("2090 - (Tipo do cartão lido)\n\nConteúdo[" + mensagem + "]");
                    });
                    break;

                default:
                    // System.Windows.Forms.MessageBox.Show("nTipoCampo: [" + tipoCampo.ToString() + "]\nConteudo: [" + mensagem.ToString() + "]", "RotinaResultado");
                    this.Invoke((MethodInvoker)delegate ()
                    {
                        listBox1.Items.Add(" [" + tipoCampo.ToString() + "]\nConteudo: [" + mensagem.ToString() + "]");
                    });
                    break;
            }
            this.Invoke((MethodInvoker)delegate ()
            {
                pnlcarregando.Visible = false;
            });
            return 0;
        }

        public static int EntradaDadosUsuario(string mensagem)
        {
            try
            {
                return int.Parse(Microsoft.VisualBasic.Interaction.InputBox(mensagem));
            }
            catch
            {
                throw new Exception("Dados de Entrada Inválidos");
            }
        }

        public static string EntradaDadosUsuarioStr(string mensagem)
        {
            return Microsoft.VisualBasic.Interaction.InputBox(mensagem);
        }

        public int TEFCancelamento(string valor, string cupomFiscal, string dataFiscal, string horario, string operador, string restricoes)
        {
            //Conversão dos parâmetros de entrada em bytes
            byte[] _valor = Encoding.ASCII.GetBytes(valor + "\0");
            byte[] _cupomFiscal = Encoding.ASCII.GetBytes(cupomFiscal + "\0");
            byte[] _dataFiscal = Encoding.ASCII.GetBytes(dataFiscal + "\0");
            byte[] _horario = Encoding.ASCII.GetBytes(horario + "\0");
            byte[] _operador = Encoding.ASCII.GetBytes(operador + "\0");
            byte[] _restricoes = Encoding.ASCII.GetBytes(restricoes + "\0");

            //Variáveis de status do andamento da operação
            int continua;
            int comando = 0;
            int tipoCampo = 0;
            short tamanhoMinimo = 0;
            short tamanhoMaximo = 0;
            byte[] buffer = new byte[20000];


            int retorno = clsTEF.Inicia(200, _valor, _cupomFiscal, _dataFiscal, _horario, _operador, _restricoes);

            while (retorno == 10000)
            {
                retorno = clsTEF.Continua(ref comando, ref tipoCampo, ref tamanhoMinimo, ref tamanhoMaximo, buffer, buffer.Length, 0);

                if (comando == 0)
                {
                    continua = RotinaResultado(tipoCampo, buffer);
                }
                else
                {
                    byte[] aux = null;
                    continua = RotinaColeta(comando, tipoCampo, ref tamanhoMinimo, ref tamanhoMaximo, buffer, buffer, ref aux);

                    if (aux != null)
                    {
                        clsTEF.LimparBuffer(buffer);

                        for (int i = 0; i < aux.Length; i++)
                        {
                            buffer[i] = aux[i];
                        }
                    }


                }
            }

            if (retorno == 0)
            {
                clsTEF.FinalizaTransacaoSiTefInterativo(1, _cupomFiscal, _dataFiscal, _horario);
            }

            return retorno;


        }

        public int TEFVendaCartaoDigitado(string numCart, string dataVenc, string codSeg, string valor, string cupomFiscal, string dataFiscal, string horario, string operador, string restricoes)
        {
            if (!TEF.Configurado)
            {
                int ret = clsTEF.Configura("127.0.0.1", "00000000", "SW000001");
            }

            int comando = 0;
            int continua = 0;
            int tipoCampo = 0;
            short tamanhoMinimo = 0;
            short tamanhoMaximo = 0;
            string msg = "";

            byte[] _valor = Encoding.ASCII.GetBytes(valor + "\0");
            byte[] _cupomFiscal = Encoding.ASCII.GetBytes(cupomFiscal + "\0");
            byte[] _dataFiscal = Encoding.ASCII.GetBytes(dataFiscal + "\0");
            byte[] _horario = Encoding.ASCII.GetBytes(horario + "\0");
            byte[] _operador = Encoding.ASCII.GetBytes(operador + "\0");
            byte[] _restricoes = Encoding.ASCII.GetBytes(restricoes + "\0");

            byte[] buffer = new byte[20000];

            int retorno = clsTEF.Inicia(3, _valor, _cupomFiscal, _dataFiscal, _horario, _operador, _restricoes);

            while (retorno == 10000)
            {
                retorno = clsTEF.Continua(ref comando, ref tipoCampo, ref tamanhoMinimo, ref tamanhoMaximo, buffer, buffer.Length, 0);

                if (comando == 0)
                {
                    continua = RotinaResultadoCartDig(tipoCampo, buffer);
                }
                else
                {
                    byte[] aux = null;
                    continua = RotinaColetaCartDig(comando, tipoCampo, ref tamanhoMinimo, ref tamanhoMaximo, buffer, buffer, ref aux);

                    if (aux != null)
                    {
                        clsTEF.LimparBuffer(buffer);

                        for (int i = 0; i < aux.Length; i++)
                        {
                            buffer[i] = aux[i];
                        }
                    }


                }
            }

            if (retorno == 0)
            {
                clsTEF.FinalizaTransacaoSiTefInterativo(1, _cupomFiscal, _dataFiscal, _horario);
            }
            else
            {
                // MessageBox.Show("Erro na finalização da transação. retorno: " + retorno);
                fmok.Mostrar("Erro na finalização da transação. retorno: " + retorno);
                clsTEF.FinalizaTransacaoSiTefInterativo(0, _cupomFiscal, _dataFiscal, _horario);
                try
                {
                    Cancelado = true;
                    int index = grid.Rows.Count - 1;
                    if (index >= 0) contareceber.removerParcela(int.Parse(dt.Rows[index]["par_id"].ToString()));
                    Carregar(false);
                }
                catch { }

                Invoke((MethodInvoker)delegate ()
                {
                    Close();
                });

                if (pnlmeiopagamento.Visible)
                {
                    pnlmeiopagamento.Visible = false;
                }
                else
                    btncancelar.PerformClick();
            }


            return retorno;
        }

        public int TEFVenda(int funcao, string valor, string cupomFiscal, string dataFiscal, string horario, string operador, string restricoes, bool finaliza)
        {
            if (!TEF.Configurado)
            {
                int ret = clsTEF.Configura("127.0.0.1", "00000000", "SW000001");
            }

            int comando = 0;
            int continua = 0;
            int tipoCampo = 0;
            short tamanhoMinimo = 0;
            short tamanhoMaximo = 0;
            string msg = "";

            byte[] _valor = Encoding.ASCII.GetBytes(valor + "\0");
            byte[] _cupomFiscal = Encoding.ASCII.GetBytes(cupomFiscal + "\0");
            byte[] _dataFiscal = Encoding.ASCII.GetBytes(dataFiscal + "\0");
            byte[] _horario = Encoding.ASCII.GetBytes(horario + "\0");
            byte[] _operador = Encoding.ASCII.GetBytes(operador + "\0");
            byte[] _restricoes = Encoding.ASCII.GetBytes(restricoes + "\0");

            byte[] buffer = new byte[20000];

            int retorno = clsTEF.Inicia(funcao, _valor, _cupomFiscal, _dataFiscal, _horario, _operador, _restricoes);

            while (retorno == 10000)
            {
                retorno = clsTEF.Continua(ref comando, ref tipoCampo, ref tamanhoMinimo, ref tamanhoMaximo, buffer, buffer.Length, 0);

                if (comando == 0)
                {
                    continua = RotinaResultado(tipoCampo, buffer);
                }
                else
                {
                    byte[] aux = null;
                    continua = RotinaColeta(comando, tipoCampo, ref tamanhoMinimo, ref tamanhoMaximo, buffer, buffer, ref aux);

                    if (aux != null)
                    {
                        clsTEF.LimparBuffer(buffer);

                        for (int i = 0; i < aux.Length; i++)
                        {
                            buffer[i] = aux[i];
                        }
                    }


                }
            }

            if (finaliza)
            {
                if (retorno == 0)
                {
                    clsTEF.FinalizaTransacaoSiTefInterativo(1, _cupomFiscal, _dataFiscal, _horario);
                }
                else
                {
                    // MessageBox.Show("Erro na finalização da transação. retorno: " + retorno);
                    fmok.Mostrar("Erro na finalização da transação. retorno: " + retorno);
                    clsTEF.FinalizaTransacaoSiTefInterativo(0, _cupomFiscal, _dataFiscal, _horario);
                    try
                    {
                        Cancelado = true;
                        int index = grid.Rows.Count - 1;
                        if (index >= 0) contareceber.removerParcela(int.Parse(dt.Rows[index]["par_id"].ToString()));
                        Carregar(true);
                    }
                    catch { }
                    Invoke((MethodInvoker)delegate ()
                    {
                        Close();
                    });

                    if (pnlmeiopagamento.Visible)
                    {
                        pnlmeiopagamento.Visible = false;
                    }
                    else
                        btncancelar.PerformClick();
                }
            }

            return retorno;
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {

            string DataHoje = DateTime.Now.ToString().Replace("/", "-").Substring(0, 10);
            string HoraHoje = DateTime.Now.ToString();
            DataHoje = DataHoje.Substring(6, 4) + DataHoje.Substring(3, 2) + DataHoje.Substring(0, 2);
            HoraHoje = HoraHoje.Substring(11, 8);
            HoraHoje = HoraHoje.Substring(0, 2) + HoraHoje.Substring(3, 2) + HoraHoje.Substring(6, 2);

            bool finaliza = false;
            bool trOk = true;

            idmeiopagamento = Convert.ToInt32(Ptipopag);
            decimal valor = decimal.Parse(ttbtotal.Text);
            contareceber.insereContaReceber(idvenda, 1, valor, DateTime.Now.Date, true, idcaixa, 0, "Cartão Crédito", idmeiopagamento);


            ttbtroco.Text = troco.ToString("00.00");

            lstPag = new List<Pagamento>();
            lstPag.Add(new Pagamento(idmeiopagamento.ToString(), "Cartão Crédito", Convert.ToDouble(valor), idvenda.ToString()));

            CItemVenda ItemVenda = new CItemVenda();
            string cupomFiscal = ItemVenda.pesquisarproxcupom().ToString();
            string dataFiscal = DataHoje;
            string horario = HoraHoje;
            string operador = "CLIENTE";
            string restricoes = "";

            for (int i = 0; i < lstPag.Count; i++)
            {
                if (trOk)
                {
                    this.Invoke(setMensFormaPagamento, new object[] { lstPag[i].desc_pagamento.ToString() });

                    if (i == lstPag.Count - 1)
                        finaliza = true;

                    int res = TEFVenda(int.Parse(lstPag[i].id_pagamento), lstPag[i].val_pagamento.ToString("0.00"), lstPag[i].num_cupom, dataFiscal, horario, operador, restricoes, finaliza);

                    trOk = (res == 0);

                    if (trOk)
                    {
                        FechaVenda();
                    }
                }
            }

        }

        void reader_SpeakCompleted(object sender, SpeakCompletedEventArgs e)
        {
            string modelo = "IDLE";
        }

        public void falar(string texto)
        {
            ttbtroco.Text = texto;
            //     reader.Dispose();
            if (ttbtroco.Text != "")
            {

                reader = new SpeechSynthesizer();
                label2.Text = "Formas de pagamento..";
                reader.SpeakCompleted += new EventHandler<SpeakCompletedEventArgs>(reader_SpeakCompleted);
            }
            else
            {
                MessageBox.Show("Digite alguma frase", "Message", MessageBoxButtons.OK);
            }
        }

        public FPagamento(int idvenda, int idcompra, decimal valortotal, decimal desconto)
        {
            InitializeComponent();
            lbltitulo.MouseDown += new MouseEventHandler(Form3_MouseDown);
            lbltitulo.MouseMove += new MouseEventHandler(Form3_MouseMove);
            gridmeiopagamento.AutoGenerateColumns = false;
            grid.AutoGenerateColumns = false;
            gridproduto.AutoGenerateColumns = false;
            pnltitulo.BackColor = Color.FromArgb(50, Color.Black);
            ok = false;
            this.idvenda = idvenda;
            this.idcompra = idcompra;
            this.idcaixa = idcaixa;
            this.valortotal = valortotal;
            this.desconto = desconto;

            FPDV PDV = new FPDV(0, 0, 0);
            string socartao = PDV.SomenteCartao;
            if (VerCheque == "sim") button2.Visible = true;
            if (socartao == "sim")
            {
                button2.Visible = false;
                btncartao.Visible = false;
            }


            if (idvenda <= 0)
            {
                ttbdesconto.ReadOnly = true;
            }

            vetidparcela = new int[1000];
            datahora = DateTime.Now;

            Carregar(false);

            falar("Insira o seu cartão de crédito ou débito.");
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

        public void RequisitaTEF(string vendaID, string numCupom, string valor, string parcela)
        {
            INTPOS = "000-000 = CRT" + " " + @"
001-000 = " + vendaID + @"
002-000 = " + numCupom + @"
003-000 = " + valor + @"
017-000 = 1" + " " + @"
018-000 = " + parcela + @" 
999-999 = 0" + "" + @"";
            Caminhoreq = @"C:\Client\Req\INTPOS.001A";
            System.IO.File.WriteAllText(Caminhoreq, INTPOS);
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

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {


            decimal valor = 0;
            try
            {
                valor = decimal.Parse(ttbtotal.Text);
            }
            catch { }
            if (valor > 0)
            {
                if (keyData == Keys.F1)
                {
                    // abrirmeiopagamento();
                    btndinheiro.PerformClick();
                    return true;
                }


                if (keyData == Keys.F2)
                {
                    // abrirmeiopagamento();
                    btndebito.PerformClick();
                    return true;
                }


                if (keyData == Keys.F3)
                {
                    // abrirmeiopagamento();
                    btncartao.PerformClick();
                    return true;
                }


                if (keyData == Keys.F4)
                {
                    // abrirmeiopagamento();
                    btncheque.PerformClick();
                    return true;
                }


                if (keyData == Keys.F5)
                {
                    // abrirmeiopagamento();
                    btnconvenio.PerformClick();
                    return true;
                }


                if (keyData == Keys.F6)
                {
                    // abrirmeiopagamento();
                    btndesconto.PerformClick();
                    return true;
                }
            }
            //if (keyData == Keys.Enter)
            // {
            //btnconfirmar.PerformClick();
            //   return true;
            // }



            if (keyData == Keys.Escape)
            {
                try
                {
                    int index = grid.Rows.Count - 1;
                    if (index >= 0) contareceber.removerParcela(int.Parse(dt.Rows[index]["par_id"].ToString()));
                    Carregar(false);
                }
                catch { }

                Close();

                if (pnlmeiopagamento.Visible)
                {
                    pnlmeiopagamento.Visible = false;
                }
                else
                    btncancelar.PerformClick();
                return true;
            }
            if (keyData == Keys.Delete)
            {
                try
                {
                    int index = grid.Rows.Count - 1;
                    contareceber.removerParcela(int.Parse(dt.Rows[index]["par_id"].ToString()));
                    Carregar(false);
                }
                catch { }
                return true;
            }



            // Call the base class
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void abrirmeiopagamento()
        {
            pnlmeiopagamento.Visible = !pnlmeiopagamento.Visible;
            pnlmeiopagamento.Left = 0;
            pnlmeiopagamento.Top = 36;

            if (pnlmeiopagamento.Visible)
            {
                gridmeiopagamento.Focus();
                CMeioPagamento c = new CMeioPagamento();
                dtmeiopagamento = c.carregar();
                gridmeiopagamento.DataSource = dtmeiopagamento;
            }
        }

        private void btnfechar_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btnmeiopagamento_Click(object sender, EventArgs e)
        {
            abrirmeiopagamento();
        }

        private void selecionarMeioPagamento()
        {

        }

        private void gridmeiopagamento_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (dtmeiopagamento.Rows.Count > 0)
                    if (dtmeiopagamento.Rows[gridmeiopagamento.SelectedRows[0].Index][0].ToString() != "")
                    {
                        selecionarMeioPagamento();
                    }
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            decimal valor = 0;
            try
            {
                valor = decimal.Parse(ttbtotal.Text);
            }
            catch { }
            if (valor <= 0)
            {
                btndinheiro.Enabled = false;
                btncheque.Enabled = false;
                btndebito.Enabled = false;
                btncartao.Enabled = false;
                btnconvenio.Enabled = false;
                btndesconto.Enabled = false;
            }
            else
            {
                btndinheiro.Enabled = true;
                btncheque.Enabled = true;
                btndebito.Enabled = true;
                btncartao.Enabled = true;
                btnconvenio.Enabled = true;
                btndesconto.Enabled = true;
            }
        }

        private void btnsair_Click(object sender, EventArgs e)
        {
            ok = true;
            Close();
        }

        private void Carregar(bool fechar)
        {

            if (idvenda > 0)
            {
                CCaixa caixa = new CCaixa();
                idcaixa = caixa.pegaIdAberto();
                valorpago = 0;
                dt = contareceber.carregarParcelas(idvenda);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    valorpago += decimal.Parse(dt.Rows[i]["par_valor"].ToString());
                }

                if (fechar == false)
                {
                    venda = new CVenda();
                    venda.pesquisarvendaIdVenda(idvenda);
                    desconto = venda.ven_desconto;
                    grid.DataSource = dt;

                    CItemVenda c = new CItemVenda();
                    DataTable dtitemvenda = c.pesquisar(idvenda);
                    gridproduto.DataSource = dtitemvenda;
                }
            }
            else
            {

                CCaixa caixa = new CCaixa();
                idcaixa = caixa.pegaIdAberto();

            }

            mostrar();
        }

        private void limparvetor()
        {
            for (int i = 0; i < vetidparcela.Length; i++)
            {
                vetidparcela[i] = 0;
            }
        }

        private void mostrar()
        {
            try
            {
                ttbsubtotal.Text = valortotal.ToString("00.00");
                ttbdesconto.Text = desconto.ToString("00.00");
                ttbtotalpago.Text = valorpago.ToString("00.00");
                calcular();
            }
            catch { }
        }

        private void calcular()
        {
            ttbtotalpago.Text = valorpago.ToString("00.00");

            decimal totalapagar = (valortotal - valorpago - desconto);
            if (totalapagar < 0)
                totalapagar = 0;
            ttbtotal.Text = totalapagar.ToString("00.00");

            if (valorpago > valortotal - desconto)
                troco = valorpago - (valortotal - desconto);
            else
                troco = 0;

            ttbtroco.Text = troco.ToString("00.00");
        }

        private void btncancelar_Click(object sender, EventArgs e)
        {
            removerVetor();
            Carregar(false);
            if (decimal.Parse(ttbtotal.Text) <= 0)
            {
                ok = true;

            }
            else
                ok = false;
            Close();
        }

        public void FechaVenda()
        {
            removerVetor();
            Carregar(true);
            if (decimal.Parse(ttbtotal.Text) <= 0)
            {
                ok = true;

            }
            else
                ok = false;
            try
            {
                Close();
            }
            catch
            {
                this.Invoke((MethodInvoker)delegate ()
                {
                    Close();
                });
            }

            GravaListBox();
        }

        private void btnconfirmar_Click(object sender, EventArgs e)
        {
            if (idvenda <= 0)
            {
                if (troco < 0)
                {
                    return;
                }
            }


            if (idvenda > 0)
            {
                CVenda c = new CVenda();
                c.atualizarDesconto(idvenda, desconto);
            }

            if (troco >= 0)
            {
                ok = true;
            }
            else
                ok = false;

            if (idvenda > 0)
            {

                Close();
            }
            else
            {
                if (ok)
                    Close();
                else
                    Carregar(false);
            }

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void inserirVetor(int[] vetor)
        {
            bool ok = true;
            for (int i = 0; i < vetor.Length; i++)
            {
                ok = true;
                for (int j = 0; j < vetidparcela.Length; j++)
                {
                    if (ok)
                        if (vetidparcela[j] == 0)
                        {
                            ok = false;
                            vetidparcela[j] = vetor[i];

                        }
                }
            }
        }

        private void removerVetor()
        {
            for (int i = 0; i < vetidparcela.Length; i++)
            {
                if (vetidparcela[i] != 0)
                {
                    if (idvenda > 0)
                        contareceber.removerParcela(vetidparcela[i]);
                    else
                        contapagar.removerParcela(vetidparcela[i]);
                }
            }
        }

        private void ttbvalor_TextChanged(object sender, EventArgs e)
        {
            calcular();
        }

        private void FPagamento_Load(object sender, EventArgs e)
        {
            datahora = DateTime.Now;
        }

        public FPagamento(double t, List<Pagamento> lstPag)
        {
            InitializeComponent();

            //  string str = lstPag[0].desc_pagamento;

            //  TEF = new clsTEF();

            //  setMensOp += new dSetMensOp(setMsgOp);
            //  setMensCli += new dSetMensCli(setMsgCli);
            //  setMensDoisVisores += new dSetMensDoisVisores(setMsgDoisVisores);
            //  setMensFormaPagamento += new dSetMensFormaPagamento(setMsgFormaPag);
            ////  fechaJanela += new dFechaJanela(fecha);

            //  TOTAL = t;
            //  this.lstPag = lstPag;
        }

        private void btnenter_Click(object sender, EventArgs e)
        {



        }

        private void gridmeiopagamento_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (dtmeiopagamento.Rows.Count > 0)
                selecionarMeioPagamento();

        }

        private void ttbvalor_KeyDown(object sender, KeyEventArgs e)
        {

        }

        private void lbltitulo_Click(object sender, EventArgs e)
        {

        }

        private void gridproduto_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            this.gridproduto.Rows[e.RowIndex].Cells[0].Value
= (e.RowIndex + 1).ToString();
        }

        private void btndesconto_Click(object sender, EventArgs e)
        {
            FInput f = new FInput();
            if (f.Mostrar("Aplicar Desconto", "Informe a porcentagem de desconto"))
            {
                try
                {
                    //ttbdesconto.Text = (decimal.Parse(ttbdesconto.Text) + decimal.Parse(f.valor)).ToString("00.00");
                    CVenda c = new CVenda();
                    decimal desconto = decimal.Parse(ttbsubtotal.Text) * (decimal.Parse(f.valor) / 100);
                    c.aplicardesconto(idvenda, desconto);
                    Carregar(false);
                }
                catch { }
            }
        }

        private void btndinheiro_Click(object sender, EventArgs e)
        {
            Ptipopag = "01";
            FInput f = new FInput();
            f.HabilitaSenha = "sim";
            if (f.Mostrar("Dinheiro", "Informe o valor de pagamento", ttbtotal.Text))
            {
                try
                {
                    decimal valor = decimal.Parse(f.valor);
                    if (valor <= 0)
                    {
                        fmok.Mostrar("Valor de pagamento inválido!");
                        return;
                    }
                    idmeiopagamento = 1;
                    if (valor > decimal.Parse(ttbtotal.Text))
                    {
                        decimal troco = valor - decimal.Parse(ttbtotal.Text); ;
                        valor = decimal.Parse(ttbtotal.Text);

                        contareceber.insereContaReceber(idvenda, 1, valor, DateTime.Now.Date, true, idcaixa, 0, "Dinheiro", idmeiopagamento);

                        idmeiopagamento = 0;
                        Carregar(false);
                        ttbtroco.Text = troco.ToString("00.00");
                    }
                    else
                    {
                        contareceber.insereContaReceber(idvenda, 1, valor, DateTime.Now.Date, true, idcaixa, 0, "Dinheiro", idmeiopagamento);

                        idmeiopagamento = 0;
                        FechaVenda();
                        //   Carregar();
                    }


                }
                catch { fmok.Mostrar("Valor de pagamento inválido!"); }
            }
        }

        private void grid_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            /*  try
              {
                  int id = int.Parse(dt.Rows[e.RowIndex]["par_id"].ToString());
                  CContaReceber c = new CContaReceber();
                  c.removerParcela(id);
                  Carregar();
              }
              catch { }*/

        }

        private void btncartao_Click(object sender, EventArgs e)
        {

        }

        private void btndebito_Click(object sender, EventArgs e)
        {
            FInput f = new FInput();
            if (f.Mostrar("Débito", "Confirme o valor de pagamento", ttbtotal.Text))
            {
                try
                {
                    decimal valorpagamento = decimal.Parse(f.valor);
                    if (valorpagamento <= 0)
                    {
                        fmok.Mostrar("Valor de pagamento inválido!");
                        return;
                    }

                    idmeiopagamento = 1;
                    decimal restante = decimal.Parse(ttbtotal.Text);
                    /*if (valorpagamento > restante)
                    {
                        decimal troco = valorpagamento - restante;
                        valordepagamento = restante;
                    }*/
                    MGM mgm = new MGM();

                    CContaReceber c = new CContaReceber();
                    avista = true;
                    c.insereContaReceber(idvenda, 1, valorpagamento, DateTime.Now, avista, idcaixa, idcliente, "Cartão de Débito", 4);

                    Carregar(false);
                    troco.ToString("00.00");

                }
                catch
                {
                    fmok.Mostrar("Valor de pagamento inválido!");
                }


            }
        }

        private void btncheque_Click(object sender, EventArgs e)
        {
            Ptipopag = "02";
            FInput f = new FInput();
            FInputDataPicker fdata = new FInputDataPicker("Primeiro vencimento", DateTime.Now.AddMonths(1), "Quantidade de Parcelas", "1");

            if (f.Mostrar("Cheque", "Confirme o valor de pagamento", ttbtotal.Text))
            {
                try
                {
                    decimal valorpagamento = decimal.Parse(f.valor);
                    if (valorpagamento <= 0)
                    {
                        fmok.Mostrar("Valor de pagamento inválido!");
                        return;
                    }
                    try
                    {
                        fdata.ShowDialog();

                        if (fdata.ok)
                        {
                            int quantidade = int.Parse(fdata.valor);
                            if (quantidade <= 0)
                            {
                                fmok.Mostrar("Quantidade de Parcelas inválido!");
                                return;
                            }


                            DateTime data = fdata.data;
                            idmeiopagamento = 2;
                            decimal restante = decimal.Parse(ttbtotal.Text);
                            /*if (valorpagamento > restante)
                            {
                                decimal troco = valorpagamento - restante;
                                valordepagamento = restante;
                            }*/
                            MGM mgm = new MGM();
                            decimal[] vetparcela = mgm.gerarParcelamento(quantidade, valorpagamento);
                            for (int i = 0; i < vetparcela.Length; i++)
                            {
                                CContaReceber c = new CContaReceber();
                                avista = false;
                                c.insereContaReceber(idvenda, i + 1, vetparcela[i], data.AddMonths(i), avista, idcaixa, idcliente, "Cheque", 2);
                            }

                            troco.ToString("00.00");

                            FechaVenda();
                        }


                    }
                    catch { fmok.Mostrar("Quantidade de Parcelas inválido!"); }
                }
                catch
                {
                    fmok.Mostrar("Valor de pagamento inválido!");
                }


            }
        }

        private void btnconvenio_Click(object sender, EventArgs e)
        {
            FInput f = new FInput();
            FInputDataPicker fdata = new FInputDataPicker("Primeiro vencimento!", DateTime.Now.AddMonths(1), "Quantidade de parcelas", "1");

            if (f.Mostrar("Convenio", "Informe o valor de pagamento", ttbtotal.Text))
            {
                try
                {
                    decimal valorpagamento = decimal.Parse(f.valor);
                    if (valorpagamento <= 0)
                    {
                        fmok.Mostrar("Valor de pagamento inválido!");
                        return;
                    }
                    try
                    {
                        fdata.ShowDialog();

                        if (fdata.ok)
                        {
                            int quantidade = int.Parse(fdata.valor);
                            if (quantidade <= 0)
                            {
                                fmok.Mostrar("Quantidade de Parcelas inválido!");
                                return;
                            }


                            DateTime data = fdata.data;

                            if (venda.cli_id <= 0)
                            {
                                FConsultaCliente fcli = new FConsultaCliente();
                                fcli.ShowDialog();
                                if (!fcli.ok)
                                {
                                    fmok.Mostrar("Cliente inválido!");
                                    return;

                                }
                                idcliente = int.Parse(fcli.dt.Rows[fcli.index]["cli_id"].ToString());
                                venda.atualizarClienteFuncionario(venda.ven_id, idcliente, venda.fun_id);

                            }
                            idmeiopagamento = 1;
                            decimal restante = decimal.Parse(ttbtotal.Text);
                            /*if (valorpagamento > restante)
                            {
                                decimal troco = valorpagamento - restante;
                                valordepagamento = restante;
                            }*/
                            MGM mgm = new MGM();
                            decimal[] vetparcela = mgm.gerarParcelamento(quantidade, valorpagamento);
                            for (int i = 0; i < vetparcela.Length; i++)
                            {
                                CContaReceber c = new CContaReceber();
                                avista = false;
                                c.insereContaReceber(idvenda, i + 1, vetparcela[i], data.AddMonths(i), avista, idcaixa, idcliente, "Marcar", 5);
                            }


                            Carregar(false);
                            troco.ToString("00.00");
                            /* if (valorpagamento > restante)
                             {
                                 ttbtroco.Text = troco.ToString("00.00");

                                 idmeiopagamento = 0;
                                 Carregar();
                             }*/
                        }


                    }
                    catch { fmok.Mostrar("Quantidade de Parcelas inválido!"); }
                }
                catch
                {
                    fmok.Mostrar("Valor de pagamento inválido!");
                }


            }
        }

        private void btncancelardesconto_Click(object sender, EventArgs e)
        {
            CVenda venda = new CVenda();
            venda.pesquisarvendaIdVenda(idvenda);
            decimal desconto = -1 * venda.ven_desconto;
            venda.aplicardesconto(idvenda, desconto);
            Carregar(false);
        }

        private void grid_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dt.Rows.Count > 0)
            {
                if (e.RowIndex >= 0)
                {
                    if (e.ColumnIndex == 0)
                    {
                        try
                        {
                            int id = int.Parse(dt.Rows[e.RowIndex]["par_id"].ToString());
                            CContaReceber c = new CContaReceber();
                            c.removerParcela(id);
                            Carregar(false);
                        }
                        catch { }
                    }
                }

            }
        }

        private void TimerSts_Tick(object sender, EventArgs e)
        {
            string fileName = @"C:\Client\Resp\IntPos.Sts"; ;
            if (File.Exists(fileName))
            {
                File.Delete(fileName);
            }

            string fileName2 = @"C:\Client\Resp\IntPos.001";
            if (File.Exists(fileName2))
            {
                // File.Delete(fileName2);
                TimerSts.Enabled = false;
                FechaVenda();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                Cancelado = true;
                int index = grid.Rows.Count - 1;
                if (index >= 0) contareceber.removerParcela(int.Parse(dt.Rows[index]["par_id"].ToString()));
                Carregar(false);
            }
            catch { }

            Close();

            if (pnlmeiopagamento.Visible)
            {
                pnlmeiopagamento.Visible = false;
            }
            else
                btncancelar.PerformClick();

        }

        private void button3_MouseClick(object sender, MouseEventArgs e)
        {
            // double t;
            //  List<Pagamento> lstPag  ;
            string str = "Cartão Débito";

            TEF = new clsTEF();

            setMensOp += new dSetMensOp(setMsgOp);
            setMensCli += new dSetMensCli(setMsgCli);
            setMensDoisVisores += new dSetMensDoisVisores(setMsgDoisVisores);
            setMensFormaPagamento += new dSetMensFormaPagamento(setMsgFormaPag);
            //  fechaJanela += new dFechaJanela(fecha);

            TOTAL = Convert.ToDouble(TotalPago);
            //  this.lstPag = lstPag;
        }

        private void backgroundWorker1_ProgressChanged(object sender, System.ComponentModel.ProgressChangedEventArgs e)
        {

        }

        private void bwCanc_DoWork(object sender, DoWorkEventArgs e)
        {
            string DataHoje = DateTime.Now.ToString().Replace("/", "-").Substring(0, 10);
            string HoraHoje = DateTime.Now.ToString();
            DataHoje = DataHoje.Substring(6, 4) + DataHoje.Substring(3, 2) + DataHoje.Substring(0, 2);
            HoraHoje = HoraHoje.Substring(11, 8);
            HoraHoje = HoraHoje.Substring(0, 2) + HoraHoje.Substring(3, 2) + HoraHoje.Substring(6, 2);

            CItemVenda ItemVenda = new CItemVenda();
            string cupomFiscal = ItemVenda.pesquisarproxcupom().ToString();
            //string cupomFiscal = "12345";
            string dataFiscal = DataHoje;
            string horario = HoraHoje;
            string operador = "CLIENTE";
            string restricoes = "";

            TEFCancelamento(TOTAL.ToString("0.00"), cupomFiscal, dataFiscal, horario, operador, restricoes);

        }

        private void button4_MouseClick(object sender, MouseEventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            // double t;
            //  List<Pagamento> lstPag  ;
            Ptipopag = "03";
            string str = "Cartão Crédito";

            TEF = new clsTEF();

            setMensOp += new dSetMensOp(setMsgOp);
            setMensCli += new dSetMensCli(setMsgCli);
            setMensDoisVisores += new dSetMensDoisVisores(setMsgDoisVisores);
            setMensFormaPagamento += new dSetMensFormaPagamento(setMsgFormaPag);
            //  fechaJanela += new dFechaJanela(fecha);

            TOTAL = Convert.ToDouble(TotalPago);

            bwVenda.RunWorkerAsync();

            idmeiopagamento = 0;
            //       FechaVenda();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Ptipopag = "04";
            string str = "Cartão Débito";

            TEF = new clsTEF();

            setMensOp += new dSetMensOp(setMsgOp);
            setMensCli += new dSetMensCli(setMsgCli);
            setMensDoisVisores += new dSetMensDoisVisores(setMsgDoisVisores);
            setMensFormaPagamento += new dSetMensFormaPagamento(setMsgFormaPag);
            //  fechaJanela += new dFechaJanela(fecha);

            TOTAL = Convert.ToDouble(TotalPago);

            bwVenda.RunWorkerAsync();

            idmeiopagamento = 0;
        }
    }
}
