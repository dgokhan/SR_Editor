using DevExpress.XtraEditors.DXErrorProvider;
using System;
using System.Windows.Forms;

namespace SR_Editor.Core
{
	public class UniqueValueValidatinRule : ValidationRule
	{
		private int entityKey;

		private string tableName;

		private string columnName;

		public string ColumnName
		{
			get
			{
				return this.columnName;
			}
			set
			{
				this.columnName = value;
			}
		}

		public int EntityKey
		{
			get
			{
				return this.entityKey;
			}
			set
			{
				this.entityKey = value;
			}
		}

		public string TableName
		{
			get
			{
				return this.tableName;
			}
			set
			{
				this.tableName = value;
			}
		}

		public UniqueValueValidatinRule(string pTableName, string pColumnName, int pEntityKey)
		{
			this.EntityKey = pEntityKey;
			this.columnName = pColumnName;
			this.tableName = pTableName;
		}



		public override bool Validate(Control control, object value)
		{
			bool isValidated = false;
			if (!value.IsBos())
			{
				string newValue = value.ToString();
                /*newValue = newValue.Replace("'", "''");
				if (value is string)
				{
                    newValue = string.Concat("'", newValue, "'");
				}*/
               /* var ent = new Silkroad.SilkroadModel();

                isValidated = !ent.IsExist(this.tableName, this.columnName, newValue, this.entityKey);*/
			}
			return isValidated;
		}
	}
}