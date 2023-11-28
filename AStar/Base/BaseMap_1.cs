using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Astar.Tool;

namespace Astar.Base
{
    public class BaseMap_1
    {
        public int Width { get; private set; }
        public int Height { get; private set; }
        public Point StartPoint { get; private set; }
        public Point EndPoint { get; private set; }
        public bool[,] Walls { get; private set; }
        private bool[,] DetectedAreas { get; set; }
        internal Point[] Offsets { get; } = new Point[]
        {
            new Point(1, 0),
            new Point(0, 1),
            new Point(-1, 0),
            new Point(0, -1),
            new Point(1, -1),
            new Point(1, 1),
            new Point(-1, 1),
            new Point(-1, -1),
        };
        private List<Rectangle> Rectangles { get; set; }

        public BaseMap_1(int width, int height) : this(width, height, new Point(), new Point(width - 1, height - 1)) { }
        public BaseMap_1(int width, int height, Point startPoint, Point endPoint)
        {
            Width = width;
            Height = height;
            StartPoint = startPoint;
            EndPoint = endPoint;
            Walls = new bool[width, height];
            DetectedAreas = new bool[width, height];
            Rectangles = new List<Rectangle>();
        }

        public void ResetSize(int? width = null, int? height = null)
        {
            var oldWidth = Width;
            var oldHeight = Height;

            Width = width ?? Width;
            Height = height ?? Height;

            var tmpWalls = new bool[Width, Height];
            for (var i = 0; i < width; i++)
            {
                for (var j = 0; j < height; j++)
                {
                    tmpWalls[i, j] = Walls[(int)((i + 0.5m) / Width * oldWidth), (int)((j + 0.5m) / Height * oldHeight)];
                }
            }
            Walls = tmpWalls;

            ResetPath();
        }

        public void ResetPoint(Point? startPoint = null, Point? endPoint = null)
        {
            StartPoint = startPoint ?? StartPoint;
            EndPoint = endPoint ?? EndPoint;

            ResetPath();
        }

        public virtual void ResetPath()
        {
            DetectedAreas = new bool[Width, Height];
        }

        public void DrawWall(Point p, bool isWall = true)
        {
            DrawWall(p.X, p.Y, isWall);
        }
        public void DrawWall(int x, int y, bool isWall = true)
        {
            try { Walls[x, y] = isWall; } catch { }

            ResetPath();
        }

        public void ClearWall()
        {
            Walls = new bool[Width, Height];

            ResetPath();
        }

        protected void SetDetectBlock(Point p, bool isDetected = true)
        {
            SetDetectBlock(p.X, p.Y, isDetected);
        }

        protected void SetDetectBlock(int x, int y, bool isDetected = true)
        {
            DetectedAreas[x, y] = isDetected;
        }

        public bool IsDetectedOrWall(Point point)
        {
            return IsDetectedOrWall(point.X, point.Y);
        }

        public bool IsDetectedOrWall(int x, int y)
        {
            if (x < 0 || y < 0 || x >= Width || y >= Height)
            {
                return true;
            }

            return DetectedAreas[x, y] || Walls[x, y];
        }

        public void CalcRectangles()
        {
            Rectangles = MapSegmentation.GetMoveableRectangles(Walls);
        }

        public List<Rectangle> GetRectangles()
        {
            var result = new List<Rectangle>();
            foreach (var rect in Rectangles)
            {
                result.Add(rect);
            }
            return result;
        }
    }
}
