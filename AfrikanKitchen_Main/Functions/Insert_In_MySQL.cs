using Microsoft.VisualBasic;
using MySql.Data.MySqlClient;
using Org.BouncyCastle.Asn1.X509;
using System.Data;

namespace AfrikanKitchen_Main.Functions
{
    public class Insert_In_MySQL
    {
        public static string SubscribeToNewsletter(string email)
        {
            try
            {
                //CONNECT TO DATABASE
                using (MySqlConnection conn = new MySqlConnection(DataLibrary.connectionString))
                {
                    if (!Read_From_MySQL.DoesItExistOneWhereOne(tableName: "MailingList", columnName: "Email", columnVal: email))
                    {
                        conn.Open();

                        MySqlCommand cmd = conn.CreateCommand();
                        cmd.CommandType = CommandType.Text;
                        //Insert Using Concatenation.
                        cmd.CommandText = "INSERT into MailingList(email) VALUES(@email)";
                        cmd.Parameters.AddWithValue("@email", email);
                        cmd.ExecuteNonQuery();
                        conn.Close();
                    }

                    return "success";
                }
            }
            catch (Exception e)
            {
                return "error: " + e.Message;
            }
        }
        
        public static string AddBankAccount(string uuid, string accountnumber, string bankname)
        {
            try
            {
                //CONNECT TO DATABASE
                using (MySqlConnection conn = new MySqlConnection(DataLibrary.connectionString))
                {
                    conn.Open();

                    MySqlCommand cmd = conn.CreateCommand();
                    cmd.CommandType = CommandType.Text;
                    //Insert Using Concatenation.
                    cmd.CommandText = "INSERT into BankAccount(uuid, accountnumber, bankname) VALUES(@uuid, @accountnumber, @bankname)";
                    cmd.Parameters.AddWithValue("@uuid", uuid);
                    cmd.Parameters.AddWithValue("@accountnumber", accountnumber);
                    cmd.Parameters.AddWithValue("@bankname", bankname);
                    cmd.ExecuteNonQuery();
                    conn.Close();

                    return "success";
                }
            }
            catch (Exception e)
            {
                return "error: " + e.Message;
            }
        }
        
        public static string AddPartner(string businessname, string email, string password, string accounttype)
        {
            try
            {
                //CONNECT TO DATABASE
                using (MySqlConnection conn = new MySqlConnection(DataLibrary.connectionString))
                {
                    conn.Open();

                    MySqlCommand cmd = conn.CreateCommand();
                    cmd.CommandType = CommandType.Text;
                    //Insert Using Concatenation.
                    cmd.CommandText = "INSERT into Partners(uuid, email, password, accounttype, temppin) VALUES(uuid(), @email, @password, @accounttype, @temppin)";
                    cmd.Parameters.AddWithValue("@businessname", businessname);
                    cmd.Parameters.AddWithValue("@email", email);
                    cmd.Parameters.AddWithValue("@password", password);
                    cmd.Parameters.AddWithValue("@accounttype", accounttype);
                    cmd.Parameters.AddWithValue("@temppin", ValueFormatter.GenerateRamdomCapsLetterAndDigitsOnly(16));
                    cmd.Parameters.AddWithValue("@emailverified", accounttype == "regular" ? "false" : "true");
                    cmd.ExecuteNonQuery();
                    conn.Close();

                    return "success";
                }
            }
            catch (Exception e)
            {
                return "error: " + e.Message;
            }
        }
        
