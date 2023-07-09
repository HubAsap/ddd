using AfrikanKitchen_Main.Functions;
using AfrikanKitchen_Main.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using static System.Net.Mime.MediaTypeNames;
using System.Xml.Linq;
using System;
using static Microsoft.Extensions.Logging.EventSource.LoggingEventSource;
using System.Collections.Generic;
using Google.Protobuf.WellKnownTypes;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Security.Cryptography;

namespace AfrikanKitchen_Main.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class MainController : Controller
    {
        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public IActionResult ChangePassword(string uuid, string usertype, string accesstoken, string password)
        {
            //1. Check if all the parameters above are not invalid or contain a null value.
            if (ModelState.IsValid)
            {
                //2. The usertype will be used to identify what table to fetch the users temporal pin.
                string tempPin = Read_From_MySQL.SelectOneWhereTwo(tableName: usertype, columnName: "temppin", where1: "uuid", val1: uuid, where2: "accounttype", val2: "regular");

                //3: Compare the access token with users temporal pin, if they are the same Continue else return an status 400(error meesage). Temporal pin and access token will be converted to lower, before comparing them.
                if (tempPin != null && tempPin.ToLower() == accesstoken.ToLower())
                {
                    //4. Password is encrypted before updating.
                    password = ValueFormatter.Crypt(text: password);

                    //5. Password is updated for user(Either customer, admin, or partner).
                    Update_To_MySql.UpdateOnColumnWhereTwo(tableName: usertype, updateColumn: "password", updateVal: password, columnWhere1: "uuid", columnVal1: uuid, columnWhere2: "accounttype", columnVal2: "regular");

                    //6. Temporal pin is updated to prevent future account breach from un-authorized users.
                    Update_To_MySql.UpdateOnColumnWhereOne(tableName: usertype, updateColumn: "temppin", updateVal: ValueFormatter.GenerateRamdomCapsLetterAndDigitsOnly(16), columnWhere: "uuid", columnVal: uuid);

                    //7. Success message is returned
                    return Ok("success");
                }
                else
                {
                    return BadRequest("Session Expired, Please try again.");
                }
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public IActionResult EditProfile(string uuid, string usertype, string accesstoken, string name, string image, string tagline = "", string showlogooninvoice = "true", string showlogoonemail = "true")
        {
            //1. Check if all the parameters above are not invalid or contain a null value.
            if (ModelState.IsValid)
            {
                //2. The usertype will be used to identify what table to fetch the users temporal pin.
                string tempPin = Read_From_MySQL.SelectOneWhereOne(tableName: usertype, columnName: "temppin", where1: "uuid", val1: uuid);

                //3: Compare the access token with users temporal pin, if they are the same Continue else return an status 400(error meesage). Temporal pin and access token will be converted to lower, before comparing them.
                if (tempPin != null && tempPin.ToLower() == accesstoken.ToLower())
                {
                    switch (usertype.ToLower())
                    {
                        //Username is always saved in lower case
                        case "customers":
                            Update_To_MySql.UpdateProfileForCustomers(uuid: uuid, name: name.ToLower(), image: image);
                            break;

                        //Business name or store nam is always saved in lower case
                        case "partners":
                            Update_To_MySql.UpdateProfileForPartners(uuid: uuid, businessname: name.ToLower(), tagline: tagline, companylogo: image, showlogooninvoice: showlogooninvoice, showlogoonemail: showlogoonemail);
                            break;
                    }

                    //4. Temporal pin is updated to prevent future account breach from un-authorized users.
                    Update_To_MySql.UpdateOnColumnWhereOne(tableName: usertype, updateColumn: "temppin", updateVal: ValueFormatter.GenerateRamdomCapsLetterAndDigitsOnly(16), columnWhere: "uuid", columnVal: uuid);

                    //5. Success message is returned
                    return Ok("success");
                }
                else
                {
                    return BadRequest("Session Expired, Please try again.");
                }
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public IActionResult AddToNewsletter(string email)
        {
            //1. Check if all the parameters above are not invalid or contain a null value.
            if (ModelState.IsValid)
            {
                //2. Validate email format. If the format is wrong jump to step 4( Return success message), else continue.
                if (ValueFormatter.IsValidEmail(email))
                {
                    //3. Add user to mailing list.
                    Insert_In_MySQL.SubscribeToNewsletter(email: email);
                }

                //4. Success message is returned
                return Ok("success");
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public IActionResult UpdateNotificationSettings(string uuid, string usertype, string accesstoken, string orders_push = "true", string orders_email = "false", string orders_sms = "false", string tags_push = "true", string tags_email = "false", string tags_sms = "false", string reminder_push = "true", string reminder_email = "false", string reminder_sms = "false", string others_push = "true", string others_email = "false", string others_sms = "false")
        {
            //1. Check if all the parameters above are not invalid or contain a null value.
            if (ModelState.IsValid)
            {
                //2. The usertype will be used to identify what table to fetch the users temporal pin.
                string tempPin = Read_From_MySQL.SelectOneWhereOne(tableName: usertype, columnName: "temppin", where1: "uuid", val1: uuid);

                //3: Compare the access token with users temporal pin, if they are the same Continue else return an status 400(error meesage). Temporal pin and access token will be converted to lower, before comparing them.
                if (tempPin != null && tempPin.ToLower() == accesstoken.ToLower())
                {
                    //4. Update order notification settings
                    Update_To_MySql.UpdateThreeColumnWhereThree(tableName: "NotificationSettings", updateColumn1: "push", updateVal1: orders_push, updateColumn2: "email", updateVal2: orders_email, updateColumn3: "sms", updateVal3: orders_sms, columnWhere1: "uuid", columnVal1: uuid, columnWhere2: "notificationname", columnVal2: "orders", columnWhere3: "usertype", columnVal3: usertype);

                    //5. Update tags notification settings
                    Update_To_MySql.UpdateThreeColumnWhereThree(tableName: "NotificationSettings", updateColumn1: "push", updateVal1: tags_push, updateColumn2: "email", updateVal2: tags_email, updateColumn3: "sms", updateVal3: tags_sms, columnWhere1: "uuid", columnVal1: uuid, columnWhere2: "notificationname", columnVal2: "tags", columnWhere3: "usertype", columnVal3: usertype);

                    //6. Update reminder notification settings
                    Update_To_MySql.UpdateThreeColumnWhereThree(tableName: "NotificationSettings", updateColumn1: "push", updateVal1: reminder_push, updateColumn2: "email", updateVal2: reminder_email, updateColumn3: "sms", updateVal3: reminder_sms, columnWhere1: "uuid", columnVal1: uuid, columnWhere2: "notificationname", columnVal2: "reminder", columnWhere3: "usertype", columnVal3: usertype);

                    //7. Update others notification settings
                    Update_To_MySql.UpdateThreeColumnWhereThree(tableName: "NotificationSettings", updateColumn1: "push", updateVal1: others_push, updateColumn2: "email", updateVal2: others_email, updateColumn3: "sms", updateVal3: others_sms, columnWhere1: "uuid", columnVal1: uuid, columnWhere2: "notificationname", columnVal2: "others", columnWhere3: "usertype", columnVal3: usertype);

                    //8. Temporal pin is updated to prevent future account breach from un-authorized users.
                    Update_To_MySql.UpdateOnColumnWhereOne(tableName: usertype, updateColumn: "temppin", updateVal: ValueFormatter.GenerateRamdomCapsLetterAndDigitsOnly(16), columnWhere: "uuid", columnVal: uuid);

                    //9. Success message is returned
                    return Ok("success");
                }
                else
                {
                    return BadRequest("Session Expired, Please try again.");
                }
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public IActionResult AddBankAccount(string uuid, string accesstoken, string bankname, string accountnumber)
        {
            //1. Check if all the parameters above are not invalid or contain a null value.
            if (ModelState.IsValid)
            {
                //2. Fetch the users temporal pin.
                string tempPin = Read_From_MySQL.SelectOneWhereOne(tableName: "Partners", columnName: "temppin", where1: "uuid", val1: uuid);

                //3: Compare the access token with users temporal pin, if they are the same Continue else return status 400(error meesage). Temporal pin and access token will be converted to lower, before comparing them.
                if (tempPin != null && tempPin.ToLower() == accesstoken.ToLower())
                {
                    //4. Check if bank account has been added before, continue if not. Else return status 400(error meesage).
                    bool doesBankAccountAlreadyExist = Read_From_MySQL.DoesItExistOneWhereThree(tableName: "BankAccount", columnName1: "uuid", columnVal1: uuid, columnName2: "bankname", columnVal2: bankname, columnName3: "accountnumber", columnVal3: accountnumber);

                    if (!doesBankAccountAlreadyExist)
                    {
                        //5. Add bank account
                        Insert_In_MySQL.AddBankAccount(uuid: uuid, bankname: bankname, accountnumber: accountnumber);

                        //6. Temporal pin is updated to prevent future account breach from un-authorized users.
                        Update_To_MySql.UpdateOnColumnWhereOne(tableName: "Partners", updateColumn: "temppin", updateVal: ValueFormatter.GenerateRamdomCapsLetterAndDigitsOnly(16), columnWhere: "uuid", columnVal: uuid);

                        //7. Success message is returned
                        return Ok("success");
                    }
                    else
                    {
                        return BadRequest("Bank account already added");
                    }
                }
                else
                {
                    return BadRequest("Session Expired, Please try again.");
                }
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public IActionResult SignIn(string usertype, string accounttype, string email, string password)
        {
            //1. Check if all the parameters above are not invalid or contain a null value.
            if (ModelState.IsValid)
            {
                bool doesItExist = false;
                string actualaccounttype = Read_From_MySQL.SelectOneWhereOne(tableName: usertype, columnName: "accounttype", where1: "email", val1: email);

                //2. Validate accounttype
                if (actualaccounttype == accounttype)
                {
                    if (accounttype == "regular")
                    {
                        //3. Password is encrypted before validation.
                        password = ValueFormatter.Crypt(text: password);

                        doesItExist = Read_From_MySQL.DoesItExistOneWhereThree(tableName: usertype, columnName1: "email", columnVal1: email, columnName2: "password", columnVal2: password, columnName3: "accounttype", columnVal3: accounttype);

                        //3.1. If credentials don't match, return an error message that email or password is incorrect.
                        if (!doesItExist)
                        {
                            return BadRequest("Email or password incorrect.");
                        }
                    }
                    else
                    {
                        doesItExist = Read_From_MySQL.DoesItExistOneWhereTwo(tableName: usertype, columnName1: "email", columnVal1: email, columnName2: "accounttype", columnVal2: accounttype);

                        //4. If user does not exists with a google account, facebook, or apple account then return a command to register the user.
                        if (!doesItExist)
                        {
                            return BadRequest("cmd: Register-User");
                        }
                    }
                }
                else
                {
                    return BadRequest("This user registered using the " + actualaccounttype + " account. Kindly sign in using the " + actualaccounttype + " account.");
                }

                //5. Success message is returned
                return Ok("success");
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public IActionResult AddUser(string usertype, string accounttype, string name, string email, string password)
        {
            //1. Check if all the parameters above are not invalid or contain a null value.
            if (ModelState.IsValid)
            {
                //2. Add User
                switch (usertype.ToLower())
                {
                    case "customers":

                        //2.1. Password is encrypted if account type is regular.
                        if (accounttype == "regular")
                        {
                            password = ValueFormatter.Crypt(text: password);
                        }

                        Insert_In_MySQL.AddPartner(businessname: name, email: email, password: password, accounttype: accounttype);
                        break;

                    case "partners":

                        //2.2. Password is encrypted if account type is regular.
                        if (accounttype == "regular")
                        {
                            password = ValueFormatter.Crypt(text: password);
                        }

                        Insert_In_MySQL.AddCustomer(name: name, email: email, password: password, accounttype: accounttype);
                        break;
                }

                //5. Success message is returned
                return Ok("success");
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public IActionResult AddDocument(string uuid, string accesstoken, string filepath, string usertype)
        {
            //1. Check if all the parameters above are not invalid or contain a null value.
            if (ModelState.IsValid)
            {
                //2. Check if user exist with the provided uuid.
                bool doesUserExist = Read_From_MySQL.DoesItExistOneWhereOne(tableName: usertype, columnName: "uuid", columnVal: uuid);

                if (doesUserExist)
                {
                    //3. The usertype will be used to identify what table to fetch the users temporal pin.
                    string tempPin = Read_From_MySQL.SelectOneWhereOne(tableName: usertype, columnName: "temppin", where1: "uuid", val1: uuid);

                    //4: Compare the access token with users temporal pin, if they are the same Continue else return an status 400(error meesage). Temporal pin and access token will be converted to lower, before comparing them.
                    if (tempPin != null && tempPin.ToLower() == accesstoken.ToLower())
                    {
                        //5. Documents saved
                        Insert_In_MySQL.AddDocument(uuid: uuid, filepath: filepath, usertype: usertype);

                        //9. Success message is returned
                        return Ok("success");
                    }
                    else
                    {
                        return BadRequest("Session Expired, Please try again.");
                    }
                }
                else
                {
                    return BadRequest("Session Expired, Please try again.");
                }
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<OrdersModel>))]
        [ProducesResponseType(400)]
        public IActionResult FetchChat(string chatid)
        {
            var chats = Read_From_MySQL.FetchChat(chatid: chatid);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            else
                return Ok(chats);
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<OrdersModel>))]
        [ProducesResponseType(400)]
        public IActionResult FetchContacts(string uuid, string status = "all", int limit = 1000, bool isSearch = false, string keyword = "")
        {
            var contacts = Read_From_MySQL.FetchContacts(uuid: uuid);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            else
                return Ok(contacts);
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<OrdersModel>))]
        [ProducesResponseType(400)]
        public IActionResult FetchOrderForPartners(string uuid, string status = "all", int limit = 1000, bool isSearch = false, string keyword = "")
        {
            var orders = Read_From_MySQL.Fetch_Orders_For_Partners(uuid: uuid, status: status, limit: limit, isSearch: isSearch, keyword: keyword);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            else
                return Ok(orders);
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<OrdersModel>))]
        [ProducesResponseType(400)]
        public IActionResult FetchOrderForCustomers(string uuid, string status = "all", int limit = 1000, bool isSearch = false, string keyword = "")
        {
            var orders = Read_From_MySQL.Fetch_Orders_For_Customers(uuid: uuid, status: status, limit: limit, isSearch: isSearch, keyword: keyword);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            else
                return Ok(orders);
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<OrdersModel>))]
        [ProducesResponseType(400)]
        public IActionResult FetchOrderForAdmin(string status = "all", int limit = 1000, bool isSearch = false, string keyword = "")
        {
            var orders = Read_From_MySQL.Fetch_Orders_For_Admin(status: status, limit: limit, isSearch: isSearch, keyword: keyword);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            else
                return Ok(orders);
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<MenuModel>))]
        [ProducesResponseType(400)]
        public IActionResult FetchMenuForPartners(string uuid, string status = "all", int limit = 1000, bool isSearch = false, string keyword = "")
        {
            var menu = Read_From_MySQL.Fetch_Menu_For_Partners(uuid: uuid, status: status, limit: limit, isSearch: isSearch, keyword: keyword);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            else
                return Ok(menu);
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<MenuModel>))]
        [ProducesResponseType(400)]
        public IActionResult FetchMenuForCustomers(string status = "all", int limit = 1000, bool isSearch = false, string keyword = "")
        {
            var menu = Read_From_MySQL.Fetch_Menu_For_Customers(status: status, limit: limit, isSearch: isSearch, keyword: keyword);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            else
                return Ok(menu);
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<DiscountModel>))]
        [ProducesResponseType(400)]
        public IActionResult FetchDiscountsForPartners(string uuid, string status = "all", int limit = 1000, bool isSearch = false, string keyword = "")
        {
            var discount = Read_From_MySQL.Fetch_Discounts_For_Partners(uuid: uuid, status: status, limit: limit, isSearch: isSearch, keyword: keyword);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            else
                return Ok(discount);
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<DiscountModel>))]
        [ProducesResponseType(400)]
        public IActionResult FetchDiscountsForCustomers(string status = "all", int limit = 1000, bool isSearch = false, string keyword = "")
        {
            var discount = Read_From_MySQL.Fetch_Discounts_For_Customers(status: status, limit: limit, isSearch: isSearch, keyword: keyword);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            else
                return Ok(discount);
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<RestaurantsModel>))]
        [ProducesResponseType(400)]
        public IActionResult FetchRestaurants(string status = "all", int limit = 1000, bool isSearch = false, string keyword = "")
        {
            var restaurants = Read_From_MySQL.Fetch_Restaurants_For_Customers(status: status, limit: limit, isSearch: isSearch, keyword: keyword);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            else
                return Ok(restaurants);
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<CardModel>))]
        [ProducesResponseType(400)]
        public IActionResult FetchCard(string uuid)
        {
            var cards = Read_From_MySQL.Fetch_Cards_For_Customers(uuid: uuid);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            else
                return Ok(cards);
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<CartModel>))]
        [ProducesResponseType(400)]
        public IActionResult FetchCart(string uuid)
        {
            var cart = Read_From_MySQL.Fetch_Cart_For_Customers(uuid: uuid);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            else
                return Ok(cart);
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<FavouritesModel>))]
        [ProducesResponseType(400)]
        public IActionResult FetchFavourites(string uuid)
        {
            var favourites = Read_From_MySQL.Fetch_Favourites_For_Customers(uuid: uuid);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            else
                return Ok(favourites);
        }

        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public IActionResult AddToCart(string uuid, string productid, int quantity = 1)
        {
            //1. Check if all the parameters above are not invalid or contain a null value.
            if (ModelState.IsValid)
            {
                //2. Get the maximum cart item per customer
                int maxCartItemPerCustomer = int.Parse(Read_From_MySQL.SelectOneWhereOne(tableName: "AppData", columnName: "value", where1: "Variable", val1: "Maximum Cart Item Per Customer"));

                //3. Count total number item in customers cart
                int customerMaxCartItem = Read_From_MySQL.CountTotalWhereOne(tableName: "Cart", columnName: "UUID", columnVal: uuid);

                if (maxCartItemPerCustomer == customerMaxCartItem)
                {
                    return BadRequest("You have reached the maximum number of items you can add to your cart.");
                }
                else
                {
                    //4. Check if user exist with the provided uuid.
                    bool doesUserExist = Read_From_MySQL.DoesItExistOneWhereOne(tableName: "Customers", columnName: "uuid", columnVal: uuid);

                    if (doesUserExist)
                    {
                        //5. Check if recipe exist with the provided productid.
                        doesUserExist = Read_From_MySQL.DoesItExistOneWhereOne(tableName: "Recipe", columnName: "productid", columnVal: productid);

                        if (doesUserExist)
                        {
                            //6. Add Product/Recipe To Cart
                            Insert_In_MySQL.AddToCart(uuid: uuid, productid: productid, quantity: quantity);

                            //7. Success message is returned
                            return Ok("success");
                        }
                        else
                        {
                            return BadRequest("Session Expired, Please try again.");
                        }
                    }
                    else
                    {
                        return BadRequest("Session Expired, Please try again.");
                    }
                }
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public IActionResult AddToFavourites(string uuid, string productid)
        {
            //1. Check if all the parameters above are not invalid or contain a null value.
            if (ModelState.IsValid)
            {
                //2. Get the maximum favourite item per customer
                int maxFavouritesItemPerCustomer = int.Parse(Read_From_MySQL.SelectOneWhereOne(tableName: "AppData", columnName: "value", where1: "Variable", val1: "Maximum Favourite Item Per Customer"));

                //3. Count total number item on customers favourites list
                int customerMaxFavouritesItem = Read_From_MySQL.CountTotalWhereOne(tableName: "Favourites", columnName: "UUID", columnVal: uuid);

                if (maxFavouritesItemPerCustomer == customerMaxFavouritesItem)
                {
                    return BadRequest("You have reached the maximum number of items you can add to your favourites.");
                }
                else
                {
                    //4. Check if user exist with the provided uuid.
                    bool doesUserExist = Read_From_MySQL.DoesItExistOneWhereOne(tableName: "Customers", columnName: "uuid", columnVal: uuid);

                    if (doesUserExist)
                    {
                        //5. Check if recipe exist with the provided productid.
                        doesUserExist = Read_From_MySQL.DoesItExistOneWhereOne(tableName: "Recipe", columnName: "productid", columnVal: productid);

                        if (doesUserExist)
                        {
                            //6. Add Product/Recipe To Cart
                            Insert_In_MySQL.AddToFavourites(uuid: uuid, productid: productid);

                            //7. Success message is returned.
                            return Ok("success");
                        }
                        else
                        {
                            return BadRequest("Session Expired, Please try again.");
                        }
                    }
                    else
                    {
                        return BadRequest("Session Expired, Please try again.");
                    }
                }
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public IActionResult RemoveFromCart(string id)
        {
            //1. Check if all the parameters above are not invalid or contain a null value.
            if (ModelState.IsValid)
            {
                //2. Permanently remove product/recipe from cart for a customer
                Delete_On_MySQL.Delete1Item(row: id, table: "cart");

                //3. Success message is returned.
                return Ok("success");
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public IActionResult RemoveFromFavourites(string id)
        {
            //1. Check if all the parameters above are not invalid or contain a null value.
            if (ModelState.IsValid)
            {
                //2. Permanently remove product/recipe from favourites for a customer
                Delete_On_MySQL.Delete1Item(row: id, table: "favourites");

                //3. Success message is returned.
                return Ok("success");
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public IActionResult AddDiscount(string uuid, string bannerfilepath, string headline, string body, string duration, string calltoaction, string discount)
        {
            //1. Check if all the parameters above are not invalid or contain a null value.
            if (ModelState.IsValid)
            {
                //2. Get the maximum number of live discounts a partner/business can create
                int maxNumberOfLiveDiscountsPerPartner = int.Parse(Read_From_MySQL.SelectOneWhereOne(tableName: "AppData", columnName: "value", where1: "Variable", val1: "Maximum Discounts Per Partner"));

                //3. Count total number of live discount for this parner/business
                int totalNumberOfLiveDiscounts = Read_From_MySQL.CountTotalWhereTwo(tableName: "Discount", columnName1: "Status", columnVal1: "live", columnName2: "UUID", columnVal2: uuid);

                if (maxNumberOfLiveDiscountsPerPartner > totalNumberOfLiveDiscounts)
                {
                    //4. Add Discount
                    Insert_In_MySQL.AddDiscount(uuid: uuid, bannerfilepath: bannerfilepath, headline: headline, body: body, duration: duration, calltoaction: calltoaction, discount: discount);
                }
                else
                {
                    return BadRequest("You have reached the maximum number of live discounts. Delete or disable some.");
                }

                //5. Success message is returned.
                return Ok("success");
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public IActionResult RequestPayout(string uuid, string accesstoken, string amount, string bankname, string accountnumber)
        {
            //1. Check if all the parameters above are not invalid or contain a null value.
            if (ModelState.IsValid)
            {
                //2. Fetch the users temporal pin.
                string tempPin = Read_From_MySQL.SelectOneWhereOne(tableName: "Partners", columnName: "temppin", where1: "uuid", val1: uuid);

                //3: Compare the access token with users temporal pin, if they are the same Continue else return status 400(error meesage). Temporal pin and access token will be converted to lower, before comparing them.
                if (tempPin != null && tempPin.ToLower() == accesstoken.ToLower())
                {
                    //4. Check if user exist with the provided uuid.
                    bool doesUserExist = Read_From_MySQL.DoesItExistOneWhereOne(tableName: "Customers", columnName: "uuid", columnVal: uuid);

                    if (doesUserExist)
                    {
                        //2. Get withdrawal limit 
                        int withdrawalLimit = int.Parse(Read_From_MySQL.SelectOneWhereOne(tableName: "AppData", columnName: "value", where1: "Variable", val1: "Withdrawal Limit"));

                        if (withdrawalLimit > int.Parse(amount))
                        {
                            //4. Request withdrawal
                            Insert_In_MySQL.RequestPayout(uuid: uuid, amount: amount, bankname: bankname, accountnumber: accountnumber);
                        }
                        else
                        {
                            return BadRequest("Your withdrawal limit is $" + withdrawalLimit + ".");
                        }

                        //7. Success message is returned.
                        return Ok("success");
                    }
                    else
                    {
                        return BadRequest("Session Expired, Please try again.");
                    }
                }
                else
                {
                    return BadRequest("Session Expired, Please try again.");
                }
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public IActionResult PlaceOrder(string usertype, string accesstoken, string upid, string ucid, string orderid, string address, string phonenumber)
        {
            //1. Check if all the parameters above are not invalid or contain a null value.
            if (ModelState.IsValid)
            {
                //2. Fetch the users temporal pin.
                string tempPin = Read_From_MySQL.SelectOneWhereOne(tableName: usertype, columnName: "temppin", where1: "uuid", val1: ucid);

                //3: Compare the access token with users temporal pin, if they are the same Continue else return status 400(error meesage). Temporal pin and access token will be converted to lower, before comparing them.
                if (tempPin != null && tempPin.ToLower() == accesstoken.ToLower())
                {
                    //4. Check if user exist with the provided uuid.
                    bool doesUserExist = Read_From_MySQL.DoesItExistOneWhereOne(tableName: "Customers", columnName: "uuid", columnVal: ucid);

                    if (doesUserExist)
                    {
                        //2. Place Order
                        Insert_In_MySQL.PlaceOrder(upid: upid, ucid: ucid, orderid: orderid, address: address, phonenumber: phonenumber);

                        //7. Success message is returned.
                        return Ok("success");
                    }
                    else
                    {
                        return BadRequest("Session Expired, Please try again.");
                    }
                }
                else
                {
                    return BadRequest("Session Expired, Please try again.");
                }
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public IActionResult AddReview(string upid, string ucid, string review, string stars)
        {
            //1. Check if all the parameters above are not invalid or contain a null value.
            if (ModelState.IsValid)
            {
                //2. Check if customer has reviewed this partner/business before
                bool hasReviewed = Read_From_MySQL.DoesItExistOneWhereTwo(tableName: "Reviews", columnName1: "upid", columnVal1: upid, columnName2: "ucid", columnVal2: ucid);

                if (!hasReviewed)
                {
                    //3.1. If customer has not reviewed this partner/business add review
                    Insert_In_MySQL.AddReview(upid: upid, ucid: ucid, review: review, stars: stars);
                }
                else
                {
                    //3.2. If customer has reviewed this partner/business, update review
                    Update_To_MySql.UpdateTwoColumnWhereTwo(tableName: "Reviews", updateColumn: "review", updateVal: review, updateColumn2: "stars", updateVal2: stars, columnWhere: "upid", columnVal: upid, columnWhere2: "ucid", columnVal2: ucid);
                }

                //4. Success message is returned.
                return Ok("success");
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public IActionResult AddCard(string uuid, string cardholder, string cardnumber, string cvc, string expirydate)
        {
            //1. Check if all the parameters above are not invalid or contain a null value.
            if (ModelState.IsValid)
            {
                //2. Check if card already exists
                bool hasCard = Read_From_MySQL.DoesItExistOneWhereTwo(tableName: "Cards", columnName1: "cardnumber", columnVal1: cardnumber, columnName2: "cvc", columnVal2: cvc);

                if (!hasCard)
                {
                    //3. Add Card
                    Insert_In_MySQL.AddCard(uuid: uuid, cardholder: cardholder, cardnumber: cardnumber, cvc: cvc, expirydate: expirydate);
                }
                else
                {
                    return BadRequest("Similar card already exist.");
                }

                //4. Success message is returned.
                return Ok("success");
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public IActionResult DeleteAccount(string uuid, string usertype)
        {
            //1. Check if all the parameters above are not invalid or contain a null value.
            if (ModelState.IsValid)
            {
                //2. Delete users account
                Update_To_MySql.UpdateOnColumnWhereOne(tableName: usertype, updateColumn: "status", updateVal: "deleted", columnWhere: "uuid", columnVal: uuid);

                //3. Success message is returned.
                return Ok("success");
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public IActionResult DeleteDiscount(string uuid, string accesstoken, string id)
        {
            //1. Check if all the parameters above are not invalid or contain a null value.
            if (ModelState.IsValid)
            {
                //2. Check if user exist with the provided uuid.
                bool doesUserExist = Read_From_MySQL.DoesItExistOneWhereOne(tableName: "Partners", columnName: "uuid", columnVal: uuid);

                if (doesUserExist)
                {
                    //3. Fetch users temporal pin.
                    string tempPin = Read_From_MySQL.SelectOneWhereOne(tableName: "Partners", columnName: "temppin", where1: "uuid", val1: uuid);

                    //4: Compare the access token with users temporal pin, if they are the same Continue else return an status 400(error meesage). Temporal pin and access token will be converted to lower, before comparing them.
                    if (tempPin != null && tempPin.ToLower() == accesstoken.ToLower())
                    {
                        //5. Check if discount has been used.
                        string used = Read_From_MySQL.SelectOneWhereOne(tableName: "Discount", columnName: "used", where1: "id", val1: id);

                        if (used == "false")
                        {
                            //6.1. Permanently remove discount for a partner/business.
                            Delete_On_MySQL.Delete1Item(row: id, table: "Discount");
                        }
                        else
                        {
                            //6.2 Delete discount by chnaging status to deleted
                            Update_To_MySql.UpdateOnColumnWhereOne(tableName: "Discount", updateColumn: "status", updateVal: "deleted", columnWhere: "id", columnVal: id);
                        }

                        //7. Success message is returned.
                        return Ok("success");
                    }
                    else
                    {
                        return BadRequest("Session Expired, Please try again.");
                    }
                }
                else
                {
                    return BadRequest("Session Expired, Please try again.");
                }
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public IActionResult SendMessage(string uuid, string accesstoken, string recipientid, string payload, string replytext, string type, string chatid = "new", string usertype = "Customer")
        {
            //payload is the text or image path (if the type of message e=sent was an image) sent.

            //1. Check if all the parameters above are not invalid or contain a null value.
            if (ModelState.IsValid)
            {
                //2. Check if user exist with the provided uuid.
                bool doesUserExist = Read_From_MySQL.DoesItExistOneWhereOne(tableName: usertype, columnName: "uuid", columnVal: uuid);

                if (doesUserExist)
                {
                    //3. Fetch users temporal pin.
                    string tempPin = Read_From_MySQL.SelectOneWhereOne(tableName: usertype, columnName: "temppin", where1: "uuid", val1: uuid);

                    //4: Compare the access token with users temporal pin, if they are the same Continue else return an status 400(error meesage). Temporal pin and access token will be converted to lower, before comparing them.
                    if (tempPin != null && tempPin.ToLower() == accesstoken.ToLower())
                    {
                        //5. Send message
                        Insert_In_MySQL.SendMessage(chatid: chatid, fromid: uuid, toid: recipientid, message: payload, replytext: replytext, type: type, usertype: usertype);

                        //9. Success message is returned.
                        return Ok("success");
                    }
                    else
                    {
                        return BadRequest("Session Expired, Please try again.");
                    }
                }
                else
                {
                    return BadRequest("Session Expired, Please try again.");
                }
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

    }
}
