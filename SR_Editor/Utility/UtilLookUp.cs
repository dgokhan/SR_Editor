using DevExpress.Data;
using DevExpress.Utils;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraGrid.Views.Grid;
using SR_Editor.Core.Utility;
using SR_Editor.LookUp.Base;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Forms;

namespace SR_Editor.Core
{
	public static class UtilLookUp
	{
		private static string GetNullText()
		{
			string str;
			return "Choose";
			return str;
		}

		public static void InitLookupEdit(LookUpEdit lookUpEdit)
		{
			UtilLookUp.InitLookupEdit(lookUpEdit, "Id", "Aciklama");
		}

		public static void InitLookupEdit(LookUpEdit lookUpEdit, string valueMember, string displayMember)
		{
			UtilLookUp.InitLookupEdit(lookUpEdit, valueMember, displayMember, UtilLookUp.GetNullText());
		}

		public static void InitLookupEdit(LookUpEdit lookUpEdit, string valueMember, string displayMember, string nullText)
		{
			lookUpEdit.Properties.ValueMember = valueMember;
			lookUpEdit.Properties.DisplayMember = displayMember;
			lookUpEdit.Properties.Columns.Clear();
			LookUpColumnInfoCollection columns = lookUpEdit.Properties.Columns;
			LookUpColumnInfo[] lookUpColumnInfo = new LookUpColumnInfo[] { new LookUpColumnInfo(lookUpEdit.Properties.ValueMember, "", 5, FormatType.None, "", false, HorzAlignment.Default, ColumnSortOrder.None), new LookUpColumnInfo(lookUpEdit.Properties.DisplayMember) };
			columns.AddRange(lookUpColumnInfo);
			lookUpEdit.Properties.ShowHeader = false;
			lookUpEdit.Properties.ShowFooter = false;
			lookUpEdit.Properties.NullText = nullText;
			lookUpEdit.ButtonClick += new ButtonPressedEventHandler(UtilLookUp.lookUpEdit_ButtonClick);
		}

		public static void InitLookupEdit(CheckedComboBoxEdit checkedComboBoxEdit)
		{
			UtilLookUp.InitLookupEdit(checkedComboBoxEdit, "Id", "Aciklama");
		}

		public static void InitLookupEdit(CheckedComboBoxEdit checkedComboBoxEdit, string valueMember, string displayMember)
		{
			UtilLookUp.InitLookupEdit(checkedComboBoxEdit, valueMember, displayMember, UtilLookUp.GetNullText());
		}

		public static void InitLookupEdit(CheckedComboBoxEdit checkedComboBoxEdit, string valueMember, string displayMember, string nullText)
		{
			checkedComboBoxEdit.Properties.ValueMember = valueMember;
			checkedComboBoxEdit.Properties.DisplayMember = displayMember;
			checkedComboBoxEdit.Properties.NullText = nullText;
		}

		public static void InitLookupEdit(RepositoryItemLookUpEdit lookUpEdit)
		{
			UtilLookUp.InitLookupEdit(lookUpEdit, "Id", "Aciklama");
		}

		public static void InitLookupEdit(RepositoryItemLookUpEdit lookUpEdit, string valueMember, string displayMember)
		{
			UtilLookUp.InitLookupEdit(lookUpEdit, valueMember, displayMember, UtilLookUp.GetNullText());
		}

		public static void InitLookupEdit(RepositoryItemLookUpEdit lookUpEdit, string valueMember, string displayMember, string nullText)
		{
			lookUpEdit.ValueMember = valueMember;
			lookUpEdit.DisplayMember = displayMember;
			lookUpEdit.Columns.Clear();
			LookUpColumnInfoCollection columns = lookUpEdit.Columns;
			LookUpColumnInfo[] lookUpColumnInfo = new LookUpColumnInfo[] { new LookUpColumnInfo(lookUpEdit.ValueMember, "", 5, FormatType.None, "", false, HorzAlignment.Default, ColumnSortOrder.None), new LookUpColumnInfo(lookUpEdit.DisplayMember) };
			columns.AddRange(lookUpColumnInfo);
			lookUpEdit.ShowHeader = false;
			lookUpEdit.ShowFooter = false;
			lookUpEdit.NullText = nullText;
		}

