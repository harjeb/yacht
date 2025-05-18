using System;

namespace ComputerYacht
{
	// Token: 0x02000006 RID: 6
	public class Computer
	{
		// Token: 0x06000024 RID: 36 RVA: 0x0000322B File Offset: 0x0000142B
		public bool HoldDice(Yacht yYacht, int RollIndex)
		{
			return this.HoldDiceForScore(yYacht, this.CalculateBestScoreItem(yYacht, RollIndex)) && RollIndex != 2 && !this.AllHeld(yYacht);
		}

		// Token: 0x06000025 RID: 37 RVA: 0x0000324E File Offset: 0x0000144E
		public int GetScoringLocation(Yacht yYacht)
		{
			return this.GetBestScoreIndex(yYacht);
		}

		// Token: 0x06000026 RID: 38 RVA: 0x00003258 File Offset: 0x00001458
		protected bool ViableBonusScore(Yacht yYacht, int Score, int Index)
		{
			int num = 0;
			int num2 = Score;
			if (Score == 0)
			{
				return false;
			}
			for (int i = 0; i <= 5; i++)
			{
				if (yYacht.CurrentPlayersScore(i) == -1 && i != Index)
				{
					num++;
				}
			}
			if (num > 2 && Score / (Index + 1) >= 3)
			{
				return true;
			}
			for (int j = 0; j <= 5; j++)
			{
				int num3 = yYacht.CurrentPlayersScore(j);
				if (yYacht.CurrentPlayersScore(j) == -1)
				{
					if (j != Index)
					{
						num2 += (j + 1) * 7 / 2;
					}
				}
				else
				{
					num2 += num3;
				}
			}
			return num2 >= 63;
		}

		// Token: 0x06000027 RID: 39 RVA: 0x000032E0 File Offset: 0x000014E0
		protected int GetDiceScoreBalance(Yacht yYacht, int Index)
		{
			switch (Index)
			{
			case 0:
			case 1:
			case 2:
			case 3:
			case 4:
			case 5:
				return 30 - ((Index + 1) * 3 - yYacht.ScoreValue(Index, false));
			default:
				return yYacht.ScoreValue(Index, false);
			}
		}

		// Token: 0x06000028 RID: 40 RVA: 0x00003328 File Offset: 0x00001528
		protected int GetBestScoreIndex(Yacht yYacht)
		{
			int num = 0;
			int num2 = -1;
			for (int i = 0; i < 13; i++)
			{
				int diceScoreBalance = this.GetDiceScoreBalance(yYacht, i);
				int num3 = diceScoreBalance * this.INDEX_STORAGE_LOCATION_WEIGHTS[i];
				if (yYacht.CurrentPlayersScore(i) == -1 && (num2 == -1 || num3 >= num) && (i < 0 || i > 5 || this.ViableBonusScore(yYacht, yYacht.GetDiceScore(i), i)))
				{
					num = num3;
					num2 = i;
				}
			}
			if (num2 == -1 || num == 0)
			{
				num = 0;
				num2 = -1;
				for (int j = 6; j < 13; j++)
				{
					int num3 = yYacht.GetDiceScore(j);
					if (yYacht.CurrentPlayersScore(j) == -1 && (num2 == -1 || num3 >= num))
					{
						num = num3;
						num2 = j;
					}
				}
			}
			if (num2 == -1 || num == 0)
			{
				num2 = 0;
				for (int k = 0; k < this.Take0s.Length; k++)
				{
					if (yYacht.CurrentPlayersScore(this.Take0s[k]) == -1)
					{
						num2 = this.Take0s[k];
						break;
					}
				}
			}
			return num2;
		}

		// Token: 0x06000029 RID: 41 RVA: 0x00003418 File Offset: 0x00001618
		protected int TotalOf(int[] Ints)
		{
			int num = 0;
			for (int i = 0; i < Ints.Length; i++)
			{
				num += Ints[i];
			}
			return num;
		}

