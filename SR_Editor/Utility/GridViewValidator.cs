using DevExpress.XtraEditors.Controls;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraGrid.Views.Grid;
using System;
using System.Collections;
using System.Collections.ObjectModel;

namespace SR_Editor.Core.Utility
{
	public class GridViewValidator : ObjectValidator
	{
		public ObjectValidatorList<GridColumnValidator> ColumnValidators = new ObjectValidatorList<GridColumnValidator>();

		public DevExpress.XtraGrid.Views.Grid.GridView GridView
		{
			get
			{
				return base.Object as DevExpress.XtraGrid.Views.Grid.GridView;
			}
			set
			{
				base.Object = value;
			}
		}

		public override object Object
		{
			get
			{
				return base.Object;
			}
		}

		public GridViewValidator(IObjectValidatorOwner owner) : base(owner)
		{
			this.ColumnValidators.Owner = owner;
		}

		public GridViewValidator(IObjectValidatorOwner owner, DevExpress.XtraGrid.Views.Grid.GridView gridView) : base(owner, gridView)
		{
			this.ColumnValidators.Owner = owner;
		}

		public GridViewValidator(IObjectValidatorOwner owner, DevExpress.XtraGrid.Views.Grid.GridView gridView, string message) : base(owner, gridView, message)
		{
			this.ColumnValidators.Owner = owner;
		}

		private void GridView_CellValueChanged(object sender, CellValueChangedEventArgs e)
		{
			if (this.ColumnValidators.Contains(e.Column))
			{
				this.ColumnValidators[e.Column].Validate(e.RowHandle);
			}
		}

		private void GridView_InvalidRowException(object sender, InvalidRowExceptionEventArgs e)
		{
			e.ExceptionMode = ExceptionMode.NoAction;
		}

		private void GridView_InvalidValueException(object sender, InvalidValueExceptionEventArgs e)
		{
			e.ExceptionMode = ExceptionMode.NoAction;
		}

		private void GridView_ValidateRow(object sender, ValidateRowEventArgs e)
		{
			e.Valid = this.Validate(e.RowHandle);
			if (e.Valid)
			{
				this.GridView.ClearColumnErrors();
			}
			else
			{
				string message = base.Message;
				if (string.IsNullOrEmpty(message))
				{
					message = "Hatalı girilmiş alan veya alanlar mevcut!";
				}
				e.ErrorText = message;
			}
		}

		public override void PrepareHandlers()
		{
			base.PrepareHandlers();
			this.GridView.InvalidValueException += new InvalidValueExceptionEventHandler(this.GridView_InvalidValueException);
			this.GridView.CellValueChanged += new CellValueChangedEventHandler(this.GridView_CellValueChanged);
			this.GridView.ValidateRow += new ValidateRowEventHandler(this.GridView_ValidateRow);
			this.GridView.InvalidRowException += new InvalidRowExceptionEventHandler(this.GridView_InvalidRowException);
		}

		public override bool Validate()
		{
			return base.Validate();
		}

		public virtual bool Validate(int rowHandle)
		{
			bool flag = true;
			foreach (GridColumn column in this.GridView.Columns)
			{
				if (!this.ColumnValidators.Contains(column))
				{
					continue;
				}
				flag = flag & this.ColumnValidators[column].Validate(rowHandle);
			}
			return flag;
		}
	}
}