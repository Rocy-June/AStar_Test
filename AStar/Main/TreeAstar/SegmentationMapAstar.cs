using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Astar.Base;
using Astar.Map;
using Astar.Tool;

namespace Astar.Main.TreeAstar
{
    public sealed class SegmentationMapAstar : BaseAstar
    {
        private NetMap Map { get; set; }
        public int Level { get; private set; } = -1; // 从 width / 4 开始, 直到 <= 3
        public int LevelStep { get; private set; }

        public SegmentationMapAstar(int width, int height, int levelStep)
            : this(width, height, new Point(), new Point(width - 1, height - 1), levelStep)
        {
        }

        public SegmentationMapAstar(int width, int height, Point startPoint, Point endPoint, int levelStep)
            : this(new NetMap(width, height, startPoint, endPoint), levelStep)
        {
            StartNode = new PathNode(StartPoint);
            NodeQueue = new List<PathNode>
            {
                StartNode
            };
        }

        public SegmentationMapAstar(NetMap map, int levelStep) : base()
        {
            Map = map;
            LevelStep = levelStep;
        }

        public override bool TryNextStep(out List<Point>? result)
        {
            var flag = base.TryNextStep(out result);
            if (!flag && !NextLevel())
            {
                return false;
            }

            return base.TryNextStep(out result);
        }

        public override bool TryCalcResult(out List<Point>? result)
        {
            result = null;
            while (true)
            {
                var flag = base.TryCalcResult(out var tmpResult);
                if (!flag && !NextLevel())
                {
                    return false;
                }
                result = tmpResult;
                NextLevel();
            }
        }

        public override void CalcResult(out List<Point> result)
        {
            base.CalcResult(out result);
        }

        private bool NextLevel()
        {
            if (Level <= 3)
            {
                return false;
            }
            if (Level == -1)
            {
                Level = Map.Width / 4;
                Reset(new MapNodeTree(MapSegmentation.GetDoubleMoveableRectangles(Map.Walls, Level)));

                return true;
            }

            Level /= 2;
            Reset(new MapNodeTree(MapSegmentation.GetDoubleMoveableRectangles(Map.Walls, Level)));
            return true;
        }

        public void Reset()
        {
            Level = -1;
            Reset(new MapNodeTree(MapSegmentation.GetDoubleMoveableRectangles(Map.Walls)));
        }

    }
}
