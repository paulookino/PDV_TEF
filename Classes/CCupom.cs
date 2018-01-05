using Classes.FBBanco;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MGMPDV.Classes
{
    class CCupom
    {
        public int cup_id { get; set; }
        public int cup_numero { get; set; }
        public string cup_codigo { get; set; }
        public string cup_produto { get; set; }
        public string cup_aliquota { get; set; }
        public string cup_quantidade { get; set; }
        public decimal cup_total { get; set; }
        public DateTime cup_data { get; set; }
        public CCupom()
        {

        }

        public DataTable pesquisar(int idvenda)
        {
            DataTable dt = new DataTable();
            //FBBanco fb = new FBBanco();
            SQLBanco fb = new SQLBanco();
            if (fb.conecta())
            {
                fb.executeQuery("select * from CUPOM ",
                                 out dt, "@ven_id", idvenda);
                fb.desconecta();
            }

            return dt;
        }

        public int pesquisaTotalCupom()
        {
            int Total = 0;
            DataTable dt = new DataTable();
            //FBBanco fb = new FBBanco();
            SQLBanco fb = new SQLBanco();
            if (fb.conecta())
            {
                fb.executeQuery(@"select max(cup_id) as Numero from cupom  ", out dt, "@id", "0");
                fb.desconecta();
            }
            if (dt.Rows.Count > 0)
            {
                string num = dt.Rows[0]["Numero"].ToString();
                if (num == "") num = "0";
                Total = int.Parse(num);
            }
            return Total;
        }
            public int RetornaNumeroTotalDeCupons()
            {
                int Total = 0;
                DataTable dt = new DataTable();
            //FBBanco fb = new FBBanco();
            SQLBanco fb = new SQLBanco();
            if (fb.conecta())
                {
                    fb.executeQuery(@"select count(cup_id) as Numero from cupom  ", out dt, "@id", "0");
                    fb.desconecta();
                }
                if (dt.Rows.Count > 0)
                {
                    string num = dt.Rows[0]["Numero"].ToString();
                    if (num == "") num = "0";
                    Total = int.Parse(num);
                }

                return Total;
            }


        public void removerTodos()
        {
            //FBBanco fb = new FBBanco();
            SQLBanco fb = new SQLBanco();

            if (fb.conecta())
            {
                fb.executeNonQuery(@"delete from CUPOM
                    ",
                    "@id", "0"
                    );
                fb.desconecta();

            }
        }

        public int inserir(string cup_numero, string cup_codigo, string cup_produto, string cup_aliquota, string cup_quantidade, decimal cup_total)
        {
            //FBBanco fb = new FBBanco();
            SQLBanco fb = new SQLBanco();
            int idvenda = 0; ;
            if (fb.conecta())
            {
                idvenda = fb.executeScalar(@"insert into Cupom(cup_numero,cup_codigo,cup_produto,cup_aliquota,cup_quantidade, cup_total, cup_data
                    ) values (
                    @cup_numero,@cup_codigo,@cup_produto,@cup_aliquota,@cup_quantidade, @cup_total, @cup_data
                    ) returning cup_id", "@cup_numero", cup_numero, "@cup_codigo", cup_codigo, "@cup_produto", cup_produto, "@cup_aliquota", cup_aliquota, "@cup_quantidade", cup_quantidade, "@cup_total", cup_total, "@cup_data", DateTime.Now
                    );

                fb.desconecta();
                cup_id = idvenda;
                return idvenda;

            }
            cup_id = idvenda;
            return idvenda;
        }

        public void remover(int iditemvenda)
        {
            //FBBanco fb = new FBBanco();
            SQLBanco fb = new SQLBanco();

            if (fb.conecta())
            {
                fb.executeNonQuery(@"delete from Cupom where cup_id=@id
                    ",
                    "@id", iditemvenda
                    );
                fb.desconecta();

            }
        }

        public void removertodos(int iditemvenda)
        {
            //FBBanco fb = new FBBanco();
            SQLBanco fb = new SQLBanco();

            if (fb.conecta())
            {
                fb.executeNonQuery(@"delete from Cupom 
                    ",
                    "@id", iditemvenda
                    );
                fb.desconecta();

            }
        }

    }
}
