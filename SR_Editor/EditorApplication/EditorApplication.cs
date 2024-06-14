using DevExpress.LookAndFeel;
using DevExpress.Skins;
using DevExpress.UserSkins;
using DevExpress.XtraBars;
using DevExpress.XtraBars.Ribbon;
using DevExpress.XtraEditors.Controls;
using SR_Editor.Core;
using SR_Editor.Core.Exceptions;
using SR_Editor.LookUp;
using SR_Editor.Forms;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using SR_Editor.Utility;
using Dapper;
using DevExpress.XtraBars.ToolbarForm;
using DevExpress.Utils;
using RoyaleSupport;

namespace SR_Editor.EditorApplication
{
    public class EditorApplication
    {
        public static DefaultLookAndFeel defaultLookAndFeelMain;

        private static BarManager barManager;

        private static Bar menuBar;

        private static Bar statusBar;

        private static Module module;

        public static Module Module
        {
            get
            {
                return EditorApplication.module;
            }
        }

        private static ApplicationMenu appMenu;

        private static string sqlConnectionString;

        public static string SqlConnectionString
        {
            get
            {
                return EditorApplication.sqlConnectionString;
            }
            set
            {
                EditorApplication.sqlConnectionString = value;
            }
        }

        private RibbonControl ribbonControl;

        public static bool IsDilDebugMode;

        private static CultureInfo currentCulture;

        public static string AccessToken;

        public static CultureInfo CurrentCulture
        {
            get
            {
                if (EditorApplication.currentCulture == null)
                {
                    //if (!DilTipiList.Liste.Contains(EditorApplication.languageId))
                    {
                        EditorApplication.currentCulture = CultureInfo.CreateSpecificCulture("en-EN");
                    }
                    /*else
                    {
                        EditorApplication.currentCulture = CultureInfo.CreateSpecificCulture(DilTipiList.Liste[EditorApplication.languageId].Culture);
                    }
                    if (EditorApplication.LanguageId != 3)
                    {
                        EditorApplication.currentCulture.NumberFormat.NumberDecimalSeparator = ",";
                        EditorApplication.currentCulture.NumberFormat.NumberGroupSeparator = ".";
                    }
                    else*/
                    {
                        EditorApplication.currentCulture.NumberFormat.NumberDecimalSeparator = ".";
                        EditorApplication.currentCulture.NumberFormat.NumberGroupSeparator = ",";
                    }
                    EditorApplication.currentCulture.DateTimeFormat.AMDesignator = "";
                    EditorApplication.currentCulture.DateTimeFormat.PMDesignator = "";
                    EditorApplication.currentCulture.DateTimeFormat.DateSeparator = ".";
                    EditorApplication.currentCulture.DateTimeFormat.LongTimePattern = "HH:mm:ss";
                    EditorApplication.currentCulture.DateTimeFormat.ShortTimePattern = "HH:mm";
                    EditorApplication.currentCulture.DateTimeFormat.ShortDatePattern = "dd.MM.yyyy";
                    EditorApplication.currentCulture.DateTimeFormat.LongDatePattern = "dd MMMM yyyy dddd";
                }
                return EditorApplication.currentCulture;
            }
            set
            {
                EditorApplication.currentCulture = value;
            }
        }

        public static int LanguageId { get; internal set; }
        public static ApplicationConfigurationDto Configuration { get; set; }

        private static void SetClientRegistrySkin(string pSkinName)
        {
            //Registry.CurrentUser.CreateSubKey(EnumRegistryKey.Skin).SetValue("PusulaSkin", pSkinName);
        }

        public static Dictionary<int, Dictionary<DatabaseType, string>> Connections;
        public static string ToolConnection;
        public static string AccountConnection;
        public static string PanelConnection;

        public static void InitApplication()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(true);
            EditorApplication.SetDevExpressDefaults();

            //EditorApplication.GetRegistryKeys();

        }
        private static void SetDevExpressDefaults()
        {
            OfficeSkins.Register();
            BonusSkins.Register();
            SkinManager.EnableFormSkins();
            SkinManager.EnableMdiFormSkins();
            Localizer.Active = Localizer.CreateDefaultLocalizer();
        }

