using DevExpress.XtraEditors;
using System;

namespace SR_Editor.Core
{
	public class RelatedLookUpEdit
	{
		public DevExpress.XtraEditors.LookUpEdit LookUpEdit;

		public string TableName;

		public string KeyColumn;

		public string ValueColumn;

		public string Where;

		public string ParentColumnName;

		public bool ShowAllState;

		public RelatedLookUpEdit()
		{
		}
	}
}