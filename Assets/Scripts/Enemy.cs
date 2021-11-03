using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Player
{
    public int[,] PlayerBoardGrid { get; set; }
    public int CurrentDifficulty { get; set; } = 0;
    int VerticalCoord { get; set; }
    int HorizontalCoord { get; set; }
    int[] AttackCoords { get; set; }
    int MissOnPurposeChance { get; set; } = 33;

    public int[] Attack()
    {
        switch (CurrentDifficulty)
        {
            case 1:
                BabyAI();
                break;

            case 2:
                EasyAI();
                break;

            case 3:
                NormalAI();
                break;

            case 4:
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
        GenerateCoordsWhichHaventBeenUsed();
        if (PlayerBoardGrid[VerticalCoord, HorizontalCoord] == 1)
        {
            var missCheck = Random.Range(1, 101);
            if (missCheck <= MissOnPurposeChance)
            {
                GenerateCoordsWhichHaventBeenUsed();
            }
        }
    }

    /// <summary>
    /// This AI intelligence level will simply attack positions that haven't been hit yet
    /// </summary>
    public void EasyAI()
    {
        GenerateCoordsWhichHaventBeenUsed();
    }

    /// <summary>
    /// This AI intelligence level will: attack positions that aren't near to other already hit positions
    /// </summary>
    public void NormalAI()
    {
        GenerateStrategicCoordsWithUnpopulatedNeighbours();
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
            GenerateStrategicCoordsWithUnpopulatedNeighbours();
        }

    }

    public void GenerateRandomCoordinates()
    {
        VerticalCoord = Random.Range(board.FirstGridPos, board.LastVerticalGridPos);
        HorizontalCoord = Random.Range(board.FirstGridPos, board.LastHorizontalGridPos);
    }

    public void GenerateCoordsWhichHaventBeenUsed()
    {
        GenerateRandomCoordinates();
        while (true)
        {
            if (PlayerBoardGrid[VerticalCoord, HorizontalCoord] == 2 || PlayerBoardGrid[VerticalCoord, HorizontalCoord] == 3)
            {
                GenerateRandomCoordinates();
            }
            else
            {
                break;
            }
        }

    }

    public void GenerateStrategicCoordsWithUnpopulatedNeighbours()
    {
        GenerateCoordsWhichHaventBeenUsed();
        while (CheckIfThereAreNeighbours())
        {
            GenerateCoordsWhichHaventBeenUsed();
        }
    }

    public void GeneratePopulatedCoords()
    {
        while (PlayerBoardGrid[VerticalCoord, HorizontalCoord] != 1)
        {
            GenerateStrategicCoordsWithUnpopulatedNeighbours();
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
