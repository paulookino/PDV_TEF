using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.IO;

namespace MGMPDV.TEF
{
    class clsTEF
    {
        private static int _tamanhoRecebido = 0;
        private static bool _configurado = false;

        public bool Configurado
        {
            get { return _configurado; }
        }

        private static byte[] _recebido = new byte[20000];



        public clsTEF()
        { }

        public void setText(System.Windows.Forms.Label lbl, string txt)
        {
            lbl.Text = txt;
        }

        public enum formaPagamento { Cheque = 1, Debito = 2, Credito = 3 };





        public static int Configura(string endereco, string loja, string terminal)
        {
            //TESTE
            string str = Directory.GetCurrentDirectory();
            //-------------------------------------------

            byte[] _endereco = Encoding.ASCII.GetBytes(endereco + "\0");
            byte[] _loja = Encoding.ASCII.GetBytes(loja + "\0");
            byte[] _terminal = Encoding.ASCII.GetBytes(terminal + "\0");

            try
            {
                int result = clsTEF.ConfiguraIntSiTefInterativo(_endereco, _loja, _terminal, 0);

                _configurado = (result == 0);

                return result;
            }
            catch (System.Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message, "Erro");
            }

            return -999;
        }

        public static void LimparBuffer(byte[] buffer)
        {
            for (int i = 0; i < buffer.Length; i++)
            {
                buffer[i] = 0;
            }
        }

        private int LeCampo(short tamanhoMinimo, short tamanhoMaximo, byte[] pMensagem, byte[] pCampo)
        {
            return 0;
        }



        public static int Configura(byte[] pEnderecoIP, byte[] pCodigoLoja, byte[] pNumeroTerminal, short ConfiguraResultado)
        {
            return ConfiguraIntSiTefInterativo(pEnderecoIP, pCodigoLoja, pNumeroTerminal, ConfiguraResultado);
        }

        public static int Inicia(int Funcao, byte[] pValor, byte[] pCupomFiscal, byte[] pDataFiscal, byte[] pHorario, byte[] pOperador, byte[] pRestricoes)
        {
            return IniciaFuncaoSiTefInterativo(Funcao, pValor, pCupomFiscal, pDataFiscal, pHorario, pOperador, pRestricoes);
        }

        public static int Continua(ref int pComando, ref int pTipoCampo, ref short pTamMinimo, ref short pTamMaximo, byte[] pBuffer, int TamBuffer, int Continua)
        {
            return ContinuaFuncaoSiTefInterativo(ref pComando, ref pTipoCampo, ref pTamMinimo, ref pTamMaximo, pBuffer, TamBuffer, Continua);
        }

        public static void Finaliza(short pConfirma, byte[] pCupomFiscal, byte[] pDataFiscal, byte[] pHoraFiscal)
        {
            Finaliza(pConfirma, pCupomFiscal, pDataFiscal, pHoraFiscal);
        }

        [DllImport("CliSiTef32I.dll", EntryPoint = "ConfiguraIntSiTefInterativo", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern int ConfiguraIntSiTefInterativo(byte[] pEnderecoIP, byte[] pCodigoLoja, byte[] pNumeroTerminal, short ConfiguraResultado);

        [DllImport("CliSitef32I.dll", EntryPoint = "IniciaFuncaoSiTefInterativo", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern int IniciaFuncaoSiTefInterativo(int Funcao, byte[] pValor, byte[] pCupomFiscal, byte[] pDataFiscal, byte[] pHorario, byte[] pOperador, byte[] pRestricoes);

        [DllImport("CliSitef32I.dll", EntryPoint = "ContinuaFuncaoSiTefInterativo", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern int ContinuaFuncaoSiTefInterativo(ref int pComando, ref int pTipoCampo, ref short pTamMinimo, ref short pTamMaximo, byte[] pBuffer, int TamBuffer, int Continua);

        [DllImport("CliSitef32I.dll", EntryPoint = "FinalizaTransacaoSiTefInterativo", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern void FinalizaTransacaoSiTefInterativo(short pConfirma, byte[] pCupomFiscal, byte[] pDataFiscal, byte[] pHoraFiscal);

        [DllImport("CliSiTef32I.dll", EntryPoint = "EnviaRecebeSiTefDireto", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern int EnviaRecebeSiTefDireto(short RedeDestino, short FuncaoSiTef, short OffsetCartao, byte[] pDadosTx, short TamDadosTx, byte[] pDadosRx, short TamMaxDadosRx, short[] pCodigoResposta, short TempoEsperaRx, byte[] pNumeroCupon, byte[] pDataFiscal, byte[] pHorario, byte[] pOperador, short TipoTransacao);

        [DllImport("CliSiTef32I.dll", EntryPoint = "LeSimNaoPinPad", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern int LeSimNaoPinPad(byte[] pMsgDisplay);

        [DllImport("CliSiTef32I.dll", EntryPoint = "AbrePinPad", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern int AbrePinPad();

        [DllImport("CliSiTef32I.dll", EntryPoint = "FechaPinPad", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern int FechaPinPad();

        [DllImport("CliSiTef32I.dll", EntryPoint = "LeCartaoDireto", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern int LeCartaoDireto(byte[] pMsgDisplay, byte[] trilha1, byte[] trilha2);

    }
}
