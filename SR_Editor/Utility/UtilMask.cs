using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Mask;
using DevExpress.XtraEditors.Repository;
/*using SR_Editor.Core.EditorApplication;
using SR_Editor.Core.EditorApplication.Parametre;
using SR_Editor.Core.EditorApplication.Parametre.HastaKabul;*/
using System;
using System.Windows.Forms;

namespace SR_Editor.Core
{
	public class UtilMask
	{
		public UtilMask()
		{
		}

		public static void SetMask(TextEdit pTextEdit, string pRegExMask)
		{
			pTextEdit.Properties.Mask.EditMask = pRegExMask;
			pTextEdit.Properties.Mask.MaskType = MaskType.RegEx;
		}

		public static void SetMask(TextEdit pTextEdit, MaskType pMaskTipi, string pMask)
		{
			pTextEdit.Properties.Mask.EditMask = pMask;
			pTextEdit.Properties.Mask.MaskType = pMaskTipi;
		}

		public static void SetMask(TextEdit pTextEdit, EnumMaskTypes pMaskTipi)
		{
			switch (pMaskTipi)
			{
				case EnumMaskTypes.Date:
				{
					pTextEdit.Properties.Mask.EditMask = "(0?[1-9]|[12]\\d|30|31)/(0?[1-9]|10|11|12)/([12]\\d{3})";
					pTextEdit.Properties.Mask.MaskType = MaskType.RegEx;
					pTextEdit.Properties.Mask.UseMaskAsDisplayFormat = true;
					pTextEdit.Properties.Mask.AutoComplete = AutoCompleteType.Strong;
					return;
				}
				case EnumMaskTypes.Phone:
				{
					pTextEdit.Properties.Mask.EditMask = "\\([0-9]\\d\\d\\)\\d\\d\\d-\\d\\d-\\d\\d";
					pTextEdit.Properties.Mask.MaskType = MaskType.RegEx;
					pTextEdit.Properties.Mask.UseMaskAsDisplayFormat = true;
					pTextEdit.Properties.Mask.AutoComplete = AutoCompleteType.Strong;
					return;
				}
				case EnumMaskTypes.Numeric:
				{
					pTextEdit.Properties.Mask.EditMask = "\\d+";
					pTextEdit.Properties.Mask.MaskType = MaskType.RegEx;
					pTextEdit.Properties.Mask.UseMaskAsDisplayFormat = true;
					pTextEdit.Properties.Mask.AutoComplete = AutoCompleteType.Strong;
					return;
				}
				case EnumMaskTypes.Email:
				{
					pTextEdit.Properties.Mask.EditMask = "\\w+([-+.]\\w+)*@\\w+([-.]\\w+)*\\.\\w+([-.]\\w+)*";
					pTextEdit.Properties.Mask.MaskType = MaskType.RegEx;
					pTextEdit.Properties.Mask.UseMaskAsDisplayFormat = true;
					pTextEdit.Properties.Mask.AutoComplete = AutoCompleteType.Strong;
					return;
				}
				case EnumMaskTypes.IdentityNumber:
				{
					if (15/*Editor.Core.EditorApplication.EditorApplication.Parametre.HastaKabul.HastaKayit.KimlikNoKarakterSayisi*/ > 0)
					{
						pTextEdit.Properties.Mask.EditMask = string.Concat("\\d{", 15, "}");
						pTextEdit.Properties.Mask.MaskType = MaskType.RegEx;
						pTextEdit.Properties.Mask.UseMaskAsDisplayFormat = true;
						pTextEdit.Properties.Mask.AutoComplete = AutoCompleteType.Strong;
						return;
					}
					else
					{
						pTextEdit.Properties.Mask.UseMaskAsDisplayFormat = true;
						pTextEdit.Properties.Mask.AutoComplete = AutoCompleteType.Strong;
						return;
					}
				}
				case EnumMaskTypes.PhoneEx:
				{
					pTextEdit.Properties.Mask.UseMaskAsDisplayFormat = true;
					pTextEdit.Properties.Mask.AutoComplete = AutoCompleteType.Strong;
					return;
				}
				case EnumMaskTypes.Alphabetic:
				{
					pTextEdit.Properties.Mask.EditMask = "[a-zçışöüğə ]+";
					pTextEdit.Properties.Mask.MaskType = MaskType.RegEx;
					pTextEdit.Properties.Mask.UseMaskAsDisplayFormat = true;
					pTextEdit.Properties.Mask.AutoComplete = AutoCompleteType.Strong;
					return;
				}
				case EnumMaskTypes.AlphabeticUpperCase:
				{
					pTextEdit.Properties.Mask.EditMask = "[A-ZÇİŞÖÜĞƏ ]+";
					pTextEdit.Properties.Mask.MaskType = MaskType.RegEx;
					pTextEdit.KeyPress += new KeyPressEventHandler(UtilMask.TextEdit_KeyPress);
					pTextEdit.Properties.Mask.UseMaskAsDisplayFormat = true;
					pTextEdit.Properties.Mask.AutoComplete = AutoCompleteType.Strong;
					return;
				}
				case EnumMaskTypes.Percentage:
				{
					pTextEdit.Properties.Mask.EditMask = "p";
					pTextEdit.Properties.Mask.MaskType = MaskType.Numeric;
					pTextEdit.Properties.Mask.UseMaskAsDisplayFormat = true;
					pTextEdit.Properties.Mask.AutoComplete = AutoCompleteType.Strong;
					return;
				}
				case EnumMaskTypes.Currency:
				{
					pTextEdit.Properties.Mask.EditMask = "c";
					pTextEdit.Properties.Mask.MaskType = MaskType.Numeric;
					pTextEdit.Properties.Mask.UseMaskAsDisplayFormat = true;
					pTextEdit.Properties.Mask.AutoComplete = AutoCompleteType.Strong;
					return;
				}
				case EnumMaskTypes.Decimal:
				{
					pTextEdit.Properties.Mask.EditMask = "n2";
					pTextEdit.Properties.Mask.MaskType = MaskType.Numeric;
					pTextEdit.Properties.Mask.UseMaskAsDisplayFormat = true;
					pTextEdit.Properties.Mask.AutoComplete = AutoCompleteType.Strong;
					return;
				}
				default:
				{
					pTextEdit.Properties.Mask.UseMaskAsDisplayFormat = true;
					pTextEdit.Properties.Mask.AutoComplete = AutoCompleteType.Strong;
					return;
				}
			}
		}

