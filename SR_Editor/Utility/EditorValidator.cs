using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using System;
using System.Windows.Forms;

namespace SR_Editor.Core.Utility
{
	public class EditorValidator : ControlValidator
	{
		public override System.Windows.Forms.Control Control
		{
			get
			{
				return base.Control;
			}
		}

		public BaseEdit Editor
		{
			get
			{
				return base.Control as BaseEdit;
			}
			set
			{
				base.Control = value;
			}
		}

		public EditorValidator(IObjectValidatorOwner owner) : base(owner)
		{
		}

		public EditorValidator(IObjectValidatorOwner owner, BaseEdit editor) : base(owner, editor)
		{
		}

		public EditorValidator(IObjectValidatorOwner owner, BaseEdit editor, string message) : base(owner, editor, message)
		{
		}

		public override bool DoValidate()
		{
			bool flag;
			bool isModified = this.Editor.IsModified;
			try
			{
				if (!isModified)
				{
					this.Editor.IsModified = true;
				}
				flag = this.Editor.DoValidate();
			}
			finally
			{
				if (!isModified)
				{
					this.Editor.IsModified = isModified;
				}
			}
			return flag;
		}

		private void Editor_InvalidValueException(object sender, InvalidValueExceptionEventArgs e)
		{
			e.ExceptionMode = ExceptionMode.NoAction;
		}

		public override void PrepareHandlers()
		{
			base.PrepareHandlers();
			this.Editor.InvalidValue += new InvalidValueExceptionEventHandler(this.Editor_InvalidValueException);
		}

		public override bool Validate()
		{
			bool flag;
			bool flag1;
			if (!base.IsTextRequired)
			{
				flag1 = true;
			}
			else
			{
				flag1 = (this.Editor.EditValue == null ? false : !string.IsNullOrEmpty(this.Editor.EditValue.ToString()));
			}
			if (flag1)
			{
				base.ClearError();
			}
			else
			{
				string message = base.Message;
				if (string.IsNullOrEmpty(message))
				{
					message = "Değer girilmeli.";
				}
				if (base.Owner.IsValidating)
				{
					base.SetError(message, true);
					flag = false;
					return flag;
				}
				base.SetError(message, false);
			}
			flag = true;
			return flag;
		}
	}
}