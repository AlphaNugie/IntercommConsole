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
        }

        ///// <summary>
        ///// 在TabPage页中加载窗体对象
        ///// </summary>
        ///// <param name="form">需在TabPage页中加载或显示的窗体对象</param>
        ///// <param name="dock">停靠方式，假如为null则不停靠</param>
        //private void ShowForm(Form form, DockStyle? dock)
        //{
        //    if (form == null)
        //        return;

        //    //假如Tab页已存在，选中该页面
        //    foreach (TabPage tabPage in this.tabControl_Charts.TabPages)
        //        if (tabPage.Name == form.Name)
        //        {
        //            this.tabControl_Charts.SelectedTab = tabPage;
        //            return;
        //        }

        //    //在TabControl中显示包含该页面的TabPage
        //    form.TopLevel = false; //不置顶
        //    if (dock != null)
        //        form.Dock = dock.Value; //控件停靠方式
        //    form.FormBorderStyle = FormBorderStyle.None; //页面无边框
        //    TabPage page = new TabPage();
        //    page.Controls.Add(form);
        //    page.Text = form.Text;
        //    page.Name = form.Name;
        //    page.AutoScroll = true;
        //    this.Invoke(new MethodInvoker(delegate
        //    {
        //        this.tabControl_Charts.TabPages.Add(page);
        //        this.tabControl_Charts.SelectedTab = page;
        //        form.Show();
        //    }));
        //}
    }
}
