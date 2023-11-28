using System.DirectoryServices.ActiveDirectory;
using Astar.Base;
using Astar.Map;
using Astar.Main;
using Core;
using Extension;
using Astar.Main.NetAstar;

namespace TestWinForm
{
    public partial class MainForm : Form
    {
        private NetMapAstar Astar { get; set; }
        private List<Point> LastStep { get; set; }
        private SettingType SettingType { get; set; }
        private Thread? AutoStepThread { get; set; }
        private CancellationTokenSource AutoStepCancellationTokenSource { get; set; }
        private CancellationToken AutoStepCancellationToken { get; set; }
        private long Steps { get; set; }
        public MainForm()
        {
            InitializeComponent();

            DoubleBuffered = true;

            Astar = new NetMapAstar(
                new NetMap(
                    (int)NumericUpDown_MapX.Value,
                    (int)NumericUpDown_MapY.Value,
                    new Point(),
                    new Point((int)NumericUpDown_MapX.Value - 1, (int)NumericUpDown_MapY.Value - 1)));
            LastStep = new List<Point>();

            AutoStepCancellationTokenSource = new CancellationTokenSource();
            AutoStepCancellationToken = AutoStepCancellationTokenSource.Token;
        }

        private int CalcRatioNumber(int? x, int? y, double ratio)
        {
            if (x != null)
            {
                return (x.Value / ratio).ToInt32();
            }
            if (y != null)
            {
                return (y.Value * ratio).ToInt32();
            }

            return 0;
        }

        private void RenderMap()
        {
            var perLineX = (Panel_Canvas.Width - 2) / NumericUpDown_MapX.Value;
            var perLineY = (Panel_Canvas.Height - 2) / NumericUpDown_MapY.Value;

            using var buffer = new Bitmap(Panel_Canvas.Width, Panel_Canvas.Height);

            using (var pen = new Pen(Color.Black, 1))
            using (var bg = Graphics.FromImage(buffer))
            {
                bg.Clear(Panel_Canvas.BackColor);

                var x = Astar.Map.Walls.GetLength(0);
                var y = Astar.Map.Walls.GetLength(1);
                for (var i = 0; i < x; i++)
                {
                    for (var j = 0; j < y; j++)
                    {
                        if (Astar.Map.Walls[i, j])
                        {
                            bg.FillRectangle(
                                new SolidBrush(Color.DarkSlateGray),
                                new RectangleF(
                                    new PointF((float)(i * perLineX), (float)(j * perLineY)),
                                    new SizeF((float)perLineX, (float)perLineY)));
                        }
                    }
                }

                var readOnlyNode = Astar.StartNode.GetReadOnlyNode();
                DrawPathNode(bg, perLineX, perLineY, readOnlyNode);

                foreach (var point in LastStep)
                {
                    bg.FillRectangle(
                                new SolidBrush(Color.HotPink),
                                new RectangleF(
                                    new PointF((float)(point.X * perLineX), (float)(point.Y * perLineY)),
                                    new SizeF((float)perLineX, (float)perLineY)));
                }

                var queuePoints = Astar.GetQueuePoints();
                foreach (var point in queuePoints)
                {
                    bg.FillRectangle(
                                new SolidBrush(Color.DeepSkyBlue),
                                new RectangleF(
                                    new PointF((float)(point.X * perLineX), (float)(point.Y * perLineY)),
                                    new SizeF((float)perLineX, (float)perLineY)));
                }

                bg.FillRectangle(
                    new SolidBrush(Color.LightGreen),
                    new RectangleF(
                        new PointF((float)(Astar.StartPoint.X * perLineX), (float)(Astar.StartPoint.Y * perLineY)),
                        new SizeF((float)perLineX, (float)perLineY)));
                bg.FillRectangle(
                    new SolidBrush(Color.Red),
                    new RectangleF(
                        new PointF((float)(Astar.EndPoint.X * perLineX), (float)(Astar.EndPoint.Y * perLineY)),
                        new SizeF((float)perLineX, (float)perLineY)));

                for (var i = 1; i < NumericUpDown_MapX.Value; i++)
                {
                    bg.DrawLine(pen, new PointF((float)(perLineX * i), 0), new PointF((float)(perLineX * i), Panel_Canvas.Height));
                }
                for (var i = 1; i < NumericUpDown_MapY.Value; i++)
                {
                    bg.DrawLine(pen, new PointF(0, (float)(perLineY * i)), new PointF(Panel_Canvas.Width, (float)(perLineY * i)));
                }

                pen.Color = Color.Blue;
                pen.Width = 2;

                var rects = Astar.Map.GetRectangles();
                for (var i = 0; i < rects.Count; i++)
                {
                    bg.DrawRectangle(pen, (float)(perLineX * rects[i].Left), (float)(perLineY * rects[i].Top), (float)(perLineX * rects[i].Width), (float)(perLineY * rects[i].Height));
                }

                bg.DrawString($"MouseState: {SettingType}", new Font("Microsoft YaHei", 9), new SolidBrush(Color.Black), new Point());
                bg.DrawString(Steps.ToString(), new Font("Microsoft YaHei", 9, FontStyle.Bold), new SolidBrush(Color.Black), new Point(0, Panel_Canvas.Height - 16));
            }

            using var g = Panel_Canvas.CreateGraphics();
            g.DrawImage(buffer, new Point());
        }
        private void DrawPathNode(Graphics bg, decimal width, decimal height, ReadOnlyNode node)
        {
            bg.FillRectangle(
                new SolidBrush(Color.DarkGray),
                new RectangleF(
                    new PointF((float)(node.Location.X * width), (float)(node.Location.Y * height)),
                    new SizeF((float)width, (float)height)));

            foreach (var cNode in node.NextNodes)
            {
                DrawPathNode(bg, width, height, cNode);
            }
        }

