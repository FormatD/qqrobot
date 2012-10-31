using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Format.WebQQ.WebQQ2.DLL;
using Format.WebQQ.Model.Messages;
using System.Threading;
using System.IO;
using Format.WebQQ.WebQQ2.DLL.Repository;
using Format.WebQQ.Robot.Rule;
using System.Xml;
using Format.WebQQ.Robot;
using Format.WebQQ.Model.Entities;

namespace Format.WebQQ.TestQQ
{
    public partial class WinMain : Form
    {
        private WebQQCommon webqq = null;
        private IQQDbContext db = new QQDbContext();
        private List<BaseRule> rules = new List<BaseRule>();

        private WinMain()
        {
            InitializeComponent();
        }

        public WinMain(WebQQCommon communication)
            : this()
        {
            this.webqq = communication;
            RegisteEvent(communication);
            LoadRules();
        }

        private void LoadRules()
        {
            ConfigManager config = ConfigManager.Instance;
            txtBadWord.Text = string.Join(Environment.NewLine, config.GetBadWords());
            txtBlackList.Text = string.Join(Environment.NewLine, config.GetBlackList());

            rules.Add(new BlackListRule(webqq, config));
            rules.Add(new BadWordsRule(webqq, config));
            rules.Add(new JoinRequestRule(webqq, config));
        }

        private void RegisteEvent(WebQQCommon communication)
        {
            communication.OnAddFriendEvent += communication_OnAddFriendEvent;
            //communication.OnFriendMessageEvent+=communication_OnFriendMessageEvent;
            communication.OnOffLine += communication_OnOffLine;
            communication.OnGroupMessageArrivedEvent += communication_OnQunMessageEvent;
            communication.OnJoinRequested += communication_OnSystemMessageArrive;
        }

        void communication_OnSystemMessageArrive(Format.WebQQ.Model.Messages.JoinGroupRequestMessage message)
        {
            string content = string.Format("user:{0} 请求加入群: {1},并说{2}"
                , message.MemberUin
                , message.GroupUin,
            message.RequestMessage);
            Console.WriteLine(content);
            webqq.SendGroupMessage(message.GroupUin, content, "");
            //webqq.AcceptJoinRequest(message);
            webqq.sendFriendMessage(message.MemberUin, "welcome");
        }

        void communication_OnOffLine(string userName)
        {
            Console.WriteLine("Offline");
        }

        void communication_OnQunMessageEvent(GroupMessage groupMessage)
        {
            Console.WriteLine("Group {5}({4})=>{0}({1}) {2} :\n\t{3}",
                groupMessage.TrueSenderUin,
                webqq.CurrentUser.GetGroupmate(
                    groupMessage.GroupUin,
                    groupMessage.SenderUin
                    ).DisplayName,
                groupMessage.SentTime,
                groupMessage.Message,
                groupMessage.TrueGroupUin,
                groupMessage.GroupEntity.GroupName
                );

            db.GroupMessages.Add(groupMessage);
            db.Submit();
        }

        void communication_OnAddFriendEvent(string fuin, string message)
        {
            Console.WriteLine("User Message from {0}:{1}", fuin, message);
        }

        private void PrintOutGroupData()
        {
            Console.WriteLine("Get Group list...");
            webqq.GetGroupList();
            foreach (var group in webqq.CurrentUser.Groups.Values)
            {
                Console.WriteLine("{0}({1})", group.GroupName, group.GroupId);
                foreach (var groupMember in group.Members.Values)
                {
                    Console.WriteLine("\t{0}", groupMember.DisplayName);
                }
            }
            BindGroups();
        }

        private void BindGroups()
        {
            this.lbGroups.DataSource = webqq.CurrentUser.Groups.Values.ToList();
            this.lbGroups.DisplayMember = "GroupName";
        }

        private void WinMain_Shown(object sender, EventArgs e)
        {
            PrintOutGroupData();
        }

        private void tabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            SaveConfig();
        }

        private void WinMain_FormClosed(object sender, FormClosedEventArgs e)
        {
            webqq.LogOut();
            SaveConfig();
        }

        private void SaveConfig()
        {
            Format.WebQQ.Robot.ConfigManager.Instance.SaveBadWords(txtBadWord.Text);
            Format.WebQQ.Robot.ConfigManager.Instance.SaveBlackList(txtBlackList.Text);
        }

        private void lbGroups_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            GroupEntity groupEntity = lbGroups.SelectedItem as GroupEntity;
            if (groupEntity != null)
            {
                GroupForm gf = new GroupForm(webqq, db, groupEntity);
                gf.Show();
            }
        }

        class AddingUser
        {
            public long Uin { get; set; }
            public string State { get; set; }
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog dlg = new OpenFileDialog())
            {
                if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    var qqnumbers = File.ReadAllLines(dlg.FileName);
                    this.dataGridView1.AutoGenerateColumns = true;
                    this.dataGridView1.DataSource = qqnumbers.Select(
                         (s) =>
                         {
                             long uin;
                             long.TryParse(s, out uin);
                             return uin;
                         }).Where(u => u > 0)
                         .Select(uin => new AddingUser { Uin = uin })
                         .ToList();
                }
            }
        }

        private void btnRequestToAdd_Click(object sender, EventArgs e)
        {
            var list = dataGridView1.DataSource as List<AddingUser>;
            var selectedUsers = dataGridView1.SelectedCells.Cast<DataGridViewCell>()
                .Select(cell => cell.RowIndex)
                .Distinct()
                .Select(index => list[index])
                .ToList();

            foreach (var user in selectedUsers)
            {
                user.State = webqq.AddFriend(user.Uin);
            }

            dataGridView1.DataSource = null;
            dataGridView1.DataSource = list;
        }

    }

    public enum MessageType
    {
        UserMessage,
        GroupMessage
    }
}