        public static void Start(bool debug = false)
        {
            UnhandledExceptionManager.UnhandledYakala();
            //Application.ApplicationExit += new EventHandler(EditorApplication.Application_ApplicationExit);
            EditorApplication.InitModulInfo();
            //EditorApplication.GetConnectionInfoFromFile();
            //EditorApplication.SetAllSqlConntectionString(null);
            if (Thread.CurrentThread.CurrentCulture != EditorApplication.CurrentCulture)
            {
                Thread.CurrentThread.CurrentCulture = EditorApplication.CurrentCulture;
                Thread.CurrentThread.CurrentUICulture = EditorApplication.CurrentCulture;
            }
            if (FormLogin.GirisFormuAc() == DialogResult.OK)
            {
                Application.Run(new FormMainRibbon());
            }
        }

        public static EditorIslemSonuc TestConnection(string pStrConnection)
        {
            EditorIslemSonuc EditorIslemSonuc = new EditorIslemSonuc(EnumSonucTipi.IslemBasarisiz)
            {
                MesajBaslik = "",
                MesajIcerik = ""
            };
            SqlConnection sqlConnection = new SqlConnection(pStrConnection);
            try
            {
                try
                {
                    sqlConnection.Open();
                    EditorIslemSonuc.SonucKodu = EnumSonucTipi.IslemBasarili;
                }
                catch (Exception exception)
                {
                    EditorIslemSonuc.MesajIcerik = exception.Message;
                }
            }
            finally
            {
                sqlConnection.Close();
                sqlConnection.Dispose();
            }
            return EditorIslemSonuc;
        }


        private static void AddMenuItem(BarSubItem parentItem, ModuleInfo moduleItem)
        {
            // if ((!UtilConfig.IsLisansAktif ? true : UtilConfig.ListModuleLisans.FindAll((ModuleLisans p) => p.ModuleLisansKey == moduleItem.LisansKey).Count != 0))
            {
                BarButtonItem barButtonItem = new BarButtonItem();
                if (EditorApplication.menuBar == null)
                {
                    barButtonItem.Id = (EditorApplication.appMenu.Manager.GetNewItemId());
                }
                else
                {
                    barButtonItem.Id = (EditorApplication.menuBar.Manager.GetNewItemId());
                }
                if (EditorApplication.LanguageId == 1)
                {
                    barButtonItem.Hint = (moduleItem.Caption);
                }
                barButtonItem.Caption = (moduleItem.Caption).CeviriYap();
                barButtonItem.Tag = (moduleItem);
                barButtonItem.PaintStyle = BarItemPaintStyle.Caption;
                parentItem.Manager.Items.Add(barButtonItem);
                parentItem.LinksPersistInfo.Add(new LinkPersistInfo(barButtonItem));
                barButtonItem.ItemShortcut = (moduleItem.ItemShortcut);
                barButtonItem.ItemClick += EditorApplication.MenuItem_ItemClick;
                /*if ((EditorApplication.listFormSikKullanilan == null || EditorApplication.appMenu == null ? false : EditorApplication.appMenu.get_Ribbon() != null))
                {
                    if (EditorApplication.listFormSikKullanilan.Find((FormSikKullanilan p) => p.Kodu == moduleItem.FullKey) != null)
                    {
                        BarStaticItem barStaticItem = new BarStaticItem();
                        barStaticItem.set_Caption(moduleItem.Caption);
                        barStaticItem.set_Tag(moduleItem);
                        barStaticItem.add_ItemClick(new ItemClickEventHandler(null, EditorApplication.MenuItem_ItemClick));
                        EditorApplication.appMenu.get_Ribbon().get_Toolbar().get_ItemLinks().Add(barStaticItem);
                    }
                }*/
            }
        }