        private Point GetCursorToControl(Control c)
        {
            var p = Cursor.Position;
            p.X = p.X - Location.X - c.Location.X - 9;
            p.Y = p.Y - Location.Y - c.Location.Y - 32;
            return p;
        }

        private Point GetBlockPoint(Point p)
        {
            return GetBlockPoint(p.X, p.Y);
        }
        private Point GetBlockPoint(int x, int y)
        {
            var perLineX = (decimal)(Panel_Canvas.Width - 2) / Astar.Map.Width;
            var perLineY = (decimal)(Panel_Canvas.Height - 2) / Astar.Map.Height;
            return new Point((int)(x / perLineX), (int)(y / perLineY));
        }

        private void ResetMap()
        {
            var oldWidth = Astar.Map.Width;
            var oldHeight = Astar.Map.Height;
            Astar.Map.ResetSize((int)NumericUpDown_MapX.Value, (int)NumericUpDown_MapY.Value);
            Astar.Map.ResetPoint(
                new Point(
                    (int)((Astar.StartPoint.X + 0.5m) / oldWidth * NumericUpDown_MapX.Value),
                    (int)((Astar.StartPoint.Y + 0.5m) / oldHeight * NumericUpDown_MapY.Value)),
                new Point(
                    (int)((Astar.EndPoint.X + 0.5m) / oldWidth * NumericUpDown_MapX.Value),
                    (int)((Astar.EndPoint.Y + 0.5m) / oldHeight * NumericUpDown_MapY.Value)));
            Astar.Reset();
            RenderMap();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            RenderMap();
        }

        private void NumericUpDown_MapX_ValueChanged(object sender, EventArgs e)
        {
            if (CheckBox_Ratio.Checked)
            {
                NumericUpDown_MapY.Value = CalcRatioNumber((int)NumericUpDown_MapX.Value, null, 16d / 9);
            }

            Astar.ResetCalculation();
            Steps = 0;
            LastStep.Clear();
            ResetMap();
        }

        private void NumericUpDown_MapY_ValueChanged(object sender, EventArgs e)
        {
            if (CheckBox_Ratio.Checked)
            {
                NumericUpDown_MapX.Value = CalcRatioNumber(null, (int)NumericUpDown_MapY.Value, 16d / 9);
            }

            Astar.ResetCalculation();
            Steps = 0;
            LastStep.Clear();
            ResetMap();
        }

        private void Button_SetStart_Click(object sender, EventArgs e)
        {
            SettingType = SettingType.Start;
            RenderMap();
        }

        private void Button_SetEnd_Click(object sender, EventArgs e)
        {
            SettingType = SettingType.End;
            RenderMap();
        }

        private void Button_SetWall_Click(object sender, EventArgs e)
        {
            SettingType = SettingType == SettingType.Wall
                ? SettingType.None
                : SettingType.Wall;
            RenderMap();
        }

        private void Button_ClearMap_Click(object sender, EventArgs e)
        {
            Astar.Map.ClearWall();
            Astar.ResetCalculation();
            LastStep.Clear();
            RenderMap();
        }

        private void Button_Split_Click(object sender, EventArgs e)
        {
            Astar.Map.CalcRectangles();
            RenderMap();
        }

