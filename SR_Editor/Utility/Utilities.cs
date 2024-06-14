using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SR_Editor.Core.Utility
{
    public class Utilities
    {
        public Utilities()
        {
        }
        
        
        public static string GetMoneyToTrString(string number, int pDilId)
        {
            return "";
            /*int i;
            object value;
            int num = 0;
            string str = "";
            while (true)
            {
                if ((num >= number.Length ? true : !char.IsNumber(number[num])))
                {
                    break;
                }
                str = string.Concat(str, number[num]);
                num++;
            }
            num++;
            string str1 = "";
            while (true)
            {
                if ((num >= number.Length ? true : !char.IsNumber(number[num])))
                {
                    break;
                }
                str1 = string.Concat(str1, number[num]);
                num++;
            }
            int num1 = 0;
            num1 = (str.Length % 3 != 0 ? str.Length / 3 + 1 : str.Length / 3);
            int length = str.Length % 3;
            int[] numArray = new int[num1];
            int num2 = 0;
            if (length != 0)
            {
                numArray[0] = int.Parse(str.Substring(0, length));
                num2 = 1;
            }
            for (i = length; i + 2 < str.Length; i += 3)
            {
                numArray[num2] = int.Parse(str.Substring(i, 3));
                num2++;
            }
            string str2 = "";
            string[] strArrays = new string[] { "", "Bir", "İki", "Üç", "Dört", "Beş", "Altı", "Yedi", "Sekiz", "Dokuz" };
            string[] strArrays1 = strArrays;
            strArrays = new string[] { "", "On", "Yirmi", "Otuz", "Kırk", "Elli", "Altmış", "Yetmiş", "Seksen", "Doksan", "Yüz" };
            string[] strArrays2 = strArrays;
            strArrays = new string[] { "", "Bin", "Milyon", "Milyar", "Trilyon" };
            string[] str3 = strArrays;
            strArrays = new string[] { " TL", " KURUŞ" };
            string[] strArrays3 = strArrays;
            if (pDilId != 1)
            {
                for (i = 0; i < strArrays1.Count<string>(); i++)
                {
                    if (strArrays1[i] != "")
                    {
                        value = PusulaEntities.Instance.Sistem.ForeignKeyRelationQuery.GetValue(string.Concat("select [Sistem].[ufns_GetTutarOkunusu]('", strArrays1[i], "')"));
                        if (value != null)
                        {
                            strArrays1[i] = Convert.ToString(value);
                        }
                    }
                }
                for (i = 0; i < strArrays2.Count<string>(); i++)
                {
                    if (strArrays2[i] != "")
                    {
                        value = PusulaEntities.Instance.Sistem.ForeignKeyRelationQuery.GetValue(string.Concat("select [Sistem].[ufns_GetTutarOkunusu]('", strArrays2[i], "')"));
                        if (value != null)
                        {
                            strArrays2[i] = Convert.ToString(value);
                        }
                    }
                }
                for (i = 0; i < str3.Count<string>(); i++)
                {
                    if (str3[i] != "")
                    {
                        value = PusulaEntities.Instance.Sistem.ForeignKeyRelationQuery.GetValue(string.Concat("select [Sistem].[ufns_GetTutarOkunusu]('", str3[i], "')"));
                        if (value != null)
                        {
                            str3[i] = Convert.ToString(value);
                        }
                    }
                }
                for (i = 0; i < strArrays3.Count<string>(); i++)
                {
                    if (strArrays3[i] != "")
                    {
                        value = PusulaEntities.Instance.Sistem.ForeignKeyRelationQuery.GetValue(string.Concat("select [Sistem].[ufns_GetTutarOkunusu]('", strArrays3[i], "')"));
                        if (value != null)
                        {
                            strArrays3[i] = Convert.ToString(value);
                        }
                    }
                }
            }
            for (i = (int)numArray.Length - 1; i >= 0; i--)
            {
                int num3 = 0;
                string str4 = "";
                if (numArray[i] / 100 != 0)
                {
                    num3 = 1000;
                    str4 = (numArray[i] % num3 / 100 != 1 ? string.Concat(str4, strArrays1[numArray[i] % num3 / 100], strArrays2[10]) : string.Concat(str4, strArrays2[10]));
                }
                if (numArray[i] / 10 != 0)
                {
                    num3 = 100;
                    str4 = string.Concat(str4, strArrays2[numArray[i] % num3 / 10]);
                }
                num3 = 10;
                if (numArray[i] % num3 != 0)
                {
                    str4 = (((int)numArray.Length != 2 || numArray[i] != numArray[0] || numArray[0] % num3 != 1 ? true : !string.IsNullOrWhiteSpace(str4)) ? string.Concat(str4, strArrays1[numArray[i] % num3]) : str4 ?? "");
                }
                str2 = string.Concat(str4, str3[(int)numArray.Length - 1 - i], str2);
            }
            str2 = string.Concat(str2, strArrays3[0]);
            if (str1 != "")
            {
                if (decimal.Parse(str1) != new decimal(0))
                {
                    str2 = string.Concat(str2, strArrays2[int.Parse(str1) % 100 / 10]);
                    str2 = string.Concat(str2, strArrays1[int.Parse(str1) % 10]);
                    str2 = string.Concat(str2, strArrays3[1]);
                }
            }
            return str2;
            */
        }

        public static string GetMoneyToUSDString(string number)
        {
            int i;
            int num = 0;
            string str = "";
            while (true)
            {
                if ((num >= number.Length ? true : !char.IsNumber(number[num])))
                {
                    break;
                }
                str = string.Concat(str, number[num]);
                num++;
            }
            num++;
            string str1 = "";
            while (true)
            {
                if ((num >= number.Length ? true : !char.IsNumber(number[num])))
                {
                    break;
                }
                str1 = string.Concat(str1, number[num]);
                num++;
            }
            int num1 = 0;
            num1 = (str.Length % 3 != 0 ? str.Length / 3 + 1 : str.Length / 3);
            int length = str.Length % 3;
            int[] numArray = new int[num1];
            int num2 = 0;
            if (length != 0)
            {
                numArray[0] = int.Parse(str.Substring(0, length));
                num2 = 1;
            }
            for (i = length; i + 2 < str.Length; i += 3)
            {
                numArray[num2] = int.Parse(str.Substring(i, 3));
                num2++;
            }
            string str2 = "";
            string[] strArrays = new string[] { "", "One", "Two", "Three", "Four", "Five", "Six", "Seven", "Eight", "Nine" };
            string[] strArrays1 = strArrays;
            strArrays = new string[] { "", "Ten", "Twenty", "Thirty", "Fourty", "Fifty", "Sixty", "Seventy", "Eighty", "Ninety", "Hundred" };
            string[] strArrays2 = strArrays;
            strArrays = new string[] { "", "Thousand", "Million", "Billion", "Trillion" };
            string[] strArrays3 = strArrays;
            strArrays = new string[] { " US Dollar ", " Cent " };
            string[] strArrays4 = strArrays;
            for (i = (int)numArray.Length - 1; i >= 0; i--)
            {
                int num3 = 0;
                string str3 = "";
                if (numArray[i] / 100 != 0)
                {
                    num3 = 1000;
                    str3 = (numArray[i] % num3 / 100 != 1 ? string.Concat(str3, strArrays1[numArray[i] % num3 / 100], strArrays2[10]) : string.Concat(str3, strArrays2[10]));
                }
                if (numArray[i] / 10 != 0)
                {
                    num3 = 100;
                    str3 = string.Concat(str3, strArrays2[numArray[i] % num3 / 10]);
                }
                num3 = 10;
                if (numArray[i] % num3 != 0)
                {
                    str3 = (((int)numArray.Length != 2 || numArray[i] != numArray[0] || numArray[0] % num3 != 1 ? true : !string.IsNullOrWhiteSpace(str3)) ? string.Concat(str3, strArrays1[numArray[i] % num3]) : str3 ?? "");
                }
                str2 = string.Concat(str3, strArrays3[(int)numArray.Length - 1 - i], str2);
            }
            str2 = string.Concat(str2, strArrays4[0]);
            if (str1 != "")
            {
                if (decimal.Parse(str1) != new decimal(0))
                {
                    str2 = string.Concat(str2, strArrays2[int.Parse(str1) % 100 / 10]);
                    str2 = string.Concat(str2, strArrays1[int.Parse(str1) % 10]);
                    str2 = string.Concat(str2, strArrays4[1]);
                }
            }
            if (str2.StartsWith("Hundred"))
            {
                str2 = string.Concat("One", str2);
            }
            if (str2.StartsWith("Thousand"))
            {
                str2 = string.Concat("One", str2);
            }
            if (str2.StartsWith("Million"))
            {
                str2 = string.Concat("One", str2);
            }
            str2 = str2.Replace("TenOne", "Eleven");
            str2 = str2.Replace("TenTwo", "Twelve");
            str2 = str2.Replace("TenThree", "Thirteen");
            str2 = str2.Replace("TenFour", "Fourteen");
            str2 = str2.Replace("TenFive", "Fifteen");
            str2 = str2.Replace("TenSix", "Sixteen");
            str2 = str2.Replace("TenSeven", "Seventeen");
            str2 = str2.Replace("TenEight", "Eighteen");
            str2 = str2.Replace("TenNine", "Nineteen");
            str2 = str2.Replace("ThousandHundred", "ThousandOneHundred");
            return str2;
        }

        public static Screen GetSecondaryScreen()
        {
            Screen screen;
            if ((int)Screen.AllScreens.Length != 1)
            {
                Screen[] allScreens = Screen.AllScreens;
                int num = 0;
                while (num < (int)allScreens.Length)
                {
                    Screen screen1 = allScreens[num];
                    if (screen1.Primary)
                    {
                        num++;
                    }
                    else
                    {
                        screen = screen1;
                        return screen;
                    }
                }
                screen = null;
            }
            else
            {
                screen = null;
            }
            return screen;
        }

        public static bool IsNumeric(object Expression)
        {
            double num;
            bool flag = double.TryParse(Convert.ToString(Expression), NumberStyles.Any, (IFormatProvider)NumberFormatInfo.InvariantInfo, out num);
            return flag;
        }
    }
}
