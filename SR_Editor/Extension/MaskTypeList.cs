using SR_Editor.LookUp.Base;
using System;
using System.Collections.ObjectModel;

namespace SR_Editor.Core
{
	public class MaskTypeList : LookUpEntityBaseList<byte, LookUpEnumMaskType>
	{
		private static MaskTypeList liste;

		public static MaskTypeList Liste
		{
			get
			{
				if (MaskTypeList.liste == null)
				{
					MaskTypeList.liste = new MaskTypeList();
				}
				return MaskTypeList.liste;
			}
		}

		public MaskTypeList()
		{
			this.Load();
		}

		public override void Load()
		{
			base.Add(new LookUpEnumMaskType(1, "Kısa Tarih(dd/mm/yyyy)"));
			base.Add(new LookUpEnumMaskType(12, "Tarih ve Saat(dd/mm/yyyy hh:mm:ss)"));
			base.Add(new LookUpEnumMaskType(3, "Sayı"));
			base.Add(new LookUpEnumMaskType(2, "Telefon"));
			base.Add(new LookUpEnumMaskType(4, "Email"));
			base.Add(new LookUpEnumMaskType(5, "Kimlik No"));
			base.Add(new LookUpEnumMaskType(7, "Küçük Harf"));
			base.Add(new LookUpEnumMaskType(8, "Büyük Harf"));
			base.Add(new LookUpEnumMaskType(11, "Küsüratli Sayı"));
			base.Load();
		}
	}
}