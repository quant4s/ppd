namespace PpdCrawler
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
            this.btnStart = new System.Windows.Forms.Button();
            this.btnLoan = new System.Windows.Forms.Button();
            this.btnCrawl = new System.Windows.Forms.Button();
            this.txtLogInfo = new System.Windows.Forms.TextBox();
            this.btnStoreToDb = new System.Windows.Forms.Button();
            this.btnStatistics = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnStart
            // 
            this.btnStart.Location = new System.Drawing.Point(12, 12);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(146, 34);
            this.btnStart.TabIndex = 0;
            this.btnStart.Text = "爬全站数据";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // btnLoan
            // 
            this.btnLoan.Location = new System.Drawing.Point(12, 180);
            this.btnLoan.Name = "btnLoan";
            this.btnLoan.Size = new System.Drawing.Size(146, 35);
            this.btnLoan.TabIndex = 1;
            this.btnLoan.Text = "投资";
            this.btnLoan.UseVisualStyleBackColor = true;
            this.btnLoan.Click += new System.EventHandler(this.btnLoan_Click);
            // 
            // btnCrawl
            // 
            this.btnCrawl.Location = new System.Drawing.Point(12, 63);
            this.btnCrawl.Name = "btnCrawl";
            this.btnCrawl.Size = new System.Drawing.Size(146, 35);
            this.btnCrawl.TabIndex = 2;
            this.btnCrawl.Text = "爬即时数据";
            this.btnCrawl.UseVisualStyleBackColor = true;
            // 
            // txtLogInfo
            // 
            this.txtLogInfo.Location = new System.Drawing.Point(309, 23);
            this.txtLogInfo.Multiline = true;
            this.txtLogInfo.Name = "txtLogInfo";
            this.txtLogInfo.Size = new System.Drawing.Size(274, 274);
            this.txtLogInfo.TabIndex = 3;
            // 
            // btnStoreToDb
            // 
            this.btnStoreToDb.Location = new System.Drawing.Point(12, 121);
            this.btnStoreToDb.Name = "btnStoreToDb";
            this.btnStoreToDb.Size = new System.Drawing.Size(146, 36);
            this.btnStoreToDb.TabIndex = 4;
            this.btnStoreToDb.Text = "Store";
            this.btnStoreToDb.UseVisualStyleBackColor = true;
            this.btnStoreToDb.Click += new System.EventHandler(this.btnStoreToDb_Click);
            // 
            // btnStatistics
            // 
            this.btnStatistics.Location = new System.Drawing.Point(15, 244);
            this.btnStatistics.Name = "btnStatistics";
            this.btnStatistics.Size = new System.Drawing.Size(142, 36);
            this.btnStatistics.TabIndex = 5;
            this.btnStatistics.Text = "统计";
            this.btnStatistics.UseVisualStyleBackColor = true;
            this.btnStatistics.Click += new System.EventHandler(this.btnStatistics_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(600, 308);
            this.Controls.Add(this.btnStatistics);
            this.Controls.Add(this.btnStoreToDb);
            this.Controls.Add(this.txtLogInfo);
            this.Controls.Add(this.btnCrawl);
            this.Controls.Add(this.btnLoan);
            this.Controls.Add(this.btnStart);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.Button btnLoan;
        private System.Windows.Forms.Button btnCrawl;
        private System.Windows.Forms.TextBox txtLogInfo;
        private System.Windows.Forms.Button btnStoreToDb;
        private System.Windows.Forms.Button btnStatistics;
    }
}

