using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Json;

namespace Format.WebQQ.Model.Entities
{
    public static class EntityBuilder
    {
        public static void ParseFriendInfo(Friend buddy, JsonValue item)
        {
            //buddy.Status = QQHelper.ParseOnlineStatus(item["stat"]).ToString();
            buddy.NickName = item["nick"].ToString().Trim('\"');
            buddy.Country = item["country"].ToString();
            buddy.PersonalElucidation = item["province"].ToString();
            buddy.City = item["city"].ToString();
            buddy.Sex = item["gender"].ToString();
            buddy.Face = item["face"].ToString();
            var birthday = item["birthday"];
            int year = (int)birthday["year"];
            int month = (int)birthday["month"];
            int day = (int)birthday["day"];
            buddy.Birthday = new DateTime(year, month, day).ToString();
            buddy.Allow = item["allow"].ToString();
            buddy.Blood = item["blood"].ToString();
            buddy.ShengXiao = item["shengxiao"].ToString();
            buddy.Constel = item["constel"].ToString();
            buddy.TelePhone = item["phone"].ToString();
            buddy.MPhone = item["mobile"].ToString();
            buddy.Email = item["email"].ToString();
            buddy.Occupation = item["occupation"].ToString();
            buddy.College = item["college"].ToString();
            buddy.HomeUrl = item["homepage"].ToString();
            buddy.PersonalElucidation = item["personal"].ToString();
        }
    }
}
