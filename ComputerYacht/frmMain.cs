using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace ComputerYacht
{
	// Token: 0x0200000C RID: 12
	public enum TurnStepPhase
	{
		READY_FOR_ROLL_1,
		AWAITING_HOLD_DECISION_1, // After roll 1, AI decides hold
		READY_FOR_ROLL_2,
		AWAITING_HOLD_DECISION_2, // After roll 2, AI decides hold
		READY_FOR_ROLL_3,
		AWAITING_SCORING_DECISION, // After roll 3, AI decides score
		TURN_COMPLETED, // Turn is scored, ready for next turn or game over check
		GAME_OVER
	}

	public partial class frmMain : Form
	{
		private TurnStepPhase currentPhase;
		private int[] currentDiceValues = new int[5];
		private bool[] currentHeldDice = new bool[5];
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
			// this.ResetYacht(this.yYacht); // Will be handled by InitializeNewGame
			// this.tmrMain.Enabled = true; // Removed for manual step
			this.InitializeNewGame(); // Initialize game state and phase
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
		private void ResetYacht(Yacht Yacht) // This method is kept for resetting the Yacht object itself
		{
			Yacht.SetupGame(new string[]
			{
				"Computer"
			}, new Computer[]
			{
				this.compPlayer // Use the class-level AI player
			});
		}

		private void InitializeNewGame()
		{
			this.ResetYacht(this.yYacht);
			if (this.yYacht != null) this.yYacht.ResetForNewGame();
			this.currentPhase = TurnStepPhase.READY_FOR_ROLL_1;
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
			UpdateUI();
			UpdateStatusMessage("点击“手动单步模拟”开始新游戏或进行第一次掷骰。");
		}

		private void UpdateStatusMessage(string message)
		{
			// Assuming lblStatusMessage is a Label control you'll add to the form's design.
			// if (this.lblStatusMessage != null) { this.lblStatusMessage.Text = message; }
			// else { Console.WriteLine("Status: " + message); } // Fallback for now
		          Console.WriteLine("Status: " + message); // Temporary, until UI elements are confirmed
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
		private void Next()
		{
			bool flag = this.yYacht.ComputerNextMove();
			string text = string.Empty;
			if (flag)
			{
				this.iGames++;
				int playerScore = this.yYacht.GetPlayerScore(0);
				this.iTotalScore += playerScore;
				if (playerScore > this.iMax)
				{
					this.iMax = playerScore;
				}
				if (playerScore < this.iMin)
				{
					this.iMin = playerScore;
				}
				if (this.yYacht.GetScore(0, 12) != 0)
				{
					this.iYachtzees++;
				}
				if (this.yYacht.GetScore(0, 6) != 0)
				{
					this.iBonuses++;
				}
				this.iScoreCounts[(int)Math.Truncate((double)playerScore / 100.0)]++;
				for (int i = 0; i < this.iScoreCounts.Length; i++)
				{
					string text2 = text;
					text = string.Concat(new string[]
					{
						text2,
						"Less Than ",
						((i + 1) * 100).ToString(),
						":\t",
						Math.Round(100.0 * (double)this.iScoreCounts[i] / (double)this.iGames, 2).ToString(),
						"\t",
						this.iScoreCounts[i].ToString(),
						"\r\n"
					});
				}
				this.tbStats.Text = string.Concat(new string[]
				{
					"GAMES PLAYED: ",
					this.iGames.ToString(),
					"\r\nTOTAL SCORE: ",
					this.iTotalScore.ToString(),
					"\r\nYACHTZEE %: ",
					(100.0 * (double)this.iYachtzees / (double)this.iGames).ToString(),
					"\r\nBONUS %: ",
					(100.0 * (double)this.iBonuses / (double)this.iGames).ToString(),
					"\r\nMIN SCORE: ",
					this.iMin.ToString(),
					"\r\nMAX SCORE: ",
					this.iMax.ToString(),
					"\r\nAVERAGE SCORE: ",
					(this.iTotalScore / this.iGames).ToString(),
					"\r\n",
					text
				});
				this.WriteScoresToFile();
				            InitializeNewGame();
			}
			UpdateUI();
		}
		*/

		// Token: 0x06000055 RID: 85 RVA: 0x00004570 File Offset: 0x00002770
		/* Removed tmrMain_Tick for manual step
		private void tmrMain_Tick(object sender, EventArgs e)
		{
			for (int i = 0; i < this.iMovesPerGame; i++)
			{
				this.Next();
			}
			this.tbDices.Text = this.yYacht.DicesToString();
			this.tbScores.Text = this.yYacht.PlayerToString(0);
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
			if (this.iMovesPerGame == 10000)
			{
				this.iMovesPerGame = 1;
				// this.tmrMain.Interval = 1500; // tmrMain is removed
				return;
			}
			this.iMovesPerGame = 10000;
			// this.tmrMain.Interval = 1; // tmrMain is removed
		}
		*/
		private void btnManualStep_Click(object sender, EventArgs e)
		{
			if (yYacht == null) InitializeNewGame();

			switch (currentPhase)
			{
				case TurnStepPhase.READY_FOR_ROLL_1:
					if (yYacht.GetCurrentTurnNumber() == 0) yYacht.ResetForNewGame(); // Ensure game is reset if starting from scratch
		                  yYacht.ResetForNewTurnIfNeeded();
					currentDiceValues = yYacht.PerformRoll(1);
					UpdateStatusMessage($"第 {yYacht.GetCurrentTurnNumber()} 回合 - 第1掷完成。AI 正在决定保留...");
					currentHeldDice = compPlayer.DecideDiceToHold(currentDiceValues, yYacht.GetRollAttemptInTurn(), yYacht.GetPlayerAvailableCategories(0));
					yYacht.ApplyHoldDecision(currentHeldDice);
					currentPhase = TurnStepPhase.READY_FOR_ROLL_2;
					UpdateStatusMessage($"AI 已为第1掷决策保留。点击进行第2掷。");
					break;

				case TurnStepPhase.READY_FOR_ROLL_2:
					currentDiceValues = yYacht.PerformRoll(2);
					UpdateStatusMessage($"第 {yYacht.GetCurrentTurnNumber()} 回合 - 第2掷完成。AI 正在决定保留...");
					currentHeldDice = compPlayer.DecideDiceToHold(currentDiceValues, yYacht.GetRollAttemptInTurn(), yYacht.GetPlayerAvailableCategories(0));
					yYacht.ApplyHoldDecision(currentHeldDice);
					currentPhase = TurnStepPhase.READY_FOR_ROLL_3;
					UpdateStatusMessage($"AI 已为第2掷决策保留。点击进行第3掷。");
					break;

				case TurnStepPhase.READY_FOR_ROLL_3:
					currentDiceValues = yYacht.PerformRoll(3);
					UpdateStatusMessage($"第 {yYacht.GetCurrentTurnNumber()} 回合 - 第3掷完成。AI 正在选择计分项...");
					ScoringDecision decision = compPlayer.ChooseScoreCategory(currentDiceValues, yYacht.GetPlayerAvailableCategories(0));
                    string[] diceStrings = new string[currentDiceValues.Length];
                    for (int i = 0; i < currentDiceValues.Length; i++)
                    {
                        diceStrings[i] = currentDiceValues[i].ToString();
                    }
                    Console.WriteLine($"[frmMain.AWAITING_SCORING_DECISION] AI Dice: {string.Join(",", diceStrings)}");
                    Console.WriteLine($"[frmMain.AWAITING_SCORING_DECISION] AI Decision: Category={decision.CategoryIndex} ({frmMain.CARD_LABELS[decision.CategoryIndex]}), Score={decision.Score}");
					bool gameJustEnded = yYacht.ApplyScoreAndFinalizeTurn(decision.CategoryIndex, decision.Score);
					UpdateStatusMessage($"AI 已选择在 {frmMain.CARD_LABELS[decision.CategoryIndex]} 计 {decision.Score} 分。");

					if (gameJustEnded)
					{
						currentPhase = TurnStepPhase.GAME_OVER;
						ProcessGameOver();
						UpdateStatusMessage($"游戏结束! 总分: {yYacht.GetPlayerScore(0)}. 点击“手动单步模拟”开始新游戏。");
					}
					else
					{
						currentPhase = TurnStepPhase.TURN_COMPLETED;
						UpdateStatusMessage($"回合 {yYacht.GetCurrentTurnNumber()-1} 结束。点击“手动单步模拟”开始下一回合。");
					}
					break;

				case TurnStepPhase.TURN_COMPLETED:
					currentPhase = TurnStepPhase.READY_FOR_ROLL_1;
					UpdateStatusMessage($"第 {yYacht.GetCurrentTurnNumber()} 回合开始。点击进行第1掷。");
					break;

				case TurnStepPhase.GAME_OVER:
					InitializeNewGame();
					break;
			}
			UpdateUI();
		}

		private void ProcessGameOver()
		{
			this.iGames++;
			int playerScore = this.yYacht.GetPlayerScore(0);
			this.iTotalScore += playerScore;
			if (playerScore > this.iMax) this.iMax = playerScore;
			if (playerScore < this.iMin || this.iGames == 1) this.iMin = playerScore;
			
			if (this.yYacht.GetPlayerScoreForCategory(0, 12) > 0) this.iYachtzees++;
			if (this.yYacht.GetPlayerScoreForCategory(0, 6) > 0) this.iBonuses++;
			
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

		private void UpdateUI()
		{
			if (this.yYacht != null)
			{
				this.tbDices.Text = this.yYacht.DicesToString();
				this.tbScores.Text = this.yYacht.PlayerToString(0);
				
				string turnInfo = $"回合: {yYacht.GetCurrentTurnNumber()}/13";
				if (currentPhase != TurnStepPhase.GAME_OVER && currentPhase != TurnStepPhase.TURN_COMPLETED && yYacht.GetRollAttemptInTurn() > 0)
				{
					turnInfo += $"  掷骰次数: {yYacht.GetRollAttemptInTurn()}";
				}
		              else if (currentPhase == TurnStepPhase.GAME_OVER)
		              {
		                  turnInfo = "游戏结束";
		              }

		              // Assuming lblTurnInfo is a Label control you'll add to the form's design.
				// if (this.lblTurnInfo != null) { this.lblTurnInfo.Text = turnInfo; }
				// else { Console.WriteLine(turnInfo); } // Fallback for now
		              Console.WriteLine(turnInfo); // Temporary
			}
		}

		// Token: 0x04000073 RID: 115
		private ComputerTester loComp = new ComputerTester();

		// Token: 0x04000074 RID: 116
		private YachtTest yYacht = new YachtTest(new Random());

		// Token: 0x04000075 RID: 117
		// private int iMovesPerGame = 10000; // Removed as tmrMain is removed

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
