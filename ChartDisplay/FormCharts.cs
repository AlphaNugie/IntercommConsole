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
        private readonly string _machineName = "S1"; //大机名称

        public FormCharts()
        {
            InitializeComponent();
        }

        private void FormCharts_Load(object sender, EventArgs e)
        {
            this.tabControl_Charts.ShowForm(new FormWheelDistChart(_machineName));
            this.tabControl_Charts.ShowForm(new FormBeltDistChart(_machineName));
            this.tabControl_Charts.ShowForm(new FormWheelSlopeChart(_machineName));
        }
    }
}