        private static BarItem AddModuleGroup(BarSubItem parentItem, ModuleInfoGroup pModuleGroup, bool isMainModule = false)
        {
            BarItem barItem = null;
            bool flag;
            //if ((!UtilConfig.IsLisansAktif ? true : UtilConfig.ListModuleLisans.FindAll((ModuleLisans p) => p.ModuleLisansKey == pModuleGroup.LisansKey).Count != 0))
            {
                BarSubItem barSubItem = new BarSubItem();
                if (EditorApplication.menuBar == null)
                {
                    barSubItem.Id = (EditorApplication.appMenu.Manager.GetNewItemId());
                }
                else
                {
                    barSubItem.Id = (EditorApplication.menuBar.Manager.GetNewItemId());
                }
                //if ((int)Akdeniz.Core.EditorApplication.EditorApplication.LanguageId == 1)
                //    barSubItem.Hint = pModuleGroup.Caption;
                barSubItem.Caption = (pModuleGroup.Caption).CeviriYap();
                barSubItem.Tag = (pModuleGroup);
                if (pModuleGroup.ImageName != "")
                {
                    if (isMainModule)
                        barSubItem.Glyph = (UtilEditorImage.Instance.GetSubModuleImage(pModuleGroup.ImageName));
                    else
                        barSubItem.Glyph = (UtilEditorImage.Instance.GetSubModuleImage(pModuleGroup.ImageName));
                }
                barSubItem.PaintStyle = BarItemPaintStyle.CaptionGlyph;
                barSubItem.MenuDrawMode = MenuDrawMode.SmallImagesText;
                barSubItem.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
                if (parentItem != null)
                {
                    parentItem.Manager.Items.Add(barSubItem);
                    parentItem.LinksPersistInfo.Add(new LinkPersistInfo(barSubItem));
                }
                else if (EditorApplication.menuBar == null)
                {
                    EditorApplication.appMenu.Manager.Items.Add(barSubItem);
                    EditorApplication.appMenu.Ribbon.Items.Add(barSubItem);
                    EditorApplication.appMenu.ItemLinks.Add(barSubItem);
                }
                else
                {
                    EditorApplication.menuBar.Manager.Items.Add(barSubItem);
                    EditorApplication.menuBar.LinksPersistInfo.Add(new LinkPersistInfo(barSubItem));
                }
                List<ModuleInfoGroup> list = (
                    from t in UtilConfig.ListModuleInfo
                    where t.ParentKey == pModuleGroup.FullKey
                    select t).ToList<ModuleInfoGroup>();
                if (list.Count > 0)
                {
                    foreach (ModuleInfoGroup moduleInfoGroup in list)
                    {
                        if (!(moduleInfoGroup is ModuleInfo))
                        {
                            EditorApplication.AddModuleGroup(barSubItem, moduleInfoGroup);
                        }
                        else
                        {
                            if (!moduleInfoGroup.IsMenuVisible)
                            {
                                flag = true;
                            }
                            else
                            {
                                flag = false;
                            }
                            if (!flag)
                            {
                                EditorApplication.AddMenuItem(barSubItem, (ModuleInfo)moduleInfoGroup);
                            }
                        }
                    }
                }
                if (barSubItem.LinksPersistInfo.Count == 0)
                {
                    barSubItem.Visibility = BarItemVisibility.Never;
                    barSubItem = null;
                }
                barItem = barSubItem;
            }
            /*else
            {
                barItem = null;
            }*/
            return barItem;
        }

        private static void AddMenuItem2(BarSubItem parentItem, ModuleInfo moduleItem)
        {
            // if ((!UtilConfig.IsLisansAktif ? true : UtilConfig.ListModuleLisans.FindAll((ModuleLisans p) => p.ModuleLisansKey == moduleItem.LisansKey).Count != 0))
            {
                BarButtonItem barButtonItem = new BarButtonItem();
                if (EditorApplication.menuBar == null)
                {
                    barButtonItem.Id = (EditorApplication.appMenu.Manager.GetNewItemId());
                }
                else
                {
                    barButtonItem.Id = (EditorApplication.menuBar.Manager.GetNewItemId());
                }
                if (EditorApplication.LanguageId == 1)
                {
                    barButtonItem.Hint = (moduleItem.Caption);
                }
                barButtonItem.Caption = (moduleItem.Caption).CeviriYap();
                barButtonItem.Tag = (moduleItem);
                barButtonItem.PaintStyle = BarItemPaintStyle.Caption;
                parentItem.Manager.Items.Add(barButtonItem);
                parentItem.LinksPersistInfo.Add(new LinkPersistInfo(barButtonItem));
                barButtonItem.ItemShortcut = (moduleItem.ItemShortcut);
                barButtonItem.ItemClick += EditorApplication.MenuItem_ItemClick;
                /*if ((EditorApplication.listFormSikKullanilan == null || EditorApplication.appMenu == null ? false : EditorApplication.appMenu.get_Ribbon() != null))
                {
                    if (EditorApplication.listFormSikKullanilan.Find((FormSikKullanilan p) => p.Kodu == moduleItem.FullKey) != null)
                    {
                        BarStaticItem barStaticItem = new BarStaticItem();
                        barStaticItem.set_Caption(moduleItem.Caption);
                        barStaticItem.set_Tag(moduleItem);
                        barStaticItem.add_ItemClick(new ItemClickEventHandler(null, EditorApplication.MenuItem_ItemClick));
                        EditorApplication.appMenu.get_Ribbon().get_Toolbar().get_ItemLinks().Add(barStaticItem);
                    }
                }*/
            }
        }