		public static void InitLookupEdit(RepositoryItemGridLookUpEdit repositoryItemGridLookUpEdit)
		{
			UtilLookUp.InitLookupEdit(repositoryItemGridLookUpEdit, "Id", "Aciklama");
		}

		public static void InitLookupEdit(RepositoryItemGridLookUpEdit repositoryItemGridLookUpEdit, string valueMember, string displayMember)
		{
			UtilLookUp.InitLookupEdit(repositoryItemGridLookUpEdit, valueMember, displayMember, UtilLookUp.GetNullText());
		}

		public static void InitLookupEdit(RepositoryItemGridLookUpEdit repositoryItemGridLookUpEdit, string valueMember, string displayMember, string nullText)
		{
			repositoryItemGridLookUpEdit.ValueMember = valueMember;
			repositoryItemGridLookUpEdit.DisplayMember = displayMember;
			repositoryItemGridLookUpEdit.View.FocusRectStyle = DrawFocusRectStyle.RowFocus;
			repositoryItemGridLookUpEdit.View.OptionsBehavior.Editable = false;
			repositoryItemGridLookUpEdit.View.OptionsView.ColumnAutoWidth = false;
			repositoryItemGridLookUpEdit.View.OptionsView.ShowFooter = false;
			repositoryItemGridLookUpEdit.View.OptionsView.ShowGroupPanel = false;
			repositoryItemGridLookUpEdit.View.OptionsView.ShowIndicator = false;
			repositoryItemGridLookUpEdit.View.OptionsView.ShowDetailButtons = false;
			repositoryItemGridLookUpEdit.View.OptionsSelection.EnableAppearanceFocusedCell = false;
			repositoryItemGridLookUpEdit.ShowFooter = false;
			repositoryItemGridLookUpEdit.NullText = nullText;
		}

		public static void InitLookupEdit(GridLookUpEdit repositoryItemGridLookUpEdit)
		{
			UtilLookUp.InitLookupEdit(repositoryItemGridLookUpEdit, "Id", "Aciklama");
		}

		public static void InitLookupEdit(GridLookUpEdit repositoryItemGridLookUpEdit, string valueMember, string displayMember)
		{
			UtilLookUp.InitLookupEdit(repositoryItemGridLookUpEdit, valueMember, displayMember, UtilLookUp.GetNullText());
		}

		public static void InitLookupEdit(GridLookUpEdit repositoryItemGridLookUpEdit, string valueMember, string displayMember, string nullText)
		{
			repositoryItemGridLookUpEdit.Properties.ValueMember = valueMember;
			repositoryItemGridLookUpEdit.Properties.DisplayMember = displayMember;
			repositoryItemGridLookUpEdit.Properties.View.FocusRectStyle = DrawFocusRectStyle.RowFocus;
			repositoryItemGridLookUpEdit.Properties.View.OptionsBehavior.Editable = false;
			repositoryItemGridLookUpEdit.Properties.View.OptionsView.ColumnAutoWidth = false;
			repositoryItemGridLookUpEdit.Properties.View.OptionsView.ShowFooter = false;
			repositoryItemGridLookUpEdit.Properties.View.OptionsView.ShowGroupPanel = false;
			repositoryItemGridLookUpEdit.Properties.View.OptionsView.ShowIndicator = false;
			repositoryItemGridLookUpEdit.Properties.View.OptionsView.ShowDetailButtons = false;
			repositoryItemGridLookUpEdit.Properties.View.OptionsSelection.EnableAppearanceFocusedCell = false;
			repositoryItemGridLookUpEdit.Properties.ShowFooter = false;
			repositoryItemGridLookUpEdit.Properties.NullText = nullText;
		}

