using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace CCData
{
    [MetadataType(typeof(Organization.MetaData))]
    public partial class Organization: IValidatableObject, ICreatableEntity, IModifiableEntity
    {
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            using (var db = new CCEntities())
            {
                if (db.Organizations.Any(o => o.Name == Name && o.OrganizationId != OrganizationId))
                    yield return new ValidationResult("Name must be unique", new[] { "Name" });
            }
        }

        #region MetaData
        private sealed class MetaData
        {
            [Required(ErrorMessage = "Organization Name is required.")]
            public string Name { get; set; }
        }
        #endregion
    }
}
