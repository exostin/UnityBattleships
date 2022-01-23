namespace Classes
{
    public class Player
    {
        public Board board = new Board();

        public int[] ChooseShipsConfiguration(int configurationIndex)
        {
            switch (configurationIndex)
            {
                case 1:
                    return new int[] { 1, 2, 3, 4, 5 };

                case 2:
                    return new int[] { 2, 2, 3, 4, 0 };

                case 3:
                    return new int[] { 3, 2, 4, 0, 0 };

                case 4:
                    return new int[] { 7, 0, 0, 0, 0 };

                case 5:
                    return new int[] { 0, 0, 0, 0, 35 };

                default:
                    return new int[] { 1, 2, 3, 4, 5 };
            }
        }
    }
}