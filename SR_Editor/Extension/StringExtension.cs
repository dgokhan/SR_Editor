using SR_Editor.LookUp;
using System;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;

namespace SR_Editor.Core
{
	public static class StringExtension
	{
		public static string CeviriYap(this string pStr)
		{
            return pStr;
			/*string ceviri;
			if (pStr == null)
			{
				ceviri = null;
			}
			else if (Editor.Core.EditorApplication.EditorApplication.LanguageId != 1)
			{
				DilCeviriEntity aktifDilCeviriEntity = DilCeviri.GetAktifDilCeviriEntity(pStr);
				if (aktifDilCeviriEntity.IsDolu())
				{
					if (aktifDilCeviriEntity.Ceviri.IsDolu())
					{
						ceviri = aktifDilCeviriEntity.Ceviri;
						return ceviri;
					}
				}
				ceviri = pStr;
			}
			else
			{
				ceviri = pStr;
			}
			return ceviri;*/
		}

		public static string CleanString(string s)
		{
			if ((s == null ? false : s.Length > 0))
			{
				StringBuilder stringBuilder = new StringBuilder(s.Length);
				string str = s;
				for (int i = 0; i < str.Length; i++)
				{
					char chr = str[i];
					if (chr == '\r')
					{
						stringBuilder.Append(chr);
					}
					else if (chr == '\n')
					{
						stringBuilder.Append(chr);
					}
					else if (chr != '\t')
					{
						stringBuilder.Append((char.IsControl(chr) ? ' ' : chr));
					}
					else
					{
						stringBuilder.Append(chr);
					}
				}
				s = stringBuilder.ToString();
			}
			return s;
		}

		public static string Description<T>(this T obj, Expression<Func<T, string>> value)
		{
			string description;
			MemberExpression body = value.Body as MemberExpression;
			if (body == null)
			{
				description = null;
			}
			else
			{
				object[] customAttributes = body.Member.GetCustomAttributes(typeof(DescriptionAttribute), true);
				description = ((DescriptionAttribute)customAttributes[0]).Description;
			}
			return description;
		}

		public static bool IsDateTime(this string txtDate)
		{
			DateTime dateTime;
			return (DateTime.TryParse(txtDate, out dateTime) ? true : false);
		}

		public static bool IsDecimal(this string str)
		{
			bool flag;
			try
			{
				Convert.ToDecimal(str);
				flag = true;
			}
			catch (Exception exception)
			{
				flag = false;
			}
			return flag;
		}

		public static bool IsNotNull(this string str)
		{
			return !string.IsNullOrWhiteSpace(str);
		}

		public static bool IsNull(this string str)
		{
			return string.IsNullOrWhiteSpace(str);
		}

		public static bool IsNumeric(this string str)
		{
			bool flag;
			try
			{
				if (str != null)
				{
					Convert.ToInt64(str);
					flag = true;
				}
				else
				{
					flag = false;
				}
			}
			catch (Exception exception)
			{
				flag = false;
			}
			return flag;
		}

		public static string Kisalt(this string pStr, int pLenght)
		{
			string str;
			if (pStr == null)
			{
				str = null;
			}
			else if (pLenght > 0)
			{
				str = (pStr.Length > pLenght ? pStr.Substring(0, pLenght) : pStr);
			}
			else
			{
				str = "";
			}
			return str;
		}

		public static byte[] ToByteArray(this string str)
		{
			byte[] numArray;
			numArray = (!str.IsNull() ? (new UTF8Encoding()).GetBytes(str) : new byte[0]);
			return numArray;
		}

		public static string ToCamelCase(this string phrase)
		{
			string str;
			if (phrase != null)
			{
				StringBuilder stringBuilder = new StringBuilder(phrase.Length);
				bool flag = true;
				string str1 = phrase;
				for (int i = 0; i < str1.Length; i++)
				{
					char chr = str1[i];
					if ((char.IsWhiteSpace(chr) || char.IsPunctuation(chr) ? false : !char.IsSeparator(chr)))
					{
						if (!flag)
						{
							stringBuilder.Append(char.ToLower(chr));
						}
						else
						{
							stringBuilder.Append(char.ToUpper(chr));
						}
						flag = false;
					}
					else
					{
						flag = true;
					}
				}
				str = stringBuilder.ToString();
			}
			else
			{
				str = string.Empty;
			}
			return str;
		}

		public static string ToEngslihCharacters(this string s)
		{
			Encoding encoding = Encoding.GetEncoding("Cyrillic");
			Encoding uTF8 = Encoding.UTF8;
			byte[] bytes = uTF8.GetBytes(s);
			byte[] numArray = Encoding.Convert(uTF8, encoding, bytes);
			return encoding.GetString(numArray);
		}

		private static string TurkishToEnglish(string original)
		{
			int i;
			string str = "";
			for (i = 0; i <= original.Length - 1; i++)
			{
				char chr = original[i];
				if (chr > 'â')
				{
					if (chr <= 'ö')
					{
						if (chr == 'ç')
						{
							str = string.Concat(str, "c");
						}
						else
						{
							if (chr == 'î')
							{
								goto Label0;
							}
							if (chr != 'ö')
							{
								goto Label1;
							}
							str = string.Concat(str, "o");
						}
					}
					else if (chr > 'ğ')
					{
						switch (chr)
						{
							case 'İ':
							{
								str = string.Concat(str, "I");
								break;
							}
							case 'ı':
							{
								goto Label0;
							}
							default:
							{
								switch (chr)
								{
									case 'Ş':
									{
										str = string.Concat(str, "S");
										break;
									}
									case 'ş':
									{
										str = string.Concat(str, "s");
										break;
									}
									default:
									{
										goto Label1;
									}
								}
								break;
							}
						}
					}
					else
					{
						switch (chr)
						{
							case 'û':
							case 'ü':
							{
								str = string.Concat(str, "u");
								break;
							}
							default:
							{
								switch (chr)
								{
									case 'Ğ':
									{
										str = string.Concat(str, "G");
										break;
									}
									case 'ğ':
									{
										str = string.Concat(str, "g");
										break;
									}
									default:
									{
										goto Label1;
									}
								}
								break;
							}
						}
					}
				}
				else if (chr <= 'I')
				{
					switch (chr)
					{
						case '(':
						case ')':
						{
							str = str ?? "";
							break;
						}
						default:
						{
							if (chr == '/')
							{
								str = string.Concat(str, " ");
								break;
							}
							else
							{
								if (chr == 'I')
								{
									goto Label0;
								}
								goto Label1;
							}
						}
					}
				}
				else if (chr <= 'Ö')
				{
					if (chr == 'Ç')
					{
						str = string.Concat(str, "C");
					}
					else
					{
						if (chr != 'Ö')
						{
							goto Label1;
						}
						str = string.Concat(str, "O");
					}
				}
				else if (chr == 'Ü')
				{
					str = string.Concat(str, "U");
				}
				else
				{
					if (chr != 'â')
					{
						goto Label1;
					}
					str = string.Concat(str, "a");
				}
            }
            Label5:
            return str.ToCamelCase();
		Label0:
			str = string.Concat(str, "i");
			goto Label5;
		Label1:
			str = string.Concat(str, original[i]);
			goto Label5;
		}
	}
}