using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Astar.Base;
using Astar.Map;
using Astar.Tool;

namespace Astar.Main.SegmentationAstar
{
    public sealed class SegmentationMapAstar : BaseAstar
    {
        public NetMap Map { get; private set; }
        public List<Rectangle> Rectangles { get; private set; }
        public int Level { get; private set; } = -1;
        public double LevelStep { get; private set; }

        public SegmentationMapAstar(int width, int height, double levelStep = 2)
            : this(width, height, new Point(), new Point(width - 1, height - 1), levelStep)
        {
        }

        public SegmentationMapAstar(int width, int height, Point startPoint, Point endPoint, double levelStep = 2)
            : this(new NetMap(width, height, startPoint, endPoint), levelStep)
        {
            StartNode = new PathNode(StartPoint);
            NodeQueue = new List<PathNode>
            {
                StartNode
            };
        }

        public SegmentationMapAstar(NetMap map, double levelStep = 2) : base(map.StartPoint, map.EndPoint)
        {
            Map = map;
            LevelStep = levelStep;
            Rectangles = MapSegmentation.GetDoubleMoveableRectangles(Map.Walls);
            NodeTree = new MapNodeTree(Rectangles, map.StartPoint, map.EndPoint);
            StartNode = new PathNode(map.StartPoint);
            NodeQueue = new List<PathNode>
            {
                StartNode
            };
        }

        public override bool TryNextStep(out List<Point>? result)
        {
            if (Level == -1 && !NextLevel())
            {
                result = null;
                return false;
            }

            var flag = base.TryNextStep(out result);
            if (!flag)
            {
                if (NextLevel())
                {
                    return base.TryNextStep(out result);
                }

                return false;
            }

            return flag;
        }

        public override bool TryCalcResult(out List<Point>? result)
        {
            result = null;
            while (true)
            {
                var flag = base.TryCalcResult(out var tmpResult);
                if (flag)
                {
                    if (!NextLevel())
                    {
                        result = tmpResult;
                        return true;
                    }

                    continue;
                }

                return false;
            }
        }

        public override void CalcResult(out List<Point> result)
        {
            base.CalcResult(out result);
        }

        private bool NextLevel()
        {
            if (Level == -1)
            {
                Level = Map.Width / 4;
                Reset(Level);

                return true;
            }
            if (Level <= 5)
            {
                return false;
            }

            Level = (int)(Level / LevelStep);
            Reset(Level);
            return true;
        }

        public void Reset()
        {
            Level = -1;
            Rectangles = MapSegmentation.GetDoubleMoveableRectangles(Map.Walls);
            Reset(new MapNodeTree(Rectangles, Map.StartPoint, Map.EndPoint));
        }

        public void Reset(int level)
        {
            Rectangles = MapSegmentation.GetDoubleMoveableRectangles(Map.Walls, level);
            Reset(new MapNodeTree(Rectangles, Map.StartPoint, Map.EndPoint));
        }

        public override void ResetCalculation()
        {
            Reset();
        }

    }
}
