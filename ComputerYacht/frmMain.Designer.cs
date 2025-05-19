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
			this.grpScoringCategories = new System.Windows.Forms.GroupBox();
			this.chkCatChance = new System.Windows.Forms.CheckBox();
			this.chkCatYachtzee = new System.Windows.Forms.CheckBox();
			this.chkCatLgStraight = new System.Windows.Forms.CheckBox();
			this.chkCatSmStraight = new System.Windows.Forms.CheckBox();
			this.chkCatFullHouse = new System.Windows.Forms.CheckBox();
			this.chkCatFourOfAKind = new System.Windows.Forms.CheckBox();
			this.chkCatThreeOfAKind = new System.Windows.Forms.CheckBox();
			this.chkCatSixes = new System.Windows.Forms.CheckBox();
			this.chkCatFives = new System.Windows.Forms.CheckBox();
			this.chkCatFours = new System.Windows.Forms.CheckBox();
			this.chkCatThrees = new System.Windows.Forms.CheckBox();
			this.chkCatTwos = new System.Windows.Forms.CheckBox();
			this.chkCatOnes = new System.Windows.Forms.CheckBox();
			this.grpScoringCategories.SuspendLayout();
			base.SuspendLayout();
			// 
			// tbDices
			// 
			this.tbDices.Anchor = (global::System.Windows.Forms.AnchorStyles.Top | global::System.Windows.Forms.AnchorStyles.Bottom | global::System.Windows.Forms.AnchorStyles.Left);
			this.tbDices.Font = new global::System.Drawing.Font("Microsoft Sans Serif", 14f, global::System.Drawing.FontStyle.Regular, global::System.Drawing.GraphicsUnit.Point, 0);
			this.tbDices.Location = new global::System.Drawing.Point(12, 12);
			this.tbDices.Multiline = true;
			this.tbDices.Name = "tbDices";
			this.tbDices.Size = new global::System.Drawing.Size(164, 380); // Adjusted height
			this.tbDices.TabIndex = 0;
			// 
			// tbScores
			// 
			this.tbScores.Anchor = (global::System.Windows.Forms.AnchorStyles.Top | global::System.Windows.Forms.AnchorStyles.Bottom | global::System.Windows.Forms.AnchorStyles.Left);
			this.tbScores.Location = new global::System.Drawing.Point(182, 12);
			this.tbScores.Multiline = true;
			this.tbScores.Name = "tbScores";
			this.tbScores.Size = new global::System.Drawing.Size(229, 380); // Adjusted height
			this.tbScores.TabIndex = 1;
			// 
			// tbStats
			// 
			this.tbStats.Anchor = (global::System.Windows.Forms.AnchorStyles.Top | global::System.Windows.Forms.AnchorStyles.Bottom | global::System.Windows.Forms.AnchorStyles.Left);
			this.tbStats.Location = new global::System.Drawing.Point(417, 12);
			this.tbStats.Multiline = true;
			this.tbStats.Name = "tbStats";
			this.tbStats.Size = new global::System.Drawing.Size(301, 380); // Adjusted height
			this.tbStats.TabIndex = 3;
			this.tbStats.MouseMove += new global::System.Windows.Forms.MouseEventHandler(this.tbStats_MouseMove);
			// 
			// btnCopy
			// 
			this.btnCopy.Anchor = (global::System.Windows.Forms.AnchorStyles.Bottom | global::System.Windows.Forms.AnchorStyles.Right);
			this.btnCopy.Location = new global::System.Drawing.Point(640, 580); // Adjusted location
			this.btnCopy.Name = "btnCopy";
			this.btnCopy.Size = new global::System.Drawing.Size(75, 23);
			this.btnCopy.TabIndex = 4; // Will be re-ordered later
			this.btnCopy.Text = "&Copy Text";
			this.btnCopy.UseVisualStyleBackColor = true;
			this.btnCopy.Click += new global::System.EventHandler(this.btnCopy_Click);
			// 
			// btnGetHoldSuggestion
			// 
			this.btnGetHoldSuggestion.Anchor = (global::System.Windows.Forms.AnchorStyles.Bottom | global::System.Windows.Forms.AnchorStyles.Left);
			this.btnGetHoldSuggestion.Location = new global::System.Drawing.Point(12, 580); // Adjusted location
			this.btnGetHoldSuggestion.Name = "btnGetHoldSuggestion";
			this.btnGetHoldSuggestion.Size = new global::System.Drawing.Size(150, 23);
			this.btnGetHoldSuggestion.TabIndex = 19; // Adjusted TabIndex
			this.btnGetHoldSuggestion.Text = "获取保留建议";
			this.btnGetHoldSuggestion.UseVisualStyleBackColor = true;
			this.btnGetHoldSuggestion.Click += new global::System.EventHandler(this.btnGetHoldSuggestion_Click);
			// 
			// txtDice1
			// 
			this.txtDice1.Location = new global::System.Drawing.Point(12, 550); // Adjusted location
			this.txtDice1.Name = "txtDice1";
			this.txtDice1.Size = new global::System.Drawing.Size(30, 20);
			this.txtDice1.TabIndex = 14; 
			// 
			// txtDice2
			// 
			this.txtDice2.Location = new global::System.Drawing.Point(48, 550); // Adjusted location
			this.txtDice2.Name = "txtDice2";
			this.txtDice2.Size = new global::System.Drawing.Size(30, 20);
			this.txtDice2.TabIndex = 15;
			// 
			// txtDice3
			// 
			this.txtDice3.Location = new global::System.Drawing.Point(84, 550); // Adjusted location
			this.txtDice3.Name = "txtDice3";
			this.txtDice3.Size = new global::System.Drawing.Size(30, 20);
			this.txtDice3.TabIndex = 16;
			// 
			// txtDice4
			// 
			this.txtDice4.Location = new global::System.Drawing.Point(120, 550); // Adjusted location
			this.txtDice4.Name = "txtDice4";
			this.txtDice4.Size = new global::System.Drawing.Size(30, 20);
			this.txtDice4.TabIndex = 17;
			// 
			// txtDice5
			// 
			this.txtDice5.Location = new global::System.Drawing.Point(156, 550); // Adjusted location
			this.txtDice5.Name = "txtDice5";
			this.txtDice5.Size = new global::System.Drawing.Size(30, 20);
			this.txtDice5.TabIndex = 18;
			// 
			// cmbRollNumber
			// 
			this.cmbRollNumber.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cmbRollNumber.FormattingEnabled = true;
			this.cmbRollNumber.Items.AddRange(new object[] {
			"1",
			"2",
			"3"});
			this.cmbRollNumber.Location = new global::System.Drawing.Point(280, 550); // Adjusted location
			this.cmbRollNumber.Name = "cmbRollNumber";
			this.cmbRollNumber.Size = new global::System.Drawing.Size(40, 21);
			this.cmbRollNumber.TabIndex = 20;
			// 
			// txtCurrentUpperScore
			// 
			this.txtCurrentUpperScore.Location = new global::System.Drawing.Point(390, 550); // Adjusted location
			this.txtCurrentUpperScore.Name = "txtCurrentUpperScore";
			this.txtCurrentUpperScore.Size = new global::System.Drawing.Size(40, 20);
			this.txtCurrentUpperScore.TabIndex = 21;
			this.txtCurrentUpperScore.Text = "0";
			// 
			// lblRollNumber
			// 
			this.lblRollNumber.AutoSize = true;
			this.lblRollNumber.Location = new global::System.Drawing.Point(200, 553); // Adjusted location
			this.lblRollNumber.Name = "lblRollNumber";
			this.lblRollNumber.Size = new global::System.Drawing.Size(74, 13);
			this.lblRollNumber.TabIndex = 22;
			this.lblRollNumber.Text = "当前掷骰次数:";
			// 
			// lblCurrentUpperScore
			// 
			this.lblCurrentUpperScore.AutoSize = true;
			this.lblCurrentUpperScore.Location = new global::System.Drawing.Point(325, 553); // Adjusted location
			this.lblCurrentUpperScore.Name = "lblCurrentUpperScore";
			this.lblCurrentUpperScore.Size = new global::System.Drawing.Size(59, 13);
			this.lblCurrentUpperScore.TabIndex = 23;
			this.lblCurrentUpperScore.Text = "上区总分:";
			// 
			// grpScoringCategories
			// 
			this.grpScoringCategories.Controls.Add(this.chkCatChance);
			this.grpScoringCategories.Controls.Add(this.chkCatYachtzee);
			this.grpScoringCategories.Controls.Add(this.chkCatLgStraight);
			this.grpScoringCategories.Controls.Add(this.chkCatSmStraight);
			this.grpScoringCategories.Controls.Add(this.chkCatFullHouse);
			this.grpScoringCategories.Controls.Add(this.chkCatFourOfAKind);
			this.grpScoringCategories.Controls.Add(this.chkCatThreeOfAKind);
			this.grpScoringCategories.Controls.Add(this.chkCatSixes);
			this.grpScoringCategories.Controls.Add(this.chkCatFives);
			this.grpScoringCategories.Controls.Add(this.chkCatFours);
			this.grpScoringCategories.Controls.Add(this.chkCatThrees);
			this.grpScoringCategories.Controls.Add(this.chkCatTwos);
			this.grpScoringCategories.Controls.Add(this.chkCatOnes);
			this.grpScoringCategories.Location = new System.Drawing.Point(12, 398); // New GroupBox location
			this.grpScoringCategories.Name = "grpScoringCategories";
			this.grpScoringCategories.Size = new System.Drawing.Size(706, 140); // Adjusted size
			this.grpScoringCategories.TabIndex = 5; // Adjusted TabIndex
			this.grpScoringCategories.TabStop = false;
			this.grpScoringCategories.Text = "可用计分项";
			// 
			// chkCatChance
			// 
			this.chkCatChance.AutoSize = true;
			this.chkCatChance.Location = new System.Drawing.Point(300, 110);
			this.chkCatChance.Name = "chkCatChance";
			this.chkCatChance.Size = new System.Drawing.Size(62, 17);
			this.chkCatChance.TabIndex = 12;
			this.chkCatChance.Text = "Chance";
			this.chkCatChance.UseVisualStyleBackColor = true;
			// 
			// chkCatYachtzee
			// 
			this.chkCatYachtzee.AutoSize = true;
			this.chkCatYachtzee.Location = new System.Drawing.Point(300, 87);
			this.chkCatYachtzee.Name = "chkCatYachtzee";
			this.chkCatYachtzee.Size = new System.Drawing.Size(70, 17);
			this.chkCatYachtzee.TabIndex = 11;
			this.chkCatYachtzee.Text = "Yachtzee";
			this.chkCatYachtzee.UseVisualStyleBackColor = true;
			// 
			// chkCatLgStraight
			// 
			this.chkCatLgStraight.AutoSize = true;
			this.chkCatLgStraight.Location = new System.Drawing.Point(300, 64);
			this.chkCatLgStraight.Name = "chkCatLgStraight";
			this.chkCatLgStraight.Size = new System.Drawing.Size(94, 17);
			this.chkCatLgStraight.TabIndex = 10;
			this.chkCatLgStraight.Text = "Large Straight";
			this.chkCatLgStraight.UseVisualStyleBackColor = true;
			// 
			// chkCatSmStraight
			// 
			this.chkCatSmStraight.AutoSize = true;
			this.chkCatSmStraight.Location = new System.Drawing.Point(300, 41);
			this.chkCatSmStraight.Name = "chkCatSmStraight";
			this.chkCatSmStraight.Size = new System.Drawing.Size(92, 17);
			this.chkCatSmStraight.TabIndex = 9;
			this.chkCatSmStraight.Text = "Small Straight";
			this.chkCatSmStraight.UseVisualStyleBackColor = true;
			// 
			// chkCatFullHouse
			// 
			this.chkCatFullHouse.AutoSize = true;
			this.chkCatFullHouse.Location = new System.Drawing.Point(300, 18);
			this.chkCatFullHouse.Name = "chkCatFullHouse";
			this.chkCatFullHouse.Size = new System.Drawing.Size(76, 17);
			this.chkCatFullHouse.TabIndex = 8;
			this.chkCatFullHouse.Text = "Full House";
			this.chkCatFullHouse.UseVisualStyleBackColor = true;
			// 
			// chkCatFourOfAKind
			// 
			this.chkCatFourOfAKind.AutoSize = true;
			this.chkCatFourOfAKind.Location = new System.Drawing.Point(150, 64);
			this.chkCatFourOfAKind.Name = "chkCatFourOfAKind";
			this.chkCatFourOfAKind.Size = new System.Drawing.Size(90, 17);
			this.chkCatFourOfAKind.TabIndex = 7;
			this.chkCatFourOfAKind.Text = "Four of a Kind";
			this.chkCatFourOfAKind.UseVisualStyleBackColor = true;
			// 
			// chkCatThreeOfAKind
			// 
			this.chkCatThreeOfAKind.AutoSize = true;
			this.chkCatThreeOfAKind.Location = new System.Drawing.Point(150, 41);
			this.chkCatThreeOfAKind.Name = "chkCatThreeOfAKind";
			this.chkCatThreeOfAKind.Size = new System.Drawing.Size(98, 17);
			this.chkCatThreeOfAKind.TabIndex = 6;
			this.chkCatThreeOfAKind.Text = "Three of a Kind";
			this.chkCatThreeOfAKind.UseVisualStyleBackColor = true;
			// 
			// chkCatSixes
			// 
			this.chkCatSixes.AutoSize = true;
			this.chkCatSixes.Location = new System.Drawing.Point(150, 18);
			this.chkCatSixes.Name = "chkCatSixes";
			this.chkCatSixes.Size = new System.Drawing.Size(51, 17);
			this.chkCatSixes.TabIndex = 5;
			this.chkCatSixes.Text = "Sixes";
			this.chkCatSixes.UseVisualStyleBackColor = true;
			// 
			// chkCatFives
			// 
			this.chkCatFives.AutoSize = true;
			this.chkCatFives.Location = new System.Drawing.Point(6, 133);
			this.chkCatFives.Name = "chkCatFives";
			this.chkCatFives.Size = new System.Drawing.Size(50, 17);
			this.chkCatFives.TabIndex = 4;
			this.chkCatFives.Text = "Fives";
			this.chkCatFives.UseVisualStyleBackColor = true;
			// 
			// chkCatFours
			// 
			this.chkCatFours.AutoSize = true;
			this.chkCatFours.Location = new System.Drawing.Point(6, 110);
			this.chkCatFours.Name = "chkCatFours";
			this.chkCatFours.Size = new System.Drawing.Size(51, 17);
			this.chkCatFours.TabIndex = 3;
			this.chkCatFours.Text = "Fours";
			this.chkCatFours.UseVisualStyleBackColor = true;
			// 
			// chkCatThrees
			// 
			this.chkCatThrees.AutoSize = true;
			this.chkCatThrees.Location = new System.Drawing.Point(6, 87);
			this.chkCatThrees.Name = "chkCatThrees";
			this.chkCatThrees.Size = new System.Drawing.Size(58, 17);
			this.chkCatThrees.TabIndex = 2;
			this.chkCatThrees.Text = "Threes";
			this.chkCatThrees.UseVisualStyleBackColor = true;
			// 
			// chkCatTwos
			// 
			this.chkCatTwos.AutoSize = true;
			this.chkCatTwos.Location = new System.Drawing.Point(6, 64);
			this.chkCatTwos.Name = "chkCatTwos";
			this.chkCatTwos.Size = new System.Drawing.Size(52, 17);
			this.chkCatTwos.TabIndex = 1;
			this.chkCatTwos.Text = "Twos";
			this.chkCatTwos.UseVisualStyleBackColor = true;
			// 
			// chkCatOnes
			// 
			this.chkCatOnes.AutoSize = true;
			this.chkCatOnes.Location = new System.Drawing.Point(6, 41);
			this.chkCatOnes.Name = "chkCatOnes";
			this.chkCatOnes.Size = new System.Drawing.Size(51, 17);
			this.chkCatOnes.TabIndex = 0;
			this.chkCatOnes.Text = "Ones";
			this.chkCatOnes.UseVisualStyleBackColor = true;
			// 
			// frmMain
			// 
			base.AutoScaleDimensions = new global::System.Drawing.SizeF(6f, 13f);
			base.AutoScaleMode = global::System.Windows.Forms.AutoScaleMode.Font;
			base.ClientSize = new global::System.Drawing.Size(730, 615); // Adjusted ClientSize
			base.Controls.Add(this.grpScoringCategories);
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
			base.Controls.Add(this.btnCopy);
			base.Controls.Add(this.tbStats);
			base.Controls.Add(this.tbScores);
			base.Controls.Add(this.tbDices);
			base.Name = "frmMain";
			this.Text = "Computer Yacht"; // Changed Form Title
			base.Load += new global::System.EventHandler(this.frmMain_Load);
			base.FormClosed += new global::System.Windows.Forms.FormClosedEventHandler(this.frmMain_FormClosed);
			base.Click += new global::System.EventHandler(this.frmMain_Click);
			this.grpScoringCategories.ResumeLayout(false);
			this.grpScoringCategories.PerformLayout();
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

		// Token: 0x04000085 RID: 133
		private global::System.Windows.Forms.Button btnCopy;

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
		private System.Windows.Forms.GroupBox grpScoringCategories;
		private System.Windows.Forms.CheckBox chkCatChance;
		private System.Windows.Forms.CheckBox chkCatYachtzee;
		private System.Windows.Forms.CheckBox chkCatLgStraight;
		private System.Windows.Forms.CheckBox chkCatSmStraight;
		private System.Windows.Forms.CheckBox chkCatFullHouse;
		private System.Windows.Forms.CheckBox chkCatFourOfAKind;
		private System.Windows.Forms.CheckBox chkCatThreeOfAKind;
		private System.Windows.Forms.CheckBox chkCatSixes;
		private System.Windows.Forms.CheckBox chkCatFives;
		private System.Windows.Forms.CheckBox chkCatFours;
		private System.Windows.Forms.CheckBox chkCatThrees;
		private System.Windows.Forms.CheckBox chkCatTwos;
		private System.Windows.Forms.CheckBox chkCatOnes;
	}
}
