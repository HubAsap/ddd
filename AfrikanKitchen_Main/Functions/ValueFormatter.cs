using System.Globalization;
using System.Text.RegularExpressions;
using System.Text;
using System.Security.Cryptography;

namespace AfrikanKitchen_Main.Functions
{
    public class ValueFormatter
    {
        private static byte[] key = new byte[8] { 1, 2, 3, 4, 5, 6, 7, 8 };
        private static byte[] iv = new byte[8] { 1, 2, 3, 4, 5, 6, 7, 8 };

        public static string GenerateNewUniqueAffiliateID()
        {
            bool codeExists = true;
            string code = "";

            do
            {
                code = GenerateRamdomCapsLetterAndDigitsOnly(10);
                if (!Read_From_MySQL.DoesItExistOneWhereOne(tableName: "Users", columnName: "RefCode", columnVal: code))
                {
                    codeExists = false;
                }
            } while (codeExists);

            return code;
        }

        public static string GenerateRamdomCapsLetterAndDigitsOnly(int max)
        {
            const string charLibray = "ABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
            StringBuilder result = new StringBuilder();
            Random rnd = new Random();
            while (0 < max--)
            {
                result.Append(charLibray[rnd.Next(charLibray.Length)]);
            }
            return result.ToString();
        }

        public static int GenerateRamdomBinaryOnly(int max = 1)
        {
            const string charLibray = "01";
            StringBuilder result = new StringBuilder();
            Random rnd = new Random();
            while (0 < max--)
            {
                result.Append(charLibray[rnd.Next(charLibray.Length)]);
            }
            return int.Parse(result.ToString());
        }

        public static string GenerateRamdomDigitsOnly(int max)
        {
            const string charLibray = "1234567890";
            StringBuilder result = new StringBuilder();
            Random rnd = new Random();
            while (0 < max--)
            {
                result.Append(charLibray[rnd.Next(charLibray.Length)]);
            }
            return result.ToString();
        }


        private static bool equalIgnoreCase(string str1, string str2)
        {

            int i = 0;


            // length of first string 

            int len1 = str1.Length;


            // length of second string 

            int len2 = str2.Length;


            // if length is not same 

            // simply return false since both string 

            // can not be same if length is not equal 

            if (len1 != len2)

                return false;


            // loop to match one by one 

            // all characters of both string 

            while (i < len1)

            {


                // if current characters of both string are same, 

                // increase value of i to compare next character 

                if (str1[i] == str2[i])

                {

                    i++;

                }


                // if any character of first string

                // is some special character 

                // or numeric character and 

                // not same as corresponding character 

                // of second string then return false 

                else if (!((str1[i] >= 'a' && str1[i] <= 'z')

                        || (str1[i] >= 'A' && str1[i] <= 'Z')))

                {

                    return false;

                }


                // do the same for second string 

                else if (!((str2[i] >= 'a' && str2[i] <= 'z')

                        || (str2[i] >= 'A' && str2[i] <= 'Z')))

                {

                    return false;

                }


                // this block of code will be executed 

                // if characters of both strings 

                // are of different cases 

                else

                {

                    // compare characters by ASCII value 

                    if (str1[i] >= 'a' && str1[i] <= 'z')

                    {

                        if (str1[i] - 32 != str2[i])

                            return false;

                    }


                    else if (str1[i] >= 'A' && str1[i] <= 'Z')

                    {

                        if (str1[i] + 32 != str2[i])

                            return false;

                    }


                    // if characters matched, 

                    // increase the value of i to compare next char 

                    i++;


                } // end of outer else block 


            } // end of while loop 


            // if all characters of the first string 

            // are matched with corresponding 

            // characters of the second string, 

            // then return true 

            return true;


        } // end of equalIgnoreCase function 

        // Function to print the same or not same 
        // if strings are equal or not equal 

        public static bool EqualIgnoreCaseUtil(string str1, string str2)
        {

            return equalIgnoreCase(str1, str2);
        }

