using System;
using System.Collections.Generic;

namespace FruitBasketGame
{
    class PlayerCreator
    {
        public static Player CreatePlayer(string name, PlayerType type, List<Guess> guesses)
        {
            Player player;
            switch (type)
            {
                case PlayerType.Random:
                    player = new RandomPlayer();
                    break;
                case PlayerType.Memory:
                    player = new MemoryPlayer();
                    break;
                case PlayerType.Thorough:
                    player = new ThoroughPlayer();
                    break;
                case PlayerType.Cheater:
                    player = new CheaterPlayer();
                    break;
                case PlayerType.ThoroughCheater:
                    player = new ThoroughCheaterPlayer();
                    break;
                default:
                    throw new ArgumentException();
            };
            player.Name = name;
            player.Type = type;

            if (player is ICheater cheater)
                cheater.Guesses = guesses;

            return player;
        }
    }
}