		public static void InitLookupEdit(RepositoryItemCheckedComboBoxEdit repositoryItemCheckedComboBoxEdit)
		{
			UtilLookUp.InitLookupEdit(repositoryItemCheckedComboBoxEdit, "Id", "Adi");
		}

		public static void InitLookupEdit(RepositoryItemCheckedComboBoxEdit repositoryItemCheckedComboBoxEdit, string valueMember, string displayMember)
		{
			UtilLookUp.InitLookupEdit(repositoryItemCheckedComboBoxEdit, valueMember, displayMember, UtilLookUp.GetNullText());
		}

		public static void InitLookupEdit(RepositoryItemCheckedComboBoxEdit repositoryItemCheckedComboBoxEdit, string valueMember, string displayMember, string nullText)
		{
			repositoryItemCheckedComboBoxEdit.ValueMember = valueMember;
			repositoryItemCheckedComboBoxEdit.DisplayMember = displayMember;
			repositoryItemCheckedComboBoxEdit.NullText = nullText;
		}

		public static void LoadLookUpEdit(LookUpEdit pLookUpEdit, string pTableName, bool pShowAllState)
		{
			UtilLookUp.LoadLookUpEdit(pLookUpEdit, pTableName, "Id", "Adi", "", pShowAllState);
		}

		public static void LoadLookUpEdit(LookUpEdit pLookUpEdit, string pTableName, string pWhere, bool pShowAllState)
		{
			UtilLookUp.LoadLookUpEdit(pLookUpEdit, pTableName, "Id", "Adi", pWhere, pShowAllState);
		}

		public static void LoadLookUpEdit(LookUpEdit pLookUpEdit, string pTableName, bool pShowAllState, LookUpEdit pParentLookUpEdit, string pParentColumnName)
		{
			UtilLookUp.LoadLookUpEdit(pLookUpEdit, pTableName, "", pShowAllState, pParentLookUpEdit, pParentColumnName);
		}

		public static void LoadLookUpEdit(LookUpEdit pLookUpEdit, string pTableName, string pWhere, bool pShowAllState, LookUpEdit pParentLookUpEdit, string pParentColumnName)
		{
			RelatedLookUpEdit relatedLookUpEdit = new RelatedLookUpEdit()
			{
				LookUpEdit = pLookUpEdit,
				TableName = pTableName,
				KeyColumn = "Id",
				ValueColumn = "Adi",
				Where = pWhere,
				ShowAllState = pShowAllState,
				ParentColumnName = pParentColumnName
			};
			pParentLookUpEdit.Tag = relatedLookUpEdit;
			UtilLookUp.InitLookupEdit(pLookUpEdit);
			pParentLookUpEdit.EditValueChanged += new EventHandler(UtilLookUp.ParentLookUpEdit_EditValueChanged);
		}

		public static void LoadLookUpEdit(LookUpEdit pLookUpEdit, string pTableName, string pKeyColumn, string pValueColumn, string pWhere, bool pShowAllState, LookUpEdit pParentLookUpEdit, string pParentColumnName)
		{
			RelatedLookUpEdit relatedLookUpEdit = new RelatedLookUpEdit()
			{
				LookUpEdit = pLookUpEdit,
				TableName = pTableName,
				KeyColumn = pKeyColumn,
				ValueColumn = pValueColumn,
				Where = pWhere,
				ShowAllState = pShowAllState,
				ParentColumnName = pParentColumnName
			};
			pParentLookUpEdit.Tag = relatedLookUpEdit;
			UtilLookUp.InitLookupEdit(pLookUpEdit);
			pParentLookUpEdit.EditValueChanged += new EventHandler(UtilLookUp.ParentLookUpEdit_EditValueChanged);
		}

