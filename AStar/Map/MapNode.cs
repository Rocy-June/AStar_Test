using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core;

namespace Astar.Map
{
    [DebuggerDisplay("→{Bounds.X},↓{Bounds.Y},{Bounds.Width}x{Bounds.Height} (Node = {Location.X},{Location.Y} {PositionTypeString()})")]
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
        public NodePositionType PositionType { get; set; }
        public MapNode()
        {
            NearingNodes = new List<MapNode>();
        }

        private string PositionTypeString()
        {
            var typeStr = new List<string>(4);
            if ((PositionType & NodePositionType.Top) > 0)
            {
                typeStr.Add("Top");
            }
            if ((PositionType & NodePositionType.Bottom) > 0)
            {
                typeStr.Add("Bottom");
            }
            if ((PositionType & NodePositionType.Left) > 0)
            {
                typeStr.Add("Left");
            }
            if ((PositionType & NodePositionType.Right) > 0)
            {
                typeStr.Add("Right");
            }

            return string.Join(", ", typeStr);
        }
    }
}
