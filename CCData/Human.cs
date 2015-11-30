using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CCData.Infrastructure;
using System.Net.Mail;

namespace CCData
{
    public partial class Human: IValidatableEntity, ICreatableEntity, IModifiableEntity
    {
        #region SaveValidate
        public IEnumerable<DbValidationError> SaveValidate()
        {
            var errors = new List<DbValidationError>();

            if (string.IsNullOrWhiteSpace(Firstname))
                errors.Add(new DbValidationError("First Name is required", "Firstname"));

            if (string.IsNullOrWhiteSpace(Lastname))
                errors.Add(new DbValidationError("Last Name is required", "Lastname"));

            if (string.IsNullOrWhiteSpace(Username))
                errors.Add(new DbValidationError("Username is required", "Username"));

            if (string.IsNullOrWhiteSpace(Password))
                errors.Add(new DbValidationError("Password is required", "Password"));

            if (string.IsNullOrWhiteSpace(Email))
                errors.Add(new DbValidationError("Email Address is required", "Email"));
            else
            {
                try { new MailAddress(Email); }
                catch (FormatException)
                { errors.Add(new DbValidationError("Invalid Email Address", "Email")); }
            }

            if (errors.Count == 0)
            {
                using (var db = new CCEntities())
                {
                    if(db.Humen.Any(h => h.Username.Equals(Username, StringComparison.InvariantCultureIgnoreCase) 
                            && h.HumanId != HumanId))
                        errors.Add(new DbValidationError("Username must be unique", "Username"));

                    if (!db.Roles.Any(r => r.RoleId == RoleId))
                        errors.Add(new DbValidationError("Invalid Role", "RoleId"));                    
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
