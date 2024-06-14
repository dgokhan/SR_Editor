using SR_Editor.Core;
using SR_Editor.Core.EF;
using SR_Editor.Core.EF.Core;
using SR_Editor.LookUp;
using System;
using System.ComponentModel;
using System.Runtime.Remoting;
using System.Windows.Forms;

namespace SR_Editor.Core.Controls
{
	public class ReportModuleInfo : ModuleInfo
	{
		public ReportModuleInfo()
		{
		}

		private void AktifReportForm_FormClosed(object sender, FormClosedEventArgs e)
		{
			(sender as System.Windows.Forms.Form).Dispose();
			GC.Collect();
			GC.WaitForPendingFinalizers();
		}
        
		private RaporTasarim GetVarsayilanRaporTasarim(UtilParameters pFormParams)
		{
			int? nullable = null;
			int? nullable1 = null;
			int? nullable2 = null;
			int? nullable3 = null;
			string str = null;
			if (pFormParams.Contains("KurumId"))
			{
				nullable1 = new int?((int)pFormParams["KurumId"]);
			}
			if (pFormParams.Contains("KurumTipiId"))
			{
				nullable2 = new int?((int)pFormParams["KurumTipiId"]);
			}
			if (pFormParams.Contains("ProtokolTipiId"))
			{
				nullable3 = Convert.ToInt32(pFormParams["ProtokolTipiId"]);
			}
			if (pFormParams.Contains("GelisTipiId"))
			{
				str = pFormParams["GelisTipiId"].ToString();
			}
			if (!pFormParams.Contains("RaporTasarimId"))
			{
				RaporTasarimVarsayilan byRaporTasarimKodu = CoreEntities.Instance.RaporTasarimVarsayilanQuery.GetByRaporTasarimKodu(this.FullKey, nullable2, nullable1, str, nullable3);
				if (byRaporTasarimKodu != null)
				{
					nullable = new int?(byRaporTasarimKodu.RaporTasarimId);
				}
			}
			else
			{
				nullable = new int?(Convert.ToInt32(pFormParams["RaporTasarimId"]));
			}
			RaporTasarim byIdKodu = CoreEntities.Instance.RaporTasarimQuery.GetByIdKodu(this.FullKey, nullable);
			if ((!byIdKodu.IsDolu() || !pFormParams.Contains("IsDigerTasarimlarReadOnly") ? false : Convert.ToBoolean(pFormParams["IsDigerTasarimlarReadOnly"])))
			{
				//byIdKodu.EkIsDigerTasarimlarReadOnly = new bool?(true);
			}
			return byIdKodu;
		}
        
		public override void Show(UtilParameters pFormParams)
		{
			bool flag;
			string fullKey = this.FullKey;
			ObjectHandle objectHandle = Activator.CreateInstance(base.AssemblyName, fullKey);
			if (objectHandle == null)
			{
				UtilMessage.Show(EnumUtilMessage.ModulBulunamadi, null, string.Concat(this.FullKey, " isimli modül bulunamadı."), "Modül Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Hand);
			}
			else
			{
				FormReportView formReportView = new FormReportView();
				ControlXtraReportPanel varsayilanRaporTasarim = objectHandle.Unwrap() as ControlXtraReportPanel;
				varsayilanRaporTasarim.Name = this.FullKey;
				varsayilanRaporTasarim.Module = this;
				varsayilanRaporTasarim.RaporTasarim = this.GetVarsayilanRaporTasarim(pFormParams);
				formReportView.ReportInstance = varsayilanRaporTasarim;
				formReportView.Module = this;
				formReportView.Text = base.Caption;
				formReportView.Name = this.FullKey;
				formReportView.FormClosed += new FormClosedEventHandler(this.AktifReportForm_FormClosed);
				if (pFormParams == null)
				{
					varsayilanRaporTasarim.FormParams.Clear();
					varsayilanRaporTasarim.FormParamsChanged();
				}
				else
				{
					varsayilanRaporTasarim.FormParams = pFormParams;
				}
				if (base.IsDialog)
				{
					flag = false;
				}
				else
				{
					flag = (pFormParams == null || !pFormParams.Contains("IsDialogForm") ? true : !Convert.ToBoolean(pFormParams["IsDialogForm"]));
				}
				if (flag)
				{
					formReportView.MdiParent = UtilConfig.MainForm;
					formReportView.Show();
				}
				else
				{
					formReportView.ShowDialog();
				}
			}
		}
	}
}