        private static BarItem AddModuleGroup2(BarSubItem parentItem, ModuleInfoGroup pModuleGroup)
        {
            BarItem barItem;
            bool flag;
            //if ((!UtilConfig.IsLisansAktif ? true : UtilConfig.ListModuleLisans.FindAll((ModuleLisans p) => p.ModuleLisansKey == pModuleGroup.LisansKey).Count != 0))
            {
                BarSubItem barSubItem = new BarSubItem();
                if (EditorApplication.menuBar == null)
                {
                    barSubItem.Id = (EditorApplication.appMenu.Manager.GetNewItemId());
                }
                else
                {
                    barSubItem.Id = (EditorApplication.menuBar.Manager.GetNewItemId());
                }

                barSubItem.Caption = (pModuleGroup.Caption).CeviriYap();
                barSubItem.Tag = (pModuleGroup);
                if (pModuleGroup.ImageName != "")
                {
                    //barSubItem.set_Glyph(UtilPusulaImage.Instance.GetMainModuleImage(pModuleGroup.ImageName));
                }
                barSubItem.PaintStyle = BarItemPaintStyle.CaptionGlyph;
                barSubItem.MenuDrawMode = MenuDrawMode.LargeImagesText;
                barSubItem.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
                if (parentItem != null)
                {
                    parentItem.Manager.Items.Add(barSubItem);
                    parentItem.LinksPersistInfo.Add(new LinkPersistInfo(barSubItem));
                }
                else if (EditorApplication.menuBar == null)
                {
                    EditorApplication.appMenu.Manager.Items.Add(barSubItem);
                    EditorApplication.appMenu.Ribbon.Items.Add(barSubItem);
                    EditorApplication.appMenu.ItemLinks.Add(barSubItem);
                }
                else
                {
                    EditorApplication.menuBar.Manager.Items.Add(barSubItem);
                    EditorApplication.menuBar.LinksPersistInfo.Add(new LinkPersistInfo(barSubItem));
                }
                List<ModuleInfoGroup> list = (
                    from t in UtilConfig.ListModuleInfo
                    where t.ParentKey == pModuleGroup.FullKey
                    select t).ToList<ModuleInfoGroup>();
                if (list.Count > 0)
                {
                    foreach (ModuleInfoGroup moduleInfoGroup in list)
                    {
                        if (!(moduleInfoGroup is ModuleInfo))
                        {
                            EditorApplication.AddModuleGroup2(barSubItem, moduleInfoGroup);
                        }
                        else
                        {
                            if (!moduleInfoGroup.IsMenuVisible)
                            {
                                flag = true;
                            }
                            else
                            {
                                flag = false;
                            }
                            if (!flag)
                            {
                                EditorApplication.AddMenuItem2(barSubItem, (ModuleInfo)moduleInfoGroup);
                            }
                        }
                    }
                }
                if (barSubItem.LinksPersistInfo.Count == 0)
                {
                    barSubItem.Visibility = BarItemVisibility.Never;
                }
                barItem = barSubItem;
            }
            /*else
            {
                barItem = null;
            }*/
            return barItem;
        }

