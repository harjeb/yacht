using System;

namespace ComputerYacht
{
	// Token: 0x02000004 RID: 4
	public class Yacht
	{
		// Token: 0x06000009 RID: 9 RVA: 0x000021D3 File Offset: 0x000003D3
		public Yacht(Random Rand)
		{
			this.Initialise(Rand);
		}

		// Token: 0x0600000A RID: 10 RVA: 0x000021E2 File Offset: 0x000003E2
		public void Initialise(Random Rand)
		{
			this.oRandom = Rand;
			this.bDicesHold = new bool[5];
			this.iDicesValue = new int[5];
			this.iDicesValueSorted = new int[5];
			this.iCurrentTurnNumber = 0; // Initialize current turn number
			this.Reset();
		}

		// Token: 0x17000003 RID: 3
		// (get) Token: 0x0600000B RID: 11 RVA: 0x00002215 File Offset: 0x00000415
		// (set) Token: 0x0600000C RID: 12 RVA: 0x0000221D File Offset: 0x0000041D
		public bool[] DicesHold
		{
			get
			{
				return this.bDicesHold;
			}
			set
			{
				this.bDicesHold = value;
			}
		}

		// Token: 0x0600000D RID: 13 RVA: 0x00002228 File Offset: 0x00000428
		public void ClearHeld()
		{
			for (int i = 0; i < 5; i++)
			{
				this.bDicesHold[i] = false;
			}
		}

		// Token: 0x0600000E RID: 14 RVA: 0x0000224C File Offset: 0x0000044C
		public void RollDices()
		{
			for (int i = 0; i < 5; i++)
			{
				if (!this.bDicesHold[i])
				{
					this.iDicesValue[i] = Yacht.ROLL_DICES[this.iDicesValue[i] - 1][(this.oRandom.Next() & 16777215) % 4];
				}
				this.iDicesValueSorted[i] = this.iDicesValue[i];
			}
			Array.Sort<int>(this.iDicesValueSorted);
		}

		// Token: 0x0600000F RID: 15 RVA: 0x000022B8 File Offset: 0x000004B8
		[Obsolete("ComputerNextMove is deprecated, use step-by-step methods in frmMain instead.")]
		public bool ComputerNextMove()
		{
			throw new NotSupportedException("ComputerNextMove is deprecated. Use frmMain's step-through logic.");
		}

		public int[] PerformRoll(int rollAttemptInTurn)
		{
			this.iRollIndex = rollAttemptInTurn - 1;
			if (this.iRollIndex < 0 || this.iRollIndex > 2)
			{
				throw new ArgumentOutOfRangeException(nameof(rollAttemptInTurn), "Roll attempt must be between 1 and 3.");
			}
			// First roll of a turn should clear holds from previous turn (if any)
			if (this.iRollIndex == 0) 
			{
				this.ClearHeld();
			}
			this.RollDices(); // RollDices respects bDicesHold
			return (int[])this.iDicesValue.Clone();
		}

		public void ApplyHoldDecision(bool[] diceToHoldFromAI)
		{
			if (diceToHoldFromAI == null || diceToHoldFromAI.Length != 5)
			{
				throw new ArgumentException("Dice to hold array must have 5 elements.", nameof(diceToHoldFromAI));
			}
			this.bDicesHold = (bool[])diceToHoldFromAI.Clone();
		}
		
		public bool ApplyScoreAndFinalizeTurn(int categoryIndexToScore, int actualScore)
		{
			this.ScoreValue(categoryIndexToScore, true, actualScore);

			this.iRollIndex = 0; 
			this.ClearHeld();    

			this.iPlayerIndex++;
			if (this.iPlayerIndex >= this.iNumberOfPlayers)
			{
				this.iPlayerIndex = 0;
				if(this.iNumberOfPlayers > 0) 
				    this.iCurrentTurnNumber++; 
			}
			// For single player game, iCurrentTurnNumber is incremented when iPlayerIndex wraps.
			// If it's the very first turn of the game, and iCurrentTurnNumber was 0, it should become 1.
            // ResetForNewGame sets it to 1. ApplyScoreAndFinalizeTurn increments it after player 0 if num players = 1.
            // This seems correct. If iCurrentTurnNumber starts at 1 (from ResetForNewGame), 
            // after player 0's first turn (if 1 player), iPlayerIndex wraps, iCurrentTurnNumber becomes 2.

			return this.IsGameOver(); 
		}

