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
        private static Rectangle GetHorizontalMaxRectangle(bool[,] map, int x, int y, bool[,] visited, int maxDistance)
        {
            var mapWidth = map.GetLength(0);
            var mapHeight = map.GetLength(1);
            var rectWidth = 0;
            var rectHeight = 1;
            while (x + rectWidth < mapWidth && rectWidth < maxDistance)
            {
                if (map[x + rectWidth, y] || visited[x + rectWidth, y])
                {
                    break;
                }

                rectWidth++;
            }
            ConfirmHorizontalVisited(visited, x, y, rectWidth);

            for (var j = y + 1; j < mapHeight && rectHeight < maxDistance; j++)
            {
                if (!NoVisitedOrWallLine(map, x, j, rectWidth, visited))
                {
                    break;
                }

                rectHeight++;
                ConfirmHorizontalVisited(visited, x, j, rectWidth);
            }

            return new Rectangle(x, y, rectWidth, rectHeight);
        }

        private static Rectangle GetVerticalMaxRectangle(bool[,] map, int x, int y, bool[,] visited, int maxDistance)
        {
            var mapWidth = map.GetLength(0);
            var mapHeight = map.GetLength(1);
            var rectWidth = 1;
            var rectHeight = 0;
            while (y + rectHeight < mapHeight && rectHeight < maxDistance)
            {
                if (map[x, y + rectHeight] || visited[x, y + rectHeight])
                {
                    break;
                }

                rectHeight++;
            }
            ConfirmVerticalVisited(visited, x, y, rectHeight);

            for (var i = x + 1; i < mapWidth && rectWidth < maxDistance; i++)
            {
                if (!NoVisitedOrWallArrange(map, i, y, rectHeight, visited))
                {
                    break;
                }

                rectWidth++;
                ConfirmVerticalVisited(visited, i, y, rectHeight);
            }

            return new Rectangle(x, y, rectWidth, rectHeight);
        }

        private static void ConfirmHorizontalVisited(bool[,] visited, int x, int y, int width)
        {
            for (var i = 0; i < width; i++)
            {
                visited[x + i, y] = true;
            }
        }

        private static void ConfirmVerticalVisited(bool[,] visited, int x, int y, int height)
        {
            for (var i = 0; i < height; i++)
            {
                visited[x, y + i] = true;
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

        private static bool NoVisitedOrWallArrange(bool[,] map, int x, int y, int height, bool[,] visited)
        {
            for (var i = 0; i < height; i++)
            {
                if (map[x, y + i] || visited[x, y + i])
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

        public static List<Rectangle> GetHorizontalMoveableRectangles(bool[,] map, int maxDistance = int.MaxValue)
        {
            var visited = new bool[map.GetLength(0), map.GetLength(1)];
            var rectangles = new List<Rectangle>();

            while (TryFindNewStartPoint(map, visited, out var x, out var y))
            {
                rectangles.Add(GetHorizontalMaxRectangle(map, x, y, visited, maxDistance));
            }

            return rectangles;
        }

        public static List<Rectangle> GetVerticalMoveableRectangles(bool[,] map, int maxDistance = int.MaxValue)
        {
            var visited = new bool[map.GetLength(0), map.GetLength(1)];
            var rectangles = new List<Rectangle>();

            while (TryFindNewStartPoint(map, visited, out var x, out var y))
            {
                rectangles.Add(GetVerticalMaxRectangle(map, x, y, visited, maxDistance));
            }

            return rectangles;
        }

        public static List<Rectangle> GetDoubleMoveableRectangles(bool[,] map, int maxDistance = int.MaxValue)
        {
            var visited = new bool[map.GetLength(0), map.GetLength(1)];
            var rectangles = new List<Rectangle>();

            while (TryFindNewStartPoint(map, visited, out var x, out var y))
            {
                var rect = DateTime.UtcNow.Ticks % 2 == 0
                    ? GetHorizontalMaxRectangle(map, x, y, visited, maxDistance)
                    : GetVerticalMaxRectangle(map, x, y, visited, maxDistance);
                rectangles.Add(rect);
            }

            return rectangles;
        }
    }
}
