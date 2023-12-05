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

        public MapNodeTree(List<Rectangle> rectangles, Point startPoint, Point endPoint) : this()
        {
            var nodes = new List<MapNode>();

            var startRect = rectangles.First(e => e.Contains(startPoint));
            var endRect = rectangles.First(e => e.Contains(endPoint));
            nodes.Add(new MapNode
            {
                Bounds = startRect,
                Location = startPoint,
                PositionType =
                    (startRect.Width == 1
                        ? NodePositionType.Left | NodePositionType.Right
                        : startPoint.X < startRect.Width / 2
                            ? NodePositionType.Left
                            : NodePositionType.Right) |
                    (startRect.Height == 1
                        ? NodePositionType.Top | NodePositionType.Bottom
                        : startPoint.Y < startRect.Height / 2
                            ? NodePositionType.Top
                            : NodePositionType.Bottom),
            });
            nodes.Add(new MapNode
            {
                Bounds = endRect,
                Location = endPoint,
                PositionType =
                    (endRect.Width == 1
                        ? NodePositionType.Left | NodePositionType.Right
                        : endRect.X < endRect.Width / 2
                            ? NodePositionType.Left
                            : NodePositionType.Right) |
                    (endRect.Height == 1
                        ? NodePositionType.Top | NodePositionType.Bottom
                        : endRect.Y < endRect.Height / 2
                            ? NodePositionType.Top
                            : NodePositionType.Bottom),
            });

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
                var intersects = nodes.Where(e => 
                    e.Bounds.Left == node.Bounds.Right && e.Bounds.Top < node.Bounds.Bottom && e.Bounds.Bottom > node.Bounds.Top ||
                    e.Bounds.Right == node.Bounds.Left && e.Bounds.Top < node.Bounds.Bottom && e.Bounds.Bottom > node.Bounds.Top ||
                    e.Bounds.Top == node.Bounds.Bottom && e.Bounds.Left < node.Bounds.Right && e.Bounds.Right > node.Bounds.Left ||
                    e.Bounds.Bottom == node.Bounds.Top && e.Bounds.Left < node.Bounds.Right && e.Bounds.Right > node.Bounds.Left);
                if ((node.PositionType & NodePositionType.Top) > 0)
                {
                    intersects = intersects.Where(e => (e.PositionType & NodePositionType.Bottom) > 0 && e.Location.Y + 1 == node.Location.Y);
                }
                if ((node.PositionType & NodePositionType.Bottom) > 0)
                {
                    intersects = intersects.Where(e => (e.PositionType & NodePositionType.Top) > 0 && e.Location.Y - 1 == node.Location.Y);
                }
                if ((node.PositionType & NodePositionType.Left) > 0)
                {
                    intersects = intersects.Where(e => (e.PositionType & NodePositionType.Right) > 0 && e.Location.X + 1 == node.Location.X);
                }
                if ((node.PositionType & NodePositionType.Right) > 0)
                {
                    intersects = intersects.Where(e => (e.PositionType & NodePositionType.Left) > 0 && e.Location.X - 1 == node.Location.X);
                }
                var otherNodes = intersects.ToList();
                node.NearingNodes.AddRange(otherNodes);

                MapNodes.TryAdd(node.Location, node);
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
