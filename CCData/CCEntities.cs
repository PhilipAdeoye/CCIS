using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Validation;
using System.Data.Entity.Infrastructure;


namespace CCData
{
    public partial class CCEntities
    {
        #region SaveChanges
        public override int SaveChanges(System.Data.Objects.SaveOptions options)
        {
            DetectChanges();
            foreach (var entry in ObjectStateManager.GetObjectStateEntries(System.Data.EntityState.Added | System.Data.EntityState.Modified))
            {
                if (!entry.IsRelationship)
                {
                    UpdateTrackedEntity(entry);
                }
            }

            return base.SaveChanges(options);
        } 
        #endregion

        #region UpdateTrackedEntity
        private void UpdateTrackedEntity(System.Data.Objects.ObjectStateEntry entry)
        {
            if (entry.State == System.Data.EntityState.Added)
            {
                var entity = entry.Entity as ICreatableEntity;
                if (entity != null)
                {
                    entity.CreatedOn = DateTime.UtcNow;
                }
            }
            else if (entry.State == System.Data.EntityState.Modified)
            {
                var entity = entry.Entity as IModifiableEntity;
                if (entity != null)
                {
                    entity.ModifiedOn = DateTime.UtcNow;
                }
            }
        } 
        #endregion
    }
}
