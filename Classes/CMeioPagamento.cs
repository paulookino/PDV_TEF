using Classes.FBBanco;
using System.Data;

namespace MGMPDV.Classes
{
    class CMeioPagamento
    {
        public int mei_id { get; set; }
        public string mei_nome { get; set; }
        public decimal mei_avista { get; set; }
        public string mei_tpg { get; set; }
        public string mei_cac { get; set; }
        public string mei_cac_des { get; set; }

        public CMeioPagamento()
        {

        }

        public DataTable pesquisaIdCartao(string cartao)
        {
            DataTable dt = new DataTable();
            FBBanco fb = new FBBanco();
            if (fb.conecta())
            {
                fb.executeQuery(@"select * from meiopagamento where mei_nome=@nomecartao",
                                 out dt, "@nomecartao", cartao);
                fb.desconecta();
            }

            if (dt.Rows.Count > 0)
            {
                mei_id = int.Parse(dt.Rows[0]["mei_id"].ToString());
                mei_nome = dt.Rows[0]["mei_nome"].ToString();
                mei_avista = decimal.Parse(dt.Rows[0]["mei_avista"].ToString());
                mei_tpg = dt.Rows[0]["mei_tpg"].ToString();
                mei_cac = dt.Rows[0]["mei_cac"].ToString();
                mei_cac_des = dt.Rows[0]["mei_cac_des"].ToString();

            }

            return dt;
        }

        public DataTable carregar()
        {
            DataTable dt = new DataTable();
            FBBanco fb = new FBBanco();
            if (fb.conecta())
            {
                fb.executeQuery(@"
                                  select * from meiopagamento order by mei_id;
                                "
                    , out dt);
                fb.desconecta();
            }

            return dt;
        }

    }
}
