using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace SR_Editor.Core.Utility
{
	public class ControlValidator : ObjectValidator
	{
		private bool isTextRequired;

		public virtual System.Windows.Forms.Control Control
		{
			get
			{
				return base.Object as System.Windows.Forms.Control;
			}
			set
			{
				base.Object = value;
			}
		}

		public bool IsTextRequired
		{
			get
			{
				return this.isTextRequired;
			}
			set
			{
				this.isTextRequired = value;
				if (this.isTextRequired)
				{
					this.Control.BackColor = Color.LightYellow;
				}
			}
		}

		public override object Object
		{
			get
			{
				return base.Object;
			}
		}

		public ControlValidator(IObjectValidatorOwner owner) : base(owner)
		{
		}

		public ControlValidator(IObjectValidatorOwner owner, System.Windows.Forms.Control control) : base(owner, control)
		{
		}

		public ControlValidator(IObjectValidatorOwner owner, System.Windows.Forms.Control control, string message) : base(owner, control, message)
		{
		}

		private void Control_Validating(object sender, CancelEventArgs e)
		{
			this.OnValidating(e);
		}

		public override void PrepareHandlers()
		{
			base.PrepareHandlers();
			this.Control.Validating += new CancelEventHandler(this.Control_Validating);
		}

		public override bool Validate()
		{
			bool flag;
			if (this.Control.DataBindings.Count > 0)
			{
				this.Control.DataBindings[0].ReadValue();
			}
			if ((!this.isTextRequired ? true : !string.IsNullOrEmpty(this.Control.Text)))
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