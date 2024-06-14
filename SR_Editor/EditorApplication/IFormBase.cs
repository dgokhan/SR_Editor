using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SR_Editor.EditorApplication
{
    public interface IFormBase
    {
        void InitData();

        void InitDesign();

        void InitLookUp();

        void InitMask();

        void InitRight();

        void InitValidationRules();
    }
}
