using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace ComputerYacht
{
	// Token: 0x0200000C RID: 12
	public partial class frmMain : Form
	{
		// Token: 0x0600004E RID: 78 RVA: 0x0000410C File Offset: 0x0000230C
		public frmMain()
		{
			this.InitializeComponent();
		}

		// Token: 0x0600004F RID: 79 RVA: 0x00004164 File Offset: 0x00002364
		private void frmMain_Load(object sender, EventArgs e)
		{
			this.loComp.Add(8);
			this.ResetYacht(this.yYacht);
			this.tmrMain.Enabled = true;
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
				new Computer()
			});
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
				this.ResetYacht(this.yYacht);
			}
		}

		// Token: 0x06000055 RID: 85 RVA: 0x00004570 File Offset: 0x00002770
		private void tmrMain_Tick(object sender, EventArgs e)
		{
			for (int i = 0; i < this.iMovesPerGame; i++)
			{
				this.Next();
			}
			this.tbDices.Text = this.yYacht.DicesToString();
			this.tbScores.Text = this.yYacht.PlayerToString(0);
		}

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
		private void btnPause_Click(object sender, EventArgs e)
		{
			if (this.iMovesPerGame == 10000)
			{
				this.iMovesPerGame = 1;
				this.tmrMain.Interval = 1500;
				return;
			}
			this.iMovesPerGame = 10000;
			this.tmrMain.Interval = 1;
		}

		// Token: 0x04000073 RID: 115
		private ComputerTester loComp = new ComputerTester();

		// Token: 0x04000074 RID: 116
		private YachtTest yYacht = new YachtTest(new Random());

		// Token: 0x04000075 RID: 117
		private int iMovesPerGame = 10000;

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
