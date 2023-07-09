using AfrikanKitchen_Main.Models;
using Microsoft.VisualBasic;
using MySql.Data.MySqlClient;
using System.Collections.Generic;

namespace AfrikanKitchen_Main.Functions
{
    public class Read_From_MySQL
    {
        public static List<CartModel> Fetch_Cart_For_Customers(string uuid)
        {
            List<CartModel> instanceModel = new List<CartModel>();

            try
            {
                using (MySqlConnection con = new MySqlConnection(DataLibrary.connectionString))
                {
                    con.Open();
                    MySqlDataReader dr;
                    MySqlCommand mysqlCommand = new MySqlCommand("SELECT c.id, r.image, r.name, r.productid, r.price, c.quantity " +
                        "FROM Recipe r " +
                        "INNER JOIN Cart c ON c.productid = r.productid " +
                        "WHERE c.uuid = '" + uuid + "' " +
                        "Order By ID DESC " +
                        "LIMIT 1000", con);
                    dr = mysqlCommand.ExecuteReader();

                    while (dr.Read())
                    {
                        instanceModel.Add(new CartModel()
                        {
                            id = dr["id"].ToString(),
                            image = dr["image"].ToString(),
                            name = dr["name"].ToString(),
                            productid = dr["productid"].ToString(),
                            price = dr["price"].ToString(),
                            quantity = dr["quantity"].ToString(),
                        });
                    }

                    return instanceModel;
                }
            }
            catch (Exception)
            {
                return instanceModel;
            }
        }
        
        public static List<CardModel> Fetch_Cards_For_Customers(string uuid)
        {
            List<CardModel> instanceModel = new List<CardModel>();

            try
            {
                using (MySqlConnection con = new MySqlConnection(DataLibrary.connectionString))
                {
                    con.Open();
                    MySqlDataReader dr;
                    MySqlCommand cmd = new MySqlCommand("SELECT id, cardholder, cardnumber, cvc, expirydate, status, serverdatetime FROM Cards WHERE uuid=@uuid ORDER BY ID DESC", con);
                    cmd.Parameters.AddWithValue("@uuid", uuid);
                    dr = cmd.ExecuteReader();

                    while (dr.Read())
                    {
                        instanceModel.Add(new CardModel()
                        {
                            id = dr["id"].ToString(),
                            cardholder = dr["cardholder"].ToString(),
                            cardnumber = dr["cardnumber"].ToString(),
                            cvc = dr["cvc"].ToString(),
                            expirydate = dr["expirydate"].ToString(),
                            status = dr["status"].ToString(),
                            serverdatetime = dr["serverdatetime"].ToString(),
                        });
                    }

                    return instanceModel;
                }
            }
            catch (Exception)
            {
                return instanceModel;
            }
        }
        
        public static List<FavouritesModel> Fetch_Favourites_For_Customers(string uuid)
        {
            List<FavouritesModel> instanceModel = new List<FavouritesModel>();

            try
            {
                using (MySqlConnection con = new MySqlConnection(DataLibrary.connectionString))
                {
                    con.Open();
                    MySqlDataReader dr;
                    MySqlCommand mysqlCommand = new MySqlCommand("SELECT f.id, r.image, r.name, r.productid, r.price " +
                        "FROM Recipe r " +
                        "INNER JOIN Favourites f ON f.productid = r.productid " +
                        "WHERE f.uuid = '" + uuid + "' " +
                    "Order By ID DESC " +
                        "LIMIT 1000", con);
                    
                    dr = mysqlCommand.ExecuteReader();

                    while (dr.Read())
                    {
                        instanceModel.Add(new FavouritesModel()
                        {
                            id = dr["id"].ToString(),
                            image = dr["image"].ToString(),
                            name = dr["name"].ToString(),
                            productid = dr["productid"].ToString(),
                            price = dr["price"].ToString(),
                        });
                    }

                    return instanceModel;
                }
            }
            catch (Exception)
            {
                return instanceModel;
            }
        }