		public static void SetMask(BaseEdit pBaseEdit, byte pMaskTipi)
		{
			byte num;
			if (pBaseEdit is DateEdit)
			{
				num = pMaskTipi;
				if (num == 1)
				{
					(pBaseEdit as DateEdit).Properties.Mask.EditMask = "d";
				}
				else if (num == 12)
				{
					(pBaseEdit as DateEdit).Properties.Mask.EditMask = "g";
				}
				(pBaseEdit as DateEdit).Properties.Mask.UseMaskAsDisplayFormat = true;
				(pBaseEdit as DateEdit).Properties.Mask.AutoComplete = AutoCompleteType.Strong;
			}
			else if (pBaseEdit is TextEdit)
			{
				num = pMaskTipi;
				switch (num)
				{
					case 1:
					{
						(pBaseEdit as TextEdit).Properties.Mask.EditMask = "(0?[1-9]|[12]\\d|30|31)/(0?[1-9]|10|11|12)/([12]\\d{3})";
						(pBaseEdit as TextEdit).Properties.Mask.MaskType = MaskType.RegEx;
						goto case 6;
					}
					case 2:
					{
						(pBaseEdit as TextEdit).Properties.Mask.EditMask = "\\([0-9]\\d\\d\\)\\d\\d\\d-\\d\\d-\\d\\d";
						(pBaseEdit as TextEdit).Properties.Mask.MaskType = MaskType.RegEx;
						goto case 6;
					}
					case 3:
					{
						(pBaseEdit as TextEdit).Properties.Mask.EditMask = "\\d+";
						(pBaseEdit as TextEdit).Properties.Mask.MaskType = MaskType.RegEx;
						goto case 6;
					}
					case 4:
					{
						(pBaseEdit as TextEdit).Properties.Mask.EditMask = "\\w+([-+.]\\w+)*@\\w+([-.]\\w+)*\\.\\w+([-.]\\w+)*";
						(pBaseEdit as TextEdit).Properties.Mask.MaskType = MaskType.RegEx;
						goto case 6;
					}
					case 5:
					{
						(pBaseEdit as TextEdit).Properties.Mask.EditMask = "\\d{11}";
						(pBaseEdit as TextEdit).Properties.Mask.MaskType = MaskType.RegEx;
						goto case 6;
					}
					case 6:
					{
						(pBaseEdit as TextEdit).Properties.Mask.UseMaskAsDisplayFormat = true;
						(pBaseEdit as TextEdit).Properties.Mask.AutoComplete = AutoCompleteType.Strong;
						break;
					}
					case 7:
					{
						(pBaseEdit as TextEdit).Properties.Mask.EditMask = "[a-zçışöüğ ]+";
						(pBaseEdit as TextEdit).Properties.Mask.MaskType = MaskType.RegEx;
						goto case 6;
					}
					case 8:
					{
						(pBaseEdit as TextEdit).Properties.Mask.EditMask = "[A-ZÇİŞÖÜĞ ]+";
						(pBaseEdit as TextEdit).Properties.Mask.MaskType = MaskType.RegEx;
						(pBaseEdit as TextEdit).KeyPress += new KeyPressEventHandler(UtilMask.TextEdit_KeyPress);
						goto case 6;
					}
					case 9:
					{
						(pBaseEdit as TextEdit).Properties.Mask.EditMask = "p";
						(pBaseEdit as TextEdit).Properties.Mask.MaskType = MaskType.Numeric;
						goto case 6;
					}
					case 10:
					{
						(pBaseEdit as TextEdit).Properties.Mask.EditMask = "c";
						(pBaseEdit as TextEdit).Properties.Mask.MaskType = MaskType.Numeric;
						goto case 6;
					}
					case 11:
					{
						(pBaseEdit as TextEdit).Properties.Mask.EditMask = "n2";
						(pBaseEdit as TextEdit).Properties.Mask.MaskType = MaskType.Numeric;
						goto case 6;
					}
					case 12:
					{
						(pBaseEdit as TextEdit).Properties.Mask.EditMask = "g";
						(pBaseEdit as TextEdit).Properties.Mask.MaskType = MaskType.DateTime;
						goto case 6;
					}
					default:
					{
						goto case 6;
					}
				}
			}
		}

		private static void TextEdit_KeyPress(object sender, KeyPressEventArgs e)
		{
			TextEdit textEdit = sender as TextEdit;
			if (textEdit.Properties.CharacterCasing != CharacterCasing.Upper)
			{
				if (textEdit.Properties.CharacterCasing == CharacterCasing.Lower)
				{
					if (e.KeyChar == 'I')
					{
						e.KeyChar = 'ı';
					}
					else if (e.KeyChar == 'İ')
					{
						e.KeyChar = 'i';
					}
				}
			}
			else if (e.KeyChar == 'i')
			{
				e.KeyChar = 'İ';
			}
			else if (e.KeyChar == 'ı')
			{
				e.KeyChar = 'I';
			}
		}
	}
}