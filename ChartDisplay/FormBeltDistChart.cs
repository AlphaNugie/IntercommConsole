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
    public partial class FormBeltDistChart : Form
    {
        private readonly string _machineName; //大机名称
        private readonly int _maxCount = 60 * 5;
        private readonly int _num = 1;//每次删除增加点的数目
        private readonly OracleProvider provider = new OracleProvider(Def.HostAddress, Def.ServiceName, Def.UserName, Def.Password);
        private readonly Queue<BeltDistPowers> _dataQueue;

        public FormBeltDistChart(string machine_name)
        {
            InitializeComponent();
            _machineName = machine_name;
            _dataQueue = new Queue<BeltDistPowers>(_maxCount);
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
                BeltDistPowers group = _dataQueue.ElementAt(i);
                this.chart1.Series["BeltDist"].Points.AddXY(i, group.BeltDist);
                this.chart1.Series["WheelPowerRaw"].Points.AddXY(i, group.WheelPowerRaw);
                this.chart1.Series["WheelPowerPolished"].Points.AddXY(i, group.WheelPowerPolished);
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
            chartArea.AxisY.Minimum = -1;
            chartArea.AxisY.Maximum = 5; //斗轮功率最大值，功率范围为1000~10000，除以2000，则范围为0.5~5
            chartArea.AxisX.Interval = 20;
            chartArea.AxisX.MajorGrid.LineColor = Color.Silver;
            chartArea.AxisY.MajorGrid.LineColor = Color.Silver;
            Series seriesBeltDist = new Series("BeltDist") { ChartArea = "C1", Color = Color.Red, ChartType = SeriesChartType.Line }, seriesWheelPowerRaw = new Series("WheelPowerRaw") { ChartArea = "C1", Color = Color.Blue, ChartType = SeriesChartType.Line }, seriesWheelPowerPolished = new Series("WheelPowerPolished") { ChartArea = "C1", Color = Color.Orange, ChartType = SeriesChartType.Line };
            Legend legendBeltDist = new Legend("皮带雷达测距") { ForeColor = Color.Red }, legendWheelPowerRaw = new Legend("斗轮功率(原始") { ForeColor = Color.Blue }, legendWheelPowerPolished = new Legend("斗轮功率(滤波") { ForeColor = Color.Orange };
            Title title = new Title("料流雷达测距折线图", Docking.Top, new Font("Microsoft Sans Serif", 12F), Color.RoyalBlue);

            this.chart1.ChartAreas.Clear();
            this.chart1.ChartAreas.Add(chartArea);
            //Series
            this.chart1.Series.Clear();
            this.chart1.Series.Add(seriesBeltDist);
            this.chart1.Series.Add(seriesWheelPowerRaw);
            this.chart1.Series.Add(seriesWheelPowerPolished);
            //Legends
            this.chart1.Legends.Clear();
            this.chart1.Legends.Add(legendBeltDist);
            this.chart1.Legends.Add(legendWheelPowerRaw);
            this.chart1.Legends.Add(legendWheelPowerPolished);
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
            string sqlString = string.Format("select t.belt_dist, t.wheel_power_raw, t.wheel_power_polished from t_rcms_machineposture_time t where t.machine_name = '{0}'", _machineName);
            DataTable table = this.provider.Query(sqlString);
            if (table == null || table.Rows.Count == 0)
                return;
            this._dataQueue.Enqueue(BeltDistPowers.GetInstance(table.Rows[0]));
        }
    }

    public class BeltDistPowers
    {
        public double BeltDist { get; set; }

        public double WheelPowerRaw { get; set; }

        public double WheelPowerPolished { get; set; }

        public static BeltDistPowers GetInstance(DataRow row)
        {
            return row == null ? null : new BeltDistPowers() { BeltDist = double.Parse(row["belt_dist"].ToString()), WheelPowerRaw = double.Parse(row["wheel_power_raw"].ToString()) / 2000, WheelPowerPolished = double.Parse(row["wheel_power_polished"].ToString()) / 2000 };
        }
    }
}
