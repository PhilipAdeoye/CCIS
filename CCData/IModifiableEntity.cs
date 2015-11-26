using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CCData
{
    public interface IModifiableEntity
    {
        DateTime? ModifiedOn { get; set; }
    }
}
