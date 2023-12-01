using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Astar.Main;
using Core;

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
            var nodes = new List<MapNode>();
            foreach (var rect in rectangles)
            {
                if (rect.Width == 1)
                {
                    nodes.Add(new MapNode
                    {
                        Bounds = rect,
                        Location = new Point(rect.X, rect.Y + rect.Height / 2),
                        PositionType = NodePositionType.Left | NodePositionType.Right,
                    });
                }
                else
                {
                    nodes.Add(new MapNode
                    {
                        Bounds = rect,
                        Location = new Point(rect.X, rect.Y + rect.Height / 2),
                        PositionType = NodePositionType.Left
                    });
                    nodes.Add(new MapNode
                    {
                        Bounds = rect,
                        Location = new Point(rect.X + rect.Width - 1, rect.Y + rect.Height / 2),
                        PositionType = NodePositionType.Right
                    });
                }



                if (rect.Height == 1)
                {
                    nodes.Add(new MapNode
                    {
                        Bounds = rect,
                        Location = new Point(rect.X + rect.Width / 2, rect.Y),
                        PositionType = NodePositionType.Top | NodePositionType.Bottom
                    });
                }
                else
                {
                    nodes.Add(new MapNode
                    {
                        Bounds = rect,
                        Location = new Point(rect.X + rect.Width / 2, rect.Y),
                        PositionType = NodePositionType.Top
                    });
                    nodes.Add(new MapNode
                    {
                        Bounds = rect,
                        Location = new Point(rect.X + rect.Width / 2, rect.Y + rect.Height - 1),
                        PositionType = NodePositionType.Bottom
                    });
                }
            }
            foreach (var node in nodes)
            {
                node.NearingNodes.AddRange(nodes.Where(e => e.Bounds == node.Bounds && e != node));
                node.NearingNodes.AddRange(
                    nodes.Where(e =>
                        e.Bounds.IntersectsWith(node.Bounds) &&
                        e.Bounds.Top == node.Bounds.Bottom + 1 ||
                        e.Bounds.Bottom == node.Bounds.Top - 1 ||
                        e.Bounds.Left == node.Bounds.Right + 1 ||
                        e.Bounds.Right == node.Bounds.Left - 1));

                MapNodes.Add(node.Location, node);
            }

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
