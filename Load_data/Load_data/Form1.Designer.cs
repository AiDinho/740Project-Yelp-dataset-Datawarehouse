namespace Load_data
{
    partial class Form1
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.FBDpatsh = new System.Windows.Forms.FolderBrowserDialog();
            this.TBPath = new System.Windows.Forms.TextBox();
            this.btnLoad = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.lblProcess = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // TBPath
            // 
            this.TBPath.Location = new System.Drawing.Point(31, 33);
            this.TBPath.Name = "TBPath";
            this.TBPath.Size = new System.Drawing.Size(351, 20);
            this.TBPath.TabIndex = 0;
            this.TBPath.Click += new System.EventHandler(this.ShowFBDPatsh);
            // 
            // btnLoad
            // 
            this.btnLoad.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnLoad.Location = new System.Drawing.Point(31, 59);
            this.btnLoad.Name = "btnLoad";
            this.btnLoad.Size = new System.Drawing.Size(432, 70);
            this.btnLoad.TabIndex = 1;
            this.btnLoad.Text = "Load data to MS SQL Server";
            this.btnLoad.UseVisualStyleBackColor = true;
            this.btnLoad.Click += new System.EventHandler(this.LoadData);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(388, 30);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 2;
            this.button2.Text = "Path...";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.ShowFBDPatsh);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.Location = new System.Drawing.Point(3, 3);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(484, 24);
            this.label1.TabIndex = 3;
            this.label1.Text = "Please enter path for a folder with DATA - files with JSON";
            // 
            // lblProcess
            // 
            this.lblProcess.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lblProcess.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblProcess.Location = new System.Drawing.Point(0, 132);
            this.lblProcess.Name = "lblProcess";
            this.lblProcess.Size = new System.Drawing.Size(488, 63);
            this.lblProcess.TabIndex = 4;
            this.lblProcess.Text = "1";
            this.lblProcess.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblProcess.UseMnemonic = false;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(488, 195);
            this.Controls.Add(this.lblProcess);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.btnLoad);
            this.Controls.Add(this.TBPath);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Form1";
            this.Text = "Cube downloading";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.FolderBrowserDialog FBDpatsh;
        private System.Windows.Forms.TextBox TBPath;
        private System.Windows.Forms.Button btnLoad;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblProcess;
    }
}

