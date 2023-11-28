using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Astar.Base;
using Astar.Map;

namespace Astar.Main.TreeAstar
{
    public class TreeMapAstar : BaseMap_1
    {
        public PathNode StartNode { get; private set; }
        private List<PathNode> NodeQueue { get; set; }

        protected TreeMapAstar(int width, int height) : this(width, height, new Point(), new Point(width - 1, height - 1))
        {
        }

        protected TreeMapAstar(int width, int height, Point startPoint, Point endPoint) : base(width, height, startPoint, endPoint)
        {
            StartNode = new PathNode(StartPoint);
            NodeQueue = new List<PathNode>
            {
                StartNode
            };
        }

    }
}
