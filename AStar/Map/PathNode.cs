using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Astar.Map
{
    [DebuggerDisplay("Location = {Location.X},{Location.Y}; Steps = {Location.Steps.ToString(\"F2\")}")]
    public class PathNode
    {
        internal Point Location { get; private set; }
        internal double Steps { get; private set; }
        internal PathNode? PrevNode { get; private set; }
        internal List<PathNode> NextNodes { get; private set; }

        internal PathNode(int x, int y, PathNode? from = null, double cost = 1) : this(new Point(x, y), from, cost)
        {

        }

        internal PathNode(Point location, PathNode? from = null, double cost = 1)
        {
            Location = location;
            PrevNode = from;
            NextNodes = new List<PathNode>();

            from?.AddNode(this, cost);
        }

        public static List<Point> GetFullPath(PathNode? pathNode)
        {
            var result = new List<Point>();
            if (pathNode == null)
            {
                return result;
            }

            do
            {
                result.Insert(0, pathNode.Location);
                pathNode = pathNode?.PrevNode;
            }
            while (pathNode != null);

            return result;
        }

        public ReadOnlyNode GetReadOnlyNode()
        {
            return ReadOnlyNode.Build(this);
        }

        internal void AddNode(PathNode pathNode, double cost)
        {
            pathNode.PrevNode = this;
            pathNode.Steps = Steps + cost;
            NextNodes.Add(pathNode);
        }
    }
}
