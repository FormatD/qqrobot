using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Format.WebQQ.Model.Entities
{
    public class GroupMember
    {
        public Guid ID { get; set; }

        public long Uin { get; set; }

        public long GroupId { get; set; }

        public string Card { get; set; }

        public string NickName { get; set; }

        public string DisplayName
        {
            get
            {
                return string.IsNullOrWhiteSpace(Card) ? NickName : Card;
            }
        }

        public string Country { get; set; }

        public string PersonalElucidation { get; set; }

        public string City { get; set; }

        public int State { get; set; }

        public int MFlag { get; set; }
    }
}