        public static string AddCustomer(string name, string email, string password, string accounttype)
        {
            try
            {
                //CONNECT TO DATABASE
                using (MySqlConnection conn = new MySqlConnection(DataLibrary.connectionString))
                {
                    conn.Open();

                    MySqlCommand cmd = conn.CreateCommand();
                    cmd.CommandType = CommandType.Text;
                    //Insert Using Concatenation.
                    cmd.CommandText = "INSERT into Customers(uuid, email, password, accounttype, temppin, emailverified) VALUES(uuid(), @email, @password, @accounttype, @temppin, @emailverified)";
                    cmd.Parameters.AddWithValue("@name", name);
                    cmd.Parameters.AddWithValue("@email", email);
                    cmd.Parameters.AddWithValue("@password", password);
                    cmd.Parameters.AddWithValue("@accounttype", accounttype);
                    cmd.Parameters.AddWithValue("@temppin", ValueFormatter.GenerateRamdomCapsLetterAndDigitsOnly(16));
                    cmd.Parameters.AddWithValue("@emailverified", accounttype == "regular" ? "false" : "true");
                    cmd.ExecuteNonQuery();
                    conn.Close();

                    return "success";
                }
            }
            catch (Exception e)
            {
                return "error: " + e.Message;
            }
        }
        
        public static string AddDocument(string uuid, string filepath, string usertype)
        {
            try
            {
                //CONNECT TO DATABASE
                using (MySqlConnection conn = new MySqlConnection(DataLibrary.connectionString))
                {
                    conn.Open();

                    MySqlCommand cmd = conn.CreateCommand();
                    cmd.CommandType = CommandType.Text;
                    //Insert Using Concatenation.
                    cmd.CommandText = "INSERT into Documents(uuid, filepath, usertype) VALUES(@uuid, @filepath, @usertype)";
                    cmd.Parameters.AddWithValue("@uuid", uuid);
                    cmd.Parameters.AddWithValue("@filepath", filepath);
                    cmd.Parameters.AddWithValue("@usertype", usertype);
                    cmd.ExecuteNonQuery();
                    conn.Close();

                    return "success";
                }
            }
            catch (Exception e)
            {
                return "error: " + e.Message;
            }
        }
        
        public static string AddToCart(string uuid, string productid, int quantity)
        {
            try
            {
                //CONNECT TO DATABASE
                using (MySqlConnection conn = new MySqlConnection(DataLibrary.connectionString))
                {
                    conn.Open();

                    MySqlCommand cmd = conn.CreateCommand();
                    cmd.CommandType = CommandType.Text;
                    //Insert Using Concatenation.
                    cmd.CommandText = "INSERT into Cart(uuid, productid, quantity) VALUES(@uuid, @productid, @quantity)";
                    cmd.Parameters.AddWithValue("@uuid", uuid);
                    cmd.Parameters.AddWithValue("@productid", productid);
                    cmd.Parameters.AddWithValue("@quantity", quantity);
                    cmd.ExecuteNonQuery();
                    conn.Close();

                    return "success";
                }
            }
            catch (Exception e)
            {
                return "error: " + e.Message;
            }
        }
        
        public static string AddToFavourites(string uuid, string productid)
        {
            try
            {
                //CONNECT TO DATABASE
                using (MySqlConnection conn = new MySqlConnection(DataLibrary.connectionString))
                {
                    conn.Open();

                    MySqlCommand cmd = conn.CreateCommand();
                    cmd.CommandType = CommandType.Text;
                    //Insert Using Concatenation.
                    cmd.CommandText = "INSERT into Favourites(uuid, productid) VALUES(@uuid, @productid)";
                    cmd.Parameters.AddWithValue("@uuid", uuid);
                    cmd.Parameters.AddWithValue("@productid", productid);
                    cmd.ExecuteNonQuery();
                    conn.Close();

                    return "success";
                }
            }
            catch (Exception e)
            {
                return "error: " + e.Message;
            }
        }

        public static string AddDiscount(string uuid, string bannerfilepath, string headline, string body, string duration, string calltoaction, string discount)
        {
            try
            {
                //CONNECT TO DATABASE
                using (MySqlConnection conn = new MySqlConnection(DataLibrary.connectionString))
                {
                    conn.Open();

                    MySqlCommand cmd = conn.CreateCommand();
                    cmd.CommandType = CommandType.Text;
                    //Insert Using Concatenation.
                    cmd.CommandText = "INSERT into Discount(uuid, bannerfilepath, headline, body, duration, calltoaction, discount) VALUES(@uuid, @bannerfilepath, @headline, @body, @duration, @calltoaction, @discount)";
                    cmd.Parameters.AddWithValue("@uuid", uuid);
                    cmd.Parameters.AddWithValue("@bannerfilepath", bannerfilepath);
                    cmd.Parameters.AddWithValue("@headline", headline);
                    cmd.Parameters.AddWithValue("@body", body);
                    cmd.Parameters.AddWithValue("@duration", duration);
                    cmd.Parameters.AddWithValue("@calltoaction", calltoaction);
                    cmd.Parameters.AddWithValue("@discount", discount);
                    cmd.ExecuteNonQuery();
                    conn.Close();

                    return "success";
                }
            }
            catch (Exception e)
            {
                return "error: " + e.Message;
            }
        }
        