		public void ResetForNewTurnIfNeeded()
		{
            // This method is called by frmMain at the start of READY_FOR_ROLL_1.
            // ApplyScoreAndFinalizeTurn should have already reset iRollIndex and ClearHeld.
            // This can be a safeguard. If iRollIndex is not 0, it means something is off or it's the very first roll of the game.
            if (iRollIndex != 0 && iCurrentTurnNumber > 0) // Don't clear if it's the absolute start of the game before any turn logic
            {
                 //This might indicate an unexpected state if a turn didn't finalize correctly.
            }
		}

		public void ResetForNewGame()
		{
			this.Reset(); // Resets player/roll indices, initializes dice
			this.iCurrentTurnNumber = 1; 
			if (this.iScores != null && this.iNumberOfPlayers > 0)
			{
				for (int i = 0; i < this.iNumberOfPlayers; i++)
				{
					for (int j = 0; j < this.iScores.GetLength(1); j++)
					{
						this.iScores[i, j] = Yacht.CARD_DEFAULTS[j];
					}
				}
			}
            else if (this.iScores == null && this.iNumberOfPlayers > 0) // Should be handled by SetupGame
            {
                this.iScores = new int[this.iNumberOfPlayers, Yacht.CARD_LABELS.Length];
                 for (int i = 0; i < this.iNumberOfPlayers; i++)
				{
					for (int j = 0; j < this.iScores.GetLength(1); j++)
					{
						this.iScores[i, j] = Yacht.CARD_DEFAULTS[j];
					}
				}
            }
			this.iPlayerIndex = 0; 
		}

		// Token: 0x06000010 RID: 16 RVA: 0x000023A8 File Offset: 0x000005A8
		public int GetScore(int Player, int Item)
		{
			if (Player < 0 || Player >= iNumberOfPlayers || Item < 0 || Item >= iScores.GetLength(1))
				return 0; // Or throw exception
			return this.iScores[Player, Item];
		}

		// Token: 0x06000011 RID: 17 RVA: 0x000023B8 File Offset: 0x000005B8
		public int ScoreValue(int Item, bool Store, int? preCalculatedScore = null)
		{
			int scoreToActuallyStore;

			if (Store && preCalculatedScore.HasValue)
			{
				scoreToActuallyStore = preCalculatedScore.Value;
			}
			else
			{
				scoreToActuallyStore = this.GetDiceScore(Item);
			}
			
			if (!Store) // If just querying, return the calculated score
			{
				return scoreToActuallyStore;
			}

			// Proceed with storing the score
			this.iScores[this.iPlayerIndex, Item] = scoreToActuallyStore;
			
			// Handle Yachtzee bonus for multiple Yachtzees
			bool isCurrentRollAYachtzee = (this.iDicesValueSorted[0] == this.iDicesValueSorted[1] && 
			                               this.iDicesValueSorted[1] == this.iDicesValueSorted[2] && 
			                               this.iDicesValueSorted[2] == this.iDicesValueSorted[3] && 
			                               this.iDicesValueSorted[3] == this.iDicesValueSorted[4]);

			if (isCurrentRollAYachtzee && this.iScores[this.iPlayerIndex, INDEX_YACHT] > 0) 
			{
                // If already scored a Yachtzee (value > 0), and this roll is also a Yachtzee
                // Add 100 bonus to the Yachtzee score slot, regardless of where this Yachtzee is being scored now.
                // (Unless it's being scored in the Yachtzee slot itself for the *first* time, which is covered by scoreToActuallyStore being 50)
                if (Item != INDEX_YACHT || (Item == INDEX_YACHT && scoreToActuallyStore == 50 && this.iScores[this.iPlayerIndex, INDEX_YACHT] > 50) )
                {
                     // If we are scoring this yachtzee in a number category, or if we are scoring it in the yachtzee category but it's a subsequent one.
                     // The original logic: `this.iScores[this.iPlayerIndex, 12] += 100;`
                     // This implies the bonus is always added to the Yachtzee category score.
                     this.iScores[this.iPlayerIndex, INDEX_YACHT] += 100;
                }
			}
            else if (isCurrentRollAYachtzee && Item == INDEX_YACHT && scoreToActuallyStore == 50)
            {
                // This is the first Yachtzee being scored in the Yachtzee box.
                // No *additional* bonus here, the 50 is the score.
                // The iScores[this.iPlayerIndex, Item] = scoreToActuallyStore; above handles this.
            }


			// Upper section bonus
			if (Item <= INDEX_SIXES) 
			{
				if (this.iScores[this.iPlayerIndex, INDEX_TOPBONUS] == 0) 
				{
					int upperTotal = 0;
					for (int i = INDEX_ONES; i <= INDEX_SIXES; i++)
					{
						upperTotal += ((this.iScores[this.iPlayerIndex, i] < 0) ? 0 : this.iScores[this.iPlayerIndex, i]);
					}
					if (upperTotal >= 63)
					{
						this.iScores[this.iPlayerIndex, INDEX_TOPBONUS] = 35;
					}
				}
			}
			return scoreToActuallyStore; // Return the score recorded for the specified item
		}

