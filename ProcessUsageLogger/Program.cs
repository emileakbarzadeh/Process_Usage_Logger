using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;

namespace ProcessUsageLogger
{
    class Program
    {
        private static Process[] processList = Process.GetProcesses();
        private static TimeSpan[] oldTimes = new TimeSpan[processList.Length];
        static void Main(string[] args)
        {
            int timeSpanMs = 0;
            if (args.Length == 0)
            {
                Console.WriteLine("What interval would you like to log at (in ms)?");
                timeSpanMs = Convert.ToInt32(Console.ReadLine());
            }
            else
            {
                timeSpanMs = Convert.ToInt32(args[0]);
            }
            //Get filename and make file
            string filename = DateTime.Now.ToString("dd-MM-yyyy HH_mm_ss") + ".txt";
            using (var stream = File.CreateText(filename));
            
            while (true)
            {
                //create and open file for writing
                using (StreamWriter sw = File.AppendText(filename))
                {
                    sw.WriteLine(DateTime.Now.ToString());
                    //Run the calculations and get data
                    var procPercentDict = GetProcessorUse(timeSpanMs);
                    foreach (var util in procPercentDict)
                    {
                        if (util.Value != 0)
                        {
                            Console.WriteLine(util.Key + " : " + util.Value);
                            sw.WriteLine(util.Key + " : " + util.Value);
                        }
                    }
                    
                    Console.WriteLine("\n");
                    sw.WriteLine("########################\n\n\n");
                }
                //Wait
                Thread.Sleep(timeSpanMs);
            }
        }
        static List<KeyValuePair<string, double>> GetProcessorUse(int timeSpanMs)
        {
            if(Process.GetProcesses().Length != processList.Length)
            {
                processList = Process.GetProcesses();
                oldTimes = new TimeSpan[processList.Length];
            }
            double[] percentUtil = new double[processList.Length];
            double totalPercent;
            TimeSpan util;
            totalPercent = 0;
            var returnDictionary = new List<KeyValuePair<string, double>>();

            //Run through list of processes
            for (int i = 1; i < processList.Length; i++)
            {
                Process proc = processList[i];
                //Get processor time but catch if it doesnt exist anymore
                try
                {
                    util = proc.TotalProcessorTime;
                }
                catch (System.InvalidOperationException)
                { 
                    util = TimeSpan.Zero;
                }
                percentUtil[i] = 100 * (util - oldTimes[i]).TotalMilliseconds / timeSpanMs;
                totalPercent += percentUtil[i];
                oldTimes[i] = util;

                returnDictionary.Add(new KeyValuePair<string, double>(processList[i].ProcessName, percentUtil[i]));
            }
            returnDictionary.Add(new KeyValuePair<string, double>("total", totalPercent));
            return returnDictionary;
        }
    }
}
