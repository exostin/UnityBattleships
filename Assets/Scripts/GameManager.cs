using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    Player player = new Player();
    Enemy enemy = new Enemy();

    public int turnCount = 1;
    public string boardVerticalSize { get; set; }
    public string boardHorizontalSize { get; set; }

    // Hardcoding the configuration index because it doesn't even mean anything yet
    public int configurationIndex = 1;
    public int difficultyIndex { get; set; }
    public int playerVerticalAttackCoord { get; set; }
    public int playerHorizontalAttackCoord { get; set; }

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
    }

    void Start()
    {
        // Set the configurationIndex through UI
        // Set the difficultyIndex through UI
        // Set the boardVerticalSize and boardHorizontalSize through UI

        // Check for the input bounds inside Unity
        //(configurationIndex >= 1 && configurationIndex <= 5)
        //&& (difficultyIndex >= 1 && difficultyIndex <= 4)
        //boardVerticalSize <= 26 + 2 && boardHorizontalSize <= 26 + 2 && boardVerticalSize >= 3 + 2 && boardHorizontalSize >= 3 + 2

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
}