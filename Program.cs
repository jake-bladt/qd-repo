using System;
using System.Diagnostics;
using System.IO;

namespace execit
{
    class Program
    {
        const string DQUOTE = "\"";

        static void Main(string[] args)
        {
            var startDate = new DateTime(2014, 2, 8);
            var endDate = new DateTime(2021, 8, 1);
            
            var currDate = startDate;
            var lineIndex = 0;
            var lines = new String[100000];
            var rnd = new Random();

            while(currDate < endDate)
            {
                var iters = 1 + rnd.NextDouble() * 4;
                var steps = Convert.ToInt32(1999 + rnd.NextDouble() * 3999);
                var currDs = currDate.ToString("yyyyMMdd");

                for (int i = 0; i < iters; i++)
                {
                    var hrs = rnd.NextDouble() * 4;
                    var mins = rnd.NextDouble() * 59;
                    currDate = currDate.AddHours(hrs);
                    currDate = currDate.AddMinutes(mins);

                    steps += Convert.ToInt32(rnd.NextDouble() * 2999);
                    var echoLine = $"echo {currDs}:{steps} >> tdata.txt";
                    lines[lineIndex++] = echoLine;

                    var commitDate = currDate.ToString("ddd MMM dd HH:mm yyyy");
                    var commitMessage = $"Data for {currDs}";
                    var commitLine = $"git commit --allow-empty --date={DQUOTE}{commitDate} + " +
                        $"0100{DQUOTE} -m {DQUOTE}{commitMessage}{DQUOTE}";
                    lines[lineIndex++] = "git add -A";
                    lines[lineIndex++] = commitLine;
                }

                var aIdx = 0;

                using(StreamWriter batFile = new StreamWriter("ef.bat"))
                {
                    while (!String.IsNullOrEmpty(lines[aIdx]))
                    {
                        batFile.WriteLine(lines[aIdx]);
                        aIdx++;
                    }
                }

            }


        }

        public static void ExecuteCommand(string Command)
        {
            ProcessStartInfo ProcessInfo;
            Process Process;

            ProcessInfo = new ProcessStartInfo("cmd.exe", "/K " + Command);
            ProcessInfo.CreateNoWindow = true;
            ProcessInfo.UseShellExecute = true;

            Process = Process.Start(ProcessInfo);
        }
    }
}
