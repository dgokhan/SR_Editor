using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SR_Editor.Core;

namespace SR_Editor.EditorApplication.Character.Action
{
    public class CharacterActionModule : ModuleInfoGroup
    {
        public ModuleInfo Character
        {
            get
            {
                return base["FormCharacter"];
            }
        }
        
        public ModuleInfo CharacterPunishment
        {
            get
            {
                return base["FormCharacterPunishment"];
            }
        }

        public CharacterActionModule()
        {
            base.NameSpace = "SR_Editor.Modules.Character.Action";
            base.Caption = "İşlemler";

            base.Add(new ModuleInfo()
            {
                Key = "FormCharacter",
                Caption = "Karakter Sorgulama",
                IsMenuVisible = false,
                IsDialog = false,
                Permission = RoyaleSupportPermissions.Account.Default
            });

            base.Add(new ModuleInfo()
            {
                Key = "FormCharacterPunishment",
                Caption = "Karakter Yasaklama",
                IsMenuVisible = false,
                IsDialog = true,
                Permission = RoyaleSupportPermissions.Account.ChatPunishment
            });
        }
    }
}