        public static void InitMenu(ApplicationMenu pApplicationMenu)
        {
            EditorApplication.appMenu = pApplicationMenu;
            EditorApplication.InitRibbonControl();
            EditorApplication.appMenu.MenuDrawMode = MenuDrawMode.LargeImagesText;
            EditorApplication.appMenu.Manager.BeginUpdate();
            EditorApplication.appMenu.Manager.BeginInit();
            try
            {
                List<ModuleInfoGroup> list = (
                    from t in UtilConfig.ListModuleInfo
                    where t.ParentKey == EditorApplication.Module.Key
                    select t).ToList<ModuleInfoGroup>();
                foreach (ModuleInfoGroup moduleInfoGroup in list)
                {
                    //if ((Session.IsAdmin() ? true : (
                    //    from t in KullaniciForm.List
                    //    where t.EkKodu.StartsWith(moduleInfoGroup.FullKey)
                    //    select t).Count<KullaniciForm>() > 0))
                    {
                        EditorApplication.AddModuleGroup(null, moduleInfoGroup);
                        //EditorApplication.AddModuleGroup2(null, moduleInfoGroup);
                    }

                }

                //UserLookAndFeel.Default.SetSkinStyle("Caramel");
                UserLookAndFeel.Default.SetSkinStyle(SkinStyle.Office2019Colorful, SkinSvgPalette.Office2019Colorful);
                EditorApplication.appMenu.Manager.GetController().PaintStyleName = ("Skin");
            }
            finally
            {
                try
                {
                    EditorApplication.appMenu.Manager.EndInit();
                    EditorApplication.appMenu.Manager.EndUpdate();
                }
                catch
                {
                }
            }
        }

        public static void InitMenu(BarManager pBarManager, Bar pMenuBar, Bar pStatusBar)
        {
            EditorApplication.barManager = pBarManager;
            EditorApplication.menuBar = pMenuBar;
            EditorApplication.statusBar = pStatusBar;
            if (EditorApplication.menuBar != null)
            {
                EditorApplication.menuBar.Manager.BeginUpdate();
                EditorApplication.menuBar.Manager.BeginInit();
                EditorApplication.menuBar.Manager.MainMenu.BeginUpdate();
                try
                {
                    EditorApplication.menuBar.BeginUpdate();
                    EditorApplication.menuBar.ClearLinks();
                    List<ModuleInfoGroup> list = (
                        from t in UtilConfig.ListModuleInfo
                        where t.ParentKey == EditorApplication.Module.Key
                        select t).ToList<ModuleInfoGroup>();
                    foreach (ModuleInfoGroup moduleInfoGroup in list)
                    {
                        //if ((Session.IsAdmin() ? true : (
                        //    from t in KullaniciForm.List
                        //    where t.EkKodu.StartsWith(moduleInfoGroup.FullKey)
                        //    select t).Count<KullaniciForm>() > 0))
                        {
                            EditorApplication.AddModuleGroup(null, moduleInfoGroup);
                        }
                    }
                }
                finally
                {
                    try
                    {
                        EditorApplication.menuBar.Manager.MainMenu.EndUpdate();
                        EditorApplication.menuBar.Manager.EndInit();
                        EditorApplication.menuBar.EndUpdate();
                        EditorApplication.menuBar.Manager.EndUpdate();
                    }
                    catch
                    {
                    }
                }
            }
        }

        public static void InitRibbonMenu(RibbonControl ribbon)
        {
            RibbonPageCategory ribbonPageCategory = new RibbonPageCategory("AnaMenu", Color.Blue);

            //ribbon.PageCategories.Add(ribbonPageCategory);

            List<ModuleInfoGroup> list = (
                from t in UtilConfig.ListModuleInfo
                where t.ParentKey == EditorApplication.Module.Key
                select t).ToList<ModuleInfoGroup>();
            foreach (ModuleInfoGroup moduleInfoGroup in list)
            {
                //if ((Session.IsAdmin() ? true : (
                //    from t in KullaniciForm.List
                //    where t.EkKodu.StartsWith(moduleInfoGroup.FullKey)
                //    select t).Count<KullaniciForm>() > 0))
                {
                    var new_page = EditorApplication.AddRibbonPage(ribbonPageCategory, moduleInfoGroup);

                    if (new_page != null)
                        ribbon.Pages.Add(new_page);
                }
            }

            RibbonPageCategory ribbonPageDesign = new RibbonPageCategory("Tasarim", Color.Aqua);

#if DEBUG
            EditorApplication.LoadSkin(ribbonPageDesign);
#endif
        }

