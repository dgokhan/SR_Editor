using DevExpress.XtraEditors;
using DevExpress.XtraEditors.DXErrorProvider;
using System;
using System.Windows.Forms;

namespace SR_Editor.Core
{
	public class CompareValidatinRule : ValidationRule
	{
		private BaseEdit control1;

		private BaseEdit control2;

		private ConditionOperator conditionOperator;

		public CompareValidatinRule()
		{
		}

		public CompareValidatinRule(ConditionOperator pConditionOperator, BaseEdit pControl1)
		{
			this.conditionOperator = pConditionOperator;
			this.control1 = pControl1;
		}

		public CompareValidatinRule(ConditionOperator pConditionOperator, BaseEdit pControl1, BaseEdit pControl2)
		{
			this.conditionOperator = pConditionOperator;
			this.control1 = pControl1;
			this.control2 = pControl2;
		}

		public override bool Validate(Control control, object value)
		{
			object editValue = null;
			object obj = null;
			if (this.control1 != null)
			{
				editValue = this.control1.EditValue;
			}
			if (this.control2 != null)
			{
				obj = this.control2.EditValue;
			}
			ConditionValidationRule customCondition = UtilValidation.GetCustomCondition(this.conditionOperator, editValue, obj);
			return customCondition.Validate(control, value);
		}
	}
}