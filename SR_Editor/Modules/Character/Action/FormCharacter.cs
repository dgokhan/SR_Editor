using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.Utils;
using DevExpress.XtraEditors.DXErrorProvider;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraSplashScreen;
using GameLib;
using RoyaleSupport;
using SR_Editor.Core;
using SR_Editor.Core.Exceptions;
using SR_Editor.EditorApplication;
using SR_Editor.Forms;
using SR_Editor.LookUp;
using SR_Editor.Royale;
using DevExpress.XtraEditors.Repository;

namespace SR_Editor.Modules.Character.Action
{
    public partial class FormCharacter : FormBase, IFormBase
    {
        private GameCharacterDto karakterBilgileri;
        private GameAccountDto hesapBilgileri;
        private List<GameCharacterAffectDto> karakterAffect;

        private int shardId;
        private int charId;

        private List<CharSkill> yetenekler;


        private readonly Dictionary<EnumJob, Dictionary<string, List<int>>> skillMap =
            new Dictionary<EnumJob, Dictionary<string, List<int>>>();

        Dictionary<string, Image> imageCache = new Dictionary<string, Image>(StringComparer.OrdinalIgnoreCase);

        public FormCharacter()
        {
            InitializeComponent();

            skillMap = new Dictionary<EnumJob, Dictionary<string, List<int>>>();
            skillMap.Add(EnumJob.JOB_WARRIOR, new Dictionary<string, List<int>>()
            {
                { "1", new List<int>() { 1, 2, 3, 4, 5 /*, 6*/ } },
                { "2", new List<int>() { 16, 17, 18, 19, 20 /*, 21*/ } },
                { "HORSE", new List<int>() { 137, 138, 139 } },
                { "SUPPORT", new List<int>() { 122, 123, 121, 124, 125, 129, 130, 131 } },
            });
            skillMap.Add(EnumJob.JOB_ASSASSIN, new Dictionary<string, List<int>>()
            {
                { "1", new List<int>() { 31, 32, 33, 34, 35 /*, 36*/ } },
                { "2", new List<int>() { 46, 47, 48, 49, 50 /*, 51*/ } },
                { "HORSE", new List<int>() { 137, 138, 139 } },
                { "SUPPORT", new List<int>() { 122, 123, 121, 124, 125, 129, 130, 131 } },
            });
            skillMap.Add(EnumJob.JOB_SURA, new Dictionary<string, List<int>>()
            {
                { "1", new List<int>() { 61, 62, 63, 64, 65, 66 } },
                { "2", new List<int>() { 76, 77, 78, 79, 80, 81 } },
                { "HORSE", new List<int>() { 137, 138, 139 } },
                { "SUPPORT", new List<int>() { 122, 123, 121, 124, 125, 129, 130, 131 } },
            });
            skillMap.Add(EnumJob.JOB_SHAMAN, new Dictionary<string, List<int>>()
            {
                { "1", new List<int>() { 91, 92, 93, 94, 95, 96 } },
                { "2", new List<int>() { 106, 107, 108, 109, 110, 111 } },
                { "HORSE", new List<int>() { 137, 138, 139 } },
                { "SUPPORT", new List<int>() { 122, 123, 121, 124, 125, 129, 130, 131 } },
            });
            skillMap.Add(EnumJob.JOB_WOLFMAN, new Dictionary<string, List<int>>()
            {
                { "1", new List<int>() { 170, 171, 172, 173, 174, 175 } },
                { "2", new List<int>() { 0, 0, 0, 0, 0, 0 } },
                { "HORSE", new List<int>() { 137, 138, 139 } },
                { "SUPPORT", new List<int>() { 122, 123, 121, 124, 125, 129, 130, 131 } },
            });
        }


        public void InitData()
        {
            if (!this.FormParams.Contains("ShardId") || !this.FormParams.Contains("CharId"))
                throw new ExceptionIsKurali("Lütfen sunucu ve karakter tanımlayınız.");

            shardId = Convert.ToInt32(this.FormParams["ShardId"]);
            charId = Convert.ToInt32(this.FormParams["CharId"]);

            this.bindingSourceJobTipi.DataSource = JobTipi.Liste;
            this.bindingSourceAffectTipi.DataSource = AffectTipi.Liste;

            Listele();
        }

        protected override void Listele()
        {

            var api = new RoyaleSupportClient();

            Loading(() =>
            {
                var characterInfo = api.GameCharacter(shardId, charId);
                karakterBilgileri = characterInfo;
                bindingSourceKarakterBilgileri.DataSource = karakterBilgileri;

                var accountInfo = api.FindAccountById(characterInfo.Account_id);
                hesapBilgileri = accountInfo;
                bindingSourceHesapBilgileri.DataSource = hesapBilgileri;

                if (hesapBilgileri.Status == "BLOCK")
                {
                    layoutControlGroupHesap.AppearanceGroup.BorderColor = DXColor.Red;
                }
                else if (hesapBilgileri.Status == "OK")
                {
                    layoutControlGroupHesap.AppearanceGroup.BorderColor = DXColor.Green;
                }

                var grade = GetAlignmentGrade(characterInfo.Alignment);
                var alignmentName = GetAlignmentTitleName(characterInfo.Alignment);
                var alignmentColor = GetAlignmentGolor(characterInfo.Alignment);

                this.Text = $"{karakterBilgileri.Name} - Karakter Sorgulama";

                textEditSiralama.BackColor = Color.Black;
                textEditSiralama.ForeColor = alignmentColor;
                textEditSiralama.Text = $"{alignmentName} ({characterInfo.Alignment})";

                textEditOynamaSuresi.Text = ToHumanReadable(characterInfo.Play_time);

                textEditBeceri.Text = GetSkillGroup((EnumRace)characterInfo.Job, characterInfo.Skill_group);

                textEditGold.Text = $"{characterInfo.Gold:#,##0}".Replace(",", ".");

                yetenekler = new List<CharSkill>();

                skillMap.TryGetValue(((EnumRace)characterInfo.Job).RaceToJob(), out var charSkillList);

                xtraTabControl1.SelectedTabPageIndex = 0;


                var currentSkillGroup = characterInfo.Skill_group;

                var memoryStream = new MemoryStream(characterInfo.Skill_level);
                var reader = new BinaryReader(memoryStream);
                for (int i = 0; i < 255; i++)
                {
                    var skillData = new CharSkill();
                    skillData.SkillId = (byte)i;
                    skillData.MasterType = reader.ReadByte();
                    skillData.Level = reader.ReadByte();
                    skillData.NextRead = reader.ReadInt32();

                    if (currentSkillGroup > 0)
                    {
                        var skills = charSkillList[currentSkillGroup.ToString()];

                        if (skills.Contains(i))
                            yetenekler.Add(skillData);
                    }

                    {
                        var skills = charSkillList["SUPPORT"];

                        if (skills.Contains(i))
                            yetenekler.Add(skillData);
                    }

                    {
                        var skills = charSkillList["HORSE"];

                        if (skills.Contains(i))
                            yetenekler.Add(skillData);
                    }

                }

                memoryStream.Dispose();
                reader.Dispose();

                bindingSourceYetenekler.DataSource = yetenekler;

                ListeleEtkiler();
            });

        }

