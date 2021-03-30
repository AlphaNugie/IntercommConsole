using CommonLib.DataUtil;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace ChartDisplay
{
    public partial class FormWheelSlopeChart : Form
    {
        private readonly string _machineName; //大机名称
        private readonly int _maxCount = 60 * 5;
        private readonly int _num = 1;//每次删除增加点的数目
        private readonly OracleProvider provider = new OracleProvider(Def.HostAddress, Def.ServiceName, Def.UserName, Def.Password);
        private readonly Queue<WheelSlopes> _dataQueue;

        public FormWheelSlopeChart(string machine_name)
        {
            InitializeComponent();
            _machineName = machine_name;
            _dataQueue = new Queue<WheelSlopes>(_maxCount);
            this.InitChart();
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
            this.UpdateQueueValue();
            foreach (var series in this.chart1.Series)
                series.Points.Clear();
            for (int i = 0; i < _dataQueue.Count; i++)
            {
                WheelSlopes group = _dataQueue.ElementAt(i);
                this.chart1.Series["WheelLeft"].Points.AddXY(i, group.WheelLeftSlope);
                this.chart1.Series["WheelRight"].Points.AddXY(i, group.WheelRightSlope);
                this.chart1.Series["YawAngle"].Points.AddXY(i, group.YawAngle);
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
            chartArea.AxisY.Minimum = 0;
            chartArea.AxisY.Maximum = 36; //Y轴最大值，回转角度范围为-180~180，除以10加上18，则范围为0~36
            chartArea.AxisX.Interval = 5;
            chartArea.AxisX.MajorGrid.LineColor = Color.Silver;
            chartArea.AxisY.MajorGrid.LineColor = Color.Silver;
            Series seriesWheelLeft = new Series("WheelLeft") { ChartArea = "C1", Color = Color.Red, ChartType = SeriesChartType.Line }, seriesWheelRight = new Series("WheelRight") { ChartArea = "C1", Color = Color.Blue, ChartType = SeriesChartType.Line }, seriesYawAngle = new Series("YawAngle") { ChartArea = "C1", Color = Color.Orange, ChartType = SeriesChartType.Line };
            Legend legendWheelLeft = new Legend("斗轮左") { ForeColor = Color.Red }, legendWheelRight = new Legend("斗轮右") { ForeColor = Color.Blue }, legendYaw = new Legend("回转") { ForeColor = Color.Orange };
            Title title = new Title("斗轮雷达斜率折线图", Docking.Top, new Font("Microsoft Sans Serif", 12F), Color.RoyalBlue);

            this.chart1.ChartAreas.Clear();
            this.chart1.ChartAreas.Add(chartArea);
            //Series
            this.chart1.Series.Clear();
            this.chart1.Series.Add(seriesWheelLeft);
            this.chart1.Series.Add(seriesWheelRight);
            this.chart1.Series.Add(seriesYawAngle);
            //Legends
            this.chart1.Legends.Clear();
            this.chart1.Legends.Add(legendWheelLeft);
            this.chart1.Legends.Add(legendWheelRight);
            this.chart1.Legends.Add(legendYaw);
            //设置标题
            this.chart1.Titles.Clear();
            this.chart1.Titles.Add(title);
        }

        //更新队列中的值
        private void UpdateQueueValue()
        {

            if (this._dataQueue.Count > _maxCount)
                //先出列
                for (int i = 0; i < _num; i++)
                    this._dataQueue.Dequeue();
            string sqlString = string.Format("select t.wheel_left_slope, t.wheel_right_slope, t.yaw_plc from t_rcms_machineposture_time t where t.machine_name = '{0}'", _machineName);
            DataTable table = this.provider.Query(sqlString);
            if (table == null || table.Rows.Count == 0)
                return;
            this._dataQueue.Enqueue(WheelSlopes.GetInstance(table.Rows[0]));
        }
    }

    public class WheelSlopes
    {
        public double WheelLeftSlope { get; set; }

        public double WheelRightSlope { get; set; }

        public double YawAngle { get; set; }

        public static WheelSlopes GetInstance(DataRow row)
        {
            return row == null ? null : new WheelSlopes() { WheelLeftSlope = double.Parse(row["wheel_left_slope"].ToString()), WheelRightSlope = double.Parse(row["wheel_right_slope"].ToString()), YawAngle = double.Parse(row["yaw_plc"].ToString()) / 10 + 18 };
        }
    }
}
