using CommonLib.Clients;
using CommonLib.DataUtil;
using CommonLib.Events;
using CommonLib.Function;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace ChartDisplay
{
    public partial class FormBlockDistChart : Form
    {
        //private readonly string _machineName; //大机名称
        //private readonly int _maxCount = 60 * 5;
        private readonly int _maxCount = 60 * 10;
        private readonly int _num = 1;//每次删除增加点的数目
        //private readonly OracleProvider provider = new OracleProvider(Def.HostAddress, Def.ServiceName, Def.UserName, Def.Password);
        private readonly string _localIp;
        private readonly int _localPort = 43993;
        private string _received;
        private readonly DerivedUdpClient _udpClient;
        private readonly Queue<BlockDistances> _dataQueue;

        public FormBlockDistChart(string machine_name)
        {
            InitializeComponent();
            //_machineName = machine_name;
            _dataQueue = new Queue<BlockDistances>(_maxCount);
            _localIp = Functions.GetIPAddressV4();
            _udpClient = new DerivedUdpClient(_localIp, _localPort, true, false);
            _udpClient.DataReceived += new DataReceivedEventHandler(UdpClient_DataReceived);
            this.InitChart();
        }

        private void UdpClient_DataReceived(object sender, DataReceivedEventArgs eventArgs)
        {
            _received = eventArgs.ReceivedInfo_String;
            //throw new NotImplementedException();
        }

        /// <summary>
        /// 初始化事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Init_Click(object sender, EventArgs e)
        {
            InitChart();
        }

        /// <summary>
        /// 开始事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Start_Click(object sender, EventArgs e)
        {
            this.timer1.Start();
        }

        /// <summary>
        /// 停止事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Stop_Click(object sender, EventArgs e)
        {
            this.timer1.Stop();
        }

        /// <summary>
        /// 定时器事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Timer1_Tick(object sender, EventArgs e)
        {
            BlockDistances dists;
            this.UpdateQueueValue(out dists);
            if (_start)
                //File.AppendAllLines(_fullPath, new string[] { string.Format("{0:yyyy-MM-dd HH:mm:ss},{1},{2},{3},{4},{5},{6}", DateTime.Now, dists.LeftFront, dists.LeftMiddle, dists.LeftBack, dists.RightFront, dists.RightMiddle, dists.RightBack) });
                File.AppendAllLines(string.Format(@"{0}{1:yyyyMMddHH}.csv", _filePath, DateTime.Now), new string[] { string.Format("{0:yyyy-MM-dd HH:mm:ss},{1},{2},{3},{4},{5},{6}", DateTime.Now, dists.LeftFront, dists.LeftMiddle, dists.LeftBack, dists.RightFront, dists.RightMiddle, dists.RightBack) });
            foreach (var series in this.chart1.Series)
                series.Points.Clear();
            for (int i = 0; i < _dataQueue.Count; i++)
            {
                BlockDistances group = _dataQueue.ElementAt(i);
                //if (group.LeftFront != 50 && checkBox_LeftFront.Checked)
                //    this.chart1.Series["LeftFront"].Points.AddXY(i, group.LeftFront);
                //if (group.LeftMiddle != 50 && checkBox_LeftMiddle.Checked)
                //    this.chart1.Series["LeftMiddle"].Points.AddXY(i, group.LeftMiddle);
                //if (group.LeftBack != 50 && checkBox_LeftBack.Checked)
                //    this.chart1.Series["LeftBack"].Points.AddXY(i, group.LeftBack);
                //if (group.RightFront != 50 && checkBox_RightFront.Checked)
                //    this.chart1.Series["RightFront"].Points.AddXY(i, group.RightFront);
                //if (group.RightMiddle != 50 && checkBox_RightMiddle.Checked)
                //    this.chart1.Series["RightMiddle"].Points.AddXY(i, group.RightMiddle);
                //if (group.RightBack != 50 && checkBox_RightBack.Checked)
                //    this.chart1.Series["RightBack"].Points.AddXY(i, group.RightBack);
                if (checkBox_LeftFront.Checked)
                    this.chart1.Series["LeftFront"].Points.AddXY(i, group.LeftFront);
                if (checkBox_LeftMiddle.Checked)
                    this.chart1.Series["LeftMiddle"].Points.AddXY(i, group.LeftMiddle);
                if (checkBox_LeftBack.Checked)
                    this.chart1.Series["LeftBack"].Points.AddXY(i, group.LeftBack);
                if (checkBox_RightFront.Checked)
                    this.chart1.Series["RightFront"].Points.AddXY(i, group.RightFront);
                if (checkBox_RightMiddle.Checked)
                    this.chart1.Series["RightMiddle"].Points.AddXY(i, group.RightMiddle);
                if (checkBox_RightBack.Checked)
                    this.chart1.Series["RightBack"].Points.AddXY(i, group.RightBack);
            }
        }

        /// <summary>
        /// 初始化图表
        /// </summary>
        private void InitChart()
        {
            //定义图表区域
            ChartArea chartArea = new ChartArea("C1");
            //设置图表显示样式
            chartArea.AxisX.Minimum = 0;
            //chartArea.AxisX.Interval = 5;
            chartArea.AxisX.Interval = 10;
            chartArea.AxisY.Minimum = -4;
            chartArea.AxisY.Maximum = 52; //网格测距最大值为60
            chartArea.AxisY.Interval = 2;
            chartArea.AxisX.MajorGrid.LineColor = Color.Silver;
            chartArea.AxisY.MajorGrid.LineColor = Color.Silver;
            Series seriesLeftFront = new Series("LeftFront") { ChartArea = "C1", Color = Color.Red, ChartType = SeriesChartType.Line }, seriesLeftMiddle = new Series("LeftMiddle") { ChartArea = "C1", Color = Color.Blue, ChartType = SeriesChartType.Line }, seriesLeftBack = new Series("LeftBack") { ChartArea = "C1", Color = Color.Orange, ChartType = SeriesChartType.Line }, seriesRightFront = new Series("RightFront") { ChartArea = "C1", Color = Color.Black, ChartType = SeriesChartType.Line }, seriesRightMiddle = new Series("RightMiddle") { ChartArea = "C1", Color = Color.Green, ChartType = SeriesChartType.Line }, seriesRightBack = new Series("RightBack") { ChartArea = "C1", Color = Color.Purple, ChartType = SeriesChartType.Line };
            Legend legendLeftFront = new Legend("左前") { ForeColor = Color.Red }, legendLeftMiddle = new Legend("左中") { ForeColor = Color.Blue }, legendLeftBack = new Legend("左后") { ForeColor = Color.Orange }, legendRightFront = new Legend("右前") { ForeColor = Color.Black }, legendRightMiddle = new Legend("右中") { ForeColor = Color.Green }, legendRightBack = new Legend("右后") { ForeColor = Color.Purple };
            Title title = new Title("网格测距折线图", Docking.Top, new Font("Microsoft Sans Serif", 12F), Color.RoyalBlue);

            this.chart1.ChartAreas.Clear();
            this.chart1.ChartAreas.Add(chartArea);
            //Series
            this.chart1.Series.Clear();
            this.chart1.Series.Add(seriesLeftFront);
            this.chart1.Series.Add(seriesLeftMiddle);
            this.chart1.Series.Add(seriesLeftBack);
            this.chart1.Series.Add(seriesRightFront);
            this.chart1.Series.Add(seriesRightMiddle);
            this.chart1.Series.Add(seriesRightBack);
            //Legends
            this.chart1.Legends.Clear();
            this.chart1.Legends.Add(legendLeftFront);
            this.chart1.Legends.Add(legendLeftMiddle);
            this.chart1.Legends.Add(legendLeftBack);
            this.chart1.Legends.Add(legendRightFront);
            this.chart1.Legends.Add(legendRightMiddle);
            this.chart1.Legends.Add(legendRightBack);
            //设置标题
            this.chart1.Titles.Clear();
            this.chart1.Titles.Add(title);
        }

        //更新队列中的值
        private void UpdateQueueValue(out BlockDistances dists)
        {

            if (this._dataQueue.Count > _maxCount)
                //先出列
                for (int i = 0; i < _num; i++)
                    this._dataQueue.Dequeue();
            string r = _received;
            //this._dataQueue.Enqueue(BlockDistances.GetInstance(r));
            dists = new BlockDistances(r);
            this._dataQueue.Enqueue(dists);
        }

        bool _start = false;
        string _filePath = @"D:\Mei2\DistanceRecording\", _fileName = string.Empty, _fullPath;
        private void Button_StartRecord_Click(object sender, EventArgs e)
        {
            _start = button_StartRecord.Text.Equals("开始记录");
            button_StartRecord.Text = _start ? "停止记录" : "开始记录";
            if (_start)
            {
                _fullPath = string.Format(@"{0}{1:yyyyMMddHHmmss}.csv", _filePath, DateTime.Now);
                if (!Directory.Exists(_filePath))
                    Directory.CreateDirectory(_filePath);
                File.AppendAllLines(_fullPath, new string[] { "time,left_front,left_middle,left_back,right_front,right_middle,right_back" });
            }
        }
    }

    public class BlockDistances
    {
        /// <summary>
        /// 大臂左前距离
        /// </summary>
        public double LeftFront { get; set; }

        /// <summary>
        /// 大臂左中距离
        /// </summary>
        public double LeftMiddle { get; set; }

        /// <summary>
        /// 大臂左后距离
        /// </summary>
        public double LeftBack { get; set; }

        /// <summary>
        /// 大臂右前距离
        /// </summary>
        public double RightFront { get; set; }

        /// <summary>
        /// 大臂右中距离
        /// </summary>
        public double RightMiddle { get; set; }

        /// <summary>
        /// 大臂右后距离
        /// </summary>
        public double RightBack { get; set; }

        public BlockDistances(string received)
        {
            Update(received);
        }

        public void Update(string received)
        {
            if (string.IsNullOrWhiteSpace(received))
                return;
            string[] parts = received.Split(',');
            if (parts.Length < 8)
                return;
            LeftFront = double.Parse(parts[1]);
            LeftMiddle = double.Parse(parts[2]);
            LeftBack = double.Parse(parts[3]);
            RightFront = double.Parse(parts[4]);
            RightMiddle = double.Parse(parts[5]);
            RightBack = double.Parse(parts[6]);
        }

        public static BlockDistances GetInstance(string received)
        {
            BlockDistances dist = new BlockDistances(received);
            return dist;
        }
    }
}
