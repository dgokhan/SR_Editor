using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DevExpress.XtraBars;
using System.Deployment.Application;
using System.IO;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using System.Media;
using BansheeGz.BGDatabase;
using DevExpress.XtraBars.Alerter;
using DevExpress.XtraTab;
using SR_Editor.Core;
using Application = System.Windows.Forms.Application;
using Color = System.Drawing.Color;
using SR_Editor.Utility;

namespace SR_Editor.Forms
{
    public partial class FormMainRibbon : DevExpress.XtraBars.Ribbon.RibbonForm
    {

        private Thread threadPeriyodik = null;


        public FormMainRibbon()
        {
            this.InitializeComponent();


            IPAddress[] hostAddresses = Dns.GetHostAddresses(Dns.GetHostName());
            string str = "";
            for (int i = 0; i < (int)hostAddresses.Length; i++)
            {
                if (hostAddresses[i].AddressFamily == AddressFamily.InterNetwork)
                {
                    str = hostAddresses[i].ToString();
                }
            }


            var asset = "bansheegz_database.bytes";
            var file = File.ReadAllBytes(Path.Combine(Environment.CurrentDirectory, asset));
            BGRepo.I.Load(file);
            //
            //asset = Path.Combine(Environment.CurrentDirectory, $"bansheegz_database_locale_tr.bytes");
            BGRepo.I.Addons.Get<BGAddonLocalization>().Repo.RepoLoader = new CustomLocalizationLoader();

            BGRepo.I.Addons.Get<BGAddonLocalization>().CurrentLocale = "tr";

            this.barStaticItemVersion.Caption = (UtilVersion.Version);
            this.barStaticItemClient.Caption = (string.Concat(str, " ", Environment.MachineName));
            this.barStaticItemGirisZamani.Caption = (DateTime.Now.ToString("dd.MM.yyyy HH:mm"));
            this.barStaticItemKullanici.Caption = string.Format("{0} ({1} {2})", EditorApplication.EditorApplication.Configuration.CurrentUser.UserName, EditorApplication.EditorApplication.Configuration.CurrentUser.Name, EditorApplication.EditorApplication.Configuration.CurrentUser.SurName);
            //this.barStaticItemHastane.Caption = Session.GetKullanici.SubeAdi;


            var asd = EditorApplication.EditorApplication.Configuration.Auth.GrantedPolicies;

            EditorApplication.EditorApplication.defaultLookAndFeelMain = this.defaultLookAndFeel1;
            EditorApplication.EditorApplication.InitMenu(this.applicationMenu);
            EditorApplication.EditorApplication.InitRibbonMenu(this.ribbon);
            UtilConfig.MainForm = this;

            //EditorApplication.EditorApplication.Module.Sistem.Ortak.Giris.Show(null);

            /*
            if ((!Kullanici.AktifKullanici.EkPersonelTipiId.HasValue ? false : Kullanici.AktifKullanici.EkPersonelTipiId.Value == 11))
            {
                UtilParameters utilParameter = new UtilParameters();
                utilParameter.Add("IsAcilisEkrani", true);
                if (EditorApplication.Module.HastaKabul.Vezne.VezneDonem.IsDolu())
                {
                    EditorApplication.Module.HastaKabul.Vezne.VezneDonem.Show(utilParameter);
                }
            }
            if (EditorApplication.Module.Sistem.Versiyon.Giris.IsDolu())
            {
                EditorApplication.Module.Sistem.Versiyon.Giris.Show(null);
            }
            */
            this.VersiyonKontrol();
            this.ShowFormKullaniciAcilisFormu();


            this.Text = "Royale Support";

            this.PageShowMode();


            //if (debug)
            {
                Program.ShowDebugWindow();
            }
        }

        private void barButtonItemBaglanti_ItemClick(object sender, ItemClickEventArgs e)
        {
            //(new FormConnectionInfo()).ShowDialog();
        }

        private void barButtonItemEkraniKilitle_ItemClick(object sender, ItemClickEventArgs e)
        {
        }

        private void barButtonItemHatirlatici_ItemClick(object sender, ItemClickEventArgs e)
        {
            UtilParameters utilParameter = new UtilParameters();
            utilParameter.Add("IsHatirlatici", true);
            //EditorApplication.Module.Ortak.Mesaj.CanliMesajGonder.Show(utilParameter);
        }

        private void barButtonItemKullaniciBarKodu(object sender, ItemClickEventArgs e)
        {
            //EditorApplication.Module.IK.Rapor.KullaniciBarkodu.Show(new UtilParameters());
        }

