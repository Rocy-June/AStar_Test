using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Astar.Map
{
    public class ReadOnlyNode
    {
        public Point Location { get; private set; }
        public double Steps { get; private set; }
        public ReadOnlyNode? PrevNode { get; private set; }
        public List<ReadOnlyNode> NextNodes { get; private set; }

        private ReadOnlyNode()
        {
            NextNodes = new List<ReadOnlyNode>();
        }

        public static ReadOnlyNode Build(PathNode startNode)
        {
            if (startNode.PrevNode != null)
            {
                return Build(startNode.PrevNode);
            }

            return CreateNode(startNode);
        }

        private static ReadOnlyNode CreateNode(PathNode node, ReadOnlyNode? prevNode = null)
        {
            var tmp = new ReadOnlyNode
            {
                Location = node.Location,
                Steps = node.Steps,
                PrevNode = prevNode,
            };
            tmp.NextNodes = node.NextNodes.Select(e => CreateNode(e, tmp)).ToList();

            return tmp;
        }
    }
}