		// Token: 0x06000012 RID: 18 RVA: 0x00002514 File Offset: 0x00000714
		public void SetupGame(string[] Players, Computer[] Computers)
		{
			this.Reset(); // Basic reset of indices and dice
			this.iNumberOfPlayers = Players.Length;
			this.sPlayerNames = Players;
			this.oComputers = Computers;
			this.bComputers = new bool[Players.Length];
			this.iScores = new int[this.iNumberOfPlayers, Yacht.CARD_LABELS.Length];
			for (int i = 0; i < this.iNumberOfPlayers; i++)
			{
				this.bComputers[i] = (this.oComputers.Length > i && this.oComputers[i] != null);
				for (int j = 0; j < this.iScores.GetLength(1); j++)
				{
					this.iScores[i, j] = Yacht.CARD_DEFAULTS[j];
				}
			}
			this.iPlayerIndex = 0; // Start with player 0
            this.iCurrentTurnNumber = 1; // Game starts at turn 1
		}

		// Token: 0x06000013 RID: 19 RVA: 0x000025C4 File Offset: 0x000007C4
		public int GetPlayerScore(int Player)
		{
			if (Player < 0 || Player >= iNumberOfPlayers || iScores == null) return 0;
			int num = 0;
			for (int i = 0; i < Yacht.CARD_LABELS.Length; i++)
			{
				if (this.iScores[Player, i] >= 0)
				{
					num += this.iScores[Player, i];
				}
			}
			return num;
		}

