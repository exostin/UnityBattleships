using System.Linq;
using UnityEngine;

public class Board
{
    public int FirstGridPos { get; set; } = 1;
    public int LastVerticalGridPos { get; set; }
    public int LastHorizontalGridPos { get; set; }
    public Field[,] BoardFields { get; set; }

    /// <summary>
    /// Populate a board with a defined number of ships of specified type
    /// </summary>
    /// <param name="shipsConfiguration"> {5s, 4s, 3s, 2s, 1s} - how many of which ship to place</param>
    public void PopulateBoard(int[] shipsConfiguration, int verticalSize, int horizontalSize)
    {
        BoardFields = new Field[verticalSize, horizontalSize];
        CheckDimensions();
        for (int i = 0; i < horizontalSize; i++)
        {
            for (int j = 0; j < verticalSize; j++)
            {
                BoardFields[j, i] = new Field((int)BoardFieldType.Empty, vert: j, hor: i);
            }
        }

        // Calculate the lowest possible number of ships that can be placed on the board without overlapping
        // Board surface / area the ship takes being near wall or another ship
        var shipsTotalToPlace = ((verticalSize - 2) * (horizontalSize - 2)) / 6;
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
        if (BoardFields[vertical, horizontal].Type == (int)BoardFieldType.Empty &&
            (BoardFields[vertical + 1, horizontal].Type != (int)BoardFieldType.Ship) &&
            (BoardFields[vertical - 1, horizontal].Type != (int)BoardFieldType.Ship) &&
            (BoardFields[vertical, horizontal + 1].Type != (int)BoardFieldType.Ship) &&
            (BoardFields[vertical, horizontal - 1].Type != (int)BoardFieldType.Ship) &&
            (BoardFields[vertical - 1, horizontal + 1].Type != (int)BoardFieldType.Ship) &&
            (BoardFields[vertical - 1, horizontal - 1].Type != (int)BoardFieldType.Ship) &&
            (BoardFields[vertical + 1, horizontal + 1].Type != (int)BoardFieldType.Ship) &&
            (BoardFields[vertical + 1, horizontal - 1].Type != (int)BoardFieldType.Ship))
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
        if (BoardFields[verticalCoordinate, horizontalCoordinate].Type == (int)BoardFieldType.Ship)
        {
            // Mark that spot as hit
            BoardFields[verticalCoordinate, horizontalCoordinate].Type = (int)BoardFieldType.Shipwreck;
            return true;
        }
        else
        {
            // Mark that spot as a miss
            BoardFields[verticalCoordinate, horizontalCoordinate].Type = (int)BoardFieldType.Mishit;
            return false;
        }
    }

    /// <summary>
    /// Checks if the generated board contains any ship
    /// </summary>
    public bool CheckIfDefeated()
    {
        for (int i = 0; i < LastHorizontalGridPos + 1; i++)
        {
            for (int j = 0; j < LastVerticalGridPos + 1; j++)
            {
                if (BoardFields[j, i].Type == (int)BoardFieldType.Ship)
                {
                    return false;
                }
            }
        }
        return true;
    }
}