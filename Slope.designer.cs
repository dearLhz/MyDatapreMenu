namespace MyDatapreMenu
{
    partial class frmSlope
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmSlope));
            this.label1 = new System.Windows.Forms.Label();
            this.comboBoxInData = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.radioDegree = new System.Windows.Forms.RadioButton();
            this.radioPercent = new System.Windows.Forms.RadioButton();
            this.label3 = new System.Windows.Forms.Label();
            this.txtZFactor = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtCellSize = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txtOutPath = new System.Windows.Forms.TextBox();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnGO = new System.Windows.Forms.Button();
            this.btnOpenData = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 17);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(77, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "输入数据源：";
            // 
            // comboBoxInData
            // 
            this.comboBoxInData.FormattingEnabled = true;
            this.comboBoxInData.Location = new System.Drawing.Point(95, 14);
            this.comboBoxInData.Name = "comboBoxInData";
            this.comboBoxInData.Size = new System.Drawing.Size(194, 20);
            this.comboBoxInData.TabIndex = 1;
            this.comboBoxInData.SelectedIndexChanged += new System.EventHandler(this.comboBoxInData_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 63);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(89, 12);
            this.label2.TabIndex = 3;
            this.label2.Text = "输出测量单位：";
            // 
            // radioDegree
            // 
            this.radioDegree.AutoSize = true;
            this.radioDegree.Checked = true;
            this.radioDegree.Location = new System.Drawing.Point(126, 61);
            this.radioDegree.Name = "radioDegree";
            this.radioDegree.Size = new System.Drawing.Size(35, 16);
            this.radioDegree.TabIndex = 4;
            this.radioDegree.TabStop = true;
            this.radioDegree.Text = "度";
            this.radioDegree.UseVisualStyleBackColor = true;
            // 
            // radioPercent
            // 
            this.radioPercent.AutoSize = true;
            this.radioPercent.Location = new System.Drawing.Point(194, 61);
            this.radioPercent.Name = "radioPercent";
            this.radioPercent.Size = new System.Drawing.Size(59, 16);
            this.radioPercent.TabIndex = 5;
            this.radioPercent.TabStop = true;
            this.radioPercent.Text = "百分比";
            this.radioPercent.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 112);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 12);
            this.label3.TabIndex = 6;
            this.label3.Text = "Z 因子：";
            // 
            // txtZFactor
            // 
            this.txtZFactor.Location = new System.Drawing.Point(95, 108);
            this.txtZFactor.Name = "txtZFactor";
            this.txtZFactor.Size = new System.Drawing.Size(194, 21);
            this.txtZFactor.TabIndex = 7;
            this.txtZFactor.Text = "1";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 156);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(89, 12);
            this.label4.TabIndex = 8;
            this.label4.Text = "输出象素大小：";
            // 
            // txtCellSize
            // 
            this.txtCellSize.Location = new System.Drawing.Point(95, 152);
            this.txtCellSize.Name = "txtCellSize";
            this.txtCellSize.Size = new System.Drawing.Size(194, 21);
            this.txtCellSize.TabIndex = 9;
            this.txtCellSize.Text = "1000";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 202);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(65, 12);
            this.label5.TabIndex = 10;
            this.label5.Text = "输出栅格：";
            // 
            // txtOutPath
            // 
            this.txtOutPath.Location = new System.Drawing.Point(95, 198);
            this.txtOutPath.Name = "txtOutPath";
            this.txtOutPath.Size = new System.Drawing.Size(194, 21);
            this.txtOutPath.TabIndex = 11;
            // 
            // btnSave
            // 
            this.btnSave.Image = ((System.Drawing.Image)(resources.GetObject("btnSave.Image")));
            this.btnSave.Location = new System.Drawing.Point(307, 196);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(25, 25);
            this.btnSave.TabIndex = 12;
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(247, 245);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(67, 25);
            this.btnCancel.TabIndex = 13;
            this.btnCancel.Text = "关闭";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnGO
            // 
            this.btnGO.Location = new System.Drawing.Point(126, 247);
            this.btnGO.Name = "btnGO";
            this.btnGO.Size = new System.Drawing.Size(67, 25);
            this.btnGO.TabIndex = 14;
            this.btnGO.Text = "开始";
            this.btnGO.UseVisualStyleBackColor = true;
            this.btnGO.Click += new System.EventHandler(this.btnGO_Click);
            // 
            // btnOpenData
            // 
            this.btnOpenData.Image = ((System.Drawing.Image)(resources.GetObject("btnOpenData.Image")));
            this.btnOpenData.Location = new System.Drawing.Point(307, 12);
            this.btnOpenData.Name = "btnOpenData";
            this.btnOpenData.Size = new System.Drawing.Size(25, 25);
            this.btnOpenData.TabIndex = 2;
            this.btnOpenData.UseVisualStyleBackColor = true;
            this.btnOpenData.Click += new System.EventHandler(this.btnOpenData_Click);
            // 
            // frmSlope
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(344, 282);
            this.Controls.Add(this.btnGO);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.txtOutPath);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.txtCellSize);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txtZFactor);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.radioPercent);
            this.Controls.Add(this.radioDegree);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btnOpenData);
            this.Controls.Add(this.comboBoxInData);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmSlope";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "坡度分析";
            this.Load += new System.EventHandler(this.frmSlope_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox comboBoxInData;
        private System.Windows.Forms.Button btnOpenData;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.RadioButton radioDegree;
        private System.Windows.Forms.RadioButton radioPercent;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtZFactor;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtCellSize;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtOutPath;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnGO;
    }
}