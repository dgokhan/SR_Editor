using SR_Editor.Core.Utility;
using SR_Editor.LookUp;
using System;
using System.Collections.Generic;

namespace SR_Editor.Core.Exceptions
{
	public class ExceptionIsKurali : ExceptionBase
	{
		public override string Baslik
		{
			get
			{
				return UtilLanguage.GetCeviriStr("İşlem Hatası", true);
			}
		}

		public ExceptionIsKurali(string message) : base(UtilLanguage.GetCeviriStr(message, true))
		{
			base.HataTipi = ExceptionType.Warning;
		}

		public ExceptionIsKurali(EnumUtilMessage pEnumUtilMessage, List<UtilMessageParametre> pUtilMessageParametre, string message) : base(/*DilCeviri.GetAktifMesajCeviri(pEnumUtilMessage, pUtilMessageParametre, "", message).MesajIcerik*/message)
		{
			base.HataTipi = ExceptionType.Warning;
		}
	}
}