        public static double ArithEvaluator(String expr)
        {
            try
            {

                Stack<String> stack = new Stack<String>();

                string value = "";
                for (int i = 0; i < expr.Length; i++)
                {
                    String s = expr.Substring(i, 1);
                    char chr = s.ToCharArray()[0];

                    if (!char.IsDigit(chr) && chr != '.' && value != "")
                    {
                        stack.Push(value);
                        value = "";
                    }

                    if (s.Equals("("))
                    {

                        string innerExp = "";
                        i++; //Fetch Next Character
                        int bracketCount = 0;
                        for (; i < expr.Length; i++)
                        {
                            s = expr.Substring(i, 1);

                            if (s.Equals("("))
                                bracketCount++;

                            if (s.Equals(")"))
                                if (bracketCount == 0)
                                    break;
                                else
                                    bracketCount--;


                            innerExp += s;
                        }

                        stack.Push(ArithEvaluator(innerExp).ToString());

                    }
                    else if (s.Equals("+")) stack.Push(s);
                    else if (s.Equals("-")) stack.Push(s);
                    else if (s.Equals("*")) stack.Push(s);
                    else if (s.Equals("/")) stack.Push(s);
                    else if (s.Equals("sqrt")) stack.Push(s);
                    else if (s.Equals(")"))
                    {
                    }
                    else if (char.IsDigit(chr) || chr == '.')
                    {
                        value += s;

                        if (value.Split('.').Length > 2)
                            throw new Exception("Invalid decimal.");

                        if (i == (expr.Length - 1))
                            stack.Push(value);

                    }
                    else
                        throw new Exception("Invalid character.");

                }


                double result = 0;
                while (stack.Count >= 3)
                {

                    double right = Convert.ToDouble(stack.Pop());
                    string op = stack.Pop();
                    double left = Convert.ToDouble(stack.Pop());

                    if (op == "+") result = left + right;
                    else if (op == "+") result = left + right;
                    else if (op == "-") result = left - right;
                    else if (op == "*") result = left * right;
                    else if (op == "/") result = left / right;

                    stack.Push(result.ToString());
                }


                return Convert.ToDouble(stack.Pop());
            }
            catch (Exception)
            {
                return 0.00;
            }

        }

        public static bool IsLetterUpperCase(char c)
        {
            return (c >= 'A' && c <= 'Z');
        }

        public static bool IsLetterLowerCase(char c)
        {
            return (c >= 'a' && c <= 'z');
        }

        public static bool IsDigit(char c)
        {
            return c >= '0' && c <= '9';
        }

        public static bool IsSymbol(char c)
        {
            return c > 32 && c < 127 && !IsDigit(c) && !IsLetterUpperCase(c) && IsLetterLowerCase(c);
        }

        public static bool isStringUrl(string uriName)
        {
            Uri uriResult;
            bool result = Uri.TryCreate(uriName, UriKind.Absolute, out uriResult)
                && uriResult.Scheme == Uri.UriSchemeHttp;
            return result;
        }

        public static bool IsNullOrEmptyOrAllSpaces(string str)
        {
            if (str == null || str.Length == 0)
            {
                return true;
            }

            for (int i = 0; i < str.Length; i++)
            {
                if (!Char.IsWhiteSpace(str[i])) return false;
            }

            return true;
        }

        public static bool HasSymbol(string myVal)
        {
            var regexItem = new Regex("^[a-zA-Z0-9 ]*$");

            return regexItem.IsMatch(myVal) ? true : false;
        }
        
        public static bool ValidateU(string myVal)
        {
            var regexItem = new Regex("^[a-zA-Z0-9_]*$");

            return regexItem.IsMatch(myVal) ? true : false;
        }

        public static string BalanceFormatter_NoDescimalPlace(string currencySymbol, double balance)
        {
            string formattedBalance = "";

            NumberFormatInfo format = new NumberFormatInfo();
            int[] indSizes = { 3, 3, 2 };
            format.CurrencyDecimalDigits = 0;
            format.CurrencyDecimalSeparator = ".";
            format.CurrencyGroupSeparator = ",";
            format.CurrencySymbol = currencySymbol;
            format.CurrencyGroupSizes = indSizes;

            formattedBalance = balance.ToString("C", format);
            return formattedBalance;
        }
        
        public static string BalanceFormatter(string currencySymbol, double balance)
        {
            string formattedBalance = "";

            NumberFormatInfo format = new NumberFormatInfo();
            int[] indSizes = { 3, 3, 2 };
            format.CurrencyDecimalDigits = 2;
            format.CurrencyDecimalSeparator = ".";
            format.CurrencyGroupSeparator = ",";
            format.CurrencySymbol = currencySymbol;
            format.CurrencyGroupSizes = indSizes;

            formattedBalance = balance.ToString("C", format);
            return formattedBalance;
        }

        public static bool IsANumber(string val = "")
        {
            if (val == "" || val == null)
            {
                return false;
            }
            else
            {
                return Regex.IsMatch(val, @"^\d+$");
            }
        }

        public static bool ISADecimal(string val)
        {
            return Decimal.TryParse(val, out _);
        }
        
        public static bool IsValidEmail(string email)
        {
            return Regex.IsMatch(email, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);
        }

        public static string Crypt(string text)
        {
            SymmetricAlgorithm algorithm = DES.Create();
            ICryptoTransform transform = algorithm.CreateEncryptor(key, iv);
            byte[] inputbuffer = Encoding.Unicode.GetBytes(text);
            byte[] outputBuffer = transform.TransformFinalBlock(inputbuffer, 0, inputbuffer.Length);
            return Convert.ToBase64String(outputBuffer);
        }

        public static string Decrypt(string text)
        {
            SymmetricAlgorithm algorithm = DES.Create();
            ICryptoTransform transform = algorithm.CreateDecryptor(key, iv);
            byte[] inputbuffer = Convert.FromBase64String(text);
            byte[] outputBuffer = transform.TransformFinalBlock(inputbuffer, 0, inputbuffer.Length);
            return Encoding.Unicode.GetString(outputBuffer);
        }
    }
}
