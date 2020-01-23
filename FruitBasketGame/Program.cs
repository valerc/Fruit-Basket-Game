using System;

namespace FruitBasketGame
{
    class Program
    {
        static void Main(string[] args)
        {
            bool exit;
            do
            {
                var game = new Game();
                game.Play();

                Console.WriteLine("Exit - press E button");
                Console.WriteLine("Continue - press any other button");
                exit = Console.ReadKey().Key == ConsoleKey.E;
            } while (!exit);
        }
    }
}