        private void barButtonItemProgramdanCikis_ItemClick(object sender, ItemClickEventArgs e)
        {
            base.Close();
        }

        private void barButtonItemSifreDegistir_ItemClick(object sender, ItemClickEventArgs e)
        {

        }

        private void barButtonItemTekrarBaslat_ItemClick(object sender, ItemClickEventArgs e)
        {
            this.YenidenBaslat();
        }

        private void barStaticItemVersion_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (this.barStaticItemVersion.Appearance.ForeColor == Color.Red)
            {
                this.ShowVersiyonForm(false);
            }
        }


        private void CheckForUpdateCompletedEventHandler(object sender, CheckForUpdateCompletedEventArgs e)
        {
            /*try
            {
                if (e.UpdateAvailable)
                {
                    Version availableVersion = e.AvailableVersion;
                    Version currentVersion = ApplicationDeployment.CurrentDeployment.CurrentVersion;
                    Version versiyon = this.GetVersiyon(EditorApplication.Parametre.Sistem.Version.AktifMinumumVersiyon);
                    Version version = this.GetVersiyon(EditorApplication.Parametre.Sistem.Version.AktifMaksimumVersiyon);
                    if (availableVersion.ToString() != currentVersion.ToString())
                    {
                        this.barStaticItemVersion.get_Appearance().set_ForeColor(Color.Red);
                        this.barStaticItemVersion.get_Appearance().set_Font(new Font("Tahoma", 7.8f, FontStyle.Underline, GraphicsUnit.Point, 162));
                        if (!EditorApplication.Parametre.Sistem.Version.AktifVersiyonKurulumuMecburiOlsun)
                        {
                            if ((!EditorApplication.Parametre.Sistem.Version.FarkliVersiyonKontroluYapilsin ? false : availableVersion != currentVersion))
                            {
                                this.ShowVersiyonForm(false);
                            }
                        }
                        else if (!(!(versiyon != null) || !(availableVersion >= versiyon) ? true : !(currentVersion < versiyon)))
                        {
                            this.ShowVersiyonForm(true);
                        }
                        else if ((!(version != null) || !(availableVersion <= version) ? false : currentVersion > version))
                        {
                            this.ShowVersiyonForm(true);
                        }
                    }
                }
            }
            catch
            {
            }*/
        }

        private void FormMainRibbon_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                //if (!(Kullanici.AktifKullanici.IsBos() || !Kullanici.AktifKullanici.IsVezne ? false : !Kullanici.AktifKullanici.EkVezneDonemId.IsBos()))
                {
                    if (UtilMessage.Show("Uygulamadan çıkmak istediğinize emin misiniz?", "Sistem Çıkış", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                    {
                        e.Cancel = true;
                    }
                }
                /* else if (UtilMessage.Show(EnumUtilMessage.KasaTeslimVezneDonemKontrol, null, "Kasa teslimi yapacaksaniz vezne döneminizi kapatınız!.\r\n Uygulamadan çıkmak istediğinize emin misiniz?", "Sistem Çıkış", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                 {
                     e.Cancel = true;
                 }
                 */
                if (!e.Cancel)
                {
                    Environment.Exit(0);
                }
            }
        }

        private void FormMainRibbon_Load(object sender, EventArgs e)
        {
            //EditorApplication.GetClientRegistrySkin();
        }

        private Version GetVersiyon(string pVersiyon)
        {
            Version version;
            if (!string.IsNullOrEmpty(pVersiyon))
            {
                string[] strArrays = pVersiyon.Split(new char[] { '.' });
                if ((int)strArrays.Length == 4)
                {
                    string[] strArrays1 = strArrays;
                    int ınt32 = 0;
                    while (ınt32 < (int)strArrays1.Length)
                    {
                        if (strArrays1[ınt32].IsNumeric())
                        {
                            ınt32++;
                        }
                        else
                        {
                            version = null;
                            return version;
                        }
                    }
                    version = new Version(pVersiyon);
                }
                else
                {
                    version = null;
                }
            }
            else
            {
                version = null;
            }
            return version;
        }


        private void PageShowMode()
        {
            /*if (EditorApplication.Parametre.Sistem.Genel.AcikSekmeleriKapatabilme)
            {
                this.xtraTabbedMdiManager1.set_ClosePageButtonShowMode(4);
            }*/
#if DEPLOY
            this.xtraTabbedMdiManager1.HeaderButtonsShowMode = TabButtonShowMode.Never;
#endif
        }

