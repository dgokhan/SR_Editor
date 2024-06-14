using System;

namespace SR_Editor.Core.Utility
{
	public class ObjectValidatorList : ObjectValidatorList<ObjectValidator>
	{
		public ObjectValidatorList()
		{
		}

		public ObjectValidatorList(IObjectValidatorOwner owner) : base(owner)
		{
		}
	}
}