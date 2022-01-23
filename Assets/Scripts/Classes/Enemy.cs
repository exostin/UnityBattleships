using Enums;
using UnityEngine;

namespace Classes
{
    public class Enemy : Player
    {
        public Field[,] PlayerBoardGrid { get; set; }
        public int CurrentDifficulty { get; set; }
        private int VerticalCoord { get; set; }
        private int HorizontalCoord { get; set; }
        private int[] AttackCoords { get; set; }
        private int MissOnPurposeChance { get; set; } = 33;

        public int[] LastPlayerAttackCoords { get; set; }

        public int[] DoAnAttack()
        {
            switch (CurrentDifficulty)
            {
                case (int)DifficultyLevel.Dumb:
                    DumbAI();
                    break;

                case (int)DifficultyLevel.Normal:
                    NormalAI();
                    break;

                case (int)DifficultyLevel.Impossible:
                    ImpossibleAI();
                    break;

                default:
                    NormalAI();
                    break;
            }
            AttackCoords = new int[] { VerticalCoord, HorizontalCoord };
            return AttackCoords;
        }

        /// <summary>
        /// Mimic player attacks
        /// </summary>
        public void DumbAI()
        {
            int vert = LastPlayerAttackCoords[0];
            int hor = LastPlayerAttackCoords[1];

            if (PlayerBoardGrid[vert, hor].Type != (int)BoardFieldType.Mishit
                && PlayerBoardGrid[vert, hor].Type != (int)BoardFieldType.Shipwreck)
            {
                VerticalCoord = vert;
                HorizontalCoord = hor;
            }
            else
            {
                GenerateUnusedCoords();
            }
        }

        /// <summary>
        /// Attack positions that aren't near to other already hit positions
        /// </summary>
        public void NormalAI()
        {
            GenerateUnusedCoordsWithUnpopulatedNeighbours();
        }

        /// <summary>
        /// Generate a guaranteed hit coordinates and have a small chance to reroll it
        /// </summary>
        public void ImpossibleAI()
        {
            GeneratePopulatedCoords();

            var missCheck = Random.Range(1, 101);
            if (missCheck <= MissOnPurposeChance)
            {
                GenerateUnusedCoordsWithUnpopulatedNeighbours();
            }
        }

        public void GenerateUnusedCoords()
        {
            while (true)
            {
                int vert = Random.Range(board.FirstGridPos, board.LastVerticalGridPos);
                int hor = Random.Range(board.FirstGridPos, board.LastHorizontalGridPos);
                if (PlayerBoardGrid[vert, hor].Type != (int)BoardFieldType.Mishit
                    && PlayerBoardGrid[vert, hor].Type != (int)BoardFieldType.Shipwreck)
                {
                    VerticalCoord = vert;
                    HorizontalCoord = hor;
                    break;
                }
            }
        }

        public void GenerateUnusedCoordsWithUnpopulatedNeighbours()
        {
            GenerateUnusedCoords();
            while (CheckForNeighbouringShipwrecks())
            {
                GenerateUnusedCoords();
            }
        }

        public void GeneratePopulatedCoords()
        {
            while (PlayerBoardGrid[VerticalCoord, HorizontalCoord].Type != (int)BoardFieldType.Ship)
            {
                GenerateUnusedCoordsWithUnpopulatedNeighbours();
            }
        }

        public bool CheckForNeighbouringShipwrecks()
        {
            if ((PlayerBoardGrid[VerticalCoord + 1, HorizontalCoord].Type != (int)BoardFieldType.Shipwreck) &&
                (PlayerBoardGrid[VerticalCoord - 1, HorizontalCoord].Type != (int)BoardFieldType.Shipwreck) &&
                (PlayerBoardGrid[VerticalCoord, HorizontalCoord + 1].Type != (int)BoardFieldType.Shipwreck) &&
                (PlayerBoardGrid[VerticalCoord, HorizontalCoord - 1].Type != (int)BoardFieldType.Shipwreck) &&
                (PlayerBoardGrid[VerticalCoord - 1, HorizontalCoord + 1].Type != (int)BoardFieldType.Shipwreck) &&
                (PlayerBoardGrid[VerticalCoord - 1, HorizontalCoord - 1].Type != (int)BoardFieldType.Shipwreck) &&
                (PlayerBoardGrid[VerticalCoord + 1, HorizontalCoord + 1].Type != (int)BoardFieldType.Shipwreck) &&
                (PlayerBoardGrid[VerticalCoord + 1, HorizontalCoord - 1].Type != (int)BoardFieldType.Shipwreck))
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}