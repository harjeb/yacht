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
			this.btnCopy = new global::System.Windows.Forms.Button();
			// this.btnPause = new global::System.Windows.Forms.Button(); // Removed for manual step
			this.btnGetHoldSuggestion = new global::System.Windows.Forms.Button();
			this.txtDice1 = new System.Windows.Forms.TextBox();
			this.txtDice2 = new System.Windows.Forms.TextBox();
			this.txtDice3 = new System.Windows.Forms.TextBox();
			this.txtDice4 = new System.Windows.Forms.TextBox();
			this.txtDice5 = new System.Windows.Forms.TextBox();
			this.cmbRollNumber = new System.Windows.Forms.ComboBox();
			this.txtCurrentUpperScore = new System.Windows.Forms.TextBox();
			this.lblRollNumber = new System.Windows.Forms.Label();
			this.lblCurrentUpperScore = new System.Windows.Forms.Label();
			base.SuspendLayout();
			this.tbDices.Anchor = (global::System.Windows.Forms.AnchorStyles.Top | global::System.Windows.Forms.AnchorStyles.Bottom | global::System.Windows.Forms.AnchorStyles.Left);
			this.tbDices.Font = new global::System.Drawing.Font("Microsoft Sans Serif", 14f, global::System.Drawing.FontStyle.Regular, global::System.Drawing.GraphicsUnit.Point, 0);
			this.tbDices.Location = new global::System.Drawing.Point(12, 12);
			this.tbDices.Multiline = true;
			this.tbDices.Name = "tbDices";
			this.tbDices.Size = new global::System.Drawing.Size(164, 410); // Adjusted size to make space for new textboxes
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
			// this.tmrMain.Interval = 1; // Removed for manual step
			// this.tmrMain.Tick += new global::System.EventHandler(this.tmrMain_Tick); // Removed for manual step
			this.btnCopy.Anchor = (global::System.Windows.Forms.AnchorStyles.Bottom | global::System.Windows.Forms.AnchorStyles.Right);
			this.btnCopy.Location = new global::System.Drawing.Point(640, 476);
			this.btnCopy.Name = "btnCopy";
			this.btnCopy.Size = new global::System.Drawing.Size(75, 23);
			this.btnCopy.TabIndex = 4;
			this.btnCopy.Text = "&Copy Text";
			this.btnCopy.UseVisualStyleBackColor = true;
			this.btnCopy.Click += new global::System.EventHandler(this.btnCopy_Click);
			// Removed btnPause for manual step
			// this.btnPause.Anchor = (global::System.Windows.Forms.AnchorStyles.Bottom | global::System.Windows.Forms.AnchorStyles.Left);
			// this.btnPause.Location = new global::System.Drawing.Point(12, 476);
			// this.btnPause.Name = "btnPause";
			// this.btnPause.Size = new global::System.Drawing.Size(75, 23);
			// this.btnPause.TabIndex = 5;
			// this.btnPause.Text = "(Un)Slow";
			// this.btnPause.UseVisualStyleBackColor = true;
			// this.btnPause.Click += new global::System.EventHandler(this.btnPause_Click);
			this.btnGetHoldSuggestion.Anchor = (global::System.Windows.Forms.AnchorStyles.Bottom | global::System.Windows.Forms.AnchorStyles.Left);
			this.btnGetHoldSuggestion.Location = new global::System.Drawing.Point(12, 500); // Adjusted location
			this.btnGetHoldSuggestion.Name = "btnGetHoldSuggestion";
			this.btnGetHoldSuggestion.Size = new global::System.Drawing.Size(150, 23);
			this.btnGetHoldSuggestion.TabIndex = 13; // Adjusted TabIndex
			this.btnGetHoldSuggestion.Text = "获取保留建议";
			this.btnGetHoldSuggestion.UseVisualStyleBackColor = true;
			this.btnGetHoldSuggestion.Click += new global::System.EventHandler(this.btnGetHoldSuggestion_Click);
			// 
			// txtDice1
			// 
			this.txtDice1.Location = new global::System.Drawing.Point(12, 470); // Adjusted location
			this.txtDice1.Name = "txtDice1";
			this.txtDice1.Size = new global::System.Drawing.Size(30, 20);
			this.txtDice1.TabIndex = 6;
			//
			// txtDice2
			//
			this.txtDice2.Location = new global::System.Drawing.Point(48, 470); // Adjusted location
			this.txtDice2.Name = "txtDice2";
			this.txtDice2.Size = new global::System.Drawing.Size(30, 20);
			this.txtDice2.TabIndex = 7;
			//
			// txtDice3
			//
			this.txtDice3.Location = new global::System.Drawing.Point(84, 470); // Adjusted location
			this.txtDice3.Name = "txtDice3";
			this.txtDice3.Size = new global::System.Drawing.Size(30, 20);
			this.txtDice3.TabIndex = 8;
			//
			// txtDice4
			//
			this.txtDice4.Location = new global::System.Drawing.Point(120, 470); // Adjusted location
			this.txtDice4.Name = "txtDice4";
			this.txtDice4.Size = new global::System.Drawing.Size(30, 20);
			this.txtDice4.TabIndex = 9;
			//
			// txtDice5
			//
			this.txtDice5.Location = new global::System.Drawing.Point(156, 470); // Adjusted location
			this.txtDice5.Name = "txtDice5";
			this.txtDice5.Size = new global::System.Drawing.Size(30, 20);
			this.txtDice5.TabIndex = 10;
			//
			// cmbRollNumber
			//
			this.cmbRollNumber.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cmbRollNumber.FormattingEnabled = true;
			this.cmbRollNumber.Items.AddRange(new object[] {
			"1",
			"2",
			"3"});
			this.cmbRollNumber.Location = new global::System.Drawing.Point(280, 470); // Adjusted location
			this.cmbRollNumber.Name = "cmbRollNumber";
			this.cmbRollNumber.Size = new global::System.Drawing.Size(40, 21);
			this.cmbRollNumber.TabIndex = 11;
			//
			// txtCurrentUpperScore
			//
			this.txtCurrentUpperScore.Location = new global::System.Drawing.Point(390, 470); // Adjusted location
			this.txtCurrentUpperScore.Name = "txtCurrentUpperScore";
			this.txtCurrentUpperScore.Size = new global::System.Drawing.Size(40, 20);
			this.txtCurrentUpperScore.TabIndex = 12;
			this.txtCurrentUpperScore.Text = "0";
			//
			// lblRollNumber
			//
			this.lblRollNumber.AutoSize = true;
			this.lblRollNumber.Location = new global::System.Drawing.Point(200, 473); // Adjusted location
			this.lblRollNumber.Name = "lblRollNumber";
			this.lblRollNumber.Size = new global::System.Drawing.Size(74, 13);
			this.lblRollNumber.TabIndex = 14;
			this.lblRollNumber.Text = "当前掷骰次数:";
			//
			// lblCurrentUpperScore
			//
			this.lblCurrentUpperScore.AutoSize = true;
			this.lblCurrentUpperScore.Location = new global::System.Drawing.Point(325, 473); // Adjusted location
			this.lblCurrentUpperScore.Name = "lblCurrentUpperScore";
			this.lblCurrentUpperScore.Size = new global::System.Drawing.Size(59, 13);
			this.lblCurrentUpperScore.TabIndex = 15;
			this.lblCurrentUpperScore.Text = "上区总分:";
			//
			base.AutoScaleDimensions = new global::System.Drawing.SizeF(6f, 13f);
			base.AutoScaleMode = global::System.Windows.Forms.AutoScaleMode.Font;
			base.ClientSize = new global::System.Drawing.Size(730, 535); // Adjusted ClientSize
			base.Controls.Add(this.lblCurrentUpperScore);
			base.Controls.Add(this.lblRollNumber);
			base.Controls.Add(this.txtCurrentUpperScore);
			base.Controls.Add(this.cmbRollNumber);
			base.Controls.Add(this.txtDice5);
			base.Controls.Add(this.txtDice4);
			base.Controls.Add(this.txtDice3);
			base.Controls.Add(this.txtDice2);
			base.Controls.Add(this.txtDice1);
			base.Controls.Add(this.btnGetHoldSuggestion);
			// base.Controls.Add(this.btnPause); // Removed for manual step
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
		// private global::System.Windows.Forms.Timer tmrMain; // Removed for manual step

		// Token: 0x04000085 RID: 133
		private global::System.Windows.Forms.Button btnCopy;

		// Token: 0x04000086 RID: 134
		// private global::System.Windows.Forms.Button btnPause; // Removed for manual step
		private global::System.Windows.Forms.Button btnGetHoldSuggestion;
		private System.Windows.Forms.TextBox txtDice1;
		private System.Windows.Forms.TextBox txtDice2;
		private System.Windows.Forms.TextBox txtDice3;
		private System.Windows.Forms.TextBox txtDice4;
		private System.Windows.Forms.TextBox txtDice5;
		private System.Windows.Forms.ComboBox cmbRollNumber;
		private System.Windows.Forms.TextBox txtCurrentUpperScore;
		private System.Windows.Forms.Label lblRollNumber;
		private System.Windows.Forms.Label lblCurrentUpperScore;
	}
}
