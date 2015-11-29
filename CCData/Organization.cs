﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace CCData
{
    public partial class Organization : IValidatableEntity, ICreatableEntity, IModifiableEntity
    {        
        public class Ids
        {
            public const long KGB = 1;
        } 

        #region SaveValidate
        public IEnumerable<DbValidationError> SaveValidate()
        {
            var errors = new List<DbValidationError>();

            if (string.IsNullOrWhiteSpace(Name))
                errors.Add(new DbValidationError("Organization Name is required", "Name"));

            if (errors.Count == 0)
            {
                using (var db = new CCEntities())
                {
                    if (db.Organizations.Any(o => o.Name == Name && o.OrganizationId != OrganizationId))
                        errors.Add(new DbValidationError("Name must be unique", "Name"));
                }
            }

            return errors;
        } 
        #endregion

        #region DeleteValidate
        public IEnumerable<DbValidationError> DeleteValidate()
        {
            return new List<DbValidationError> { };
        } 
        #endregion
    }
}
