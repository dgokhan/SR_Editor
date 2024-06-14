//using SR_Editor.Core.EditorApplication;
using SR_Editor.Core.Utility;
using SR_Editor.LookUp;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace SR_Editor.Core
{
    public class UtilMessage
    {
        public UtilMessage()
        {
        }

        public static DialogResult Show(string pMesaj)
        {
            DialogResult dialogResult;
            string ceviriStr = UtilLanguage.GetCeviriStr(pMesaj, false);
            if ((ceviriStr != pMesaj/* ? true : Editor.Core.EditorApplication.EditorApplication.LanguageId == 1*/))
            {
                MessageBox.Show(ceviriStr, UtilLanguage.GetCeviriStr("HBYS Mesaj", false), MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                dialogResult = DialogResult.OK;
            }
            else
            {
                dialogResult = DialogResult.OK;
            }
            return dialogResult;
        }

        public static DialogResult Show(string pMesaj, string pCaption)
        {
            DialogResult dialogResult;
            string ceviriStr = UtilLanguage.GetCeviriStr(pMesaj, false);
            /*
            if ((ceviriStr != pMesaj ? true : Editor.Core.EditorApplication.EditorApplication.LanguageId == 1))
            {
                MessageBox.Show(ceviriStr, UtilLanguage.GetCeviriStr(pCaption, false), MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                dialogResult = DialogResult.OK;
            }
            else
            {
                dialogResult = DialogResult.OK;
            }
            */
            MessageBox.Show(ceviriStr, UtilLanguage.GetCeviriStr(pCaption, false), MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            dialogResult = DialogResult.OK;
            return dialogResult;
        }

        public static DialogResult Show(string pMesaj, string pCaption, MessageBoxButtons pButtons)
        {
            DialogResult dialogResult;
            string ceviriStr = UtilLanguage.GetCeviriStr(pMesaj, false);
            //dialogResult = ((!(ceviriStr == pMesaj) || /*Editor.Core.EditorApplication.EditorApplication.LanguageId == 1 ? true : */pButtons != MessageBoxButtons.OK) ? MessageBox.Show(ceviriStr, UtilLanguage.GetCeviriStr(pCaption, false), pButtons, MessageBoxIcon.Asterisk) : DialogResult.OK);
            MessageBox.Show(ceviriStr, UtilLanguage.GetCeviriStr(pCaption, false), pButtons, MessageBoxIcon.Asterisk);
            dialogResult = DialogResult.OK;
            return dialogResult;
        }

        public static DialogResult Show(string pMesaj, string pCaption, MessageBoxButtons pButtons, MessageBoxIcon pIcon)
        {
            DialogResult dialogResult;
            string ceviriStr = UtilLanguage.GetCeviriStr(pMesaj, false);
            //dialogResult = ((!(ceviriStr == pMesaj) ||/* Editor.Core.EditorApplication.EditorApplication.LanguageId == 1 ? true :*/ pButtons != MessageBoxButtons.OK) ? MessageBox.Show(ceviriStr, UtilLanguage.GetCeviriStr(pCaption, false), pButtons, pIcon) : DialogResult.OK);
            dialogResult = MessageBox.Show(ceviriStr, UtilLanguage.GetCeviriStr(pCaption, false), pButtons, pIcon);
            return dialogResult;
        }

        public static DialogResult Show(string pMesaj, string pCaption, MessageBoxButtons pButtons, MessageBoxIcon pIcon, MessageBoxDefaultButton pDefaultButton)
        {
            DialogResult dialogResult;
            string ceviriStr = UtilLanguage.GetCeviriStr(pMesaj, false);
            //dialogResult = ((!(ceviriStr == pMesaj) ||/* Editor.Core.EditorApplication.EditorApplication.LanguageId == 1 ? true :*/ pButtons != MessageBoxButtons.OK) ? MessageBox.Show(ceviriStr, UtilLanguage.GetCeviriStr(pCaption, false), pButtons, pIcon, pDefaultButton) : DialogResult.OK);
            dialogResult = MessageBox.Show(ceviriStr, UtilLanguage.GetCeviriStr(pCaption, false), pButtons, pIcon,pDefaultButton);
            return dialogResult;
        }


        public static DialogResult Show(EnumUtilMessage pEnumMessageBox, List<string> pListParametre, string pMesaj, string pCaption, MessageBoxButtons pButtons, MessageBoxIcon pIcon)
        {
            // DilMesajCeviriEntity aktifMesajCeviri = DilCeviri.GetAktifMesajCeviri(pEnumMessageBox, pListParametre, pCaption, pMesaj);
            DialogResult dialogResult = MessageBox.Show(pMesaj, pCaption, pButtons, pIcon);
            //aktifMesajCeviri = null;
            return dialogResult;
        }

        public static DialogResult Show(EnumUtilMessage pEnumMessageBox, List<string> pListParametre, string pMesaj)
        {
            //DilMesajCeviriEntity aktifMesajCeviri = DilCeviri.GetAktifMesajCeviri(pEnumMessageBox, pListParametre, "HBYS Mesaj", pMesaj);
            MessageBox.Show(pMesaj, "Mesaj", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            //aktifMesajCeviri = null;
            return DialogResult.OK;
        }

        public static DialogResult Show(EnumUtilMessage pEnumMessageBox, List<string> pListParametre, string pMesaj, string pCaption)
        {
            //DilMesajCeviriEntity aktifMesajCeviri = DilCeviri.GetAktifMesajCeviri(pEnumMessageBox, pListParametre, pCaption, pMesaj);
            MessageBox.Show(pMesaj, pCaption, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            //aktifMesajCeviri = null;
            return DialogResult.OK;
        }

        public static DialogResult Show(EnumUtilMessage pEnumMessageBox, List<string> pListParametre, string pMesaj, string pCaption, MessageBoxButtons pButtons)
        {
            //DilMesajCeviriEntity aktifMesajCeviri = DilCeviri.GetAktifMesajCeviri(pEnumMessageBox, pListParametre, pCaption, pMesaj);
            DialogResult dialogResult = MessageBox.Show(pMesaj, pCaption, pButtons, MessageBoxIcon.Asterisk);
            //aktifMesajCeviri = null;
            return dialogResult;
        }


        public static DialogResult Show(EnumUtilMessage pEnumMessageBox, List<string> pListParametre, string pMesaj, string pCaption, MessageBoxButtons pButtons, MessageBoxIcon pIcon, MessageBoxDefaultButton pDefaultButton)
        {
            //DilMesajCeviriEntity aktifMesajCeviri = DilCeviri.GetAktifMesajCeviri(pEnumMessageBox, pListParametre, pCaption, pMesaj);
            DialogResult dialogResult = MessageBox.Show(pMesaj, pCaption, pButtons, pIcon, pDefaultButton);
            //aktifMesajCeviri = null;
            return dialogResult;
        }

        public static void ShowIslemBasariliMesaji(string pBaslik)
        {
            UtilMessage.Show(UtilLanguage.GetCeviriStr("İşlem başarıyla gerçekleştirilmiştir.", true), UtilLanguage.GetCeviriStr(pBaslik, true), MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        }
        
        public static void ShowIslemBasarisizMesaji(string pBaslik, string pAciklama)
        {
            UtilMessage.Show(pAciklama, UtilLanguage.GetCeviriStr(pBaslik, true), MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        public static void ShowIslemBasariliMesaji(string pBaslik, string pAciklama)
        {
            UtilMessage.Show(pAciklama, UtilLanguage.GetCeviriStr(pBaslik, true), MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        }

        public static void ShowIslemBasariliMesaji(EnumUtilMessage pEnumMessageBox, List<string> pListParametre, string pBaslik)
        {
            UtilMessage.Show(pEnumMessageBox, pListParametre, UtilLanguage.GetCeviriStr("İşlem başarıyla gerçekleştirilmiştir.", true), UtilLanguage.GetCeviriStr(pBaslik, true), MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        }

        public static DialogResult ShowSoruMesaji(string pMesaj, string pBaslik)
        {
            DialogResult dialogResult = MessageBox.Show(UtilLanguage.GetCeviriStr(pMesaj, true), UtilLanguage.GetCeviriStr(pBaslik, true), MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            return dialogResult;
        }

        public static DialogResult ShowSoruMesaji(EnumUtilMessage pEnumMessageBox, List<string> pListParametre, string pMesaj, string pBaslik)
        {
            //DilMesajCeviriEntity aktifMesajCeviri = DilCeviri.GetAktifMesajCeviri(pEnumMessageBox, pListParametre, pBaslik, pMesaj);
            DialogResult dialogResult = MessageBox.Show(pMesaj, pBaslik, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            //aktifMesajCeviri = null;
            return dialogResult;
        }

        public static void ShowUyariMesaji(string pMesaj, string pBaslik)
        {
            //string ceviriStr = UtilLanguage.GetCeviriStr(pMesaj, false);
            //if ((ceviriStr != pMesaj/* ? true : Editor.Core.EditorApplication.EditorApplication.LanguageId == 1)*/)
            {
                MessageBox.Show(pMesaj, UtilLanguage.GetCeviriStr(pBaslik, true), MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        public static void ShowUyariMesaji(EnumUtilMessage pEnumMessageBox, List<string> pListParametre, string pMesaj, string pBaslik)
        {
            //DilMesajCeviriEntity aktifMesajCeviri = DilCeviri.GetAktifMesajCeviri(pEnumMessageBox, pListParametre, pBaslik, pMesaj);
            MessageBox.Show(pMesaj, pBaslik, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            //aktifMesajCeviri = null;
        }
    }
}