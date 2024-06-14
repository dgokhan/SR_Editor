namespace SR_Editor.Core.Controls
{
    using DevExpress.Utils;
    using DevExpress.Utils.Menu;
    using DevExpress.XtraBars;
    using DevExpress.XtraEditors;
    using DevExpress.XtraEditors.Controls;
    using DevExpress.XtraEditors.Repository;
    using DevExpress.XtraLayout;
    using DevExpress.XtraLayout.Utils;
    using DevExpress.XtraRichEdit;
    using DevExpress.XtraRichEdit.API.Native;
    using DevExpress.XtraRichEdit.Design;
    using DevExpress.XtraRichEdit.UI;
    using System;
    using System.Collections;
    using System.ComponentModel;
    using System.Drawing;
    using System.IO;
    using System.Windows.Forms;
    partial class RichTextEditor
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;
        private RichTextFind dlgFind;
        private RichTextReplace dlgReplace;
        private Cursor currentCursor;
        private ImageList imageList1;
        private PopupMenu popupMenu1;
        private LayoutControl layoutControl1;
        private RichEditControl richEditControl1;
        private BarManager barManager1;
        private CutItem cutItem1;
        private CopyItem copyItem1;
        private PasteItem pasteItem1;
        private PasteSpecialItem pasteSpecialItem1;
        private StandaloneBarDockControl standaloneBarDockControl1;
        private FontBar fontBar1;
        private ChangeFontNameItem changeFontNameItem1;
        private RepositoryItemFontEdit repositoryItemFontEdit1;
        private ChangeFontSizeItem changeFontSizeItem1;
        private RepositoryItemRichEditFontSizeEdit repositoryItemRichEditFontSizeEdit1;
        private ChangeFontColorItem changeFontColorItem1;
        private ChangeFontBackColorItem changeFontBackColorItem1;
        private ToggleFontBoldItem toggleFontBoldItem1;
        private ToggleFontItalicItem toggleFontItalicItem1;
        private ToggleFontUnderlineItem toggleFontUnderlineItem1;
        private ToggleFontDoubleUnderlineItem toggleFontDoubleUnderlineItem1;
        private ToggleFontStrikeoutItem toggleFontStrikeoutItem1;
        private ToggleFontDoubleStrikeoutItem toggleFontDoubleStrikeoutItem1;
        private ToggleFontSuperscriptItem toggleFontSuperscriptItem1;
        private ToggleFontSubscriptItem toggleFontSubscriptItem1;
        private ChangeTextCaseItem changeTextCaseItem1;
        private MakeTextUpperCaseItem makeTextUpperCaseItem1;
        private MakeTextLowerCaseItem makeTextLowerCaseItem1;
        private ToggleTextCaseItem toggleTextCaseItem1;
        private FontSizeIncreaseItem fontSizeIncreaseItem1;
        private FontSizeDecreaseItem fontSizeDecreaseItem1;
        private ClearFormattingItem clearFormattingItem1;
        private ShowFontFormItem showFontFormItem1;
        private ParagraphBar paragraphBar1;
        private ToggleParagraphAlignmentLeftItem toggleParagraphAlignmentLeftItem1;
        private ToggleParagraphAlignmentCenterItem toggleParagraphAlignmentCenterItem1;
        private ToggleParagraphAlignmentRightItem toggleParagraphAlignmentRightItem1;
        private ToggleParagraphAlignmentJustifyItem toggleParagraphAlignmentJustifyItem1;
        private ChangeParagraphLineSpacingItem changeParagraphLineSpacingItem1;
        private SetSingleParagraphSpacingItem setSingleParagraphSpacingItem1;
        private SetSesquialteralParagraphSpacingItem setSesquialteralParagraphSpacingItem1;
        private SetDoubleParagraphSpacingItem setDoubleParagraphSpacingItem1;
        private ShowLineSpacingFormItem showLineSpacingFormItem1;
        private AddSpacingBeforeParagraphItem addSpacingBeforeParagraphItem1;
        private RemoveSpacingBeforeParagraphItem removeSpacingBeforeParagraphItem1;
        private AddSpacingAfterParagraphItem addSpacingAfterParagraphItem1;
        private RemoveSpacingAfterParagraphItem removeSpacingAfterParagraphItem1;
        private ToggleBulletedListItem toggleBulletedListItem1;
        private ToggleNumberingListItem toggleNumberingListItem1;
        private ToggleMultiLevelListItem toggleMultiLevelListItem1;
        private DecreaseIndentItem decreaseIndentItem1;
        private IncreaseIndentItem ıncreaseIndentItem1;
        private ToggleShowWhitespaceItem toggleShowWhitespaceItem1;
        private ShowParagraphFormItem showParagraphFormItem1;
        private EditingBar editingBar1;
        private FindItem findItem1;
        private ReplaceItem replaceItem1;
        private BarDockControl barDockControlTop;
        private BarDockControl barDockControlBottom;
        private BarDockControl barDockControlLeft;
        private BarDockControl barDockControlRight;
        private ChangeStyleItem changeStyleItem1;
        private RepositoryItemRichEditStyleEdit repositoryItemRichEditStyleEdit1;
        private PanelControl panelControl1;
        private LayoutControlGroup layoutControlGroup1;
        private LayoutControlItem layoutControlItemToolBar;
        private LayoutControlItem layoutControlItem2;
        private RichEditBarController richEditBarController1;
        private PagesBar pagesBar1;
        private InsertPageBreakItem ınsertPageBreakItem1;
        private TablesBar tablesBar1;
        private InsertTableItem ınsertTableItem1;
        private IllustrationsBar ıllustrationsBar1;
        private InsertPictureItem ınsertPictureItem1;
        private HeaderFooterBar headerFooterBar1;
        private EditPageFooterItem editPageFooterItem1;
        private InsertPageNumberItem ınsertPageNumberItem1;
        private InsertPageCountItem ınsertPageCountItem1;
        private SymbolsBar symbolsBar1;
        private InsertSymbolItem ınsertSymbolItem1;
        private FileNewItem fileNewItem1;
        private FileOpenItem fileOpenItem1;
        private FileSaveItem fileSaveItem1;
        private FileSaveAsItem fileSaveAsItem1;
        private QuickPrintItem quickPrintItem1;
        private PrintItem printItem1;
        private PrintPreviewItem printPreviewItem1;
        private UndoItem undoItem1;
        private RedoItem redoItem1;
        private InsertBookmarkItem ınsertBookmarkItem1;
        private InsertHyperlinkItem ınsertHyperlinkItem1;
        private EditPageHeaderItem editPageHeaderItem1;
        private RichTextColor cp;
        private ComponentResourceManager componentResourceManager;

        public event RichTextEditor.RichTextEventHandler OnRichTextBoxTextChanged;

        public event RichTextEditor.RichTextEventHandler OnSaveButtonClick;

        public event RichTextEditor.RichTextEventHandler OnOpenButtonClick;

        public event RichTextEditor.RichTextEventHandler OnPrintButtonClick;

        public event EventHandler OnMhtTextChanged;

        public event EventHandler OnModifiedChanged;

        public event EventHandler OnTextChanged;

        public event EventHandler OnLeave;

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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RichTextEditor));
            this.imageList1 = new System.Windows.Forms.ImageList();
            this.popupMenu1 = new DevExpress.XtraBars.PopupMenu();
            this.layoutControl1 = new DevExpress.XtraLayout.LayoutControl();
            this.richEditControl1 = new DevExpress.XtraRichEdit.RichEditControl();
            this.barManager1 = new DevExpress.XtraBars.BarManager();
            this.fontBar1 = new DevExpress.XtraRichEdit.UI.FontBar();
            this.changeFontNameItem1 = new DevExpress.XtraRichEdit.UI.ChangeFontNameItem();
            this.repositoryItemFontEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemFontEdit();
            this.changeFontSizeItem1 = new DevExpress.XtraRichEdit.UI.ChangeFontSizeItem();
            this.repositoryItemRichEditFontSizeEdit1 = new DevExpress.XtraRichEdit.Design.RepositoryItemRichEditFontSizeEdit();
            this.changeFontColorItem1 = new DevExpress.XtraRichEdit.UI.ChangeFontColorItem();
            this.changeFontBackColorItem1 = new DevExpress.XtraRichEdit.UI.ChangeFontBackColorItem();
            this.toggleFontBoldItem1 = new DevExpress.XtraRichEdit.UI.ToggleFontBoldItem();
            this.toggleFontItalicItem1 = new DevExpress.XtraRichEdit.UI.ToggleFontItalicItem();
            this.toggleFontUnderlineItem1 = new DevExpress.XtraRichEdit.UI.ToggleFontUnderlineItem();
            this.toggleFontDoubleUnderlineItem1 = new DevExpress.XtraRichEdit.UI.ToggleFontDoubleUnderlineItem();
            this.toggleFontStrikeoutItem1 = new DevExpress.XtraRichEdit.UI.ToggleFontStrikeoutItem();
            this.toggleFontDoubleStrikeoutItem1 = new DevExpress.XtraRichEdit.UI.ToggleFontDoubleStrikeoutItem();
            this.toggleFontSuperscriptItem1 = new DevExpress.XtraRichEdit.UI.ToggleFontSuperscriptItem();
            this.toggleFontSubscriptItem1 = new DevExpress.XtraRichEdit.UI.ToggleFontSubscriptItem();
            this.changeTextCaseItem1 = new DevExpress.XtraRichEdit.UI.ChangeTextCaseItem();
            this.makeTextUpperCaseItem1 = new DevExpress.XtraRichEdit.UI.MakeTextUpperCaseItem();
            this.makeTextLowerCaseItem1 = new DevExpress.XtraRichEdit.UI.MakeTextLowerCaseItem();
            this.toggleTextCaseItem1 = new DevExpress.XtraRichEdit.UI.ToggleTextCaseItem();
            this.fontSizeIncreaseItem1 = new DevExpress.XtraRichEdit.UI.FontSizeIncreaseItem();
            this.fontSizeDecreaseItem1 = new DevExpress.XtraRichEdit.UI.FontSizeDecreaseItem();
            this.clearFormattingItem1 = new DevExpress.XtraRichEdit.UI.ClearFormattingItem();
            this.showFontFormItem1 = new DevExpress.XtraRichEdit.UI.ShowFontFormItem();
            this.standaloneBarDockControl1 = new DevExpress.XtraBars.StandaloneBarDockControl();
            this.paragraphBar1 = new DevExpress.XtraRichEdit.UI.ParagraphBar();
            this.toggleParagraphAlignmentLeftItem1 = new DevExpress.XtraRichEdit.UI.ToggleParagraphAlignmentLeftItem();
            this.toggleParagraphAlignmentCenterItem1 = new DevExpress.XtraRichEdit.UI.ToggleParagraphAlignmentCenterItem();
            this.toggleParagraphAlignmentRightItem1 = new DevExpress.XtraRichEdit.UI.ToggleParagraphAlignmentRightItem();
            this.toggleParagraphAlignmentJustifyItem1 = new DevExpress.XtraRichEdit.UI.ToggleParagraphAlignmentJustifyItem();
            this.changeParagraphLineSpacingItem1 = new DevExpress.XtraRichEdit.UI.ChangeParagraphLineSpacingItem();
            this.setSingleParagraphSpacingItem1 = new DevExpress.XtraRichEdit.UI.SetSingleParagraphSpacingItem();
            this.setSesquialteralParagraphSpacingItem1 = new DevExpress.XtraRichEdit.UI.SetSesquialteralParagraphSpacingItem();
            this.setDoubleParagraphSpacingItem1 = new DevExpress.XtraRichEdit.UI.SetDoubleParagraphSpacingItem();
            this.showLineSpacingFormItem1 = new DevExpress.XtraRichEdit.UI.ShowLineSpacingFormItem();
            this.addSpacingBeforeParagraphItem1 = new DevExpress.XtraRichEdit.UI.AddSpacingBeforeParagraphItem();
            this.removeSpacingBeforeParagraphItem1 = new DevExpress.XtraRichEdit.UI.RemoveSpacingBeforeParagraphItem();
            this.addSpacingAfterParagraphItem1 = new DevExpress.XtraRichEdit.UI.AddSpacingAfterParagraphItem();
            this.removeSpacingAfterParagraphItem1 = new DevExpress.XtraRichEdit.UI.RemoveSpacingAfterParagraphItem();
            this.toggleBulletedListItem1 = new DevExpress.XtraRichEdit.UI.ToggleBulletedListItem();
            this.toggleNumberingListItem1 = new DevExpress.XtraRichEdit.UI.ToggleNumberingListItem();
            this.toggleMultiLevelListItem1 = new DevExpress.XtraRichEdit.UI.ToggleMultiLevelListItem();
            this.decreaseIndentItem1 = new DevExpress.XtraRichEdit.UI.DecreaseIndentItem();
            this.ıncreaseIndentItem1 = new DevExpress.XtraRichEdit.UI.IncreaseIndentItem();
            this.toggleShowWhitespaceItem1 = new DevExpress.XtraRichEdit.UI.ToggleShowWhitespaceItem();
            this.showParagraphFormItem1 = new DevExpress.XtraRichEdit.UI.ShowParagraphFormItem();
            this.editingBar1 = new DevExpress.XtraRichEdit.UI.EditingBar();
            this.findItem1 = new DevExpress.XtraRichEdit.UI.FindItem();
            this.replaceItem1 = new DevExpress.XtraRichEdit.UI.ReplaceItem();
            this.pagesBar1 = new DevExpress.XtraRichEdit.UI.PagesBar();
            this.ınsertPageBreakItem1 = new DevExpress.XtraRichEdit.UI.InsertPageBreakItem();
            this.tablesBar1 = new DevExpress.XtraRichEdit.UI.TablesBar();
            this.ınsertTableItem1 = new DevExpress.XtraRichEdit.UI.InsertTableItem();
            this.ıllustrationsBar1 = new DevExpress.XtraRichEdit.UI.IllustrationsBar();
            this.ınsertPictureItem1 = new DevExpress.XtraRichEdit.UI.InsertPictureItem();
            this.headerFooterBar1 = new DevExpress.XtraRichEdit.UI.HeaderFooterBar();
            this.editPageFooterItem1 = new DevExpress.XtraRichEdit.UI.EditPageFooterItem();
            this.ınsertPageNumberItem1 = new DevExpress.XtraRichEdit.UI.InsertPageNumberItem();
            this.ınsertPageCountItem1 = new DevExpress.XtraRichEdit.UI.InsertPageCountItem();
            this.symbolsBar1 = new DevExpress.XtraRichEdit.UI.SymbolsBar();
            this.ınsertSymbolItem1 = new DevExpress.XtraRichEdit.UI.InsertSymbolItem();
            this.barDockControlTop = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlBottom = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlLeft = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlRight = new DevExpress.XtraBars.BarDockControl();
            this.cutItem1 = new DevExpress.XtraRichEdit.UI.CutItem();
            this.copyItem1 = new DevExpress.XtraRichEdit.UI.CopyItem();
            this.pasteItem1 = new DevExpress.XtraRichEdit.UI.PasteItem();
            this.pasteSpecialItem1 = new DevExpress.XtraRichEdit.UI.PasteSpecialItem();
            this.changeStyleItem1 = new DevExpress.XtraRichEdit.UI.ChangeStyleItem();
            this.repositoryItemRichEditStyleEdit1 = new DevExpress.XtraRichEdit.Design.RepositoryItemRichEditStyleEdit();
            this.fileNewItem1 = new DevExpress.XtraRichEdit.UI.FileNewItem();
            this.fileOpenItem1 = new DevExpress.XtraRichEdit.UI.FileOpenItem();
            this.fileSaveItem1 = new DevExpress.XtraRichEdit.UI.FileSaveItem();
            this.fileSaveAsItem1 = new DevExpress.XtraRichEdit.UI.FileSaveAsItem();
            this.quickPrintItem1 = new DevExpress.XtraRichEdit.UI.QuickPrintItem();
            this.printItem1 = new DevExpress.XtraRichEdit.UI.PrintItem();
            this.printPreviewItem1 = new DevExpress.XtraRichEdit.UI.PrintPreviewItem();
            this.undoItem1 = new DevExpress.XtraRichEdit.UI.UndoItem();
            this.redoItem1 = new DevExpress.XtraRichEdit.UI.RedoItem();
            this.ınsertBookmarkItem1 = new DevExpress.XtraRichEdit.UI.InsertBookmarkItem();
            this.ınsertHyperlinkItem1 = new DevExpress.XtraRichEdit.UI.InsertHyperlinkItem();
            this.editPageHeaderItem1 = new DevExpress.XtraRichEdit.UI.EditPageHeaderItem();
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.layoutControlGroup1 = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layoutControlItemToolBar = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem2 = new DevExpress.XtraLayout.LayoutControlItem();
            this.richEditBarController1 = new DevExpress.XtraRichEdit.UI.RichEditBarController();
            ((System.ComponentModel.ISupportInitialize)(this.popupMenu1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).BeginInit();
            this.layoutControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemFontEdit1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemRichEditFontSizeEdit1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemRichEditStyleEdit1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemToolBar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.richEditBarController1)).BeginInit();
            this.SuspendLayout();
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Magenta;
            this.imageList1.Images.SetKeyName(0, "");
            this.imageList1.Images.SetKeyName(1, "");
            this.imageList1.Images.SetKeyName(2, "");
            this.imageList1.Images.SetKeyName(3, "");
            this.imageList1.Images.SetKeyName(4, "");
            this.imageList1.Images.SetKeyName(5, "");
            this.imageList1.Images.SetKeyName(6, "");
            this.imageList1.Images.SetKeyName(7, "");
            this.imageList1.Images.SetKeyName(8, "");
            this.imageList1.Images.SetKeyName(9, "");
            this.imageList1.Images.SetKeyName(10, "");
            this.imageList1.Images.SetKeyName(11, "");
            this.imageList1.Images.SetKeyName(12, "");
            this.imageList1.Images.SetKeyName(13, "");
            this.imageList1.Images.SetKeyName(14, "");
            this.imageList1.Images.SetKeyName(15, "");
            this.imageList1.Images.SetKeyName(16, "");
            this.imageList1.Images.SetKeyName(17, "");
            this.imageList1.Images.SetKeyName(18, "");
            this.imageList1.Images.SetKeyName(19, "");
            this.imageList1.Images.SetKeyName(20, "");
            this.imageList1.Images.SetKeyName(21, "");
            this.imageList1.Images.SetKeyName(22, "");
            // 
            // popupMenu1
            // 
            this.popupMenu1.Manager = this.barManager1;
            this.popupMenu1.MenuBarWidth = 20;
            this.popupMenu1.Name = "popupMenu1";
            // 
            // layoutControl1
            // 
            this.layoutControl1.Controls.Add(this.richEditControl1);
            this.layoutControl1.Controls.Add(this.panelControl1);
            this.layoutControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutControl1.Location = new System.Drawing.Point(0, 0);
            this.layoutControl1.Name = "layoutControl1";
            this.layoutControl1.Root = this.layoutControlGroup1;
            this.layoutControl1.Size = new System.Drawing.Size(554, 369);
            this.layoutControl1.TabIndex = 0;
            this.layoutControl1.Text = "layoutControl1";
            // 
            // richEditControl1
            // 
            this.richEditControl1.Location = new System.Drawing.Point(2, 36);
            this.richEditControl1.MenuManager = this.barManager1;
            this.richEditControl1.Name = "richEditControl1";
            this.richEditControl1.Size = new System.Drawing.Size(550, 331);
            this.richEditControl1.TabIndex = 5;
            this.richEditControl1.Text = "richEditControl";
            this.richEditControl1.PopupMenuShowing += new DevExpress.XtraRichEdit.PopupMenuShowingEventHandler(this.richEditControl1_PopupMenuShowing);
            this.richEditControl1.MhtTextChanged += new System.EventHandler(this.MhtTextChanged);
            this.richEditControl1.ModifiedChanged += new System.EventHandler(this.ModifiedChanged);
            this.richEditControl1.TextChanged += new System.EventHandler(this.TextChanged);
            // 
            // barManager1
            // 
            this.barManager1.Bars.AddRange(new DevExpress.XtraBars.Bar[] {
            this.fontBar1,
            this.paragraphBar1,
            this.editingBar1,
            this.pagesBar1,
            this.tablesBar1,
            this.ıllustrationsBar1,
            this.headerFooterBar1,
            this.symbolsBar1});
            this.barManager1.DockControls.Add(this.barDockControlTop);
            this.barManager1.DockControls.Add(this.barDockControlBottom);
            this.barManager1.DockControls.Add(this.barDockControlLeft);
            this.barManager1.DockControls.Add(this.barDockControlRight);
            this.barManager1.DockControls.Add(this.standaloneBarDockControl1);
            this.barManager1.Form = this;
            this.barManager1.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
            this.cutItem1,
            this.copyItem1,
            this.pasteItem1,
            this.pasteSpecialItem1,
            this.changeFontNameItem1,
            this.changeFontSizeItem1,
            this.changeFontColorItem1,
            this.changeFontBackColorItem1,
            this.toggleFontBoldItem1,
            this.toggleFontItalicItem1,
            this.toggleFontUnderlineItem1,
            this.toggleFontDoubleUnderlineItem1,
            this.toggleFontStrikeoutItem1,
            this.toggleFontDoubleStrikeoutItem1,
            this.toggleFontSuperscriptItem1,
            this.toggleFontSubscriptItem1,
            this.changeTextCaseItem1,
            this.makeTextUpperCaseItem1,
            this.makeTextLowerCaseItem1,
            this.toggleTextCaseItem1,
            this.fontSizeIncreaseItem1,
            this.fontSizeDecreaseItem1,
            this.clearFormattingItem1,
            this.showFontFormItem1,
            this.toggleParagraphAlignmentLeftItem1,
            this.toggleParagraphAlignmentCenterItem1,
            this.toggleParagraphAlignmentRightItem1,
            this.toggleParagraphAlignmentJustifyItem1,
            this.changeParagraphLineSpacingItem1,
            this.setSingleParagraphSpacingItem1,
            this.setSesquialteralParagraphSpacingItem1,
            this.setDoubleParagraphSpacingItem1,
            this.showLineSpacingFormItem1,
            this.addSpacingBeforeParagraphItem1,
            this.removeSpacingBeforeParagraphItem1,
            this.addSpacingAfterParagraphItem1,
            this.removeSpacingAfterParagraphItem1,
            this.toggleBulletedListItem1,
            this.toggleNumberingListItem1,
            this.toggleMultiLevelListItem1,
            this.decreaseIndentItem1,
            this.ıncreaseIndentItem1,
            this.toggleShowWhitespaceItem1,
            this.showParagraphFormItem1,
            this.changeStyleItem1,
            this.findItem1,
            this.replaceItem1,
            this.fileNewItem1,
            this.fileOpenItem1,
            this.fileSaveItem1,
            this.fileSaveAsItem1,
            this.quickPrintItem1,
            this.printItem1,
            this.printPreviewItem1,
            this.undoItem1,
            this.redoItem1,
            this.ınsertPageBreakItem1,
            this.ınsertTableItem1,
            this.ınsertPictureItem1,
            this.ınsertBookmarkItem1,
            this.ınsertHyperlinkItem1,
            this.editPageHeaderItem1,
            this.editPageFooterItem1,
            this.ınsertPageNumberItem1,
            this.ınsertPageCountItem1,
            this.ınsertSymbolItem1});
            this.barManager1.MaxItemId = 66;
            this.barManager1.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repositoryItemFontEdit1,
            this.repositoryItemRichEditFontSizeEdit1,
            this.repositoryItemRichEditStyleEdit1});
            // 
            // fontBar1
            // 
            this.fontBar1.Control = this.richEditControl1;
            this.fontBar1.DockCol = 0;
            this.fontBar1.DockRow = 0;
            this.fontBar1.DockStyle = DevExpress.XtraBars.BarDockStyle.Standalone;
            this.fontBar1.FloatLocation = new System.Drawing.Point(531, 164);
            this.fontBar1.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.changeFontNameItem1),
            new DevExpress.XtraBars.LinkPersistInfo(this.changeFontSizeItem1),
            new DevExpress.XtraBars.LinkPersistInfo(this.changeFontColorItem1),
            new DevExpress.XtraBars.LinkPersistInfo(this.changeFontBackColorItem1),
            new DevExpress.XtraBars.LinkPersistInfo(this.toggleFontBoldItem1),
            new DevExpress.XtraBars.LinkPersistInfo(this.toggleFontItalicItem1),
            new DevExpress.XtraBars.LinkPersistInfo(this.toggleFontUnderlineItem1),
            new DevExpress.XtraBars.LinkPersistInfo(this.toggleFontDoubleUnderlineItem1),
            new DevExpress.XtraBars.LinkPersistInfo(this.toggleFontStrikeoutItem1),
            new DevExpress.XtraBars.LinkPersistInfo(this.toggleFontDoubleStrikeoutItem1),
            new DevExpress.XtraBars.LinkPersistInfo(this.toggleFontSuperscriptItem1),
            new DevExpress.XtraBars.LinkPersistInfo(this.toggleFontSubscriptItem1),
            new DevExpress.XtraBars.LinkPersistInfo(this.changeTextCaseItem1),
            new DevExpress.XtraBars.LinkPersistInfo(this.fontSizeIncreaseItem1),
            new DevExpress.XtraBars.LinkPersistInfo(this.fontSizeDecreaseItem1),
            new DevExpress.XtraBars.LinkPersistInfo(this.clearFormattingItem1),
            new DevExpress.XtraBars.LinkPersistInfo(this.showFontFormItem1)});
            this.fontBar1.Offset = 307;
            this.fontBar1.StandaloneBarDockControl = this.standaloneBarDockControl1;
            // 
            // changeFontNameItem1
            // 
            this.changeFontNameItem1.Edit = this.repositoryItemFontEdit1;
            this.changeFontNameItem1.Id = 4;
            this.changeFontNameItem1.ItemAppearance.Normal.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.changeFontNameItem1.ItemAppearance.Normal.Options.UseFont = true;
            this.changeFontNameItem1.Name = "changeFontNameItem1";
            // 
            // repositoryItemFontEdit1
            // 
            this.repositoryItemFontEdit1.AutoHeight = false;
            this.repositoryItemFontEdit1.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repositoryItemFontEdit1.Name = "repositoryItemFontEdit1";
            // 
            // changeFontSizeItem1
            // 
            this.changeFontSizeItem1.Edit = this.repositoryItemRichEditFontSizeEdit1;
            this.changeFontSizeItem1.Id = 5;
            this.changeFontSizeItem1.Name = "changeFontSizeItem1";
            // 
            // repositoryItemRichEditFontSizeEdit1
            // 
            this.repositoryItemRichEditFontSizeEdit1.AutoHeight = false;
            this.repositoryItemRichEditFontSizeEdit1.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repositoryItemRichEditFontSizeEdit1.Control = this.richEditControl1;
            this.repositoryItemRichEditFontSizeEdit1.Name = "repositoryItemRichEditFontSizeEdit1";
            // 
            // changeFontColorItem1
            // 
            this.changeFontColorItem1.Id = 6;
            this.changeFontColorItem1.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("changeFontColorItem1.ImageOptions.Image")));
            this.changeFontColorItem1.ImageOptions.LargeImage = ((System.Drawing.Image)(resources.GetObject("changeFontColorItem1.ImageOptions.LargeImage")));
            this.changeFontColorItem1.Name = "changeFontColorItem1";
            // 
            // changeFontBackColorItem1
            // 
            this.changeFontBackColorItem1.Id = 7;
            this.changeFontBackColorItem1.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("changeFontBackColorItem1.ImageOptions.Image")));
            this.changeFontBackColorItem1.ImageOptions.LargeImage = ((System.Drawing.Image)(resources.GetObject("changeFontBackColorItem1.ImageOptions.LargeImage")));
            this.changeFontBackColorItem1.Name = "changeFontBackColorItem1";
            // 
            // toggleFontBoldItem1
            // 
            this.toggleFontBoldItem1.Id = 8;
            this.toggleFontBoldItem1.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("toggleFontBoldItem1.ImageOptions.Image")));
            this.toggleFontBoldItem1.ImageOptions.LargeImage = ((System.Drawing.Image)(resources.GetObject("toggleFontBoldItem1.ImageOptions.LargeImage")));
            this.toggleFontBoldItem1.Name = "toggleFontBoldItem1";
            // 
            // toggleFontItalicItem1
            // 
            this.toggleFontItalicItem1.Id = 9;
            this.toggleFontItalicItem1.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("toggleFontItalicItem1.ImageOptions.Image")));
            this.toggleFontItalicItem1.ImageOptions.LargeImage = ((System.Drawing.Image)(resources.GetObject("toggleFontItalicItem1.ImageOptions.LargeImage")));
            this.toggleFontItalicItem1.Name = "toggleFontItalicItem1";
            // 
            // toggleFontUnderlineItem1
            // 
            this.toggleFontUnderlineItem1.Id = 10;
            this.toggleFontUnderlineItem1.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("toggleFontUnderlineItem1.ImageOptions.Image")));
            this.toggleFontUnderlineItem1.ImageOptions.LargeImage = ((System.Drawing.Image)(resources.GetObject("toggleFontUnderlineItem1.ImageOptions.LargeImage")));
            this.toggleFontUnderlineItem1.Name = "toggleFontUnderlineItem1";
            // 
            // toggleFontDoubleUnderlineItem1
            // 
            this.toggleFontDoubleUnderlineItem1.Id = 11;
            this.toggleFontDoubleUnderlineItem1.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("toggleFontDoubleUnderlineItem1.ImageOptions.Image")));
            this.toggleFontDoubleUnderlineItem1.ImageOptions.LargeImage = ((System.Drawing.Image)(resources.GetObject("toggleFontDoubleUnderlineItem1.ImageOptions.LargeImage")));
            this.toggleFontDoubleUnderlineItem1.Name = "toggleFontDoubleUnderlineItem1";
            // 
            // toggleFontStrikeoutItem1
            // 
            this.toggleFontStrikeoutItem1.Id = 12;
            this.toggleFontStrikeoutItem1.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("toggleFontStrikeoutItem1.ImageOptions.Image")));
            this.toggleFontStrikeoutItem1.ImageOptions.LargeImage = ((System.Drawing.Image)(resources.GetObject("toggleFontStrikeoutItem1.ImageOptions.LargeImage")));
            this.toggleFontStrikeoutItem1.Name = "toggleFontStrikeoutItem1";
            // 
            // toggleFontDoubleStrikeoutItem1
            // 
            this.toggleFontDoubleStrikeoutItem1.Id = 13;
            this.toggleFontDoubleStrikeoutItem1.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("toggleFontDoubleStrikeoutItem1.ImageOptions.Image")));
            this.toggleFontDoubleStrikeoutItem1.ImageOptions.LargeImage = ((System.Drawing.Image)(resources.GetObject("toggleFontDoubleStrikeoutItem1.ImageOptions.LargeImage")));
            this.toggleFontDoubleStrikeoutItem1.Name = "toggleFontDoubleStrikeoutItem1";
            // 
            // toggleFontSuperscriptItem1
            // 
            this.toggleFontSuperscriptItem1.Id = 14;
            this.toggleFontSuperscriptItem1.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("toggleFontSuperscriptItem1.ImageOptions.Image")));
            this.toggleFontSuperscriptItem1.ImageOptions.LargeImage = ((System.Drawing.Image)(resources.GetObject("toggleFontSuperscriptItem1.ImageOptions.LargeImage")));
            this.toggleFontSuperscriptItem1.Name = "toggleFontSuperscriptItem1";
            // 
            // toggleFontSubscriptItem1
            // 
            this.toggleFontSubscriptItem1.Id = 15;
            this.toggleFontSubscriptItem1.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("toggleFontSubscriptItem1.ImageOptions.Image")));
            this.toggleFontSubscriptItem1.ImageOptions.LargeImage = ((System.Drawing.Image)(resources.GetObject("toggleFontSubscriptItem1.ImageOptions.LargeImage")));
            this.toggleFontSubscriptItem1.Name = "toggleFontSubscriptItem1";
            // 
            // changeTextCaseItem1
            // 
            this.changeTextCaseItem1.Id = 16;
            this.changeTextCaseItem1.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("changeTextCaseItem1.ImageOptions.Image")));
            this.changeTextCaseItem1.ImageOptions.LargeImage = ((System.Drawing.Image)(resources.GetObject("changeTextCaseItem1.ImageOptions.LargeImage")));
            this.changeTextCaseItem1.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.makeTextUpperCaseItem1),
            new DevExpress.XtraBars.LinkPersistInfo(this.makeTextLowerCaseItem1),
            new DevExpress.XtraBars.LinkPersistInfo(this.toggleTextCaseItem1)});
            this.changeTextCaseItem1.Name = "changeTextCaseItem1";
            // 
            // makeTextUpperCaseItem1
            // 
            this.makeTextUpperCaseItem1.Id = 17;
            this.makeTextUpperCaseItem1.Name = "makeTextUpperCaseItem1";
            // 
            // makeTextLowerCaseItem1
            // 
            this.makeTextLowerCaseItem1.Id = 18;
            this.makeTextLowerCaseItem1.Name = "makeTextLowerCaseItem1";
            // 
            // toggleTextCaseItem1
            // 
            this.toggleTextCaseItem1.Id = 19;
            this.toggleTextCaseItem1.Name = "toggleTextCaseItem1";
            // 
            // fontSizeIncreaseItem1
            // 
            this.fontSizeIncreaseItem1.Id = 20;
            this.fontSizeIncreaseItem1.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("fontSizeIncreaseItem1.ImageOptions.Image")));
            this.fontSizeIncreaseItem1.ImageOptions.LargeImage = ((System.Drawing.Image)(resources.GetObject("fontSizeIncreaseItem1.ImageOptions.LargeImage")));
            this.fontSizeIncreaseItem1.Name = "fontSizeIncreaseItem1";
            // 
            // fontSizeDecreaseItem1
            // 
            this.fontSizeDecreaseItem1.Id = 21;
            this.fontSizeDecreaseItem1.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("fontSizeDecreaseItem1.ImageOptions.Image")));
            this.fontSizeDecreaseItem1.ImageOptions.LargeImage = ((System.Drawing.Image)(resources.GetObject("fontSizeDecreaseItem1.ImageOptions.LargeImage")));
            this.fontSizeDecreaseItem1.Name = "fontSizeDecreaseItem1";
            // 
            // clearFormattingItem1
            // 
            this.clearFormattingItem1.Id = 22;
            this.clearFormattingItem1.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("clearFormattingItem1.ImageOptions.Image")));
            this.clearFormattingItem1.ImageOptions.LargeImage = ((System.Drawing.Image)(resources.GetObject("clearFormattingItem1.ImageOptions.LargeImage")));
            this.clearFormattingItem1.Name = "clearFormattingItem1";
            // 
            // showFontFormItem1
            // 
            this.showFontFormItem1.Id = 23;
            this.showFontFormItem1.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("showFontFormItem1.ImageOptions.Image")));
            this.showFontFormItem1.ImageOptions.LargeImage = ((System.Drawing.Image)(resources.GetObject("showFontFormItem1.ImageOptions.LargeImage")));
            this.showFontFormItem1.Name = "showFontFormItem1";
            // 
            // standaloneBarDockControl1
            // 
            this.standaloneBarDockControl1.CausesValidation = false;
            this.standaloneBarDockControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.standaloneBarDockControl1.Location = new System.Drawing.Point(2, 2);
            this.standaloneBarDockControl1.Manager = this.barManager1;
            this.standaloneBarDockControl1.Name = "standaloneBarDockControl1";
            this.standaloneBarDockControl1.Size = new System.Drawing.Size(546, 26);
            this.standaloneBarDockControl1.Text = "standaloneBarDockControl1";
            // 
            // paragraphBar1
            // 
            this.paragraphBar1.Control = this.richEditControl1;
            this.paragraphBar1.DockCol = 6;
            this.paragraphBar1.DockRow = 0;
            this.paragraphBar1.DockStyle = DevExpress.XtraBars.BarDockStyle.Standalone;
            this.paragraphBar1.FloatLocation = new System.Drawing.Point(383, 223);
            this.paragraphBar1.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.toggleParagraphAlignmentLeftItem1),
            new DevExpress.XtraBars.LinkPersistInfo(this.toggleParagraphAlignmentCenterItem1),
            new DevExpress.XtraBars.LinkPersistInfo(this.toggleParagraphAlignmentRightItem1),
            new DevExpress.XtraBars.LinkPersistInfo(this.toggleParagraphAlignmentJustifyItem1),
            new DevExpress.XtraBars.LinkPersistInfo(this.changeParagraphLineSpacingItem1),
            new DevExpress.XtraBars.LinkPersistInfo(this.toggleBulletedListItem1),
            new DevExpress.XtraBars.LinkPersistInfo(this.toggleNumberingListItem1),
            new DevExpress.XtraBars.LinkPersistInfo(this.toggleMultiLevelListItem1),
            new DevExpress.XtraBars.LinkPersistInfo(this.decreaseIndentItem1),
            new DevExpress.XtraBars.LinkPersistInfo(this.ıncreaseIndentItem1),
            new DevExpress.XtraBars.LinkPersistInfo(this.toggleShowWhitespaceItem1),
            new DevExpress.XtraBars.LinkPersistInfo(this.showParagraphFormItem1)});
            this.paragraphBar1.Offset = 412;
            this.paragraphBar1.StandaloneBarDockControl = this.standaloneBarDockControl1;
            // 
            // toggleParagraphAlignmentLeftItem1
            // 
            this.toggleParagraphAlignmentLeftItem1.Id = 24;
            this.toggleParagraphAlignmentLeftItem1.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("toggleParagraphAlignmentLeftItem1.ImageOptions.Image")));
            this.toggleParagraphAlignmentLeftItem1.ImageOptions.LargeImage = ((System.Drawing.Image)(resources.GetObject("toggleParagraphAlignmentLeftItem1.ImageOptions.LargeImage")));
            this.toggleParagraphAlignmentLeftItem1.Name = "toggleParagraphAlignmentLeftItem1";
            // 
            // toggleParagraphAlignmentCenterItem1
            // 
            this.toggleParagraphAlignmentCenterItem1.Id = 25;
            this.toggleParagraphAlignmentCenterItem1.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("toggleParagraphAlignmentCenterItem1.ImageOptions.Image")));
            this.toggleParagraphAlignmentCenterItem1.ImageOptions.LargeImage = ((System.Drawing.Image)(resources.GetObject("toggleParagraphAlignmentCenterItem1.ImageOptions.LargeImage")));
            this.toggleParagraphAlignmentCenterItem1.Name = "toggleParagraphAlignmentCenterItem1";
            // 
            // toggleParagraphAlignmentRightItem1
            // 
            this.toggleParagraphAlignmentRightItem1.Id = 26;
            this.toggleParagraphAlignmentRightItem1.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("toggleParagraphAlignmentRightItem1.ImageOptions.Image")));
            this.toggleParagraphAlignmentRightItem1.ImageOptions.LargeImage = ((System.Drawing.Image)(resources.GetObject("toggleParagraphAlignmentRightItem1.ImageOptions.LargeImage")));
            this.toggleParagraphAlignmentRightItem1.Name = "toggleParagraphAlignmentRightItem1";
            // 
            // toggleParagraphAlignmentJustifyItem1
            // 
            this.toggleParagraphAlignmentJustifyItem1.Id = 27;
            this.toggleParagraphAlignmentJustifyItem1.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("toggleParagraphAlignmentJustifyItem1.ImageOptions.Image")));
            this.toggleParagraphAlignmentJustifyItem1.ImageOptions.LargeImage = ((System.Drawing.Image)(resources.GetObject("toggleParagraphAlignmentJustifyItem1.ImageOptions.LargeImage")));
            this.toggleParagraphAlignmentJustifyItem1.Name = "toggleParagraphAlignmentJustifyItem1";
            // 
            // changeParagraphLineSpacingItem1
            // 
            this.changeParagraphLineSpacingItem1.Id = 28;
            this.changeParagraphLineSpacingItem1.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("changeParagraphLineSpacingItem1.ImageOptions.Image")));
            this.changeParagraphLineSpacingItem1.ImageOptions.LargeImage = ((System.Drawing.Image)(resources.GetObject("changeParagraphLineSpacingItem1.ImageOptions.LargeImage")));
            this.changeParagraphLineSpacingItem1.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.setSingleParagraphSpacingItem1),
            new DevExpress.XtraBars.LinkPersistInfo(this.setSesquialteralParagraphSpacingItem1),
            new DevExpress.XtraBars.LinkPersistInfo(this.setDoubleParagraphSpacingItem1),
            new DevExpress.XtraBars.LinkPersistInfo(this.showLineSpacingFormItem1),
            new DevExpress.XtraBars.LinkPersistInfo(this.addSpacingBeforeParagraphItem1),
            new DevExpress.XtraBars.LinkPersistInfo(this.removeSpacingBeforeParagraphItem1),
            new DevExpress.XtraBars.LinkPersistInfo(this.addSpacingAfterParagraphItem1),
            new DevExpress.XtraBars.LinkPersistInfo(this.removeSpacingAfterParagraphItem1)});
            this.changeParagraphLineSpacingItem1.Name = "changeParagraphLineSpacingItem1";
            // 
            // setSingleParagraphSpacingItem1
            // 
            this.setSingleParagraphSpacingItem1.Id = 29;
            this.setSingleParagraphSpacingItem1.Name = "setSingleParagraphSpacingItem1";
            // 
            // setSesquialteralParagraphSpacingItem1
            // 
            this.setSesquialteralParagraphSpacingItem1.Id = 30;
            this.setSesquialteralParagraphSpacingItem1.Name = "setSesquialteralParagraphSpacingItem1";
            // 
            // setDoubleParagraphSpacingItem1
            // 
            this.setDoubleParagraphSpacingItem1.Id = 31;
            this.setDoubleParagraphSpacingItem1.Name = "setDoubleParagraphSpacingItem1";
            // 
            // showLineSpacingFormItem1
            // 
            this.showLineSpacingFormItem1.Id = 32;
            this.showLineSpacingFormItem1.Name = "showLineSpacingFormItem1";
            // 
            // addSpacingBeforeParagraphItem1
            // 
            this.addSpacingBeforeParagraphItem1.Id = 33;
            this.addSpacingBeforeParagraphItem1.Name = "addSpacingBeforeParagraphItem1";
            // 
            // removeSpacingBeforeParagraphItem1
            // 
            this.removeSpacingBeforeParagraphItem1.Id = 34;
            this.removeSpacingBeforeParagraphItem1.Name = "removeSpacingBeforeParagraphItem1";
            // 
            // addSpacingAfterParagraphItem1
            // 
            this.addSpacingAfterParagraphItem1.Id = 35;
            this.addSpacingAfterParagraphItem1.Name = "addSpacingAfterParagraphItem1";
            // 
            // removeSpacingAfterParagraphItem1
            // 
            this.removeSpacingAfterParagraphItem1.Id = 36;
            this.removeSpacingAfterParagraphItem1.Name = "removeSpacingAfterParagraphItem1";
            // 
            // toggleBulletedListItem1
            // 
            this.toggleBulletedListItem1.Id = 37;
            this.toggleBulletedListItem1.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("toggleBulletedListItem1.ImageOptions.Image")));
            this.toggleBulletedListItem1.ImageOptions.LargeImage = ((System.Drawing.Image)(resources.GetObject("toggleBulletedListItem1.ImageOptions.LargeImage")));
            this.toggleBulletedListItem1.Name = "toggleBulletedListItem1";
            // 
            // toggleNumberingListItem1
            // 
            this.toggleNumberingListItem1.Id = 38;
            this.toggleNumberingListItem1.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("toggleNumberingListItem1.ImageOptions.Image")));
            this.toggleNumberingListItem1.ImageOptions.LargeImage = ((System.Drawing.Image)(resources.GetObject("toggleNumberingListItem1.ImageOptions.LargeImage")));
            this.toggleNumberingListItem1.Name = "toggleNumberingListItem1";
            // 
            // toggleMultiLevelListItem1
            // 
            this.toggleMultiLevelListItem1.Id = 39;
            this.toggleMultiLevelListItem1.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("toggleMultiLevelListItem1.ImageOptions.Image")));
            this.toggleMultiLevelListItem1.ImageOptions.LargeImage = ((System.Drawing.Image)(resources.GetObject("toggleMultiLevelListItem1.ImageOptions.LargeImage")));
            this.toggleMultiLevelListItem1.Name = "toggleMultiLevelListItem1";
            // 
            // decreaseIndentItem1
            // 
            this.decreaseIndentItem1.Id = 40;
            this.decreaseIndentItem1.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("decreaseIndentItem1.ImageOptions.Image")));
            this.decreaseIndentItem1.ImageOptions.LargeImage = ((System.Drawing.Image)(resources.GetObject("decreaseIndentItem1.ImageOptions.LargeImage")));
            this.decreaseIndentItem1.Name = "decreaseIndentItem1";
            // 
            // ıncreaseIndentItem1
            // 
            this.ıncreaseIndentItem1.Id = 41;
            this.ıncreaseIndentItem1.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("ıncreaseIndentItem1.ImageOptions.Image")));
            this.ıncreaseIndentItem1.ImageOptions.LargeImage = ((System.Drawing.Image)(resources.GetObject("ıncreaseIndentItem1.ImageOptions.LargeImage")));
            this.ıncreaseIndentItem1.Name = "ıncreaseIndentItem1";
            // 
            // toggleShowWhitespaceItem1
            // 
            this.toggleShowWhitespaceItem1.Id = 42;
            this.toggleShowWhitespaceItem1.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("toggleShowWhitespaceItem1.ImageOptions.Image")));
            this.toggleShowWhitespaceItem1.ImageOptions.LargeImage = ((System.Drawing.Image)(resources.GetObject("toggleShowWhitespaceItem1.ImageOptions.LargeImage")));
            this.toggleShowWhitespaceItem1.Name = "toggleShowWhitespaceItem1";
            // 
            // showParagraphFormItem1
            // 
            this.showParagraphFormItem1.Id = 43;
            this.showParagraphFormItem1.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("showParagraphFormItem1.ImageOptions.Image")));
            this.showParagraphFormItem1.ImageOptions.LargeImage = ((System.Drawing.Image)(resources.GetObject("showParagraphFormItem1.ImageOptions.LargeImage")));
            this.showParagraphFormItem1.Name = "showParagraphFormItem1";
            // 
            // editingBar1
            // 
            this.editingBar1.Control = this.richEditControl1;
            this.editingBar1.DockCol = 7;
            this.editingBar1.DockRow = 0;
            this.editingBar1.DockStyle = DevExpress.XtraBars.BarDockStyle.Standalone;
            this.editingBar1.FloatLocation = new System.Drawing.Point(305, 265);
            this.editingBar1.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.findItem1),
            new DevExpress.XtraBars.LinkPersistInfo(this.replaceItem1)});
            this.editingBar1.Offset = 476;
            this.editingBar1.StandaloneBarDockControl = this.standaloneBarDockControl1;
            // 
            // findItem1
            // 
            this.findItem1.Id = 44;
            this.findItem1.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("findItem1.ImageOptions.Image")));
            this.findItem1.ImageOptions.LargeImage = ((System.Drawing.Image)(resources.GetObject("findItem1.ImageOptions.LargeImage")));
            this.findItem1.Name = "findItem1";
            // 
            // replaceItem1
            // 
            this.replaceItem1.Id = 45;
            this.replaceItem1.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("replaceItem1.ImageOptions.Image")));
            this.replaceItem1.ImageOptions.LargeImage = ((System.Drawing.Image)(resources.GetObject("replaceItem1.ImageOptions.LargeImage")));
            this.replaceItem1.Name = "replaceItem1";
            // 
            // pagesBar1
            // 
            this.pagesBar1.Control = this.richEditControl1;
            this.pagesBar1.DockCol = 2;
            this.pagesBar1.DockRow = 0;
            this.pagesBar1.DockStyle = DevExpress.XtraBars.BarDockStyle.Standalone;
            this.pagesBar1.FloatLocation = new System.Drawing.Point(539, 249);
            this.pagesBar1.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.ınsertPageBreakItem1)});
            this.pagesBar1.Offset = 317;
            this.pagesBar1.StandaloneBarDockControl = this.standaloneBarDockControl1;
            // 
            // ınsertPageBreakItem1
            // 
            this.ınsertPageBreakItem1.Id = 47;
            this.ınsertPageBreakItem1.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("ınsertPageBreakItem1.ImageOptions.Image")));
            this.ınsertPageBreakItem1.ImageOptions.LargeImage = ((System.Drawing.Image)(resources.GetObject("ınsertPageBreakItem1.ImageOptions.LargeImage")));
            this.ınsertPageBreakItem1.Name = "ınsertPageBreakItem1";
            // 
            // tablesBar1
            // 
            this.tablesBar1.Control = this.richEditControl1;
            this.tablesBar1.DockCol = 5;
            this.tablesBar1.DockRow = 0;
            this.tablesBar1.DockStyle = DevExpress.XtraBars.BarDockStyle.Standalone;
            this.tablesBar1.FloatLocation = new System.Drawing.Point(438, 272);
            this.tablesBar1.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.ınsertTableItem1)});
            this.tablesBar1.Offset = 494;
            this.tablesBar1.StandaloneBarDockControl = this.standaloneBarDockControl1;
            // 
            // ınsertTableItem1
            // 
            this.ınsertTableItem1.Id = 48;
            this.ınsertTableItem1.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("ınsertTableItem1.ImageOptions.Image")));
            this.ınsertTableItem1.ImageOptions.LargeImage = ((System.Drawing.Image)(resources.GetObject("ınsertTableItem1.ImageOptions.LargeImage")));
            this.ınsertTableItem1.Name = "ınsertTableItem1";
            // 
            // ıllustrationsBar1
            // 
            this.ıllustrationsBar1.Control = this.richEditControl1;
            this.ıllustrationsBar1.DockCol = 3;
            this.ıllustrationsBar1.DockRow = 0;
            this.ıllustrationsBar1.DockStyle = DevExpress.XtraBars.BarDockStyle.Standalone;
            this.ıllustrationsBar1.FloatLocation = new System.Drawing.Point(574, 216);
            this.ıllustrationsBar1.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.ınsertPictureItem1)});
            this.ıllustrationsBar1.Offset = 350;
            this.ıllustrationsBar1.StandaloneBarDockControl = this.standaloneBarDockControl1;
            // 
            // ınsertPictureItem1
            // 
            this.ınsertPictureItem1.Id = 49;
            this.ınsertPictureItem1.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("ınsertPictureItem1.ImageOptions.Image")));
            this.ınsertPictureItem1.ImageOptions.LargeImage = ((System.Drawing.Image)(resources.GetObject("ınsertPictureItem1.ImageOptions.LargeImage")));
            this.ınsertPictureItem1.Name = "ınsertPictureItem1";
            // 
            // headerFooterBar1
            // 
            this.headerFooterBar1.Control = this.richEditControl1;
            this.headerFooterBar1.DockCol = 1;
            this.headerFooterBar1.DockRow = 0;
            this.headerFooterBar1.DockStyle = DevExpress.XtraBars.BarDockStyle.Standalone;
            this.headerFooterBar1.FloatLocation = new System.Drawing.Point(464, 163);
            this.headerFooterBar1.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.editPageFooterItem1),
            new DevExpress.XtraBars.LinkPersistInfo(this.ınsertPageNumberItem1),
            new DevExpress.XtraBars.LinkPersistInfo(this.ınsertPageCountItem1)});
            this.headerFooterBar1.Offset = 228;
            this.headerFooterBar1.StandaloneBarDockControl = this.standaloneBarDockControl1;
            // 
            // editPageFooterItem1
            // 
            this.editPageFooterItem1.Id = 50;
            this.editPageFooterItem1.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("editPageFooterItem1.ImageOptions.Image")));
            this.editPageFooterItem1.ImageOptions.LargeImage = ((System.Drawing.Image)(resources.GetObject("editPageFooterItem1.ImageOptions.LargeImage")));
            this.editPageFooterItem1.Name = "editPageFooterItem1";
            // 
            // ınsertPageNumberItem1
            // 
            this.ınsertPageNumberItem1.Id = 51;
            this.ınsertPageNumberItem1.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("ınsertPageNumberItem1.ImageOptions.Image")));
            this.ınsertPageNumberItem1.ImageOptions.LargeImage = ((System.Drawing.Image)(resources.GetObject("ınsertPageNumberItem1.ImageOptions.LargeImage")));
            this.ınsertPageNumberItem1.Name = "ınsertPageNumberItem1";
            // 
            // ınsertPageCountItem1
            // 
            this.ınsertPageCountItem1.Id = 52;
            this.ınsertPageCountItem1.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("ınsertPageCountItem1.ImageOptions.Image")));
            this.ınsertPageCountItem1.ImageOptions.LargeImage = ((System.Drawing.Image)(resources.GetObject("ınsertPageCountItem1.ImageOptions.LargeImage")));
            this.ınsertPageCountItem1.Name = "ınsertPageCountItem1";
            // 
            // symbolsBar1
            // 
            this.symbolsBar1.Control = this.richEditControl1;
            this.symbolsBar1.DockCol = 4;
            this.symbolsBar1.DockRow = 0;
            this.symbolsBar1.DockStyle = DevExpress.XtraBars.BarDockStyle.Standalone;
            this.symbolsBar1.FloatLocation = new System.Drawing.Point(621, 306);
            this.symbolsBar1.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.ınsertSymbolItem1)});
            this.symbolsBar1.Offset = 403;
            this.symbolsBar1.StandaloneBarDockControl = this.standaloneBarDockControl1;
            // 
            // ınsertSymbolItem1
            // 
            this.ınsertSymbolItem1.Id = 53;
            this.ınsertSymbolItem1.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("ınsertSymbolItem1.ImageOptions.Image")));
            this.ınsertSymbolItem1.ImageOptions.LargeImage = ((System.Drawing.Image)(resources.GetObject("ınsertSymbolItem1.ImageOptions.LargeImage")));
            this.ınsertSymbolItem1.Name = "ınsertSymbolItem1";
            // 
            // barDockControlTop
            // 
            this.barDockControlTop.CausesValidation = false;
            this.barDockControlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.barDockControlTop.Location = new System.Drawing.Point(0, 0);
            this.barDockControlTop.Manager = this.barManager1;
            this.barDockControlTop.Size = new System.Drawing.Size(554, 0);
            // 
            // barDockControlBottom
            // 
            this.barDockControlBottom.CausesValidation = false;
            this.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.barDockControlBottom.Location = new System.Drawing.Point(0, 369);
            this.barDockControlBottom.Manager = this.barManager1;
            this.barDockControlBottom.Size = new System.Drawing.Size(554, 0);
            // 
            // barDockControlLeft
            // 
            this.barDockControlLeft.CausesValidation = false;
            this.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.barDockControlLeft.Location = new System.Drawing.Point(0, 0);
            this.barDockControlLeft.Manager = this.barManager1;
            this.barDockControlLeft.Size = new System.Drawing.Size(0, 369);
            // 
            // barDockControlRight
            // 
            this.barDockControlRight.CausesValidation = false;
            this.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.barDockControlRight.Location = new System.Drawing.Point(554, 0);
            this.barDockControlRight.Manager = this.barManager1;
            this.barDockControlRight.Size = new System.Drawing.Size(0, 369);
            // 
            // cutItem1
            // 
            this.cutItem1.Id = 0;
            this.cutItem1.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("cutItem1.ImageOptions.Image")));
            this.cutItem1.ImageOptions.LargeImage = ((System.Drawing.Image)(resources.GetObject("cutItem1.ImageOptions.LargeImage")));
            this.cutItem1.Name = "cutItem1";
            // 
            // copyItem1
            // 
            this.copyItem1.Id = 1;
            this.copyItem1.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("copyItem1.ImageOptions.Image")));
            this.copyItem1.ImageOptions.LargeImage = ((System.Drawing.Image)(resources.GetObject("copyItem1.ImageOptions.LargeImage")));
            this.copyItem1.Name = "copyItem1";
            // 
            // pasteItem1
            // 
            this.pasteItem1.Id = 2;
            this.pasteItem1.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("pasteItem1.ImageOptions.Image")));
            this.pasteItem1.ImageOptions.LargeImage = ((System.Drawing.Image)(resources.GetObject("pasteItem1.ImageOptions.LargeImage")));
            this.pasteItem1.Name = "pasteItem1";
            // 
            // pasteSpecialItem1
            // 
            this.pasteSpecialItem1.Id = 3;
            this.pasteSpecialItem1.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("pasteSpecialItem1.ImageOptions.Image")));
            this.pasteSpecialItem1.ImageOptions.LargeImage = ((System.Drawing.Image)(resources.GetObject("pasteSpecialItem1.ImageOptions.LargeImage")));
            this.pasteSpecialItem1.Name = "pasteSpecialItem1";
            // 
            // changeStyleItem1
            // 
            this.changeStyleItem1.Edit = this.repositoryItemRichEditStyleEdit1;
            this.changeStyleItem1.Id = 46;
            this.changeStyleItem1.Name = "changeStyleItem1";
            // 
            // repositoryItemRichEditStyleEdit1
            // 
            this.repositoryItemRichEditStyleEdit1.AutoHeight = false;
            this.repositoryItemRichEditStyleEdit1.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repositoryItemRichEditStyleEdit1.Control = this.richEditControl1;
            this.repositoryItemRichEditStyleEdit1.Name = "repositoryItemRichEditStyleEdit1";
            // 
            // fileNewItem1
            // 
            this.fileNewItem1.Id = 54;
            this.fileNewItem1.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("fileNewItem1.ImageOptions.Image")));
            this.fileNewItem1.ImageOptions.LargeImage = ((System.Drawing.Image)(resources.GetObject("fileNewItem1.ImageOptions.LargeImage")));
            this.fileNewItem1.Name = "fileNewItem1";
            // 
            // fileOpenItem1
            // 
            this.fileOpenItem1.Id = 55;
            this.fileOpenItem1.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("fileOpenItem1.ImageOptions.Image")));
            this.fileOpenItem1.ImageOptions.LargeImage = ((System.Drawing.Image)(resources.GetObject("fileOpenItem1.ImageOptions.LargeImage")));
            this.fileOpenItem1.Name = "fileOpenItem1";
            // 
            // fileSaveItem1
            // 
            this.fileSaveItem1.Id = 56;
            this.fileSaveItem1.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("fileSaveItem1.ImageOptions.Image")));
            this.fileSaveItem1.ImageOptions.LargeImage = ((System.Drawing.Image)(resources.GetObject("fileSaveItem1.ImageOptions.LargeImage")));
            this.fileSaveItem1.Name = "fileSaveItem1";
            // 
            // fileSaveAsItem1
            // 
            this.fileSaveAsItem1.Id = 57;
            this.fileSaveAsItem1.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("fileSaveAsItem1.ImageOptions.Image")));
            this.fileSaveAsItem1.ImageOptions.LargeImage = ((System.Drawing.Image)(resources.GetObject("fileSaveAsItem1.ImageOptions.LargeImage")));
            this.fileSaveAsItem1.Name = "fileSaveAsItem1";
            // 
            // quickPrintItem1
            // 
            this.quickPrintItem1.Id = 58;
            this.quickPrintItem1.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("quickPrintItem1.ImageOptions.Image")));
            this.quickPrintItem1.ImageOptions.LargeImage = ((System.Drawing.Image)(resources.GetObject("quickPrintItem1.ImageOptions.LargeImage")));
            this.quickPrintItem1.Name = "quickPrintItem1";
            // 
            // printItem1
            // 
            this.printItem1.Id = 59;
            this.printItem1.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("printItem1.ImageOptions.Image")));
            this.printItem1.ImageOptions.LargeImage = ((System.Drawing.Image)(resources.GetObject("printItem1.ImageOptions.LargeImage")));
            this.printItem1.Name = "printItem1";
            // 
            // printPreviewItem1
            // 
            this.printPreviewItem1.Id = 60;
            this.printPreviewItem1.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("printPreviewItem1.ImageOptions.Image")));
            this.printPreviewItem1.ImageOptions.LargeImage = ((System.Drawing.Image)(resources.GetObject("printPreviewItem1.ImageOptions.LargeImage")));
            this.printPreviewItem1.Name = "printPreviewItem1";
            // 
            // undoItem1
            // 
            this.undoItem1.Id = 61;
            this.undoItem1.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("undoItem1.ImageOptions.Image")));
            this.undoItem1.ImageOptions.LargeImage = ((System.Drawing.Image)(resources.GetObject("undoItem1.ImageOptions.LargeImage")));
            this.undoItem1.Name = "undoItem1";
            // 
            // redoItem1
            // 
            this.redoItem1.Id = 62;
            this.redoItem1.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("redoItem1.ImageOptions.Image")));
            this.redoItem1.ImageOptions.LargeImage = ((System.Drawing.Image)(resources.GetObject("redoItem1.ImageOptions.LargeImage")));
            this.redoItem1.Name = "redoItem1";
            // 
            // ınsertBookmarkItem1
            // 
            this.ınsertBookmarkItem1.Id = 63;
            this.ınsertBookmarkItem1.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("ınsertBookmarkItem1.ImageOptions.Image")));
            this.ınsertBookmarkItem1.ImageOptions.LargeImage = ((System.Drawing.Image)(resources.GetObject("ınsertBookmarkItem1.ImageOptions.LargeImage")));
            this.ınsertBookmarkItem1.Name = "ınsertBookmarkItem1";
            // 
            // ınsertHyperlinkItem1
            // 
            this.ınsertHyperlinkItem1.Id = 64;
            this.ınsertHyperlinkItem1.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("ınsertHyperlinkItem1.ImageOptions.Image")));
            this.ınsertHyperlinkItem1.ImageOptions.LargeImage = ((System.Drawing.Image)(resources.GetObject("ınsertHyperlinkItem1.ImageOptions.LargeImage")));
            this.ınsertHyperlinkItem1.Name = "ınsertHyperlinkItem1";
            // 
            // editPageHeaderItem1
            // 
            this.editPageHeaderItem1.Id = 65;
            this.editPageHeaderItem1.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("editPageHeaderItem1.ImageOptions.Image")));
            this.editPageHeaderItem1.ImageOptions.LargeImage = ((System.Drawing.Image)(resources.GetObject("editPageHeaderItem1.ImageOptions.LargeImage")));
            this.editPageHeaderItem1.Name = "editPageHeaderItem1";
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this.standaloneBarDockControl1);
            this.panelControl1.Location = new System.Drawing.Point(2, 2);
            this.panelControl1.MaximumSize = new System.Drawing.Size(0, 30);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(550, 30);
            this.panelControl1.TabIndex = 4;
            // 
            // layoutControlGroup1
            // 
            this.layoutControlGroup1.CustomizationFormText = "layoutControlGroup1";
            this.layoutControlGroup1.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
            this.layoutControlGroup1.GroupBordersVisible = false;
            this.layoutControlGroup1.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItemToolBar,
            this.layoutControlItem2});
            this.layoutControlGroup1.Location = new System.Drawing.Point(0, 0);
            this.layoutControlGroup1.Name = "layoutControlGroup1";
            this.layoutControlGroup1.Padding = new DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0);
            this.layoutControlGroup1.Size = new System.Drawing.Size(554, 369);
            this.layoutControlGroup1.TextVisible = false;
            // 
            // layoutControlItemToolBar
            // 
            this.layoutControlItemToolBar.Control = this.panelControl1;
            this.layoutControlItemToolBar.CustomizationFormText = "layoutControlItem1";
            this.layoutControlItemToolBar.Location = new System.Drawing.Point(0, 0);
            this.layoutControlItemToolBar.Name = "layoutControlItemToolBar";
            this.layoutControlItemToolBar.Size = new System.Drawing.Size(554, 34);
            this.layoutControlItemToolBar.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItemToolBar.TextVisible = false;
            // 
            // layoutControlItem2
            // 
            this.layoutControlItem2.Control = this.richEditControl1;
            this.layoutControlItem2.CustomizationFormText = "layoutControlItem2";
            this.layoutControlItem2.Location = new System.Drawing.Point(0, 34);
            this.layoutControlItem2.Name = "layoutControlItem2";
            this.layoutControlItem2.Size = new System.Drawing.Size(554, 335);
            this.layoutControlItem2.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem2.TextVisible = false;
            // 
            // richEditBarController1
            // 
            this.richEditBarController1.BarItems.Add(this.cutItem1);
            this.richEditBarController1.BarItems.Add(this.copyItem1);
            this.richEditBarController1.BarItems.Add(this.pasteItem1);
            this.richEditBarController1.BarItems.Add(this.pasteSpecialItem1);
            this.richEditBarController1.BarItems.Add(this.changeFontNameItem1);
            this.richEditBarController1.BarItems.Add(this.changeFontSizeItem1);
            this.richEditBarController1.BarItems.Add(this.changeFontColorItem1);
            this.richEditBarController1.BarItems.Add(this.changeFontBackColorItem1);
            this.richEditBarController1.BarItems.Add(this.toggleFontBoldItem1);
            this.richEditBarController1.BarItems.Add(this.toggleFontItalicItem1);
            this.richEditBarController1.BarItems.Add(this.toggleFontUnderlineItem1);
            this.richEditBarController1.BarItems.Add(this.toggleFontDoubleUnderlineItem1);
            this.richEditBarController1.BarItems.Add(this.toggleFontStrikeoutItem1);
            this.richEditBarController1.BarItems.Add(this.toggleFontDoubleStrikeoutItem1);
            this.richEditBarController1.BarItems.Add(this.toggleFontSuperscriptItem1);
            this.richEditBarController1.BarItems.Add(this.toggleFontSubscriptItem1);
            this.richEditBarController1.BarItems.Add(this.changeTextCaseItem1);
            this.richEditBarController1.BarItems.Add(this.makeTextUpperCaseItem1);
            this.richEditBarController1.BarItems.Add(this.makeTextLowerCaseItem1);
            this.richEditBarController1.BarItems.Add(this.toggleTextCaseItem1);
            this.richEditBarController1.BarItems.Add(this.fontSizeIncreaseItem1);
            this.richEditBarController1.BarItems.Add(this.fontSizeDecreaseItem1);
            this.richEditBarController1.BarItems.Add(this.clearFormattingItem1);
            this.richEditBarController1.BarItems.Add(this.showFontFormItem1);
            this.richEditBarController1.BarItems.Add(this.toggleParagraphAlignmentLeftItem1);
            this.richEditBarController1.BarItems.Add(this.toggleParagraphAlignmentCenterItem1);
            this.richEditBarController1.BarItems.Add(this.toggleParagraphAlignmentRightItem1);
            this.richEditBarController1.BarItems.Add(this.toggleParagraphAlignmentJustifyItem1);
            this.richEditBarController1.BarItems.Add(this.changeParagraphLineSpacingItem1);
            this.richEditBarController1.BarItems.Add(this.setSingleParagraphSpacingItem1);
            this.richEditBarController1.BarItems.Add(this.setSesquialteralParagraphSpacingItem1);
            this.richEditBarController1.BarItems.Add(this.setDoubleParagraphSpacingItem1);
            this.richEditBarController1.BarItems.Add(this.showLineSpacingFormItem1);
            this.richEditBarController1.BarItems.Add(this.addSpacingBeforeParagraphItem1);
            this.richEditBarController1.BarItems.Add(this.removeSpacingBeforeParagraphItem1);
            this.richEditBarController1.BarItems.Add(this.addSpacingAfterParagraphItem1);
            this.richEditBarController1.BarItems.Add(this.removeSpacingAfterParagraphItem1);
            this.richEditBarController1.BarItems.Add(this.toggleBulletedListItem1);
            this.richEditBarController1.BarItems.Add(this.toggleNumberingListItem1);
            this.richEditBarController1.BarItems.Add(this.toggleMultiLevelListItem1);
            this.richEditBarController1.BarItems.Add(this.decreaseIndentItem1);
            this.richEditBarController1.BarItems.Add(this.ıncreaseIndentItem1);
            this.richEditBarController1.BarItems.Add(this.toggleShowWhitespaceItem1);
            this.richEditBarController1.BarItems.Add(this.showParagraphFormItem1);
            this.richEditBarController1.BarItems.Add(this.changeStyleItem1);
            this.richEditBarController1.BarItems.Add(this.findItem1);
            this.richEditBarController1.BarItems.Add(this.replaceItem1);
            this.richEditBarController1.BarItems.Add(this.fileNewItem1);
            this.richEditBarController1.BarItems.Add(this.fileOpenItem1);
            this.richEditBarController1.BarItems.Add(this.fileSaveItem1);
            this.richEditBarController1.BarItems.Add(this.fileSaveAsItem1);
            this.richEditBarController1.BarItems.Add(this.quickPrintItem1);
            this.richEditBarController1.BarItems.Add(this.printItem1);
            this.richEditBarController1.BarItems.Add(this.printPreviewItem1);
            this.richEditBarController1.BarItems.Add(this.undoItem1);
            this.richEditBarController1.BarItems.Add(this.redoItem1);
            this.richEditBarController1.BarItems.Add(this.ınsertPageBreakItem1);
            this.richEditBarController1.BarItems.Add(this.ınsertTableItem1);
            this.richEditBarController1.BarItems.Add(this.ınsertPictureItem1);
            this.richEditBarController1.BarItems.Add(this.ınsertBookmarkItem1);
            this.richEditBarController1.BarItems.Add(this.ınsertHyperlinkItem1);
            this.richEditBarController1.BarItems.Add(this.editPageHeaderItem1);
            this.richEditBarController1.BarItems.Add(this.editPageFooterItem1);
            this.richEditBarController1.BarItems.Add(this.ınsertPageNumberItem1);
            this.richEditBarController1.BarItems.Add(this.ınsertPageCountItem1);
            this.richEditBarController1.BarItems.Add(this.ınsertSymbolItem1);
            this.richEditBarController1.Control = this.richEditControl1;
            // 
            // RichTextEditor
            // 
            this.Controls.Add(this.layoutControl1);
            this.Controls.Add(this.barDockControlLeft);
            this.Controls.Add(this.barDockControlRight);
            this.Controls.Add(this.barDockControlBottom);
            this.Controls.Add(this.barDockControlTop);
            this.Name = "RichTextEditor";
            this.Size = new System.Drawing.Size(554, 369);
            this.Leave += new System.EventHandler(this.RichTextEditor_Leave);
            ((System.ComponentModel.ISupportInitialize)(this.popupMenu1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).EndInit();
            this.layoutControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemFontEdit1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemRichEditFontSizeEdit1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemRichEditStyleEdit1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemToolBar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.richEditBarController1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
    }
}
