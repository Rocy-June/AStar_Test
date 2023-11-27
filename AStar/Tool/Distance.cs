using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Astar.Tool
{
    public class Distance
    {
        /// <summary>
        /// 获取曼哈顿距离
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns></returns>
        public static double GetManhattan(Point from, Point to)
        {
            return Math.Abs(to.X - from.X) + Math.Abs(to.Y - from.Y);
        }

        /// <summary>
        /// 获取欧几里得距离
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns></returns>
        public static double GetEuclidean(Point from, Point to)
        {
            return Math.Sqrt
            (
                Math.Pow(Math.Abs(from.X - to.X), 2) +
                Math.Pow(Math.Abs(from.Y - to.Y), 2)
            );
        }
    }
}
