using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Windows.Forms;
using FirebirdSql.Data.FirebirdClient;
using MGMPDV.Classes;


namespace MGMPDV.Classes
{
    class SQLBanco
    {
        private string strcon =
        "Data Source=sql7003.site4now.net;" +
        "Initial Catalog=DB_A2EE18_portal;" +
        "User id=DB_A2EE18_portal_admin;" +
        "Password=4rtr4x2308;";
        private SqlConnection con = null;
        private SqlTransaction trans = null;

        CIniFile Ini = new CIniFile();
        string basedados = "";

        public SQLBanco()
        {
            //TextReader f = null;
            //try{
            //    f = File.OpenText(Application.StartupPath + "\\Conexao.ini");
            //}
            //catch { MessageBox.Show("Erro ao abrir arquivo de conexão"); return; }

            //string caminho = f.ReadLine();

            //f.Close();

            //if (caminho.Trim()=="")
            //{
            //    MessageBox.Show("Erro caminho do banco invalido");
            //}

            Ini.IniFile("checkout");
            basedados = Ini.IniReadString("base", "banco", "");

            strcon = strcon.Replace("@caminho", basedados);
        }

        public bool conecta()
        {
            bool resultado = false;
            try
            {

                con = new SqlConnection(strcon);
                con.Open();
                resultado = true;
            }
            catch (Exception ex)
            {
                // Console.WriteLine("{0} Exception Caught.", ex);
                // MessageBox.Show(ex.ToString());
            }

            return resultado;
        }

        public void desconecta() //con!=null existe uma conexao 
        {
            if ((con != null) && (con.State == System.Data.ConnectionState.Open))
            {
                con.Close();
                con = null;
            }
        }

        public void beginTransaction()
        {
            if ((con != null) && (con.State == System.Data.ConnectionState.Open))
                trans = con.BeginTransaction();
        }

        public void commitTransaction()
        {
            if ((con != null) && (trans != null) && (con.State == System.Data.ConnectionState.Open))
            {
                trans.Commit();
                trans = null;
            }
        }

        public void rollbackTransaction()
        {
            if ((con != null) && (trans != null) && (con.State == System.Data.ConnectionState.Open))
            {
                trans.Rollback();
                trans = null;
            }
        }
        //params combinação de dois valores [][]- 0 sera nome do parametro e outro valor
        public bool executeQuery(String sql, out DataTable dt, params Object[] parametros) //metodo que retorna v ou f joga pro adaptador e carrega no dataTable
        {
            dt = new DataTable();
            try
            {
                SqlCommand cmd = new SqlCommand(sql, con);
                for (int i = 0; i < parametros.Length; i += 2)
                    cmd.Parameters.AddWithValue(parametros[i].ToString(), parametros[i + 1]);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                //MessageBox.Show(cmd.Parameters[0].ToString());
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool executeNonQuery(String sql, params Object[] parametros)//metodo com comando sql
        {
            try
            {

                SqlCommand cmd = new SqlCommand(sql, con);
                cmd.Transaction = trans;
                for (int i = 0; i < parametros.Length; i += 2)
                    cmd.Parameters.AddWithValue(parametros[i].ToString(), parametros[i + 1]);
                cmd.ExecuteNonQuery(); //não retorna dados so retorna se deu certo ou nao
                //MessageBox.Show(cmd.Parameters[0].ToString());
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                //Console.WriteLine(ex.Message);
                return false;
            }
        }

        public int getIdentity()
        {
            SqlCommand cmd = new SqlCommand("SELECT @@IDENTITY", con);
            cmd.Transaction = trans;
            object o = cmd.ExecuteScalar();
            if (o != null)
                return Convert.ToInt32(o);
            else
                return 0;
        }

        public int executeScalar(string sql)
        {
            SqlCommand cmd = new SqlCommand(sql, con);
            cmd.Transaction = trans;
            object o = cmd.ExecuteScalar();
            if (o != DBNull.Value)
                return (Convert.ToInt32(o));
            else
                return 0;
        }

        public int executeScalar(string sql, params Object[] parametros)
        {
            SqlCommand cmd = new SqlCommand(sql, con);
            cmd.Transaction = trans;
            for (int i = 0; i < parametros.Length; i += 2)
                cmd.Parameters.AddWithValue(parametros[i].ToString(), parametros[i + 1]);
            object o = cmd.ExecuteScalar();
            if (o != DBNull.Value)
                return (Convert.ToInt32(o));
            else
                return 0;
        }

    }
}

