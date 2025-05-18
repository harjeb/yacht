using System;

namespace ComputerYacht
{
	// Token: 0x02000003 RID: 3
	public static class Dice
	{
		// Token: 0x06000005 RID: 5 RVA: 0x000020A0 File Offset: 0x000002A0
		private static int[] CreateDice(int Dice)
		{
			int[] array = new int[Dice];
			for (int i = 0; i < Dice; i++)
			{
				array[i] = 1;
			}
			return array;
		}

		// Token: 0x06000006 RID: 6 RVA: 0x000020C8 File Offset: 0x000002C8
		private static bool IncrementDice(ref int[] Dice)
		{
			int num = Dice.Length - 1;
			Dice[num]++;
			while (num >= 0 && Dice[num] == 7)
			{
				Dice[num] = 1;
				num--;
				if (num != -1)
				{
					Dice[num]++;
				}
			}
			return num == -1;
		}

		// Token: 0x06000007 RID: 7 RVA: 0x00002124 File Offset: 0x00000324
		private static bool DiceMatchCounts(ref int[] Dice, ref int[] Counts)
		{
			int[] array = new int[Counts.Length];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = 0;
			}
			for (int j = 0; j < Dice.Length; j++)
			{
				array[Dice[j] - 1]++;
			}
			for (int k = 0; k < Counts.Length; k++)
			{
				if (array[k] < Counts[k])
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06000008 RID: 8 RVA: 0x00002190 File Offset: 0x00000390
		public static double CalculateDiceChances(int Dice, int[] Counts)
		{
			int num = 0;
			int num2 = 0;
			int[] array = Dice.CreateDice(Dice);
			do
			{
				num2++;
				if (Dice.DiceMatchCounts(ref array, ref Counts))
				{
					num++;
				}
			}
			while (!Dice.IncrementDice(ref array));
			return 1.0 * (double)num / (double)num2;
		}
	}
}