		// Token: 0x0600002A RID: 42 RVA: 0x0000343C File Offset: 0x0000163C
		protected long CalculateChance(Yacht yYacht, int[] Dice)
		{
			int num = this.HoldSpecificDice(yYacht, Dice, null, false);
			int num2 = 5 - this.TotalOf(Dice);
			if (num != 0)
			{
				return (long)(num2 * 100 / num);
			}
			return 65535L;
		}

		// Token: 0x0600002B RID: 43 RVA: 0x00003470 File Offset: 0x00001670
		protected long CalculateChance(Yacht yYacht, int RollIndex, int ScoreIndex)
		{
			int[] scoreIndexRequiredDice = this.GetScoreIndexRequiredDice(yYacht, ScoreIndex);
			return this.CalculateChance(yYacht, scoreIndexRequiredDice) * (long)this.SCOREINDEX_ATTAIN_WEIGHTING[RollIndex % 2][ScoreIndex];
		}

		// Token: 0x0600002C RID: 44 RVA: 0x0000349C File Offset: 0x0000169C
		protected int[] GetScoreIndexRequiredDice(Yacht yYacht, int ScoreIndex)
		{
			int[] array = new int[6];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = 0;
			}
			if (ScoreIndex == -1)
			{
				return array;
			}
			int num = this.ScoreIndexToGameIndex(ScoreIndex);
			switch (num)
			{
			case 0:
			case 1:
			case 2:
			case 3:
			case 4:
			case 5:
				array[num] = 3;
				break;
			case 7:
				array[ScoreIndex - 6] = 3;
				break;
			case 8:
				array[ScoreIndex - 12] = 4;
				break;
			case 9:
				array[(ScoreIndex - 18) % 6] = 2;
				array[(ScoreIndex - 18) / 6] += 3;
				break;
			case 10:
			{
				int i = ScoreIndex - 54;
				int num2 = i + 3;
				do
				{
					array[i] = 1;
					i++;
				}
				while (i <= num2);
				break;
			}
			case 11:
			{
				int i = ScoreIndex - 57;
				int num2 = i + 4;
				do
				{
					array[i] = 1;
					i++;
				}
				while (i <= num2);
				break;
			}
			case 12:
				array[ScoreIndex - 59] = 5;
				break;
			}
			return array;
		}

		// Token: 0x0600002D RID: 45 RVA: 0x00003585 File Offset: 0x00001785
		protected int ScoreIndexToGameIndex(int ScoreIndex)
		{
			return Computer.SCOREINDEX_INDEXES[ScoreIndex];
		}

		// Token: 0x0600002E RID: 46 RVA: 0x00003590 File Offset: 0x00001790
		protected void CalcHighValues(Yacht yYacht, ref int iHighCount, ref int iHighValue)
		{
			int[] diceCounts = yYacht.GetDiceCounts();
			iHighCount = 0;
			iHighValue = 0;
			for (int i = 0; i < 6; i++)
			{
				if (diceCounts[i] > iHighCount)
				{
					iHighCount = diceCounts[i];
					iHighValue = i + 1;
				}
			}
		}

		// Token: 0x0600002F RID: 47 RVA: 0x000035C8 File Offset: 0x000017C8
		protected double CalculateExactChance(Yacht yYacht, int ScoreIndex)
		{
			int[] scoreIndexRequiredDice = this.GetScoreIndexRequiredDice(yYacht, ScoreIndex);
			int num = this.TotalOf(scoreIndexRequiredDice);
			this.HoldSpecificDice(yYacht, scoreIndexRequiredDice, null, false);
			int num2 = 5 - (num - this.TotalOf(scoreIndexRequiredDice));
			if (num2 == 0)
			{
				return 1.0;
			}
			return Dice.CalculateDiceChances(num2, scoreIndexRequiredDice);
		}

