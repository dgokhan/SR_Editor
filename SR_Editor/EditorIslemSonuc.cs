using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SR_Editor.Core
{
    public class EditorIslemSonuc
    {
        private string sonucKodu;

        private string mesajBaslik;

        private string mesajIcerik;

        private string sonucHataKodu;

        private object entity;

        private List<object> listEntity;

        private MessageBoxIcon icon;

        private List<string> listUyariListesi;

        public object Entity
        {
            get
            {
                return this.entity;
            }
            set
            {
                this.entity = value;
            }
        }

        public MessageBoxIcon Icon
        {
            get
            {
                return this.icon;
            }
            set
            {
                this.icon = value;
            }
        }

        public List<object> ListEntity
        {
            get
            {
                return this.listEntity;
            }
            set
            {
                this.listEntity = value;
            }
        }

        public List<string> ListUyariListesi
        {
            get
            {
                return this.listUyariListesi;
            }
            set
            {
                this.listUyariListesi = value;
            }
        }

        public string MesajBaslik
        {
            get
            {
                return this.mesajBaslik;
            }
            set
            {
                this.mesajBaslik = value;
            }
        }

        public string MesajIcerik
        {
            get
            {
                return this.mesajIcerik;
            }
            set
            {
                this.mesajIcerik = value;
            }
        }

        public string SonucHataKodu
        {
            get
            {
                return this.sonucHataKodu;
            }
            set
            {
                this.sonucHataKodu = value;
            }
        }

        public string SonucKodu
        {
            get
            {
                return this.sonucKodu;
            }
            set
            {
                this.sonucKodu = value;
            }
        }

        public EditorIslemSonuc()
        {
        }

        public EditorIslemSonuc(string pSonucKodu)
        {
            if (!(pSonucKodu == "İşlem Başarısız"))
            {
                this.icon = MessageBoxIcon.Asterisk;
                this.mesajBaslik = "İşlem Başarılı";
            }
            else
            {
                this.icon = MessageBoxIcon.Hand;
                this.mesajBaslik = "İşlem Başarısız";
            }
            this.sonucKodu = pSonucKodu;
        }

        public EditorIslemSonuc(string pSonucKodu, MessageBoxIcon pIcon)
        {
            this.sonucKodu = pSonucKodu;
            this.icon = pIcon;
        }

        public EditorIslemSonuc(string pSonucKodu, string pMesajIcerik, string pMesajBaslik, MessageBoxIcon pIcon)
        {
            this.sonucKodu = pSonucKodu;
            this.mesajIcerik = pMesajIcerik;
            this.mesajBaslik = pMesajBaslik;
            this.icon = pIcon;
        }
    }
}
