using System;
using System.Collections.Generic;
using System.Linq;

namespace FruitBasketGame
{
    interface ICheater
    {
        List<Guess> Guesses { get; set; }
    }

    abstract class Player
    {
        public string Name { get; set; }
        public PlayerType Type { get; set; }
        protected Random random = new Random();
        public abstract int Guess();
    }

    class RandomPlayer : Player
    {
        public override int Guess()
        {
            return random.Next(41, 140);
        }
    }

    class MemoryPlayer : Player
    {
        HashSet<int> MemoryPlayerGuesses { get; set; } = new HashSet<int>();
        public override int Guess()
        {
            while (true)
            {
                var number = random.Next(41, 140);
                if (!MemoryPlayerGuesses.Contains(number))
                {
                    MemoryPlayerGuesses.Add(number);
                    return number;
                }
            }
        }
    }

    class ThoroughPlayer : Player
    {
        int number = 41;
        public override int Guess()
        {
            return number++;
        }
    }

    class CheaterPlayer : Player, ICheater
    {
        public List<Guess> Guesses { get; set; }
        public override int Guess()
        {
            while (true)
            {
                var number = random.Next(41, 140);
                if (!Guesses.Select(x => x.Value).Contains(number))
                    return number;
            }
        }
    }

    class ThoroughCheaterPlayer : Player, ICheater
    {
        int number = 41;
        public List<Guess> Guesses { get; set; }
        public override int Guess()
        {
            if (!Guesses.Select(x => x.Value).Contains(number))
                return number++;

            while (true)
            {
                if (!Guesses.Select(x => x.Value).Contains(++number))
                    return number++;
            }
        }
    }
}
