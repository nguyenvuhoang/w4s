using System.Globalization;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace O24OpenAPI.Web.CMS.Utils;

  public static class Utility
    {
        
        /// <summary>
        /// 
        /// </summary>
        public static string ComputeMd5Hash(JObject obj)
        {
            string jsonString = obj.ToString();
            using (MD5 md5Hash = MD5.Create())
            {
                byte[] bytes = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(jsonString));
                StringBuilder builder = new StringBuilder();
                foreach (byte b in bytes)
                {
                    builder.Append(b.ToString("x2"));
                }
                return builder.ToString();
            }
        }
        
        //format dd/mm/yyyy HH:MM:ss 
        /// <summary>
        /// 
        /// </summary>
        public static string FormatDatetime(string datetime, string formatInput, string formatOutput)
        {
            string dateout = "";

            dateout = DateTime.ParseExact(datetime, formatInput, null).ToString(formatOutput);

            return dateout;
        }
        /// <summary>
        /// 
        /// </summary>
        public static string FormatDatetime(string datetime, string formatOutput)
        {
            string dateout = "";

            dateout = DateTime.Parse(datetime).ToString(formatOutput);

            return dateout;
        }
        /// <summary>
        /// 
        /// </summary>
        public static DateTime IsDateTime1(string s)
        {
            return DateTime.Parse(s, new CultureInfo("vi-VN", false));
        }
        /// <summary>
        /// 
        /// </summary>
        public static DateTime IsDateTime2(string s)
        {
            return DateTime.Parse(s);
        }

        /// <summary>
        /// 
        /// </summary>
        public static double isDouble(string s, bool isCulture)
        {

            if (isCulture)
            {
                CultureInfo dk = new CultureInfo("en-US");
                return double.Parse(s, dk);
            }
            else
            {
                return double.Parse(s);
            }

        }
        /// <summary>
        /// 
        /// </summary>
        public static double CheckIsDouble(string s, bool isCulture = false)
        {
            try
            {
                if (isCulture)
                {
                    CultureInfo dk = new CultureInfo("en-US");
                    return double.Parse(s, dk);
                }
                else
                {
                    return double.Parse(s);
                }
            }
            catch { return 0; }

        }

        /// <summary>
        /// 
        /// </summary>
        public static long isLong(string s, bool isCulture)
        {
            if (isCulture)
            {
                CultureInfo dk = new CultureInfo("en-US");
                return long.Parse(s, dk);
            }
            else
            {
                return long.Parse(s);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public static decimal isDecimal(string s, bool isCulture)
        {
            if (string.IsNullOrEmpty(s))
            {
                return 0;
            }
            if (isCulture)
            {
                CultureInfo dk = new CultureInfo("en-US");
                return decimal.Parse(s, dk);
            }
            else
            {
                return decimal.Parse(s);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static string FormatMoneyInputToView(string m, string CCYID)
        {
            try
            {
                CultureInfo dk = new CultureInfo("en-US");
                string m1 = m;

                if (m == "" || m == "0" || m == "0,00" || m == "0.00")
                {
                    m1 = "0";
                }
                else
                {
                    switch (CCYID)
                    {
                        case "MMK":
                            //return double.Parse(m).ToString(",", dk);
                            //return double.Parse(m,dk).ToString("#,##0.##");
                            //m1 = double.Parse(m,dk).ToString("#,#", dk); VND
                            m1 = double.Parse(m, dk).ToString("#,#0.00", dk);
                            //return string.Format(dk, "{0:0,0}", double.Parse(m,dk));
                            //return Double.Parse(m,dk.NumberFormat).ToString("N00", dk.NumberFormat);
                            //return String.Format(CultureInfo.CreateSpecificCulture("en-US"), "{0:C}",m);
                            break;
                        default:
                            //return double.Parse(m).ToString(",", dk);
                            //return double.Parse(m,dk).ToString("#,##0.##");
                            m1 = double.Parse(m, dk).ToString("#,#0.00", dk);
                            //return string.Format(dk, "{0:0,0}", double.Parse(m,dk));
                            //return Double.Parse(m,dk.NumberFormat).ToString("N00", dk.NumberFormat);
                            //return String.Format(CultureInfo.CreateSpecificCulture("en-US"), "{0:C}",m);
                            break;

                    }
                }

                return m1;
            }
            catch (Exception)
            {
                return m;
            }
        }

        /// <summary>
        /// ham nay format khi lay data tu bat ky dau ma so dang english
        /// </summary>
        /// <param name="m"></param>
        /// <param name="CCYID"></param>
        /// <returns></returns>
        public static string FormatMoneyInput(string m, string CCYID)
        {
            try
            {
                CultureInfo dk = new CultureInfo("en-US");
                string m1 = m;

                if (m == "" || m == "0" || m == "0,00" || m == "0.00")
                {
                    m1 = "0";
                }
                else
                {
                    switch (CCYID)
                    {
                        case "MMK":
                            //return double.Parse(m).ToString(",", dk);
                            //return double.Parse(m,dk).ToString("#,##0.##");
                            //m1 = double.Parse(m,dk).ToString("0,0", dk); VND
                            m1 = double.Parse(m, dk).ToString("0,0.00", dk);
                            //return string.Format(dk, "{0:0,0}", double.Parse(m,dk));
                            //return Double.Parse(m,dk.NumberFormat).ToString("N00", dk.NumberFormat);
                            //return String.Format(CultureInfo.CreateSpecificCulture("en-US"), "{0:C}",m);
                            break;
                        default:
                            //return double.Parse(m).ToString(",", dk);
                            //return double.Parse(m,dk).ToString("#,##0.##");
                            m1 = double.Parse(m, dk).ToString("0,0.00", dk);
                            //return string.Format(dk, "{0:0,0}", double.Parse(m,dk));
                            //return Double.Parse(m,dk.NumberFormat).ToString("N00", dk.NumberFormat);
                            //return String.Format(CultureInfo.CreateSpecificCulture("en-US"), "{0:C}",m);
                            break;

                    }
                }

                return m1.Replace(",", "");
            }
            catch (Exception)
            {
                return m;
            }
        }

        #region Format Text, QueryString and Input Validation

        /// <summary>
        /// Performs querystring validation
        /// </summary>
        /// <returns>Validate for potential SQL and XSS injection</returns>
        public static string KillSqlInjection(string TexttoValidate)
        {
            string TextVal;

            TextVal = TexttoValidate;
            if (string.IsNullOrEmpty(TextVal))
            {
                return TextVal;
            }

            //Build an array of characters that need to be filter.
            string[] strDirtyQueryString = { "xp_", "$", "#", "%", "*", "^", "&", "!", ";", "--", "<", ">", "script", "iframe", "delete", "drop", "exec" };

            //Loop through all items in the array
            foreach (string item in strDirtyQueryString)
            {
                if (TextVal.IndexOf(item) != -1)
                {
                    TextVal = TextVal.Replace(item, "");
                }
            }

            return TextVal;
        }
        #endregion

        /// <summary>
        /// 
        /// </summary>
        public static int IsInt(string text)
        {
            int i = 0;
            i = int.Parse(text.Replace(",", "").Replace(".", ""));

            return i;
        }

        /// <summary>
        /// 
        /// </summary>
        public static bool IsDateTime(string date)
        {
            try
            {
                DateTime.Parse(date);
                return true;
            }
            catch
            {
                return false;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public static bool IsDateTimeViet(string date)
        {
            try
            {
                DateTime.Parse(date, new CultureInfo("vi-VN", false));
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static string FormatStringCore(string input)
        {
            //string oP= input.Split('.')[0];
            //return oP;

            return input;

        }

        /// <summary>
        /// 
        /// </summary>
        public static string GenPassword(int lengt)
        {
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var random = new Random();
            var result = new string(
                Enumerable.Repeat(chars, lengt)
                          .Select(s => s[random.Next(s.Length)])
                          .ToArray());
            return result;
        }
        /// <summary>
        /// 
        /// </summary>
        public static string GetID(string prefix, string custcode, string custType, int length)
        {
            int a = length - (prefix + custcode + custType).Length;
            string pre = prefix + custcode + custType;

            if (a > 2)
            {
                string sub = "";
                for (int i = 0; i < a; i++)
                {
                    sub += Convert.ToInt32(new Random().Next(0, 9)).ToString();
                    Thread.Sleep(50);
                }

                return pre + sub;
            }
            else
            {
                return pre + new Random().Next(10000);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static string IntegerToWords(long inputNum)
        {
            int dig1, dig2, dig3, level = 0, lasttwo, threeDigits;

            string retval = "";
            string x = "";
            string[] ones ={
                "zero",
                "one",
                "two",
                "three",
                "four",
                "five",
                "six",
                "seven",
                "eight",
                "nine",
                "ten",
                "eleven",
                "twelve",
                "thirteen",
                "fourteen",
                "fifteen",
                "sixteen",
                "seventeen",
                "eighteen",
                "nineteen"
              };
            string[] tens ={
                "zero",
                "ten",
                "twenty",
                "thirty",
                "forty",
                "fifty",
                "sixty",
                "seventy",
                "eighty",
                "ninety"
              };
            string[] thou ={
                "",
                "thousand",
                "million",
                "billion",
                "trillion",
                "quadrillion",
                "quintillion"
              };

            bool isNegative = false;
            if (inputNum < 0)
            {
                isNegative = true;
                inputNum *= -1;
            }

            if (inputNum == 0)
        {
            return "zero";
        }

        string s = inputNum.ToString();

            while (s.Length > 0)
            {
                // Get the three rightmost characters
                x = s.Length < 3 ? s : s.Substring(s.Length - 3, 3);

                // Separate the three digits
                threeDigits = int.Parse(x);
                lasttwo = threeDigits % 100;
                dig1 = threeDigits / 100;
                dig2 = lasttwo / 10;
                dig3 = threeDigits % 10;

                // append a "thousand" where appropriate
                if (level > 0 && dig1 + dig2 + dig3 > 0)
                {
                    retval = thou[level] + " " + retval;
                    retval = retval.Trim();
                }

                // check that the last two digits is not a zero
                if (lasttwo > 0)
                {
                    if (lasttwo < 20) // if less than 20, use "ones" only
                {
                    retval = ones[lasttwo] + " " + retval;
                }
                else // otherwise, use both "tens" and "ones" array
                {
                    retval = tens[dig2] + " " + ones[dig3] + " " + retval;
                }
            }

                // if a hundreds part is there, translate it
                if (dig1 > 0)
            {
                retval = ones[dig1] + " hundred " + retval;
            }

            s = s.Length - 3 > 0 ? s.Substring(0, s.Length - 3) : "";
                level++;
            }

            while (retval.IndexOf("  ") > 0)
        {
            retval = retval.Replace("  ", " ");
        }

        retval = retval.Trim();

            if (isNegative)
        {
            retval = "negative " + retval;
        }

        retval = retval[0].ToString().ToUpper() + retval.Substring(1);
            return retval;
        }

        /// <summary>
        /// 
        /// </summary>
        public static DateTime ConvertStringToDateTime(string datetimeString)
        {
            CultureInfo provider = CultureInfo.InvariantCulture;
            return DateTime.ParseExact(datetimeString, "dd/MM/yyyy", provider);
        }

        /// <summary>
        /// 
        /// </summary>
        public static string ConvertDateTimeToString(DateTime datetime)
        {
            CultureInfo provider = CultureInfo.InvariantCulture;
            return datetime.ToString("dd/MM/yyyy", provider);
        }

        /// <summary>
        /// 
        /// </summary>
        public static string ConvertDateTimeToStringDatetime(DateTime datetime)
        {
            CultureInfo provider = CultureInfo.InvariantCulture;
            return datetime.ToString("dd/MM/yyyy HH:mm:ss", provider);
        }

        /// <summary>
        /// 
        /// </summary>
        public static bool IsDate(string tempDate, string[] fortmat)
        {
            if (DateTime.TryParseExact(tempDate, fortmat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public static T ToModel<T>(this IDictionary<string, object> source)
               where T : class, new()
        {
            var model = new T();
            try
            {
                var jsonFields = JsonConvert.SerializeObject(source);
                model = JsonConvert.DeserializeObject<T>(jsonFields);
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return model;
        }

        /// <summary>
        /// 
        /// </summary>
        public static IDictionary<string, object> AsDictionary(this object source, BindingFlags bindingAttr = BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Instance)
        {
            return source.GetType().GetProperties(bindingAttr).ToDictionary
            (
                propInfo => propInfo.Name,
                propInfo => propInfo.GetValue(source, null)
            );

        }

        /// <summary>
        /// Check is number
        /// </summary>
        public static bool IsNumber(string num)
        {
            try
            {
                int number = int.Parse(num);
                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }

        /// <summary>
        /// Injection
        /// </summary>
        public static string Injection(string str)
        {
            string strChange = "";
            List<string> key = new List<string> { "*", "%", "{", "}", "=", "@@", "@", ";", "/*", "*/" };
            foreach (var item in key)
            {
                if (str.Contains(item))
                {
                    str = str.Replace(item, strChange).Trim();
                }
            }

            return str;
        }

        /// <summary>
        /// 
        /// </summary>
        public static JObject ToLowerKey(this JObject jObject)
        {
            if (jObject == null)
        {
            return null;
        }

        JObject jsOriginal = new JObject();

            foreach (var jsValue in jObject)
            {
                jsOriginal.Add(jsValue.Key.ToLower(), jsValue.Value.ToString().Trim());
            }
            return jsOriginal;
        }

        /// <summary>
        /// 
        /// </summary>
        public static JArray ToLowerKey(this JArray jObject)
        {
            if (jObject == null)
        {
            return null;
        }

        JArray jsOriginal = new JArray();

            foreach (var js in jObject.Children<JObject>())
            {
                jsOriginal.Add(js.ToLowerKey());
            }

            return jsOriginal;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="jObject"></param>
        /// <returns></returns>
        public static JArray LowerKey(this JArray jObject)
        {
            if (jObject == null)
        {
            return null;
        }

        JArray jsOriginal = new JArray();

            foreach (var js in jObject.Children<JObject>())
            {
                jsOriginal.Add(js.ConvertToJObject());
            }

            return jsOriginal;
        }

        /// <summary>
        /// 
        /// </summary>
        public static JObject ToUpperKey(this JObject jObject)
        {
            if (jObject == null)
        {
            return null;
        }

        JObject jsOriginal = new JObject();

            foreach (var jsValue in jObject)
            {
                jsOriginal.Add(jsValue.Key.ToUpper(), jsValue.Value.ToString().Trim());
            }

            return jsOriginal;
        }

        /// <summary>
        /// 
        /// </summary>
        public static void Rename(this JToken token, string newName)
        {
            var parent = token.Parent;
            if (parent == null)
        {
            throw new InvalidOperationException("The parent is missing.");
        }

        var newToken = new JProperty(newName, token);
            parent.Replace(newToken);
        }

        /// <summary>
        /// 
        /// </summary>
        public static void AddLowerKey(this JObject jsObject, string propertyName, JToken value)
        {
            jsObject.Add(propertyName.ToLower(), value);
        }
    }