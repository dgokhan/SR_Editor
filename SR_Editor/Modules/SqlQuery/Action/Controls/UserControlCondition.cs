using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraRichEdit.Services;
using DevExpress.Office.Utils;
using SR_Editor.Extension;

namespace SR_Editor.Modules.SqlQuery.Action.Controls
{
    public partial class UserControlCondition : UserControl
    {
        public UserControlCondition()
        {
            InitializeComponent();

            richEditControlSorgu.Options.Search.RegExResultMaxGuaranteedLength = 2000;
            richEditControlSorgu.ReplaceService<ISyntaxHighlightService>(new CustomSyntaxHighlightService(richEditControlSorgu.Document));

            //Specify the richEdit's layout settings 
            richEditControlSorgu.ActiveViewType = DevExpress.XtraRichEdit.RichEditViewType.Simple;
            richEditControlSorgu.Document.Sections[0].Page.Width = Units.InchesToDocumentsF(80f);
            richEditControlSorgu.Document.DefaultCharacterProperties.FontName = "Courier New";
            richEditControlSorgu.Document.DefaultCharacterProperties.FontSize = 9;
        }
    }
}
