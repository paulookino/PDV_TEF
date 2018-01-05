using MGMPDV.Classes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Speech.Synthesis;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MGMPDV.Formularios
{
   
    public partial class PainelMerchan : Form
    {
        SpeechSynthesizer reader;

        string arqmedia = string.Empty;
        string Mensagem1 = string.Empty;
        string Mensagem2 = string.Empty;
        string Mensagem3 = string.Empty;
        string Mensagem4 = string.Empty;
        string Mensagem5 = string.Empty;
        string Mensagem6 = string.Empty;
        string Mensagem7 = string.Empty;


        CIniFile Ini = new CIniFile();


        public void falar(string texto)
        {
            textBox1.Text = texto;
            //     reader.Dispose();
            if (textBox1.Text != "")
            {

                reader = new SpeechSynthesizer();
                reader.SpeakAsync(textBox1.Text);
                button2.Enabled = true;
                reader.SpeakCompleted += new EventHandler<SpeakCompletedEventArgs>(reader_SpeakCompleted);
            }
            else
            {
                MessageBox.Show("Digite alguma frase", "Message", MessageBoxButtons.OK);
            }
        }
        void reader_SpeakCompleted(object sender, SpeakCompletedEventArgs e)
        {
           // label2.Text = "IDLE";
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
        public PainelMerchan()
        {
            InitializeComponent();

            Ini.IniFile("checkout");
            Mensagem1 = Ini.IniReadString("voz", "mensagem1", "");
            Mensagem2 = Ini.IniReadString("voz", "mensagem2", "");
            Mensagem3 = Ini.IniReadString("voz", "mensagem3", "");
            Mensagem4 = Ini.IniReadString("voz", "mensagem4", "");
            Mensagem5 = Ini.IniReadString("voz", "mensagem5", "");
            Mensagem6 = Ini.IniReadString("voz", "mensagem6", "");
            Mensagem7 = Ini.IniReadString("voz", "mensagem7", "");

            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(1024, 768);
            this.KeyPreview = true;
            this.TopMost = true;

            Ini.IniFile("checkout");
            arqmedia = Ini.IniReadString("video", "caminho", "");

            this.TopMost = true;

            iniciavideo();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog video = new OpenFileDialog();

            if (video.ShowDialog() == DialogResult.OK)
            {
                MediaVideo.URL = video.FileName;

            }
        }

        public void iniciavideo()
        {
            MediaVideo.URL =  arqmedia;
        }

        private void MediaVideo_PlayStateChange(object sender, AxWMPLib._WMPOCXEvents_PlayStateChangeEvent e)
        {
            if (MediaVideo.playState == WMPLib.WMPPlayState.wmppsStopped)
            {
                MediaVideo.Ctlcontrols.play();
            }
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            saudacao();
            System.Threading.Thread.Sleep(1500);
            Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            falar("se lasque!");
        }
    }
}
