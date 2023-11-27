using System.Diagnostics;

namespace Core
{
    public static class Game
    {
        private static Thread CalcThread { get; set; }
        static Game()
        {
            CalcThread = new Thread(PhysicThreadMethod);
            CalcThread.Start();
        }

        private static void PhysicThreadMethod()
        {
            var sw = new Stopwatch();

            sw.Start();
            Start();
            sw.Stop();

            var prevTime = sw.ElapsedMilliseconds;

            while (true)
            {
                sw.Restart();

                try
                {
                    Update(prevTime);
                }
                catch { }

                sw.Stop();
                prevTime = sw.ElapsedMilliseconds;

            }
        }

        private static void Start()
        {

        }

        private static void Update(double time)
        {

        }
    }
}