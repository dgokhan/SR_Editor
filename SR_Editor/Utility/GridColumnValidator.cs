using DevExpress.Utils;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Base;
using System;
using System.Drawing;

namespace SR_Editor.Core.Utility
{
	public class GridColumnValidator : ObjectValidator
	{
		private bool isTextRequired;

		public GridColumn Column
		{
			get
			{
				return base.Object as GridColumn;
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
					this.Column.AppearanceCell.BackColor = Color.LightYellow;
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

		public GridColumnValidator(IObjectValidatorOwner owner) : base(owner)
		{
		}

		public GridColumnValidator(IObjectValidatorOwner owner, GridColumn column) : base(owner, column)
		{
		}

		public GridColumnValidator(IObjectValidatorOwner owner, GridColumn column, string message) : base(owner, column, message)
		{
		}

		public override bool Validate()
		{
			return true;
		}

		public virtual bool Validate(int rowHandle)
		{
			bool flag;
			bool flag1;
			object rowCellValue = this.Column.View.GetRowCellValue(rowHandle, this.Column);
			if (!this.isTextRequired)
			{
				flag1 = true;
			}
			else
			{
				flag1 = (rowCellValue == null ? false : !string.IsNullOrEmpty(rowCellValue.ToString()));
			}
			if (flag1)
			{
				this.Column.View.SetColumnError(this.Column, "");
				flag = true;
			}
			else
			{
				string message = base.Message;
				if (string.IsNullOrEmpty(message))
				{
					message = string.Format("{0} alanına değer girilmeli.", this.Column.Caption);
				}
				this.Column.View.SetColumnError(this.Column, message);
				flag = false;
			}
			return flag;
		}
	}
}