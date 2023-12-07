using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Astar.Main;
using Astar.Tool;
using Core;
using Extension;

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
            var dicNodes = new Dictionary<Point, MapNode>();

            var startRect = rectangles.First(e => e.Contains(startPoint));
            var endRect = rectangles.First(e => e.Contains(endPoint));

            dicNodes.TryAdd(startPoint, new MapNode
            {
                Bounds = startRect,
                Location = startPoint
            });
            dicNodes.TryAdd(endPoint, new MapNode
            {
                Bounds = endRect,
                Location = endPoint
            });

            // let the rectangle side nodes become a new node,
            // unless there already has a node,
            // then judge the edge let the nodes have multiple position type

            foreach (var rect in rectangles)
            {
                var tmpLocation = new Point(rect.Left, rect.Top);
                dicNodes.TryAdd(tmpLocation, new MapNode
                {
                    Bounds = rect,
                    Location = tmpLocation
                });

                tmpLocation = new Point(rect.Right - 1, rect.Top);
                dicNodes.TryAdd(tmpLocation, new MapNode
                {
                    Bounds = rect,
                    Location = tmpLocation
                });

                tmpLocation = new Point(rect.Left, rect.Bottom - 1);
                dicNodes.TryAdd(tmpLocation, new MapNode
                {
                    Bounds = rect,
                    Location = tmpLocation
                });

                tmpLocation = new Point(rect.Right - 1, rect.Bottom - 1);
                dicNodes.TryAdd(tmpLocation, new MapNode
                {
                    Bounds = rect,
                    Location = tmpLocation
                });

                var tmpNearing = rectangles.Count(e => rect.Top == e.Bottom && rect.Left < e.Right && rect.Right > e.Left);
                var tmpSplits = tmpNearing + 1m;
                var tmpPerSec = rect.Width / tmpSplits;
                for (var i = 1; i < tmpSplits; i++)
                {
                    tmpLocation = new Point((int)(rect.Left + tmpPerSec * i), rect.Top);
                    dicNodes.TryAdd(tmpLocation, new MapNode
                    {
                        Bounds = rect,
                        Location = tmpLocation
                    });
                }

                tmpNearing = rectangles.Count(e => rect.Bottom == e.Top && rect.Left < e.Right && rect.Right > e.Left);
                tmpSplits = tmpNearing + 1m;
                tmpPerSec = rect.Width / tmpSplits;
                for (var i = 1; i < tmpSplits; i++)
                {
                    tmpLocation = new Point((int)(rect.Left + tmpPerSec * i), rect.Bottom - 1);
                    dicNodes.TryAdd(tmpLocation, new MapNode
                    {
                        Bounds = rect,
                        Location = tmpLocation
                    });
                }

                tmpNearing = rectangles.Count(e => rect.Left == e.Right && rect.Top < e.Bottom && rect.Bottom > e.Top);
                tmpSplits = tmpNearing + 1m;
                tmpPerSec = rect.Height / tmpSplits;
                for (var i = 1; i < tmpSplits; i++)
                {
                    tmpLocation = new Point(rect.Left, (int)(rect.Top + tmpPerSec * i));
                    dicNodes.TryAdd(tmpLocation, new MapNode
                    {
                        Bounds = rect,
                        Location = tmpLocation
                    });
                }

                tmpNearing = rectangles.Count(e => rect.Right == e.Left && rect.Top < e.Bottom && rect.Bottom > e.Top);
                tmpSplits = tmpNearing + 1m;
                tmpPerSec = rect.Height / tmpSplits;
                for (var i = 1; i < tmpSplits; i++)
                {
                    tmpLocation = new Point(rect.Right - 1, (int)(rect.Top + tmpPerSec * i));
                    dicNodes.TryAdd(tmpLocation, new MapNode
                    {
                        Bounds = rect,
                        Location = tmpLocation
                    });
                }
            }

            var nodes = dicNodes.Select(e => e.Value).ToList();
            foreach (var node in nodes)
            {
                node.PositionType =
                    (node.Location.X == node.Bounds.Left ? NodePositionType.Left : 0) |
                    (node.Location.X == node.Bounds.Right - 1 ? NodePositionType.Right : 0) |
                    (node.Location.Y == node.Bounds.Top ? NodePositionType.Top : 0) |
                    (node.Location.Y == node.Bounds.Bottom - 1 ? NodePositionType.Bottom : 0);
            }
            foreach (var node in nodes)
            {
                node.NearingNodes.AddRange(nodes.Where(e => e.Bounds == node.Bounds && e != node));

                var tmp = nodes
                    .Where(e =>
                        e.Bounds != node.Bounds &&
                        e.Bounds.IsNearingWith(node.Bounds, true) &&
                        Distance.GetEuclidean(e.Location, node.Location) < 2)
                    .ToList();
                node.NearingNodes.AddRange(tmp);

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
