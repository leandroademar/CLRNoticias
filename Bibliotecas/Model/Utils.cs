using System.Data;
using System.Data.SqlClient;
namespace Bibliotecas.Model
{
    public static class Utils
    {

        public static bool QueryPerigosa(string dsQuery)
        {

            var query = dsQuery.ToUpper();

            if (query.Contains("INSERT "))
                return true;

            if (query.Contains("INTO "))
                return true;

            if (query.Contains("DELETE "))
                return true;

            if (query.Contains("TRUNCATE "))
                return true;

            if (query.Contains("UPDATE "))
                return true;

            if (query.Contains("DROP "))
                return true;

            if (query.Contains("ALTER "))
                return true;

            if (query.Contains("CREATE "))
                return true;

            if (query.Contains("DBCC "))
                return true;

            if (query.Contains("EXEC "))
                return true;

            if (query.Contains("BACKUP "))
                return true;

            if (query.Contains("RESTORE "))
                return true;

            if (query.Contains("GRANT "))
                return true;

            if (query.Contains("REVOKE "))
                return true;

            if (query.Contains("DISABLE "))
                return true;

            if (query.Contains("sp_"))
                return true;


            return false;

        }


        public static DataTable ExecutaQueryRetornaDataTable(string dsServidor, string dsQuery)
        {

            using (var con = new SqlConnection(Servidor.Localhost.Replace("LOCALHOST", dsServidor)))
            {

                con.Open();

                using (var cmd = new SqlCommand(dsQuery, con))
                {

                    using (var sda = new SqlDataAdapter(cmd))
                    {

                        var dt = new DataTable();
                        sda.Fill(dt);

                        return dt;

                    }

                }

            }

        }


        public static string ExecutaQueryScalar(string dsServidor, string dsQuery)
        {

            string retorno;

            using (var con = new SqlConnection(Servidor.Localhost.Replace("LOCALHOST", dsServidor)))
            {

                con.Open();

                using (var cmd = new SqlCommand(dsQuery, con))
                {
                    retorno = (cmd.ExecuteScalar() == null) ? "" : cmd.ExecuteScalar().ToString();
                }
            }

            return retorno;

        }

    }

}