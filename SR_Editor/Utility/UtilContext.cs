using SR_Editor.Core.Base;
using SR_Editor.Core.EF;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.Objects;
using System.Data.Objects.DataClasses;

namespace SR_Editor.Core
{
	public class UtilContext : IDisposable
	{
		private ObjectContext context;

		public UtilContext(ObjectContext context)
		{
			this.context = context;
			this.context.SavingChanges += new EventHandler(this.WhenSavingChanges);
		}

		public void DiscardChanges()
		{
			foreach (ObjectStateEntry objectStateEntry in this.context.ObjectStateManager.GetObjectStateEntries(EntityState.Added | EntityState.Deleted | EntityState.Modified))
			{
				this.DiscardChanges(objectStateEntry);
			}
		}

		public void DiscardChanges(IEntityWithKey pEntity)
		{
			ObjectStateEntry objectStateEntry = this.context.ObjectStateManager.GetObjectStateEntry(pEntity.EntityKey);
			this.DiscardChanges(objectStateEntry);
		}

		private void DiscardChanges(ObjectStateEntry pEntry)
		{
			EntityState state = pEntry.State;
			if (state == EntityState.Added)
			{
				pEntry.ChangeState(EntityState.Detached);
			}
			else if (state == EntityState.Deleted)
			{
				pEntry.ChangeState(EntityState.Unchanged);
			}
			else if (state == EntityState.Modified)
			{
				for (int i = 0; i < pEntry.OriginalValues.FieldCount; i++)
				{
					pEntry.CurrentValues.SetValue(i, pEntry.OriginalValues[i]);
				}
				pEntry.AcceptChanges();
			}
		}

		public void Dispose()
		{
			if (this.context != null)
			{
				this.context.SavingChanges -= new EventHandler(this.WhenSavingChanges);
				this.context = null;
			}
		}

		protected virtual int GetLoginId()
		{
			return 2;
		}

		private void WhenSavingChanges(object sender, EventArgs e)
		{
			//ObjectStateEntry objectStateEntry = null;
			IEntityBase entity;
			foreach (ObjectStateEntry objectStateEntry in this.context.ObjectStateManager.GetObjectStateEntries(EntityState.Added))
			{
				if (objectStateEntry.Entity is IEntityBase)
				{
					entity = (IEntityBase)objectStateEntry.Entity;
					entity.CreatedByLoginId = Session.GetLoginId;
                    entity.CreatedDate = DateTime.Now;
					if (entity.State == 0)
					{
						entity.State = 1;
					}
				}
			}
			foreach (ObjectStateEntry objectStateEntry1 in this.context.ObjectStateManager.GetObjectStateEntries(EntityState.Modified))
			{
				if (objectStateEntry1.Entity is IEntityBase)
				{
					entity = (IEntityBase)objectStateEntry1.Entity;
					entity.ModifiedByLoginId = Session.GetLoginId;
					entity.ModifiedDate = UtilDateTime.Instance.Now;
				}
			}
		}
	}
}