        public static List<OrdersModel> Fetch_Orders_For_Admin(string status = "all", int limit = 1000, bool isSearch = false, string keyword = "")
        {
            List<OrdersModel> instanceModel = new List<OrdersModel>();

            try
            {
                using (MySqlConnection con = new MySqlConnection(DataLibrary.connectionString))
                {
                    con.Open();
                    MySqlDataReader dr;
                    MySqlCommand sqlCommand = new MySqlCommand();
                    sqlCommand.Connection = con;

                    string subQueryA = "SELECT ID, orderid, address, deliverytime, phonenumber, status, serverdatetime, (SELECT name FROM customers c WHERE c.uuid=ucid) AS customername, (SELECT image FROM customers c WHERE c.uuid=ucid) AS customerimage From Orders ";
                    string subQueryB = " orderid LIKE '%" + keyword + "%' OR address LIKE '%" + keyword + "%' OR serverDateTime LIKE '%" + keyword + "%'";
                    string subQueryC = " ORDER BY ID DESC LIMIT " + limit + " ";

                    string query = subQueryA + (isSearch ? status == "all" ? " " + subQueryB + " " + subQueryC : "WHERE status = '" + status + "' AND (" + subQueryB + ")" + subQueryC : status == "all" ? " " + subQueryC : " WHERE  status ='" + status + "'" + subQueryC);

                    sqlCommand.CommandText = query;
                    dr = sqlCommand.ExecuteReader();

                    while (dr.Read())
                    {
                        instanceModel.Add(new OrdersModel()
                        {
                            id = dr["ID"].ToString(),
                            orderid = dr["orderid"].ToString(),
                            address = dr["address"].ToString(),
                            deliverytime = dr["deliverytime"].ToString(),
                            phonenumber = dr["phonenumber"].ToString(),
                            status = dr["status"].ToString(),
                            customername = dr["customername"].ToString(),
                            customerimage = dr["customerimage"].ToString(),
                            serverdatetime = dr["serverdatetime"].ToString(),
                        });
                    }

                    return instanceModel;
                }
            }
            catch (Exception)
            {
                return instanceModel;
            }
        }
        
        public static List<OrdersModel> Fetch_Orders_For_Partners(string uuid, string status = "all", int limit = 1000, bool isSearch = false, string keyword = "")
        {
            List<OrdersModel> instanceModel = new List<OrdersModel>();

            try
            {
                using (MySqlConnection con = new MySqlConnection(DataLibrary.connectionString))
                {
                    con.Open();
                    MySqlDataReader dr;
                    MySqlCommand sqlCommand = new MySqlCommand();
                    sqlCommand.Connection = con;

                    string subQueryA = "SELECT ID, orderid, address, deliverytime, phonenumber, status, serverdatetime, (SELECT name FROM customers c WHERE c.uuid=ucid) AS customername, (SELECT image FROM customers c WHERE c.uuid=ucid) AS customerimage From Orders ";
                    string subQueryB = " orderid LIKE '%" + keyword + "%' OR address LIKE '%" + keyword + "%' OR serverDateTime LIKE '%" + keyword + "%'";
                    string subQueryC = " ORDER BY ID DESC LIMIT " + limit + " ";

                    string query = subQueryA + (isSearch ? status == "all" ? "WHERE upid ='" + uuid + "' AND (" + subQueryB + ") " + subQueryC : "WHERE upid = '" + uuid + "' AND status = '" + status + "' AND (" + subQueryB + ")" + subQueryC : status == "all" ? " WHERE upid = '" + uuid + "' " + subQueryC : " WHERE upid = '" + uuid + "' AND status ='" + status + "'" + subQueryC);

                    sqlCommand.CommandText = query;
                    dr = sqlCommand.ExecuteReader();

                    while (dr.Read())
                    {
                        instanceModel.Add(new OrdersModel()
                        {
                            id = dr["ID"].ToString(),
                            orderid = dr["orderid"].ToString(),
                            address = dr["address"].ToString(),
                            deliverytime = dr["deliverytime"].ToString(),
                            phonenumber = dr["phonenumber"].ToString(),
                            status = dr["status"].ToString(),
                            customername = dr["customername"].ToString(),
                            customerimage = dr["customerimage"].ToString(),
                            serverdatetime = dr["serverdatetime"].ToString(),
                        });
                    }

                    return instanceModel;
                }
            }
            catch (Exception)
            {
                return instanceModel;
            }
        }
        
