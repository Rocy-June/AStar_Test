using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Astar.Main.NetAstar;

namespace Astar.Map
{
    public class MapNodeTree
    {
        internal Dictionary<Point, MapNode> MapNodes { get; set; }
        internal Dictionary<Point, bool> NodeDetected { get; set; }

        private MapNodeTree()
        {
            MapNodes = new Dictionary<Point, MapNode>();
            NodeDetected = new Dictionary<Point, bool>();
        }

        public MapNodeTree(NetMap map) : this() 
        {
            var nodes = new MapNode[map.Width, map.Height];
            for (var i = 0; i < map.Width; i++)
            {
                for (var j = 0; j < map.Height; j++)
                {
                    if (map.Walls[i, j])
                    {
                        continue;
                    }
                    nodes[i, j] = new MapNode()
                    {
                        Location = new Point(i, j)
                    };
                }
            }
            for (var i = 0; i < map.Width; i++)
            {
                for (var j = 0; j < map.Height; j++)
                {
                    if (nodes[i, j] == null)
                    {
                        continue;
                    }

                    foreach (var offset in map.Offsets)
                    {
                        if (i + offset.X < 0 || i + offset.X >= map.Width ||
                            j + offset.Y < 0 || j + offset.Y >= map.Height ||
                            nodes[i + offset.X, j + offset.Y] == null)
                        {
                            continue;
                        }

                        nodes[i, j].NearingNodes.Add(nodes[i + offset.X, j + offset.Y]);
                    }
                }
            }

            foreach (var node in nodes)
            {
                if (node == null) continue;

                MapNodes.Add(node.Location, node);
            }
        }

        public MapNodeTree(List<Rectangle> rectangles) : this()
        {
            var nodes = rectangles.Select(e => new MapNode { Bounds = e }).ToList();
            foreach (var node in nodes)
            {
                var nearingRects = FindAdjacentRectangles(rectangles, node.Bounds);
                foreach (var nearingRect in nearingRects)
                {
                    var nearingNode = nodes.Find(e => e.Bounds == nearingRect);
                    if (nearingNode == null)
                    {
                        continue;
                    }

                    node.NearingNodes.Add(nearingNode);
                }
            }

            foreach (var node in nodes)
            {
                if (node == null) continue;

                MapNodes.Add(node.Location, node);
            }
        }

        private static List<Rectangle> FindAdjacentRectangles(List<Rectangle> rects, Rectangle detectRect)
        {
            return rects
                .FindAll(e =>
                    e.Top == detectRect.Bottom ||
                    e.Bottom == detectRect.Top ||
                    e.Left == detectRect.Right ||
                    e.Right == detectRect.Left);
        }

        internal void SetDetectBlock(int x, int y, bool isDetected = true)
        {
            SetDetectBlock(new Point(x, y), isDetected);
        }

        internal void SetDetectBlock(Point p, bool isDetected = true)
        {
            NodeDetected[p] = isDetected;
        }

        public void ClearDetectedNodes()
        {
            NodeDetected.Clear();
        }
    }
}
