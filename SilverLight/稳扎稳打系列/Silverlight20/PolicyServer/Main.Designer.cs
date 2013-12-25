namespace PolicyServer
{
    partial class Main
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
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.btnStartup = new System.Windows.Forms.Button();
            this.btnPause = new System.Windows.Forms.Button();
            this.txtMsg = new System.Windows.Forms.TextBox();
            this.lblStatus = new System.Windows.Forms.Label();
            this.lblList = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnStartup
            // 
            this.btnStartup.Location = new System.Drawing.Point(12, 12);
            this.btnStartup.Name = "btnStartup";
            this.btnStartup.Size = new System.Drawing.Size(106, 23);
            this.btnStartup.TabIndex = 0;
            this.btnStartup.Text = "启动Policy服务";
            this.btnStartup.UseVisualStyleBackColor = true;
            this.btnStartup.Click += new System.EventHandler(this.btnStartup_Click);
            // 
            // btnPause
            // 
            this.btnPause.Location = new System.Drawing.Point(124, 12);
            this.btnPause.Name = "btnPause";
            this.btnPause.Size = new System.Drawing.Size(106, 23);
            this.btnPause.TabIndex = 1;
            this.btnPause.Text = "暂停Policy服务";
            this.btnPause.UseVisualStyleBackColor = true;
            this.btnPause.Click += new System.EventHandler(this.btnPause_Click);
            // 
            // txtMsg
            // 
            this.txtMsg.Location = new System.Drawing.Point(12, 66);
            this.txtMsg.Multiline = true;
            this.txtMsg.Name = "txtMsg";
            this.txtMsg.Size = new System.Drawing.Size(360, 263);
            this.txtMsg.TabIndex = 2;
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.Location = new System.Drawing.Point(236, 17);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(29, 12);
            this.lblStatus.TabIndex = 3;
            this.lblStatus.Text = "状态";
            // 
            // lblList
            // 
            this.lblList.AutoSize = true;
            this.lblList.Location = new System.Drawing.Point(12, 51);
            this.lblList.Name = "lblList";
            this.lblList.Size = new System.Drawing.Size(233, 12);
            this.lblList.TabIndex = 4;
            this.lblList.Text = "接收到策略文件的客户端IP及其端口列表：";
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(384, 341);
            this.Controls.Add(this.lblList);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.txtMsg);
            this.Controls.Add(this.btnPause);
            this.Controls.Add(this.btnStartup);
            this.Name = "Main";
            this.Text = "PolicyServer";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnStartup;
        private System.Windows.Forms.Button btnPause;
        private System.Windows.Forms.TextBox txtMsg;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.Label lblList;
    }
}

