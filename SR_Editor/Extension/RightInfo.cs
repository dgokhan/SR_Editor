using System;

namespace SR_Editor.Core
{
	public class RightInfo : ParameterInfo
	{
		private string tableName;

		private string selectColumn = "Adi";

		private string @where;

		public string SelectColumn
		{
			get
			{
				return this.selectColumn;
			}
			set
			{
				this.selectColumn = value;
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

		public string Where
		{
			get
			{
				return this.@where;
			}
			set
			{
				this.@where = value;
			}
		}

		public RightInfo()
		{
		}
	}
}