        public static string RequestPayout(string uuid, string amount, string bankname, string accountnumber)
        {
            try
            {
                //CONNECT TO DATABASE
                using (MySqlConnection conn = new MySqlConnection(DataLibrary.connectionString))
                {
                    conn.Open();

                    MySqlCommand cmd = conn.CreateCommand();
                    cmd.CommandType = CommandType.Text;
                    //Insert Using Concatenation.
                    cmd.CommandText = "INSERT into WithdrawalRequest(uuid, amount, bankname) VALUES(@uuid, @amount, @bankname, @accountnumber)";
                    cmd.Parameters.AddWithValue("@uuid", uuid);
                    cmd.Parameters.AddWithValue("@amount", amount);
                    cmd.Parameters.AddWithValue("@bankname", bankname);
                    cmd.Parameters.AddWithValue("@accountnumber", accountnumber);
                    cmd.ExecuteNonQuery();
                    conn.Close();

                    return "success";
                }
            }
            catch (Exception e)
            {
                return "error: " + e.Message;
            }
        }

        public static string PlaceOrder(string upid, string ucid, string orderid, string address, string phonenumber)
        {
            try
            {
                //CONNECT TO DATABASE
                using (MySqlConnection conn = new MySqlConnection(DataLibrary.connectionString))
                {
                    conn.Open();

                    MySqlCommand cmd = conn.CreateCommand();
                    cmd.CommandType = CommandType.Text;
                    //Insert Using Concatenation.
                    cmd.CommandText = "INSERT into Orders(upid, ucid, orderid, address, phonenumber) VALUES(@upid, @ucid, @orderid, @address, @phonenumber)";
                    cmd.Parameters.AddWithValue("@upid", upid);
                    cmd.Parameters.AddWithValue("@ucid", ucid);
                    cmd.Parameters.AddWithValue("@orderid", orderid);
                    cmd.Parameters.AddWithValue("@address", address);
                    cmd.Parameters.AddWithValue("@phonenumber", phonenumber);
                    cmd.ExecuteNonQuery();
                    conn.Close();

                    return "success";
                }
            }
            catch (Exception e)
            {
                return "error: " + e.Message;
            }
        }
        
        public static string AddReview(string upid, string ucid, string review, string stars)
        {
            try
            {
                //CONNECT TO DATABASE
                using (MySqlConnection conn = new MySqlConnection(DataLibrary.connectionString))
                {
                    conn.Open();

                    MySqlCommand cmd = conn.CreateCommand();
                    cmd.CommandType = CommandType.Text;
                    //Insert Using Concatenation.
                    cmd.CommandText = "INSERT into Reviews(upid, ucid, review, stars) VALUES(@upid, @ucid, @review, @stars)";
                    cmd.Parameters.AddWithValue("@upid", upid);
                    cmd.Parameters.AddWithValue("@ucid", ucid);
                    cmd.Parameters.AddWithValue("@review", review);
                    cmd.Parameters.AddWithValue("@stars", stars);
                    cmd.ExecuteNonQuery();
                    conn.Close();

                    return "success";
                }
            }
            catch (Exception e)
            {
                return "error: " + e.Message;
            }
        }

