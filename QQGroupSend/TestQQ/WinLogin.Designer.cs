namespace Format.WebQQ.TestQQ
{
    partial class WinLogin
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
            this.btnLogin = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.lblUin = new System.Windows.Forms.Label();
            this.lblPassWord = new System.Windows.Forms.Label();
            this.lblVerifyCode = new System.Windows.Forms.Label();
            this.txtUin = new System.Windows.Forms.TextBox();
            this.txtPassWord = new System.Windows.Forms.TextBox();
            this.txtVerifyCode = new System.Windows.Forms.TextBox();
            this.pbVerifyCode = new System.Windows.Forms.PictureBox();
            this.lbGetNew = new System.Windows.Forms.LinkLabel();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbVerifyCode)).BeginInit();
            this.SuspendLayout();
            // 
            // btnLogin
            // 
            this.btnLogin.Location = new System.Drawing.Point(332, 142);
            this.btnLogin.Name = "btnLogin";
            this.btnLogin.Size = new System.Drawing.Size(75, 23);
            this.btnLogin.TabIndex = 0;
            this.btnLogin.Text = "Login";
            this.btnLogin.UseVisualStyleBackColor = true;
            this.btnLogin.Click += new System.EventHandler(this.btnLogin_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(236, 142);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 0;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 4;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 44.02516F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 55.97484F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 187F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 32F));
            this.tableLayoutPanel1.Controls.Add(this.lblUin, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.lblPassWord, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.lblVerifyCode, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.txtUin, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.txtPassWord, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.txtVerifyCode, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.pbVerifyCode, 2, 2);
            this.tableLayoutPanel1.Controls.Add(this.lbGetNew, 3, 2);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(30, 12);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 49F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(377, 106);
            this.tableLayoutPanel1.TabIndex = 1;
            // 
            // lblUin
            // 
            this.lblUin.AutoSize = true;
            this.lblUin.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblUin.Location = new System.Drawing.Point(3, 0);
            this.lblUin.Name = "lblUin";
            this.lblUin.Size = new System.Drawing.Size(63, 28);
            this.lblUin.TabIndex = 0;
            this.lblUin.Text = "QQ号";
            this.lblUin.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblPassWord
            // 
            this.lblPassWord.AutoSize = true;
            this.lblPassWord.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblPassWord.Location = new System.Drawing.Point(3, 28);
            this.lblPassWord.Name = "lblPassWord";
            this.lblPassWord.Size = new System.Drawing.Size(63, 28);
            this.lblPassWord.TabIndex = 1;
            this.lblPassWord.Text = "密码";
            this.lblPassWord.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblVerifyCode
            // 
            this.lblVerifyCode.AutoSize = true;
            this.lblVerifyCode.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblVerifyCode.Location = new System.Drawing.Point(3, 56);
            this.lblVerifyCode.Name = "lblVerifyCode";
            this.lblVerifyCode.Size = new System.Drawing.Size(63, 50);
            this.lblVerifyCode.TabIndex = 2;
            this.lblVerifyCode.Text = "验证码";
            this.lblVerifyCode.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txtUin
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.txtUin, 3);
            this.txtUin.Location = new System.Drawing.Point(72, 3);
            this.txtUin.Name = "txtUin";
            this.txtUin.Size = new System.Drawing.Size(266, 21);
            this.txtUin.TabIndex = 3;
            this.txtUin.Text = "1677010151";
            this.txtUin.Leave += new System.EventHandler(this.txtUin_Leave);
            // 
            // txtPassWord
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.txtPassWord, 3);
            this.txtPassWord.Location = new System.Drawing.Point(72, 31);
            this.txtPassWord.Name = "txtPassWord";
            this.txtPassWord.PasswordChar = '*';
            this.txtPassWord.Size = new System.Drawing.Size(266, 21);
            this.txtPassWord.TabIndex = 3;
            this.txtPassWord.Text = "goscan@livecn";
            // 
            // txtVerifyCode
            // 
            this.txtVerifyCode.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtVerifyCode.Location = new System.Drawing.Point(72, 59);
            this.txtVerifyCode.Name = "txtVerifyCode";
            this.txtVerifyCode.Size = new System.Drawing.Size(82, 21);
            this.txtVerifyCode.TabIndex = 3;
            // 
            // pbVerifyCode
            // 
            this.pbVerifyCode.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pbVerifyCode.Location = new System.Drawing.Point(160, 59);
            this.pbVerifyCode.Name = "pbVerifyCode";
            this.pbVerifyCode.Size = new System.Drawing.Size(181, 44);
            this.pbVerifyCode.TabIndex = 4;
            this.pbVerifyCode.TabStop = false;
            // 
            // lbGetNew
            // 
            this.lbGetNew.AutoSize = true;
            this.lbGetNew.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbGetNew.Location = new System.Drawing.Point(347, 56);
            this.lbGetNew.Name = "lbGetNew";
            this.lbGetNew.Size = new System.Drawing.Size(27, 50);
            this.lbGetNew.TabIndex = 5;
            this.lbGetNew.TabStop = true;
            this.lbGetNew.Text = "换一个";
            this.lbGetNew.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // WinLogin
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(430, 177);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnLogin);
            this.Name = "WinLogin";
            this.Text = "WebQQ";
            this.Shown += new System.EventHandler(this.WinLogin_Shown);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbVerifyCode)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnLogin;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label lblUin;
        private System.Windows.Forms.Label lblPassWord;
        private System.Windows.Forms.Label lblVerifyCode;
        private System.Windows.Forms.TextBox txtUin;
        private System.Windows.Forms.TextBox txtPassWord;
        private System.Windows.Forms.TextBox txtVerifyCode;
        private System.Windows.Forms.PictureBox pbVerifyCode;
        private System.Windows.Forms.LinkLabel lbGetNew;
    }
}