        public static List<OrdersModel> Fetch_Orders_For_Customers(string uuid, string status = "all", int limit = 1000, bool isSearch = false, string keyword = "")
        {
            List<OrdersModel> instanceModel = new List<OrdersModel>();

            try
            {
                using (MySqlConnection con = new MySqlConnection(DataLibrary.connectionString))
                {
                    con.Open();
                    MySqlDataReader dr;
                    MySqlCommand sqlCommand = new MySqlCommand();
                    sqlCommand.Connection = con;

                    string subQueryA = "SELECT ID, orderid, address, deliverytime, phonenumber, status, serverdatetime From Orders ";
                    string subQueryB = " orderid LIKE '%" + keyword + "%' OR address LIKE '%" + keyword + "%' OR serverDateTime LIKE '%" + keyword + "%'";
                    string subQueryC = " ORDER BY ID DESC LIMIT " + limit + " ";

                    string query = subQueryA + (isSearch ? status == "all" ? "WHERE ucid ='" + uuid + "' AND (" + subQueryB + ") " + subQueryC : "WHERE ucid = '" + uuid + "' AND status = '" + status + "' AND (" + subQueryB + ")" + subQueryC : status == "all" ? " WHERE ucid = '" + uuid + "' " + subQueryC : " WHERE ucid = '" + uuid + "' AND status ='" + status + "'" + subQueryC);

                    sqlCommand.CommandText = query;
                    dr = sqlCommand.ExecuteReader();

                    while (dr.Read())
                    {
                        instanceModel.Add(new OrdersModel()
                        {
                            id = dr["ID"].ToString(),
                            orderid = dr["orderid"].ToString(),
                            address = dr["address"].ToString(),
                            deliverytime = dr["deliverytime"].ToString(),
                            phonenumber = dr["phonenumber"].ToString(),
                            status = dr["status"].ToString(),
                            serverdatetime = dr["serverdatetime"].ToString(),
                        });
                    }

                    return instanceModel;
                }
            }
            catch (Exception)
            {
                return instanceModel;
            }
        }

        public static List<MenuModel> Fetch_Menu_For_Partners(string uuid, string status = "all", int limit = 1000, bool isSearch = false, string keyword = "")
        {
            List<MenuModel> instanceModel = new List<MenuModel>();

            try
            {
                using (MySqlConnection con = new MySqlConnection(DataLibrary.connectionString))
                {
                    con.Open();
                    MySqlDataReader dr;
                    MySqlCommand sqlCommand = new MySqlCommand();
                    sqlCommand.Connection = con;

                    string subQueryA = "SELECT ID, productid, uuid, deliverytime, name, description, image, price, status, serverdatetime From recipe ";
                    string subQueryB = " productid LIKE '%" + keyword + "%' OR name LIKE '%" + keyword + "%' OR description LIKE '%" + keyword + "%' OR price LIKE '%" + keyword + "%' OR serverDateTime LIKE '%" + keyword + "%'";
                    string subQueryC = " ORDER BY ID DESC LIMIT " + limit + " ";

                    string query = subQueryA + (isSearch ? status == "all" ? "WHERE uuid ='" + uuid + "' AND (" + subQueryB + ") " + subQueryC : "WHERE uuid = '" + uuid + "' AND status = '" + status + "' AND (" + subQueryB + ")" + subQueryC : status == "all" ? " WHERE uuid = '" + uuid + "' " + subQueryC : " WHERE uuid = '" + uuid + "' AND status ='" + status + "'" + subQueryC);

                    sqlCommand.CommandText = query;
                    dr = sqlCommand.ExecuteReader();

                    while (dr.Read())
                    {
                        instanceModel.Add(new MenuModel()
                        {
                            id = dr["ID"].ToString(),
                            uuid = dr["uuid"].ToString(),
                            deliverytime = dr["deliverytime"].ToString(),
                            name = dr["name"].ToString(),
                            description = dr["description"].ToString(),
                            image = dr["image"].ToString(),
                            price = dr["price"].ToString(),
                            status = dr["status"].ToString(),
                            serverdatetime = dr["serverdatetime"].ToString(),
                        });
                    }

                    return instanceModel;
                }
            }
            catch (Exception)
            {
                return instanceModel;
            }
        }
        