        public static string AddCard(string uuid, string cardholder, string cardnumber, string cvc, string expirydate)
        {
            try
            {
                //CONNECT TO DATABASE
                using (MySqlConnection conn = new MySqlConnection(DataLibrary.connectionString))
                {
                    conn.Open();

                    MySqlCommand cmd = conn.CreateCommand();
                    cmd.CommandType = CommandType.Text;
                    //Insert Using Concatenation.
                    cmd.CommandText = "INSERT into Cards(uuid, cardholder, cardnumber, cvc, expirydate) VALUES(@uuid, @cardholder, @cardnumber, @cvc, @expirydate)";
                    cmd.Parameters.AddWithValue("@uuid", uuid);
                    cmd.Parameters.AddWithValue("@cardholder", cardholder);
                    cmd.Parameters.AddWithValue("@cardnumber", cardnumber);
                    cmd.Parameters.AddWithValue("@cvc", cvc);
                    cmd.Parameters.AddWithValue("@expirydate", expirydate);
                    cmd.ExecuteNonQuery();
                    conn.Close();

                    return "success";
                }
            }
            catch (Exception e)
            {
                return "error: " + e.Message;
            }
        }
        
        public static string AddContact(string userid, string contactid, string chatid, string message, string contacttype, string type)
        {
            try
            {
                //CONNECT TO DATABASE
                using (MySqlConnection conn = new MySqlConnection(DataLibrary.connectionString))
                {
                    conn.Open();

                    MySqlCommand cmd = conn.CreateCommand();
                    cmd.CommandType = CommandType.Text;
                    //Insert Using Concatenation.
                    cmd.CommandText = "INSERT into ChatContacts(userid, contactid, chatid, message, contacttype, type) VALUES(@userid, @contactid, @chatid, @message, @contacttype, @type)";
                    cmd.Parameters.AddWithValue("@userid", userid);
                    cmd.Parameters.AddWithValue("@contactid", contactid);
                    cmd.Parameters.AddWithValue("@chatid", chatid);
                    cmd.Parameters.AddWithValue("@message", message);
                    cmd.Parameters.AddWithValue("@contacttype", contacttype);
                    cmd.Parameters.AddWithValue("@type", type);
                    cmd.ExecuteNonQuery();
                    conn.Close();

                    return "success";
                }
            }
            catch (Exception e)
            {
                return "error: " + e.Message;
            }
        }
        
        public static string SendMessage(string chatid, string fromid, string toid, string message, string replytext, string type, string usertype)
        {
            try
            {
                if (chatid == "new")
                {
                    //Add contact for Partner
                    AddContact(userid: fromid, contactid: toid, chatid: chatid, message: message, contacttype: usertype.ToLower() == "partners" ? "customer" : "partner", type: type);

                    //Add contact for Customer
                    AddContact(userid: toid, contactid: fromid, chatid: chatid, message: message, contacttype: usertype.ToLower() == "partners" ? "customer" : "partner", type: type);
                }
                else
                {
                    //Update contacts
                    Update_To_MySql.UpdateOnColumnWhereOne(tableName: "chatcontacts", updateColumn: "message", updateVal: message, columnWhere: "chatid", columnVal: chatid);
                }

                //CONNECT TO DATABASE
                using (MySqlConnection conn = new MySqlConnection(DataLibrary.connectionString))
                {
                    conn.Open();

                    MySqlCommand cmd = conn.CreateCommand();
                    cmd.CommandType = CommandType.Text;
                    //Insert Using Concatenation.
                    cmd.CommandText = "INSERT into Chats(chatid, fromid, toid, message, replytext, type) VALUES(@chatid, @fromid, @toid, @message, @replytext, @type)";
                    cmd.Parameters.AddWithValue("@chatid", chatid);
                    cmd.Parameters.AddWithValue("@fromid", fromid);
                    cmd.Parameters.AddWithValue("@toid", toid);
                    cmd.Parameters.AddWithValue("@message", message);
                    cmd.Parameters.AddWithValue("@replytext", replytext);
                    cmd.Parameters.AddWithValue("@type", type);
                    cmd.ExecuteNonQuery();
                    conn.Close();

                    return "success";
                }
            }
            catch (Exception e)
            {
                return "error: " + e.Message;
            }
        }
    }
}