		// Token: 0x06000014 RID: 20 RVA: 0x00002608 File Offset: 0x00000808
		public int GetDiceScore(int Item)
		{
			int num = 0;
			bool flag = this.iDicesValueSorted[0] == this.iDicesValueSorted[1] && this.iDicesValueSorted[1] == this.iDicesValueSorted[2] && this.iDicesValueSorted[2] == this.iDicesValueSorted[3] && this.iDicesValueSorted[3] == this.iDicesValueSorted[4];
			bool flag2 = flag && this.iScores[this.iPlayerIndex, 12] >= 0 && this.iScores[this.iPlayerIndex, this.iDicesValueSorted[0] - 1] >= 0;
			switch (Item)
			{
			case 0:
			case 1:
			case 2:
			case 3:
			case 4:
			case 5:
				num += ((this.iDicesValueSorted[0] != Item + 1) ? 0 : (Item + 1));
				num += ((this.iDicesValueSorted[1] != Item + 1) ? 0 : (Item + 1));
				num += ((this.iDicesValueSorted[2] != Item + 1) ? 0 : (Item + 1));
				num += ((this.iDicesValueSorted[3] != Item + 1) ? 0 : (Item + 1));
				num += ((this.iDicesValueSorted[4] != Item + 1) ? 0 : (Item + 1));
				break;
			case 7: // 3 of a Kind
				if (flag2 || (this.iDicesValueSorted[0] == this.iDicesValueSorted[1] && this.iDicesValueSorted[1] == this.iDicesValueSorted[2]) || (this.iDicesValueSorted[1] == this.iDicesValueSorted[2] && this.iDicesValueSorted[2] == this.iDicesValueSorted[3]) || (this.iDicesValueSorted[2] == this.iDicesValueSorted[3] && this.iDicesValueSorted[3] == this.iDicesValueSorted[4]))
				{
					num = this.iDicesValueSorted[0] + this.iDicesValueSorted[1] + this.iDicesValueSorted[2] + this.iDicesValueSorted[3] + this.iDicesValueSorted[4];
				}
				break;
			case 8: // 4 of a Kind
				if (flag2 || (this.iDicesValueSorted[0] == this.iDicesValueSorted[1] && this.iDicesValueSorted[1] == this.iDicesValueSorted[2] && this.iDicesValueSorted[2] == this.iDicesValueSorted[3]) || (this.iDicesValueSorted[1] == this.iDicesValueSorted[2] && this.iDicesValueSorted[2] == this.iDicesValueSorted[3] && this.iDicesValueSorted[3] == this.iDicesValueSorted[4]))
				{
					num = this.iDicesValueSorted[0] + this.iDicesValueSorted[1] + this.iDicesValueSorted[2] + this.iDicesValueSorted[3] + this.iDicesValueSorted[4];
				}
				break;
			case 9: // Full House
				if ((this.iDicesValueSorted[0] == this.iDicesValueSorted[1] && this.iDicesValueSorted[2] == this.iDicesValueSorted[3] && this.iDicesValueSorted[3] == this.iDicesValueSorted[4]) || (this.iDicesValueSorted[0] == this.iDicesValueSorted[1] && this.iDicesValueSorted[1] == this.iDicesValueSorted[2] && this.iDicesValueSorted[3] == this.iDicesValueSorted[4]))
				{
					num = 25;
				}
				break;
			case 10: // Small Straight
				if (flag2 || (this.iDicesValueSorted[0] + 1 == this.iDicesValueSorted[1] && this.iDicesValueSorted[1] + 1 == this.iDicesValueSorted[2] && (this.iDicesValueSorted[2] + 1 == this.iDicesValueSorted[3] || this.iDicesValueSorted[2] + 1 == this.iDicesValueSorted[4])))
				{
					num = 30;
				}
				else if (this.iDicesValueSorted[1] + 1 == this.iDicesValueSorted[2] && this.iDicesValueSorted[2] + 1 == this.iDicesValueSorted[3] && (this.iDicesValueSorted[0] + 1 == this.iDicesValueSorted[3] || this.iDicesValueSorted[3] + 1 == this.iDicesValueSorted[4]))
				{
					num = 30;
				}
				else if (this.iDicesValueSorted[2] + 1 == this.iDicesValueSorted[3] && this.iDicesValueSorted[3] + 1 == this.iDicesValueSorted[4] && (this.iDicesValueSorted[0] + 1 == this.iDicesValueSorted[2] || this.iDicesValueSorted[1] + 1 == this.iDicesValueSorted[2]))
				{
					num = 30;
				}
				break;
			case 11: // Large Straight
				if (flag2 || (this.iDicesValueSorted[0] + 1 == this.iDicesValueSorted[1] && this.iDicesValueSorted[1] + 1 == this.iDicesValueSorted[2] && this.iDicesValueSorted[2] + 1 == this.iDicesValueSorted[3] && this.iDicesValueSorted[3] + 1 == this.iDicesValueSorted[4]))
				{
					num = 40;
				}
				break;
			case 12: // Yacht
				num = (flag ? 50 : 0);
				break;
			case 13: // Chance
				num = this.iDicesValueSorted[0] + this.iDicesValueSorted[1] + this.iDicesValueSorted[2] + this.iDicesValueSorted[3] + this.iDicesValueSorted[4];
				break;
			}
			return num;
		}

		// Token: 0x06000015 RID: 21 RVA: 0x00002ACC File Offset: 0x00000CCC
		public int GetPlayerIndex()
		{
			return this.iPlayerIndex;
		}

		// Token: 0x06000016 RID: 22 RVA: 0x00002AD4 File Offset: 0x00000CD4
		// public int GetRollIndex() // Original name
		// {
		// 	return this.iRollIndex;
		// }
		public int GetRollAttemptInTurn() // New name as per spec
		{
			return this.iRollIndex + 1; // iRollIndex is 0-based
		}


		// Token: 0x06000017 RID: 23 RVA: 0x00002ADC File Offset: 0x00000CDC
		public int CurrentPlayersScore(int index) // Gets score for current player at specific category index
		{
		    if (iPlayerIndex < 0 || iPlayerIndex >= iNumberOfPlayers || index < 0 || index >= iScores.GetLength(1)) return 0;
			return this.iScores[this.iPlayerIndex, index];
		}

		// Token: 0x06000018 RID: 24 RVA: 0x00002AF0 File Offset: 0x00000CF0
		public int GetDicesValueSorted(int Index)
		{
			return this.iDicesValueSorted[Index];
		}

