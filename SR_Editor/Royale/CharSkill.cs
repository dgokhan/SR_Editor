using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SR_Editor.Royale
{
    public class CharSkill
    {
        public byte SkillId { get; set; }
        public byte MasterType { get; set; }
        public byte Level { get; set; }
        public int NextRead { get; set; }
    }
}
