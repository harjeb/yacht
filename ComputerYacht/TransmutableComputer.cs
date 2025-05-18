using System;
using System.Text;

namespace ComputerYacht
{
	// Token: 0x02000008 RID: 8
	public class TransmutableComputer : Computer
	{
		// Token: 0x0600003E RID: 62 RVA: 0x00003B9C File Offset: 0x00001D9C
		public void Assign(TransmutableComputer Computer)
		{
			this.Take0s = (int[])Computer.Take0s.Clone();
			this.INDEX_STORAGE_LOCATION_WEIGHTS = (int[])Computer.INDEX_STORAGE_LOCATION_WEIGHTS.Clone();
			for (int i = 0; i < this.SCOREINDEX_ATTAIN_WEIGHTING.Length; i++)
			{
				this.SCOREINDEX_ATTAIN_WEIGHTING[i] = (int[])Computer.SCOREINDEX_ATTAIN_WEIGHTING[i].Clone();
			}
		}

		// Token: 0x0600003F RID: 63 RVA: 0x00003C04 File Offset: 0x00001E04
		protected void AdjustValue(ref int Value, int Min, int Max, int By, Random Random)
		{
			if (Random.Next(1000) > 499)
			{
				Value += Random.Next(By) + 1;
			}
			else
			{
				Value -= Random.Next(By) + 1;
			}
			if (Value > Max)
			{
				Value = Max;
			}
			if (Value < Min)
			{
				Value = Min;
			}
		}

		// Token: 0x06000040 RID: 64 RVA: 0x00003C58 File Offset: 0x00001E58
		public string TransmutableSettingsToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (int num in this.Take0s)
			{
				stringBuilder.Append(num.ToString() + ",");
			}
			stringBuilder.AppendLine();
			foreach (int num2 in this.INDEX_STORAGE_LOCATION_WEIGHTS)
			{
				stringBuilder.Append(num2.ToString() + ",");
			}
			stringBuilder.AppendLine();
			stringBuilder.AppendLine();
			foreach (int[] array in this.SCOREINDEX_ATTAIN_WEIGHTING)
			{
				foreach (int num3 in array)
				{
					stringBuilder.Append(num3.ToString() + ",");
				}
				stringBuilder.AppendLine();
			}
			return stringBuilder.ToString();
		}

		// Token: 0x06000041 RID: 65 RVA: 0x00003D58 File Offset: 0x00001F58
		public void Transmute(Random Rand)
		{
			for (int i = 0; i < 5 + Rand.Next(45); i++)
			{
				this.AdjustValue(ref this.SCOREINDEX_ATTAIN_WEIGHTING[Rand.Next(this.SCOREINDEX_ATTAIN_WEIGHTING.Length)][Rand.Next(this.SCOREINDEX_ATTAIN_WEIGHTING[0].Length)], 0, 50, 2, Rand);
				this.AdjustValue(ref this.INDEX_STORAGE_LOCATION_WEIGHTS[Rand.Next(this.INDEX_STORAGE_LOCATION_WEIGHTS.Length)], 0, 30, 2, Rand);
			}
			for (int j = 0; j < Rand.Next(5); j++)
			{
				int num = Rand.Next(this.Take0s.Length);
				int num2 = Rand.Next(this.Take0s.Length);
				if (num != num2)
				{
					int num3 = this.Take0s[num];
					this.Take0s[num] = this.Take0s[num2];
					this.Take0s[num2] = num3;
				}
			}
		}

		// Token: 0x06000042 RID: 66 RVA: 0x00003E30 File Offset: 0x00002030
		public int Score(YachtTest Yacht, int Games, int Bias)
		{
			Yacht.Computers[0] = this;
			this.iScore = 0;
			int i = 0;
			while (i < Games)
			{
				Yacht.SetupGame(new string[]
				{
					"Computer"
				}, new Computer[]
				{
					this
				});
				while (!Yacht.ComputerNextMove())
				{
				}
				i++;
				int playerScore = Yacht.GetPlayerScore(0);
				if (playerScore > Bias)
				{
					this.iScore += playerScore;
				}
			}
			this.iScore /= i + 1;
			return this.iScore;
		}

		// Token: 0x06000043 RID: 67 RVA: 0x00003EB8 File Offset: 0x000020B8
		public TransmutableComputer Transmutation(Random Rand)
		{
			TransmutableComputer transmutableComputer = new TransmutableComputer();
			transmutableComputer.Assign(this);
			transmutableComputer.Transmute(Rand);
			return transmutableComputer;
		}

		// Token: 0x06000044 RID: 68 RVA: 0x00003EDC File Offset: 0x000020DC
		public TransmutableComputer Clone()
		{
			TransmutableComputer transmutableComputer = new TransmutableComputer();
			transmutableComputer.Assign(this);
			return transmutableComputer;
		}

		// Token: 0x0400006D RID: 109
		public int iScore;
	}
}
