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
        public Dictionary<Point, MapNode> MapNodes { get; set; } // todo: turn it to internal.
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

            foreach (var rect in rectangles)
            {
                // todo: let the rect. side nodes become a new node, except there already has a node, then judge the edge let the nodes have multiple position type

                if (rect.Width == 1)
                {
                    var location = new Point(rect.X, rect.Y + rect.Height / 2);
                    var node = nodes.FirstOrDefault(e => e.Location == location);
                    if (node != default)
                    {
                        node.PositionType |= NodePositionType.Left | NodePositionType.Right;
                    }
                    else
                    {
                        nodes.Add(new MapNode
                        {
                            Bounds = rect,
                            Location = location,
                            PositionType = NodePositionType.Left | NodePositionType.Right,
                        });
                    }
                }
                else
                {
                    var leftLocation = new Point(rect.X, rect.Y + rect.Height / 2);
                    var leftNode = nodes.FirstOrDefault(e => e.Location == leftLocation);
                    if (leftNode != default)
                    {
                        leftNode.PositionType |= NodePositionType.Left;
                    }
                    else
                    {
                        nodes.Add(new MapNode
                        {
                            Bounds = rect,
                            Location = leftLocation,
                            PositionType = NodePositionType.Left
                        });
                    }

                    var rightLocation = new Point(rect.X + rect.Width - 1, rect.Y + rect.Height / 2);
                    var rightNode = nodes.FirstOrDefault(e => e.Location == rightLocation);
                    if (rightNode != default)
                    {
                        rightNode.PositionType |= NodePositionType.Right;
                    }
                    else
                    {
                        nodes.Add(new MapNode
                        {
                            Bounds = rect,
                            Location = rightLocation,
                            PositionType = NodePositionType.Right
                        });
                    }
                }

                if (rect.Height == 1)
                {
                    var location = new Point(rect.X + rect.Width / 2, rect.Y);
                    var node = nodes.FirstOrDefault(e => e.Location == location);
                    if (node != default)
                    {
                        node.PositionType |= NodePositionType.Top | NodePositionType.Bottom;
                    }
                    else
                    {
                        nodes.Add(new MapNode
                        {
                            Bounds = rect,
                            Location = location,
                            PositionType = NodePositionType.Top | NodePositionType.Bottom
                        });
                    }
                }
                else
                {
                    var topLocation = new Point(rect.X + rect.Width / 2, rect.Y);
                    var topNode = nodes.FirstOrDefault(e => e.Location == topLocation);
                    if (topNode != default)
                    {
                        topNode.PositionType |= NodePositionType.Top;
                    }
                    else
                    {

                        nodes.Add(new MapNode
                        {
                            Bounds = rect,
                            Location = topLocation,
                            PositionType = NodePositionType.Top
                        });
                    }

                    var bottomLocation = new Point(rect.X + rect.Width / 2, rect.Y + rect.Height - 1);
                    var bottomNode = nodes.FirstOrDefault(e => e.Location == bottomLocation);
                    if (bottomNode != default)
                    {
                        bottomNode.PositionType |= NodePositionType.Bottom;
                    }
                    else
                    {
                        nodes.Add(new MapNode
                        {
                            Bounds = rect,
                            Location = bottomLocation,
                            PositionType = NodePositionType.Bottom
                        });
                    }
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

            if (!nodes.Any(e => e.Location == startPoint))
            {
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
            }
            if (!nodes.Any(e => e.Location == endPoint))
            {
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
