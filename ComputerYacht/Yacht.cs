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
		public bool ComputerNextMove()
		{
			if (this.iPlayerIndex == -1)
			{
				this.iPlayerIndex = 0;
			}
			if (this.iNumberOfPlayers > 0 && this.oComputers.Length > this.iPlayerIndex && this.oComputers[this.iPlayerIndex] != null)
			{
				if (this.iRollIndex <= 2)
				{
					this.RollDices();
					if (this.iRollIndex != 2)
					{
						this.oComputers[this.iPlayerIndex].HoldDice(this, this.iRollIndex);
					}
					this.iRollIndex++;
				}
				if (this.iRollIndex == 3)
				{
					int scoringLocation = this.oComputers[this.iPlayerIndex].GetScoringLocation(this);
					this.ScoreValue(scoringLocation, true);
					this.iPlayerIndex++;
					this.iRollIndex = 0;
					this.ClearHeld();
					if (this.iPlayerIndex >= this.iNumberOfPlayers)
					{
						this.iPlayerIndex = 0;
					}
				}
			}
			return this.IsGameOver(this.iPlayerIndex);
		}

		// Token: 0x06000010 RID: 16 RVA: 0x000023A8 File Offset: 0x000005A8
		public int GetScore(int Player, int Item)
		{
			return this.iScores[Player, Item];
		}

		// Token: 0x06000011 RID: 17 RVA: 0x000023B8 File Offset: 0x000005B8
		public int ScoreValue(int Item, bool Store)
		{
			int num = 0;
			int diceScore = this.GetDiceScore(Item);
			int num2 = 0;
			if (this.iScores[this.iPlayerIndex, 12] > 0 && diceScore > 0 && this.iDicesValueSorted[0] == this.iDicesValueSorted[1] && this.iDicesValueSorted[1] == this.iDicesValueSorted[2] && this.iDicesValueSorted[2] == this.iDicesValueSorted[3] && this.iDicesValueSorted[3] == this.iDicesValueSorted[4])
			{
				if (Store)
				{
					this.iScores[this.iPlayerIndex, 12] += 100;
				}
				num2 += 100;
			}
			if (!Store)
			{
				num = this.iScores[this.iPlayerIndex, Item];
			}
			this.iScores[this.iPlayerIndex, Item] = diceScore;
			num2 += diceScore;
			if (this.iScores[this.iPlayerIndex, 6] == 0)
			{
				int num3 = 0;
				for (int i = 0; i < 6; i++)
				{
					num3 += ((this.iScores[this.iPlayerIndex, i] < 0) ? 0 : this.iScores[this.iPlayerIndex, i]);
				}
				if (num3 >= 63)
				{
					this.iScores[this.iPlayerIndex, 6] = 35;
					num2 += 35;
				}
			}
			if (!Store)
			{
				this.iScores[this.iPlayerIndex, Item] = num;
			}
			return num2;
		}

		// Token: 0x06000012 RID: 18 RVA: 0x00002514 File Offset: 0x00000714
		public void SetupGame(string[] Players, Computer[] Computers)
		{
			this.Reset();
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
		}

		// Token: 0x06000013 RID: 19 RVA: 0x000025C4 File Offset: 0x000007C4
		public int GetPlayerScore(int Player)
		{
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
			case 7:
				if (flag2 || (this.iDicesValueSorted[0] == this.iDicesValueSorted[1] && this.iDicesValueSorted[1] == this.iDicesValueSorted[2]) || (this.iDicesValueSorted[1] == this.iDicesValueSorted[2] && this.iDicesValueSorted[2] == this.iDicesValueSorted[3]) || (this.iDicesValueSorted[2] == this.iDicesValueSorted[3] && this.iDicesValueSorted[3] == this.iDicesValueSorted[4]))
				{
					num = this.iDicesValueSorted[0] + this.iDicesValueSorted[1] + this.iDicesValueSorted[2] + this.iDicesValueSorted[3] + this.iDicesValueSorted[4];
				}
				break;
			case 8:
				if (flag2 || (this.iDicesValueSorted[0] == this.iDicesValueSorted[1] && this.iDicesValueSorted[1] == this.iDicesValueSorted[2] && this.iDicesValueSorted[2] == this.iDicesValueSorted[3]) || (this.iDicesValueSorted[1] == this.iDicesValueSorted[2] && this.iDicesValueSorted[2] == this.iDicesValueSorted[3] && this.iDicesValueSorted[3] == this.iDicesValueSorted[4]))
				{
					num = this.iDicesValueSorted[0] + this.iDicesValueSorted[1] + this.iDicesValueSorted[2] + this.iDicesValueSorted[3] + this.iDicesValueSorted[4];
				}
				break;
			case 9:
				if ((this.iDicesValueSorted[0] == this.iDicesValueSorted[1] && this.iDicesValueSorted[2] == this.iDicesValueSorted[3] && this.iDicesValueSorted[3] == this.iDicesValueSorted[4]) || (this.iDicesValueSorted[0] == this.iDicesValueSorted[1] && this.iDicesValueSorted[1] == this.iDicesValueSorted[2] && this.iDicesValueSorted[3] == this.iDicesValueSorted[4]))
				{
					num = 25;
				}
				break;
			case 10:
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
			case 11:
				if (flag2 || (this.iDicesValueSorted[0] + 1 == this.iDicesValueSorted[1] && this.iDicesValueSorted[1] + 1 == this.iDicesValueSorted[2] && this.iDicesValueSorted[2] + 1 == this.iDicesValueSorted[3] && this.iDicesValueSorted[3] + 1 == this.iDicesValueSorted[4]))
				{
					num = 40;
				}
				break;
			case 12:
				num = (flag ? 50 : 0);
				break;
			case 13:
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
		public int GetRollIndex()
		{
			return this.iRollIndex;
		}

		// Token: 0x06000017 RID: 23 RVA: 0x00002ADC File Offset: 0x00000CDC
		public int CurrentPlayersScore(int index)
		{
			return this.iScores[this.iPlayerIndex, index];
		}

		// Token: 0x06000018 RID: 24 RVA: 0x00002AF0 File Offset: 0x00000CF0
		public int GetDicesValueSorted(int Index)
		{
			return this.iDicesValueSorted[Index];
		}

		// Token: 0x06000019 RID: 25 RVA: 0x00002AFA File Offset: 0x00000CFA
		public int GetDicesValue(int Index)
		{
			return this.iDicesValue[Index];
		}

		// Token: 0x0600001A RID: 26 RVA: 0x00002B04 File Offset: 0x00000D04
		public int GetDicesSum()
		{
			int num = 0;
			for (int i = 0; i < 5; i++)
			{
				num += this.GetDicesValue(i);
			}
			return num;
		}

		// Token: 0x0600001B RID: 27 RVA: 0x00002B2C File Offset: 0x00000D2C
		public int[] GetDiceCounts()
		{
			int[] array = new int[6];
			for (int i = 0; i < 5; i++)
			{
				array[this.GetDicesValue(i) - 1]++;
			}
			return array;
		}

		// Token: 0x0600001C RID: 28 RVA: 0x00002B69 File Offset: 0x00000D69
		protected void Reset()
		{
			this.iNumberOfPlayers = 0;
			this.iRollIndex = 0;
			this.iPlayerIndex = -1;
			this.InitializeDices();
		}

		// Token: 0x0600001D RID: 29 RVA: 0x00002B88 File Offset: 0x00000D88
		protected int GetPlayerBonusScore(int Player)
		{
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
		protected int GetBestScoreIndex()
		{
			int num = 0;
			int num2 = -1;
			for (int i = 0; i < 13; i++)
			{
				if (this.iScores[this.iPlayerIndex, i] == -1)
				{
					int diceScore = this.GetDiceScore(i);
					if (num2 == -1 || diceScore >= num)
					{
						num = diceScore;
						num2 = i;
					}
				}
			}
			if (num2 == -1 || num == 0)
			{
				if (this.iScores[this.iPlayerIndex, 13] == -1)
				{
					num2 = 13;
				}
				else
				{
					num2 = 0;
					for (int j = 0; j < Yacht.CARD_LABELS.Length; j++)
					{
						if (this.iScores[this.iPlayerIndex, j] == -1)
						{
							num2 = j;
							break;
						}
					}
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
		protected bool IsGameOver(int Player)
		{
			for (int i = 0; i < Yacht.CARD_LABELS.Length; i++)
			{
				if (this.iScores[Player, i] == -1)
				{
					return false;
				}
			}
			return true;
		}

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
		protected int iRollIndex;

		// Token: 0x0400001A RID: 26
		protected int iPlayerIndex;

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
