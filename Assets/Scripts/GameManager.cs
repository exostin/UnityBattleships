using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    Player player = new Player();
    Enemy enemy = new Enemy();

    /// <summary>
    /// 0 - empty, 1 - ship, 2 - miss, 3 - hit
    /// </summary>
    public Sprite[] spriteList;

    public GameObject boardElement;
    public GameObject playerBoardParent;
    public GameObject enemyBoardParent;

    public int turnCount = 1;
    public int screenHalf;
    public string boardVerticalSize { get; set; }
    public string boardHorizontalSize { get; set; }

    // Hardcoding the configuration index because it doesn't even mean anything yet
    public int configurationIndex = 1;
    public int difficultyIndex { get; set; }
    public int playerVerticalAttackCoord { get; set; }
    public int playerHorizontalAttackCoord { get; set; }

    public int lastHorizontalGridPos { get; set; }
    public int lastVerticalGridPos { get; set; }

    void Start()
    {
        screenHalf = Screen.width / 2;
        //while (true)
        //{
        //    Pass turnCount value to the turn counter
        //    Print the player.board.GeneratedBoard
        //    Print the enemy.board.GeneratedBoard

        //    Get player attack coordinates and assign values accordingly
        //    Attack by invoking Attack();
        //turnCount++;
        //}
    } 
    public void PrintPlayerGrid(int lastHorizontalGridPos, int lastVerticalGridPos, int[,] playerGrid)
    {
        for (int i = 1; i < lastHorizontalGridPos; i++)
        {
            for (int j = 1; j < lastVerticalGridPos; j++)
            {
                GameObject boardElementClone = Instantiate(boardElement, new Vector2(j*60, i*60), Quaternion.identity, playerBoardParent.transform);

                boardElementClone.GetComponent<Image>().sprite = spriteList[playerGrid[i, j]];

                //GameObject boardElementClone = Instantiate(boardElement);
                //boardElementClone.transform.position = new Vector2(i, j);
                //boardElementClone.GetComponent<SpriteRenderer>().sprite = spriteList[playerGrid[i, j]];
            }
        }
    }

    public void PrintEnemyGrid(int lastHorizontalGridPos, int lastVerticalGridPos, int[,] enemyGrid)
    {
        for (int i = 1; i < lastHorizontalGridPos; i++)
        {
            for (int j = 1; j < lastVerticalGridPos; j++)
            {
                GameObject boardElementCloneEnemy = Instantiate(boardElement, new Vector2(screenHalf + (j * 60), i * 60), Quaternion.identity, enemyBoardParent.transform);

                if (enemyGrid[i, j] != 1)
                {
                    boardElementCloneEnemy.GetComponent<Image>().sprite = spriteList[enemyGrid[i, j]];
                }
                else
                {
                    boardElementCloneEnemy.GetComponent<Image>().sprite = spriteList[0];
                }
            }
        }
    }

    /// <summary>
    /// Fires an attack
    /// </summary>
    /// <param name="playerTurn">If true, then the attack is made on the enemy board</param>
    public void Attack(bool playerTurn)
    {
        if (playerTurn)
        {
            enemy.board.LaunchAttack(playerVerticalAttackCoord, playerHorizontalAttackCoord);
        }
        else
        {
            player.board.LaunchAttack(enemy.Attack()[0], enemy.Attack()[1]);
        }
    }



    /// <summary>
    /// Checks if either side has been defeated
    /// </summary>
    /// <returns>0 - neither has lost, 1 - enemy lost, 2 - player lost</returns>
    public int CheckDefeat()
    {
        if (enemy.board.CheckIfDefeated())
        {
            return 1;
        }
        else if (player.board.CheckIfDefeated())
        {
            return 2;
        }
        else
        {
            return 0;
        }
    }
    public void PlayGame()
    {
        enemy.CurrentDifficulty = difficultyIndex;
        enemy.board.PopulateBoard(player.ChooseShips(configurationIndex), Convert.ToInt32(boardVerticalSize), Convert.ToInt32(boardHorizontalSize));
        player.board.PopulateBoard(player.ChooseShips(configurationIndex), Convert.ToInt32(boardVerticalSize), Convert.ToInt32(boardHorizontalSize));
        enemy.PlayerBoardGrid = player.board.GeneratedBoard;
        lastHorizontalGridPos = player.board.LastHorizontalGridPos;
        lastVerticalGridPos = player.board.LastVerticalGridPos;
        PrintPlayerGrid(lastHorizontalGridPos,lastVerticalGridPos, player.board.GeneratedBoard);
        PrintEnemyGrid(lastHorizontalGridPos, lastVerticalGridPos, enemy.board.GeneratedBoard);
    }
}