		// Token: 0x06000019 RID: 25 RVA: 0x00002AFA File Offset: 0x00000CFA
		// public int GetDicesValue(int Index) // Original name
		// {
		// 	return this.iDicesValue[Index];
		// }
		public int[] GetCurrentDiceValues() // New name as per spec
		{
		    return (int[])this.iDicesValue.Clone();
		}
        public bool[] GetCurrentHeldDice()
        {
            return (bool[])this.bDicesHold.Clone();
        }


		// Token: 0x0600001A RID: 26 RVA: 0x00002B04 File Offset: 0x00000D04
		public int GetDicesSum()
		{
			int num = 0;
			for (int i = 0; i < 5; i++)
			{
				num += this.iDicesValue[i]; // Use iDicesValue directly
			}
			return num;
		}

		// Token: 0x0600001B RID: 27 RVA: 0x00002B2C File Offset: 0x00000D2C
		public int[] GetDiceCounts()
		{
			int[] array = new int[6];
			for (int i = 0; i < 5; i++)
			{
				array[this.iDicesValue[i] - 1]++; // Use iDicesValue directly
			}
			return array;
		}

		// Token: 0x0600001C RID: 28 RVA: 0x00002B69 File Offset: 0x00000D69
		protected void Reset()
		{

			this.iRollIndex = 0;
			this.iPlayerIndex = 0; // Default to player 0; SetupGame will confirm/override if needed.
			this.iCurrentTurnNumber = 0; // Will be set to 1 by ResetForNewGame or SetupGame
			this.InitializeDices();
            this.ClearHeld();
		}

		// Token: 0x0600001D RID: 29 RVA: 0x00002B88 File Offset: 0x00000D88
		protected int GetPlayerBonusScore(int Player)
		{
		    if (Player < 0 || Player >= iNumberOfPlayers || iScores == null) return 0;
			int num = 63;
			for (int i = 0; i < 6; i++)
			{
				if (this.iScores[Player, i] >= 0)
				{
					num -= this.iScores[Player, i];
				}
			}
			if (num < 0)
			{
				num = 0;
			}
			return num;
		}

		// Token: 0x0600001E RID: 30 RVA: 0x00002BCB File Offset: 0x00000DCB
		protected int GetBonusScore()
		{
			return this.GetPlayerBonusScore(this.iPlayerIndex);
		}

		// Token: 0x0600001F RID: 31 RVA: 0x00002BDC File Offset: 0x00000DDC
		protected int GetBestScoreIndex() // This is AI logic, should ideally be in Computer.cs
		{
			int num = 0;
			int num2 = -1;
			for (int i = 0; i < 13; i++) // Iterate up to INDEX_YACHT, not Chance initially
			{
			    if (i == INDEX_TOPBONUS) continue; // Skip bonus category
				if (this.iScores[this.iPlayerIndex, i] == -1) // If category is available
				{
					int diceScore = this.GetDiceScore(i);
					if (num2 == -1 || diceScore >= num)
					{
						num = diceScore;
						num2 = i;
					}
				}
			}
			// If no good score found, or best is 0, consider Chance or first available 0
			if (num2 == -1 || num == 0)
			{
				if (this.iScores[this.iPlayerIndex, INDEX_CHANCE] == -1) // If Chance is available
				{
					num2 = INDEX_CHANCE;
				}
				else // Find any available category to score 0
				{
					for (int j = 0; j < Yacht.CARD_LABELS.Length; j++)
					{
					    if (j == INDEX_TOPBONUS) continue;
						if (this.iScores[this.iPlayerIndex, j] == -1)
						{
							num2 = j;
							break;
						}
					}
				}
			}
			// If still -1 (all categories scored, which shouldn't happen if IsGameOver is working)
			if (num2 == -1) { 
			    // Fallback: find first available, even if it was checked. This indicates an issue.
			    for (int j = 0; j < Yacht.CARD_LABELS.Length; j++) {
			        if (j == INDEX_TOPBONUS) continue;
			        if (this.iScores[this.iPlayerIndex, j] == -1) { num2 = j; break; }
			    }
			}
			return num2;
		}

		// Token: 0x06000020 RID: 32 RVA: 0x00002C7C File Offset: 0x00000E7C
		protected void InitializeDices()
		{
			for (int i = 0; i < 5; i++)
			{
				this.iDicesValue[i] = 1 + (this.oRandom.Next() & 16777215) % 6;
			}
		}

