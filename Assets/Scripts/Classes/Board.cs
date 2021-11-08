using System.Linq;
using UnityEngine;

public class Board
{
    public int FirstGridPos { get; set; } = 1;
    public int LastVerticalGridPos { get; set; }
    public int LastHorizontalGridPos { get; set; }
    public int[,] GeneratedBoard { get; set; }

    /// <summary>
    /// Populate a board with a defined number of ships of specified type
    /// </summary>
    /// <param name="shipsConfiguration"> {5s, 4s, 3s, 2s, 1s} - how many of which ship to place</param>
    public void PopulateBoard(int[] shipsConfiguration, int vertical, int horizontal)
    {
        GeneratedBoard = new int[vertical, horizontal];
        CheckDimensions();

        // Calculate the lowest possible number of ships that can be placed on the board without overlapping
        // Board surface / area the ship takes being near wall or another ship
        var shipsTotalToPlace = ((vertical - 2) * (horizontal - 2)) / 6;
        if (shipsTotalToPlace < 1) { shipsTotalToPlace = 1; }

        // Loop generating x ships in random locations which cannot overlap or be next to eachother
        for (int i = 0; i < shipsTotalToPlace;)
        {
            while (true)
            {
                var randomVerticalStartPoint = Random.Range(FirstGridPos, LastVerticalGridPos);
                var randomHorizontalStartPoint = Random.Range(FirstGridPos, LastHorizontalGridPos);
                if (!CheckIfTheresSpaceForShip(randomVerticalStartPoint, randomHorizontalStartPoint))
                {
                    GeneratedBoard[randomVerticalStartPoint, randomHorizontalStartPoint] = (int)BoardFieldType.Ship;
                    i++;
                    break;
                }
                else
                {
                    continue;
                }
            }
        }
    }

    /// <summary>
    /// Checks the horizontal and vertical dimensions of the generated board
    /// </summary>
    private void CheckDimensions()
    {
        LastVerticalGridPos = GeneratedBoard.GetLength(0) - 1;
        LastHorizontalGridPos = GeneratedBoard.GetLength(1) - 1;
    }

    /// <summary>
    /// Checks all neighbouring spots for existence of any other ship
    /// </summary>
    /// <param name="vertical">Vertical coordinate to check</param>
    /// <param name="horizontal">Vertical coordinate to check</param>
    /// <returns>true if there is a ship nearby, false if there are none</returns>
    public bool CheckIfTheresSpaceForShip(int vertical, int horizontal)
    {
        if (GeneratedBoard[vertical, horizontal] == (int)BoardFieldType.Empty &&
            (GeneratedBoard[vertical + 1, horizontal] != (int)BoardFieldType.Ship) &&
            (GeneratedBoard[vertical - 1, horizontal] != (int)BoardFieldType.Ship) &&
            (GeneratedBoard[vertical, horizontal + 1] != (int)BoardFieldType.Ship) &&
            (GeneratedBoard[vertical, horizontal - 1] != (int)BoardFieldType.Ship) &&
            (GeneratedBoard[vertical - 1, horizontal + 1] != (int)BoardFieldType.Ship) &&
            (GeneratedBoard[vertical - 1, horizontal - 1] != (int)BoardFieldType.Ship) &&
            (GeneratedBoard[vertical + 1, horizontal + 1] != (int)BoardFieldType.Ship) &&
            (GeneratedBoard[vertical + 1, horizontal - 1] != (int)BoardFieldType.Ship))
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    /// <summary>
    /// Launches an attack at the generated board
    /// </summary>
    /// <param name="verticalCoordinate"></param>
    /// <param name="horizontalCoordinate"></param>
    /// <returns>true if something was hit, false if it was a miss</returns>
    public bool LaunchAttack(int verticalCoordinate, int horizontalCoordinate)
    {
        if (GeneratedBoard[verticalCoordinate, horizontalCoordinate] == (int)BoardFieldType.Ship)
        {
            // Mark that spot as hit
            GeneratedBoard[verticalCoordinate, horizontalCoordinate] = (int)BoardFieldType.Shipwreck;
            return true;
        }
        else
        {
            // Mark that spot as a miss
            GeneratedBoard[verticalCoordinate, horizontalCoordinate] = (int)BoardFieldType.Mishit;
            return false;
        }
    }

    /// <summary>
    /// Checks if the generated board contains any ship (a spot with a value of 1)
    /// </summary>
    public bool CheckIfDefeated()
    {
        if (!GeneratedBoard.Cast<int>().Contains((int)BoardFieldType.Ship))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}