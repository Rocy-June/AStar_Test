using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Astar.Map
{
    public class MapNode
    {
        public Point Location { get; set; }
        public Rectangle Bounds
        {
            get { return _Bounds; }
            set
            {
                Location = new Point(value.X + value.Width / 2, value.Y + value.Height / 2);
                _Bounds = value;
            }
        }
        public List<MapNode> NearingNodes { get; set; }
        private Rectangle _Bounds { get; set; }
        public MapNode()
        {
            NearingNodes = new List<MapNode>();
        }
    }
}
