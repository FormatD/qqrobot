using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Format.WebQQ.WebQQ2.DLL;
using System.IO;
using System.Threading;
using Format.WebQQ.Model.Entities;
using Format.WebQQ.Model.Enum;

namespace Format.WebQQ.TestQQ
{
    public partial class WinLogin : Form
    {
        private WebQQCommon communication = null;
        private bool needVerifyCode = true;

        public WinLogin()
        {
            InitializeComponent();
        }

        private UserStatus Login()
        {
            if (communication.Login() == UserStatus.Logined && communication.InitWebQQ())
            {
                ThreadPool.QueueUserWorkItem((_) => communication.StartPullThread());
            }

            return communication.CurrentUser.Status;
        }

        private void SetVerifyCodeVisible(bool showVerifyCode)
        {
            needVerifyCode = showVerifyCode;
            lblVerifyCode.Visible = showVerifyCode;
            pbVerifyCode.Visible = showVerifyCode;
            txtVerifyCode.Visible = showVerifyCode;
            lbGetNew.Visible = showVerifyCode;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            if (needVerifyCode)
            {
                communication.loginInfo.VerifyCode = txtVerifyCode.Text.Trim();
            }

            UserStatus status = Login();
            if (status == UserStatus.Logined)
            {
                this.Hide();
                new WinMain(communication).ShowDialog();
            }
            else
            {
                MessageBox.Show(status.ToString());
            }
        }

        private void txtUin_Leave(object sender, EventArgs e)
        {
            CheckVerifyCode();
        }

        private void CheckVerifyCode()
        {
            string userName = txtUin.Text.Trim();
            string passWord = txtPassWord.Text.Trim();

            if (string.IsNullOrWhiteSpace(userName)
                || string.IsNullOrWhiteSpace(passWord))
            {
                return;
            }

            LoginInfo loginInfo = new LoginInfo { Username = userName, PassWord = passWord };
            communication = new WebQQCommon(loginInfo);
            if (communication.IsNeedVerifyCode())
            {
                Stream stream = communication.GetVerifyCodePicStream();
                if (stream != null)
                {
                    Image image = Image.FromStream(stream);
                    this.pbVerifyCode.Image = image;
                    //image.Save(@"C:\123.bmp");

                    SetVerifyCodeVisible(true);
                }
                else
                {

                }
            }
            else
            {
                SetVerifyCodeVisible(false);
            }
        }

        private void WinLogin_Shown(object sender, EventArgs e)
        {
            CheckVerifyCode();
            txtUin.Focus();
        }
    }
}