        public string GetSkillGroup(EnumRace race, int skillGroup)
        {

            if (race == EnumRace.MAIN_RACE_WARRIOR_M || race == EnumRace.MAIN_RACE_WARRIOR_W)
            {
                if (skillGroup == 1)
                {
                    return "Bedensel";
                }
                else if (skillGroup == 2)
                {
                    return "Güçlü Beden";
                }
            }
            else if (race == EnumRace.MAIN_RACE_SURA_M || race == EnumRace.MAIN_RACE_SURA_W)
            {
                if (skillGroup == 1)
                {
                    return "Büyülü Silah";
                }
                else if (skillGroup == 2)
                {
                    return "Kara Büyü";
                }
            }
            else if (race == EnumRace.MAIN_RACE_ASSASSIN_M || race == EnumRace.MAIN_RACE_ASSASSIN_W)
            {
                if (skillGroup == 1)
                {
                    return "Yakın Dövüş";
                }
                else if (skillGroup == 2)
                {
                    return "Uzak Dövüş";
                }
            }
            else if (race == EnumRace.MAIN_RACE_SHAMAN_M || race == EnumRace.MAIN_RACE_SHAMAN_W)
            {
                if (skillGroup == 1)
                {
                    return "Ejderha Gücü";
                }
                else if (skillGroup == 2)
                {
                    return "İyileştirme";
                }
            }

            return "Seçilmemiş";
        }

        public string GetSkillName(int skillIndex, int skillGrade = -1)
        {
            var skillData = SkillData.GetEntityByKeySkillData_Key_Id(skillIndex);

            string key = string.Empty;
            if (skillGrade != -1)
            {
                if (skillGrade >= 0 && skillGrade < (int)GameLib.EnumSkillConst.SKILL_GRADE_COUNT)
                    key = GetNameLocalizedKey(skillIndex, skillGrade);
            }
            else
                key = GetNameLocalizedKey(skillIndex);

            var localized = LocaleSkill.GetEntityByKeyNameKey(key);
            return localized.LocalizedValue;
        }

        private string GetNameLocalizedKey(int skillId, int grade = -1)
        {
            if (grade <= 0)
                grade = 1;

            if (grade > (int)EnumSkillConst.SKILL_GRADE_COUNT)
                grade = (int)EnumSkillConst.SKILL_GRADE_COUNT;

            return $"{skillId}_name{grade}";
        }

        private string ToHumanReadable(int minute)
        {
            TimeSpan t = TimeSpan.FromMinutes(minute);
            return string.Format("{0:D1} Gün {1:D1} Saat {2:D1} Dakika",
                t.Days,
                t.Hours,
                t.Minutes);
        }

        private int GetAlignmentGrade(int alignment)
        {
            if (alignment >= 12000)
                return 0;
            else if (alignment >= 8000)
                return 1;
            else if (alignment >= 4000)
                return 2;
            else if (alignment >= 1000)
                return 3;
            else if (alignment >= 0)
                return 4;
            else if (alignment > -4000)
                return 5;
            else if (alignment > -8000)
                return 6;
            else if (alignment > -12000)
                return 7;

            return 8;
        }


        private Color GetAlignmentGolor(int alignment)
        {
            if (alignment >= 12000)
                return Color.Aqua;
            else if (alignment >= 8000)
                return Color.Aquamarine;
            else if (alignment >= 4000)
                return Color.CadetBlue;
            else if (alignment >= 1000)
                return Color.CornflowerBlue;
            else if (alignment >= 0)
                return Color.White;
            else if (alignment > -4000)
                return Color.Orange;
            else if (alignment > -8000)
                return Color.DarkOrange;
            else if (alignment > -12000)
                return Color.OrangeRed;

            return Color.Red;
        }

        private string GetAlignmentTitleName(int alignment)
        {
            if (alignment >= 12000)
                return "Kahraman";
            else if (alignment >= 8000)
                return "Soylu";
            else if (alignment >= 4000)
                return "İyi";
            else if (alignment >= 1000)
                return "Arkadaşça";
            else if (alignment >= 0)
                return "Tarafsız";
            else if (alignment > -4000)
                return "Agresif";
            else if (alignment > -8000)
                return "Hileli";
            else if (alignment > -12000)
                return "Kötü Niyetli";

            return "Zalim";
        }

        public void InitDesign()
        {
        }

        public void InitLookUp()
        {
            UtilLookUp.InitLookupEdit(this.lookUpEditJob, "Id", "Aciklama");
            UtilLookUp.InitLookupEdit(this.repositoryItemLookUpEditJobTipi, "Id", "Aciklama");
        }

        public void InitMask()
        {

        }

        public void InitRight()
        {

        }

        public override void FormParamsChanged()
        {
            this.InitData();
            this.InitDesign();
            this.InitMask();
            this.InitLookUp();
            this.InitValidationRules();
            this.InitRight();
        }

        public void InitValidationRules()
        {
            //this.ValidationProvider.SetValidationRule((Control)this.textEditAccountName, (ValidationRuleBase)UtilValidation.GetNotEmptyCondition());
        }

        private void Loading(System.Action action, System.Action fail = null, System.Action final = null)
        {
            SplashScreenManager.CloseForm(false);
            SplashScreenManager.ShowForm(typeof(EditorLoading));

            bool success = false;
            try
            {
                action?.Invoke();
                success = true;
            }
            catch (Exception ex)
            {
                fail?.Invoke();
                throw ex;
            }
            finally
            {
                SplashScreenManager.CloseForm(false);

                if (success)
                    final?.Invoke();
            }
        }

