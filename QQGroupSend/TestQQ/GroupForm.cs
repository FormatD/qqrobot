using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Format.WebQQ.Contract;
using Format.WebQQ.WebQQ2.DLL;
using Format.WebQQ.Model.Entities;

namespace Format.WebQQ.TestQQ
{
    public partial class GroupForm : Form
    {
        private readonly IWebQQ webqq = null;
        private readonly IQQDbContext db = new QQDbContext();
        private readonly GroupEntity groupEntity;
        private readonly User me = null;

        private GroupForm()
        {
            InitializeComponent();
        }
        public GroupForm(IWebQQ webqq, IQQDbContext context, GroupEntity groupEntity)
            : this()
        {
            this.webqq = webqq;
            this.me = webqq.CurrentUser;
            this.db = context;
            this.groupEntity = groupEntity;

            InitGroupUI();
        }

        private void InitGroupUI()
        {
            this.Text = string.Format("{0}{1}", groupEntity.GroupName, groupEntity.GroupId);
            this.txtAnnance.Text = groupEntity.Memo;
            foreach (var item in groupEntity.Members.Values)
            {
                lvMember.Items.Add(item.Uin.ToString(), item.DisplayName, 0);
            }
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            btnSend.Enabled = false;
            string message = rtbChat.Text;
            if (webqq.SendGroupMessage(groupEntity.GroupId, message, ""))
                PrintHistoryLine(me.BuddyInfo.NickName, me.BuddyInfo.Uin, DateTime.Now, message);
            rtbChat.Text = string.Empty;
            btnSend.Enabled = true;
        }

        private void PrintHistoryLine(string nickName, long trueUin, DateTime sendTime, string message)
        {
            txtHistory.Text += string.Format("{0} ({1}) {2}\r\n\t{3}\r\n",
                                              nickName,
                                              trueUin,
                                              sendTime,
                                              message
                                              );
        }
    }
}
