namespace ComputerYacht
{
	// Token: 0x0200000C RID: 12
	public partial class frmMain : global::System.Windows.Forms.Form
	{
		// Token: 0x0600005B RID: 91 RVA: 0x00004633 File Offset: 0x00002833
		protected override void Dispose(bool disposing)
		{
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		// Token: 0x0600005C RID: 92 RVA: 0x00004654 File Offset: 0x00002854
		private void InitializeComponent()
		{
			this.components = new global::System.ComponentModel.Container();
			this.tbDices = new global::System.Windows.Forms.TextBox();
			this.tbScores = new global::System.Windows.Forms.TextBox();
			this.tbStats = new global::System.Windows.Forms.TextBox();
			this.tmrMain = new global::System.Windows.Forms.Timer(this.components);
			this.btnCopy = new global::System.Windows.Forms.Button();
			this.btnPause = new global::System.Windows.Forms.Button();
			base.SuspendLayout();
			this.tbDices.Anchor = (global::System.Windows.Forms.AnchorStyles.Top | global::System.Windows.Forms.AnchorStyles.Bottom | global::System.Windows.Forms.AnchorStyles.Left);
			this.tbDices.Font = new global::System.Drawing.Font("Microsoft Sans Serif", 14f, global::System.Drawing.FontStyle.Regular, global::System.Drawing.GraphicsUnit.Point, 0);
			this.tbDices.Location = new global::System.Drawing.Point(12, 12);
			this.tbDices.Multiline = true;
			this.tbDices.Name = "tbDices";
			this.tbDices.Size = new global::System.Drawing.Size(164, 456);
			this.tbDices.TabIndex = 0;
			this.tbScores.Anchor = (global::System.Windows.Forms.AnchorStyles.Top | global::System.Windows.Forms.AnchorStyles.Bottom | global::System.Windows.Forms.AnchorStyles.Left);
			this.tbScores.Location = new global::System.Drawing.Point(182, 12);
			this.tbScores.Multiline = true;
			this.tbScores.Name = "tbScores";
			this.tbScores.Size = new global::System.Drawing.Size(229, 456);
			this.tbScores.TabIndex = 1;
			this.tbStats.Anchor = (global::System.Windows.Forms.AnchorStyles.Top | global::System.Windows.Forms.AnchorStyles.Bottom | global::System.Windows.Forms.AnchorStyles.Left);
			this.tbStats.Location = new global::System.Drawing.Point(417, 12);
			this.tbStats.Multiline = true;
			this.tbStats.Name = "tbStats";
			this.tbStats.Size = new global::System.Drawing.Size(301, 456);
			this.tbStats.TabIndex = 3;
			this.tbStats.MouseMove += new global::System.Windows.Forms.MouseEventHandler(this.tbStats_MouseMove);
			this.tmrMain.Interval = 1;
			this.tmrMain.Tick += new global::System.EventHandler(this.tmrMain_Tick);
			this.btnCopy.Anchor = (global::System.Windows.Forms.AnchorStyles.Bottom | global::System.Windows.Forms.AnchorStyles.Right);
			this.btnCopy.Location = new global::System.Drawing.Point(640, 476);
			this.btnCopy.Name = "btnCopy";
			this.btnCopy.Size = new global::System.Drawing.Size(75, 23);
			this.btnCopy.TabIndex = 4;
			this.btnCopy.Text = "&Copy Text";
			this.btnCopy.UseVisualStyleBackColor = true;
			this.btnCopy.Click += new global::System.EventHandler(this.btnCopy_Click);
			this.btnPause.Anchor = (global::System.Windows.Forms.AnchorStyles.Bottom | global::System.Windows.Forms.AnchorStyles.Left);
			this.btnPause.Location = new global::System.Drawing.Point(12, 476);
			this.btnPause.Name = "btnPause";
			this.btnPause.Size = new global::System.Drawing.Size(75, 23);
			this.btnPause.TabIndex = 5;
			this.btnPause.Text = "(Un)Slow";
			this.btnPause.UseVisualStyleBackColor = true;
			this.btnPause.Click += new global::System.EventHandler(this.btnPause_Click);
			base.AutoScaleDimensions = new global::System.Drawing.SizeF(6f, 13f);
			base.AutoScaleMode = global::System.Windows.Forms.AutoScaleMode.Font;
			base.ClientSize = new global::System.Drawing.Size(730, 512);
			base.Controls.Add(this.btnPause);
			base.Controls.Add(this.btnCopy);
			base.Controls.Add(this.tbStats);
			base.Controls.Add(this.tbScores);
			base.Controls.Add(this.tbDices);
			base.Name = "frmMain";
			this.Text = "Form1";
			base.Load += new global::System.EventHandler(this.frmMain_Load);
			base.FormClosed += new global::System.Windows.Forms.FormClosedEventHandler(this.frmMain_FormClosed);
			base.Click += new global::System.EventHandler(this.frmMain_Click);
			base.ResumeLayout(false);
			base.PerformLayout();
		}

		// Token: 0x04000080 RID: 128
		private global::System.ComponentModel.IContainer components;

		// Token: 0x04000081 RID: 129
		private global::System.Windows.Forms.TextBox tbDices;

		// Token: 0x04000082 RID: 130
		private global::System.Windows.Forms.TextBox tbScores;

		// Token: 0x04000083 RID: 131
		private global::System.Windows.Forms.TextBox tbStats;

		// Token: 0x04000084 RID: 132
		private global::System.Windows.Forms.Timer tmrMain;

		// Token: 0x04000085 RID: 133
		private global::System.Windows.Forms.Button btnCopy;

		// Token: 0x04000086 RID: 134
		private global::System.Windows.Forms.Button btnPause;
	}
}