        public static void AddRibbonPageGroup(RibbonPage parentPage, ModuleInfoGroup moduleInfoGroup)
        {
            RibbonPageGroup new_group = new RibbonPageGroup();
            new_group.Text = moduleInfoGroup.Caption.CeviriYap();
            new_group.ShowCaptionButton = false;
            new_group.AllowMinimize = false;

            var new_item = AddModuleGroup(null, moduleInfoGroup, true);
            if (new_item != null)
                new_group.ItemLinks.Add(new_item);

            if (new_group.ItemLinks.Count == 0)
            {
                new_group.Visible = false;
            }
            else
                parentPage.Groups.Add(new_group);
        }

        public static RibbonPage AddRibbonPage(RibbonPageCategory parentPage, ModuleInfoGroup moduleInfoGroup)
        {
            RibbonPage new_page = new RibbonPage();
            //new_page.Id = (EditorApplication.menuBar.Manager.GetNewItemId());
            //new_page.Hint = (moduleInfoGroup.Caption);
            new_page.Text = (moduleInfoGroup.Caption).CeviriYap();
            new_page.Tag = (moduleInfoGroup);
            new_page.Appearance.Font = new Font(new_page.Appearance.Font.FontFamily, 8.0F, FontStyle.Bold);
            if (moduleInfoGroup.ImageName != "")
            {
                new_page.Image = (UtilEditorImage.Instance.GetMainModuleImage(moduleInfoGroup.ImageName));
            }
            //new_page.PaintStyle = BarItemPaintStyle.CaptionGlyph;
            //new_page.MenuDrawMode = MenuDrawMode.LargeImagesText;
            new_page.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            parentPage.Pages.Add(new_page);

            bool isInvisible = false;

            List<ModuleInfoGroup> list = (
                    from t in UtilConfig.ListModuleInfo
                    where t.ParentKey == moduleInfoGroup.FullKey
                    select t).ToList<ModuleInfoGroup>();
            if (list.Count > 0)
            {
                foreach (ModuleInfoGroup mig in list)
                {
                    if (!(mig is ModuleInfo))
                    {
                        EditorApplication.AddRibbonPageGroup(new_page, mig);
                    }
                    else
                    {
                        if (!mig.IsMenuVisible)
                        {
                            isInvisible = true;
                        }
                        else
                        {
                            isInvisible = false;
                        }
                        if (!isInvisible)
                        {
                            //EditorApplication.AddRibbonMenuItem(new_page, (ModuleInfo)mig);
                        }
                    }
                }
            }
            if (new_page.Groups.Count == 0)
            {
                new_page.Visible = false;
                new_page = null;
            }

            return new_page;

        }

        public static void InitMenu2(BarManager pBarManager, Bar pMenuBar, Bar pStatusBar)
        {
            EditorApplication.barManager = pBarManager;
            EditorApplication.menuBar = pMenuBar;
            EditorApplication.statusBar = pStatusBar;
            if (EditorApplication.menuBar != null)
            {
                EditorApplication.menuBar.Manager.BeginUpdate();
                EditorApplication.menuBar.Manager.BeginInit();
                EditorApplication.menuBar.Manager.MainMenu.BeginUpdate();
                try
                {
                    EditorApplication.menuBar.BeginUpdate();
                    EditorApplication.menuBar.ClearLinks();
                    List<ModuleInfoGroup> list = (
                        from t in UtilConfig.ListModuleInfo
                        where t.ParentKey == EditorApplication.Module.Key
                        select t).ToList<ModuleInfoGroup>();
                    foreach (ModuleInfoGroup moduleInfoGroup in list)
                    {
                        //if ((Session.IsAdmin() ? true : (
                        //    from t in KullaniciForm.List
                        //    where t.EkKodu.StartsWith(moduleInfoGroup.FullKey)
                        //    select t).Count<KullaniciForm>() > 0))
                        {
                            EditorApplication.AddModuleGroup2(null, moduleInfoGroup);
                        }
                    }
                }
                finally
                {
                    try
                    {
                        EditorApplication.menuBar.Manager.MainMenu.EndUpdate();
                        EditorApplication.menuBar.Manager.EndInit();
                        EditorApplication.menuBar.EndUpdate();
                        EditorApplication.menuBar.Manager.EndUpdate();
                    }
                    catch
                    {
                    }
                }
            }
        }

