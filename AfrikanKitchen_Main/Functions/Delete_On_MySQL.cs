using MySql.Data.MySqlClient;

namespace AfrikanKitchen_Main.Functions
{
    public class Delete_On_MySQL
    {
        public static string Delete1Item(string row, string table)
        {
            try
            {
                using (MySqlConnection con = new MySqlConnection(DataLibrary.connectionString))
                {
                    con.Open();
                    string insertQUERY = "DELETE FROM " + table + " WHERE ID=@ID";
                    MySqlCommand cmd = new MySqlCommand(insertQUERY, con);
                    cmd.Parameters.AddWithValue("@ID", row);
                    cmd.ExecuteNonQuery();

                    return "success";
                }
            }
            catch (Exception e)
            {
                return "Error: " + e;
            }
        }

        public static string DeleteItemWHERE1(string tableName, string where, string val1)
        {
            try
            {
                using (MySqlConnection con = new MySqlConnection(DataLibrary.connectionString))
                {
                    con.Open();
                    string insertQUERY = "DELETE FROM " + tableName + " WHERE " + where + "=@val";
                    MySqlCommand cmd = new MySqlCommand(insertQUERY, con);
                    cmd.Parameters.AddWithValue("@val", val1);
                    cmd.ExecuteNonQuery();

                    return "success";
                }
            }
            catch (Exception e)
            {
                return "Error: " + e;
            }
        }

        public static string DeleteItemWHERE2(string tableName, string where1, string val1, string where2, string val2)
        {
            try
            {
                using (MySqlConnection con = new MySqlConnection(DataLibrary.connectionString))
                {
                    con.Open();
                    string insertQUERY = "DELETE FROM " + tableName + " WHERE " + where1 + "=@val1 AND " + where2 + "=@val2";
                    MySqlCommand cmd = new MySqlCommand(insertQUERY, con);
                    cmd.Parameters.AddWithValue("@val1", val1);
                    cmd.Parameters.AddWithValue("@val2", val2);
                    cmd.ExecuteNonQuery();

                    return "success";
                }
            }
            catch (Exception e)
            {
                return "Error: " + e;
            }
        }
    }
}