        public static List<MenuModel> Fetch_Menu_For_Customers(string status = "all", int limit = 1000, bool isSearch = false, string keyword = "")
        {
            List<MenuModel> instanceModel = new List<MenuModel>();

            try
            {
                using (MySqlConnection con = new MySqlConnection(DataLibrary.connectionString))
                {
                    con.Open();
                    MySqlDataReader dr;
                    MySqlCommand sqlCommand = new MySqlCommand();
                    sqlCommand.Connection = con;

                    string subQueryA = "SELECT ID, productid, uuid, deliverytime, name, description, image, price, status, serverdatetime From recipe ";
                    string subQueryB = " productid LIKE '%" + keyword + "%' OR name LIKE '%" + keyword + "%' OR description LIKE '%" + keyword + "%' OR price LIKE '%" + keyword + "%' OR serverDateTime LIKE '%" + keyword + "%'";
                    string subQueryC = " ORDER BY ID DESC LIMIT " + limit + " ";

                    string query = subQueryA + (isSearch ? status == "all" ? " " + subQueryB + " " + subQueryC : "WHERE status = '" + status + "' AND (" + subQueryB + ")" + subQueryC : status == "all" ? " " + subQueryC : " WHERE status ='" + status + "' " + subQueryC);

                    sqlCommand.CommandText = query;
                    dr = sqlCommand.ExecuteReader();

                    while (dr.Read())
                    {
                        instanceModel.Add(new MenuModel()
                        {
                            id = dr["ID"].ToString(),
                            uuid = dr["uuid"].ToString(),
                            deliverytime = dr["deliverytime"].ToString(),
                            name = dr["name"].ToString(),
                            description = dr["description"].ToString(),
                            image = dr["image"].ToString(),
                            price = dr["price"].ToString(),
                            status = dr["status"].ToString(),
                            serverdatetime = dr["serverdatetime"].ToString(),
                        });
                    }

                    return instanceModel;
                }
            }
            catch (Exception)
            {
                return instanceModel;
            }
        }
        
        public static List<DiscountModel> Fetch_Discounts_For_Partners(string uuid, string status = "all", int limit = 1000, bool isSearch = false, string keyword = "")
        {
            List<DiscountModel> instanceModel = new List<DiscountModel>();

            try
            {
                using (MySqlConnection con = new MySqlConnection(DataLibrary.connectionString))
                {
                    con.Open();
                    MySqlDataReader dr;
                    MySqlCommand sqlCommand = new MySqlCommand();
                    sqlCommand.Connection = con;

                    string subQueryA = "SELECT ID, bannerfilepath, headline, body, duration, calltoaction, status, uuid, discount, serverdatetime From Discount ";
                    string subQueryB = " headline LIKE '%" + keyword + "%' OR body LIKE '%" + keyword + "%' OR duration LIKE '%" + keyword + "%' OR calltoaction LIKE '%" + keyword + "%' OR serverDateTime LIKE '%" + keyword + "%'";
                    string subQueryC = " ORDER BY ID DESC LIMIT " + limit + " ";

                    string query = subQueryA + (isSearch ? status == "all" ? "WHERE uuid ='" + uuid + "' AND (" + subQueryB + ") " + subQueryC : "WHERE uuid = '" + uuid + "' AND status = '" + status + "' AND (" + subQueryB + ")" + subQueryC : status == "all" ? " WHERE uuid = '" + uuid + "' " + subQueryC : " WHERE uuid = '" + uuid + "' AND status ='" + status + "'" + subQueryC);

                    sqlCommand.CommandText = query;
                    dr = sqlCommand.ExecuteReader();

                    while (dr.Read())
                    {
                        instanceModel.Add(new DiscountModel()
                        {
                            id = dr["ID"].ToString(),
                            uuid = dr["uuid"].ToString(),
                            bannerfilepath = dr["bannerfilepath"].ToString(),
                            headline = dr["headline"].ToString(),
                            body = dr["body"].ToString(),
                            duration = dr["duration"].ToString(),
                            calltoaction = dr["calltoaction"].ToString(),
                            discount = dr["discount"].ToString(),
                            status = dr["status"].ToString(),
                            serverdatetime = dr["serverdatetime"].ToString(),
                        });
                    }

                    return instanceModel;
                }
            }
            catch (Exception)
            {
                return instanceModel;
            }
        }
        
