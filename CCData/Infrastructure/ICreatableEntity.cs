using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CCData.Infrastructure
{
    public interface ICreatableEntity
    {
        DateTime? CreatedOn { get; set; }
    }
}
