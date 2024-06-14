using DevExpress.XtraEditors;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraGrid.Views.Grid;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Windows.Forms;

namespace SR_Editor.Core.Utility
{
	public class ObjectValidatorList<TValidator> : KeyedCollection<object, TValidator>
	where TValidator : ObjectValidator
	{
		private IObjectValidatorOwner owner;

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

		public ObjectValidatorList()
		{
		}

		public ObjectValidatorList(IObjectValidatorOwner owner)
		{
			this.owner = owner;
		}

		public ObjectValidator Add(object obj, string message)
		{
			ObjectValidator objectValidator = this.AddValidator(obj);
			objectValidator.Message = message;
			return objectValidator;
		}

		public ControlValidator Add(Control control)
		{
			return this.AddValidator(control) as ControlValidator;
		}

		public ControlValidator Add(Control control, string message, bool isTextRequired)
		{
			ControlValidator controlValidator = this.Add(control);
			controlValidator.Message = message;
			controlValidator.IsTextRequired = isTextRequired;
			return controlValidator;
		}

		public GridColumnValidator Add(GridColumn column)
		{
			return this.AddValidator(column) as GridColumnValidator;
		}

		public GridColumnValidator Add(GridColumn column, string message, bool isTextRequired)
		{
			GridColumnValidator gridColumnValidator = this.Add(column);
			gridColumnValidator.Message = message;
			gridColumnValidator.IsTextRequired = isTextRequired;
			return gridColumnValidator;
		}

		protected void AddByBinding(Control control, Binding binding)
		{
			if (!base.Contains(control))
			{
				try
				{
					object dataSource = binding.DataSource;
					DataTable item = null;
					if (dataSource is BindingSource)
					{
						dataSource = (dataSource as BindingSource).DataSource;
					}
					if (dataSource is DataSet)
					{
						item = (dataSource as DataSet).Tables[binding.BindingMemberInfo.BindingMember];
					}
					else if (dataSource is DataTable)
					{
						item = dataSource as DataTable;
					}
					else if (dataSource is DataRow)
					{
						item = (dataSource as DataRow).Table;
					}
					if (item != null)
					{
						if (!item.Columns[binding.BindingMemberInfo.BindingField].AllowDBNull)
						{
							this.Add(control, null, true);
						}
					}
				}
				catch
				{
				}
			}
		}

		public void AddByBinding(GridControl control)
		{
			try
			{
				object dataSource = control.DataSource;
				DataTable item = null;
				if (dataSource is BindingSource)
				{
					dataSource = (dataSource as BindingSource).DataSource;
				}
				if (dataSource is DataSet)
				{
					item = (dataSource as DataSet).Tables[control.DataMember];
				}
				else if (dataSource is DataTable)
				{
					item = dataSource as DataTable;
				}
				else if (dataSource is DataRow)
				{
					item = (dataSource as DataRow).Table;
				}
				foreach (BaseView view in control.Views)
				{
					if (!(view is GridView))
					{
						continue;
					}
					GridView gridView = view as GridView;
					if (gridView.OptionsBehavior.Editable)
					{
						GridViewValidator gridViewValidator = this.AddValidator(gridView) as GridViewValidator;
						foreach (GridColumn column in gridView.Columns)
						{
							if ((column.OptionsColumn.ReadOnly || !item.Columns.Contains(column.FieldName) ? true : item.Columns[column.FieldName].AllowDBNull))
							{
								continue;
							}
							gridViewValidator.ColumnValidators.Add(column, null, true);
						}
					}
				}
			}
			catch
			{
			}
		}

		public void AddByBinding(Control control)
		{
			if (control.DataBindings.Count > 0)
			{
				this.AddByBinding(control, control.DataBindings[0]);
			}
			foreach (Control control1 in control.Controls)
			{
				this.AddByBinding(control1);
			}
		}

		public ObjectValidator AddValidator(object obj)
		{
			ObjectValidator item = null;
			if (base.Contains(obj))
			{
				item = base[obj];
			}
			if (item == null)
			{
				if (obj is GridView)
				{
					item = new GridViewValidator(this.Owner, obj as GridView);
				}
				else if (obj is BaseEdit)
				{
					item = new EditorValidator(this.Owner, obj as BaseEdit);
				}
				else if (obj is Control)
				{
					item = new ControlValidator(this.Owner, obj as Control);
				}
				else if (obj is GridColumn)
				{
					item = new GridColumnValidator(this.Owner, obj as GridColumn);
				}
				base.Add((TValidator)(item as TValidator));
			}
			return item;
		}

		protected override object GetKeyForItem(TValidator item)
		{
			return item.Object;
		}

		public bool Validate()
		{
			bool flag = true;
			foreach (TValidator tValidator in this)
			{
				flag = flag & tValidator.DoValidate();
			}
			return flag;
		}
	}
}