using System;

namespace GuessWurdz
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            using (GuessWurdz game = new GuessWurdz())
            {
                game.Run();
            }
        }
    }
}

