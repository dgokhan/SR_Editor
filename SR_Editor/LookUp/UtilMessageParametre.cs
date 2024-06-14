using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SR_Editor.LookUp
{

    public class UtilMessageParametre
    {
        private string adi;

        private string degeri;

        public string Adi
        {
            get
            {
                return this.adi;
            }
            set
            {
                this.adi = value;
            }
        }

        public string Degeri
        {
            get
            {
                return this.degeri;
            }
            set
            {
                this.degeri = value;
            }
        }

        public UtilMessageParametre()
        {
        }
    }
}
