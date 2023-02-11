namespace BRIX.Library.DiceValue
{
    public class Dice
    {
        public Dice() { }

        public Dice(int numberOfFaces, int count = 1)
        {
            NumberOfFaces = numberOfFaces;
            Count = count;
        }

        public Dice(EStandartDice numberOfFaces, int count = 1)
        {
            NumberOfFaces = (int)numberOfFaces;
            Count = count;
        }

        public int NumberOfFaces { get; set; }

        public int Count { get; set; }
    }
}
