using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class UI
{
    public static void PrintPlayerGrid(int lastHorizontalGridPos, int lastVerticalGridPos, int[,] playerGrid)
    {
        //char[] letters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();

        //for (int i = 0; i < lastVerticalGridPos - 1; i++)
        //{
        //    Console.Write(string.Format("{0} ", letters[i]));
        //}
        //Console.Write(Environment.NewLine);
        //Console.ResetColor();

        //for (int i = 1; i < lastHorizontalGridPos; i++)
        //{
        //    if (i >= 10)
        //    {
        //        Console.Write($"{i} ");
        //    }
        //    else
        //    {
        //        Console.Write($"{i}  ");
        //    }

        //    for (int j = 1; j < lastVerticalGridPos; j++)
        //    {
        //        Console.Write(string.Format("{0} ", playerGrid[i, j]));
        //    }
        //    Console.Write(Environment.NewLine);
        //}
    }

    public static void PrintEnemyGrid(int lastHorizontalGridPos, int lastVerticalGridPos, int[,] enemyGrid)
    {
        //char[] letters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();

        //for (int i = 0; i < lastVerticalGridPos - 1; i++)
        //{
        //    Console.Write(string.Format("{0} ", letters[i]));
        //}
        //Console.Write(Environment.NewLine);

        //for (int i = 1; i < lastHorizontalGridPos; i++)
        //{
        //    if (i >= 10)
        //    {
        //        Console.Write($"{i} ");
        //    }
        //    else
        //    {
        //        Console.Write($"{i}  ");
        //    }

        //    for (int j = 1; j < lastVerticalGridPos; j++)
        //    {
        //        if (enemyGrid[i, j] != 1)
        //        {
        //            Console.Write(string.Format("{0} ", enemyGrid[i, j]));
        //        }
        //        else
        //        {
        //            Console.Write("0 ");
        //        }
        //    }
        //    Console.Write(Environment.NewLine);
        //}
    }
}
