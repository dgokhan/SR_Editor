using System;
using System.Diagnostics;

namespace SR_Editor.Core
{
	public class UtilDateTime
	{
		private Stopwatch stopWatch = new Stopwatch();

		private static UtilDateTime instance;

		private DateTime baslangicZamani = DateTime.MinValue;

		public DateTime AktifAyBaslangic
		{
			get
			{
				int year = this.Now.Year;
				DateTime now = this.Now;
				DateTime dateTime = new DateTime(year, now.Month, 1, 0, 0, 0);
				return dateTime;
			}
		}

		public DateTime AktifAyBitis
		{
			get
			{
				int year = this.Now.Year;
				int month = this.Now.Month;
				int num = this.Now.Year;
				DateTime now = this.Now;
				DateTime dateTime = new DateTime(year, month, DateTime.DaysInMonth(num, now.Month), 23, 59, 29);
				return dateTime;
			}
		}

		public DateTime AktifGunBaslangic
		{
			get
			{
				int year = this.Now.Year;
				int month = this.Now.Month;
				DateTime now = this.Now;
				DateTime dateTime = new DateTime(year, month, now.Day, 0, 0, 0);
				return dateTime;
			}
		}

		public DateTime AktifGunBitis
		{
			get
			{
				int year = this.Now.Year;
				int month = this.Now.Month;
				DateTime now = this.Now;
				DateTime dateTime = new DateTime(year, month, now.Day, 23, 59, 29);
				return dateTime;
			}
		}

		public DateTime AktifYilBaslangic
		{
			get
			{
				DateTime now = this.Now;
				DateTime dateTime = new DateTime(now.Year, 1, 1, 0, 0, 0);
				return dateTime;
			}
		}

		public DateTime AktifYilBitis
		{
			get
			{
				DateTime now = this.Now;
				DateTime dateTime = new DateTime(now.Year, 12, 31, 23, 59, 29);
				return dateTime;
			}
		}

		public static UtilDateTime Instance
		{
			get
			{
				if (UtilDateTime.instance == null)
				{
					UtilDateTime.instance = new UtilDateTime();
				}
				else if (UtilDateTime.instance.stopWatch.ElapsedMilliseconds > (long)10800000)
				{
					UtilDateTime.instance = new UtilDateTime();
				}
				return UtilDateTime.instance;
			}
			set
			{
				UtilDateTime.instance = value;
			}
		}

		public DateTime Now
		{
			get
			{
				return this.baslangicZamani.AddMilliseconds((double)this.stopWatch.ElapsedMilliseconds);
			}
		}

		public UtilDateTime()
		{
			DateTime? dateTime = null;
			try
			{
                dateTime = DateTime.Now;//BaseEntityAdapter.GetDateTime();
			}
			catch (Exception exception)
			{
			}
			if (!dateTime.HasValue)
			{
				this.baslangicZamani = DateTime.Now;
			}
			else
			{
				this.baslangicZamani = dateTime.Value;
			}
			this.stopWatch.Restart();
		}

		public DateTime TarihAyBaslangic(DateTime pTarih)
		{
			DateTime dateTime = new DateTime(pTarih.Year, pTarih.Month, 1, 0, 0, 0);
			return dateTime;
		}

		public DateTime TarihAyBitis(DateTime pTarih)
		{
			DateTime dateTime = new DateTime(pTarih.Year, pTarih.Month, DateTime.DaysInMonth(pTarih.Year, pTarih.Month), 23, 59, 29);
			return dateTime;
		}

		public DateTime TarihGunBaslangic(DateTime pTarih)
		{
			DateTime dateTime = new DateTime(pTarih.Year, pTarih.Month, pTarih.Day, 0, 0, 0);
			return dateTime;
		}

		public DateTime TarihGunBitis(DateTime pTarih)
		{
			DateTime dateTime = new DateTime(pTarih.Year, pTarih.Month, pTarih.Day, 23, 59, 29);
			return dateTime;
		}

		public DateTime TarihYilBaslangic(DateTime pTarih)
		{
			DateTime dateTime = new DateTime(pTarih.Year, 1, 1, 0, 0, 0);
			return dateTime;
		}

		public DateTime TarihYilBitis(DateTime pTarih)
		{
			DateTime dateTime = new DateTime(pTarih.Year, 12, 31, 23, 59, 29);
			return dateTime;
		}
	}
}