		// Token: 0x06000030 RID: 48 RVA: 0x00003614 File Offset: 0x00001814
		protected int CalculateBestScoreItem(Yacht yYacht, int RollsLeft)
		{
			int num = -1;
			long num2 = 0L;
			int num3 = -1;
			double num4 = 0.0;
			if (yYacht.CurrentPlayersScore(12) != 0)
			{
				int num5 = 0;
				int num6 = 0;
				this.CalcHighValues(yYacht, ref num5, ref num6);
				if (num5 >= 3)
				{
					num3 = num6 + 59 - 1;
					num4 = 1.0 / Math.Pow(6.0, (double)(5 - num5));
				}
			}
			for (int i = 0; i <= 64; i++)
			{
				if (yYacht.CurrentPlayersScore(this.ScoreIndexToGameIndex(i)) == -1)
				{
					long num7 = this.CalculateChance(yYacht, RollsLeft, i);
					if (num == -1 || num7 >= num2)
					{
						num = i;
						num2 = num7;
					}
				}
			}
			if (num3 != -1 && num3 != num && this.CalculateExactChance(yYacht, num) < num4)
			{
				num = num3;
			}
			return num;
		}

		// Token: 0x06000031 RID: 49 RVA: 0x000036D0 File Offset: 0x000018D0
		protected bool CanBetterScore(Yacht yYacht, int ScoreIndex)
		{
			int[] diceCounts = yYacht.GetDiceCounts();
			int num = this.ScoreIndexToGameIndex(ScoreIndex);
			int dicesSum = yYacht.GetDicesSum();
			switch (num)
			{
			case 0:
			case 1:
			case 2:
			case 3:
			case 4:
			case 5:
				return diceCounts[num] != 5;
			default:
				return true;
			case 7:
				return dicesSum - (ScoreIndex - 6 + 1) != 12;
			case 8:
				return dicesSum - (ScoreIndex - 6 + 1) != 6;
			case 9:
			case 11:
			case 12:
				return false;
			case 10:
				return yYacht.CurrentPlayersScore(11) == -1;
			}
		}

