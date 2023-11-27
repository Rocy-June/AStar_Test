namespace TestWinForm
{
    partial class MainForm_1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            panel_Canvas = new Panel();
            groupBox_Control = new GroupBox();
            button_Split = new Button();
            button_ClearMap = new Button();
            button_AutoStep = new Button();
            button_ToEnd = new Button();
            button_SetWall = new Button();
            button_SetEnd = new Button();
            button_SetStart = new Button();
            button_NextStep = new Button();
            checkBox_Ratio = new CheckBox();
            label_MapX = new Label();
            numericUpDown_MapY = new NumericUpDown();
            numericUpDown_MapX = new NumericUpDown();
            groupBox_Control.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)numericUpDown_MapY).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numericUpDown_MapX).BeginInit();
            SuspendLayout();
            // 
            // panel_Canvas
            // 
            panel_Canvas.BorderStyle = BorderStyle.FixedSingle;
            panel_Canvas.Location = new Point(12, 69);
            panel_Canvas.Name = "panel_Canvas";
            panel_Canvas.Size = new Size(800, 450);
            panel_Canvas.TabIndex = 1;
            panel_Canvas.Paint += panel_Canvas_Paint;
            panel_Canvas.MouseDown += panel_Canvas_MouseDown;
            panel_Canvas.MouseMove += panel_Canvas_MouseMove;
            // 
            // groupBox_Control
            // 
            groupBox_Control.Controls.Add(button_Split);
            groupBox_Control.Controls.Add(button_ClearMap);
            groupBox_Control.Controls.Add(button_AutoStep);
            groupBox_Control.Controls.Add(button_ToEnd);
            groupBox_Control.Controls.Add(button_SetWall);
            groupBox_Control.Controls.Add(button_SetEnd);
            groupBox_Control.Controls.Add(button_SetStart);
            groupBox_Control.Controls.Add(button_NextStep);
            groupBox_Control.Controls.Add(checkBox_Ratio);
            groupBox_Control.Controls.Add(label_MapX);
            groupBox_Control.Controls.Add(numericUpDown_MapY);
            groupBox_Control.Controls.Add(numericUpDown_MapX);
            groupBox_Control.Location = new Point(12, 12);
            groupBox_Control.Name = "groupBox_Control";
            groupBox_Control.Size = new Size(800, 51);
            groupBox_Control.TabIndex = 0;
            groupBox_Control.TabStop = false;
            groupBox_Control.Text = "控制器";
            // 
            // button_Split
            // 
            button_Split.Location = new Point(516, 22);
            button_Split.Name = "button_Split";
            button_Split.Size = new Size(65, 23);
            button_Split.TabIndex = 8;
            button_Split.Text = "切分";
            button_Split.UseVisualStyleBackColor = true;
            button_Split.Click += button_Split_Click;
            // 
            // button_ClearMap
            // 
            button_ClearMap.Location = new Point(484, 22);
            button_ClearMap.Name = "button_ClearMap";
            button_ClearMap.Size = new Size(26, 23);
            button_ClearMap.TabIndex = 7;
            button_ClearMap.Text = "清";
            button_ClearMap.UseVisualStyleBackColor = true;
            button_ClearMap.Click += button_ClearMap_Click;
            // 
            // button_AutoStep
            // 
            button_AutoStep.Location = new Point(658, 23);
            button_AutoStep.Name = "button_AutoStep";
            button_AutoStep.Size = new Size(65, 23);
            button_AutoStep.TabIndex = 10;
            button_AutoStep.Text = "自动";
            button_AutoStep.UseVisualStyleBackColor = true;
            button_AutoStep.Click += button_AutoStep_Click;
            // 
            // button_ToEnd
            // 
            button_ToEnd.Location = new Point(729, 22);
            button_ToEnd.Name = "button_ToEnd";
            button_ToEnd.Size = new Size(65, 23);
            button_ToEnd.TabIndex = 11;
            button_ToEnd.Text = "直接计算";
            button_ToEnd.UseVisualStyleBackColor = true;
            button_ToEnd.Click += button_ToEnd_Click;
            // 
            // button_SetWall
            // 
            button_SetWall.Location = new Point(452, 22);
            button_SetWall.Name = "button_SetWall";
            button_SetWall.Size = new Size(26, 23);
            button_SetWall.TabIndex = 6;
            button_SetWall.Text = "墙";
            button_SetWall.UseVisualStyleBackColor = true;
            button_SetWall.Click += button_SetWall_Click;
            // 
            // button_SetEnd
            // 
            button_SetEnd.Location = new Point(420, 22);
            button_SetEnd.Name = "button_SetEnd";
            button_SetEnd.Size = new Size(26, 23);
            button_SetEnd.TabIndex = 5;
            button_SetEnd.Text = "终";
            button_SetEnd.UseVisualStyleBackColor = true;
            button_SetEnd.Click += button_SetEnd_Click;
            // 
            // button_SetStart
            // 
            button_SetStart.Location = new Point(388, 22);
            button_SetStart.Name = "button_SetStart";
            button_SetStart.Size = new Size(26, 23);
            button_SetStart.TabIndex = 4;
            button_SetStart.Text = "起";
            button_SetStart.UseVisualStyleBackColor = true;
            button_SetStart.Click += button_SetStart_Click;
            // 
            // button_NextStep
            // 
            button_NextStep.Location = new Point(587, 22);
            button_NextStep.Name = "button_NextStep";
            button_NextStep.Size = new Size(65, 23);
            button_NextStep.TabIndex = 9;
            button_NextStep.Text = "下一步";
            button_NextStep.UseVisualStyleBackColor = true;
            button_NextStep.Click += button_NextStep_Click;
            // 
            // checkBox_Ratio
            // 
            checkBox_Ratio.AutoSize = true;
            checkBox_Ratio.Checked = true;
            checkBox_Ratio.CheckState = CheckState.Checked;
            checkBox_Ratio.Location = new Point(192, 23);
            checkBox_Ratio.Name = "checkBox_Ratio";
            checkBox_Ratio.Size = new Size(75, 21);
            checkBox_Ratio.TabIndex = 3;
            checkBox_Ratio.Text = "等比缩放";
            checkBox_Ratio.UseVisualStyleBackColor = true;
            // 
            // label_MapX
            // 
            label_MapX.AutoSize = true;
            label_MapX.Location = new Point(89, 24);
            label_MapX.Name = "label_MapX";
            label_MapX.Size = new Size(14, 17);
            label_MapX.TabIndex = 1;
            label_MapX.Text = "x";
            // 
            // numericUpDown_MapY
            // 
            numericUpDown_MapY.Location = new Point(109, 22);
            numericUpDown_MapY.Maximum = new decimal(new int[] { 25535, 0, 0, 0 });
            numericUpDown_MapY.Minimum = new decimal(new int[] { 9, 0, 0, 0 });
            numericUpDown_MapY.Name = "numericUpDown_MapY";
            numericUpDown_MapY.Size = new Size(77, 23);
            numericUpDown_MapY.TabIndex = 2;
            numericUpDown_MapY.Value = new decimal(new int[] { 9, 0, 0, 0 });
            numericUpDown_MapY.ValueChanged += numericUpDown_MapY_ValueChanged;
            // 
            // numericUpDown_MapX
            // 
            numericUpDown_MapX.Location = new Point(6, 22);
            numericUpDown_MapX.Maximum = new decimal(new int[] { 25535, 0, 0, 0 });
            numericUpDown_MapX.Minimum = new decimal(new int[] { 16, 0, 0, 0 });
            numericUpDown_MapX.Name = "numericUpDown_MapX";
            numericUpDown_MapX.Size = new Size(77, 23);
            numericUpDown_MapX.TabIndex = 0;
            numericUpDown_MapX.Value = new decimal(new int[] { 16, 0, 0, 0 });
            numericUpDown_MapX.ValueChanged += numericUpDown_MapX_ValueChanged;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(824, 531);
            Controls.Add(groupBox_Control);
            Controls.Add(panel_Canvas);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            Name = "MainForm";
            Text = "MainForm";
            Load += MainForm_Load;
            groupBox_Control.ResumeLayout(false);
            groupBox_Control.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)numericUpDown_MapY).EndInit();
            ((System.ComponentModel.ISupportInitialize)numericUpDown_MapX).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Panel panel_Canvas;
        private GroupBox groupBox_Control;
        private Label label_MapX;
        private NumericUpDown numericUpDown_MapY;
        private NumericUpDown numericUpDown_MapX;
        private CheckBox checkBox_Ratio;
        private Button button_NextStep;
        private Button button_ToEnd;
        private Button button_SetWall;
        private Button button_SetEnd;
        private Button button_SetStart;
        private Button button_AutoStep;
        private Button button_ClearMap;
        private Button button_Split;
    }
}