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

    public bool IsPlayerTurn { get; set; } = true;
    public string boardVerticalSize { get; set; }
    public string boardHorizontalSize { get; set; }

    // Hardcoding the configuration index because it doesn't even mean anything yet
    public int configurationIndex = 1;

    public int difficultyIndex { get; set; }
    public int playerVerticalAttackCoord { get; set; }
    public int playerHorizontalAttackCoord { get; set; }

    public int lastHorizontalGridPos { get; set; }
    public int lastVerticalGridPos { get; set; }

    public void PrintPlayerGrid(int lastHorizontalGridPos, int lastVerticalGridPos, int[,] playerGrid)
    {
        for (int i = 1; i < lastHorizontalGridPos; i++)
        {
            for (int j = 1; j < lastVerticalGridPos; j++)
            {
                GameObject playerShip = Instantiate(playerShipPrefab, new Vector2(i * 60, Screen.height + (-j * 60)), Quaternion.identity, playerBoardParent.transform);

                playerShip.GetComponent<Image>().sprite = spriteList[playerGrid[j, i]];
                playerShip.GetComponent<Ship>().HorCoord = i;
                playerShip.GetComponent<Ship>().VertCoord = j;

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
                GameObject enemyShip = Instantiate(enemyShipPrefab, new Vector2((Screen.width / 2) + (i * 60), Screen.height + (-j * 60)), Quaternion.identity, enemyBoardParent.transform);
                //Or this instead:
                //GameObject enemyShip = Instantiate(enemyShipPrefab, enemyBoardParent.transform, false);
                //enemyShip.transform.position = new Vector2(screenHalf + (i * 60), screenMaxHeight + (-j * 60));
                enemyShip.GetComponent<Ship>().HorCoord = i;
                enemyShip.GetComponent<Ship>().VertCoord = j;
                if (enemyGrid[j, i] != 1)
                {
                    enemyShip.GetComponent<Image>().sprite = spriteList[enemyGrid[j, i]];
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
        player.board.LaunchAttack(enemy.Attack()[0], enemy.Attack()[1]);
        RefreshBoard(2);
        if (player.board.CheckIfDefeated())
        {
            Debug.Log("You lose!");
            SceneManager.LoadScene(0);
        }
        enemy.board.LaunchAttack(playerVerticalAttackCoord, playerHorizontalAttackCoord);
        RefreshBoard(1);
        if (enemy.board.CheckIfDefeated())
        {
            Debug.Log("You win!");
            SceneManager.LoadScene(0);
        }

        turnCount++;
    }

    public void PlayGame()
    {
        enemy.CurrentDifficulty = difficultyIndex;
        enemy.board.PopulateBoard(player.ChooseShips(configurationIndex), Convert.ToInt32(boardVerticalSize) + 2, Convert.ToInt32(boardHorizontalSize) + 2);
        player.board.PopulateBoard(player.ChooseShips(configurationIndex), Convert.ToInt32(boardVerticalSize) + 2, Convert.ToInt32(boardHorizontalSize) + 2);
        enemy.PlayerBoardGrid = player.board.GeneratedBoard;
        lastHorizontalGridPos = player.board.LastHorizontalGridPos;
        lastVerticalGridPos = player.board.LastVerticalGridPos;
        RefreshBoard();
    }

    public void RefreshBoard(int whichBoard = 0)
    {
        if (whichBoard == 1)
        {
            ClearGrid(playerBoardParent);
            PrintPlayerGrid(lastHorizontalGridPos, lastVerticalGridPos, player.board.GeneratedBoard);
            ShowTurn();
        }
        if (whichBoard == 2)
        {
            ClearGrid(enemyBoardParent);
            PrintEnemyGrid(lastHorizontalGridPos, lastVerticalGridPos, enemy.board.GeneratedBoard);
            ShowTurn();
        }
        else
        {
            ClearGrid(playerBoardParent);
            ClearGrid(enemyBoardParent);
            PrintPlayerGrid(lastHorizontalGridPos, lastVerticalGridPos, player.board.GeneratedBoard);
            PrintEnemyGrid(lastHorizontalGridPos, lastVerticalGridPos, enemy.board.GeneratedBoard);
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