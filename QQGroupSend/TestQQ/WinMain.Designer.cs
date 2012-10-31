namespace Format.WebQQ.TestQQ
{
    partial class WinMain
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tabGroup = new System.Windows.Forms.TabPage();
            this.lbGroups = new System.Windows.Forms.ListBox();
            this.tabFriend = new System.Windows.Forms.TabPage();
            this.tabConfig = new System.Windows.Forms.TabPage();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.minResponseCount = new System.Windows.Forms.DomainUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.txtBlackList = new System.Windows.Forms.TextBox();
            this.txtAllowedUrl = new System.Windows.Forms.TextBox();
            this.txtBadWord = new System.Windows.Forms.TextBox();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.allowVote = new System.Windows.Forms.CheckBox();
            this.removeUrlSender = new System.Windows.Forms.CheckBox();
            this.removeKeyWordSender = new System.Windows.Forms.CheckBox();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.state = new System.Windows.Forms.ToolStripStatusLabel();
            this.tabAddFriend = new System.Windows.Forms.TabPage();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.btnLoad = new System.Windows.Forms.Button();
            this.btnRequestToAdd = new System.Windows.Forms.Button();
            this.tabControl.SuspendLayout();
            this.tabGroup.SuspendLayout();
            this.tabConfig.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.tabAddFriend.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.tabGroup);
            this.tabControl.Controls.Add(this.tabFriend);
            this.tabControl.Controls.Add(this.tabConfig);
            this.tabControl.Controls.Add(this.tabAddFriend);
            this.tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl.Location = new System.Drawing.Point(0, 0);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(284, 450);
            this.tabControl.TabIndex = 15;
            this.tabControl.SelectedIndexChanged += new System.EventHandler(this.tabControl_SelectedIndexChanged);
            // 
            // tabGroup
            // 
            this.tabGroup.Controls.Add(this.lbGroups);
            this.tabGroup.Location = new System.Drawing.Point(4, 22);
            this.tabGroup.Name = "tabGroup";
            this.tabGroup.Padding = new System.Windows.Forms.Padding(3);
            this.tabGroup.Size = new System.Drawing.Size(276, 424);
            this.tabGroup.TabIndex = 0;
            this.tabGroup.Text = "群";
            this.tabGroup.UseVisualStyleBackColor = true;
            // 
            // lbGroups
            // 
            this.lbGroups.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbGroups.FormattingEnabled = true;
            this.lbGroups.ItemHeight = 12;
            this.lbGroups.Location = new System.Drawing.Point(3, 3);
            this.lbGroups.Name = "lbGroups";
            this.lbGroups.Size = new System.Drawing.Size(270, 418);
            this.lbGroups.TabIndex = 1;
            this.lbGroups.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.lbGroups_MouseDoubleClick);
            // 
            // tabFriend
            // 
            this.tabFriend.Location = new System.Drawing.Point(4, 22);
            this.tabFriend.Name = "tabFriend";
            this.tabFriend.Padding = new System.Windows.Forms.Padding(3);
            this.tabFriend.Size = new System.Drawing.Size(276, 424);
            this.tabFriend.TabIndex = 1;
            this.tabFriend.Text = "好友";
            this.tabFriend.UseVisualStyleBackColor = true;
            // 
            // tabConfig
            // 
            this.tabConfig.Controls.Add(this.flowLayoutPanel1);
            this.tabConfig.Location = new System.Drawing.Point(4, 22);
            this.tabConfig.Name = "tabConfig";
            this.tabConfig.Padding = new System.Windows.Forms.Padding(3);
            this.tabConfig.Size = new System.Drawing.Size(276, 424);
            this.tabConfig.TabIndex = 2;
            this.tabConfig.Text = "配置";
            this.tabConfig.UseVisualStyleBackColor = true;
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Controls.Add(this.groupBox1);
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(3, 3);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(270, 418);
            this.flowLayoutPanel1.TabIndex = 0;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.minResponseCount);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.txtBlackList);
            this.groupBox1.Controls.Add(this.txtAllowedUrl);
            this.groupBox1.Controls.Add(this.txtBadWord);
            this.groupBox1.Controls.Add(this.checkBox1);
            this.groupBox1.Controls.Add(this.allowVote);
            this.groupBox1.Controls.Add(this.removeUrlSender);
            this.groupBox1.Controls.Add(this.removeKeyWordSender);
            this.groupBox1.Location = new System.Drawing.Point(3, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(262, 345);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "删除群成员规则";
            // 
            // minResponseCount
            // 
            this.minResponseCount.Items.Add("1");
            this.minResponseCount.Items.Add("2");
            this.minResponseCount.Items.Add("3");
            this.minResponseCount.Items.Add("4");
            this.minResponseCount.Items.Add("5");
            this.minResponseCount.Items.Add("6");
            this.minResponseCount.Items.Add("7");
            this.minResponseCount.Items.Add("8");
            this.minResponseCount.Items.Add("9");
            this.minResponseCount.Items.Add("10");
            this.minResponseCount.Items.Add("11");
            this.minResponseCount.Items.Add("12");
            this.minResponseCount.Items.Add("13");
            this.minResponseCount.Items.Add("14");
            this.minResponseCount.Items.Add("15");
            this.minResponseCount.Items.Add("16");
            this.minResponseCount.Items.Add("17");
            this.minResponseCount.Items.Add("18");
            this.minResponseCount.Items.Add("19");
            this.minResponseCount.Items.Add("20");
            this.minResponseCount.Items.Add("30");
            this.minResponseCount.Items.Add("50");
            this.minResponseCount.Items.Add("100");
            this.minResponseCount.Location = new System.Drawing.Point(108, 207);
            this.minResponseCount.Name = "minResponseCount";
            this.minResponseCount.Size = new System.Drawing.Size(39, 21);
            this.minResponseCount.TabIndex = 4;
            this.minResponseCount.Text = "5";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(22, 209);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(77, 12);
            this.label1.TabIndex = 3;
            this.label1.Text = "最少响应人数";
            // 
            // txtBlackList
            // 
            this.txtBlackList.Location = new System.Drawing.Point(22, 258);
            this.txtBlackList.Multiline = true;
            this.txtBlackList.Name = "txtBlackList";
            this.txtBlackList.Size = new System.Drawing.Size(234, 53);
            this.txtBlackList.TabIndex = 1;
            // 
            // txtAllowedUrl
            // 
            this.txtAllowedUrl.Location = new System.Drawing.Point(22, 123);
            this.txtAllowedUrl.Multiline = true;
            this.txtAllowedUrl.Name = "txtAllowedUrl";
            this.txtAllowedUrl.Size = new System.Drawing.Size(234, 53);
            this.txtAllowedUrl.TabIndex = 1;
            // 
            // txtBadWord
            // 
            this.txtBadWord.Location = new System.Drawing.Point(22, 42);
            this.txtBadWord.Multiline = true;
            this.txtBadWord.Name = "txtBadWord";
            this.txtBadWord.Size = new System.Drawing.Size(234, 53);
            this.txtBadWord.TabIndex = 1;
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(6, 236);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(72, 16);
            this.checkBox1.TabIndex = 0;
            this.checkBox1.Text = "QQ号码为";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // allowVote
            // 
            this.allowVote.AutoSize = true;
            this.allowVote.Location = new System.Drawing.Point(6, 182);
            this.allowVote.Name = "allowVote";
            this.allowVote.Size = new System.Drawing.Size(156, 16);
            this.allowVote.TabIndex = 0;
            this.allowVote.Text = "允许投票选择删除群成员";
            this.allowVote.UseVisualStyleBackColor = true;
            // 
            // removeUrlSender
            // 
            this.removeUrlSender.AutoSize = true;
            this.removeUrlSender.Location = new System.Drawing.Point(6, 101);
            this.removeUrlSender.Name = "removeUrlSender";
            this.removeUrlSender.Size = new System.Drawing.Size(144, 16);
            this.removeUrlSender.TabIndex = 0;
            this.removeUrlSender.Text = "包含非以下网站的链接";
            this.removeUrlSender.UseVisualStyleBackColor = true;
            // 
            // removeKeyWordSender
            // 
            this.removeKeyWordSender.AutoSize = true;
            this.removeKeyWordSender.Location = new System.Drawing.Point(6, 20);
            this.removeKeyWordSender.Name = "removeKeyWordSender";
            this.removeKeyWordSender.Size = new System.Drawing.Size(96, 16);
            this.removeKeyWordSender.TabIndex = 0;
            this.removeKeyWordSender.Text = "发言内容包含";
            this.removeKeyWordSender.UseVisualStyleBackColor = true;
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.state});
            this.statusStrip1.Location = new System.Drawing.Point(0, 428);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(284, 22);
            this.statusStrip1.TabIndex = 16;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // state
            // 
            this.state.Name = "state";
            this.state.Size = new System.Drawing.Size(0, 17);
            // 
            // tabAddFriend
            // 
            this.tabAddFriend.Controls.Add(this.btnRequestToAdd);
            this.tabAddFriend.Controls.Add(this.btnLoad);
            this.tabAddFriend.Controls.Add(this.dataGridView1);
            this.tabAddFriend.Location = new System.Drawing.Point(4, 22);
            this.tabAddFriend.Name = "tabAddFriend";
            this.tabAddFriend.Padding = new System.Windows.Forms.Padding(3);
            this.tabAddFriend.Size = new System.Drawing.Size(276, 424);
            this.tabAddFriend.TabIndex = 3;
            this.tabAddFriend.Text = "好友添加";
            this.tabAddFriend.UseVisualStyleBackColor = true;
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.dataGridView1.Location = new System.Drawing.Point(3, 29);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowTemplate.Height = 23;
            this.dataGridView1.Size = new System.Drawing.Size(270, 392);
            this.dataGridView1.TabIndex = 0;
            // 
            // btnLoad
            // 
            this.btnLoad.Location = new System.Drawing.Point(8, 6);
            this.btnLoad.Name = "btnLoad";
            this.btnLoad.Size = new System.Drawing.Size(57, 25);
            this.btnLoad.TabIndex = 1;
            this.btnLoad.Text = "Load ";
            this.btnLoad.UseVisualStyleBackColor = true;
            this.btnLoad.Click += new System.EventHandler(this.btnLoad_Click);
            // 
            // btnRequestToAdd
            // 
            this.btnRequestToAdd.Location = new System.Drawing.Point(71, 6);
            this.btnRequestToAdd.Name = "btnRequestToAdd";
            this.btnRequestToAdd.Size = new System.Drawing.Size(57, 25);
            this.btnRequestToAdd.TabIndex = 1;
            this.btnRequestToAdd.Text = "Add";
            this.btnRequestToAdd.UseVisualStyleBackColor = true;
            this.btnRequestToAdd.Click += new System.EventHandler(this.btnRequestToAdd_Click);
            // 
            // WinMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 450);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.tabControl);
            this.Name = "WinMain";
            this.Text = "WinMain";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.WinMain_FormClosed);
            this.Shown += new System.EventHandler(this.WinMain_Shown);
            this.tabControl.ResumeLayout(false);
            this.tabGroup.ResumeLayout(false);
            this.tabConfig.ResumeLayout(false);
            this.flowLayoutPanel1.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.tabAddFriend.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage tabGroup;
        private System.Windows.Forms.ListBox lbGroups;
        private System.Windows.Forms.TabPage tabFriend;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel state;
        private System.Windows.Forms.TabPage tabConfig;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox removeKeyWordSender;
        private System.Windows.Forms.TextBox txtBadWord;
        private System.Windows.Forms.TextBox txtAllowedUrl;
        private System.Windows.Forms.CheckBox removeUrlSender;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox allowVote;
        private System.Windows.Forms.DomainUpDown minResponseCount;
        private System.Windows.Forms.TextBox txtBlackList;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.TabPage tabAddFriend;
        private System.Windows.Forms.Button btnRequestToAdd;
        private System.Windows.Forms.Button btnLoad;
        private System.Windows.Forms.DataGridView dataGridView1;

    }
}