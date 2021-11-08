using System.Linq;
using UnityEngine;

public class Board
{
    public int FirstGridPos { get; set; } = 1;
    public int LastVerticalGridPos { get; set; }
    public int LastHorizontalGridPos { get; set; }
    public Field[,] BoardFields { get; set; }

    private const int defaultBoardVerticalSize = 12;
    private const int defaultBoardHorizontalSize = 12;

    public Board()
    {
        BoardFields = new Field[defaultBoardVerticalSize, defaultBoardHorizontalSize];
        for (int i = 0; i < defaultBoardVerticalSize; i++)
        {
            for (int j = 0; j < defaultBoardHorizontalSize; j++)
            {
                BoardFields[i, j] = new Field((int)BoardFieldType.Empty, vert: i, hor: j);
            }
        }
    }

    /// <summary>
    /// Populate a board with a defined number of ships of specified type
    /// </summary>
    /// <param name="shipsConfiguration"> {5s, 4s, 3s, 2s, 1s} - how many of which ship to place</param>
    public void PopulateBoard(int[] shipsConfiguration, int vertical, int horizontal)
    {
        BoardFields = new Field[vertical, horizontal];
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
                    BoardFields[randomVerticalStartPoint, randomHorizontalStartPoint] =
                        new Field((int)BoardFieldType.Ship, randomVerticalStartPoint, randomHorizontalStartPoint);
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
        LastVerticalGridPos = BoardFields.GetLength(0) - 1;
        LastHorizontalGridPos = BoardFields.GetLength(1) - 1;
    }

    /// <summary>
    /// Checks all neighbouring spots for existence of any other ship
    /// </summary>
    /// <param name="vertical">Vertical coordinate to check</param>
    /// <param name="horizontal">Vertical coordinate to check</param>
    /// <returns>true if there is a ship nearby, false if there are none</returns>
    public bool CheckIfTheresSpaceForShip(int vertical, int horizontal)
    {
        if (BoardFields[vertical, horizontal] == (int)BoardFieldType.Empty &&
            (BoardFields[vertical + 1, horizontal] != (int)BoardFieldType.Ship) &&
            (BoardFields[vertical - 1, horizontal] != (int)BoardFieldType.Ship) &&
            (BoardFields[vertical, horizontal + 1] != (int)BoardFieldType.Ship) &&
            (BoardFields[vertical, horizontal - 1] != (int)BoardFieldType.Ship) &&
            (BoardFields[vertical - 1, horizontal + 1] != (int)BoardFieldType.Ship) &&
            (BoardFields[vertical - 1, horizontal - 1] != (int)BoardFieldType.Ship) &&
            (BoardFields[vertical + 1, horizontal + 1] != (int)BoardFieldType.Ship) &&
            (BoardFields[vertical + 1, horizontal - 1] != (int)BoardFieldType.Ship))
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
        if (BoardFields[verticalCoordinate, horizontalCoordinate] == (int)BoardFieldType.Ship)
        {
            // Mark that spot as hit
            BoardFields[verticalCoordinate, horizontalCoordinate] = (int)BoardFieldType.Shipwreck;
            return true;
        }
        else
        {
            // Mark that spot as a miss
            BoardFields[verticalCoordinate, horizontalCoordinate] = (int)BoardFieldType.Mishit;
            return false;
        }
    }

    /// <summary>
    /// Checks if the generated board contains any ship (a spot with a value of 1)
    /// </summary>
    public bool CheckIfDefeated()
    {
        if (!BoardFields.Cast<int>().Contains((int)BoardFieldType.Ship))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}