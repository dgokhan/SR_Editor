using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Deployment.Application;
using System.Drawing;
using System.IO;
using System.Text;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.Utils.Animation;
using DevExpress.XtraBars;
using DevExpress.XtraBars.Docking2010.Customization;
using DevExpress.XtraBars.Docking2010.Views;
using DevExpress.XtraBars.Docking2010.Views.WindowsUI;
using DevExpress.XtraBars.ToolbarForm;
using DevExpress.XtraEditors;
using DevExpress.XtraSplashScreen;
using SR_Editor.Core;

namespace SR_Editor.Forms
{
    public partial class FormMain : ToolbarForm
    {
        private Thread threadPeriyodik = null;

        private TimeSpan _timeSpanGirisZamani;

        private readonly string programAdi = "ContaSoft XMIES";
        public FormMain()
        {
            InitializeComponent();
            this.Text = programAdi;
            this.barStaticItemGirisZamani.Caption = DateTime.Now.ToString();

            UtilConfig.MainForm = this;

            IPAddress[] hostAddresses = Dns.GetHostAddresses(Dns.GetHostName());
            string ip = "";
            for (int i = 0; i < (int)hostAddresses.Length; i++)
            {
                if (hostAddresses[i].AddressFamily == AddressFamily.InterNetwork)
                {
                    ip = hostAddresses[i].ToString();
                }
            }

            this.barStaticItemVersiyon.Caption = (UtilVersion.ActiveVersion.ToString());
            //this.barStaticItemClient.Caption = (string.Concat(str, " ", Environment.MachineName));
            this.barStaticItemIp.Caption = ip;
            this.barStaticItemBilgisayar.Caption = Environment.MachineName;
            //this.barStaticItemGirisZamani.Caption = (Session.GirisZamani.ToString("dd.MM.yyyy HH:mm:ss"));
            //this.barSubItemKullanici.Caption = (Session.GetUsername);
            //this.barStaticItemKurum.Caption = Session.GetKullanici.SubeAdi;
            //this.barStaticItemBaslik.Caption = $"{Session.GetKullanici.SubeAdi} | {programAdi}";

            //AkdenizApplication.AkdenizApplication.defaultLookAndFeelMain = this.defaultLookAndFeel1;
            //AkdenizApplication.AkdenizApplication.InitMenu(this.applicationMenu);
            //EditorApplication.EditorApplication.InitToolbarMenu(this.toolbarFormControl1);

            this.VersiyonKontrol();
            //this.ShowFormKullaniciAcilisFormu();

            this.Text = $"{programAdi}";

            InitTransitionManager();

        }
        private void InitTransitionManager()
        {
            var fadeTransition1 = new DevExpress.Utils.Animation.FadeTransition();
            var transition1 = new DevExpress.Utils.Animation.Transition();
            transition1.BarWaitingIndicatorProperties.Caption = "";
            transition1.BarWaitingIndicatorProperties.Description = "";
            transition1.EasingMode = DevExpress.Data.Utils.EasingMode.EaseInOut;
            transition1.LineWaitingIndicatorProperties.AnimationElementCount = 5;
            transition1.LineWaitingIndicatorProperties.Caption = "";
            transition1.LineWaitingIndicatorProperties.Description = "";
            transition1.RingWaitingIndicatorProperties.AnimationElementCount = 5;
            transition1.RingWaitingIndicatorProperties.Caption = "";
            transition1.RingWaitingIndicatorProperties.Description = "";
            transition1.ShowWaitingIndicator = DevExpress.Utils.DefaultBoolean.True;
            transition1.TransitionType = fadeTransition1;
            transition1.WaitingAnimatorType = DevExpress.Utils.Animation.WaitingAnimatorType.Line;
            transition1.WaitingIndicatorProperties.Caption = "";
            transition1.WaitingIndicatorProperties.Description = "";
            transition1.Control = MdiClient;
            transitionManager.Transitions.Add(transition1);
        }

        private Control mdiClientCore;
        public Control MdiClient
        {
            get
            {
                if (mdiClientCore == null)
                {
                    mdiClientCore = GetMdiClient();
                }
                return mdiClientCore;
            }
        }
        private Control GetMdiClient()
        {
            foreach (Control item in Controls)
            {
                if (item is MdiClient)
                {
                    return item;
                }
            }
            return null;
        }
        private void transitionManager_CustomTransition(ITransition transition, CustomTransitionEventArgs e)
        {
            e.Regions = new Rectangle[] { GetCustomTransitionArea() };
        }
        private Rectangle GetCustomTransitionArea()
        {
            var mdiClientBounds = MdiClient.Bounds;
            var activeDocument = tabbedView.ActiveDocument;
            if (activeDocument == null)
            {
                return Bounds;
            }
            var initBounds = new Rectangle(0, activeDocument.Control.Bounds.Y, mdiClientBounds.Width, mdiClientBounds.Height);
            return initBounds;
        }

        void frmMain_Load(object sender, System.EventArgs e)
        {
            BeginInvoke(new MethodInvoker(InitDemo));
        }

