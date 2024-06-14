using SR_Editor.LookUp.Base;
using System;

namespace SR_Editor.Core
{
	public class LookUpEnumMaskType : LookUpEntityBase<byte>
	{
		public LookUpEnumMaskType()
		{
		}

		public LookUpEnumMaskType(byte pKey) : base(pKey)
		{
		}

		public LookUpEnumMaskType(byte pKey, string pValue) : base(pKey, pValue)
		{
		}
	}
}