using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Astar.Base;
using Astar.Map;
using Astar.Tool;

namespace Astar.Main
{
    public class NetMapAstar : BaseMap
    {
        public PathNode StartNode { get; private set; }
        private List<PathNode> NodeQueue { get; set; }

        public NetMapAstar(int width, int height) : this(width, height, new Point(), new Point(width - 1, height - 1))
        {
        }
        public NetMapAstar(int width, int height, Point startPoint, Point endPoint) : base(width, height, startPoint, endPoint)
        {
            StartNode = new PathNode(startPoint);
            NodeQueue = new List<PathNode>
            {
                StartNode
            };
        }

        public List<Point> GetQueuePoints()
        {
            return NodeQueue.Select(e => e.Location).ToList();
        }

        public bool TryNextStep(out List<Point>? result)
        {
            var flag = false;
            try
            {
                flag = NextStep(out var endNode);
                result = PathNode.GetFullPath(endNode);
            }
            catch
            {
                result = null;
            }

            return flag;
        }

        public bool TryCalcResult(out List<Point>? result)
        {
            try
            {
                CalcResult(out result);
                return true;
            }
            catch
            {
                result = null;
                return false;
            }
        }

        public void CalcResult(out List<Point> result)
        {
            PathNode? lastNode = null;
            var flag = true;
            while (flag)
            {
                flag = NextStep(out var outNode);
                lastNode = outNode;
            }
            result = PathNode.GetFullPath(lastNode);
        }

        private bool NextStep(out PathNode pathNode)
        {
            if (NodeQueue.Count <= 0)
            {
                throw new NoWayToGoException();
            }

            pathNode = NodeQueue[0];
            if (pathNode.Location == EndPoint)
            {
                return false;
            }

            var minDistance = CalcDistance(pathNode.Location, EndPoint);
            var minCost = minDistance + pathNode.Steps;
            for (var i = 1; i < NodeQueue.Count; i++)
            {
                if (NodeQueue[i].Location == EndPoint)
                {
                    pathNode = NodeQueue[i];
                    return false;
                }

                var nodeDistance = CalcDistance(NodeQueue[i].Location, EndPoint);
                var nodeCost = nodeDistance + NodeQueue[i].Steps;
                if (nodeDistance < minDistance && nodeCost < minCost)
                {
                    pathNode = NodeQueue[i];
                    minDistance = nodeDistance;
                    minCost = nodeCost;
                }
            }

            SetDetectBlock(pathNode.Location);
            NodeQueue.Remove(pathNode);
            foreach (var offset in Offsets)
            {
                var x = pathNode.Location.X + offset.X;
                var y = pathNode.Location.Y + offset.Y;
                var cost = Distance.GetEuclidean(pathNode.Location, new Point(x, y));
                if (IsDetectedOrWall(x, y))
                {
                    continue;
                }

                var queueNode = NodeQueue.Find(e => e.Location.X == x && e.Location.Y == y);
                if (queueNode == null)
                {
                    NodeQueue.Add(new PathNode(x, y, pathNode, cost));
                }
                else
                {
                    var newNode = new PathNode(x, y, pathNode, cost);
                    if (queueNode.Steps > newNode.Steps)
                    {
                        NodeQueue.Remove(queueNode);
                        NodeQueue.Add(newNode);
                    }
                }
            }

            return true;
        }

        public static double CalcDistance(Point from, Point to)
        {
            //return Distance.GetEuclidean(from, to);
            return Distance.GetManhattan(from, to);
        }

        public override void ResetPath()
        {
            StartNode = new PathNode(StartPoint);
            NodeQueue = new List<PathNode>
            {
                StartNode
            };

            base.ResetPath();
        }
    }
}
