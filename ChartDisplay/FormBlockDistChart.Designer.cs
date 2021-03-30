namespace ChartDisplay
{
    partial class FormBlockDistChart
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.button_Start = new System.Windows.Forms.Button();
            this.chart1 = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.button_Stop = new System.Windows.Forms.Button();
            this.button_Init = new System.Windows.Forms.Button();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.checkBox_LeftFront = new System.Windows.Forms.CheckBox();
            this.checkBox_LeftMiddle = new System.Windows.Forms.CheckBox();
            this.checkBox_LeftBack = new System.Windows.Forms.CheckBox();
            this.checkBox_RightFront = new System.Windows.Forms.CheckBox();
            this.checkBox_RightMiddle = new System.Windows.Forms.CheckBox();
            this.checkBox_RightBack = new System.Windows.Forms.CheckBox();
            this.button_StartRecord = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).BeginInit();
            this.tableLayoutPanel1.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // button_Start
            // 
            this.button_Start.Location = new System.Drawing.Point(108, 5);
            this.button_Start.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.button_Start.Name = "button_Start";
            this.button_Start.Size = new System.Drawing.Size(96, 31);
            this.button_Start.TabIndex = 0;
            this.button_Start.Text = "开始";
            this.button_Start.UseVisualStyleBackColor = true;
            this.button_Start.Click += new System.EventHandler(this.Button_Start_Click);
            // 
            // chart1
            // 
            this.chart1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.chart1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chart1.Location = new System.Drawing.Point(4, 55);
            this.chart1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.chart1.Name = "chart1";
            this.chart1.Size = new System.Drawing.Size(1184, 532);
            this.chart1.TabIndex = 1;
            this.chart1.Text = "chart1";
            // 
            // timer1
            // 
            this.timer1.Interval = 500;
            this.timer1.Tick += new System.EventHandler(this.Timer1_Tick);
            // 
            // button_Stop
            // 
            this.button_Stop.Location = new System.Drawing.Point(212, 5);
            this.button_Stop.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.button_Stop.Name = "button_Stop";
            this.button_Stop.Size = new System.Drawing.Size(96, 31);
            this.button_Stop.TabIndex = 2;
            this.button_Stop.Text = "停止";
            this.button_Stop.UseVisualStyleBackColor = true;
            this.button_Stop.Click += new System.EventHandler(this.Button_Stop_Click);
            // 
            // button_Init
            // 
            this.button_Init.Location = new System.Drawing.Point(4, 5);
            this.button_Init.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.button_Init.Name = "button_Init";
            this.button_Init.Size = new System.Drawing.Size(96, 31);
            this.button_Init.TabIndex = 3;
            this.button_Init.Text = "初始化";
            this.button_Init.UseVisualStyleBackColor = true;
            this.button_Init.Visible = false;
            this.button_Init.Click += new System.EventHandler(this.Button_Init_Click);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.chart1, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.flowLayoutPanel1, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1192, 592);
            this.tableLayoutPanel1.TabIndex = 6;
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Controls.Add(this.button_Init);
            this.flowLayoutPanel1.Controls.Add(this.button_Start);
            this.flowLayoutPanel1.Controls.Add(this.button_Stop);
            this.flowLayoutPanel1.Controls.Add(this.checkBox_LeftFront);
            this.flowLayoutPanel1.Controls.Add(this.checkBox_LeftMiddle);
            this.flowLayoutPanel1.Controls.Add(this.checkBox_LeftBack);
            this.flowLayoutPanel1.Controls.Add(this.checkBox_RightFront);
            this.flowLayoutPanel1.Controls.Add(this.checkBox_RightMiddle);
            this.flowLayoutPanel1.Controls.Add(this.checkBox_RightBack);
            this.flowLayoutPanel1.Controls.Add(this.button_StartRecord);
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(3, 3);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(1186, 44);
            this.flowLayoutPanel1.TabIndex = 2;
            // 
            // checkBox_LeftFront
            // 
            this.checkBox_LeftFront.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.checkBox_LeftFront.AutoSize = true;
            this.checkBox_LeftFront.Checked = true;
            this.checkBox_LeftFront.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_LeftFront.Location = new System.Drawing.Point(315, 8);
            this.checkBox_LeftFront.Name = "checkBox_LeftFront";
            this.checkBox_LeftFront.Size = new System.Drawing.Size(61, 24);
            this.checkBox_LeftFront.TabIndex = 4;
            this.checkBox_LeftFront.Text = "左前";
            this.checkBox_LeftFront.UseVisualStyleBackColor = true;
            // 
            // checkBox_LeftMiddle
            // 
            this.checkBox_LeftMiddle.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.checkBox_LeftMiddle.AutoSize = true;
            this.checkBox_LeftMiddle.Checked = true;
            this.checkBox_LeftMiddle.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_LeftMiddle.Location = new System.Drawing.Point(382, 8);
            this.checkBox_LeftMiddle.Name = "checkBox_LeftMiddle";
            this.checkBox_LeftMiddle.Size = new System.Drawing.Size(61, 24);
            this.checkBox_LeftMiddle.TabIndex = 4;
            this.checkBox_LeftMiddle.Text = "左中";
            this.checkBox_LeftMiddle.UseVisualStyleBackColor = true;
            // 
            // checkBox_LeftBack
            // 
            this.checkBox_LeftBack.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.checkBox_LeftBack.AutoSize = true;
            this.checkBox_LeftBack.Checked = true;
            this.checkBox_LeftBack.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_LeftBack.Location = new System.Drawing.Point(449, 8);
            this.checkBox_LeftBack.Name = "checkBox_LeftBack";
            this.checkBox_LeftBack.Size = new System.Drawing.Size(61, 24);
            this.checkBox_LeftBack.TabIndex = 4;
            this.checkBox_LeftBack.Text = "左后";
            this.checkBox_LeftBack.UseVisualStyleBackColor = true;
            // 
            // checkBox_RightFront
            // 
            this.checkBox_RightFront.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.checkBox_RightFront.AutoSize = true;
            this.checkBox_RightFront.Checked = true;
            this.checkBox_RightFront.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_RightFront.Location = new System.Drawing.Point(516, 8);
            this.checkBox_RightFront.Name = "checkBox_RightFront";
            this.checkBox_RightFront.Size = new System.Drawing.Size(61, 24);
            this.checkBox_RightFront.TabIndex = 4;
            this.checkBox_RightFront.Text = "右前";
            this.checkBox_RightFront.UseVisualStyleBackColor = true;
            // 
            // checkBox_RightMiddle
            // 
            this.checkBox_RightMiddle.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.checkBox_RightMiddle.AutoSize = true;
            this.checkBox_RightMiddle.Checked = true;
            this.checkBox_RightMiddle.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_RightMiddle.Location = new System.Drawing.Point(583, 8);
            this.checkBox_RightMiddle.Name = "checkBox_RightMiddle";
            this.checkBox_RightMiddle.Size = new System.Drawing.Size(61, 24);
            this.checkBox_RightMiddle.TabIndex = 4;
            this.checkBox_RightMiddle.Text = "右中";
            this.checkBox_RightMiddle.UseVisualStyleBackColor = true;
            // 
            // checkBox_RightBack
            // 
            this.checkBox_RightBack.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.checkBox_RightBack.AutoSize = true;
            this.checkBox_RightBack.Checked = true;
            this.checkBox_RightBack.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_RightBack.Location = new System.Drawing.Point(650, 8);
            this.checkBox_RightBack.Name = "checkBox_RightBack";
            this.checkBox_RightBack.Size = new System.Drawing.Size(61, 24);
            this.checkBox_RightBack.TabIndex = 4;
            this.checkBox_RightBack.Text = "右后";
            this.checkBox_RightBack.UseVisualStyleBackColor = true;
            // 
            // button_StartRecord
            // 
            this.button_StartRecord.Location = new System.Drawing.Point(717, 3);
            this.button_StartRecord.Name = "button_StartRecord";
            this.button_StartRecord.Size = new System.Drawing.Size(86, 33);
            this.button_StartRecord.TabIndex = 5;
            this.button_StartRecord.Text = "开始记录";
            this.button_StartRecord.UseVisualStyleBackColor = true;
            this.button_StartRecord.Click += new System.EventHandler(this.Button_StartRecord_Click);
            // 
            // FormBlockDistChart
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1192, 592);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "FormBlockDistChart";
            this.Text = "网格测距曲线";
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).EndInit();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button button_Start;
        private System.Windows.Forms.DataVisualization.Charting.Chart chart1;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Button button_Stop;
        private System.Windows.Forms.Button button_Init;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.CheckBox checkBox_LeftFront;
        private System.Windows.Forms.CheckBox checkBox_LeftMiddle;
        private System.Windows.Forms.CheckBox checkBox_LeftBack;
        private System.Windows.Forms.CheckBox checkBox_RightFront;
        private System.Windows.Forms.CheckBox checkBox_RightMiddle;
        private System.Windows.Forms.CheckBox checkBox_RightBack;
        private System.Windows.Forms.Button button_StartRecord;
    }
}

