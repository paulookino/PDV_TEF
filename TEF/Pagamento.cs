using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MGMPDV.TEF
{
    public class Pagamento
    {
        public string id_pagamento;
        public string desc_pagamento;
        public double val_pagamento;
        public string num_cupom;

        public Pagamento(string id, string desc, double val, string numCupom)
        {
            Random rd = new Random();

            id_pagamento = id;
            desc_pagamento = desc;
            val_pagamento = val;
            num_cupom = numCupom;
        }

        public static bool numCupomExistente(string num)
        {
            List<string> txt = textoLogCupom();

            for (int i = 0; i < txt.Count; i++)
            {
                if (txt[i].Equals(num))
                    return true;
            }

            return false;
        }

        public static List<string> textoLogCupom()
        {
            try
            {
                if (!File.Exists(String.Format("{0}\\Cupons.txt", Directory.GetCurrentDirectory())))
                    File.Create(String.Format("{0}\\Cupons.txt", Directory.GetCurrentDirectory()));



                StreamReader sr = new StreamReader(String.Format("{0}\\Cupons.txt", Directory.GetCurrentDirectory()));
                List<string> txt = new List<string>();

                while (!sr.EndOfStream)
                {
                    txt.Add(sr.ReadLine());
                }

                sr.Close();

                return txt;

            }
            catch (Exception ex)
            {
                throw;
            }

        }

        private bool EscreveLogCupom(string cupom)
        {
            try
            {
                List<string> txt = textoLogCupom();

                StreamWriter sw = new StreamWriter(String.Format("{0}\\Cupons.txt", Directory.GetCurrentDirectory()));

                sw.Write(txt);

                sw.WriteLine(cupom);

                sw.Close();

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

    }
}
