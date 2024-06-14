using System;
using System.ComponentModel;
using System.Threading;

namespace SR_Editor.Core.Utility
{
	public class ObjectValidator
	{
		private IObjectValidatorOwner owner;

		private object obj;

		private string message;

		public string Message
		{
			get
			{
				return this.message;
			}
			set
			{
				this.message = value;
			}
		}

		public virtual object Object
		{
			get
			{
				return this.obj;
			}
			set
			{
				this.obj = value;
				if (this.obj != null)
				{
					this.PrepareHandlers();
				}
			}
		}

		public IObjectValidatorOwner Owner
		{
			get
			{
				return this.owner;
			}
			set
			{
				this.owner = value;
			}
		}

		public ObjectValidator()
		{
		}

		public ObjectValidator(IObjectValidatorOwner owner) : this()
		{
			this.owner = owner;
		}

		public ObjectValidator(IObjectValidatorOwner owner, object obj) : this(owner)
		{
			this.Object = obj;
		}

		public ObjectValidator(IObjectValidatorOwner owner, object obj, string message) : this(owner, obj)
		{
			this.message = message;
		}

		public void ClearError()
		{
			if (this.owner != null)
			{
				this.owner.ClearError(this.obj);
			}
		}

		public virtual bool DoValidate()
		{
			return this.Validate();
		}

		public bool InvokeValidating(CancelEventArgs e)
		{
			bool flag;
			if (this.Validating == null)
			{
				flag = false;
			}
			else
			{
				this.Validating(this.obj, e);
				flag = true;
			}
			return flag;
		}

		public virtual bool OnValidating(CancelEventArgs e)
		{
			if (!this.InvokeValidating(e))
			{
				e.Cancel = !this.Validate();
			}
			return true;
		}

		public virtual void PrepareHandlers()
		{
		}

		public void SetError()
		{
			this.SetError(this.Message);
		}

		public void SetError(string message)
		{
			this.SetError(message, false);
		}

		public void SetError(string message, bool focus)
		{
			if (this.owner != null)
			{
				this.owner.SetError(this.obj, message, focus);
			}
		}

		public virtual bool Validate()
		{
			return true;
		}

		public event CancelEventHandler Validating;
	}
}