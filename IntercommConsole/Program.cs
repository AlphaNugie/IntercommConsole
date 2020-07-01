using System;
using System.Collections.Generic;
using CommonLib.Clients;
using System.Threading;
using IntercommConsole.Tasks;
using IntercommConsole.Core;
using IntercommConsole.Model;
using Newtonsoft.Json;

namespace IntercommConsole
{
    public class Program
    {
        static void Main(string[] args)
        {
            //string json = "{\"dataId\":\"20200629112446\",\"materialHeight\":12.2,\"materialVolume\":0.0,\"reposeAngle\":0.0,\"blankingDistance\":6.88,\"runningPosition\":522.12,\"rotationAngle\":-62.78,\"pitchAngle\":8.3,\"collisionInfo\":\"11110111000000000011\"}";
            //StrategyDataSource source = JsonConvert.DeserializeObject<StrategyDataSource>(json);
            //json = JsonConvert.SerializeObject(source);
            DbDef.Update();
            Const.WriteConsoleLog("IntercommConsole启动，本地IP: " + Const.LocalIp);
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(UnhandledException_Raising); //未捕获异常触发事件
            //任务
            List<Task> tasks = new List<Task>() {
                new DataProcessTask(),
                new OpcTask(),
                new ModelBuildingServiceTask(),
                new ModelDisplayServiceTask(),
                new StrategyServiceTask(),
                new DbOracleTask(),
                new DbSqliteTask()
            };
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
