using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.IO;

public static class Path
{
    public static string OutPath;
}
namespace LoadExcel
{
    static class Program
    {
        /// <summary>
        /// 해당 응용 프로그램의 주 진입점입니다.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Path.OutPath = File.ReadAllText("OutPath.txt");             // 출력 경로 지정

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}
