using System;
using System.Collections.Generic;

namespace ComputerYacht
{
	// Token: 0x02000009 RID: 9
	public class ComputerTester
	{
		// Token: 0x06000045 RID: 69 RVA: 0x00003EF7 File Offset: 0x000020F7
		public static int CompareComputers(TransmutableComputer CompFirst, TransmutableComputer CompSecond)
		{
			return CompSecond.iScore - CompFirst.iScore;
		}

		// Token: 0x06000046 RID: 70 RVA: 0x00003F08 File Offset: 0x00002108
		public TransmutableComputer Score(int GamesPerSurvivor, int Bias)
		{
			YachtTest yacht = new YachtTest(this.oRand);
			TransmutableComputer transmutableComputer = null;
			int num = 0;
			foreach (TransmutableComputer transmutableComputer2 in this.loComputers)
			{
				int num2 = transmutableComputer2.Score(yacht, GamesPerSurvivor, Bias);
				if (num2 > num)
				{
					num = num2;
					transmutableComputer = transmutableComputer2;
				}
			}
			if (num > this.iOverallBest)
			{
				this.oOverallBest = transmutableComputer.Clone();
				this.iOverallBest = num;
			}
			return transmutableComputer;
		}

		// Token: 0x06000047 RID: 71 RVA: 0x00003F9C File Offset: 0x0000219C
		public void Add(int Count)
		{
			while (Count > 0)
			{
				this.loComputers.Add(new TransmutableComputer());
				Count--;
			}
		}

		// Token: 0x06000048 RID: 72 RVA: 0x00003FBC File Offset: 0x000021BC
		public void Transmute(int NumToSurvive, int TransmutePerSurvivor)
		{
			List<TransmutableComputer> list = new List<TransmutableComputer>();
			this.loComputers.Sort(new Comparison<TransmutableComputer>(ComputerTester.CompareComputers));
			int num = 0;
			while (num < NumToSurvive && num < this.loComputers.Count)
			{
				list.Add(this.loComputers[num]);
				num++;
			}
			this.loComputers.Clear();
			foreach (TransmutableComputer transmutableComputer in list)
			{
				this.loComputers.Add(transmutableComputer);
				for (int i = 0; i < TransmutePerSurvivor - 1; i++)
				{
					this.loComputers.Add(transmutableComputer.Transmutation(this.oRand));
				}
			}
			if (this.oOverallBest != null)
			{
				this.loComputers.Add(this.oOverallBest.Clone());
			}
			list.Clear();
		}

		// Token: 0x0400006E RID: 110
		public List<TransmutableComputer> loComputers = new List<TransmutableComputer>();

		// Token: 0x0400006F RID: 111
		public TransmutableComputer oOverallBest;

		// Token: 0x04000070 RID: 112
		public int iOverallBest;

		// Token: 0x04000071 RID: 113
		public Random oRand = new Random();
	}
}
