using DevExpress.XtraEditors;
using DevExpress.XtraEditors.DXErrorProvider;
using System;
using System.Windows.Forms;

namespace SR_Editor.Core
{
	public class NotEmptyValidatinRule : ValidationRule
	{
		private BaseEdit conditionControl;

		private BaseEdit notEmptyControl1;

		private BaseEdit notEmptyControl2;

		private object conditionValue;

		private bool? isNullCondition;

		private bool? isZeroAsEmpty;

		public NotEmptyValidatinRule()
		{
		}

		public NotEmptyValidatinRule(bool pIsZeroAsEmpty)
		{
			this.isZeroAsEmpty = new bool?(pIsZeroAsEmpty);
		}

		public NotEmptyValidatinRule(BaseEdit pConditionControl, object pConditionValue)
		{
			this.conditionControl = pConditionControl;
			this.conditionValue = pConditionValue;
		}

		public NotEmptyValidatinRule(BaseEdit pConditionControl, bool pIsNullCondition)
		{
			this.conditionControl = pConditionControl;
			this.isNullCondition = new bool?(pIsNullCondition);
		}

		public NotEmptyValidatinRule(BaseEdit pNotEmptyControl1)
		{
			this.notEmptyControl1 = pNotEmptyControl1;
		}

		public NotEmptyValidatinRule(BaseEdit pNotEmptyControl1, BaseEdit pNotEmptyControl2)
		{
			this.notEmptyControl1 = pNotEmptyControl1;
			this.notEmptyControl2 = pNotEmptyControl2;
		}

		public override bool Validate(Control control, object value)
		{
			bool flag;
			bool flag1;
			bool flag2 = false;
			BaseEdit baseEdit = (BaseEdit)control;
			if ((baseEdit.EditValue == null || baseEdit.EditValue == DBNull.Value ? false : !string.IsNullOrWhiteSpace(baseEdit.EditValue.ToString())))
			{
				flag2 = true;
			}
			if ((!flag2 || !this.isZeroAsEmpty.HasValue || !this.isZeroAsEmpty.Value ? false : baseEdit.EditValue.ToString().Trim() == "0"))
			{
				flag2 = false;
			}
			if (!flag2)
			{
				if (this.conditionControl != null)
				{
					if (!this.isNullCondition.HasValue || !this.isNullCondition.Value)
					{
						flag = true;
					}
					else
					{
						flag = (this.conditionControl.EditValue == null || this.conditionControl.EditValue == DBNull.Value ? true : string.IsNullOrWhiteSpace(this.conditionControl.EditValue.ToString()));
					}
					if (flag)
					{
						if (!this.isNullCondition.HasValue || this.isNullCondition.Value)
						{
							flag1 = true;
						}
						else
						{
							flag1 = (this.conditionControl.EditValue == null || this.conditionControl.EditValue == DBNull.Value ? false : !string.IsNullOrWhiteSpace(this.conditionControl.EditValue.ToString()));
						}
						if (!flag1)
						{
							flag2 = true;
						}
						else if ((this.isNullCondition.HasValue ? false : !object.Equals(this.conditionControl.EditValue, this.conditionValue)))
						{
							flag2 = true;
						}
					}
					else
					{
						flag2 = true;
					}
				}
				else if ((this.notEmptyControl2 != null ? true : this.notEmptyControl1 != null))
				{
					if ((this.notEmptyControl2 == null || this.notEmptyControl2.EditValue == null || this.notEmptyControl2.EditValue == DBNull.Value ? false : !string.IsNullOrWhiteSpace(this.notEmptyControl2.EditValue.ToString())))
					{
						flag2 = true;
					}
					if ((this.notEmptyControl1 == null || this.notEmptyControl1.EditValue == null || this.notEmptyControl1.EditValue == DBNull.Value ? false : !string.IsNullOrWhiteSpace(this.notEmptyControl1.EditValue.ToString())))
					{
						flag2 = true;
					}
				}
			}
			return flag2;
		}
	}
}