		// Token: 0x06000021 RID: 33 RVA: 0x00002CB4 File Offset: 0x00000EB4
		protected bool IsGameOver(int Player) // Checks if a specific player has finished
		{
		    if (Player < 0 || Player >= iNumberOfPlayers || iScores == null) return true; // Invalid player or no scores
			for (int i = 0; i < Yacht.CARD_LABELS.Length; i++)
			{
			    if (i == INDEX_TOPBONUS) continue; // Bonus is not a scorable slot by player
				if (this.iScores[Player, i] == -1)
				{
					return false;
				}
			}
			return true;
		}
        // New public IsGameOver for the whole game
        public bool IsGameOver() 
		{
            Console.WriteLine($"[Yacht.IsGameOver] Checking: iCurrentTurnNumber = {this.iCurrentTurnNumber}, iNumberOfPlayers = {this.iNumberOfPlayers}");
            if (iNumberOfPlayers == 0) 
            {
                Console.WriteLine("[Yacht.IsGameOver] Result: true (iNumberOfPlayers is 0)");
                return true; 
            }
            // Game is over if all 13 turns are completed.
            // iCurrentTurnNumber is 1-based and increments after all players in a round complete their turn.
            bool isOver = this.iCurrentTurnNumber > 13;
            Console.WriteLine($"[Yacht.IsGameOver] Result: {isOver} (iCurrentTurnNumber > 13 is {this.iCurrentTurnNumber} > 13)");
            return isOver; 
		}

        // Additional Getters for frmMain
        public bool[] GetPlayerAvailableCategories(int player)
		{
			if (player < 0 || player >= iNumberOfPlayers || iScores == null) 
                return new bool[Yacht.CARD_LABELS.Length]; // Return all false or throw

			bool[] available = new bool[Yacht.CARD_LABELS.Length];
			for (int i = 0; i < Yacht.CARD_LABELS.Length; i++)
			{
				if (i == INDEX_TOPBONUS) 
                {
                    available[i] = false; 
                }
				else
				{
					available[i] = (this.iScores[player, i] == -1);
				}
			}
			return available;
		}
		
		public bool IsCategoryAvailable(int categoryIndex) // For current player
		{
		    if (iPlayerIndex < 0 || iPlayerIndex >= iNumberOfPlayers) return false;
		    if (categoryIndex < 0 || categoryIndex >= Yacht.CARD_LABELS.Length) return false;
		    if (categoryIndex == INDEX_TOPBONUS) return false;
			return this.iScores[this.iPlayerIndex, categoryIndex] == -1;
		}

		public int GetPlayerScoreForCategory(int player, int category)
		{
		    if (player < 0 || player >= iNumberOfPlayers || category < 0 || category >= Yacht.CARD_LABELS.Length || iScores == null)
		        return 0; // Or throw
			return this.GetScore(player, category);
		}
        public int GetCurrentTurnNumber()
		{
			return this.iCurrentTurnNumber;
		}

		public string DicesToString()
		{
			string text = "";
			if (iDicesValue == null || iDicesValue.Length != 5) return "Dice not initialized";
			for (int i = 0; i < 5; i++)
			{
				text += this.iDicesValue[i].ToString();
				if (this.bDicesHold != null && this.bDicesHold.Length == 5 && this.bDicesHold[i])
				{
					text += " (H)";
				}
				if (i < 4)
				{
					text += ", ";
				}
			}
			return text;
		}

		public string PlayerToString(int Player)
		{
			string text = "";
			if (Player >= 0 && Player < this.iNumberOfPlayers && sPlayerNames != null && sPlayerNames.Length > Player && iScores != null)
			{
				text = text + this.sPlayerNames[Player] + " - Turn: " + this.iCurrentTurnNumber + "/13\r\n";
				for (int i = 0; i < Yacht.CARD_LABELS.Length; i++)
				{
					text = string.Concat(new string[]
					{
						text,
						Yacht.CARD_LABELS[i].PadRight(12), // Pad for alignment
						": ",
						(this.iScores[Player, i] == -1) ? "NS".PadRight(3) : this.iScores[Player, i].ToString().PadRight(3),
						((i +1) % 2 == 0 || i == Yacht.CARD_LABELS.Length -1) ? "\r\n" : "\t|\t" // Two columns
					});
				}
				text = text + "Total Score: ".PadRight(12) + ": " + this.GetPlayerScore(Player).ToString() + "\r\n";
			}
			else
			{
			    text = "Player data not available or game not fully initialized.";
			}
			return text;
		}


public const int NUM_CATEGORIES = 13; // Total number of standard scoring categories (excluding bonus)

