using UnityEngine;

public class Enemy : Player
{
    public int[,] PlayerBoardGrid { get; set; }
    public int CurrentDifficulty { get; set; } = 0;
    private int VerticalCoord { get; set; }
    private int HorizontalCoord { get; set; }
    private int[] AttackCoords { get; set; }
    private int MissOnPurposeChance { get; set; } = 33;

    public int[] Attack()
    {
        switch (CurrentDifficulty)
        {
            case 0:
                BabyAI();
                break;

            case 1:
                EasyAI();
                break;

            case 2:
                NormalAI();
                break;

            case 3:
                HardAI();
                break;

            default:
                NormalAI();
                break;
        }
        AttackCoords = new int[] { VerticalCoord, HorizontalCoord };
        return AttackCoords;
    }

    /// <summary>
    /// This AI intelligence level will: has a small chance to purosefully reroll the coords if they would hit a ship
    /// </summary>
    public void BabyAI()
    {
        GenerateUnusedCoords();
        if (PlayerBoardGrid[VerticalCoord, HorizontalCoord] == 1)
        {
            var missCheck = Random.Range(1, 101);
            if (missCheck <= MissOnPurposeChance)
            {
                GenerateUnusedCoords();
            }
        }
    }

    /// <summary>
    /// This AI intelligence level will simply attack positions that haven't been hit yet
    /// </summary>
    public void EasyAI()
    {
        GenerateUnusedCoords();
    }

    /// <summary>
    /// This AI intelligence level will: attack positions that aren't near to other already hit positions
    /// </summary>
    public void NormalAI()
    {
        GenerateUnusedCoordsWithUnpopulatedNeighbours();
    }

    /// <summary>
    /// This AI intelligence level will: generate a guaranteed hit coordinates and have a small chance to generate new ones that don't
    /// </summary>
    public void HardAI()
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
            if (PlayerBoardGrid[_vert, _hor] != 2 && PlayerBoardGrid[_vert, _hor] != 3)
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
        while (CheckIfThereAreNeighbours())
        {
            GenerateUnusedCoords();
        }
    }

    public void GeneratePopulatedCoords()
    {
        while (PlayerBoardGrid[VerticalCoord, HorizontalCoord] != 1)
        {
            GenerateUnusedCoordsWithUnpopulatedNeighbours();
        }
    }

    public bool CheckIfThereAreNeighbours()
    {
        if ((PlayerBoardGrid[VerticalCoord + 1, HorizontalCoord] != 3) &&
            (PlayerBoardGrid[VerticalCoord - 1, HorizontalCoord] != 3) &&
            (PlayerBoardGrid[VerticalCoord, HorizontalCoord + 1] != 3) &&
            (PlayerBoardGrid[VerticalCoord, HorizontalCoord - 1] != 3) &&
            (PlayerBoardGrid[VerticalCoord - 1, HorizontalCoord + 1] != 3) &&
            (PlayerBoardGrid[VerticalCoord - 1, HorizontalCoord - 1] != 3) &&
            (PlayerBoardGrid[VerticalCoord + 1, HorizontalCoord + 1] != 3) &&
            (PlayerBoardGrid[VerticalCoord + 1, HorizontalCoord - 1] != 3))
        {
            return false;
        }
        else
        {
            return true;
        }
    }
}