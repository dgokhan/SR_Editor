using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.Utils.Menu;
using System.Collections;
using DevExpress.XtraRichEdit.API.Native;
using DevExpress.XtraBars;
using DevExpress.XtraRichEdit;
using System.IO;
using DevExpress.XtraLayout.Utils;

namespace SR_Editor.Core.Controls
{
    public partial class RichTextEditor : UserControl
    {
        private bool modified;
        private bool open;
        private string docName;
        private bool hideFontMenu = false;
        private bool showDefaultPopupMenu = true;
        private bool isCantCopy = false;
        private bool buyukKucukHarfMenusuEklendi = false;
        private const string docNameDefault = "document1.rtf";
        private bool isBasitGoruntu;
        private bool isToolbarVisibil;

        [Bindable(true)]
        public new string Text
        {
            get
            {
                return this.richEditControl1.Text;
            }
            set
            {
                this.richEditControl1.Text = value;
            }
        }

        [Bindable(true)]
        public string Rtf
        {
            get
            {
                return this.richEditControl1.RtfText;
            }
            set
            {
                this.richEditControl1.RtfText = value;
            }
        }

        [Bindable(true)]
        public byte[] OpenDocumentBytes
        {
            get
            {
                return this.richEditControl1.OpenDocumentBytes;
            }
            set
            {
                this.richEditControl1.OpenDocumentBytes = value;
            }
        }

        public bool ReadOnly
        {
            get
            {
                return this.richEditControl1.ReadOnly;
            }
            set
            {
                this.richEditControl1.ReadOnly = value;
            }
        }

        public bool HideFontMenu
        {
            get
            {
                return this.hideFontMenu;
            }
            set
            {
                this.hideFontMenu = value;
            }
        }

        public bool ShowDefaultPopupMenu
        {
            get
            {
                return this.showDefaultPopupMenu;
            }
            set
            {
                this.showDefaultPopupMenu = value;
            }
        }

        public bool IsBasitGoruntu
        {
            get
            {
                return this.isBasitGoruntu;
            }
            set
            {
                this.isBasitGoruntu = value;
                if (this.isBasitGoruntu)
                {
                    this.richEditControl1.ActiveViewType = RichEditViewType.Simple;
                    this.layoutControlItemToolBar.Visibility = LayoutVisibility.Never;
                }
                else
                {
                    this.richEditControl1.ActiveViewType = RichEditViewType.PrintLayout;
                    this.layoutControlItemToolBar.Visibility = LayoutVisibility.Always;
                }
            }
        }

        public bool IsToolbarVisibil
        {
            get
            {
                return this.isToolbarVisibil;
            }
            set
            {
                this.isToolbarVisibil = value;
                if (this.isToolbarVisibil)
                    this.layoutControlItemToolBar.Visibility = LayoutVisibility.Always;
                else
                    this.layoutControlItemToolBar.Visibility = LayoutVisibility.Never;
            }
        }

        public bool IsCantCopy
        {
            get
            {
                return this.richEditControl1.ReadOnly;
            }
            set
            {
                this.isCantCopy = value;
                if (this.isCantCopy)
                {
                    this.richEditControl1.Options.Behavior.Copy = DocumentCapability.Disabled;
                    this.richEditControl1.Options.Behavior.Cut = DocumentCapability.Disabled;
                    this.richEditControl1.Options.Behavior.Drag = DocumentCapability.Disabled;
                    this.richEditControl1.Options.Behavior.Drop = DocumentCapability.Disabled;
                    this.richEditControl1.Options.Behavior.Open = DocumentCapability.Disabled;
                    this.richEditControl1.Options.Behavior.Paste = DocumentCapability.Disabled;
                    this.richEditControl1.Options.Behavior.Printing = DocumentCapability.Disabled;
                    this.richEditControl1.Options.Behavior.Save = DocumentCapability.Disabled;
                    this.richEditControl1.Options.Behavior.SaveAs = DocumentCapability.Disabled;
                    this.richEditControl1.Options.Behavior.ShowPopupMenu = DocumentCapability.Disabled;
                    this.richEditControl1.ReadOnly = true;
                }
                else
                {
                    this.richEditControl1.Options.Behavior.Copy = DocumentCapability.Default;
                    this.richEditControl1.Options.Behavior.Cut = DocumentCapability.Default;
                    this.richEditControl1.Options.Behavior.Drag = DocumentCapability.Default;
                    this.richEditControl1.Options.Behavior.Drop = DocumentCapability.Default;
                    this.richEditControl1.Options.Behavior.Open = DocumentCapability.Default;
                    this.richEditControl1.Options.Behavior.Paste = DocumentCapability.Default;
                    this.richEditControl1.Options.Behavior.Printing = DocumentCapability.Default;
                    this.richEditControl1.Options.Behavior.Save = DocumentCapability.Default;
                    this.richEditControl1.Options.Behavior.SaveAs = DocumentCapability.Default;
                    this.richEditControl1.Options.Behavior.ShowPopupMenu = DocumentCapability.Default;
                }
            }
        }

