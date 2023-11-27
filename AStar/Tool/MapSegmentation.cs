using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Astar.Tool
{
    internal static class MapSegmentation
    {
        private static Rectangle GetMaxRectangle(bool[,] map, int x, int y, bool[,] visited)
        {
            var mapWidth = map.GetLength(0);
            var mapHeight = map.GetLength(1);
            var rectWidth = 0;
            var rectHeight = 1;
            while (x + rectWidth < mapWidth)
            {
                if (map[x + rectWidth, y] || visited[x + rectWidth, y])
                {
                    break;
                }

                rectWidth++;
            }
            ConfirmVisited(visited, x, y, rectWidth);

            for (var j = y + 1; j < mapHeight; j++)
            {
                if (!NoVisitedOrWallLine(map, x, j, rectWidth, visited))
                {
                    break;
                }

                rectHeight++;
                ConfirmVisited(visited, x, j, rectWidth);
            }

            return new Rectangle(x, y, rectWidth, rectHeight);
        }

        private static void ConfirmVisited(bool[,] visited, int x, int y, int width)
        {
            for (var i = 0; i < width; i++)
            {
                visited[x + i, y] = true;
            }
        }

        private static bool NoVisitedOrWallLine(bool[,] map, int x, int y, int width, bool[,] visited)
        {
            for (var i = 0; i < width; i++)
            {
                if (map[x + i, y] || visited[x + i, y])
                {
                    return false;
                }
            }

            return true;
        }

        private static bool TryFindNewStartPoint(bool[,] map, bool[,] visited, out int x, out int y)
        {
            x = 0;
            y = 0;

            var width = map.GetLength(0);
            var height = map.GetLength(1);
            for (var i = 0; i < width; i++)
            {
                for (var j = 0; j < height; j++)
                {
                    if (!map[i, j] && !visited[i, j])
                    {
                        x = i;
                        y = j;
                        return true;
                    }
                }
            }

            return false;
        }

        public static List<Rectangle> GetMoveableRectangles(bool[,] map)
        {
            var visited = new bool[map.GetLength(0), map.GetLength(1)];
            var rectangles = new List<Rectangle>();

            while (TryFindNewStartPoint(map, visited, out var x, out var y))
            {
                rectangles.Add(GetMaxRectangle(map, x, y, visited));
            }

            return rectangles;
        }
    }
}
