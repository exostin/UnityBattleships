using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class UI
{
    public static void PrintPlayerAttackPrompt()
    {
        Console.Write("Enter the attack coordinates (ex: 3,B): ");
    }

    public static void PrintPlayerGrid(int lastHorizontalGridPos, int lastVerticalGridPos, int[,] playerGrid)
    {
        char[] letters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();

        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("|   YOUR GRID   |");
        Console.ResetColor();

        Console.ForegroundColor = ConsoleColor.DarkCyan;
        Console.Write("   ");
        for (int i = 0; i < lastVerticalGridPos - 1; i++)
        {
            Console.Write(string.Format("{0} ", letters[i]));
        }
        Console.Write(Environment.NewLine);
        Console.ResetColor();

        for (int i = 1; i < lastHorizontalGridPos; i++)
        {
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            if (i >= 10)
            {
                Console.Write($"{i} ");
            }
            else
            {
                Console.Write($"{i}  ");
            }
            Console.ResetColor();

            for (int j = 1; j < lastVerticalGridPos; j++)
            {
                Console.Write(string.Format("{0} ", playerGrid[i, j]));
            }
            Console.Write(Environment.NewLine);
        }
    }

    public static void PrintEnemyGrid(int lastHorizontalGridPos, int lastVerticalGridPos, int[,] enemyGrid)
    {
        char[] letters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();

        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("|   ENEMY GRID   |");
        Console.ResetColor();

        Console.ForegroundColor = ConsoleColor.DarkCyan;
        Console.Write("   ");
        for (int i = 0; i < lastVerticalGridPos - 1; i++)
        {
            Console.Write(string.Format("{0} ", letters[i]));
        }
        Console.Write(Environment.NewLine);
        Console.ResetColor();

        for (int i = 1; i < lastHorizontalGridPos; i++)
        {
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            if (i >= 10)
            {
                Console.Write($"{i} ");
            }
            else
            {
                Console.Write($"{i}  ");
            }
            Console.ResetColor();

            for (int j = 1; j < lastVerticalGridPos; j++)
            {
                if (enemyGrid[i, j] != 1)
                {
                    Console.Write(string.Format("{0} ", enemyGrid[i, j]));
                }
                else
                {
                    Console.Write("0 ");
                }
            }
            Console.Write(Environment.NewLine);
        }
    }

    public static void PrintTurnCounter(int turnCount)
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine($"#  Turn no. {turnCount}   #");
        Console.ResetColor();
    }

    public static void PrintMissMessage()
    {
        Console.WriteLine("Miss!");
    }

    public static void PrintVictoryMessage()
    {
        Console.WriteLine("\n|   Victory!!!   |\n");
    }

    public static void PrintPlayerHitMessage()
    {
        Console.WriteLine("Hit scored!");
    }

    public static void PrintPositionAlreadyHit()
    {
        Console.WriteLine("You've already fired at that location!");
    }

    public static void PrintEnemyHitOnPlayer()
    {
        Console.WriteLine("The enemy has hit your ship!");
    }

    public static void PrintPlayerDefeat()
    {
        Console.WriteLine("\n|   You lost!   |\n");
    }

    /// <summary>
    /// Method that prints different messages based on parameter that specifies which wrong input scenario happened
    /// </summary>
    /// <param name="scenario">Default - wrong input,
    /// 1 - Coordinates out of bounds,
    /// 2 - Configuration index out of bound or parsing error</param>
    public static void PrintWrongInput(int scenario = 0)
    {
        switch (scenario)
        {
            case 1:
                Console.WriteLine("Coordinates out of bounds or entered incorrectly! Try again.");
                break;

            case 2:
                Console.WriteLine("Wrong input or configuration index out of bounds! Try again.");
                break;

            case 3:
                Console.WriteLine("Wrong input, board size out of bounds or you didn't use \'x\'! Try again.");
                break;

            default:
                Console.WriteLine("Wrong input! Try again.");
                break;
        }
    }

    public static void AskForShipConfigurationTemplate()
    {
        string configurationOptions =
            "1) 5, 44, 333, 2222, 11111 - original\r\n" +
            "2) 55, 44, 333, 2222 - no 1s\r\n" +
            "3) 555, 44, 3333 - no 1s and 2s\r\n" +
            "4) 7x 5\r\n" +
            "5) 35x 1\n";

        Console.WriteLine(configurationOptions);
        Console.Write("Choose your prefered configuration: ");
    }

    public static void AskForDifficulty()
    {
        string difficultiesChoices =
            "1 - Baby\n" +
            "2 - Easy\n" +
            "3 - Normal\n" +
            "4 - Hard\n";
        Console.WriteLine(difficultiesChoices);
        Console.Write("Choose your prefered difficulty: ");
    }

    public static void AskForBoardSize()
    {
        Console.WriteLine("\nYou can choose how big should the board be, from 3x3 to 26x26");
        Console.Write("Choose your prefered board size (ex. 12x12): ");
    }
}