        public static List<DiscountModel> Fetch_Discounts_For_Customers(string status = "all", int limit = 1000, bool isSearch = false, string keyword = "")
        {
            List<DiscountModel> instanceModel = new List<DiscountModel>();

            try
            {
                using (MySqlConnection con = new MySqlConnection(DataLibrary.connectionString))
                {
                    con.Open();
                    MySqlDataReader dr;
                    MySqlCommand sqlCommand = new MySqlCommand();
                    sqlCommand.Connection = con;

                    string subQueryA = "SELECT ID, bannerfilepath, headline, body, duration, calltoaction, status, uuid, discount, serverdatetime From Discount ";
                    string subQueryB = " headline LIKE '%" + keyword + "%' OR body LIKE '%" + keyword + "%' OR duration LIKE '%" + keyword + "%' OR calltoaction LIKE '%" + keyword + "%' OR serverDateTime LIKE '%" + keyword + "%'";
                    string subQueryC = " ORDER BY ID DESC LIMIT " + limit + " ";

                    string query = subQueryA + (isSearch ? status == "all" ? " " + subQueryB + " " + subQueryC : "WHERE status = '" + status + "' AND (" + subQueryB + ")" + subQueryC : status == "all" ? " " + subQueryC : " WHERE status ='" + status + "' " + subQueryC);

                    sqlCommand.CommandText = query;
                    dr = sqlCommand.ExecuteReader();

                    while (dr.Read())
                    {
                        instanceModel.Add(new DiscountModel()
                        {
                            id = dr["ID"].ToString(),
                            uuid = dr["uuid"].ToString(),
                            bannerfilepath = dr["bannerfilepath"].ToString(),
                            headline = dr["headline"].ToString(),
                            body = dr["body"].ToString(),
                            duration = dr["duration"].ToString(),
                            calltoaction = dr["calltoaction"].ToString(),
                            discount = dr["discount"].ToString(),
                            status = dr["status"].ToString(),
                            serverdatetime = dr["serverdatetime"].ToString(),
                        });
                    }

                    return instanceModel;
                }
            }
            catch (Exception)
            {
                return instanceModel;
            }
        }
        
        public static List<RestaurantsModel> Fetch_Restaurants_For_Customers(string status = "all", int limit = 1000, bool isSearch = false, string keyword = "")
        {
            List<RestaurantsModel> instanceModel = new List<RestaurantsModel>();

            try
            {
                using (MySqlConnection con = new MySqlConnection(DataLibrary.connectionString))
                {
                    con.Open();
                    MySqlDataReader dr;
                    MySqlCommand sqlCommand = new MySqlCommand();
                    sqlCommand.Connection = con;

                    string subQueryA = "SELECT ID, businessname, image, tagline, companylogo, twitter, facebook, linkedin, status, uuid, serverdatetime From Partners ";
                    string subQueryB = " businessname LIKE '%" + keyword + "%' OR tagline LIKE '%" + keyword + "%' OR twitter LIKE '%" + keyword + "%' OR facebook LIKE '%" + keyword + "%' OR linkedin LIKE '%" + keyword + "%'";
                    string subQueryC = " ORDER BY ID DESC LIMIT " + limit + " ";

                    string query = subQueryA + (isSearch ? status == "all" ? " " + subQueryB + " " + subQueryC : "WHERE status = '" + status + "' AND (" + subQueryB + ")" + subQueryC : status == "all" ? " " + subQueryC : " WHERE status ='" + status + "' " + subQueryC);

                    sqlCommand.CommandText = query;
                    dr = sqlCommand.ExecuteReader();

                    while (dr.Read())
                    {
                        instanceModel.Add(new RestaurantsModel()
                        {
                            id = dr["ID"].ToString(),
                            businessname = dr["businessname"].ToString(),
                            image = dr["image"].ToString(),
                            tagline = dr["tagline"].ToString(),
                            companylogo = dr["companylogo"].ToString(),
                            twitter = dr["twitter"].ToString(),
                            facebook = dr["facebook"].ToString(),
                            linkedin = dr["linkedin"].ToString(),
                        });
                    }

                    return instanceModel;
                }
            }
            catch (Exception)
            {
                return instanceModel;
            }
        }
        
