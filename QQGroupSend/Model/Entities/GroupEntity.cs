using System;
using System.Collections.Generic;
using System.Linq;

namespace Format.WebQQ.Model.Entities
{
    public class GroupEntity:BaseEntity
    {
        public long GroupId { get; set; }
        public long GroupCode { get; set; }
        public string GroupName { get; set; }

        public Dictionary<long, GroupMember> Members { get; set; }

        public string Memo { get; set; }

        public int Class { get; set; }

        public int Level { get; set; }

        public long OwnerUin { get; set; }

        public DateTime CreateTime { get; set; }

        public int Option { get; set; }
    }
}