		public static void LoadLookUpEdit(LookUpEdit pLookUpEdit, string pTableName, string pKeyColumn, string pValueColumn, string pWhere, bool pShowAllState)
		{
			if ((string.IsNullOrEmpty(pWhere) ? false : !pShowAllState))
			{
				pWhere = string.Concat(pWhere, " And ");
			}
			if (!pShowAllState)
			{
				pWhere = string.Concat(pWhere, " State <> 0");
			}
			UtilLookUp.LoadLookUpEdit(pLookUpEdit, pTableName, pKeyColumn, pValueColumn, pWhere);
		}

		private static void LoadLookUpEdit(LookUpEdit pLookUpEdit, string pTableName, string pKeyColumn, string pValueColumn, string pWhere)
		{
			UtilLookUp.InitLookupEdit(pLookUpEdit);
			/*List<LookUpEntityBase<string>> lookUpEntity = (new Silkroad.SilkroadModel()).GetLookUpEntity(pTableName, pKeyColumn, pValueColumn, pWhere);
			pLookUpEdit.Properties.DataSource = lookUpEntity;
			if (!pTableName.Contains("Editor."))
			{
				pLookUpEdit.EditValueChanged += new EventHandler(UtilLookUp.pLookUpEdit_EditValueChanged);
			}*/
		}

		public static void LoadLookUpEdit(CheckedComboBoxEdit checkedComboBoxEdit, string pTableName, bool pShowAllState)
		{
			UtilLookUp.LoadLookUpEdit(checkedComboBoxEdit, pTableName, "Id", "Adi", "", pShowAllState);
		}

		public static void LoadLookUpEdit(CheckedComboBoxEdit checkedComboBoxEdit, string pTableName, string pWhere, bool pShowAllState)
		{
			UtilLookUp.LoadLookUpEdit(checkedComboBoxEdit, pTableName, "Id", "Adi", pWhere, pShowAllState);
		}

		public static void LoadLookUpEdit(CheckedComboBoxEdit checkedComboBoxEdit, string pTableName, string pKeyColumn, string pValueColumn, string pWhere, bool pShowAllState)
		{
			if ((string.IsNullOrEmpty(pWhere) ? false : !pShowAllState))
			{
				pWhere = string.Concat(pWhere, " And ");
			}
			if (!pShowAllState)
			{
				pWhere = string.Concat(pWhere, " State <> 0");
			}
			UtilLookUp.LoadLookUpEdit(checkedComboBoxEdit, pTableName, pKeyColumn, pValueColumn, pWhere);
		}

		private static void LoadLookUpEdit(CheckedComboBoxEdit checkedComboBoxEdit, string pTableName, string pKeyColumn, string pValueColumn, string pWhere)
		{
			UtilLookUp.InitLookupEdit(checkedComboBoxEdit);
			//checkedComboBoxEdit.Properties.DataSource = (new Silkroad.SilkroadModel()).GetLookUpEntity(pTableName, pKeyColumn, pValueColumn, pWhere);
		}

		public static void LoadLookUpEdit(RepositoryItemLookUpEdit pLookUpEdit, string pTableName, bool pShowAllState)
		{
			UtilLookUp.LoadLookUpEdit(pLookUpEdit, pTableName, "Id", "Adi", "", pShowAllState);
		}

		public static void LoadLookUpEdit(RepositoryItemLookUpEdit pLookUpEdit, string pTableName, string pWhere, bool pShowAllState)
		{
			UtilLookUp.LoadLookUpEdit(pLookUpEdit, pTableName, "Id", "Adi", pWhere, pShowAllState);
		}

		public static void LoadLookUpEdit(RepositoryItemLookUpEdit pLookUpEdit, string pTableName, string pKeyColumn, string pValueColumn, string pWhere, bool pShowAllState)
		{
			if ((string.IsNullOrEmpty(pWhere) ? false : !pShowAllState))
			{
				pWhere = string.Concat(pWhere, " And ");
			}
			if (!pShowAllState)
			{
				pWhere = string.Concat(pWhere, " State <> 0");
			}
			UtilLookUp.LoadLookUpEdit(pLookUpEdit, pTableName, pKeyColumn, pValueColumn, pWhere);
		}

