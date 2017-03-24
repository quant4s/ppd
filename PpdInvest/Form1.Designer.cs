namespace PpdInvest
{
    partial class Form1
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.txtLog = new System.Windows.Forms.TextBox();
            this.btnCrawl = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.numDays = new System.Windows.Forms.NumericUpDown();
            this.txtWarning = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.txtUsername = new System.Windows.Forms.TextBox();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.numDays)).BeginInit();
            this.SuspendLayout();
            // 
            // txtLog
            // 
            this.txtLog.Location = new System.Drawing.Point(0, 181);
            this.txtLog.Multiline = true;
            this.txtLog.Name = "txtLog";
            this.txtLog.Size = new System.Drawing.Size(320, 198);
            this.txtLog.TabIndex = 0;
            // 
            // btnCrawl
            // 
            this.btnCrawl.Location = new System.Drawing.Point(14, 90);
            this.btnCrawl.Name = "btnCrawl";
            this.btnCrawl.Size = new System.Drawing.Size(75, 23);
            this.btnCrawl.TabIndex = 1;
            this.btnCrawl.Text = "收集数据";
            this.btnCrawl.UseVisualStyleBackColor = true;
            this.btnCrawl.Click += new System.EventHandler(this.btnCrawl_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(185, 95);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(29, 12);
            this.label1.TabIndex = 4;
            this.label1.Text = "天前";
            // 
            // numDays
            // 
            this.numDays.Location = new System.Drawing.Point(122, 92);
            this.numDays.Maximum = new decimal(new int[] {
            900,
            0,
            0,
            0});
            this.numDays.Name = "numDays";
            this.numDays.Size = new System.Drawing.Size(57, 21);
            this.numDays.TabIndex = 5;
            this.numDays.Value = new decimal(new int[] {
            3,
            0,
            0,
            0});
            // 
            // txtWarning
            // 
            this.txtWarning.Location = new System.Drawing.Point(351, 181);
            this.txtWarning.Multiline = true;
            this.txtWarning.Name = "txtWarning";
            this.txtWarning.Size = new System.Drawing.Size(327, 198);
            this.txtWarning.TabIndex = 6;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(349, 166);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(29, 12);
            this.label2.TabIndex = 7;
            this.label2.Text = "警告";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 166);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(29, 12);
            this.label3.TabIndex = 7;
            this.label3.Text = "信息";
            // 
            // txtUsername
            // 
            this.txtUsername.Location = new System.Drawing.Point(16, 47);
            this.txtUsername.Name = "txtUsername";
            this.txtUsername.Size = new System.Drawing.Size(82, 21);
            this.txtUsername.TabIndex = 8;
            // 
            // txtPassword
            // 
            this.txtPassword.Location = new System.Drawing.Point(117, 47);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.Size = new System.Drawing.Size(77, 21);
            this.txtPassword.TabIndex = 9;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(120, 29);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(29, 12);
            this.label4.TabIndex = 10;
            this.label4.Text = "密码";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(14, 29);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(41, 12);
            this.label5.TabIndex = 10;
            this.label5.Text = "用户名";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(316, 89);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 11;
            this.button1.Text = "风险预警";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click_1);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(680, 379);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txtPassword);
            this.Controls.Add(this.txtUsername);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtWarning);
            this.Controls.Add(this.numDays);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnCrawl);
            this.Controls.Add(this.txtLog);
            this.Name = "Form1";
            this.Text = "拍拍贷投资统计生成工具";
            ((System.ComponentModel.ISupportInitialize)(this.numDays)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtLog;
        private System.Windows.Forms.Button btnCrawl;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown numDays;
        private System.Windows.Forms.TextBox txtWarning;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtUsername;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button button1;
    }
}

