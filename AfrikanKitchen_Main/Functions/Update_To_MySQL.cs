using Microsoft.VisualBasic;
using MySql.Data.MySqlClient;

namespace AfrikanKitchen_Main.Functions
{
    public class Update_To_MySql
    {
        public static string UpdateOnColumnWhereOne(string tableName, string updateColumn, string updateVal, string columnWhere, string columnVal)
        {
            try
            {
                using (MySqlConnection con = new MySqlConnection(DataLibrary.connectionString))
                {
                    con.Open();

                    //(2) Update database
                    string updateQuery = "UPDATE " + tableName + " SET " + updateColumn + "=@updateVal WHERE " + columnWhere + "=@columnVal";
                    MySqlCommand cmd2 = new MySqlCommand(updateQuery, con);
                    cmd2.Parameters.AddWithValue("@updateVal", updateVal); //1
                    cmd2.Parameters.AddWithValue("@columnVal", columnVal); //2
                    cmd2.ExecuteNonQuery();


                    //(3) Notify App
                    return "success";
                }
            }
            catch (Exception e)
            {
                return "Error: " + e;
            }
        }

        public static string UpdateTwoColumnWhereOne(string tableName, string updateColumn, string updateVal, string updateColumn2, string updateVal2, string columnWhere, string columnVal)
        {
            try
            {
                using (MySqlConnection con = new MySqlConnection(DataLibrary.connectionString))
                {
                    con.Open();

                    //(2) Update database
                    string updateQuery = "UPDATE " + tableName + " SET " + updateColumn + "=@updateVal, " + updateColumn2 + "=@updateVal2 WHERE " + columnWhere + "=@columnVal";
                    MySqlCommand cmd2 = new MySqlCommand(updateQuery, con);
                    cmd2.Parameters.AddWithValue("@updateVal", updateVal); //1
                    cmd2.Parameters.AddWithValue("@updateVal2", updateVal2); //2
                    cmd2.Parameters.AddWithValue("@columnVal", columnVal); //3
                    cmd2.ExecuteNonQuery();


                    //(3) Notify App
                    return "success";
                }
            }
            catch (Exception e)
            {
                return "Error: " + e;
            }
        }

        public static string UpdateThreeColumnWhereOne(string tableName, string updateColumn1, string updateVal1, string updateColumn2, string updateVal2, string updateColumn3, string updateVal3, string columnWhere, string columnVal)
        {
            try
            {
                using (MySqlConnection con = new MySqlConnection(DataLibrary.connectionString))
                {
                    con.Open();

                    //(2) Update database
                    string updateQuery = "UPDATE " + tableName + " SET " + updateColumn1 + "=@updateVal1, " + updateColumn2 + "=@updateVal2, " + updateColumn3 + "=@updateVal3 WHERE " + columnWhere + "=@columnVal";
                    MySqlCommand cmd2 = new MySqlCommand(updateQuery, con);
                    cmd2.Parameters.AddWithValue("@updateVal1", updateVal1); //1
                    cmd2.Parameters.AddWithValue("@updateVal2", updateVal2); //2
                    cmd2.Parameters.AddWithValue("@updateVal3", updateVal3); //3
                    cmd2.Parameters.AddWithValue("@columnVal", columnVal); //4
                    cmd2.ExecuteNonQuery();


                    //(3) Notify App
                    return "success";
                }
            }
            catch (Exception e)
            {
                return "Error: " + e;
            }
        }
        
        public static string UpdateThreeColumnWhereTwo(string tableName, string updateColumn1, string updateVal1, string updateColumn2, string updateVal2, string updateColumn3, string updateVal3, string columnWhere1, string columnVal1, string columnWhere2, string columnVal2)
        {
            try
            {
                using (MySqlConnection con = new MySqlConnection(DataLibrary.connectionString))
                {
                    con.Open();

                    //(2) Update database
                    string updateQuery = "UPDATE " + tableName + " SET " + updateColumn1 + "=@updateVal1, " + updateColumn2 + "=@updateVal2, " + updateColumn3 + "=@updateVal3 WHERE " + columnWhere2 + "=@columnVal2 AND " + columnWhere2 + "=@columnVal2";
                    MySqlCommand cmd2 = new MySqlCommand(updateQuery, con);
                    cmd2.Parameters.AddWithValue("@updateVal1", updateVal1); //1
                    cmd2.Parameters.AddWithValue("@updateVal2", updateVal2); //2
                    cmd2.Parameters.AddWithValue("@updateVal3", updateVal3); //3
                    cmd2.Parameters.AddWithValue("@columnVal1", columnVal1); //4
                    cmd2.Parameters.AddWithValue("@columnVal2", columnVal2); //5
                    cmd2.ExecuteNonQuery();


                    //(3) Notify App
                    return "success";
                }
            }
            catch (Exception e)
            {
                return "Error: " + e;
            }
        }
        