		// Token: 0x06000032 RID: 50 RVA: 0x00003768 File Offset: 0x00001968
		protected int[] GetScoreIndexImprovementDice(int[] StdDice, int ScoreIndex)
		{
			int[] array = new int[6];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = 0;
			}
			if (ScoreIndex == -1)
			{
				return array;
			}
			int num = this.ScoreIndexToGameIndex(ScoreIndex);
			switch (num)
			{
			case 0:
			case 1:
			case 2:
			case 3:
			case 4:
			case 5:
				array[num] = 5 - StdDice[num];
				break;
			case 7:
				array[ScoreIndex - 6] = 2;
				break;
			case 8:
				array[ScoreIndex - 12] = 1;
				break;
			}
			return array;
		}

		// Token: 0x06000033 RID: 51 RVA: 0x000037F8 File Offset: 0x000019F8
		protected bool HoldDiceForScore(Yacht yYacht, int ScoreIndex)
		{
			int[] scoreIndexRequiredDice = this.GetScoreIndexRequiredDice(yYacht, ScoreIndex);
			int[] scoreIndexImprovementDice = this.GetScoreIndexImprovementDice(scoreIndexRequiredDice, ScoreIndex);
			int num = this.HoldSpecificDice(yYacht, scoreIndexRequiredDice, scoreIndexImprovementDice, true);
			if (ScoreIndex != -1 && num == 0 && this.CanBetterScore(yYacht, ScoreIndex))
			{
				num = 1;
			}
			return num != 0;
		}

		// Token: 0x06000034 RID: 52 RVA: 0x0000383C File Offset: 0x00001A3C
		protected bool AllHeld(Yacht yYacht)
		{
			for (int i = 0; i < 5; i++)
			{
				if (!yYacht.DicesHold[i])
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06000035 RID: 53 RVA: 0x00003864 File Offset: 0x00001A64
		protected int DicesHeld(Yacht yYacht)
		{
			int num = 0;
			for (int i = 0; i < 5; i++)
			{
				if (yYacht.DicesHold[i])
				{
					num++;
				}
			}
			return num;
		}

		// Token: 0x06000036 RID: 54 RVA: 0x00003890 File Offset: 0x00001A90
		protected int HoldSpecificDice(Yacht yYacht, int[] Dice, int[] ExtraDice, bool Hold)
		{
			int num = 0;
			for (int i = 0; i < 5; i++)
			{
				if (Hold)
				{
					yYacht.DicesHold[i] = false;
				}
				int num2 = yYacht.GetDicesValue(i) - 1;
				if (Dice[num2] > 0)
				{
					if (Hold)
					{
						yYacht.DicesHold[i] = true;
					}
					Dice[num2]--;
				}
			}
			for (int i = 0; i < Dice.Length; i++)
			{
				num += Dice[i];
			}
			if (Hold && num == 0)
			{
				for (int i = 0; i < 5; i++)
				{
					if (!yYacht.DicesHold[i])
					{
						int num2 = yYacht.GetDicesValue(i) - 1;
						if (ExtraDice[num2] > 0)
						{
							yYacht.DicesHold[i] = true;
							ExtraDice[num2]--;
						}
					}
				}
			}
			return num;
		}

		// Token: 0x04000026 RID: 38
		protected const int COMP_INDEX_ONES = 0;

		// Token: 0x04000027 RID: 39
		protected const int COMP_INDEX_TWOS = 1;

		// Token: 0x04000028 RID: 40
		protected const int COMP_INDEX_THREES = 2;

		// Token: 0x04000029 RID: 41
		protected const int COMP_INDEX_FOURS = 3;

		// Token: 0x0400002A RID: 42
		protected const int COMP_INDEX_FIVES = 4;

		// Token: 0x0400002B RID: 43
		protected const int COMP_INDEX_SIXES = 5;

		// Token: 0x0400002C RID: 44
		protected const int COMP_INDEX_3KIND1 = 6;

		// Token: 0x0400002D RID: 45
		protected const int COMP_INDEX_3KIND2 = 7;

		// Token: 0x0400002E RID: 46
		protected const int COMP_INDEX_3KIND3 = 8;

		// Token: 0x0400002F RID: 47
		protected const int COMP_INDEX_3KIND4 = 9;

		// Token: 0x04000030 RID: 48
		protected const int COMP_INDEX_3KIND5 = 10;

		// Token: 0x04000031 RID: 49
		protected const int COMP_INDEX_3KIND6 = 11;

		// Token: 0x04000032 RID: 50
		protected const int COMP_INDEX_4KIND1 = 12;

		// Token: 0x04000033 RID: 51
		protected const int COMP_INDEX_4KIND2 = 13;

		// Token: 0x04000034 RID: 52
		protected const int COMP_INDEX_4KIND3 = 14;

		// Token: 0x04000035 RID: 53
		protected const int COMP_INDEX_4KIND4 = 15;

		// Token: 0x04000036 RID: 54
		protected const int COMP_INDEX_4KIND5 = 16;

		// Token: 0x04000037 RID: 55
		protected const int COMP_INDEX_4KIND6 = 17;

		// Token: 0x04000038 RID: 56
		protected const int COMP_INDEX_FULLHOUSE11 = 18;

		// Token: 0x04000039 RID: 57
		protected const int COMP_INDEX_FULLHOUSE12 = 19;

		// Token: 0x0400003A RID: 58
		protected const int COMP_INDEX_FULLHOUSE13 = 20;

		// Token: 0x0400003B RID: 59
		protected const int COMP_INDEX_FULLHOUSE14 = 21;

		// Token: 0x0400003C RID: 60
		protected const int COMP_INDEX_FULLHOUSE15 = 22;

		// Token: 0x0400003D RID: 61
		protected const int COMP_INDEX_FULLHOUSE16 = 23;

		// Token: 0x0400003E RID: 62
		protected const int COMP_INDEX_FULLHOUSE21 = 24;

		// Token: 0x0400003F RID: 63
		protected const int COMP_INDEX_FULLHOUSE22 = 25;

		// Token: 0x04000040 RID: 64
		protected const int COMP_INDEX_FULLHOUSE23 = 26;

		// Token: 0x04000041 RID: 65
		protected const int COMP_INDEX_FULLHOUSE24 = 27;

		// Token: 0x04000042 RID: 66
		protected const int COMP_INDEX_FULLHOUSE25 = 28;

		// Token: 0x04000043 RID: 67
		protected const int COMP_INDEX_FULLHOUSE26 = 29;

		// Token: 0x04000044 RID: 68
		protected const int COMP_INDEX_FULLHOUSE31 = 30;

		// Token: 0x04000045 RID: 69
		protected const int COMP_INDEX_FULLHOUSE32 = 31;

		// Token: 0x04000046 RID: 70
		protected const int COMP_INDEX_FULLHOUSE33 = 32;

		// Token: 0x04000047 RID: 71
		protected const int COMP_INDEX_FULLHOUSE34 = 33;

		// Token: 0x04000048 RID: 72
		protected const int COMP_INDEX_FULLHOUSE35 = 34;

		// Token: 0x04000049 RID: 73
		protected const int COMP_INDEX_FULLHOUSE36 = 35;

		// Token: 0x0400004A RID: 74
		protected const int COMP_INDEX_FULLHOUSE41 = 36;

		// Token: 0x0400004B RID: 75
		protected const int COMP_INDEX_FULLHOUSE42 = 37;

		// Token: 0x0400004C RID: 76
		protected const int COMP_INDEX_FULLHOUSE43 = 38;

		// Token: 0x0400004D RID: 77
		protected const int COMP_INDEX_FULLHOUSE44 = 39;

		// Token: 0x0400004E RID: 78
		protected const int COMP_INDEX_FULLHOUSE45 = 40;

		// Token: 0x0400004F RID: 79
		protected const int COMP_INDEX_FULLHOUSE46 = 41;

		// Token: 0x04000050 RID: 80
		protected const int COMP_INDEX_FULLHOUSE51 = 42;

		// Token: 0x04000051 RID: 81
		protected const int COMP_INDEX_FULLHOUSE52 = 43;

		// Token: 0x04000052 RID: 82
		protected const int COMP_INDEX_FULLHOUSE53 = 44;

		// Token: 0x04000053 RID: 83
		protected const int COMP_INDEX_FULLHOUSE54 = 45;

		// Token: 0x04000054 RID: 84
		protected const int COMP_INDEX_FULLHOUSE55 = 46;

		// Token: 0x04000055 RID: 85
		protected const int COMP_INDEX_FULLHOUSE56 = 47;

		// Token: 0x04000056 RID: 86
		protected const int COMP_INDEX_FULLHOUSE61 = 48;

		// Token: 0x04000057 RID: 87
		protected const int COMP_INDEX_FULLHOUSE62 = 49;

		// Token: 0x04000058 RID: 88
		protected const int COMP_INDEX_FULLHOUSE63 = 50;

		// Token: 0x04000059 RID: 89
		protected const int COMP_INDEX_FULLHOUSE64 = 51;

		// Token: 0x0400005A RID: 90
		protected const int COMP_INDEX_FULLHOUSE65 = 52;

		// Token: 0x0400005B RID: 91
		protected const int COMP_INDEX_FULLHOUSE66 = 53;

		// Token: 0x0400005C RID: 92
		protected const int COMP_INDEX_SMALLSTRAIGHT1 = 54;

		// Token: 0x0400005D RID: 93
		protected const int COMP_INDEX_SMALLSTRAIGHT2 = 55;

		// Token: 0x0400005E RID: 94
		protected const int COMP_INDEX_SMALLSTRAIGHT3 = 56;

		// Token: 0x0400005F RID: 95
		protected const int COMP_INDEX_LARGESTRAIGHT1 = 57;

		// Token: 0x04000060 RID: 96
		protected const int COMP_INDEX_LARGESTRAIGHT2 = 58;

		// Token: 0x04000061 RID: 97
		protected const int COMP_INDEX_YACHT1 = 59;

		// Token: 0x04000062 RID: 98
		protected const int COMP_INDEX_YACHT2 = 60;

		// Token: 0x04000063 RID: 99
		protected const int COMP_INDEX_YACHT3 = 61;

		// Token: 0x04000064 RID: 100
		protected const int COMP_INDEX_YACHT4 = 62;

		// Token: 0x04000065 RID: 101
		protected const int COMP_INDEX_YACHT5 = 63;

		// Token: 0x04000066 RID: 102
		protected const int COMP_INDEX_YACHT6 = 64;

		// Token: 0x04000067 RID: 103
		protected const int COMP_INDEX_CHANCE = 65;

		// Token: 0x04000068 RID: 104
		protected const int NUM_COMP_INDEXES = 66;

		// Token: 0x04000069 RID: 105
		protected static int[] SCOREINDEX_INDEXES = new int[]
		{
			0,
			1,
			2,
			3,
			4,
			5,
			7,
			7,
			7,
			7,
			7,
			7,
			8,
			8,
			8,
			8,
			8,
			8,
			9,
			9,
			9,
			9,
			9,
			9,
			9,
			9,
			9,
			9,
			9,
			9,
			9,
			9,
			9,
			9,
			9,
			9,
			9,
			9,
			9,
			9,
			9,
			9,
			9,
			9,
			9,
			9,
			9,
			9,
			9,
			9,
			9,
			9,
			9,
			9,
			10,
			10,
			10,
			11,
			11,
			12,
			12,
			12,
			12,
			12,
			12,
			13
		};

		// Token: 0x0400006A RID: 106
		protected int[] Take0s = new int[]
		{
			13,
			0,
			12,
			10,
			8,
			5,
			3,
			2,
			11,
			1,
			7,
			9,
			4
		};

		// Token: 0x0400006B RID: 107
		protected int[] INDEX_STORAGE_LOCATION_WEIGHTS = new int[]
		{
			2,
			2,
			2,
			2,
			2,
			2,
			2,
			0,
			1,
			6,
			2,
			8,
			15,
			14
		};

		// Token: 0x0400006C RID: 108
		protected int[][] SCOREINDEX_ATTAIN_WEIGHTING = new int[][]
		{
			new int[]
			{
				23,
				24,
				25,
				26,
				27,
				28,
				0,
				0,
				5,
				13,
				20,
				18,
				0,
				0,
				4,
				18,
				24,
				28,
				20,
				10,
				10,
				10,
				10,
				10,
				10,
				20,
				10,
				10,
				15,
				10,
				12,
				10,
				20,
				10,
				10,
				14,
				10,
				10,
				10,
				20,
				6,
				10,
				10,
				10,
				10,
				10,
				20,
				10,
				10,
				10,
				5,
				10,
				10,
				25,
				12,
				12,
				12,
				16,
				16,
				20,
				20,
				20,
				20,
				20,
				20,
				0
			},
			new int[]
			{
				23,
				24,
				25,
				26,
				27,
				23,
				0,
				0,
				0,
				17,
				20,
				18,
				1,
				0,
				4,
				18,
				24,
				28,
				20,
				15,
				10,
				13,
				10,
				10,
				15,
				20,
				10,
				10,
				15,
				10,
				11,
				10,
				25,
				10,
				10,
				10,
				10,
				6,
				10,
				20,
				10,
				10,
				10,
				10,
				10,
				10,
				20,
				15,
				10,
				10,
				10,
				10,
				5,
				20,
				11,
				12,
				12,
				16,
				16,
				20,
				20,
				25,
				21,
				20,
				25,
				0
			}
		};
	}
}