		private static void LoadLookUpEdit(RepositoryItemLookUpEdit pLookUpEdit, string pTableName, string pKeyColumn, string pValueColumn, string pWhere)
		{
			UtilLookUp.InitLookupEdit(pLookUpEdit);
			//pLookUpEdit.DataSource = (new Silkroad.SilkroadModel()).GetLookUpEntity(pTableName, pKeyColumn, pValueColumn, pWhere);
		}

		public static void LoadLookUpEdit(RepositoryItemGridLookUpEdit pRepositoryItemGridLookUpEdit, string pTableName, bool pShowAllState)
		{
			UtilLookUp.LoadLookUpEdit(pRepositoryItemGridLookUpEdit, pTableName, "Id", "Adi", "", pShowAllState);
		}

		public static void LoadLookUpEdit(RepositoryItemGridLookUpEdit pRepositoryItemGridLookUpEdit, string pTableName, string pWhere, bool pShowAllState)
		{
			UtilLookUp.LoadLookUpEdit(pRepositoryItemGridLookUpEdit, pTableName, "Id", "Adi", pWhere, pShowAllState);
		}

		public static void LoadLookUpEdit(RepositoryItemGridLookUpEdit pRepositoryItemGridLookUpEdit, string pTableName, string pKeyColumn, string pValueColumn, string pWhere, bool pShowAllState)
		{
			if ((string.IsNullOrEmpty(pWhere) ? false : !pShowAllState))
			{
				pWhere = string.Concat(pWhere, " And ");
			}
			if (!pShowAllState)
			{
				pWhere = string.Concat(pWhere, " State <> 0");
			}
			UtilLookUp.LoadLookUpEdit(pRepositoryItemGridLookUpEdit, pTableName, pKeyColumn, pValueColumn, pWhere);
		}

		private static void LoadLookUpEdit(RepositoryItemGridLookUpEdit pRepositoryItemGridLookUpEdit, string pTableName, string pKeyColumn, string pValueColumn, string pWhere)
		{
			UtilLookUp.InitLookupEdit(pRepositoryItemGridLookUpEdit);
			//pRepositoryItemGridLookUpEdit.DataSource = (new Silkroad.SilkroadModel()).GetLookUpEntity(pTableName, pKeyColumn, pValueColumn, pWhere);
		}

		public static void LoadLookUpEdit(GridLookUpEdit pRepositoryItemGridLookUpEdit, string pTableName, bool pShowAllState)
		{
			UtilLookUp.LoadLookUpEdit(pRepositoryItemGridLookUpEdit, pTableName, "Id", "Adi", "", pShowAllState);
		}

		public static void LoadLookUpEdit(GridLookUpEdit pRepositoryItemGridLookUpEdit, string pTableName, string pWhere, bool pShowAllState)
		{
			UtilLookUp.LoadLookUpEdit(pRepositoryItemGridLookUpEdit, pTableName, "Id", "Adi", pWhere, pShowAllState);
		}

		public static void LoadLookUpEdit(GridLookUpEdit pRepositoryItemGridLookUpEdit, string pTableName, string pKeyColumn, string pValueColumn, string pWhere, bool pShowAllState)
		{
			if ((string.IsNullOrEmpty(pWhere) ? false : !pShowAllState))
			{
				pWhere = string.Concat(pWhere, " And ");
			}
			if (!pShowAllState)
			{
				pWhere = string.Concat(pWhere, " State <> 0");
			}
			UtilLookUp.LoadLookUpEdit(pRepositoryItemGridLookUpEdit, pTableName, pKeyColumn, pValueColumn, pWhere);
		}

		private static void LoadLookUpEdit(GridLookUpEdit pRepositoryItemGridLookUpEdit, string pTableName, string pKeyColumn, string pValueColumn, string pWhere)
		{
			UtilLookUp.InitLookupEdit(pRepositoryItemGridLookUpEdit);
			//pRepositoryItemGridLookUpEdit.Properties.DataSource = (new Silkroad.SilkroadModel()).GetLookUpEntity(pTableName, pKeyColumn, pValueColumn, pWhere);
		}

