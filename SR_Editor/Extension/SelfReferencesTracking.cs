using System;
using System.Data;
using System.Data.Entity.Core;
using System.Data.Entity.Core.Objects.DataClasses;

namespace SR_Editor.Core
{
	public class SelfReferencesTracking
	{
		public string EntitySetName;

		public EntityObject NewEntityObject;

		public EntityKey OriginalKeys;

		public SelfReferencesTracking()
		{
		}
	}
}