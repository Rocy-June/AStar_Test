﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Astar.Map;
using Astar.Tool;

namespace Astar.Base
{
    public class BaseAstar
    {
        public MapNodeTree? NodeTree { get; set; } // todo: turn it to internal.
        public PathNode StartNode { get; protected set; }
        public Point StartPoint { get; private set; }
        public Point EndPoint { get; private set; }
        protected List<PathNode> NodeQueue { get; set; }

        public BaseAstar(MapNodeTree nodeTree, Point startPoint, Point endPoint)
        {
            NodeTree = nodeTree;
            StartNode = new PathNode(startPoint);
            StartPoint = startPoint;
            EndPoint = endPoint;
            NodeQueue = new List<PathNode>
            {
                StartNode
            };
        }
        public BaseAstar(Point startPoint, Point endPoint)
        {
            StartNode = new PathNode(new Point());
            StartPoint = startPoint;
            EndPoint = endPoint;
            NodeQueue = new List<PathNode>();
        }

        public virtual void Reset(MapNodeTree? nodeTree = null, Point? startPoint = null, Point? endPoint = null)
        {
            startPoint ??= StartPoint;
            endPoint ??= EndPoint;

            NodeTree = nodeTree;
            StartNode = new PathNode(startPoint.Value);
            StartPoint = startPoint.Value;
            EndPoint = endPoint.Value;
            NodeQueue = new List<PathNode>
            {
                StartNode
            };

            NodeTree?.ClearDetectedNodes();
        }

        public List<Point> GetQueuePoints()
        {
            return NodeQueue.Select(e => e.Location).ToList();
        }

        public virtual bool TryNextStep(out List<Point>? result)
        {
            var flag = TryNextStep(out PathNode? nodeResult);
            result = PathNode.GetFullPath(nodeResult);

            return flag;
        }

        protected virtual bool TryNextStep(out PathNode? result)
        {
            var flag = false;
            try
            {
                flag = NextStep(out result);
            }
            catch
            {
                result = null;
            }

            return flag;
        }

        public virtual bool TryCalcResult(out List<Point>? result)
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

        public virtual void CalcResult(out List<Point> result)
        {
            CalcResult(out PathNode? lastNode);
            result = PathNode.GetFullPath(lastNode);
        }

        protected virtual void CalcResult(out PathNode? result)
        {
            result = null;

            var flag = true;
            while (flag)
            {
                flag = NextStep(out result);
            }
        }

        private bool NextStep(out PathNode pathNode)
        {
            if (NodeTree == null)
            {
                throw new InvalidOperationException();
            }

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

            NodeTree.SetDetectBlock(pathNode.Location);
            NodeQueue.Remove(pathNode);
            var nextNodes = NodeTree.MapNodes[pathNode.Location].NearingNodes;
            foreach (var node in nextNodes)
            {
                if (NodeTree.NodeDetected.TryGetValue(node.Location, out bool flag) && flag)
                {
                    continue;
                }

                var cost = Distance.GetEuclidean(pathNode.Location, node.Location);
                var queueNode = NodeQueue.Find(e => e.Location == node.Location);
                if (queueNode == null)
                {
                    NodeQueue.Add(new PathNode(node.Location, pathNode, cost));
                }
                else
                {
                    var newNode = new PathNode(node.Location, pathNode, cost);
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

        public virtual void ResetCalculation()
        {
            StartNode = new PathNode(StartPoint);
            NodeQueue = new List<PathNode>
            {
                StartNode
            };

            NodeTree?.ClearDetectedNodes();
        }
    }
}
