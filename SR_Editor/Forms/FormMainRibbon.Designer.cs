
namespace SR_Editor.Forms
{
    using DevExpress.LookAndFeel;
    using DevExpress.XtraBars;
    using DevExpress.XtraBars.Alerter;
    using DevExpress.XtraBars.Ribbon;
    using DevExpress.XtraTabbedMdi;
    using System;
    using System.Drawing;
    using System.Threading;
    using System.Windows.Forms;
    partial class FormMainRibbon
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            DevExpress.Utils.Animation.PushTransition pushTransition1 = new DevExpress.Utils.Animation.PushTransition();
            DevExpress.Utils.Animation.Transition transition1 = new DevExpress.Utils.Animation.Transition();
            this.ribbon = new DevExpress.XtraBars.Ribbon.RibbonControl();
            this.applicationMenu = new DevExpress.XtraBars.Ribbon.ApplicationMenu(this.components);
            this.barStaticItemVersion = new DevExpress.XtraBars.BarStaticItem();
            this.barStaticItemServer = new DevExpress.XtraBars.BarStaticItem();
            this.barStaticItemKullanici = new DevExpress.XtraBars.BarStaticItem();
            this.barStaticItemGirisZamani = new DevExpress.XtraBars.BarStaticItem();
            this.barStaticItemClient = new DevExpress.XtraBars.BarStaticItem();
            this.barStaticItemHastane = new DevExpress.XtraBars.BarStaticItem();
            this.barSubItemGenel = new DevExpress.XtraBars.BarSubItem();
            this.barButtonItemSifreDegistir = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItemEkraniKilitle = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItemTekrarBaslat = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItemProgramdanCikis = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItemHatirlatici = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItemKullaniciBarkodu = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItemVersiyonYukle = new DevExpress.XtraBars.BarButtonItem();
            this.barStaticItemVersiyonBaslik = new DevExpress.XtraBars.BarStaticItem();
            this.barStaticItemHastaneBaslik = new DevExpress.XtraBars.BarStaticItem();
            this.barStaticItemKullaniciBaslik = new DevExpress.XtraBars.BarStaticItem();
            this.barStaticItemGirisZamaniBaslik = new DevExpress.XtraBars.BarStaticItem();
            this.barStaticItemIPBaslik = new DevExpress.XtraBars.BarStaticItem();
            this.barButtonItem1 = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonGroup1 = new DevExpress.XtraBars.BarButtonGroup();
            this.barButtonItem2 = new DevExpress.XtraBars.BarButtonItem();
            this.popupMenu1 = new DevExpress.XtraBars.PopupMenu(this.components);
            this.barButtonItem3 = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItem4 = new DevExpress.XtraBars.BarButtonItem();
            this.popupMenu2 = new DevExpress.XtraBars.PopupMenu(this.components);
            this.barButtonItem5 = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItem6 = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItem7 = new DevExpress.XtraBars.BarButtonItem();
            this.ribbonPageCategory1 = new DevExpress.XtraBars.Ribbon.RibbonPageCategory();
            this.ribbonPageCategory2 = new DevExpress.XtraBars.Ribbon.RibbonPageCategory();
            this.ribbonPageCategory3 = new DevExpress.XtraBars.Ribbon.RibbonPageCategory();
            this.ribbonPageCategory4 = new DevExpress.XtraBars.Ribbon.RibbonPageCategory();
            this.ribbonStatusBar = new DevExpress.XtraBars.Ribbon.RibbonStatusBar();
            this.xtraTabbedMdiManager1 = new DevExpress.XtraTabbedMdi.XtraTabbedMdiManager(this.components);
            this.defaultLookAndFeel1 = new DevExpress.LookAndFeel.DefaultLookAndFeel(this.components);
            this.alertControlZorunluCikis = new DevExpress.XtraBars.Alerter.AlertControl(this.components);
            this.workspaceManager1 = new DevExpress.Utils.WorkspaceManager(this.components);
            this.transitionManager1 = new DevExpress.Utils.Animation.TransitionManager(this.components);
            this.accordionControl1 = new DevExpress.XtraBars.Navigation.AccordionControl();
            this.accordionControlElement1 = new DevExpress.XtraBars.Navigation.AccordionControlElement();
            this.accordionControlElement2 = new DevExpress.XtraBars.Navigation.AccordionControlElement();
            this.accordionControlElement3 = new DevExpress.XtraBars.Navigation.AccordionControlElement();
            this.alertControl = new DevExpress.XtraBars.Alerter.AlertControl(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.ribbon)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.applicationMenu)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.popupMenu1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.popupMenu2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xtraTabbedMdiManager1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.accordionControl1)).BeginInit();
            this.SuspendLayout();
            // 
            // ribbon
            // 
            this.ribbon.AllowMinimizeRibbon = false;
            this.ribbon.ApplicationButtonDropDownControl = this.applicationMenu;
            this.ribbon.ApplicationButtonText = null;
            this.ribbon.ExpandCollapseItem.Id = 0;
            this.ribbon.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
            this.ribbon.ExpandCollapseItem,
            this.ribbon.SearchEditItem,
            this.barStaticItemVersion,
            this.barStaticItemServer,
            this.barStaticItemKullanici,
            this.barStaticItemGirisZamani,
            this.barStaticItemClient,
            this.barStaticItemHastane,
            this.barSubItemGenel,
            this.barButtonItemSifreDegistir,
            this.barButtonItemEkraniKilitle,
            this.barButtonItemTekrarBaslat,
            this.barButtonItemProgramdanCikis,
            this.barButtonItemKullaniciBarkodu,
            this.barButtonItemVersiyonYukle,
            this.barStaticItemVersiyonBaslik,
            this.barStaticItemHastaneBaslik,
            this.barStaticItemKullaniciBaslik,
            this.barStaticItemGirisZamaniBaslik,
            this.barStaticItemIPBaslik,
            this.barButtonItemHatirlatici,
            this.barButtonItem1,
            this.barButtonGroup1,
            this.barButtonItem2,
            this.barButtonItem3,
            this.barButtonItem4,
            this.barButtonItem5,
            this.barButtonItem6,
            this.barButtonItem7});
            this.ribbon.Location = new System.Drawing.Point(0, 0);
            this.ribbon.MaxItemId = 8;
            this.ribbon.Name = "ribbon";
            this.ribbon.PageCategories.AddRange(new DevExpress.XtraBars.Ribbon.RibbonPageCategory[] {
            this.ribbonPageCategory1,
            this.ribbonPageCategory2,
            this.ribbonPageCategory3,
            this.ribbonPageCategory4});
            this.ribbon.RibbonStyle = DevExpress.XtraBars.Ribbon.RibbonControlStyle.OfficeUniversal;
            this.ribbon.ShowApplicationButton = DevExpress.Utils.DefaultBoolean.False;
            this.ribbon.ShowExpandCollapseButton = DevExpress.Utils.DefaultBoolean.False;
            this.ribbon.ShowItemCaptionsInCaptionBar = true;
            this.ribbon.ShowItemCaptionsInPageHeader = true;
            this.ribbon.ShowItemCaptionsInQAT = true;
            this.ribbon.ShowQatLocationSelector = false;
            this.ribbon.ShowToolbarCustomizeItem = false;
            this.ribbon.Size = new System.Drawing.Size(1507, 62);
            this.ribbon.StatusBar = this.ribbonStatusBar;
            this.ribbon.Toolbar.ShowCustomizeItem = false;
            this.ribbon.ToolbarLocation = DevExpress.XtraBars.Ribbon.RibbonQuickAccessToolbarLocation.Below;
            // 
            // applicationMenu
            // 
            this.applicationMenu.Name = "applicationMenu";
            this.applicationMenu.Ribbon = this.ribbon;
            // 
            // barStaticItemVersion
            // 
            this.barStaticItemVersion.Alignment = DevExpress.XtraBars.BarItemLinkAlignment.Right;
            this.barStaticItemVersion.Caption = "1.0.0.1";
            this.barStaticItemVersion.Id = 4;
            this.barStaticItemVersion.ItemAppearance.Normal.Font = new System.Drawing.Font("Tahoma", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.barStaticItemVersion.ItemAppearance.Normal.Options.UseFont = true;
            this.barStaticItemVersion.Name = "barStaticItemVersion";
            // 
            // barStaticItemServer
            // 
            this.barStaticItemServer.Caption = "10.0.99.20\\CAML";
            this.barStaticItemServer.Id = 6;
            this.barStaticItemServer.Name = "barStaticItemServer";
            // 
            // barStaticItemKullanici
            // 
            this.barStaticItemKullanici.Caption = "a.a.";
            this.barStaticItemKullanici.Id = 8;
            this.barStaticItemKullanici.Name = "barStaticItemKullanici";
            this.barStaticItemKullanici.TextAlignment = System.Drawing.StringAlignment.Far;
            // 
            // barStaticItemGirisZamani
            // 
            this.barStaticItemGirisZamani.Caption = "GirisZamani";
            this.barStaticItemGirisZamani.Id = 10;
            this.barStaticItemGirisZamani.Name = "barStaticItemGirisZamani";
            this.barStaticItemGirisZamani.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barStaticItemGirisZamani_ItemClick);
            // 
            // barStaticItemClient
            // 
            this.barStaticItemClient.Alignment = DevExpress.XtraBars.BarItemLinkAlignment.Right;
            this.barStaticItemClient.Caption = "1.1.1.1";
            this.barStaticItemClient.Id = 11;
            this.barStaticItemClient.Name = "barStaticItemClient";
            // 
            // barStaticItemHastane
            // 
            this.barStaticItemHastane.Caption = "hastane";
            this.barStaticItemHastane.Id = 13;
            this.barStaticItemHastane.Name = "barStaticItemHastane";
            // 
            // barSubItemGenel
            // 
            this.barSubItemGenel.Alignment = DevExpress.XtraBars.BarItemLinkAlignment.Right;
            this.barSubItemGenel.Caption = "Genel İşlemler";
            this.barSubItemGenel.Id = 14;
            this.barSubItemGenel.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.barButtonItemSifreDegistir),
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.None, false, this.barButtonItemEkraniKilitle, false),
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.None, false, this.barButtonItemTekrarBaslat, false),
            new DevExpress.XtraBars.LinkPersistInfo(this.barButtonItemProgramdanCikis),
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.None, false, this.barButtonItemHatirlatici, false)});
            this.barSubItemGenel.Name = "barSubItemGenel";
            this.barSubItemGenel.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
            // 
            // barButtonItemSifreDegistir
            // 
            this.barButtonItemSifreDegistir.Caption = "Şifre Değiştir";
            this.barButtonItemSifreDegistir.Id = 15;
            this.barButtonItemSifreDegistir.Name = "barButtonItemSifreDegistir";
            this.barButtonItemSifreDegistir.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItemSifreDegistir_ItemClick);
            // 
            // barButtonItemEkraniKilitle
            // 
            this.barButtonItemEkraniKilitle.Caption = "Ekranı Kilitle";
            this.barButtonItemEkraniKilitle.Id = 16;
            this.barButtonItemEkraniKilitle.Name = "barButtonItemEkraniKilitle";
            // 
            // barButtonItemTekrarBaslat
            // 
            this.barButtonItemTekrarBaslat.Caption = "Yeniden Başlat";
            this.barButtonItemTekrarBaslat.Id = 17;
            this.barButtonItemTekrarBaslat.Name = "barButtonItemTekrarBaslat";
            // 
            // barButtonItemProgramdanCikis
            // 
            this.barButtonItemProgramdanCikis.Caption = "Programdan Çıkış";
            this.barButtonItemProgramdanCikis.Id = 18;
            this.barButtonItemProgramdanCikis.Name = "barButtonItemProgramdanCikis";
            // 
            // barButtonItemHatirlatici
            // 
            this.barButtonItemHatirlatici.Caption = "Hatırlatıcı";
            this.barButtonItemHatirlatici.Id = 26;
            this.barButtonItemHatirlatici.Name = "barButtonItemHatirlatici";
            // 
            // barButtonItemKullaniciBarkodu
            // 
            this.barButtonItemKullaniciBarkodu.Caption = "Kullanıcı Barkodu";
            this.barButtonItemKullaniciBarkodu.Id = 19;
            this.barButtonItemKullaniciBarkodu.Name = "barButtonItemKullaniciBarkodu";
            // 
            // barButtonItemVersiyonYukle
            // 
            this.barButtonItemVersiyonYukle.Caption = "Versiyon Yükle";
            this.barButtonItemVersiyonYukle.Id = 20;
            this.barButtonItemVersiyonYukle.Name = "barButtonItemVersiyonYukle";
            // 
            // barStaticItemVersiyonBaslik
            // 
            this.barStaticItemVersiyonBaslik.Alignment = DevExpress.XtraBars.BarItemLinkAlignment.Right;
            this.barStaticItemVersiyonBaslik.Caption = "Versiyon :";
            this.barStaticItemVersiyonBaslik.Id = 21;
            this.barStaticItemVersiyonBaslik.Name = "barStaticItemVersiyonBaslik";
            // 
            // barStaticItemHastaneBaslik
            // 
            this.barStaticItemHastaneBaslik.Caption = "Kurum :";
            this.barStaticItemHastaneBaslik.Id = 22;
            this.barStaticItemHastaneBaslik.Name = "barStaticItemHastaneBaslik";
            // 
            // barStaticItemKullaniciBaslik
            // 
            this.barStaticItemKullaniciBaslik.Caption = "Kullanıcı :";
            this.barStaticItemKullaniciBaslik.Id = 23;
            this.barStaticItemKullaniciBaslik.Name = "barStaticItemKullaniciBaslik";
            // 
            // barStaticItemGirisZamaniBaslik
            // 
            this.barStaticItemGirisZamaniBaslik.Caption = "Giriş Zamanı :";
            this.barStaticItemGirisZamaniBaslik.Id = 24;
            this.barStaticItemGirisZamaniBaslik.Name = "barStaticItemGirisZamaniBaslik";
            // 
            // barStaticItemIPBaslik
            // 
            this.barStaticItemIPBaslik.Alignment = DevExpress.XtraBars.BarItemLinkAlignment.Right;
            this.barStaticItemIPBaslik.Caption = "Ip :";
            this.barStaticItemIPBaslik.Id = 25;
            this.barStaticItemIPBaslik.Name = "barStaticItemIPBaslik";
            // 
            // barButtonItem1
            // 
            this.barButtonItem1.Caption = "barButtonItem1";
            this.barButtonItem1.Id = 27;
            this.barButtonItem1.Name = "barButtonItem1";
            // 
            // barButtonGroup1
            // 
            this.barButtonGroup1.Caption = "barButtonGroup1";
            this.barButtonGroup1.Id = 28;
            this.barButtonGroup1.Name = "barButtonGroup1";
            // 
            // barButtonItem2
            // 
            this.barButtonItem2.ActAsDropDown = true;
            this.barButtonItem2.ButtonStyle = DevExpress.XtraBars.BarButtonStyle.DropDown;
            this.barButtonItem2.Caption = "barButtonItem2";
            this.barButtonItem2.DropDownControl = this.popupMenu1;
            this.barButtonItem2.Id = 29;
            this.barButtonItem2.Name = "barButtonItem2";
            // 
            // popupMenu1
            // 
            this.popupMenu1.Name = "popupMenu1";
            this.popupMenu1.Ribbon = this.ribbon;
            // 
            // barButtonItem3
            // 
            this.barButtonItem3.Caption = "barButtonItem3";
            this.barButtonItem3.Id = 30;
            this.barButtonItem3.Name = "barButtonItem3";
            // 
            // barButtonItem4
            // 
            this.barButtonItem4.ActAsDropDown = true;
            this.barButtonItem4.ButtonStyle = DevExpress.XtraBars.BarButtonStyle.DropDown;
            this.barButtonItem4.Caption = "barButtonItem4";
            this.barButtonItem4.DropDownControl = this.popupMenu2;
            this.barButtonItem4.Id = 31;
            this.barButtonItem4.Name = "barButtonItem4";
            // 
            // popupMenu2
            // 
            this.popupMenu2.Name = "popupMenu2";
            this.popupMenu2.Ribbon = this.ribbon;
            // 
            // barButtonItem5
            // 
            this.barButtonItem5.Caption = "barButtonItem5";
            this.barButtonItem5.Id = 32;
            this.barButtonItem5.Name = "barButtonItem5";
            // 
            // barButtonItem6
            // 
            this.barButtonItem6.Caption = "barButtonItem6 sdfsdfsdf";
            this.barButtonItem6.Id = 33;
            this.barButtonItem6.Name = "barButtonItem6";
            // 
            // barButtonItem7
            // 
            this.barButtonItem7.Caption = "barButtonItem7";
            this.barButtonItem7.Id = 34;
            this.barButtonItem7.Name = "barButtonItem7";
            // 
            // ribbonPageCategory1
            // 
            this.ribbonPageCategory1.Appearance.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.ribbonPageCategory1.Appearance.Options.UseBackColor = true;
            this.ribbonPageCategory1.Name = "ribbonPageCategory1";
            this.ribbonPageCategory1.Text = "ribbonPageCategory1";
            // 
            // ribbonPageCategory2
            // 
            this.ribbonPageCategory2.Name = "ribbonPageCategory2";
            this.ribbonPageCategory2.Text = "ribbonPageCategory2";
            // 
            // ribbonPageCategory3
            // 
            this.ribbonPageCategory3.Name = "ribbonPageCategory3";
            this.ribbonPageCategory3.Text = "ribbonPageCategory3";
            // 
            // ribbonPageCategory4
            // 
            this.ribbonPageCategory4.Name = "ribbonPageCategory4";
            this.ribbonPageCategory4.Text = "ribbonPageCategory4";
            // 
            // ribbonStatusBar
            // 
            this.ribbonStatusBar.ItemLinks.Add(this.barStaticItemGirisZamani);
            this.ribbonStatusBar.ItemLinks.Add(this.barStaticItemKullaniciBaslik);
            this.ribbonStatusBar.ItemLinks.Add(this.barStaticItemIPBaslik);
            this.ribbonStatusBar.ItemLinks.Add(this.barStaticItemClient);
            this.ribbonStatusBar.ItemLinks.Add(this.barStaticItemVersiyonBaslik);
            this.ribbonStatusBar.ItemLinks.Add(this.barStaticItemVersion);
            this.ribbonStatusBar.ItemLinks.Add(this.barSubItemGenel);
            this.ribbonStatusBar.Location = new System.Drawing.Point(0, 651);
            this.ribbonStatusBar.Name = "ribbonStatusBar";
            this.ribbonStatusBar.Ribbon = this.ribbon;
            this.ribbonStatusBar.Size = new System.Drawing.Size(1507, 22);
            // 
            // xtraTabbedMdiManager1
            // 
            this.xtraTabbedMdiManager1.ClosePageButtonShowMode = DevExpress.XtraTab.ClosePageButtonShowMode.InAllTabPageHeaders;
            this.xtraTabbedMdiManager1.HeaderButtons = DevExpress.XtraTab.TabButtons.None;
            this.xtraTabbedMdiManager1.MdiParent = this;
            // 
            // defaultLookAndFeel1
            // 
            this.defaultLookAndFeel1.LookAndFeel.SkinName = "Office 2019 Colorful";
            // 
            // workspaceManager1
            // 
            this.workspaceManager1.TargetControl = this;
            this.workspaceManager1.TransitionType = pushTransition1;
            // 
            // transitionManager1
            // 
            transition1.BarWaitingIndicatorProperties.Caption = "";
            transition1.BarWaitingIndicatorProperties.Description = "";
            transition1.Control = this;
            transition1.LineWaitingIndicatorProperties.AnimationElementCount = 5;
            transition1.LineWaitingIndicatorProperties.Caption = "";
            transition1.LineWaitingIndicatorProperties.Description = "";
            transition1.RingWaitingIndicatorProperties.AnimationElementCount = 5;
            transition1.RingWaitingIndicatorProperties.Caption = "";
            transition1.RingWaitingIndicatorProperties.Description = "";
            transition1.WaitingIndicatorProperties.Caption = "";
            transition1.WaitingIndicatorProperties.Description = "";
            this.transitionManager1.Transitions.Add(transition1);
            // 
            // accordionControl1
            // 
            this.accordionControl1.Dock = System.Windows.Forms.DockStyle.Left;
            this.accordionControl1.Elements.AddRange(new DevExpress.XtraBars.Navigation.AccordionControlElement[] {
            this.accordionControlElement1});
            this.accordionControl1.ExpandElementMode = DevExpress.XtraBars.Navigation.ExpandElementMode.Single;
            this.accordionControl1.Location = new System.Drawing.Point(0, 62);
            this.accordionControl1.Name = "accordionControl1";
            this.accordionControl1.OptionsHamburgerMenu.DisplayMode = DevExpress.XtraBars.Navigation.AccordionControlDisplayMode.Overlay;
            this.accordionControl1.OptionsMinimizing.AllowMinimizeMode = DevExpress.Utils.DefaultBoolean.False;
            this.accordionControl1.RootDisplayMode = DevExpress.XtraBars.Navigation.AccordionControlRootDisplayMode.Footer;
            this.accordionControl1.ScrollBarMode = DevExpress.XtraBars.Navigation.ScrollBarMode.Fluent;
            this.accordionControl1.ShowGroupExpandButtons = false;
            this.accordionControl1.ShowItemExpandButtons = false;
            this.accordionControl1.Size = new System.Drawing.Size(250, 589);
            this.accordionControl1.TabIndex = 3;
            this.accordionControl1.Text = "accordionControl1";
            this.accordionControl1.Visible = false;
            // 
            // accordionControlElement1
            // 
            this.accordionControlElement1.Elements.AddRange(new DevExpress.XtraBars.Navigation.AccordionControlElement[] {
            this.accordionControlElement2,
            this.accordionControlElement3});
            this.accordionControlElement1.Expanded = true;
            this.accordionControlElement1.Name = "accordionControlElement1";
            this.accordionControlElement1.Text = "Karakter İşlemleri";
            // 
            // accordionControlElement2
            // 
            this.accordionControlElement2.Name = "accordionControlElement2";
            this.accordionControlElement2.Style = DevExpress.XtraBars.Navigation.ElementStyle.Item;
            this.accordionControlElement2.Text = "Element2";
            // 
            // accordionControlElement3
            // 
            this.accordionControlElement3.Name = "accordionControlElement3";
            this.accordionControlElement3.Style = DevExpress.XtraBars.Navigation.ElementStyle.Item;
            this.accordionControlElement3.Text = "Element3";
            // 
            // FormMainRibbon
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1507, 673);
            this.Controls.Add(this.accordionControl1);
            this.Controls.Add(this.ribbonStatusBar);
            this.Controls.Add(this.ribbon);
            this.IsMdiContainer = true;
            this.Name = "FormMainRibbon";
            this.Ribbon = this.ribbon;
            this.StatusBar = this.ribbonStatusBar;
            this.Text = "Royale Support";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormMainRibbon_FormClosing);
            this.Load += new System.EventHandler(this.FormMainRibbon_Load);
            ((System.ComponentModel.ISupportInitialize)(this.ribbon)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.applicationMenu)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.popupMenu1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.popupMenu2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xtraTabbedMdiManager1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.accordionControl1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private RibbonControl ribbon;

        private RibbonStatusBar ribbonStatusBar;

        private ApplicationMenu applicationMenu;

        private XtraTabbedMdiManager xtraTabbedMdiManager1;

        private DefaultLookAndFeel defaultLookAndFeel1;

        private BarStaticItem barStaticItemVersion;

        private BarStaticItem barStaticItemServer;

        private BarStaticItem barStaticItemKullanici;

        private BarStaticItem barStaticItemGirisZamani;

        private BarStaticItem barStaticItemClient;

        private BarStaticItem barStaticItemHastane;

        private BarSubItem barSubItemGenel;

        private BarButtonItem barButtonItemSifreDegistir;

        private BarButtonItem barButtonItemEkraniKilitle;

        private BarButtonItem barButtonItemTekrarBaslat;

        private BarButtonItem barButtonItemProgramdanCikis;

        private BarButtonItem barButtonItemKullaniciBarkodu;

        private AlertControl alertControlZorunluCikis;

        private BarButtonItem barButtonItemVersiyonYukle;

        private BarStaticItem barStaticItemVersiyonBaslik;

        private BarStaticItem barStaticItemHastaneBaslik;

        private BarStaticItem barStaticItemKullaniciBaslik;

        private BarStaticItem barStaticItemGirisZamaniBaslik;

        private BarStaticItem barStaticItemIPBaslik;

        private BarButtonItem barButtonItemHatirlatici;
        private BarButtonItem barButtonItem1;
        private BarButtonGroup barButtonGroup1;
        private BarButtonItem barButtonItem2;
        private PopupMenu popupMenu1;
        private BarButtonItem barButtonItem3;
        private RibbonPageCategory ribbonPageCategory1;
        private BarButtonItem barButtonItem4;
        private PopupMenu popupMenu2;
        private BarButtonItem barButtonItem5;
        private BarButtonItem barButtonItem6;
        private BarButtonItem barButtonItem7;
        private RibbonPageCategory ribbonPageCategory2;
        private RibbonPageCategory ribbonPageCategory3;
        private DevExpress.Utils.WorkspaceManager workspaceManager1;
        private RibbonPageCategory ribbonPageCategory4;
        private DevExpress.Utils.Animation.TransitionManager transitionManager1;
        private DevExpress.XtraBars.Navigation.AccordionControl accordionControl1;
        private DevExpress.XtraBars.Navigation.AccordionControlElement accordionControlElement1;
        private DevExpress.XtraBars.Navigation.AccordionControlElement accordionControlElement2;
        private DevExpress.XtraBars.Navigation.AccordionControlElement accordionControlElement3;
        private AlertControl alertControl;
    }
}