        /*private void ShowFormCanliMesaj(CanliMesaj pCanliMesaj)
        {
            int value = pCanliMesaj.CanliMesajId.Value;
            int? canliMesajAliciId = pCanliMesaj.CanliMesajAliciId;
            FormCanliMesaj formCanliMesaj = new FormCanliMesaj(value, canliMesajAliciId.Value, pCanliMesaj.MesajBaslik, pCanliMesaj.IsOkunmandanKapanabilir.IsBosIse(false));
            formCanliMesaj.Notify();
            List<CanliMesaj> canliMesajs = this.listeAcilanCanliMesaj;
            CanliMesaj canliMesaj = new CanliMesaj();
            canliMesajAliciId = pCanliMesaj.CanliMesajAliciId;
            canliMesaj.CanliMesajAliciId = new int?(canliMesajAliciId.Value);
            canliMesaj.CanliMesajId = pCanliMesaj.CanliMesajId;
            canliMesaj.MesajBaslik = pCanliMesaj.MesajBaslik;
            canliMesajs.Add(canliMesaj);
        }*/

        private void ShowFormKullaniciAcilisFormu()
        {
            /*bool flag;
            try
            {
                if ((!Kullanici.AktifKullanici.IsDolu() ? false : !string.IsNullOrWhiteSpace(Kullanici.AktifKullanici.AcilisFormu)))
                {
                    ModuleInfo moduleInfo = (ModuleInfo)UtilConfig.ListModuleInfo.Find((ModuleInfoGroup t) => t.FullKey == Kullanici.AktifKullanici.AcilisFormu);
                    if (!moduleInfo.IsDolu() || !moduleInfo.IsMenuVisible || KullaniciForm.List == null)
                    {
                        flag = true;
                    }
                    else
                    {
                        flag = (Kullanici.AktifKullanici.IsAdmin || moduleInfo.IsCommonModule ? false : (
                            from t in KullaniciForm.List
                            where t.EkKodu == moduleInfo.FullKey
                            select t).Count<KullaniciForm>() <= 0);
                    }
                    if (!flag)
                    {
                        moduleInfo.Show(new UtilParameters());
                    }
                }
            }
            catch
            {
            }*/
        }

        private void ShowVersiyonForm(bool pKurukumHemenBaslasin)
        {
            UtilParameters utilParameter = new UtilParameters();
            utilParameter.Add("KurulumHemenBaslasin", pKurukumHemenBaslasin);
            //EditorApplication.Module.Sistem.Versiyon.VersiyonYukleme.Show(utilParameter);
        }



        public delegate void DelegateShowZorunluCikis(string pCaption, string pMesaj);

        public delegate void DelegateZorunluKapat(string pUyari);


        public delegate void DelegateCanliMesajSesCikar(string pUyari);

        private void ShowZorunluCikis(string pCaption, string pMesaj)
        {
            this.alertControlZorunluCikis.Show((System.Windows.Forms.Form)this, pCaption, pMesaj);
        }

        private void ZorunluKapat(string pUyari)
        {
            Environment.Exit(0);
        }

        private void CanliMesajSesCikar(string pUyari)
        {
            System.Media.SystemSounds.Beep.Play();
            //new SoundPlayer(UtilAkdeniz.GetApplicationFolder() + "\\doorbell1.wav").Play();
        }

        private void VersiyonKontrol()
        {
            try
            {
                if (ApplicationDeployment.IsNetworkDeployed)
                {
                    ApplicationDeployment.CurrentDeployment.CheckForUpdateCompleted += new CheckForUpdateCompletedEventHandler(this.CheckForUpdateCompletedEventHandler);
                    ApplicationDeployment.CurrentDeployment.CheckForUpdateAsync();
                }
            }
            catch
            {
            }
        }

        public void AcikFormlariKapat()
        {

            for (int i = this.xtraTabbedMdiManager1.Pages.Count; i > 0; i--)
            {
                this.xtraTabbedMdiManager1.Pages[i - 1].MdiChild.Close();
            }
        }

        private void YenidenBaslat()
        {
            for (int i = this.xtraTabbedMdiManager1.Pages.Count; i > 0; i--)
            {
                this.xtraTabbedMdiManager1.Pages[i - 1].MdiChild.Close();
            }
            Application.Restart();
            Environment.Exit(0);
        }

        private void barStaticItemGirisZamani_ItemClick(object sender, ItemClickEventArgs e)
        {
                    }
    }
}