        public static List<ChatModel> FetchChat(string chatid)
        {
            List<ChatModel> instanceModel = new List<ChatModel>();

            try
            {
                using (MySqlConnection con = new MySqlConnection(DataLibrary.connectionString))
                {
                    con.Open();
                    MySqlDataReader dr;
                    MySqlCommand sqlCommand = new MySqlCommand();
                    sqlCommand.Connection = con;
                    string query = "SELECT ID, chatid, fromid, toid, message, replytext, type, serverdatetime From Chats WHERE chatid='" + chatid + "'";
                    sqlCommand.CommandText = query;
                    dr = sqlCommand.ExecuteReader();

                    while (dr.Read())
                    {
                        instanceModel.Add(new ChatModel()
                        {
                            id = dr["ID"].ToString(),
                            chatid = chatid,
                            fromid = dr["fromid"].ToString(),
                            toid = dr["toid"].ToString(),
                            message = dr["message"].ToString(),
                            replytext = dr["replytext"].ToString(),
                            type = dr["type"].ToString(),
                            serverdatetime = dr["serverdatetime"].ToString(),
                        });
                    }

                    return instanceModel;
                }
            }
            catch (Exception)
            {
                return instanceModel;
            }
        }
        
        public static List<ContactModel> FetchContacts(string uuid)
        {
            List<ContactModel> instanceModel = new List<ContactModel>();

            try
            {
                using (MySqlConnection con = new MySqlConnection(DataLibrary.connectionString))
                {
                    con.Open();
                    MySqlDataReader dr;
                    MySqlCommand sqlCommand = new MySqlCommand();
                    sqlCommand.Connection = con;
                    string query = "SELECT ID, chatid, userid, contactid, message, status, (SELECT count(*) FROM ChatContacts cc WHERE cc.chatid=chatid) AS seen, contacttype, type, serverdatetime From ChatContacts WHERE userid='" + uuid + "'";
                    sqlCommand.CommandText = query;
                    dr = sqlCommand.ExecuteReader();

                    while (dr.Read())
                    {
                        instanceModel.Add(new ContactModel()
                        {
                            id = dr["ID"].ToString(),
                            chatid = dr["chatid"].ToString(),
                            userid = dr["userid"].ToString(),
                            contactid = dr["contactid"].ToString(),
                            message = dr["message"].ToString(),
                            status = dr["status"].ToString(),
                            seen = dr["seen"].ToString(),
                            contacttype = dr["contacttype"].ToString(),
                            type = dr["type"].ToString(),
                            serverdatetime = dr["serverdatetime"].ToString(),
                        });
                    }

                    return instanceModel;
                }
            }
            catch (Exception)
            {
                return instanceModel;
            }
        }

