using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Astar.Base;
using Astar.Main.NetAstar;
using Astar.Map;

namespace Astar.Main.TreeAstar
{
    public sealed class SegmentationMapAstar : BaseAstar
    {
        public NetMap Map { get; set; }

        public SegmentationMapAstar(int width, int height) : this(width, height, new Point(), new Point(width - 1, height - 1))
        {
        }

        public SegmentationMapAstar(int width, int height, Point startPoint, Point endPoint) : this(new NetMap(width, height, startPoint, endPoint))
        {
            StartNode = new PathNode(StartPoint);
            NodeQueue = new List<PathNode>
            {
                StartNode
            };
        }

        public SegmentationMapAstar(NetMap map) : base()
        {
            Map = map;


        }

        public void Reset()
        {
            Reset(new MapNodeTree(Map));
        }

    }
}
