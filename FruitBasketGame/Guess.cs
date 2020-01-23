namespace FruitBasketGame
{
    struct Guess
    {
        public Guess(string name, int value)
        {
            this.PlayerName = name;
            this.Value = value;
        }
        public string PlayerName;
        public int Value;
    }
}