        public static bool DoesItExistOneWhereOne(string tableName, string columnName, string columnVal)
        {
            using (MySqlConnection con = new MySqlConnection(DataLibrary.connectionString))
            {
                con.Open();
                //CREATE CONNECTION COMMAND
                string existQuery = "SELECT count(*) FROM  " + tableName + " WHERE " + columnName + "='" + columnVal + "'";
                MySqlCommand cmd = new MySqlCommand(existQuery, con);
                String output = cmd.ExecuteScalar().ToString();

                if (int.Parse(output) > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        
        public static bool DoesItExistOneWhereTwo(string tableName, string columnName1, string columnVal1, string columnName2, string columnVal2)
        {
            using (MySqlConnection con = new MySqlConnection(DataLibrary.connectionString))
            {
                con.Open();
                //CREATE CONNECTION COMMAND
                string existQuery = "SELECT count(*) FROM  " + tableName + " WHERE " + columnName1 + "='" + columnVal1 + "' AND " + columnName2 + "='" + columnVal2 + "'";
                MySqlCommand cmd = new MySqlCommand(existQuery, con);
                String output = cmd.ExecuteScalar().ToString();

                if (int.Parse(output) > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        
        public static bool DoesItExistOneWhereThree(string tableName, string columnName1, string columnVal1, string columnName2, string columnVal2, string columnName3, string columnVal3)
        {
            using (MySqlConnection con = new MySqlConnection(DataLibrary.connectionString))
            {
                con.Open();
                //CREATE CONNECTION COMMAND
                string existQuery = "SELECT count(*) FROM  " + tableName + " WHERE " + columnName1 + "='" + columnVal1 + "' AND " + columnName2 + "='" + columnVal2 + "' AND " + columnName3 + "='" + columnVal3 + "'";
                MySqlCommand cmd = new MySqlCommand(existQuery, con);
                String output = cmd.ExecuteScalar().ToString();

                if (int.Parse(output) > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public static bool DoesItExistTwoWhereTwo(string tableName, string columnName1, string columnVal1, string columnName2, string columnVal2)
        {
            using (MySqlConnection con = new MySqlConnection(DataLibrary.connectionString))
            {
                con.Open();
                //CREATE CONNECTION COMMAND
                string existQuery = "SELECT count(*) FROM  " + tableName + " WHERE " + columnName1 + "='" + columnVal1 + "' AND " + columnName2 + "='" + columnVal2 + "'";
                MySqlCommand cmd = new MySqlCommand(existQuery, con);
                String output = cmd.ExecuteScalar().ToString();

                if (int.Parse(output) > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public static int CountTotalWhereOne(string tableName, string columnName, string columnVal)
        {
            using (MySqlConnection con = new MySqlConnection(DataLibrary.connectionString))
            {
                con.Open();
                //CREATE CONNECTION COMMAND
                string existQuery = "SELECT count(*) FROM  " + tableName + " WHERE " + columnName + "='" + columnVal + "'";
                MySqlCommand cmd = new MySqlCommand(existQuery, con);
                String output = cmd.ExecuteScalar().ToString();

                return int.Parse(output);
            }
        }

        public static int CountRowTotal(string tableName)
        {
            using (MySqlConnection con = new MySqlConnection(DataLibrary.connectionString))
            {
                con.Open();
                //CREATE CONNECTION COMMAND
                string existQuery = "SELECT count(*) FROM  " + tableName + "";
                MySqlCommand cmd = new MySqlCommand(existQuery, con);
                String output = cmd.ExecuteScalar().ToString();

                return int.Parse(output);
            }
        }

        public static int CountTotalWhereTwo(string tableName, string columnName1, string columnVal1, string columnName2, string columnVal2)
        {
            using (MySqlConnection con = new MySqlConnection(DataLibrary.connectionString))
            {
                con.Open();
                //CREATE CONNECTION COMMAND
                string existQuery = "SELECT count(*) FROM  " + tableName + " WHERE " + columnName1 + "='" + columnVal1 + "' AND " + columnName2 + "='" + columnVal2 + "'";
                MySqlCommand cmd = new MySqlCommand(existQuery, con);
                String output = cmd.ExecuteScalar().ToString();

                return int.Parse(output);
            }
        }

        public static string SelectOneWhereOne(string tableName, string columnName, string where1, string val1)
        {
            string[] columnData = new string[1];
            try
            {
                using (MySqlConnection con = new MySqlConnection(DataLibrary.connectionString))
                {
                    con.Open();
                    MySqlCommand cmd = new MySqlCommand("Select " + columnName + " FROM " + tableName + " WHERE " + where1 + "=@val ORDER BY ID DESC LIMIT 1", con);
                    cmd.Parameters.AddWithValue("@val", val1);
                    MySqlDataReader sdr = cmd.ExecuteReader();
                    while (sdr.Read())
                    {
                        columnData[0] = sdr.GetValue(0).ToString();
                    }

                    return columnData[0];
                }
            }
            catch (Exception)
            {
                return columnData[0];
            }

        }

        public static string SelectOneWhereOneRandom(string tableName, string columnName, string where1, string val1)
        {
            string[] columnData = new string[1];
            try
            {
                using (MySqlConnection con = new MySqlConnection(DataLibrary.connectionString))
                {
                    con.Open();
                    MySqlCommand cmd = new MySqlCommand("Select " + columnName + " FROM " + tableName + " WHERE " + where1 + "=@val ORDER BY RAND() LIMIT 1", con);
                    cmd.Parameters.AddWithValue("@val", val1);
                    MySqlDataReader sdr = cmd.ExecuteReader();
                    while (sdr.Read())
                    {
                        columnData[0] = sdr.GetValue(0).ToString();
                    }

                    return columnData[0];
                }
            }
            catch (Exception)
            {
                return columnData[0];
            }

        }

        public static string CountTakenProductLotterySlots(string lotteryid)
        {
            string data = "";
            try
            {
                using (MySqlConnection con = new MySqlConnection(DataLibrary.connectionString))
                {
                    con.Open();
                    MySqlCommand cmd = new MySqlCommand("Select count(*) FROM productlotterystakes WHERE lotteryid=@lotteryid", con);
                    cmd.Parameters.AddWithValue("@lotteryid", lotteryid);
                    MySqlDataReader sdr = cmd.ExecuteReader();
                    while (sdr.Read())
                    {
                        data = sdr.GetValue(0).ToString();
                    }

                    return data;
                }
            }
            catch (Exception)
            {
                return data;
            }

        }

        public static string CountTakenCashLotterySlots(string lotteryid)
        {
            string data = "";
            try
            {
                using (MySqlConnection con = new MySqlConnection(DataLibrary.connectionString))
                {
                    con.Open();
                    MySqlCommand cmd = new MySqlCommand("Select count(*) FROM cashlotterystakes WHERE lotteryid=@lotteryid", con);
                    cmd.Parameters.AddWithValue("@lotteryid", lotteryid);
                    MySqlDataReader sdr = cmd.ExecuteReader();
                    while (sdr.Read())
                    {
                        data = sdr.GetValue(0).ToString();
                    }

                    return data;
                }
            }
            catch (Exception)
            {
                return data;
            }

        }

        public static string SelectOneWhereTwo(string tableName, string columnName, string where1, string val1, string where2, string val2)
        {
            string[] columnData = new string[1];
            try
            {
                using (MySqlConnection con = new MySqlConnection(DataLibrary.connectionString))
                {
                    con.Open();
                    MySqlCommand cmd = new MySqlCommand("Select " + columnName + " FROM " + tableName + " WHERE " + where1 + "=@val1 AND " + where2 + "=@val2 ORDER BY ID DESC LIMIT 1", con);
                    cmd.Parameters.AddWithValue("@val1", val1);
                    cmd.Parameters.AddWithValue("@val2", val2);
                    MySqlDataReader sdr = cmd.ExecuteReader();
                    while (sdr.Read())
                    {
                        columnData[0] = sdr.GetValue(0).ToString();
                    }

                    return columnData[0];
                }
            }
            catch (Exception)
            {
                return columnData[0];
            }

        }

        public static string SelectOneWhereThree(string tableName, string columnName, string where1, string val1, string where2, string val2, string where3, string val3)
        {
            string[] columnData = new string[1];
            try
            {
                using (MySqlConnection con = new MySqlConnection(DataLibrary.connectionString))
                {
                    con.Open();
                    MySqlCommand cmd = new MySqlCommand("Select " + columnName + " FROM " + tableName + " WHERE " + where1 + "=@val1 AND " + where2 + "=@val2 AND " + where3 + "=@val3 ORDER BY ID DESC LIMIT 1", con);
                    cmd.Parameters.AddWithValue("@val1", val1);
                    cmd.Parameters.AddWithValue("@val2", val2);
                    cmd.Parameters.AddWithValue("@val3", val3);
                    MySqlDataReader sdr = cmd.ExecuteReader();
                    while (sdr.Read())
                    {
                        columnData[0] = sdr.GetValue(0).ToString();
                    }

                    return columnData[0];
                }
            }
            catch (Exception)
            {
                return columnData[0];
            }

        }
    }
}
