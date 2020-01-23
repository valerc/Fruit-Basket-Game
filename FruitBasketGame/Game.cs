using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FruitBasketGame
{
    class Game
    {
        int basketWeight;
        object lockObj = new object();
        List<Guess> guesses = new List<Guess>();
        List<Player> players = new List<Player>();
        CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

        public void Play()
        {
            InitPlayers();

            ShowPlayers();

            ThinkNumber();

            StartGuessing();

            ShowResults();
        }

        private void ShowResults()
        {
            var minDiff = guesses.Min(x => Math.Abs(x.Value - basketWeight));
            var wonGuess = guesses.First(x => Math.Abs(x.Value - basketWeight) == minDiff);
            Console.WriteLine($" Winner: {wonGuess.PlayerName}\n Guess: {wonGuess.Value}\n Total amount of attempts: {guesses.Count}");
            Console.WriteLine(new string('-', 30));
            Console.WriteLine();
        }

        private void StartGuessing()
        {
            var tasks = new List<Task>();
            foreach (var player in players)
            {
                tasks.Add(new Task(() =>
                {
                    while (!cancellationTokenSource.Token.IsCancellationRequested)
                    {
                        var guess = player.Guess();
                        lock (lockObj)
                        {
                            guesses.Add(new Guess(player.Name, guess));
                        }
                        if (basketWeight == guess || guesses.Count >= 100)
                        {
                            cancellationTokenSource.Cancel();
                        }
                        var delta = Math.Abs(basketWeight - guess);
                        Thread.Sleep(delta);
                    }
                }));
            }
            tasks.ForEach(x => x.Start());
            cancellationTokenSource.CancelAfter(1500);
            Task.WaitAll(tasks.ToArray());
        }

        private void ThinkNumber()
        {
            basketWeight = new Random().Next(41, 140);
            Console.WriteLine($"Fruit basket weight is {basketWeight} kilo(s)");
            Console.WriteLine(new string('-', 30));
            Console.WriteLine();
        }

        private void ShowPlayers()
        {
            Console.Clear();
            Console.WriteLine("Players:");
            foreach (var player in players)
            {
                Console.WriteLine($"\t{player.Name} ({player.Type})");
            }
            Console.WriteLine(new string('-', 30));
            Console.WriteLine();
        }

        private void InitPlayers()
        {
            int amount;
            bool correct;
            do
            {
                Console.Clear();
                Console.WriteLine("Enter the amount of participating players (2 through 8):");
                var amountStr = Console.ReadLine();
                correct = int.TryParse(amountStr, out amount) && amount >= 2 && amount <= 8;
            } while (!correct);

            for (int i = 0; i < amount; i++)
            {
                string name;
                int typeNum;
                do
                {
                    Console.Clear();
                    Console.WriteLine($"Enter the name of the player #{i + 1}:");
                    name = Console.ReadLine().Trim();
                    correct = name.Any() && !players.Select(x => x.Name).Contains(name);
                } while (!correct);

                do
                {
                    Console.Clear();
                    Console.WriteLine($"Enter the type of the player #{i + 1}:");
                    Console.WriteLine("\t1 - Random type\n\t2 - Memory type\n\t3 - Thorough type\n\t4 - Cheater type\n\t5 - Thorough cheater type");
                    var typeStr = Console.ReadLine();
                    correct = int.TryParse(typeStr, out typeNum) && Enum.IsDefined(typeof(PlayerType), typeNum);
                } while (!correct);

                players.Add(PlayerCreator.CreatePlayer(name, (PlayerType)typeNum, guesses));
            }
        }
    }
}
