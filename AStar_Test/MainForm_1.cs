using System.DirectoryServices.ActiveDirectory;
using Astar.Base;
using Astar.Map;
using Astar.Main;
using Core;
using Extension;

namespace TestWinForm
{
    public partial class MainForm_1 : Form
    {
        private BaseMap_1 Map { get; set; }
        private BaseAstar Astar { get; set; }
        private List<Point> LastStep { get; set; }
        private SettingType SettingType { get; set; }
        private Thread? AutoStepThread { get; set; }
        private CancellationTokenSource AutoStepCancellationTokenSource { get; set; }
        private CancellationToken AutoStepCancellationToken { get; set; }
        private long Steps { get; set; }
        public MainForm_1()
        {
            InitializeComponent();

            DoubleBuffered = true;

            Map = new BaseMap_1((int)numericUpDown_MapX.Value, (int)numericUpDown_MapY.Value);
            Astar = new BaseAstar(MapNodeTree.Create(Map), new Point(), new Point(Map.Width - 1, Map.Height - 1));
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
            var perLineX = (panel_Canvas.Width - 2) / numericUpDown_MapX.Value;
            var perLineY = (panel_Canvas.Height - 2) / numericUpDown_MapY.Value;

            using var buffer = new Bitmap(panel_Canvas.Width, panel_Canvas.Height);

            using (var pen = new Pen(Color.Black, 1))
            using (var bg = Graphics.FromImage(buffer))
            {
                bg.Clear(panel_Canvas.BackColor);

                var x = Map.Walls.GetLength(0);
                var y = Map.Walls.GetLength(1);
                for (var i = 0; i < x; i++)
                {
                    for (var j = 0; j < y; j++)
                    {
                        if (Map.Walls[i, j])
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

                for (var i = 1; i < numericUpDown_MapX.Value; i++)
                {
                    bg.DrawLine(pen, new PointF((float)(perLineX * i), 0), new PointF((float)(perLineX * i), panel_Canvas.Height));
                }
                for (var i = 1; i < numericUpDown_MapY.Value; i++)
                {
                    bg.DrawLine(pen, new PointF(0, (float)(perLineY * i)), new PointF(panel_Canvas.Width, (float)(perLineY * i)));
                }

                pen.Color = Color.Blue;
                pen.Width = 2;

                var rects = Map.GetRectangles();
                for (var i = 0; i < rects.Count; i++)
                {
                    bg.DrawRectangle(pen, (float)(perLineX * rects[i].Left), (float)(perLineY * rects[i].Top), (float)(perLineX * rects[i].Width), (float)(perLineY * rects[i].Height));
                }

                bg.DrawString($"MouseState: {SettingType}", new Font("Microsoft YaHei", 9), new SolidBrush(Color.Black), new Point());
                bg.DrawString(Steps.ToString(), new Font("Microsoft YaHei", 9, FontStyle.Bold), new SolidBrush(Color.Black), new Point(0, panel_Canvas.Height - 16));
            }

            using var g = panel_Canvas.CreateGraphics();
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
            var perLineX = (decimal)(panel_Canvas.Width - 2) / Map.Width;
            var perLineY = (decimal)(panel_Canvas.Height - 2) / Map.Height;
            return new Point((int)(x / perLineX), (int)(y / perLineY));
        }

        private void ResetMap()
        {
            var oldWidth = Map.Width;
            var oldHeight = Map.Height;
            Map.ResetSize((int)numericUpDown_MapX.Value, (int)numericUpDown_MapY.Value);
            Map.ResetPoint(
                new Point(
                    (int)((Astar.StartPoint.X + 0.5m) / oldWidth * numericUpDown_MapX.Value),
                    (int)((Astar.StartPoint.Y + 0.5m) / oldHeight * numericUpDown_MapY.Value)),
                new Point(
                    (int)((Astar.EndPoint.X + 0.5m) / oldWidth * numericUpDown_MapX.Value),
                    (int)((Astar.EndPoint.Y + 0.5m) / oldHeight * numericUpDown_MapY.Value)));
            Astar.Reset(MapNodeTree.Create(Map), Astar.StartPoint, Astar.EndPoint);
            RenderMap();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            RenderMap();
        }

        private void numericUpDown_MapX_ValueChanged(object sender, EventArgs e)
        {
            if (checkBox_Ratio.Checked)
            {
                numericUpDown_MapY.Value = CalcRatioNumber((int)numericUpDown_MapX.Value, null, 16d / 9);
            }

            Astar.ResetCalculation();
            Steps = 0;
            LastStep.Clear();
            ResetMap();
        }

        private void numericUpDown_MapY_ValueChanged(object sender, EventArgs e)
        {
            if (checkBox_Ratio.Checked)
            {
                numericUpDown_MapX.Value = CalcRatioNumber(null, (int)numericUpDown_MapY.Value, 16d / 9);
            }

            Astar.ResetCalculation();
            Steps = 0;
            LastStep.Clear();
            ResetMap();
        }

        private void button_SetStart_Click(object sender, EventArgs e)
        {
            SettingType = SettingType.Start;
            RenderMap();
        }

        private void button_SetEnd_Click(object sender, EventArgs e)
        {
            SettingType = SettingType.End;
            RenderMap();
        }

        private void button_SetWall_Click(object sender, EventArgs e)
        {
            SettingType = SettingType == SettingType.Wall
                ? SettingType.None
                : SettingType.Wall;
            RenderMap();
        }

        private void button_ClearMap_Click(object sender, EventArgs e)
        {
            Map.ClearWall();
            Astar.ResetCalculation();
            LastStep.Clear();
            RenderMap();
        }

        private void button_Split_Click(object sender, EventArgs e)
        {
            Map.CalcRectangles();
            RenderMap();
        }

        private void button_NextStep_Click(object sender, EventArgs e)
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

        private void button_AutoStep_Click(object sender, EventArgs e)
        {
            var time = 600;

            if (AutoStepThread?.IsAlive == true)
            {
                button_SetStart.Enabled = true;
                button_SetEnd.Enabled = true;
                button_SetWall.Enabled = true;
                button_NextStep.Enabled = true;
                button_ToEnd.Enabled = true;
                button_AutoStep.Text = "自动";
                AutoStepCancellationTokenSource.Cancel();
                return;
            }

            AutoStepCancellationTokenSource = new CancellationTokenSource();
            AutoStepCancellationToken = AutoStepCancellationTokenSource.Token;

            SettingType = SettingType.None;
            button_SetStart.Enabled = false;
            button_SetEnd.Enabled = false;
            button_SetWall.Enabled = false;
            button_NextStep.Enabled = false;
            button_ToEnd.Enabled = false;
            button_AutoStep.Text = "取消";
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

                    Thread.Sleep(time / Map.Width);
                }

                Invoke(() =>
                {
                    button_SetStart.Enabled = true;
                    button_SetEnd.Enabled = true;
                    button_SetWall.Enabled = true;
                    button_NextStep.Enabled = true;
                    button_ToEnd.Enabled = true;
                    button_AutoStep.Text = "自动";
                });
                MessageBox.Show("执行完毕");
            })
            {
                IsBackground = true
            };
            AutoStepThread.Start();
        }

