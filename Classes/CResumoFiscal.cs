using Classes.FBBanco;
using System;
using System.Data;

namespace MGMPDV.Classes
{
    class CResumoFiscal
    {
        public int id { get; set; } // id tabela
        public int nrz { get; set; } // Numero da reducao Z N
        public int ico { get; set; } // COO inicial N
        public int fco { get; set; } // COO Final   N
        public decimal vgt { get; set; } // Valor do GT N
        public decimal vmv { get; set; } // Valor do Movimento(bruto)  N
        public decimal vds { get; set; } // Valor dos descontos N
        public decimal vcn { get; set; } // Valor dos Cancelamentos N
        public DateTime dmv { get; set; } // Data do movimento C
        public string seq { get; set; } // Numero de serie do ECF C
        public string cro { get; set; } // Contador de reinicio de operacao    C
        public string mfd { get; set; } // Mfd adicional   C
        public string tip { get; set; } // Tipo de emissor C
        public string mar { get; set; } // Marca do ECF C
        public string mod { get; set; } // Modelo do ECF C
        public string vsb { get; set; } // Versao Software Basico  C
        public string dsb { get; set; } // Data Software Basico C
        public string hsb { get; set; } // Hora Software Basico    C
        public string nse { get; set; } // Sequencial C
        public int nou { get; set; } // Numero do usuario N
        public int nef { get; set; } // Numero do ECF N

        public CResumoFiscal()
        {

        }

        public int inserir(int id,int nrz,int ico,int fco,int vgt,int vmv,int vds,int vcn,DateTime dmv,string seq,
        string cro,string mfd,string tip,string mar,string mod,string vsb,string dsb,string hsb,string nse,int nou,int nef)
        {
            FBBanco fb = new FBBanco();
            int idvenda = 0; ;
            if (fb.conecta())
            {
                idvenda = fb.executeScalar(@"insert into Venda(id,nrz,ico,fco,vgt,vmv,vds,vcn,dmv,seq,cro,mfd,tip,mar,mod,vsb,dsb,hsb,nse,nou,nef
                    ) values ( @id,@nrz,@ico,@fco,@vgt,@vmv,@vds,@vcn,@dmv,@seq,@cro,@mfd,@tip,@mar,@mod,@vsb,@dsb,@hsb,@nse,@nou,@nef
                    ) returning @id",id,"@nrz",nrz,"@ico",ico,"@fco",fco, "@vgt",vgt,"@vmv",vmv, "@vds",vds,"@vcn",vcn,"@dmv",dmv,"@seq",seq,
        "@cro", cro, "@mfd", mfd, "@tip", tip, "@mar", mar, "@mod", mod, "@vsb", vsb, "@dsb", dsb, "@hsb", hsb, "@nse", nse, "@nou", nou, "@nef", nef
                    );

                fb.desconecta();
                id = idvenda;
                return idvenda;

            }
            id = idvenda;
            return idvenda;
        }

        public DataTable pesquisarResuno(DateTime data)
        {
            string DT1 = Convert.ToString(data).ToString().Substring(0, 10);
            DT1 = DT1.Replace("/", ".");

            DataTable dt = new DataTable();
            FBBanco fb = new FBBanco();
            if (fb.conecta())
            {
                fb.executeQuery(@"select * from ResumoFiscal v 
                                  where (dmv = @data) ",
                                 out dt, "@data", DT1);
                fb.desconecta();
            }

            if (dt.Rows.Count > 0)
            {
                id = int.Parse(dt.Rows[0]["id"].ToString());
                nrz = int.Parse(dt.Rows[0]["nrz"].ToString());
                ico = int.Parse(dt.Rows[0]["ico"].ToString());
                fco = int.Parse(dt.Rows[0]["fco"].ToString());
                vgt = decimal.Parse(dt.Rows[0]["vgt"].ToString());
                vmv = decimal.Parse(dt.Rows[0]["vmv"].ToString());
                vds = decimal.Parse(dt.Rows[0]["vds"].ToString());
                vcn = decimal.Parse(dt.Rows[0]["vcn"].ToString());
                dmv = DateTime.Parse(dt.Rows[0]["dmv"].ToString());
                seq = dt.Rows[0]["seq"].ToString();
                cro = dt.Rows[0]["cro"].ToString();
                mfd = dt.Rows[0]["mfd"].ToString();
                tip = dt.Rows[0]["tip"].ToString();
                mar = dt.Rows[0]["mar"].ToString();
                mod = dt.Rows[0]["mod"].ToString();
                vsb = dt.Rows[0]["vsb"].ToString();
                dsb = dt.Rows[0]["dsb"].ToString();
                hsb = dt.Rows[0]["hsb"].ToString();
                nse = dt.Rows[0]["nse"].ToString();
                nou = int.Parse(dt.Rows[0]["nou"].ToString());
                nef = int.Parse(dt.Rows[0]["nef"].ToString());
            }

            return dt;
        }


    }
}
