using UnityEngine;

public class Enemy : Player
{
    public Field[,] PlayerBoardGrid { get; set; }
    public int CurrentDifficulty { get; set; }
    private int VerticalCoord { get; set; }
    private int HorizontalCoord { get; set; }
    private int[] AttackCoords { get; set; }
    private int MissOnPurposeChance { get; set; } = 33;

    public int[] DoAnAttack()
    {
        switch (CurrentDifficulty)
        {
            case (int)DifficultyLevel.Baby:
                BabyAI();
                break;

            case (int)DifficultyLevel.Easy:
                EasyAI();
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
    /// Has a small chance to purposefully reroll the coords if they would hit a ship
    /// </summary>
    public void BabyAI()
    {
        GenerateUnusedCoords();
        if (PlayerBoardGrid[VerticalCoord, HorizontalCoord].Type == (int)BoardFieldType.Ship)
        {
            var missCheck = Random.Range(1, 101);
            if (missCheck <= MissOnPurposeChance)
            {
                GenerateUnusedCoords();
            }
        }
    }

    /// <summary>
    /// Simply attack positions that haven't been hit yet
    /// </summary>
    public void EasyAI()
    {
        GenerateUnusedCoords();
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
            int _vert = Random.Range(board.FirstGridPos, board.LastVerticalGridPos);
            int _hor = Random.Range(board.FirstGridPos, board.LastHorizontalGridPos);
            if (PlayerBoardGrid[_vert, _hor].Type != (int)BoardFieldType.Mishit
                && PlayerBoardGrid[_vert, _hor].Type != (int)BoardFieldType.Shipwreck)
            {
                VerticalCoord = _vert;
                HorizontalCoord = _hor;
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