        private static void InitMenuModulInfo()
        {
            List<ModuleInfoGroup> moduleInfoGroups = new List<ModuleInfoGroup>();
            //moduleInfoGroups = (!Session.IsAdmin() ? (
            //    from t in UtilConfig.ListModuleInfo
            //    where (
            //        from t2 in KullaniciForm.List
            //        where t2.EkKodu == t.FullKey
            //        select t2).Count<KullaniciForm>() > 0
            //    select t).ToList<ModuleInfoGroup>() : UtilConfig.ListModuleInfo);
        }

        public static void InitModulInfo()
        {
            EditorApplication.module = new Module();
           // EditorApplication.yetki = new Yetki.Yetki();
            //EditorApplication.parametre = new Parametre();
        }

        private static void InitRibbonControl()
        {
            if ((EditorApplication.appMenu == null ? false : EditorApplication.appMenu.Ribbon != null))
            {
                //EditorApplication.appMenu.get_Ribbon().add_ShowCustomizationMenu(new RibbonCustomizationMenuEventHandler(null, EditorApplication.RibbonControl_ShowCustomizationMenu));
                //EditorApplication.sikKullanilanaEkleBarButtonItem.set_Caption(UtilLanguage.GetCeviriStr("Sık kullanılanlara ekle", false));
                //EditorApplication.sikKullanilanaEkleBarButtonItem.add_ItemClick(new ItemClickEventHandler(null, EditorApplication.SikKullanilanaEkleBarButtonItem_Click));
                //EditorApplication.sikKullanilandanCikarBarButtonItem.set_Caption(UtilLanguage.GetCeviriStr("Sık kullanılanlardan çıkar", false));
                //EditorApplication.sikKullanilandanCikarBarButtonItem.add_ItemClick(new ItemClickEventHandler(null, EditorApplication.SikKullanilandanCikarBarButtonItem_Click));
                //EditorApplication.acilisFormuYapBarButtonItem.set_Caption(UtilLanguage.GetCeviriStr("Açılış ekranı yap", false));
                //EditorApplication.acilisFormuYapBarButtonItem.add_ItemClick(new ItemClickEventHandler(null, EditorApplication.AcilisFormuYapBarButtonItem_Click));
                //EditorApplication.listFormSikKullanilan = CoreEntities.Instance.FormSikKullanilanQuery.GetByKullaniciId(Kullanici.AktifKullanici.Id);
            }
        }

