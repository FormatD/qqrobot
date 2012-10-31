using System;
using System.Collections.Generic;
using System.Linq;
using Format.WebQQ.Model.Entities;
using Format.WebQQ.Model.Enum;

namespace Format.WebQQ.Model.Entities
{
    public class User
    {
        public Friend BuddyInfo { get; set; }

        public Dictionary<long, Friend> Friends { get; set; }
        
        public Dictionary<long, GroupEntity> Groups { get; set; }

        public Dictionary<string, Friend> TempSessionUser { get; set; }
        
        public UserStatus Status { get; set; }

        public GroupMember GetGroupmate(long groupuin, long uin)
        {
            if (Groups.ContainsKey(groupuin) &&
                Groups[groupuin].Members.ContainsKey(uin))
            {
                return Groups[groupuin].Members[uin];
            }
            return null;
        }
    }
}
