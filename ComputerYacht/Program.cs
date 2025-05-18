using System;
using System.Windows.Forms;

namespace ComputerYacht
{
	// Token: 0x0200000A RID: 10
	internal static class Program
	{
		// Token: 0x0600004A RID: 74 RVA: 0x000040CE File Offset: 0x000022CE
		[STAThread]
		private static void Main()
		{
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			Application.Run(new frmMain());
		}
	}
}
