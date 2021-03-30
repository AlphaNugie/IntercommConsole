using CommonLib.Function;
using CommonLib.UIControlUtil;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ChartDisplay
{
    public partial class FormCharts : Form
    {
        //private readonly IniFileHelper _iniHelper;
        //private readonly string _machineName; //大机名称

        public FormCharts()
        {
            InitializeComponent();
            //_iniHelper = new IniFileHelper(@"..\..\..\IntercommConsole\bin\Debug\Config.ini");
            //_machineName = _iniHelper.ReadData("Main", "MachineName");
        }

        private void FormCharts_Load(object sender, EventArgs e)
        {
            this.tabControl_Charts.ShowForm(new FormWheelDistChart(Def.MachineName));
            this.tabControl_Charts.ShowForm(new FormBeltDistChart(Def.MachineName));
            //this.tabControl_Charts.ShowForm(new FormWheelSlopeChart(Def.MachineName));
            this.tabControl_Charts.ShowForm(new FormWheelSurfaceAngleChart(Def.MachineName));
            this.tabControl_Charts.ShowForm(new FormBlockDistChart(Def.MachineName));
        }
    }
}
