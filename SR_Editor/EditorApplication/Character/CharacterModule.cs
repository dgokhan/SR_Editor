using SR_Editor.Core;
using SR_Editor.EditorApplication.Character.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SR_Editor.EditorApplication.Character.Action;

namespace SR_Editor.EditorApplication.Character
{
    public class CharacterModule : ModuleInfoGroup
    {
        private CharacterQueryModule characterQueryModule;
        public CharacterQueryModule CharacterQueryModule
        {
            get
            {
                if (this.characterQueryModule == null)
                {
                    this.characterQueryModule = new CharacterQueryModule();
                }
                return this.characterQueryModule;
            }
        }
        private CharacterActionModule characterActionModule;
        public CharacterActionModule CharacterActionModule
        {
            get
            {
                if (this.characterActionModule == null)
                {
                    this.characterActionModule = new CharacterActionModule();
                }
                return this.characterActionModule;
            }
        }
        public CharacterModule()
        {
            base.AssemblyName = "SR_Editor";
            base.NameSpace = "SR_Editor.Modules.Character";
            base.Caption = "Karakter İşlemleri";
            //base.ImageName = "Defination.png";
            base.Add(this.CharacterQueryModule);
            base.Add(this.CharacterActionModule);
        }
    }
}
