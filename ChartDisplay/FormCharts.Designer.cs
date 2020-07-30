namespace ChartDisplay
{
    partial class FormCharts
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.tabControl_Charts = new System.Windows.Forms.TabControl();
            this.SuspendLayout();
            // 
            // tabControl_Charts
            // 
            this.tabControl_Charts.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl_Charts.Location = new System.Drawing.Point(0, 0);
            this.tabControl_Charts.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tabControl_Charts.Name = "tabControl_Charts";
            this.tabControl_Charts.SelectedIndex = 0;
            this.tabControl_Charts.Size = new System.Drawing.Size(1116, 601);
            this.tabControl_Charts.TabIndex = 0;
            // 
            // FormCharts
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1116, 601);
            this.Controls.Add(this.tabControl_Charts);
            this.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "FormCharts";
            this.Text = "FormCharts";
            this.Load += new System.EventHandler(this.FormCharts_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl_Charts;
    }
}