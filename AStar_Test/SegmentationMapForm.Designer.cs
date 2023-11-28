namespace TestWinForm
{
    partial class SegmentationMapForm
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
            Panel_Canvas = new Panel();
            GroupBox_Control = new GroupBox();
            Button_Split = new Button();
            Button_ClearMap = new Button();
            Button_AutoStep = new Button();
            Button_ToEnd = new Button();
            Button_SetWall = new Button();
            Button_SetEnd = new Button();
            Button_SetStart = new Button();
            Button_NextStep = new Button();
            CheckBox_Ratio = new CheckBox();
            Label_MapX = new Label();
            NumericUpDown_MapY = new NumericUpDown();
            NumericUpDown_MapX = new NumericUpDown();
            ComboBox_SplitType = new ComboBox();
            GroupBox_Control.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)NumericUpDown_MapY).BeginInit();
            ((System.ComponentModel.ISupportInitialize)NumericUpDown_MapX).BeginInit();
            SuspendLayout();
            // 
            // Panel_Canvas
            // 
            Panel_Canvas.BorderStyle = BorderStyle.FixedSingle;
            Panel_Canvas.Location = new Point(12, 69);
            Panel_Canvas.Name = "Panel_Canvas";
            Panel_Canvas.Size = new Size(800, 450);
            Panel_Canvas.TabIndex = 1;
            Panel_Canvas.Paint += Panel_Canvas_Paint;
            Panel_Canvas.MouseDown += Panel_Canvas_MouseDown;
            Panel_Canvas.MouseMove += Panel_Canvas_MouseMove;
            // 
            // GroupBox_Control
            // 
            GroupBox_Control.Controls.Add(ComboBox_SplitType);
            GroupBox_Control.Controls.Add(Button_Split);
            GroupBox_Control.Controls.Add(Button_ClearMap);
            GroupBox_Control.Controls.Add(Button_AutoStep);
            GroupBox_Control.Controls.Add(Button_ToEnd);
            GroupBox_Control.Controls.Add(Button_SetWall);
            GroupBox_Control.Controls.Add(Button_SetEnd);
            GroupBox_Control.Controls.Add(Button_SetStart);
            GroupBox_Control.Controls.Add(Button_NextStep);
            GroupBox_Control.Controls.Add(CheckBox_Ratio);
            GroupBox_Control.Controls.Add(Label_MapX);
            GroupBox_Control.Controls.Add(NumericUpDown_MapY);
            GroupBox_Control.Controls.Add(NumericUpDown_MapX);
            GroupBox_Control.Location = new Point(12, 12);
            GroupBox_Control.Name = "GroupBox_Control";
            GroupBox_Control.Size = new Size(800, 51);
            GroupBox_Control.TabIndex = 0;
            GroupBox_Control.TabStop = false;
            GroupBox_Control.Text = "控制器";
            // 
            // Button_Split
            // 
            Button_Split.Location = new Point(516, 22);
            Button_Split.Name = "Button_Split";
            Button_Split.Size = new Size(65, 23);
            Button_Split.TabIndex = 8;
            Button_Split.Text = "切分";
            Button_Split.UseVisualStyleBackColor = true;
            Button_Split.Click += Button_Split_Click;
            // 
            // Button_ClearMap
            // 
            Button_ClearMap.Location = new Point(369, 22);
            Button_ClearMap.Name = "Button_ClearMap";
            Button_ClearMap.Size = new Size(26, 23);
            Button_ClearMap.TabIndex = 7;
            Button_ClearMap.Text = "清";
            Button_ClearMap.UseVisualStyleBackColor = true;
            Button_ClearMap.Click += Button_ClearMap_Click;
            // 
            // Button_AutoStep
            // 
            Button_AutoStep.Location = new Point(658, 23);
            Button_AutoStep.Name = "Button_AutoStep";
            Button_AutoStep.Size = new Size(65, 23);
            Button_AutoStep.TabIndex = 10;
            Button_AutoStep.Text = "自动";
            Button_AutoStep.UseVisualStyleBackColor = true;
            Button_AutoStep.Click += Button_AutoStep_Click;
            // 
            // Button_ToEnd
            // 
            Button_ToEnd.Location = new Point(729, 22);
            Button_ToEnd.Name = "Button_ToEnd";
            Button_ToEnd.Size = new Size(65, 23);
            Button_ToEnd.TabIndex = 11;
            Button_ToEnd.Text = "直接计算";
            Button_ToEnd.UseVisualStyleBackColor = true;
            Button_ToEnd.Click += Button_ToEnd_Click;
            // 
            // Button_SetWall
            // 
            Button_SetWall.Location = new Point(337, 22);
            Button_SetWall.Name = "Button_SetWall";
            Button_SetWall.Size = new Size(26, 23);
            Button_SetWall.TabIndex = 6;
            Button_SetWall.Text = "墙";
            Button_SetWall.UseVisualStyleBackColor = true;
            Button_SetWall.Click += Button_SetWall_Click;
            // 
            // Button_SetEnd
            // 
            Button_SetEnd.Location = new Point(305, 22);
            Button_SetEnd.Name = "Button_SetEnd";
            Button_SetEnd.Size = new Size(26, 23);
            Button_SetEnd.TabIndex = 5;
            Button_SetEnd.Text = "终";
            Button_SetEnd.UseVisualStyleBackColor = true;
            Button_SetEnd.Click += Button_SetEnd_Click;
            // 
            // Button_SetStart
            // 
            Button_SetStart.Location = new Point(273, 22);
            Button_SetStart.Name = "Button_SetStart";
            Button_SetStart.Size = new Size(26, 23);
            Button_SetStart.TabIndex = 4;
            Button_SetStart.Text = "起";
            Button_SetStart.UseVisualStyleBackColor = true;
            Button_SetStart.Click += Button_SetStart_Click;
            // 
            // Button_NextStep
            // 
            Button_NextStep.Location = new Point(587, 22);
            Button_NextStep.Name = "Button_NextStep";
            Button_NextStep.Size = new Size(65, 23);
            Button_NextStep.TabIndex = 9;
            Button_NextStep.Text = "下一步";
            Button_NextStep.UseVisualStyleBackColor = true;
            Button_NextStep.Click += Button_NextStep_Click;
            // 
            // CheckBox_Ratio
            // 
            CheckBox_Ratio.AutoSize = true;
            CheckBox_Ratio.Checked = true;
            CheckBox_Ratio.CheckState = CheckState.Checked;
            CheckBox_Ratio.Location = new Point(192, 23);
            CheckBox_Ratio.Name = "CheckBox_Ratio";
            CheckBox_Ratio.Size = new Size(75, 21);
            CheckBox_Ratio.TabIndex = 3;
            CheckBox_Ratio.Text = "等比缩放";
            CheckBox_Ratio.UseVisualStyleBackColor = true;
            // 
            // Label_MapX
            // 
            Label_MapX.AutoSize = true;
            Label_MapX.Location = new Point(89, 24);
            Label_MapX.Name = "Label_MapX";
            Label_MapX.Size = new Size(14, 17);
            Label_MapX.TabIndex = 1;
            Label_MapX.Text = "x";
            // 
            // NumericUpDown_MapY
            // 
            NumericUpDown_MapY.Location = new Point(109, 22);
            NumericUpDown_MapY.Maximum = new decimal(new int[] { 25535, 0, 0, 0 });
            NumericUpDown_MapY.Minimum = new decimal(new int[] { 9, 0, 0, 0 });
            NumericUpDown_MapY.Name = "NumericUpDown_MapY";
            NumericUpDown_MapY.Size = new Size(77, 23);
            NumericUpDown_MapY.TabIndex = 2;
            NumericUpDown_MapY.Value = new decimal(new int[] { 9, 0, 0, 0 });
            NumericUpDown_MapY.ValueChanged += NumericUpDown_MapY_ValueChanged;
            // 
            // NumericUpDown_MapX
            // 
            NumericUpDown_MapX.Location = new Point(6, 22);
            NumericUpDown_MapX.Maximum = new decimal(new int[] { 25535, 0, 0, 0 });
            NumericUpDown_MapX.Minimum = new decimal(new int[] { 16, 0, 0, 0 });
            NumericUpDown_MapX.Name = "NumericUpDown_MapX";
            NumericUpDown_MapX.Size = new Size(77, 23);
            NumericUpDown_MapX.TabIndex = 0;
            NumericUpDown_MapX.Value = new decimal(new int[] { 16, 0, 0, 0 });
            NumericUpDown_MapX.ValueChanged += NumericUpDown_MapX_ValueChanged;
            // 
            // ComboBox_SplitType
            // 
            ComboBox_SplitType.DropDownStyle = ComboBoxStyle.DropDownList;
            ComboBox_SplitType.FormattingEnabled = true;
            ComboBox_SplitType.Items.AddRange(new object[] { "横向", "纵向", "时间随机" });
            ComboBox_SplitType.Location = new Point(401, 21);
            ComboBox_SplitType.Name = "ComboBox_SplitType";
            ComboBox_SplitType.Size = new Size(109, 25);
            ComboBox_SplitType.TabIndex = 12;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(824, 531);
            Controls.Add(GroupBox_Control);
            Controls.Add(Panel_Canvas);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            Name = "MainForm";
            Text = "MainForm";
            Load += MainForm_Load;
            GroupBox_Control.ResumeLayout(false);
            GroupBox_Control.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)NumericUpDown_MapY).EndInit();
            ((System.ComponentModel.ISupportInitialize)NumericUpDown_MapX).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Panel Panel_Canvas;
        private GroupBox GroupBox_Control;
        private Label Label_MapX;
        private NumericUpDown NumericUpDown_MapY;
        private NumericUpDown NumericUpDown_MapX;
        private CheckBox CheckBox_Ratio;
        private Button Button_NextStep;
        private Button Button_ToEnd;
        private Button Button_SetWall;
        private Button Button_SetEnd;
        private Button Button_SetStart;
        private Button Button_AutoStep;
        private Button Button_ClearMap;
        private Button Button_Split;
        private ComboBox ComboBox_SplitType;
    }
}