        private void barButtonItem3_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            UtilParameters pFormParams = new UtilParameters();
            pFormParams.Add("ShardId", shardId);
            pFormParams.Add("CharId", charId);
            EditorApplication.EditorApplication.Module.CharacterModule.CharacterActionModule.CharacterPunishment.Show(pFormParams);
        }

        private void xtraTabPage14_Paint(object sender, PaintEventArgs e)
        {

        }

        private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            UtilParameters pFormParams = new UtilParameters();
            pFormParams.Add("AccountId", hesapBilgileri.Id);
            pFormParams.Add("AccountName", hesapBilgileri.Username);
            EditorApplication.EditorApplication.Module.AccountModule.AccountQueryModule.Account.Show(pFormParams);
        }

        private void xtraTabControl1_Selected(object sender, DevExpress.XtraTab.TabPageEventArgs e)
        {

            var index = e.PageIndex;
            if (index == 0) // etkiler
            {
                ListeleEtkiler();
            }
            else if (index == 1) // esyalar
            {
                ListeleEsyalar();
            }
            else if (index == 2) // hizli erisim
            {
            }
            else if (index == 3) // beceriler
            {
            }
            else if (index == 4) // giris kayitlari
            {
                dateLoginStart.DateTime = DateTime.Now.AddMonths(-3);
                dateLoginFinish.DateTime = DateTime.Now;

                ListeleGirisKayitlari(dateLoginStart.DateTime, dateLoginFinish.DateTime);
            }
            else if (index == 5) // seviye kayitlari
            {
                ListeleSeviyeKayitlari();
            }
            else if (index == 6) // balikcilik kayitlari
            {
                dateFishStart.DateTime = DateTime.Now.AddMonths(-3);
                dateFishFinish.DateTime = DateTime.Now;

                ListeleBalikcilikKayitlari(dateFishStart.DateTime, dateFishFinish.DateTime);
            }
            else if (index == 7) // ticaret kayitlari
            {
                dateTradeStart.DateTime = DateTime.Now.AddMonths(-3);
                dateTradeFinish.DateTime = DateTime.Now;

                ListeleTicaretKayitlari(dateTradeStart.DateTime, dateTradeFinish.DateTime);
            }
            else if (index == 8) // gorev odul kayitlari
            {
                dateQuestRewardStart.DateTime = DateTime.Now.AddMonths(-3);
                dateQuestRewardFinish.DateTime = DateTime.Now;

                ListeleGorevOdulKayitlari(dateQuestRewardStart.DateTime, dateQuestRewardFinish.DateTime);
            }
            else if (index == 9) // quest flagleri
            {
                ListeleGorevFlagleri();
            }
            else if (index == 10) // pazar
            {
                dateSaleItemStart.DateTime = DateTime.Now.AddMonths(-1);
                dateSaleItemFinish.DateTime = DateTime.Now;

                ListeleAktifPazarUstBilgileri();
                KisiselDukkanDepoKayitlari(); 
                ListeleAktifPazarKayitlari();
                ListeleAktifPazarLogKayitlari(dateSaleItemStart.DateTime, dateSaleItemFinish.DateTime);
            }
            else if (index == 11) // depo kayıtları
            {
                dateStorageStart.DateTime = DateTime.Now.AddMonths(-3);
                dateStorageFinish.DateTime = DateTime.Now;

                ListeleDepoKayitlari(dateStorageStart.DateTime, dateStorageFinish.DateTime);
                ListeleAktifDepoKayitlari();
                ListeleAktifNesneDepoKayitlari();
            }
            else if (index == 12) // esya kullanma sayilari
            {
                dateItemUsingStart.DateTime = DateTime.Now.AddMonths(-3);
                dateItemUsingFinish.DateTime = DateTime.Now;

                ListeleEsyaKullanmaKayitlari(dateItemUsingStart.DateTime, dateItemUsingFinish.DateTime);
            }
            else if (index == 13) // esya toplama kayitlari
            {
                dateItemPickStart.DateTime = DateTime.Now.AddMonths(-1);
                dateItemPickFinish.DateTime = DateTime.Now;

                ListeleEsyaToplamaKayitlari(dateItemPickStart.DateTime, dateItemPickFinish.DateTime);
            }
            else if (index == 14) // esya atmakayitlari
            {
            }
            else if (index == 15) // esya yukseltme kayitlari
            {
                dateItemUpgradeStart.DateTime = DateTime.Now.AddMonths(-3);
                dateItemUpgradeFinish.DateTime = DateTime.Now;

                ListeleEsyaYukseltmeKayitlari(dateItemUpgradeStart.DateTime, dateItemUpgradeFinish.DateTime);
            }
            else if (index == 16) // yasaklama kayitlari
            {
                ListeleYasaklamaKayitlari();
            }
            else if (index == 17) // yasaklama kayitlari
            {
                ListeleHesapYasaklamaKayitlari();
            }
        }

        private void ListeleYasaklamaKayitlari()
        {
            var api = new RoyaleSupportClient();

            Loading(() =>
            {
                var data = api.GameCharacterPunishmentsByCharId(shardId, charId).ToList();
                bindingSourceYasaklamaGecmisi.DataSource = data;
            });
        }

        private void ListeleHesapYasaklamaKayitlari()
        {
            var api = new RoyaleSupportClient();

            Loading(() =>
            {
                var data = api.GameCharacterPunishmentsByCharId(shardId, charId).ToList();
                bindingSourceYasaklamaGecmisi.DataSource = data;
            });
        }

        private void ListeleEsyaYukseltmeKayitlari(DateTime startDate, DateTime finishDate)
        {
            var api = new RoyaleSupportClient();

            Loading(() =>
            {
                var data = api.GameCharacterItemRefineLog(shardId, charId).ToList();
                bindingSourceEsyaYukseltmeKayitlari.DataSource = data;
            });
        }

        private void ListeleEsyaToplamaKayitlari(DateTime startDate, DateTime finishDate)
        {
            var api = new RoyaleSupportClient();

            Loading(() =>
            {
                var data = api.GameCharacterItemPickLog(shardId, charId, startDate, finishDate).ToList();
                bindingSourceEsyaToplamaKayitlari.DataSource = data.OrderByDescending(x => x.Time);
            });
        }

        private void ListeleEsyaKullanmaKayitlari(DateTime startDate, DateTime finishDate)
        {
            var api = new RoyaleSupportClient();

            Loading(() =>
            {
                var data = api.GameCharacterItemUseLog(shardId, charId, startDate, finishDate).ToList();
                bindingSourceEsyaKullanmaKayitlari.DataSource = data.OrderByDescending(x => x.Time);
            });
        }

        private void ListeleDepoKayitlari(DateTime startDate, DateTime finishDate)
        {
            var api = new RoyaleSupportClient();

            Loading(() =>
            {
                var data = api.GameCharacterStorageLog(shardId, charId, startDate, finishDate).ToList();
                bindingSourceDepoKayitlari.DataSource = data.OrderByDescending(x => x.Time);
            });
        }
        private void ListeleAktifDepoKayitlari()
        {
            var api = new RoyaleSupportClient();

            Loading(() =>
            {
                var data = api.GameCharacterActiveStorage(shardId, charId).ToList();
                bindingSourceAktifDepoKayitlari.DataSource = data.OrderBy(x=>x.Pos);
            });
        }
        
        private void ListeleAktifNesneDepoKayitlari()
        {
            var api = new RoyaleSupportClient();

            Loading(() =>
            {
                var data = api.GameCharacterActiveMallStorage(shardId, karakterBilgileri.Name).ToList();
                bindingSourceNesneDepoKayitlari.DataSource = data.OrderBy(x=>x.Pos);
            });
        }

        private void ListeleTicaretKayitlari(DateTime startDate, DateTime finishDate)
        {
            var api = new RoyaleSupportClient();

            Loading(() =>
            {
                var data = api.GameCharacterExchangeLog(shardId, charId, startDate, finishDate).ToList();
                bindingSourceTicaretKayitlari.DataSource = data.OrderByDescending(x => x.Time);
            });
        }
        private void ListeleGorevFlagleri()
        {
            var api = new RoyaleSupportClient();

            Loading(() =>
            {
                var data = api.GameCharacterQuestFlagLog(shardId, charId).ToList();
                bindingSourceQuestFlagleri.DataSource = data;
            });
        }

        private void ListeleGorevOdulKayitlari(DateTime startDate, DateTime finishDate)
        {
            var api = new RoyaleSupportClient();

            Loading(() =>
            {
                var data = api.GameCharacterQuestRewardLog(shardId, charId, startDate, finishDate).ToList();
                bindingSourceGorevOdulKayitlari.DataSource = data.OrderByDescending(x => x.Time);
            });
        }

        private void ListeleGirisKayitlari(DateTime startDate, DateTime finishDate)
        {
            var api = new RoyaleSupportClient();

            Loading(() =>
            {
                var data = api.FindLoginLogByAccountId(hesapBilgileri.Id, startDate, finishDate).ToList();
                bindingSourceGirisKayitlari.DataSource = data.Where(p => p.Player_id == charId).OrderByDescending(x=>x.Login_time);
            });
        }

        private void ListeleSeviyeKayitlari()
        {
            var api = new RoyaleSupportClient();

            Loading(() =>
            {
                var data = api.GameCharacterLevelLog(shardId, charId).ToList();
                bindingSourceSeviyeKayitlari.DataSource = data;
            });
        }

        private void ListeleAktifPazarUstBilgileri()
        {
            try
            {
                var api = new RoyaleSupportClient();

                Loading(() =>
                {
                    var data = api.GameCharacterMarket(shardId, charId);
                    bindingSourceAktifPazarUstBilgileri.DataSource = data;
                });
            }
            catch (Exception)
            { 
            }
        }

        private void ListeleAktifPazarKayitlari()
        {
            try
            {
                var api = new RoyaleSupportClient();

                Loading(() =>
                {
                    var data = api.GameCharacterPartsOnSaleItems(shardId, charId);
                    bindingSourceAktifPazarKayitlari.DataSource = data;
                });
            }
            catch (Exception)
            {

            }
        }

        private void ListeleAktifPazarLogKayitlari(DateTime startDate, DateTime finishDate)
        {
            try
            {
                var api = new RoyaleSupportClient();

                Loading(() =>
                {
                    var data = api.GameCharacterPartsOnSaleItemLogs(shardId, charId, startDate, finishDate);
                    bindingSourceAktifPazarLogKayitlari.DataSource = data.OrderByDescending(x => x.Time);
                });
            }
            catch (Exception)
            {
                 
            }
        }

        private void KisiselDukkanDepoKayitlari()
        {
            try
            {
                var api = new RoyaleSupportClient();

                Loading(() =>
                {
                    var data = api.GameCharacterMarketStorageItems(shardId, charId);
                    bindingSourceKisiselDukkanDepoKayitlari.DataSource = data;
                });
            }
            catch (Exception)
            {
                 
            }
        }

        private void ListeleEtkiler()
        {
            var api = new RoyaleSupportClient();

            Loading(() =>
            {
                var data = api.GameCharacterAffect(shardId, charId);
                bindingSourceEtkiler.DataSource = data;
            });
        }

        private void ListeleEsyalar()
        {
            var api = new RoyaleSupportClient();

            Loading(() =>
            {
                var data = api.GameCharacterItem(shardId, charId);
                bindingSourceEsyalar.DataSource = data;
            });
        }

        private void ListeleBalikcilikKayitlari(DateTime startDate, DateTime finishDate)
        {
            var api = new RoyaleSupportClient();

            Loading(() =>
            {
                var data = api.GameCharacterFishLog(shardId, charId, startDate, finishDate);
                bindingSourceBalikcilikKayitlari.DataSource = data.OrderByDescending(x => x.Time);
            });
        }

        private void gridView1_CustomUnboundColumnData(object sender,
            DevExpress.XtraGrid.Views.Base.CustomColumnDataEventArgs e)
        {
            if (e.Column.FieldName == "Icon" && e.IsGetData)
            {
                GridView view = sender as GridView;
                var fileName = Convert.ToInt32(view.GetRowCellValue(view.GetRowHandle(e.ListSourceRowIndex), "Vnum"));
                if (fileName == 0)
                    return;

                if (!imageCache.ContainsKey(fileName.ToString()))
                {
                    Image img = GetImage(Convert.ToInt32(fileName));

                    var oldFileName = fileName;
                    if (img == null)
                    {
                        fileName = fileName - (fileName % 10);
                        img = GetImage(Convert.ToInt32(fileName));

                        if (img != null)
                        {
                            imageCache[oldFileName.ToString()] = img;
                            imageCache[fileName.ToString()] = img;
                        }
                        else
                        {
                            imageCache[oldFileName.ToString()] = null;
                            imageCache[fileName.ToString()] = null;
                        }
                    }
                    else
                    {
                        imageCache[fileName.ToString()] = img;
                    }

                }

                e.Value = imageCache[fileName.ToString()];
            }
        }

        Image GetImage(int itemId)
        {
            try
            {
                using (WebClient webClient = new WebClient())
                {
                    byte[] data = webClient.DownloadData($@"https://billing.royale.com.tr/icon/item/{itemId:D5}.png");

                    using (MemoryStream mem = new MemoryStream(data))
                    {
                        var yourImage = Image.FromStream(mem);
                        {
                            return yourImage;
                        }
                    }

                }
            }
            catch (Exception e)
            {
                return null;
            }

            return null;
        }

        private void gridView2_CustomUnboundColumnData(object sender,
            DevExpress.XtraGrid.Views.Base.CustomColumnDataEventArgs e)
        {
            if (e.Column.FieldName == "Icon" && e.IsGetData)
            {
                GridView view = sender as GridView;
                var fileName = Convert.ToInt32(view.GetRowCellValue(view.GetRowHandle(e.ListSourceRowIndex), "Vnum"));
                if (fileName == 0)
                    return;

                if (!imageCache.ContainsKey(fileName.ToString()))
                {
                    Image img = GetImage(Convert.ToInt32(fileName));

                    var oldFileName = fileName;
                    if (img == null)
                    {
                        fileName = fileName - (fileName % 10);
                        img = GetImage(Convert.ToInt32(fileName));

                        if (img != null)
                        {
                            imageCache[oldFileName.ToString()] = img;
                            imageCache[fileName.ToString()] = img;
                        }
                        else
                        {
                            imageCache[oldFileName.ToString()] = null;
                            imageCache[fileName.ToString()] = null;
                        }
                    }
                    else
                    {
                        imageCache[fileName.ToString()] = img;
                    }

                }

                e.Value = imageCache[fileName.ToString()];
            }
        }

        private void gridViewYetenekler_CustomUnboundColumnData(object sender,
            DevExpress.XtraGrid.Views.Base.CustomColumnDataEventArgs e)
        {

            if (e.Column.FieldName == "SkillName" && e.IsGetData)
            {
                GridView view = sender as GridView;
                var val = Convert.ToInt32(view.GetRowCellValue(view.GetRowHandle(e.ListSourceRowIndex), "SkillId"));
                if (val == 0)
                    return;
                var masterType = Convert.ToInt32(view.GetRowCellValue(view.GetRowHandle(e.ListSourceRowIndex), "MasterType"));
                var level = Convert.ToInt32(view.GetRowCellValue(view.GetRowHandle(e.ListSourceRowIndex), "Level"));

                string str = string.Empty;

                var skillName = GetSkillName(val, masterType);
                str += skillName;
                switch (masterType)
                {
                    case 0:
                        str += $" ({level})";
                        break;
                    case 1:
                        str += $" (M {level - 19})";
                        break;
                    case 2:
                        str += $" (GM {level - 29})";
                        break;
                    case 3:
                        str += $" (P)";
                        break;
                }

                e.Value = str;
            }
        }

        private void gridViewYetenekler_RowClick(object sender, RowClickEventArgs e)
        {
            var row = this.gridViewYetenekler.FocusedRowHandle < 0 ? null : (CharSkill)this.gridViewYetenekler.GetRow(this.gridViewYetenekler.FocusedRowHandle);

            if (row != null)
            {
                var api = new RoyaleSupportClient();
                Loading(() =>
                {
                    var skillData = api.GameCharacterSkill(shardId, charId, row.SkillId).ToList();
                    bindingSourceYetenekGecmisi.DataSource = skillData;
                }, () =>
                {
                    bindingSourceYetenekGecmisi.DataSource = null;
                });
                return;
            }

            bindingSourceYetenekGecmisi.DataSource = null;
        }

        private void gridViewSeviyeKayitlari_CustomUnboundColumnData(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDataEventArgs e)
        {
            if (e.Column.FieldName == "PlayTimeCalculated" && e.IsGetData)
            {
                GridView view = sender as GridView;
                var playTime = Convert.ToInt32(view.GetRowCellValue(view.GetRowHandle(e.ListSourceRowIndex), "Playtime"));

                e.Value = ToHumanReadable(playTime);
            }
            else if (e.Column.FieldName == "DiffCalculated" && e.IsGetData)
            {
                GridView view = sender as GridView;
                var playTime = Convert.ToInt32(view.GetRowCellValue(view.GetRowHandle(e.ListSourceRowIndex), "Diff"));

                e.Value = ToHumanReadable(playTime);
            }
        }

        private void gridViewTicaretKayitlari_CustomUnboundColumnData(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDataEventArgs e)
        {
            if (e.Column.FieldName == "Icon" && e.IsGetData)
            {
                GridView view = sender as GridView;
                var type = view.GetRowCellValue(view.GetRowHandle(e.ListSourceRowIndex), "Type").ToString();

                if (type == "CHARACTER")
                {
                    if (!imageCache.ContainsKey("gold"))
                    {
                        Image img = Image.FromFile(Path.Combine(Environment.CurrentDirectory, "frm_gold.png"));
                        imageCache["gold"] = img;
                    }
                    e.Value = imageCache["gold"];
                    return;
                }

                var fileName = Convert.ToInt32(view.GetRowCellValue(view.GetRowHandle(e.ListSourceRowIndex), "Vnum"));
                if (fileName == 0)
                    return;

                if (!imageCache.ContainsKey(fileName.ToString()))
                {
                    Image img = GetImage(Convert.ToInt32(fileName));

                    var oldFileName = fileName;
                    if (img == null)
                    {
                        fileName = fileName - (fileName % 10);
                        img = GetImage(Convert.ToInt32(fileName));

                        if (img != null)
                        {
                            imageCache[oldFileName.ToString()] = img;
                            imageCache[fileName.ToString()] = img;
                        }
                        else
                        {
                            imageCache[oldFileName.ToString()] = null;
                            imageCache[fileName.ToString()] = null;
                        }
                    }
                    else
                    {
                        imageCache[fileName.ToString()] = img;
                    }

                }

                e.Value = imageCache[fileName.ToString()];
            }
            else if (e.Column.FieldName == "ItemName" && e.IsGetData)
            {
                GridView view = sender as GridView;
                var type = view.GetRowCellValue(view.GetRowHandle(e.ListSourceRowIndex), "Type").ToString();

                if (type == "CHARACTER")
                {
                    var what = view.GetRowCellValue(view.GetRowHandle(e.ListSourceRowIndex), "What").ToString();
                    e.Value = what;
                }
                else
                {
                    var vnum = view.GetRowCellValue(view.GetRowHandle(e.ListSourceRowIndex), "Vnum").ToString();
                    var countNumber = view.GetRowCellValue(view.GetRowHandle(e.ListSourceRowIndex), "Count");
                    var count = Convert.ToInt32(countNumber.ToString());

                    var itemName = LocaleItem.GetEntityByKeyNameKey(vnum.ToString()).LocalizedValue;

                    e.Value = $"{count}x - {itemName}";
                }

            }
        }

        private void gridViewGorevOdulKayitlari_CustomDrawCell(object sender, DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventArgs e)
        {
            if (e.Column.FieldName == "Description")
            {
                if (e.CellValue != null && !string.IsNullOrWhiteSpace(e.CellValue.ToString()))
                    if (e.CellValue.ToString() == "EXP")
                        e.Appearance.BackColor = Color.Aquamarine;
                    else
                        e.Appearance.BackColor = Color.Orange;
            }
        }

        private void gridViewGorevOdulKayitlari_CustomUnboundColumnData(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDataEventArgs e)
        {
            if (e.Column.FieldName == "Icon" && e.IsGetData)
            {
                GridView view = sender as GridView;
                var type = view.GetRowCellValue(view.GetRowHandle(e.ListSourceRowIndex), "Description").ToString();

                if (type != "ITEM")
                    return;

                var fileName = Convert.ToInt32(view.GetRowCellValue(view.GetRowHandle(e.ListSourceRowIndex), "How"));
                if (fileName == 0)
                    return;

                if (!imageCache.ContainsKey(fileName.ToString()))
                {
                    Image img = GetImage(Convert.ToInt32(fileName));

                    var oldFileName = fileName;
                    if (img == null)
                    {
                        fileName = fileName - (fileName % 10);
                        img = GetImage(Convert.ToInt32(fileName));

                        if (img != null)
                        {
                            imageCache[oldFileName.ToString()] = img;
                            imageCache[fileName.ToString()] = img;
                        }
                        else
                        {
                            imageCache[oldFileName.ToString()] = null;
                            imageCache[fileName.ToString()] = null;
                        }
                    }
                    else
                    {
                        imageCache[fileName.ToString()] = img;
                    }

                }

                e.Value = imageCache[fileName.ToString()];
            }
            else if (e.Column.FieldName == "ItemName" && e.IsGetData)
            {
                GridView view = sender as GridView;
                var type = view.GetRowCellValue(view.GetRowHandle(e.ListSourceRowIndex), "Description").ToString();

                if (type == "ITEM")
                {
                    var vnum = view.GetRowCellValue(view.GetRowHandle(e.ListSourceRowIndex), "How").ToString();
                    var countNumber = view.GetRowCellValue(view.GetRowHandle(e.ListSourceRowIndex), "Hint");
                    var count = Convert.ToInt32(countNumber.ToString());

                    var itemName = LocaleItem.GetEntityByKeyNameKey(vnum.ToString()).LocalizedValue;

                    e.Value = $"{count}x - {itemName}";
                }

            }
        }

        private void gridViewEsyaKullanmaKayitlari_CustomUnboundColumnData(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDataEventArgs e)
        {
            if (e.Column.FieldName == "Icon" && e.IsGetData)
            {
                GridView view = sender as GridView;
                var fileName = Convert.ToInt32(view.GetRowCellValue(view.GetRowHandle(e.ListSourceRowIndex), "Vnum"));
                if (fileName == 0)
                    return;

                if (!imageCache.ContainsKey(fileName.ToString()))
                {
                    Image img = GetImage(Convert.ToInt32(fileName));

                    var oldFileName = fileName;
                    if (img == null)
                    {
                        fileName = fileName - (fileName % 10);
                        img = GetImage(Convert.ToInt32(fileName));

                        if (img != null)
                        {
                            imageCache[oldFileName.ToString()] = img;
                            imageCache[fileName.ToString()] = img;
                        }
                        else
                        {
                            imageCache[oldFileName.ToString()] = null;
                            imageCache[fileName.ToString()] = null;
                        }
                    }
                    else
                    {
                        imageCache[fileName.ToString()] = img;
                    }

                }

                e.Value = imageCache[fileName.ToString()];
            }
            else if (e.Column.FieldName == "ItemName" && e.IsGetData)
            {
                GridView view = sender as GridView;
                var vnum = view.GetRowCellValue(view.GetRowHandle(e.ListSourceRowIndex), "Vnum").ToString();
                var itemName = LocaleItem.GetEntityByKeyNameKey(vnum.ToString()).LocalizedValue;

                e.Value = $"{itemName}";

            }
        }

        private void gridViewEsyaToplamaKayitlari_CustomUnboundColumnData(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDataEventArgs e)
        {
            if (e.Column.FieldName == "Icon" && e.IsGetData)
            {
                GridView view = sender as GridView;
                var fileName = Convert.ToInt32(view.GetRowCellValue(view.GetRowHandle(e.ListSourceRowIndex), "Vnum"));
                if (fileName == 0)
                    return;

                if (!imageCache.ContainsKey(fileName.ToString()))
                {
                    Image img = GetImage(Convert.ToInt32(fileName));

                    var oldFileName = fileName;
                    if (img == null)
                    {
                        fileName = fileName - (fileName % 10);
                        img = GetImage(Convert.ToInt32(fileName));

                        if (img != null)
                        {
                            imageCache[oldFileName.ToString()] = img;
                            imageCache[fileName.ToString()] = img;
                        }
                        else
                        {
                            imageCache[oldFileName.ToString()] = null;
                            imageCache[fileName.ToString()] = null;
                        }
                    }
                    else
                    {
                        imageCache[fileName.ToString()] = img;
                    }

                }

                e.Value = imageCache[fileName.ToString()];
            }
            else if (e.Column.FieldName == "ItemName" && e.IsGetData)
            {
                GridView view = sender as GridView;
                var vnum = view.GetRowCellValue(view.GetRowHandle(e.ListSourceRowIndex), "Vnum")?.ToString();
                var itemName = LocaleItem.GetEntityByKeyNameKey(vnum?.ToString())?.LocalizedValue;

                e.Value = $"{itemName}";
            }
        }

        private void gridViewEsyaYukseltmeKayitlari_CustomUnboundColumnData(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDataEventArgs e)
        {
            if (e.Column.FieldName == "Icon" && e.IsGetData)
            {
                GridView view = sender as GridView;
                var fileName = Convert.ToInt32(view.GetRowCellValue(view.GetRowHandle(e.ListSourceRowIndex), "Vnum"));
                if (fileName == 0)
                    return;

                if (!imageCache.ContainsKey(fileName.ToString()))
                {
                    Image img = GetImage(Convert.ToInt32(fileName));

                    var oldFileName = fileName;
                    if (img == null)
                    {
                        fileName = fileName - (fileName % 10);
                        img = GetImage(Convert.ToInt32(fileName));

                        if (img != null)
                        {
                            imageCache[oldFileName.ToString()] = img;
                            imageCache[fileName.ToString()] = img;
                        }
                        else
                        {
                            imageCache[oldFileName.ToString()] = null;
                            imageCache[fileName.ToString()] = null;
                        }
                    }
                    else
                    {
                        imageCache[fileName.ToString()] = img;
                    }

                }

                e.Value = imageCache[fileName.ToString()];
            }
            else if (e.Column.FieldName == "ItemName" && e.IsGetData)
            {
                GridView view = sender as GridView;
                var vnum = view.GetRowCellValue(view.GetRowHandle(e.ListSourceRowIndex), "Vnum").ToString();
                var itemName = LocaleItem.GetEntityByKeyNameKey(vnum.ToString()).LocalizedValue;

                e.Value = $"{itemName}";

            }
        }

        private void gridViewEtkiler_CustomDrawCell(object sender, DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventArgs e)
        {

        }

        private void gridViewEtkiler_RowStyle(object sender, RowStyleEventArgs e)
        {
            if (e.RowHandle == -1)
                return;

            var type = (EnumAffect)Convert.ToInt32(gridViewEtkiler.GetRowCellValue(e.RowHandle, "Type"));

            if (type == EnumAffect.AFFECT_BLOCK_CHAT)
            {
                e.Appearance.BackColor = Color.Red;
                e.Appearance.ForeColor = Color.White;

                e.HighPriority = true;
            }
        }

        private void gridViewEtkiler_CustomUnboundColumnData(object sender,
            DevExpress.XtraGrid.Views.Base.CustomColumnDataEventArgs e)
        {
            if (e.Column.FieldName == "DurationText" && e.IsGetData)
            {
                GridView view = sender as GridView;
                var durationInt =
                    Convert.ToInt32(view.GetRowCellValue(view.GetRowHandle(e.ListSourceRowIndex), "Duration"));

                var type = (EnumAffect)Convert.ToInt32(view.GetRowCellValue(view.GetRowHandle(e.ListSourceRowIndex),
                    "Type"));

                if (type == EnumAffect.AFFECT_BLOCK_CHAT)
                {
                    var duration = ToHumanReadable(durationInt / 60);

                    e.Value = duration.ToString();
                }
                else
                {
                    e.Value = durationInt.ToString();
                }
            }
        }

        private void barButtonItem1_ItemClick_1(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {

            Listele();
        }

        private void btnGet_Click(object sender, EventArgs e)
        {
            try
            {

                var index = (SimpleButton)sender;
                if (index.Tag == "GirisKayitlari") // etkiler
                {
                    var startDate = dateLoginStart.DateTime;
                    var finishDate = dateLoginFinish.DateTime.AddDays(1);

                    ListeleGirisKayitlari(startDate, finishDate);
                }
                else if (index.Tag == "BalikcilikKayitlari") // balikcilik kayitlari
                {
                    var startDate = dateFishStart.DateTime;
                    var finishDate = dateFishFinish.DateTime.AddDays(1);

                    ListeleBalikcilikKayitlari(startDate, finishDate);
                }
                else if (index.Tag == "TicaretKayitlari") // ticaret kayitlari
                {
                    var startDate = dateTradeStart.DateTime;
                    var finishDate = dateTradeFinish.DateTime.AddDays(1);

                    ListeleTicaretKayitlari(startDate, finishDate);
                }
                else if (index.Tag == "GorevOdulKayitlari") // gorev odul kayitlari
                {
                    var startDate = dateQuestRewardStart.DateTime;
                    var finishDate = dateQuestRewardFinish.DateTime.AddDays(1);

                    ListeleGorevOdulKayitlari(startDate, finishDate);
                }
                else if (index.Tag == "EsyaKullanimKayitlari") // esya kullanma sayilari
                {
                    var startDate = dateItemUsingStart.DateTime;
                    var finishDate = dateItemUsingFinish.DateTime.AddDays(1);

                    ListeleEsyaKullanmaKayitlari(startDate, finishDate);
                }
                else if (index.Tag == "EsyaToplamaKayitlari") // esya toplama kayitlari
                {
                    var startDate = dateItemPickStart.DateTime;
                    var finishDate = dateItemPickFinish.DateTime.AddDays(1);

                    ListeleEsyaToplamaKayitlari(startDate, finishDate);
                }
                else if (index.Tag == "EsyaYukseltmeKayitlari") // esya yukseltme kayitlari
                {
                    var startDate = dateItemUpgradeStart.DateTime;
                    var finishDate = dateItemUpgradeFinish.DateTime.AddDays(1);

                    ListeleEsyaYukseltmeKayitlari(startDate, finishDate);
                }
                else if (index.Tag == "DepoKayitlari") // depo kayitlari
                {
                    var startDate = dateStorageStart.DateTime;
                    var finishDate = dateStorageFinish.DateTime.AddDays(1);

                    ListeleDepoKayitlari(startDate, finishDate);
                    ListeleAktifDepoKayitlari();
                    ListeleAktifNesneDepoKayitlari();
                }
                else if (index.Tag == "PazarKayitlari") // depo kayitlari
                {
                    var startDate = dateSaleItemStart.DateTime;
                    var finishDate = dateSaleItemFinish.DateTime.AddDays(1);

                    ListeleAktifPazarUstBilgileri();
                    KisiselDukkanDepoKayitlari();
                    ListeleAktifPazarKayitlari();
                    ListeleAktifPazarLogKayitlari(startDate, finishDate);
                }
            }
            catch (Exception ex)
            {

            }
        }

        private void gridView_KeyDown(object sender, KeyEventArgs e)
        {
            GridView view = sender as GridView;
            if (e.Control && e.KeyCode == Keys.C)
            {
                if (view.GetRowCellValue(view.FocusedRowHandle, view.FocusedColumn) != null && view.GetRowCellValue(view.FocusedRowHandle, view.FocusedColumn).ToString() != String.Empty)
                    Clipboard.SetText(view.GetRowCellValue(view.FocusedRowHandle, view.FocusedColumn).ToString());
                else
                    MessageBox.Show("The value in the selected cell is null or empty!");
                e.Handled = true;
            }
        }

        private void gridViewDepoKayitlari_CustomColumnDisplayText(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDisplayTextEventArgs e)
        {
            if (e.Column.FieldName == "How")
            {
                if (e.Value != null && e.Value.ToString() == "SAFEBOX PUT")
                {
                    e.DisplayText = "Depoya Aktarıldı";
                }
                else
                {
                    e.DisplayText = "Depodan Alındı";
                }
            }
        }

        private void gridViewDepoKayitlari_RowStyle(object sender, RowStyleEventArgs e)
        {
            GridView view = sender as GridView;

            if (e.RowHandle >= 0)
            {
                string status = view.GetRowCellValue(e.RowHandle, view.Columns["How"]).ToString();

                if (status == "SAFEBOX PUT")
                {
                    e.Appearance.BackColor = ColorTranslator.FromHtml("#FFECAF");
                    e.Appearance.BackColor2 = ColorTranslator.FromHtml("#FFECAF");
                }
                else if (status == "SAFEBOX GET")
                {
                    e.Appearance.BackColor = ColorTranslator.FromHtml("#DCFFCD");
                    e.Appearance.BackColor2 = ColorTranslator.FromHtml("#DCFFCD");
                }
            }
        }

        private void xtraTabControl1_CustomDrawHeaderButton(object sender, DevExpress.XtraTab.HeaderButtonCustomDrawEventArgs e)
        {
           
        }

        private void gridControl41_Click(object sender, EventArgs e)
        {

        }

        private void gridView4_RowStyle(object sender, RowStyleEventArgs e)
        {
            GridView view = sender as GridView;

            if (e.RowHandle >= 0)
            {
                string status = view.GetRowCellValue(e.RowHandle, view.Columns["Process"])?.ToString();

                if (status == "SATICIYA YANG AKTARILDI")
                {
                    e.Appearance.BackColor = ColorTranslator.FromHtml("#FFF1B2");
                    e.Appearance.BackColor2 = ColorTranslator.FromHtml("#FFF1B2");
                }
                else if (status == "ÜRÜN PAZARDAN SATILDI")
                {
                    e.Appearance.BackColor = ColorTranslator.FromHtml("#DCFFCD");
                    e.Appearance.BackColor2 = ColorTranslator.FromHtml("#DCFFCD");
                }
                else if (status == "PAZARDA SATIŞA EKLENDİ")
                {
                    e.Appearance.BackColor = ColorTranslator.FromHtml("#CFE9FF");
                    e.Appearance.BackColor2 = ColorTranslator.FromHtml("#CFE9FF");
                }
                else if (status == "PAZARDAN ÇIKARILDI")
                {
                    e.Appearance.BackColor = ColorTranslator.FromHtml("#FFCADC");
                    e.Appearance.BackColor2 = ColorTranslator.FromHtml("#FFCADC");
                }
                else if (status == "KİŞİSEL DÜKKANDAN YANG ÇEKİLDİ")
                {
                    e.Appearance.BackColor = ColorTranslator.FromHtml("#FFD28E");
                    e.Appearance.BackColor2 = ColorTranslator.FromHtml("#FFD28E");
                }
                else if (status == "ÜRÜN FİYATI DEĞİŞTİRİLDİ")
                {
                    e.Appearance.BackColor = ColorTranslator.FromHtml("#ECFFCA");
                    e.Appearance.BackColor2 = ColorTranslator.FromHtml("#ECFFCA");
                }
                else if (status == "KİŞİSEL DÜKKANDAN ITEM ÇEKİLDİ")
                {
                    e.Appearance.BackColor = ColorTranslator.FromHtml("#DBCAFF");
                    e.Appearance.BackColor2 = ColorTranslator.FromHtml("#DBCAFF");
                }
                else if (status == "PAZARDAN SATIN ALINDI")
                {
                    e.Appearance.BackColor = ColorTranslator.FromHtml("#FFCECE");
                    e.Appearance.BackColor2 = ColorTranslator.FromHtml("#FFCECE");
                }
            }
        }

        private void gridViewMarketLog_CustomColumnDisplayText(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDisplayTextEventArgs e)
        {
            if (e.Column.FieldName == "Hint")
            {
                string cellValue = e.Value?.ToString();
                if (!string.IsNullOrEmpty(cellValue))
                {
                    string[] parts = cellValue.Split(',');
                    foreach (var item in parts)
                    {
                        if (item.Contains("Yang") || item.Contains("Price"))
                        {
                            var newPart = item.ToString().Replace("Price: ", "").Replace("Yang: ", "").Replace(" (excluding tax)","").Replace(" (-tax)","");
                            string[] priceParts = newPart.Split('>');
                            foreach (var priceItem in priceParts)
                            {
                                if (!string.IsNullOrEmpty(priceItem))
                                {
                                    var newPriceItem = priceItem.ToString().Replace("-", "");
                                    if (decimal.TryParse(newPriceItem?.ToString(), out decimal value))
                                    {
                                        var newGoldValue = value.ToString("N0");
                                        cellValue = cellValue.Replace(priceItem, " " + newGoldValue);
                                        e.DisplayText = cellValue;
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
    }

}