        void InitDemo()
        {
            //SplashScreenManager.HideImage(0, this);
        }
        int fileIndex = 0;
        void AddNewDocument(string fileName)
        {
        }
        void AddNewDocument(string fileName, Stream content)
        {
            tabbedView.BeginUpdate();
            //ucCodeEditor control = new ucCodeEditor();
            //control.Name = fileName;
            //control.Text = fileName;
            //BaseDocument document = tabbedView.AddDocument(control);
            //document.Footer = Directory.GetCurrentDirectory();
            //control.LoadCode(content);
            tabbedView.EndUpdate();
            //tabbedView.Controller.Activate(document);
        }
        void repositoryItemComboBox1_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
        }
        int projectIndex = 0;
        void iNewItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            AddNewDocument(string.Format("File{0}.cs", ++projectIndex));
        }
        void iAbout_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            BarManager.About();
        }
        void iSolutionExplorer_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
        }
        void iProperties_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
        }
        void iTaskList_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            dockPanel3.Show();
        }
        void iFindResults_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            dockPanel4.Show();
        }
        void iOutput_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            dockPanel5.Show();
        }
        Cursor currentCursor;
        void Refresh(bool isWait)
        {
            if (isWait)
            {
                currentCursor = Cursor.Current;
                Cursor.Current = Cursors.WaitCursor;
            }
            else Cursor.Current = currentCursor;
            this.Refresh();
        }

        private static bool canCloseFunc(DialogResult parameter)
        {
            return parameter != DialogResult.Cancel;
        }
        private void FrmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                if (_hizliCikis)
                {
                    e.Cancel = false;
                    return;
                }

                FlyoutAction action = new FlyoutAction() { Caption = "Confirm", Description = "Close the application?" };
                Predicate<DialogResult> predicate = canCloseFunc;
                FlyoutCommand command1 = new FlyoutCommand() { Text = "Close", Result = DialogResult.Yes };
                FlyoutCommand command2 = new FlyoutCommand() { Text = "Cancel", Result = DialogResult.No };
                action.Commands.Add(command1);
                action.Commands.Add(command2);
                FlyoutProperties properties = new FlyoutProperties();
                properties.ButtonSize = new Size(100, 40);
                properties.Style = FlyoutStyle.MessageBox;
                if (FlyoutDialog.Show(this, action, properties, predicate) == DialogResult.Yes) e.Cancel = false;
                else e.Cancel = true;
            }
        }

        private void DocumentManager_DocumentActivate(object sender, DocumentEventArgs e)
        {
            if (e.Document != null)
                this.barHeaderItemAktifBaslik.Caption = e.Document.Caption;
            else
                this.barHeaderItemAktifBaslik.Caption = "";
        }

        private static bool canLockFunc(DialogResult parameter)
        {
            return parameter != DialogResult.No;
        }

        private bool _hizliCikis = false;

        private void TimerForm_Tick(object sender, EventArgs e)
        {
            _timeSpanGirisZamani += TimeSpan.FromSeconds(1);

            barStaticItemAcikKalmaSuresi.Caption = _timeSpanGirisZamani.ToString();
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

        private void CheckForUpdateCompletedEventHandler(object sender, CheckForUpdateCompletedEventArgs e)
        {
            /*try
            {
                if (e.UpdateAvailable)
                {
                    Version availableVersion = e.AvailableVersion;
                    Version currentVersion = ApplicationDeployment.CurrentDeployment.CurrentVersion;
                    Version versiyon = this.GetVersiyon(AkdenizApplication.Parametre.Sistem.Version.AktifMinumumVersiyon);
                    Version version = this.GetVersiyon(AkdenizApplication.Parametre.Sistem.Version.AktifMaksimumVersiyon);
                    if (availableVersion.ToString() != currentVersion.ToString())
                    {
                        this.barStaticItemVersion.get_Appearance().set_ForeColor(Color.Red);
                        this.barStaticItemVersion.get_Appearance().set_Font(new Font("Tahoma", 7.8f, FontStyle.Underline, GraphicsUnit.Point, 162));
                        if (!AkdenizApplication.Parametre.Sistem.Version.AktifVersiyonKurulumuMecburiOlsun)
                        {
                            if ((!AkdenizApplication.Parametre.Sistem.Version.FarkliVersiyonKontroluYapilsin ? false : availableVersion != currentVersion))
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
        public void AcikFormlariKapat()
        {
            //for (int i = this.xtraTabbedMdiManager1.Pages.Count; i > 0; i--)
            //{
            //    this.xtraTabbedMdiManager1.Pages[i - 1].MdiChild.Close();
            //}
        }

        private void YenidenBaslat()
        {
            //for (int i = this.xtraTabbedMdiManager1.Pages.Count; i > 0; i--)
            //{
            //    this.xtraTabbedMdiManager1.Pages[i - 1].MdiChild.Close();
            //}
            Application.Restart();
            Environment.Exit(0);
        }

        private void BarButtonItemEkraniKilitle_ItemClick(object sender, ItemClickEventArgs e)
        {
        }
    }
}