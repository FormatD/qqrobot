using System;
using System.Collections.Generic;
using System.Linq;

namespace Format.WebQQ.Model.Entities
{
    public class BaseEntity
    {
        public Guid ID { get; set; }

        public BaseEntity()
        {
            ID = Guid.NewGuid();
        }
    }
}
