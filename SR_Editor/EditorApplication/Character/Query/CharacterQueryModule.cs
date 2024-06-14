using SR_Editor.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SR_Editor.EditorApplication.Character.Query
{
    public class CharacterQueryModule : ModuleInfoGroup
    {
        public ModuleInfo CharacterList
        {
            get
            {
                return base["FormCharacterList"];
            }
        }

        public CharacterQueryModule()
        {
            base.NameSpace = "SR_Editor.Modules.Character.Query";
            base.Caption = "İşlemler";

            base.Add(new ModuleInfo()
            {
                Key = "FormCharacterList",
                Caption = "Karakter Arama",
                IsMenuVisible = true,
                IsDialog = false,
                Permission = RoyaleSupportPermissions.Account.Default
            });
        }
    }
}
