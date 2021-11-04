using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public Player player = new Player();
    public Enemy enemy = new Enemy();

    /// <summary>
    /// 0 - empty, 1 - ship, 2 - miss, 3 - hit
    /// </summary>
    public Sprite[] spriteList;

    public GameObject enemyShipPrefab;
    public GameObject playerShipPrefab;
    public GameObject playerBoardParent;
    public GameObject enemyBoardParent;

    public TextMeshProUGUI turnCounterText;

    public int turnCount = 1;

    public string boardVerticalSize { get; set; }
    public string boardHorizontalSize { get; set; }

    // Hardcoding the configuration index because it doesn't even mean anything yet
    public int configurationIndex = 1;

    public int difficultyIndex { get; set; }
    public int playerVerticalAttackCoord { get; set; }
    public int playerHorizontalAttackCoord { get; set; }

    public int lastHorizontalGridPos { get; set; }
    public int lastVerticalGridPos { get; set; }

    public void PrintPlayerGrid()
    {
        for (int i = 1; i < lastHorizontalGridPos; i++)
        {
            for (int j = 1; j < lastVerticalGridPos; j++)
            {
                GameObject playerShip = Instantiate(playerShipPrefab, new Vector2(i * 60, Screen.height + (-j * 60)), Quaternion.identity, playerBoardParent.transform);

                playerShip.GetComponent<Image>().sprite = spriteList[player.board.GeneratedBoard[j, i]];
                playerShip.GetComponent<Ship>().HorCoord = i;
                playerShip.GetComponent<Ship>().VertCoord = j;
            }
        }
    }

    public void PrintEnemyGrid()
    {
        for (int i = 1; i < lastHorizontalGridPos; i++)
        {
            for (int j = 1; j < lastVerticalGridPos; j++)
            {
                GameObject enemyShip = Instantiate(enemyShipPrefab, new Vector2((Screen.width / 2) + (i * 60), Screen.height + (-j * 60)), Quaternion.identity, enemyBoardParent.transform);
                enemyShip.GetComponent<Ship>().HorCoord = i;
                enemyShip.GetComponent<Ship>().VertCoord = j;
                if (enemy.board.GeneratedBoard[j, i] != 1)
                {
                    enemyShip.GetComponent<Image>().sprite = spriteList[enemy.board.GeneratedBoard[j, i]];
                }
                else
                {
                    enemyShip.GetComponent<Image>().sprite = spriteList[0];
                }
            }
        }
    }

    public void MakeTurn()
    {
        enemy.board.LaunchAttack(playerVerticalAttackCoord, playerHorizontalAttackCoord);
        RefreshBoard(2);
        if (enemy.board.CheckIfDefeated())
        {
            Debug.Log("You win!");
            SceneManager.LoadScene(0);
        }
        player.board.LaunchAttack(enemy.Attack()[0], enemy.Attack()[1]);
        RefreshBoard(1);
        if (player.board.CheckIfDefeated())
        {
            Debug.Log("You lose!");
            SceneManager.LoadScene(0);
        }

        turnCount++;
    }

    public void PlayGame()
    {
        enemy.CurrentDifficulty = difficultyIndex;
        enemy.board.PopulateBoard(player.ChooseShips(configurationIndex), Convert.ToInt32(boardVerticalSize) + 2, Convert.ToInt32(boardHorizontalSize) + 2);
        player.board.PopulateBoard(player.ChooseShips(configurationIndex), Convert.ToInt32(boardVerticalSize) + 2, Convert.ToInt32(boardHorizontalSize) + 2);
        lastHorizontalGridPos = player.board.LastHorizontalGridPos;
        lastVerticalGridPos = player.board.LastVerticalGridPos;
        enemy.PlayerBoardGrid = player.board.GeneratedBoard;
        RefreshBoard();
    }

    /// <param name="whichBoard">1 - player board, 2 - enemy board, else: both boards</param>
    public void RefreshBoard(int whichBoard = 0)
    {
        if (whichBoard == 1)
        {
            ClearGrid(playerBoardParent);
            PrintPlayerGrid();
            ShowTurn();
        }
        if (whichBoard == 2)
        {
            ClearGrid(enemyBoardParent);
            PrintEnemyGrid();
            ShowTurn();
        }
        else
        {
            ClearGrid(playerBoardParent);
            ClearGrid(enemyBoardParent);
            PrintPlayerGrid();
            PrintEnemyGrid();
            ShowTurn();
        }
    }

    private void ClearGrid(GameObject gridParent)
    {
        foreach (Transform child in gridParent.transform)
        {
            GameObject.Destroy(child.gameObject);
        }
    }

    public void ShowTurn()
    {
        turnCounterText.text = "Turn: " + turnCount;
    }
}