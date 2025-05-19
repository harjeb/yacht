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
		private TurnStepPhase currentPhase = TurnStepPhase.READY_FOR_ROLL_1; // Kept for potential future re-integration of step-by-step game
		private int[] currentDiceValues = new int[5]; // Kept for potential future re-integration
		private bool[] currentHeldDice = new bool[5]; // Kept for potential future re-integration
		private Computer compPlayer = new Computer(); // AI Player instance
		private CheckBox[] categoryCheckBoxes; // Array to hold scoring category checkboxes

		// Token: 0x0600004E RID: 78 RVA: 0x0000410C File Offset: 0x0000230C
		public frmMain()
		{
			this.InitializeComponent();
			this.InitializeCategoryCheckBoxArray(); // Initialize the checkbox array
		}

		private void InitializeCategoryCheckBoxArray()
		{
			// Assuming Yacht.NUM_CATEGORIES is 13, corresponding to the 13 selectable scoring categories.
			// The order here must match the order expected by the AI if it uses a 0-12 indexed array.
			categoryCheckBoxes = new CheckBox[] {
				chkCatOnes,           // Index 0
				chkCatTwos,           // Index 1
				chkCatThrees,         // Index 2
				chkCatFours,          // Index 3
				chkCatFives,          // Index 4
				chkCatSixes,          // Index 5
				chkCatThreeOfAKind,   // Index 6
				chkCatFourOfAKind,    // Index 7
				chkCatFullHouse,      // Index 8
				chkCatSmStraight,     // Index 9
				chkCatLgStraight,     // Index 10
				chkCatYachtzee,       // Index 11
				chkCatChance          // Index 12
			};
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
			for (int i = 0; i <= 13; i++) // CARD_LABELS has 14 items, yYacht.GetScore might expect up to index 13 (Chance)
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

			// Reset new controls
			if (cmbRollNumber != null) cmbRollNumber.SelectedIndex = 0; // Default to "1"
			if (txtCurrentUpperScore != null) txtCurrentUpperScore.Text = "0";

			// Initialize scoring category checkboxes
			if (categoryCheckBoxes != null)
			{
				for (int i = 0; i < categoryCheckBoxes.Length; i++)
				{
					if (categoryCheckBoxes[i] != null)
					{
						categoryCheckBoxes[i].Checked = true;
					}
				}
			}
		}

		private void UpdateStatusMessage(string message)
		{
			Console.WriteLine("Status: " + message);
		}

		// Token: 0x06000053 RID: 83 RVA: 0x00004274 File Offset: 0x00002474
		private void WriteHeaderToFile()
		{
			StringBuilder stringBuilder = new StringBuilder();
			for (int i = 0; i <= 13; i++) // CARD_LABELS has 14 items
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
		private bool TryParseDiceInput(out int[] diceValues)
		{
			diceValues = new int[5];
			TextBox[] diceTextBoxes = { txtDice1, txtDice2, txtDice3, txtDice4, txtDice5 };

			for (int i = 0; i < 5; i++)
			{
				if (diceTextBoxes[i] == null) // Defensive check
				{
					MessageBox.Show($"骰子 {i + 1} 对应的文本框控件不存在。", "内部错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
					Console.WriteLine($"[DEBUG frmMain] txtDice{i + 1} is null.");
					return false;
				}
				if (!int.TryParse(diceTextBoxes[i].Text, out int diceValue) || diceValue < 1 || diceValue > 6)
				{
					MessageBox.Show($"骰子 {i + 1} 的输入无效。请输入1到6之间的数字。", "输入错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
					Console.WriteLine($"[DEBUG frmMain] Invalid dice input for dice {i + 1}, value: '{diceTextBoxes[i].Text}'.");
					return false;
				}
				diceValues[i] = diceValue;
			}
			Console.WriteLine($"[DEBUG frmMain] Parsed dice array: [{string.Join(", ", diceValues)}]");
			return true;
		}

		// Helper method to parse roll number
		private bool TryParseRollNumber(out int rollNumber)
		{
			rollNumber = 0;
			if (cmbRollNumber == null) // Defensive check
			{
				MessageBox.Show("掷骰次数下拉框控件不存在。", "内部错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
				Console.WriteLine("[DEBUG frmMain] cmbRollNumber is null.");
				return false;
			}
			if (cmbRollNumber.SelectedItem == null || !int.TryParse(cmbRollNumber.SelectedItem.ToString(), out rollNumber) || rollNumber < 1 || rollNumber > 3)
			{
				MessageBox.Show("请选择有效的掷骰次数 (1, 2, 或 3)。", "输入错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
				Console.WriteLine("[DEBUG frmMain] Invalid roll number selected.");
				return false;
			}
			Console.WriteLine($"[DEBUG frmMain] Parsed roll number: {rollNumber}");
			return true;
		}

		// Helper method to parse current upper score
		private bool TryParseUpperScore(out int upperScore)
		{
			upperScore = 0;
			if (txtCurrentUpperScore == null) // Defensive check
			{
				MessageBox.Show("上区总分文本框控件不存在。", "内部错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
				Console.WriteLine("[DEBUG frmMain] txtCurrentUpperScore is null.");
				return false;
			}
			if (!int.TryParse(txtCurrentUpperScore.Text, out upperScore) || upperScore < 0)
			{
				MessageBox.Show("请输入有效的当前上区总分 (非负整数)。", "输入错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
				Console.WriteLine($"[DEBUG frmMain] Invalid upper score input: '{txtCurrentUpperScore.Text}'.");
				return false;
			}
			Console.WriteLine($"[DEBUG frmMain] Parsed current upper score: {upperScore}");
			return true;
		}

		// Helper method to get available categories from checkboxes
		private bool[] GetAvailableCategoriesFromCheckboxes()
		{
			bool[] availableCategories = new bool[Yacht.NUM_CATEGORIES];
			Console.WriteLine($"[DEBUG frmMain] categoryCheckBoxes is {(categoryCheckBoxes == null ? "NULL" : "NOT NULL")}");

			if (categoryCheckBoxes != null)
			{
				Console.WriteLine($"[DEBUG frmMain] categoryCheckBoxes.Length: {categoryCheckBoxes.Length}, Yacht.NUM_CATEGORIES: {Yacht.NUM_CATEGORIES}");
			}

			if (categoryCheckBoxes != null && categoryCheckBoxes.Length == Yacht.NUM_CATEGORIES)
			{
				Console.WriteLine("[DEBUG frmMain] Populating availableCategoriesFromCheckboxes array.");
				for (int i = 0; i < Yacht.NUM_CATEGORIES; i++)
				{
					if (categoryCheckBoxes[i] != null)
					{
						availableCategories[i] = categoryCheckBoxes[i].Checked;
					}
					else
					{
						availableCategories[i] = false; // Default to false if a specific checkbox is null
						Console.WriteLine($"[DEBUG frmMain] Warning: categoryCheckBoxes[{i}] is null. Defaulting to false for this category.");
					}
				}
			}
			else
			{
				Console.WriteLine("[DEBUG frmMain] categoryCheckBoxes array is null or length mismatch. Cannot determine available categories.");
				MessageBox.Show("计分项复选框未正确初始化。", "内部错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return null; // Indicate error
			}


			Console.WriteLine("[DEBUG frmMain] Checkbox States Before AI Call:");
			if (categoryCheckBoxes != null && categoryCheckBoxes.Length == Yacht.NUM_CATEGORIES)
			{
				for (int i = 0; i < Yacht.NUM_CATEGORIES; i++)
				{
					if (categoryCheckBoxes[i] != null)
					{
						Console.WriteLine($"[DEBUG frmMain] {categoryCheckBoxes[i].Name} (Index {i}): Checked = {categoryCheckBoxes[i].Checked}");
					}
					else
					{
						Console.WriteLine($"[DEBUG frmMain] Checkbox at Index {i} is null.");
					}
				}
			}
			else
			{
				Console.WriteLine("[DEBUG frmMain] Cannot log all checkbox states: categoryCheckBoxes array is null or length mismatch.");
			}

			return availableCategories;
		}

		private void btnGetHoldSuggestion_Click(object sender, EventArgs e)
		{
			Console.WriteLine("[DEBUG frmMain] btnGetHoldSuggestion_Click START"); // Entry log

			if (!TryParseDiceInput(out int[] manualDiceArray))
			{
				return;
			}

			if (!TryParseRollNumber(out int rollNumber))
			{
				return;
			}

			if (!TryParseUpperScore(out int currentUpperScore))
			{
				return;
			}

			bool[] availableCategoriesFromCheckboxes = GetAvailableCategoriesFromCheckboxes();
			if (availableCategoriesFromCheckboxes == null)
			{
				// Error message already shown by GetAvailableCategoriesFromCheckboxes
				return;
			}

			try
			{
				Console.WriteLine("[DEBUG frmMain] Inside try block, before SetManuallyEnteredDice.");
				yYacht.SetManuallyEnteredDice(manualDiceArray);
				Console.WriteLine("[DEBUG frmMain] After SetManuallyEnteredDice.");

				Console.WriteLine("[DEBUG frmMain] Calling compPlayer.DecideDiceToHold...");
				bool[] holdSuggestion = compPlayer.DecideDiceToHold(manualDiceArray, rollNumber, availableCategoriesFromCheckboxes, currentUpperScore);
				Console.WriteLine($"[DEBUG frmMain] compPlayer.DecideDiceToHold returned. Hold suggestion: [{string.Join(", ", holdSuggestion)}]");

				DisplayDiceHoldSuggestion(holdSuggestion);
				UpdateStatusMessage("AI 保留建议已显示。");

				this.tbDices.Text = this.yYacht.DicesToString(); // Update dice display

				if (rollNumber == 3)
				{
					UpdateStatusMessage("第三轮掷骰完成，AI 正在选择计分项...");
					Console.WriteLine("[DEBUG frmMain] Roll 3 - AI choosing category to score.");

					int[] finalDice = yYacht.GetCurrentDiceValues(); // Get dice from yYacht state set by SetManuallyEnteredDice
					int currentPlayer = yYacht.GetPlayerIndex(); // Assuming yYacht tracks the current player for scoring context
					
					// For actual scoring, AI should use the game's real available categories and upper score.
					// However, the spec for ChooseBestCategoryAndCalcScore takes these as parameters.
					// We'll use availableCategoriesFromCheckboxes and currentUpperScore from UI for now,
					// as per the context of btnGetHoldSuggestion_Click.
					// bool[] actualAvailableCategories = yYacht.GetPlayerAvailableCategories(currentPlayer);
					// int actualCurrentUpperScore = yYacht.GetPlayerUpperScore(currentPlayer);

					Tuple<int, int> decision = compPlayer.ChooseBestCategoryAndCalcScore(finalDice, availableCategoriesFromCheckboxes, currentUpperScore);
					int chosenCategoryIndex = decision.Item1;
					int calculatedScore = decision.Item2;

					Console.WriteLine($"[DEBUG frmMain] AI chose category index: {chosenCategoryIndex} with score: {calculatedScore}");

					// Record the score in the game logic
					// ApplyScoreAndFinalizeTurn also handles marking category as used and advancing turn/player
					bool gameIsOver = yYacht.ApplyScoreAndFinalizeTurn(chosenCategoryIndex, calculatedScore);
					
					UpdateStatusMessage($"AI 在类别 {frmMain.CARD_LABELS[chosenCategoryIndex]} 中获得 {calculatedScore} 分。");
					MessageBox.Show($"AI 在类别 {frmMain.CARD_LABELS[chosenCategoryIndex]} 中获得 {calculatedScore} 分。", "AI 计分", MessageBoxButtons.OK, MessageBoxIcon.Information);

					// Update UI to reflect the scored category
					if (categoryCheckBoxes != null && chosenCategoryIndex >= 0 && chosenCategoryIndex < categoryCheckBoxes.Length && categoryCheckBoxes[chosenCategoryIndex] != null)
					{
						categoryCheckBoxes[chosenCategoryIndex].Checked = false;
						categoryCheckBoxes[chosenCategoryIndex].Enabled = false;
						Console.WriteLine($"[DEBUG frmMain] Disabled checkbox for category: {frmMain.CARD_LABELS[chosenCategoryIndex]}");
					}
					
					UpdateUI(); // Refresh scoreboard and other game info

					if (gameIsOver)
					{
						ProcessGameOver(); // Handle game over statistics and messages
						UpdateStatusMessage("游戏结束！可开始新游戏或查看统计。");
						// Potentially disable further actions until InitializeNewGame or reset
					}
					else
					{
						// If not game over, the game state (turn, player) has advanced.
						// For the "Get Suggestion" button, this means the context for the *next* suggestion
						// would be for the *next* turn/player.
						// We might want to reset some UI elements like roll number for the new turn.
						if(cmbRollNumber != null) cmbRollNumber.SelectedIndex = 0; // Reset roll number to 1 for next potential turn
				                    if(txtCurrentUpperScore != null) txtCurrentUpperScore.Text = yYacht.GetPlayerUpperScore(yYacht.GetPlayerIndex()).ToString(); // Update upper score for new current player

						// Refresh available categories checkboxes based on the new game state
						bool[] newAvailableCategories = yYacht.GetPlayerAvailableCategories(yYacht.GetPlayerIndex());
						if (categoryCheckBoxes != null) {
							for(int i=0; i < categoryCheckBoxes.Length; i++) {
								if (categoryCheckBoxes[i] != null && i < newAvailableCategories.Length) {
									categoryCheckBoxes[i].Checked = newAvailableCategories[i];
									categoryCheckBoxes[i].Enabled = newAvailableCategories[i];
								}
							}
						}
				                    UpdateStatusMessage($"AI计分完毕。轮到玩家 {yYacht.GetPlayerIndex() + 1}, 回合 {yYacht.GetCurrentTurnNumber()}。");
					}
				}
				Console.WriteLine("[DEBUG frmMain] btnGetHoldSuggestion_Click END (Successful)");
			}
			catch (ArgumentException ex)
			{
				MessageBox.Show($"处理骰子输入时出错: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
				Console.WriteLine($"[DEBUG frmMain] ArgumentException: {ex.Message}");
			}
			catch (Exception ex)
			{
				MessageBox.Show($"发生意外错误: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
				Console.WriteLine($"[DEBUG frmMain] General Exception: {ex.ToString()}");
			}
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

			if (this.yYacht.GetPlayerScoreForCategory(0, Yacht.INDEX_YACHT) > 0) this.iYachtzees++; // Corrected constant
			if (this.yYacht.GetPlayerScoreForCategory(0, Yacht.INDEX_TOPBONUS) > 0) this.iBonuses++;

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
			"Ones",           // Index 0
			"Twos",           // Index 1
			"Threes",         // Index 2
			"Fours",          // Index 3
			"Fives",          // Index 4
			"Sixes",          // Index 5
			"Bonus",          // Index 6 - This is a score, not a category to choose for a turn.
			"ThreeOfAKind",   // Index 7
			"FourOfAKind",    // Index 8
			"FullHouse",      // Index 9
			"SmStraight",     // Index 10
			"LgStraight",     // Index 11
			"Yacht",          // Index 12 (Yachtzee)
			"Chance"          // Index 13
		};
	}
}