        private void Button_NextStep_Click(object sender, EventArgs e)
        {
            SettingType = SettingType.None;

            var flag = Astar.TryNextStep(out var lastStep);
            LastStep = lastStep ?? new List<Point>();
            RenderMap();

            if (!flag)
            {
                MessageBox.Show("执行完毕");
            }
        }

        private void Button_AutoStep_Click(object sender, EventArgs e)
        {
            var time = 600;

            if (AutoStepThread?.IsAlive == true)
            {
                Button_SetStart.Enabled = true;
                Button_SetEnd.Enabled = true;
                Button_SetWall.Enabled = true;
                Button_NextStep.Enabled = true;
                Button_ToEnd.Enabled = true;
                Button_AutoStep.Text = "自动";
                AutoStepCancellationTokenSource.Cancel();
                return;
            }

            AutoStepCancellationTokenSource = new CancellationTokenSource();
            AutoStepCancellationToken = AutoStepCancellationTokenSource.Token;

            SettingType = SettingType.None;
            Button_SetStart.Enabled = false;
            Button_SetEnd.Enabled = false;
            Button_SetWall.Enabled = false;
            Button_NextStep.Enabled = false;
            Button_ToEnd.Enabled = false;
            Button_AutoStep.Text = "取消";
            AutoStepThread = new Thread(() =>
            {
                var flag = true;
                while (!AutoStepCancellationToken.IsCancellationRequested && flag)
                {
                    Steps++;
                    Invoke(() =>
                    {
                        flag = Astar.TryNextStep(out var lastStep);
                        LastStep = lastStep ?? new List<Point>();
                        RenderMap();
                    });

                    Thread.Sleep(time / Astar.Map.Width);
                }

                Invoke(() =>
                {
                    Button_SetStart.Enabled = true;
                    Button_SetEnd.Enabled = true;
                    Button_SetWall.Enabled = true;
                    Button_NextStep.Enabled = true;
                    Button_ToEnd.Enabled = true;
                    Button_AutoStep.Text = "自动";
                });
                MessageBox.Show("执行完毕");
            })
            {
                IsBackground = true
            };
            AutoStepThread.Start();
        }

        private void Button_ToEnd_Click(object sender, EventArgs e)
        {
            var flag = Astar.TryCalcResult(out var lastStep);
            LastStep = lastStep ?? new List<Point>();
            RenderMap();

            if (!flag)
            {
                MessageBox.Show("执行失败");
            }
        }

        private void Panel_Canvas_MouseDown(object sender, MouseEventArgs e)
        {
            if (SettingType == SettingType.Start)
            {
                var point = GetBlockPoint(GetCursorToControl(Panel_Canvas));
                Astar.Map.ResetPoint(point);
                Astar.ResetStartPoint(point);
                Steps = 0;
                LastStep.Clear();
                SettingType = SettingType.None;
            }
            else
            if (SettingType == SettingType.End)
            {
                var point = GetBlockPoint(GetCursorToControl(Panel_Canvas));
                Astar.Map.ResetPoint(null, point);
                Astar.ResetEndPoint(point);
                Steps = 0;
                LastStep.Clear();
                SettingType = SettingType.None;
            }
            else
            if (SettingType == SettingType.Wall)
            {
                if (e.Button == MouseButtons.Left)
                {
                    Astar.Map.DrawWall(GetBlockPoint(GetCursorToControl(Panel_Canvas)), true);
                    Astar.Reset();
                    Steps = 0;
                    LastStep.Clear();
                }
                if (e.Button == MouseButtons.Right)
                {
                    Astar.Map.DrawWall(GetBlockPoint(GetCursorToControl(Panel_Canvas)), false);
                    Astar.Reset();
                    Steps = 0;
                    LastStep.Clear();
                }
            }

            RenderMap();
        }

        private void Panel_Canvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (SettingType != SettingType.Wall)
            {
                RenderMap();
                return;
            }

            if (e.Button == MouseButtons.Left)
            {
                Astar.Map.DrawWall(GetBlockPoint(GetCursorToControl(Panel_Canvas)), true);
                Astar.Reset();
                Steps = 0;
                LastStep.Clear();
                RenderMap();
                return;
            }
            if (e.Button == MouseButtons.Right)
            {
                Astar.Map.DrawWall(GetBlockPoint(GetCursorToControl(Panel_Canvas)), false);
                Astar.Reset();
                Steps = 0;
                LastStep.Clear();
                RenderMap();
                return;
            }
        }

        private void Panel_Canvas_Paint(object sender, PaintEventArgs e)
        {
            RenderMap();
        }
    }
}