		// Token: 0x04000003 RID: 3
		// Token: 0x04000003 RID: 3
		public const int INDEX_ONES = 0;
		// Token: 0x04000004 RID: 4
		public const int INDEX_TWOS = 1;
		// Token: 0x04000005 RID: 5
		public const int INDEX_THREES = 2;
		// Token: 0x04000006 RID: 6
		public const int INDEX_FOURS = 3;
		// Token: 0x04000007 RID: 7
		public const int INDEX_FIVES = 4;
		// Token: 0x04000008 RID: 8
		public const int INDEX_SIXES = 5;
		// Token: 0x04000009 RID: 9
		public const int INDEX_TOPBONUS = 6;
		// Token: 0x0400000A RID: 10
		public const int INDEX_3KIND = 7;
		// Token: 0x0400000B RID: 11
		public const int INDEX_4KIND = 8;
		// Token: 0x0400000C RID: 12
		public const int INDEX_FULLHOUSE = 9;
		// Token: 0x0400000D RID: 13
		public const int INDEX_SMLSTRAIGHT = 10;
		// Token: 0x0400000E RID: 14
		public const int INDEX_LGESTRAIGHT = 11;
		// Token: 0x0400000F RID: 15
		public const int INDEX_YACHT = 12;
		// Token: 0x04000010 RID: 16
		public const int INDEX_CHANCE = 13;

		// Token: 0x04000011 RID: 17
		protected const int MAX_PLAYER_LENGTH = 7;

		// Token: 0x04000012 RID: 18
		protected bool[] bDicesHold;

		// Token: 0x04000013 RID: 19
		protected Computer[] oComputers;

		// Token: 0x04000014 RID: 20
		protected bool[] bComputers;

		// Token: 0x04000015 RID: 21
		protected int[] iDicesValue;

		// Token: 0x04000016 RID: 22
		protected int[] iDicesValueSorted;

		// Token: 0x04000017 RID: 23
		protected int[,] iScores;

		// Token: 0x04000018 RID: 24
		protected int iNumberOfPlayers;

		// Token: 0x04000019 RID: 25
		protected int iRollIndex; // 0, 1, 2 for roll 1, 2, 3 in a player's turn

		// Token: 0x0400001A RID: 26
		protected int iPlayerIndex; // Current player index (0 to NumberOfPlayers-1)
		protected int iCurrentTurnNumber; // Current game turn number (1 to 13 for a standard game)

		// Token: 0x0400001B RID: 27
		protected string[] sPlayerNames;

		// Token: 0x0400001C RID: 28
		protected Random oRandom;

		// Token: 0x0400001D RID: 29
		public static string[] CARD_LABELS = new string[]
		{
			"Ones",
			"Twos",
			"Threes",
			"Fours",
			"Fives",
			"Sixes",
			"Bonus",
			"3 of a Kind",
			"4 of a Kind",
			"Full House",
			"S Straight",
			"L Straight",
			"Yacht",
			"Chance"
		};

		// Token: 0x0400001E RID: 30
		public static string[] CARD_ABB = new string[]
		{
			" 1",
			" 2",
			" 3",
			" 4",
			" 5",
			" 6",
			" B",
			"3K",
			"4K",
			"FH",
			"sS",
			"lS",
			" Y",
			" C"
		};

		// Token: 0x0400001F RID: 31
		protected static int[] CARD_DEFAULTS = new int[]
		{
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			0,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1
		};

		// Token: 0x04000020 RID: 32
		protected static int[][] ROLL_DICES = new int[][]
		{
			new int[]
			{
				4,
				5,
				3,
				2
			},
			new int[]
			{
				4,
				1,
				3,
				6
			},
			new int[]
			{
				2,
				1,
				5,
				6
			},
			new int[]
			{
				5,
				1,
				2,
				6
			},
			new int[]
			{
				3,
				1,
				4,
				6
			},
			new int[]
			{
				3,
				5,
				4,
				2
			}
		};

		// Token: 0x02000005 RID: 5
		private enum YachtState
		{
			// Token: 0x04000022 RID: 34
			ROLL1,
			// Token: 0x04000023 RID: 35
			ROLL2,
			// Token: 0x04000024 RID: 36
			STORE,
			// Token: 0x04000025 RID: 37
			NEXTPLAYER
		}
	}
}
