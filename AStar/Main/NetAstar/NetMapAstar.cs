using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Astar.Base;
using Astar.Map;
using Astar.Tool;

namespace Astar.Main.NetAstar
{
    public sealed class NetMapAstar : BaseAstar
    {
        public NetMap Map { get; set; }
        public NetMapAstar(int width, int height) : this(width, height, new Point(), new Point(width - 1, height - 1))
        {
        }
        public NetMapAstar(int width, int height, Point startPoint, Point endPoint) : this(new NetMap(width, height, startPoint, endPoint))
        {
            StartNode = new PathNode(startPoint);
            NodeQueue = new List<PathNode>
            {
                StartNode
            };
        }
        public NetMapAstar(NetMap map) : base(new MapNodeTree(map), map.StartPoint, map.EndPoint)
        {
            Map = map;
        }
        public void Reset() 
        {
            Reset(new MapNodeTree(Map));
        }
    }
}
