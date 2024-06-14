using DevExpress.XtraEditors;
using DevExpress.XtraEditors.DXErrorProvider;
using System;
using System.Windows.Forms;

namespace SR_Editor.Core
{
	public class UtilValidation
	{
		public UtilValidation()
		{
		}

		public static CompareValidatinRule GetCompareCondition(ConditionOperator pConditionOperator, BaseEdit pControl1)
		{
			return new CompareValidatinRule(pConditionOperator, pControl1, null);
		}

		public static CompareValidatinRule GetCompareCondition(ConditionOperator pConditionOperator, BaseEdit pControl1, BaseEdit pControl2)
		{
			return new CompareValidatinRule(pConditionOperator, pControl1, pControl2);
		}

		public static CompareAgainstControlValidationRule GetCompareCondition(CompareControlOperator pConditionOperator, Control pControl)
		{
			CompareAgainstControlValidationRule compareAgainstControlValidationRule = new CompareAgainstControlValidationRule()
			{
				CompareControlOperator = pConditionOperator,
				Control = pControl,
				ErrorText = string.Concat("Lütfen geçerli bir değer giriniz!!", UtilValidation.GetConditionInfo(pConditionOperator.ToString(), pControl.Text, null))
			};
			return compareAgainstControlValidationRule;
		}

		private static string GetConditionInfo(string pConditionOperator, object pValue1, object pValue2)
		{
			string str;
			if (pValue1 != null)
			{
				string str1 = "";
				string str2 = pConditionOperator;
				if (str2 != null)
				{
					switch (str2)
					{
						case "BeginsWith":
						{
							str1 = string.Concat(pValue1.ToString(), " ile başlamalı.");
							break;
						}
						case "Between":
						{
							str1 = string.Concat(pValue1.ToString(), "-", pValue2.ToString(), " arasında olmalı.");
							break;
						}
						case "Contains":
						{
							str1 = string.Concat(pValue1.ToString(), " 'i içermeli.");
							break;
						}
						case "EndsWith":
						{
							str1 = string.Concat(pValue1.ToString(), " ile bitmeli.");
							break;
						}
						case "Equals":
						{
							str1 = string.Concat(pValue1.ToString(), " 'e eşit olmamalı.");
							break;
						}
						case "Greater":
						{
							str1 = string.Concat(pValue1.ToString(), " 'den büyük olmalı.");
							break;
						}
						case "GreaterOrEqual":
						{
							str1 = string.Concat(pValue1.ToString(), " 'den büyük-eşit olmalı.");
							break;
						}
						case "Less":
						{
							str1 = string.Concat(pValue1.ToString(), " 'den küçük olmalı.");
							break;
						}
						case "LessOrEqual":
						{
							str1 = string.Concat(pValue1.ToString(), " 'den küçük-eşit olmalı.");
							break;
						}
						case "Like":
						{
							str1 = string.Concat(pValue1.ToString(), " 'e benzemeli.");
							break;
						}
						case "NotBetween":
						{
							str1 = string.Concat(pValue1.ToString(), "-", pValue2.ToString(), " arasında olmamalı.");
							break;
						}
						case "NotContains":
						{
							str1 = string.Concat(pValue1.ToString(), " 'i içermemeli.");
							break;
						}
						case "NotEquals":
						{
							str1 = string.Concat(pValue1.ToString(), " 'e eşit olmamalı.");
							break;
						}
						case "NotLike":
						{
							str1 = string.Concat(pValue1.ToString(), " 'e benzememeli.");
							break;
						}
					}
				}
				str = string.Concat("(", str1, ")");
			}
			else
			{
				str = "";
			}
			return str;
		}

		public static ConditionValidationRule GetCustomCondition(ConditionOperator pConditionOperator, object pValue1)
		{
			return UtilValidation.GetCustomCondition(pConditionOperator, pValue1, null);
		}

		public static ConditionValidationRule GetCustomCondition(ConditionOperator pConditionOperator, object pValue1, object pValue2)
		{
			ConditionValidationRule conditionValidationRule = new ConditionValidationRule()
			{
				ConditionOperator = pConditionOperator
			};
			if (pValue1 != null)
			{
				conditionValidationRule.Value1 = pValue1;
			}
			if (pValue2 != null)
			{
				conditionValidationRule.Value2 = pValue2;
			}
			conditionValidationRule.ErrorText = string.Concat("Lütfen geçerli bir değer giriniz!!", UtilValidation.GetConditionInfo(pConditionOperator.ToString(), pValue1, pValue2));
			return conditionValidationRule;
		}

		public static NotEmptyValidatinRule GetNotEmptyCondition()
		{
			return new NotEmptyValidatinRule()
			{
				ErrorText = "Lütfen değer giriniz!!"
			};
		}

		public static NotEmptyValidatinRule GetNotEmptyCondition(bool pIsZeroAsEmpty)
		{
			return new NotEmptyValidatinRule(pIsZeroAsEmpty)
			{
				ErrorText = "Lütfen değer giriniz!!"
			};
		}

		public static NotEmptyValidatinRule GetNotEmptyCondition(BaseEdit pConditionControl, object pConditionValue)
		{
			return new NotEmptyValidatinRule(pConditionControl, pConditionValue)
			{
				ErrorText = "Lütfen değer giriniz!!"
			};
		}

		public static NotEmptyValidatinRule GetNotEmptyCondition(BaseEdit pConditionControl, bool pIsNullCondition)
		{
			return new NotEmptyValidatinRule(pConditionControl, pIsNullCondition)
			{
				ErrorText = "Lütfen değer giriniz!!"
			};
		}

		public static NotEmptyValidatinRule GetNotEmptyCondition(BaseEdit pNotEmptyControl1)
		{
			return new NotEmptyValidatinRule(pNotEmptyControl1)
			{
				ErrorText = "Lütfen değer giriniz!!"
			};
		}

		public static NotEmptyValidatinRule GetNotEmptyCondition(BaseEdit pNotEmptyControl1, BaseEdit pNotEmptyControl2)
		{
			return new NotEmptyValidatinRule(pNotEmptyControl1, pNotEmptyControl2)
			{
				ErrorText = "Lütfen değer giriniz!!"
			};
		}

		public static UniqueValueValidatinRule GetUniqueValueCondition(string pTableName, string pColumnName, int pEntityKey)
		{
			return new UniqueValueValidatinRule(pTableName, pColumnName, pEntityKey)
			{
				ErrorText = "Bu değere sahip başka kayıt var.!!"
			};
		}
	}
}