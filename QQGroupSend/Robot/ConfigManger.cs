using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using System.IO;

namespace Format.WebQQ.Robot
{
    public class ConfigManager
    {
        private static string configPath = "config.xml";

        private List<ConfigEntity> configEntities = new List<ConfigEntity>();

        private static ConfigManager instance = null;
        public static ConfigManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ConfigManager();
                    instance.Load();
                }

                if (instance.configEntities.Count == 0)
                    instance.configEntities.Add(new ConfigEntity());
                return instance ?? new ConfigManager();
            }
        }

        private ConfigManager()
        {
        }

        public void Load()
        {
            if (File.Exists(configPath))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(List<ConfigEntity>));
                using (FileStream fs = File.OpenRead(configPath))
                {
                    configEntities = serializer.Deserialize(fs) as List<ConfigEntity>;
                }
            }
        }

        public void Save()
        {
            File.Delete(configPath);
            XmlSerializer serializer = new XmlSerializer(typeof(List<ConfigEntity>));
            using (FileStream fs = File.OpenWrite(configPath))
            {
                serializer.Serialize(fs, configEntities);
            }
        }

        public List<string> GetBadWords()
        {
            return configEntities[0].BadWords;
        }

        public List<long> GetBlackList()
        {
            return configEntities[0].BlackList;
        }

        public void SaveBadWords(string badWordString)
        {
            ConfigEntity configEntity = configEntities[0];
            configEntity.BadWords.Clear();
            foreach (var word in badWordString.Split('\n'))
            {
                if (!string.IsNullOrWhiteSpace(word))
                {
                    configEntity.BadWords.Add(word);
                }
            }
            Save();
        }

        public void SaveBlackList(string badWordString)
        {
            ConfigEntity configEntity = configEntities[0];
            configEntity.BlackList.Clear();
            foreach (var qq in badWordString.Split('\n'))
            {
                long uin = 0;
                if (long.TryParse(qq, out uin))
                {
                    configEntity.BlackList.Add(uin);
                }
            }
            Save();
        }
    }

    public class ConfigEntity
    {
        public ConfigEntity()
        {
            BlackList = new List<long>();
            BadWords = new List<string>();
            AllowedLinks = new List<string>();
        }

        public long GroupTrueUin { get; set; }

        public bool EnableBlackList { get; set; }
        public bool EnableBadWords { get; set; }
        public bool DisableLinks { get; set; }
        public bool AllowVoteToKick { get; set; }

        public List<long> BlackList { get; set; }
        public List<string> BadWords { get; set; }
        public List<string> AllowedLinks { get; set; }
        public int MinVoteMemberCount { get; set; }
    }
}
