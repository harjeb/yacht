using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace ComputerYacht
{
	// Token: 0x0200000C RID: 12
	public enum TurnStepPhase // This enum might be less relevant for the new feature but kept for now.
	{
		READY_FOR_ROLL_1,
		AWAITING_HOLD_DECISION_1, 
		READY_FOR_ROLL_2,
		AWAITING_HOLD_DECISION_2, 
		READY_FOR_ROLL_3,
		AWAITING_SCORING_DECISION, 
		TURN_COMPLETED, 
		GAME_OVER
	}

	public partial class frmMain : Form
	{
		private TurnStepPhase currentPhase; // Kept for potential future re-integration of step-by-step game
		private int[] currentDiceValues = new int[5]; // Kept for potential future re-integration
		private bool[] currentHeldDice = new bool[5]; // Kept for potential future re-integration
		private Computer compPlayer = new Computer(); // AI Player instance

		// Token: 0x0600004E RID: 78 RVA: 0x0000410C File Offset: 0x0000230C
		public frmMain()
		{
			this.InitializeComponent();
		}

		// Token: 0x0600004F RID: 79 RVA: 0x00004164 File Offset: 0x00002364
		private void frmMain_Load(object sender, EventArgs e)
		{
			this.loComp.Add(8);
			this.InitializeNewGame(); 
			if (File.Exists("Games.txt"))
			{
				File.Delete("Games.txt");
			}
			this.loFile = new FileStream("Games.txt", FileMode.Append);
			this.loFileStream = new StreamWriter(this.loFile);
			this.WriteHeaderToFile();
		}

		// Token: 0x06000050 RID: 80 RVA: 0x000041D3 File Offset: 0x000023D3
		private void btnNext_Click(object sender, EventArgs e)
		{
		}

		// Token: 0x06000051 RID: 81 RVA: 0x000041D8 File Offset: 0x000023D8
		private void WriteScoresToFile()
		{
			StringBuilder stringBuilder = new StringBuilder();
			for (int i = 0; i <= 13; i++)
			{
				stringBuilder.Append(this.yYacht.GetScore(0, i) + "\t");
			}
			stringBuilder.Append(this.yYacht.GetPlayerScore(0));
			this.loFileStream.WriteLine(stringBuilder.ToString());
		}

		// Token: 0x06000052 RID: 82 RVA: 0x00004240 File Offset: 0x00002440
		private void ResetYacht(Yacht Yacht) 
		{
			Yacht.SetupGame(new string[]
			{
				"Computer"
			}, new Computer[]
			{
				this.compPlayer 
			});
		}

		private void InitializeNewGame()
		{
			this.ResetYacht(this.yYacht);
			if (this.yYacht != null) this.yYacht.ResetForNewGame();
			// currentPhase = TurnStepPhase.READY_FOR_ROLL_1; // Less relevant for new feature
			this.iGames = 0;
			this.iTotalScore = 0;
			this.iYachtzees = 0;
			this.iBonuses = 0;
			this.iMin = 100000;
			this.iMax = 0;
			for (int i = 0; i < this.iScoreCounts.Length; i++)
			{
				this.iScoreCounts[i] = 0;
			}
			UpdateUI(); // UpdateUI might need adjustment if it relies on game phases
			UpdateStatusMessage("输入骰子点数并点击“获取保留建议”。");
			// Clear dice input textboxes
			if (txtDice1 != null) txtDice1.Text = ""; // Check for null in case InitializeComponent hasn't run
			if (txtDice2 != null) txtDice2.Text = "";
			if (txtDice3 != null) txtDice3.Text = "";
			if (txtDice4 != null) txtDice4.Text = "";
			if (txtDice5 != null) txtDice5.Text = "";
			// Clear any visual hold suggestions
			DisplayDiceHoldSuggestion(new bool[5]); 
		}

		private void UpdateStatusMessage(string message)
		{
		          Console.WriteLine("Status: " + message); 
		}

		// Token: 0x06000053 RID: 83 RVA: 0x00004274 File Offset: 0x00002474
		private void WriteHeaderToFile()
		{
			StringBuilder stringBuilder = new StringBuilder();
			for (int i = 0; i <= 13; i++)
			{
				stringBuilder.Append(frmMain.CARD_LABELS[i] + "\t");
			}
			stringBuilder.Append("Total");
			this.loFileStream.WriteLine(stringBuilder.ToString());
		}

		// Token: 0x06000054 RID: 84 RVA: 0x000042CC File Offset: 0x000024CC
		/*
		private void Next() // Old game simulation logic, commented out
		{
			// ... (original content of Next method)
		}
		*/

		// Token: 0x06000055 RID: 85 RVA: 0x00004570 File Offset: 0x00002770
		/* Removed tmrMain_Tick for manual step
		private void tmrMain_Tick(object sender, EventArgs e)
		{
			// ... (original content of tmrMain_Tick method)
		}
		*/
		// Token: 0x06000056 RID: 86 RVA: 0x000045C1 File Offset: 0x000027C1
		private void frmMain_FormClosed(object sender, FormClosedEventArgs e)
		{
			this.loFileStream.Close();
			this.loFile.Close();
		}

		// Token: 0x06000057 RID: 87 RVA: 0x000045D9 File Offset: 0x000027D9
		private void frmMain_Click(object sender, EventArgs e)
		{
		}

		// Token: 0x06000058 RID: 88 RVA: 0x000045DB File Offset: 0x000027DB
		private void tbStats_MouseMove(object sender, MouseEventArgs e)
		{
		}

		// Token: 0x06000059 RID: 89 RVA: 0x000045DD File Offset: 0x000027DD
		private void btnCopy_Click(object sender, EventArgs e)
		{
			this.tbStats.SelectAll();
			this.tbStats.Copy();
		}

		// Token: 0x0600005A RID: 90 RVA: 0x000045F5 File Offset: 0x000027F5
		/* Removed btnPause_Click for manual step
		private void btnPause_Click(object sender, EventArgs e)
		{
			// ... (original content of btnPause_Click method)
		}
		*/
		private void btnGetHoldSuggestion_Click(object sender, EventArgs e)
		{
			int[] manualDiceArray = new int[5];
			TextBox[] diceTextBoxes = { txtDice1, txtDice2, txtDice3, txtDice4, txtDice5 };

			for (int i = 0; i < 5; i++)
			{
				if (!int.TryParse(diceTextBoxes[i].Text, out int diceValue) || diceValue < 1 || diceValue > 6)
				{
					MessageBox.Show($"骰子 {i + 1} 的输入无效。请输入1到6之间的数字。", "输入错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
					return;
				}
				manualDiceArray[i] = diceValue;
			}

			try
			{
				yYacht.SetManuallyEnteredDice(manualDiceArray);
				// For AI suggestion, we assume it's like the first roll decision.
				// The AI needs to know which categories are still available to make a good decision.
				// We'll get the currently available categories for player 0.
				bool[] availableCategories = yYacht.GetPlayerAvailableCategories(0); 

				// The '1' indicates it's the equivalent of a decision after the first roll.
				bool[] holdSuggestion = compPlayer.DecideDiceToHold(manualDiceArray, 1, availableCategories); 
				DisplayDiceHoldSuggestion(holdSuggestion);
				UpdateStatusMessage("AI 保留建议已显示。");
				
				// Update tbDices to show the manually entered dice and their hold status
                this.tbDices.Text = this.yYacht.DicesToString(); 
			}
			catch (ArgumentException ex)
			{
				MessageBox.Show($"处理骰子输入时出错: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
			// The old game loop logic (switch statement based on currentPhase) is removed from this button.
			// UpdateUI() is not called here as we are not progressing the game state, only providing a suggestion.
		}

		private void DisplayDiceHoldSuggestion(bool[] holdSuggestion)
		{
			TextBox[] diceTextBoxes = { txtDice1, txtDice2, txtDice3, txtDice4, txtDice5 };
			if (holdSuggestion == null || holdSuggestion.Length != 5)
			{
				// Clear suggestion if invalid array or to reset
				for (int i = 0; i < 5; i++)
				{
					if (diceTextBoxes[i] != null) diceTextBoxes[i].BackColor = SystemColors.Window;
				}
				return;
			}

			for (int i = 0; i < 5; i++)
			{
				if (diceTextBoxes[i] != null)
				{
					diceTextBoxes[i].BackColor = holdSuggestion[i] ? Color.LightGreen : SystemColors.Window;
				}
			}
		}

		private void ProcessGameOver() // Kept for potential future re-integration
		{
			this.iGames++;
			int playerScore = this.yYacht.GetPlayerScore(0);
			this.iTotalScore += playerScore;
			if (playerScore > this.iMax) this.iMax = playerScore;
			if (playerScore < this.iMin || this.iGames == 1) this.iMin = playerScore;
			
			if (this.yYacht.GetPlayerScoreForCategory(0, 12) > 0) this.iYachtzees++; // INDEX_YACHT
			if (this.yYacht.GetPlayerScoreForCategory(0, 6) > 0) this.iBonuses++; // INDEX_TOPBONUS
			
			int scoreBracket = playerScore / 100;
			if (scoreBracket >= 0 && scoreBracket < this.iScoreCounts.Length)
			{
				this.iScoreCounts[scoreBracket]++;
			}

			string scoreDistributionText = string.Empty;
			for (int i = 0; i < this.iScoreCounts.Length; i++)
			{
				if (this.iGames > 0)
				{
					scoreDistributionText += $"Less Than {((i + 1) * 100)}:\t{Math.Round(100.0 * (double)this.iScoreCounts[i] / (double)this.iGames, 2)}%\t{this.iScoreCounts[i]}\r\n";
				}
			}

			this.tbStats.Text = string.Concat(new string[]
			{
				"GAMES PLAYED (Session): ", this.iGames.ToString(),
				"\r\nTOTAL SCORE (Session): ", this.iTotalScore.ToString(),
				"\r\nYACHTZEE % (Session): ", (this.iGames > 0 ? Math.Round(100.0 * (double)this.iYachtzees / (double)this.iGames, 2) : 0).ToString(),
				"\r\nBONUS % (Session): ", (this.iGames > 0 ? Math.Round(100.0 * (double)this.iBonuses / (double)this.iGames, 2) : 0).ToString(),
				"\r\nMIN SCORE (Session): ", this.iMin.ToString(),
				"\r\nMAX SCORE (Session): ", this.iMax.ToString(),
				"\r\nAVERAGE SCORE (Session): ", (this.iGames > 0 ? (this.iTotalScore / this.iGames) : 0).ToString(),
				"\r\n", scoreDistributionText
			});

			this.WriteScoresToFile();
		}

		private void UpdateUI() // This method might need adjustments if game state is not fully managed for the new feature
		{
			if (this.yYacht != null)
			{
				// For the new feature, tbDices might show the manually entered dice if SetManuallyEnteredDice updates yYacht's dice state
                // And DicesToString() reflects that + hold suggestion.
				this.tbDices.Text = this.yYacht.DicesToString(); 
				this.tbScores.Text = this.yYacht.PlayerToString(0); // This will show player 0's current card
				
				string turnInfo = $"回合: {yYacht.GetCurrentTurnNumber()}/13";
				// The following phase-based logic might be less relevant for the suggestion feature
				if (currentPhase != TurnStepPhase.GAME_OVER && currentPhase != TurnStepPhase.TURN_COMPLETED && yYacht.GetRollAttemptInTurn() > 0)
				{
					turnInfo += $"  掷骰次数: {yYacht.GetRollAttemptInTurn()}";
				}
		              else if (currentPhase == TurnStepPhase.GAME_OVER)
		              {
		                  turnInfo = "游戏结束";
		              }
		              Console.WriteLine(turnInfo); 
			}
		}

		// Token: 0x04000073 RID: 115
		private ComputerTester loComp = new ComputerTester();

		// Token: 0x04000074 RID: 116
		private YachtTest yYacht = new YachtTest(new Random());

		// Token: 0x04000075 RID: 117
		// private int iMovesPerGame = 10000; 

		// Token: 0x04000076 RID: 118
		private int iGames;

		// Token: 0x04000077 RID: 119
		private int iTotalScore;

		// Token: 0x04000078 RID: 120
		private int iYachtzees;

		// Token: 0x04000079 RID: 121
		private int iBonuses;

		// Token: 0x0400007A RID: 122
		private int iMin = 100000;

		// Token: 0x0400007B RID: 123
		private int iMax;

		// Token: 0x0400007C RID: 124
		private int[] iScoreCounts = new int[17];

		// Token: 0x0400007D RID: 125
		private FileStream loFile;

		// Token: 0x0400007E RID: 126
		private StreamWriter loFileStream;

		// Token: 0x0400007F RID: 127
		public static string[] CARD_LABELS = new string[]
		{
			"Ones",
			"Twos",
			"Threes",
			"Fours",
			"Fives",
			"Sixes",
			"Bonus",
			"ThreeOfAKind",
			"FourOfAKind",
			"FullHouse",
			"SmStraight",
			"LgStraight",
			"Yacht",
			"Chance"
		};
	}
}