        private void button_ToEnd_Click(object sender, EventArgs e)
        {
            var flag = Astar.TryCalcResult(out var lastStep);
            LastStep = lastStep ?? new List<Point>();
            RenderMap();

            if (!flag)
            {
                MessageBox.Show("执行失败");
            }
        }

        private void panel_Canvas_MouseDown(object sender, MouseEventArgs e)
        {
            if (SettingType == SettingType.Start)
            {
                var point = GetBlockPoint(GetCursorToControl(panel_Canvas));
                Map.ResetPoint(point);
                Astar.ResetStartPoint(point);
                Steps = 0;
                LastStep.Clear();
                SettingType = SettingType.None;
            }
            else
            if (SettingType == SettingType.End)
            {
                var point = GetBlockPoint(GetCursorToControl(panel_Canvas));
                Map.ResetPoint(null, point);
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
                    Map.DrawWall(GetBlockPoint(GetCursorToControl(panel_Canvas)), true);
                    Astar.Reset(MapNodeTree.Create(Map));
                    Steps = 0;
                    LastStep.Clear();
                }
                if (e.Button == MouseButtons.Right)
                {
                    Map.DrawWall(GetBlockPoint(GetCursorToControl(panel_Canvas)), false);
                    Astar.Reset(MapNodeTree.Create(Map));
                    Steps = 0;
                    LastStep.Clear();
                }
            }

            RenderMap();
        }

        private void panel_Canvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (SettingType != SettingType.Wall)
            {
                RenderMap();
                return;
            }

            if (e.Button == MouseButtons.Left)
            {
                Map.DrawWall(GetBlockPoint(GetCursorToControl(panel_Canvas)), true);
                Astar.Reset(MapNodeTree.Create(Map));
                Steps = 0;
                LastStep.Clear();
                RenderMap();
                return;
            }
            if (e.Button == MouseButtons.Right)
            {
                Map.DrawWall(GetBlockPoint(GetCursorToControl(panel_Canvas)), false);
                Astar.Reset(MapNodeTree.Create(Map));
                Steps = 0;
                LastStep.Clear();
                RenderMap();
                return;
            }
        }

        private void panel_Canvas_Paint(object sender, PaintEventArgs e)
        {
            RenderMap();
        }
    }
}