using System;
using System.Text;

namespace ComputerYacht
{
	// Token: 0x02000007 RID: 7
	public class YachtTest : Yacht
	{
		// Token: 0x06000038 RID: 56 RVA: 0x00003A69 File Offset: 0x00001C69
		public YachtTest(Random Rand) : base(Rand)
		{
		}

		// Token: 0x06000039 RID: 57 RVA: 0x00003A74 File Offset: 0x00001C74
		public string PlayerToString(int Player)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("Player " + this.iPlayerIndex.ToString());
			stringBuilder.AppendLine("Roll " + this.iRollIndex.ToString());
			for (int i = 0; i < this.iScores.GetLength(1); i++)
			{
				stringBuilder.Append(Yacht.CARD_ABB[i]);
				stringBuilder.Append('\t');
				if (this.iScores[Player, i] != -1)
				{
					stringBuilder.Append(this.iScores[Player, i]);
				}
				stringBuilder.AppendLine();
			}
			return stringBuilder.ToString();
		}

		// Token: 0x17000004 RID: 4
		// (get) Token: 0x0600003A RID: 58 RVA: 0x00003B1E File Offset: 0x00001D1E
		// (set) Token: 0x0600003B RID: 59 RVA: 0x00003B26 File Offset: 0x00001D26
		public Computer[] Computers
		{
			get
			{
				return this.oComputers;
			}
			set
			{
				this.oComputers = value;
			}
		}

		// Token: 0x0600003C RID: 60 RVA: 0x00003B30 File Offset: 0x00001D30
		public string DicesToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			for (int i = 0; i <= 4; i++)
			{
				stringBuilder.Append(this.iDicesValue[i].ToString());
				stringBuilder.Append('\t');
				if (this.bDicesHold[i])
				{
					stringBuilder.Append("HOLD");
				}
				stringBuilder.AppendLine();
			}
			return stringBuilder.ToString();
		}
	}
}