        private static void LoadSkin(RibbonPageCategory ribbonPage)
        {
            RibbonPage new_page = new RibbonPage();
            new_page.Text = "Genel Temalar";
            new_page.Appearance.Font = new Font(new_page.Appearance.Font.FontFamily, 8.0F, FontStyle.Bold);
            new_page.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            ribbonPage.Pages.Add(new_page);

            RibbonPage new_page1 = new RibbonPage();
            new_page1.Text = "Genel Temalar";
            new_page1.Appearance.Font = new Font(new_page1.Appearance.Font.FontFamily, 8.0F, FontStyle.Bold);
            new_page1.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            ribbonPage.Pages.Add(new_page1);

            RibbonPage new_page2 = new RibbonPage();
            new_page2.Text = "Genel Temalar";
            new_page2.Appearance.Font = new Font(new_page2.Appearance.Font.FontFamily, 8.0F, FontStyle.Bold);
            new_page2.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            ribbonPage.Pages.Add(new_page2);


            RibbonPageGroup new_group = new RibbonPageGroup();
            new_group.Text = "Tasarim";
            new_group.ShowCaptionButton = false;
            new_group.AllowMinimize = false;
            new_page.Groups.Add(new_group);


            foreach (SkinContainer skin in SkinManager.Default.Skins)
            {
                BarButtonItem barButtonItem = new BarButtonItem(EditorApplication.appMenu.Manager, skin.SkinName);
                if (skin.SkinName != "Pumpkin")
                {
                    barButtonItem.Name = (string.Concat("bi", skin.SkinName));
                    barButtonItem.Id = (EditorApplication.appMenu.Manager.GetNewItemId());
                    if (!(skin.SkinName == "Office 2007 Blue" ? false : !(skin.SkinName == "Valentine")))
                    {
                        new_group.ItemLinks.Add(barButtonItem);
                    }
                    else if (skin.SkinName.Contains("Office"))
                    {
                        //barSubItem2.AddItem(barButtonItem);
                        new_group.ItemLinks.Add(barButtonItem);
                    }
                    else if ((skin.SkinName == "Black" || skin.SkinName == "Blue" || skin.SkinName == "Caramel" || skin.SkinName == "iMaginary" || skin.SkinName == "Money Twins" || skin.SkinName == "The Asphalt World" ? false : !(skin.SkinName == "Lilian")))
                    {
                        //barSubItem3.AddItem(barButtonItem);
                        new_group.ItemLinks.Add(barButtonItem);
                    }
                    else
                    {
                        //barSubItem1.AddItem(barButtonItem);
                        new_group.ItemLinks.Add(barButtonItem);
                    }
                }
                barButtonItem.ItemClick += EditorApplication.OnSkinClick;
            }

            return;



            BarSubItem barSubItem = new BarSubItem(EditorApplication.appMenu.Manager, "Akdeniz Temalar");
            EditorApplication.appMenu.Manager.Items.Add(barSubItem);
            EditorApplication.appMenu.Ribbon.Items.Add(barSubItem);
            EditorApplication.appMenu.ItemLinks.Add(barSubItem);
            BarSubItem barSubItem1 = new BarSubItem(EditorApplication.appMenu.Manager, "Genel Temalar");
            BarSubItem barSubItem2 = new BarSubItem(EditorApplication.appMenu.Manager, "Ofis Temaları");
            BarSubItem barSubItem3 = new BarSubItem(EditorApplication.appMenu.Manager, "Bonus Temalar");
            barSubItem.AddItem(barSubItem1);
            barSubItem.AddItem(barSubItem2);
            barSubItem.AddItem(barSubItem3);
            foreach (SkinContainer skin in SkinManager.Default.Skins)
            {
                BarButtonItem barButtonItem = new BarButtonItem(EditorApplication.appMenu.Manager, skin.SkinName);
                if (skin.SkinName != "Pumpkin")
                {
                    barButtonItem.Name = (string.Concat("bi", skin.SkinName));
                    barButtonItem.Id = (EditorApplication.appMenu.Manager.GetNewItemId());
                    if (!(skin.SkinName == "Office 2007 Blue" ? false : !(skin.SkinName == "Valentine")))
                    {
                        barSubItem.AddItem(barButtonItem);
                    }
                    else if (skin.SkinName.Contains("Office"))
                    {
                        barSubItem2.AddItem(barButtonItem);
                    }
                    else if ((skin.SkinName == "Black" || skin.SkinName == "Blue" || skin.SkinName == "Caramel" || skin.SkinName == "iMaginary" || skin.SkinName == "Money Twins" || skin.SkinName == "The Asphalt World" ? false : !(skin.SkinName == "Lilian")))
                    {
                        barSubItem3.AddItem(barButtonItem);
                    }
                    else
                    {
                        barSubItem1.AddItem(barButtonItem);
                    }
                }
                barButtonItem.ItemClick += EditorApplication.OnSkinClick;
            }
        }

        public static void Login(string pUserName, string pPassword)
        {
        }

        private static void MenuItem_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (e.Item.Tag is ModuleInfo)
            {
                (e.Item.Tag as ModuleInfo).Show(null);
            }
        }

        private static void OnSkinClick(object sender, ItemClickEventArgs e)
        {
            UserLookAndFeel.Default.SetSkinStyle(e.Item.Caption);
            EditorApplication.appMenu.Manager.GetController().PaintStyleName = ("Skin");
            //EditorApplication.SetClientRegistrySkin(e.get_Item().get_Caption());
        }

    }
}