		public static void LoadLookUpEdit(RepositoryItemCheckedComboBoxEdit repositoryItemCheckedComboBoxEdit, string pTableName, bool pShowAllState)
		{
			UtilLookUp.LoadLookUpEdit(repositoryItemCheckedComboBoxEdit, pTableName, "Id", "Adi", "", pShowAllState);
		}

		public static void LoadLookUpEdit(RepositoryItemCheckedComboBoxEdit repositoryItemCheckedComboBoxEdit, string pTableName, string pWhere, bool pShowAllState)
		{
			UtilLookUp.LoadLookUpEdit(repositoryItemCheckedComboBoxEdit, pTableName, "Id", "Adi", pWhere, pShowAllState);
		}

		public static void LoadLookUpEdit(RepositoryItemCheckedComboBoxEdit repositoryItemCheckedComboBoxEdit, string pTableName, string pKeyColumn, string pValueColumn, string pWhere, bool pShowAllState)
		{
			if ((string.IsNullOrEmpty(pWhere) ? false : !pShowAllState))
			{
				pWhere = string.Concat(pWhere, " And ");
			}
			if (!pShowAllState)
			{
				pWhere = string.Concat(pWhere, " State <> 0");
			}
			UtilLookUp.LoadLookUpEdit(repositoryItemCheckedComboBoxEdit, pTableName, pKeyColumn, pValueColumn, pWhere);
		}

		private static void LoadLookUpEdit(RepositoryItemCheckedComboBoxEdit repositoryItemCheckedComboBoxEdit, string pTableName, string pKeyColumn, string pValueColumn, string pWhere)
		{
			UtilLookUp.InitLookupEdit(repositoryItemCheckedComboBoxEdit, pKeyColumn, pValueColumn);
			//repositoryItemCheckedComboBoxEdit.DataSource = (new Silkroad.SilkroadModel()).GetLookUpEntity(pTableName, pKeyColumn, pValueColumn, pWhere);
		}

		private static void lookUpEdit_ButtonClick(object sender, ButtonPressedEventArgs e)
		{
			if (e.Button.Kind == ButtonPredefines.Delete)
			{
				((LookUpEdit)sender).EditValue = null;
			}
		}

		public static void ParentLookUpEdit_EditValueChanged(object sender, EventArgs e)
		{
			LookUpEdit lookUpEdit = (LookUpEdit)sender;
			if (lookUpEdit.Tag is RelatedLookUpEdit)
			{
				RelatedLookUpEdit tag = (RelatedLookUpEdit)lookUpEdit.Tag;
				if ((lookUpEdit.EditValue == null ? true : lookUpEdit.EditValue == DBNull.Value))
				{
					tag.LookUpEdit.Properties.DataSource = null;
				}
				else if (!(lookUpEdit.EditValue.ToString() == "0"))
				{
					string where = tag.Where;
					if (!string.IsNullOrEmpty(where))
					{
						where = string.Concat(where, " And ");
					}
					where = string.Concat(where, tag.ParentColumnName, " = ", lookUpEdit.EditValue.ToString());
					UtilLookUp.LoadLookUpEdit(tag.LookUpEdit, tag.TableName, tag.KeyColumn, tag.ValueColumn, where, tag.ShowAllState);
				}
				else
				{
					lookUpEdit.EditValue = null;
				}
			}
		}

		public static void pLookUpEdit_EditValueChanged(object sender, EventArgs e)
		{
			LookUpEdit lookUpEdit = (LookUpEdit)sender;
			if ((lookUpEdit.EditValue == null || lookUpEdit.EditValue == DBNull.Value ? false : lookUpEdit.EditValue.ToString() == "0"))
			{
				lookUpEdit.EditValue = null;
			}
		}
	}
}