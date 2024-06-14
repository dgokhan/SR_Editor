using DevExpress.Utils;
using DevExpress.XtraBars;
using DevExpress.XtraLayout.Utils;
using SR_Editor.Core.Controls;
using System;
using System.Runtime.CompilerServices;

namespace SR_Editor.Core
{
	public static class CommonExtension
	{
		public static bool IsArasinda(this DateTime pObject, DateTime pBaslangicTarihi, DateTime pBitisTarihi)
		{
			return ((pObject <= pBaslangicTarihi ? true : !(pObject < pBitisTarihi)) ? false : true);
		}

		public static bool IsBos(this object s)
		{
			return !s.IsDolu();
		}

		public static bool IsBos(this string pString)
		{
			return !pString.IsDolu();
		}

		public static double IsBosIse(this double? pObject, double pBosIseDeger)
		{
			return (pObject.HasValue ? pObject.Value : pBosIseDeger);
		}

		public static decimal IsBosIse(this decimal? pObject, decimal pBosIseDeger)
		{
			return (pObject.HasValue ? pObject.Value : pBosIseDeger);
		}

		public static DateTime IsBosIse(this DateTime? pObject, DateTime pBosIseDeger)
		{
			return (pObject.HasValue ? pObject.Value : pBosIseDeger);
		}

		public static short IsBosIse(this short? pObject, short pBosIseDeger)
		{
			return (pObject.HasValue ? pObject.Value : pBosIseDeger);
		}

		public static byte IsBosIse(this byte? pObject, byte pBosIseDeger)
		{
			return (pObject.HasValue ? pObject.Value : pBosIseDeger);
		}

		public static string IsBosIse(this string pObject, string pBosIseDeger)
		{
			return (!pObject.IsBos() ? pObject : pBosIseDeger);
		}

		public static bool IsBosIse(this bool? pObject, bool pBosIseDeger)
		{
			return (pObject.HasValue ? pObject.Value : pBosIseDeger);
		}

		public static int IsBosIse(this int? pObject, int pBosIseDeger)
		{
			return (pObject.HasValue ? pObject.Value : pBosIseDeger);
		}

		public static object IsBosIse(this object pObject, object pBosIseDeger)
		{
			return ((pObject == null ? false : pObject != DBNull.Value) ? pObject : pBosIseDeger);
		}

		public static bool IsDolu(this object pObject)
		{
			bool flag;
			flag = ((pObject == null ? false : pObject != DBNull.Value) ? pObject.ToString().IsDolu() : false);
			return flag;
		}

		public static bool IsDolu(this string pString)
		{
			return ((pString == null ? false : !string.IsNullOrEmpty(pString.Trim())) ? true : false);
		}

		/*public static bool IsRtfTextDolu(this string pString)
		{
			bool flag;
			try
			{
				if ((pString == null ? false : !string.IsNullOrWhiteSpace(pString)))
				{
					RichTextEditor richTextEditor = new RichTextEditor()
					{
						rtf
					};
					if (!string.IsNullOrWhiteSpace(richTextEditor.Text))
					{
						flag = true;
						return flag;
					}
				}
				else
				{
					flag = false;
					return flag;
				}
			}
			catch
			{
			}
			flag = false;
			return flag;
		}*/

		public static BarItemVisibility ToBarItemVisibility(this bool value)
		{
			return (!value ? BarItemVisibility.Never : BarItemVisibility.Always);
		}

		public static DefaultBoolean ToDefaultBoolean(this bool value)
		{
			return (!value ? DefaultBoolean.False : DefaultBoolean.True);
		}

		public static LayoutVisibility ToLayoutVisibility(this bool value)
		{
			return (!value ? LayoutVisibility.Never : LayoutVisibility.Always);
		}

		public static string ToPascalCase(this string s)
		{
			if (s.IsDolu())
			{
				char[] charArray = s.Trim().ToCharArray();
				s = "";
				bool flag = true;
				for (int i = 0; i < (int)charArray.Length; i++)
				{
					charArray[i] = char.ToLower(charArray[i]);
					if (flag)
					{
						if (charArray[i] != ' ')
						{
							charArray[i] = char.ToUpper(charArray[i]);
							flag = false;
						}
					}
					if (charArray[i] == ' ')
					{
						flag = true;
					}
					s = string.Concat(s, charArray[i]);
				}
			}
			return s;
		}
	}
}