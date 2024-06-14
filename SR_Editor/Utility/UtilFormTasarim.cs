using DevExpress.Utils;
using DevExpress.XtraGrid.Views.BandedGrid;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraTreeList;
using SR_Editor.Core;
//using SR_Editor.Core.EF;
//using SR_Editor.Core.EF.Query;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace SR_Editor.Core.Utility
{
	public class UtilFormTasarim
	{
		public UtilFormTasarim()
		{
		}

		public static void GridViewSablonSil(GridView pGridView, string pFormKodu)
		{
			CoreEntities.Instance.FormTasarimQuery.AktifFormTasarimlariniIptalEt(pFormKodu, pGridView.Name, null);
		}

		public static void GridViewTasarimiKaydet(GridView pGridView, string pFormKodu)
		{
			CoreEntities coreEntity = CoreEntities.Yeni();
			FormTasarim byFormKoduComponentAdiAndTypeId = coreEntity.FormTasarimQuery.GetByFormKoduComponentAdiAndTypeId(pFormKodu, pGridView.Name, 2, true);
			MemoryStream memoryStream = new MemoryStream();
			pGridView.SaveLayoutToStream(memoryStream);
			memoryStream.Position = (long)0;
			if (!byFormKoduComponentAdiAndTypeId.IsBos())
			{
				byFormKoduComponentAdiAndTypeId.Deger = memoryStream.ToArray();
			}
			else
			{
				coreEntity.FormTasarimAdapter.Yeni(pFormKodu, null, 2, pGridView.Name, memoryStream.ToArray());
			}
			coreEntity.SaveChanges();
			coreEntity = null;
		}

		public static void GridViewTasarimiYukle(GridView pGridView, string pFormKodu, bool pIsSadeceFont)
		{
			FontConverter fontConverter;
			foreach (FormTasarim byGridTasarimYukel in CoreEntities.Instance.FormTasarimQuery.GetByGridTasarimYukel(pFormKodu, pGridView.Name))
			{
				if (byGridTasarimYukel.TypeId == 2)
				{
					if (!pIsSadeceFont)
					{
						MemoryStream memoryStream = new MemoryStream(byGridTasarimYukel.Deger)
						{
							Position = (long)0
						};
						pGridView.RestoreLayoutFromStream(memoryStream);
						memoryStream = null;
					}
				}
				else if (byGridTasarimYukel.TypeId == 4)
				{
					fontConverter = new FontConverter();
					pGridView.Appearance.HeaderPanel.Font = fontConverter.ConvertFromString(Encoding.UTF8.GetString(byGridTasarimYukel.Deger)) as Font;
				}
				else if (byGridTasarimYukel.TypeId == 5)
				{
					fontConverter = new FontConverter();
					pGridView.Appearance.Row.Font = fontConverter.ConvertFromString(Encoding.UTF8.GetString(byGridTasarimYukel.Deger)) as Font;
				}
				else if ((byGridTasarimYukel.TypeId != 6 ? false : pGridView is BandedGridView))
				{
					fontConverter = new FontConverter();
					(pGridView as BandedGridView).Appearance.BandPanel.Font = fontConverter.ConvertFromString(Encoding.UTF8.GetString(byGridTasarimYukel.Deger)) as Font;
				}
			}
		}

		public static void KolonFontKaydet(GridView pGridView, byte pTypeId, string pFormKodu, byte[] pDeger)
		{
			CoreEntities coreEntity = CoreEntities.Yeni();
			FormTasarim byFormKoduComponentAdiAndTypeId = coreEntity.FormTasarimQuery.GetByFormKoduComponentAdiAndTypeId(pFormKodu, pGridView.Name, pTypeId, true);
			if (!byFormKoduComponentAdiAndTypeId.IsBos())
			{
				byFormKoduComponentAdiAndTypeId.Deger = pDeger;
			}
			else
			{
				coreEntity.FormTasarimAdapter.Yeni(pFormKodu, null, pTypeId, pGridView.Name, pDeger);
			}
			coreEntity.SaveChanges();
			coreEntity = null;
		}

		public static void TreeListSablonSil(TreeList pTreeList, string pFormKodu)
		{
			CoreEntities.Instance.FormTasarimQuery.AktifFormTasarimlariniIptalEt(pFormKodu, pTreeList.Name, new byte?(3));
		}

		public static void TreeListTasarimiKaydet(TreeList pTreeList, string pFormKodu)
		{
			CoreEntities coreEntity = CoreEntities.Yeni();
			FormTasarim byFormKoduComponentAdiAndTypeId = coreEntity.FormTasarimQuery.GetByFormKoduComponentAdiAndTypeId(pFormKodu, pTreeList.Name, 3, true);
			MemoryStream memoryStream = new MemoryStream();
			pTreeList.SaveLayoutToStream(memoryStream);
			memoryStream.Position = (long)0;
			if (!byFormKoduComponentAdiAndTypeId.IsBos())
			{
				byFormKoduComponentAdiAndTypeId.Deger = memoryStream.ToArray();
			}
			else
			{
				coreEntity.FormTasarimAdapter.Yeni(pFormKodu, null, 3, pTreeList.Name, memoryStream.ToArray());
			}
			coreEntity.SaveChanges();
			coreEntity = null;
		}

		public static void TreeListTasarimiYukle(TreeList pTreeList, string pFormKodu)
		{
			FormTasarim byFormKoduComponentAdiAndTypeId = CoreEntities.Instance.FormTasarimQuery.GetByFormKoduComponentAdiAndTypeId(pFormKodu, pTreeList.Name, 3, false);
			if (byFormKoduComponentAdiAndTypeId.IsDolu())
			{
				MemoryStream memoryStream = new MemoryStream(byFormKoduComponentAdiAndTypeId.Deger)
				{
					Position = (long)0
				};
				pTreeList.RestoreLayoutFromStream(memoryStream);
				memoryStream = null;
			}
		}
	}
}