        public PopupMenu DefaultPopupMenu
        {
            get
            {
                return this.popupMenu1;
            }
        }

        public void LoadRTFDocument(MemoryStream stream)
        {
            this.richEditControl1.LoadDocument((Stream)stream, DocumentFormat.Rtf);
        }

        public RichTextEditor()
        {
            this.InitializeComponent();
        }

        private void CreateColorPopup(PopupControlContainer container)
        {
        }

        private void MhtTextChanged(object sender, EventArgs e)
        {
        }

        private void ModifiedChanged(object sender, EventArgs e)
        {
            if (this.OnModifiedChanged == null)
                return;
            this.OnModifiedChanged(sender, e);
        }

        private void TextChanged(object sender, EventArgs e)
        {
            if (this.OnTextChanged == null)
                return;
            this.OnTextChanged(sender, e);
        }

        private void RichTextEditor_Leave(object sender, EventArgs e)
        {
            if (this.OnLeave == null)
                return;
            this.OnLeave(sender, e);
        }

        public void Yazdir()
        {
            this.richEditControl1.ShowPrintPreview();
        }

        private void richEditControl1_PopupMenuShowing(object sender, DevExpress.XtraRichEdit.PopupMenuShowingEventArgs e)
        {
            if (this.hideFontMenu)
            {
                foreach (DXMenuItem dxMenuItem in (CollectionBase)e.Menu.Items)
                {
                    if (dxMenuItem.Caption == "Font")
                        dxMenuItem.Enabled = false;
                }
            }
            if (this.ReadOnly || this.isCantCopy || this.hideFontMenu)
                return;
            DXMenuItem dxMenuItem1 = new DXMenuItem();
            DXMenuItem dxMenuItem2 = new DXMenuItem();
            dxMenuItem1.Caption = "Büyük Harfe Çevir";
            dxMenuItem1.Click += new EventHandler(this.ToUpperDXMenuItem_Click);
            dxMenuItem2.Caption = "Küçük Harfe Çevir";
            dxMenuItem2.Click += new EventHandler(this.ToLowerDXMenuItem_Click);
            e.Menu.Items.Add(dxMenuItem1);
            e.Menu.Items.Add(dxMenuItem2);
        }

        private void ToUpperDXMenuItem_Click(object sender, EventArgs e)
        {
            DocumentRange selection = this.richEditControl1.Document.Selection;
            string text = this.richEditControl1.Document.GetText(this.richEditControl1.Document.Selection);
            if (!CommonExtension.IsDolu(text))
                return;
            this.richEditControl1.Document.Replace(this.richEditControl1.Document.Selection, text.ToUpper());
        }

        private void ToLowerDXMenuItem_Click(object sender, EventArgs e)
        {
            DocumentRange selection = this.richEditControl1.Document.Selection;
            string text = this.richEditControl1.Document.GetText(this.richEditControl1.Document.Selection);
            if (!CommonExtension.IsDolu(text))
                return;
            this.richEditControl1.Document.Replace(this.richEditControl1.Document.Selection, text.ToLower());
        }

        public delegate void RichTextEventHandler(object sender, EventArgs e);
    }
}
