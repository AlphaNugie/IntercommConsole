using System;
using System.Collections.Generic;
using CommonLib.Clients;
using System.Threading;
using IntercommConsole.Tasks;
using IntercommConsole.Core;
using IntercommConsole.Model;
using Newtonsoft.Json;
using CommonLib.Clients.Tasks;
using System.Linq;
using CommonLib.Function;
//using Operation_server;
using ConnectServerWrapper;
using System.Net;

namespace IntercommConsole
{
    public class Program
    {
        static void Main(string[] args)
        {
            Config.Init();
            DbDef.Update();
            #region test
            //string ipStr = "127.0.0.1";
            //IPAddress ip;
            //IPAddress.TryParse(ipStr, out ip);
            //ipStr = ip.ToString();
            //return;

            //KalmanFilter _kalmanFilter = new KalmanFilter(0.05, 0.95);
            ////double value = 0;
            //double result = 0;
            //List<double> values = new List<double>() { 1.7, 2.9, 4.5, 3, 2, 1, 1.1, 1.7, 1.9, 2.3 };
            //foreach (var value in values)
            //{
            //    double v = value;
            //    _kalmanFilter.SetValue(ref v, 0);
            //    result = v;
            //}

            //string result = string.Format("{0},{1:f3},{2:f3},{3:f3}", Config.MachineName, 444, 555, 4);
            //string result1 = HexHelper.GetStringSumResult(result);
            //result += ',';
            //string result2 = result + result.ToCharArray().Sum(c => (int)c);
            //bool b = HexHelper.IsStringSumVerified(result2);
            //b = HexHelper.IsStringSumVerified(result1);
            //return;

            //string input = "testy";
            //int sum = input.ToCharArray().Sum(c => (int)c);
            //string result = HexHelper.GetStringSumResult(input);
            //bool verified = HexHelper.IsStringSumVerified(result);
            #endregion
            Const.WriteConsoleLog("IntercommConsole启动，本地IP: " + Const.LocalIp);
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(UnhandledException_Raising); //未捕获异常触发事件
            //任务
            List<Task> tasks = new List<Task>() {
                new DataProcessTask(),
                //new BeyondStackTask() { Interval = 500 },
                new OpcTask() { Interval = Config.OpcLoopInterval },
                new OpcCommTask() { Interval = Config.OpcLoopInterval * 60 },
                new ModelBuildingServiceTask(),
                //new ModelDisplayServiceTask(),
                new ModelWebDisplayServiceTask(),
                new StrategyServiceTask() { Interval = 100 },
                //new DbOracleTask(),
                //new DbOracleRapidTask() { Interval = 50 },
                new DbSqliteTask(),
                //new PostureTask(),
                //new AngleRecordTask(),
                new InternalCommTask() { Interval = 100 },
                ////new ModelAngleRecordTask(),
                //new AntiCollRecordTask(),
            };
            //添加RCMS发送任务
            //tasks.AddRange(Const.DataServiceSqlite.GetRcmsList());
            tasks.ForEach(task =>
            {
                task.Init();
                task.Run();
            });
            #region 数据发送
            bool ended = false;
            Thread thread = new Thread(new ThreadStart(() =>
            {
                while (!ended)
                {
                    Thread.Sleep(1000);
                    tasks.ForEach(task => task.PrintTaskLogs());
                    Console.WriteLine();
                }
            }))
            { IsBackground = true };
            #endregion

            #region 启动控制
            Console.WriteLine("回车以结束发送，start in:");
            int i = 3;
            while (i > 0)
            {
                Console.WriteLine(i--);
                Thread.Sleep(1000);
            }
            thread.Start();
            #endregion
            Console.ReadLine();
            ended = true;
            Thread.Sleep(2000);
            tasks.ForEach(task => task.Stop());
            Console.WriteLine("回车退出");
            Console.ReadLine();
        }

        #region 事件
        /// <summary>
        /// 未捕获异常触发事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private static void UnhandledException_Raising(object sender, UnhandledExceptionEventArgs args)
        {
            Exception e = (Exception)args.ExceptionObject;
            FileClient.WriteFailureInfo(new string[] { string.Format("未处理异常被触发，运行时是否即将终止：{0}，错误信息：{1}", args.IsTerminating, e.Message), e.StackTrace, e.TargetSite.ToString() }, "UnhandledException", "unhandled " + DateTime.Now.ToString("yyyy-MM-dd"));
        }
        #endregion
    }
}