        public static string UpdateThreeColumnWhereThree(string tableName, string updateColumn1, string updateVal1, string updateColumn2, string updateVal2, string updateColumn3, string updateVal3, string columnWhere1, string columnVal1, string columnWhere2, string columnVal2, string columnWhere3, string columnVal3)
        {
            try
            {
                using (MySqlConnection con = new MySqlConnection(DataLibrary.connectionString))
                {
                    con.Open();

                    //(2) Update database
                    string updateQuery = "UPDATE " + tableName + " SET " + updateColumn1 + "=@updateVal1, " + updateColumn2 + "=@updateVal2, " + updateColumn3 + "=@updateVal3 WHERE " + columnWhere2 + "=@columnVal2 AND " + columnWhere2 + "=@columnVal2 AND " + columnWhere3 + "=@columnVal3";
                    MySqlCommand cmd2 = new MySqlCommand(updateQuery, con);
                    cmd2.Parameters.AddWithValue("@updateVal1", updateVal1); //1
                    cmd2.Parameters.AddWithValue("@updateVal2", updateVal2); //2
                    cmd2.Parameters.AddWithValue("@updateVal3", updateVal3); //3
                    cmd2.Parameters.AddWithValue("@columnVal1", columnVal1); //4
                    cmd2.Parameters.AddWithValue("@columnVal2", columnVal2); //5
                    cmd2.Parameters.AddWithValue("@columnVal3", columnVal3); //6
                    cmd2.ExecuteNonQuery();


                    //(3) Notify App
                    return "success";
                }
            }
            catch (Exception e)
            {
                return "Error: " + e;
            }
        }

        public static string UpdateTwoColumnWhereTwo(string tableName, string updateColumn, string updateVal, string updateColumn2, string updateVal2, string columnWhere, string columnVal, string columnWhere2, string columnVal2)
        {
            try
            {
                using (MySqlConnection con = new MySqlConnection(DataLibrary.connectionString))
                {
                    con.Open();

                    //(2) Update database
                    string updateQuery = "UPDATE " + tableName + " SET " + updateColumn + "=@updateVal, " + updateColumn2 + "=@updateVal2 WHERE " + columnWhere + "=@columnVal AND " + columnWhere2 + "=@columnVal2";
                    MySqlCommand cmd2 = new MySqlCommand(updateQuery, con);
                    cmd2.Parameters.AddWithValue("@updateVal", updateVal); //1
                    cmd2.Parameters.AddWithValue("@updateVal2", updateVal2); //1
                    cmd2.Parameters.AddWithValue("@columnVal", columnVal); //3
                    cmd2.Parameters.AddWithValue("@columnVal2", columnVal2); //4
                    cmd2.ExecuteNonQuery();


                    //(3) Notify App
                    return "success";
                }
            }
            catch (Exception e)
            {
                return "Error: " + e;
            }
        }

        public static string UpdateOnColumnWhereTwo(string tableName, string updateColumn, string updateVal, string columnWhere1, string columnVal1, string columnWhere2, string columnVal2)
        {
            try
            {
                using (MySqlConnection con = new MySqlConnection(DataLibrary.connectionString))
                {
                    con.Open();

                    //(2) Update database
                    string updateQuery = "UPDATE " + tableName + " SET " + updateColumn + "=@updateVal WHERE " + columnWhere1 + "=@columnVal1 AND " + columnWhere2 + "=@columnVal2";
                    MySqlCommand cmd2 = new MySqlCommand(updateQuery, con);
                    cmd2.Parameters.AddWithValue("@updateVal", updateVal); //1
                    cmd2.Parameters.AddWithValue("@columnVal1", columnVal1); //2
                    cmd2.Parameters.AddWithValue("@columnVal2", columnVal2); //3
                    cmd2.ExecuteNonQuery();


                    //(3) Notify App
                    return "success";
                }
            }
            catch (Exception e)
            {
                return "Error: " + e;
            }
        }
        
        public static string UpdateProfileForPartners(string uuid, string businessname, string tagline, string companylogo, string showlogooninvoice, string showlogoonemail)
        {
            try
            {
                using (MySqlConnection con = new MySqlConnection(DataLibrary.connectionString))
                {
                    con.Open();

                    //(2) Update database
                    string updateQuery = "UPDATE Partners SET businessname=@businessname, tagline=@tagline, companylogo=@companylogo, showlogooninvoice=@showlogooninvoice, showlogoonemail=@showlogoonemail WHERE uuid=@uuid";
                    MySqlCommand cmd2 = new MySqlCommand(updateQuery, con);
                    cmd2.Parameters.AddWithValue("@businessname", businessname); //1
                    cmd2.Parameters.AddWithValue("@tagline", tagline); //2
                    cmd2.Parameters.AddWithValue("@companylogo", companylogo); //3
                    cmd2.Parameters.AddWithValue("@showlogooninvoice", showlogooninvoice); //4
                    cmd2.Parameters.AddWithValue("@showlogoonemail", showlogoonemail); //5
                    cmd2.Parameters.AddWithValue("@uuid", uuid); //6
                    cmd2.ExecuteNonQuery();


                    //(3) Notify App
                    return "success";
                }
            }
            catch (Exception e)
            {
                return "Error: " + e;
            }
        }
        
        public static string UpdateProfileForCustomers(string uuid, string name, string image)
        {
            try
            {
                using (MySqlConnection con = new MySqlConnection(DataLibrary.connectionString))
                {
                    con.Open();

                    //(2) Update database
                    string updateQuery = "UPDATE Customers SET name=@name, image=@image WHERE uuid=@uuid";
                    MySqlCommand cmd2 = new MySqlCommand(updateQuery, con);
                    cmd2.Parameters.AddWithValue("@name", name); //1
                    cmd2.Parameters.AddWithValue("@image", image); //2
                    cmd2.Parameters.AddWithValue("@uuid", uuid); //3
                    cmd2.ExecuteNonQuery();


                    //(3) Notify App
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
