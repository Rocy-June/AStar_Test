using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Extension
{
    public static class RectangleExtension
    {
        public static bool IsNearingWith(this Rectangle rectThis, Rectangle rectThat, bool cornerAllow = false)
        {
            return 
                rectThis.Top == rectThat.Bottom &&
                    (cornerAllow 
                        ? rectThis.Left <= rectThat.Right && rectThis.Right >= rectThat.Left 
                        : rectThis.Left < rectThat.Right && rectThis.Right > rectThat.Left) ||
                rectThis.Bottom == rectThat.Top && 
                    (cornerAllow
                        ? rectThis.Left <= rectThat.Right && rectThis.Right >= rectThat.Left
                        : rectThis.Left < rectThat.Right && rectThis.Right > rectThat.Left) ||
                rectThis.Left == rectThat.Right &&
                    (cornerAllow
                        ? rectThis.Top <= rectThat.Bottom && rectThis.Bottom >= rectThat.Top
                        : rectThis.Top < rectThat.Bottom && rectThis.Bottom > rectThat.Top) ||
                rectThis.Right == rectThat.Left &&
                    (cornerAllow
                        ? rectThis.Top <= rectThat.Bottom && rectThis.Bottom >= rectThat.Top
                        : rectThis.Top < rectThat.Bottom && rectThis.Bottom > rectThat.Top);
        }
    }
}
