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

    private int turnCount = 1;

    public string BoardVerticalSize { get; set; }
    public string BoardHorizontalSize { get; set; }

    // Hardcoding the configuration index because it doesn't even mean anything yet
    private int configurationIndex = 1;

    public int DifficultyIndex { get; set; }
    public int PlayerVerticalAttackCoord { get; set; }
    public int PlayerHorizontalAttackCoord { get; set; }

    public int LastHorizontalGridPos { get; set; }
    public int LastVerticalGridPos { get; set; }

    public void PrintPlayerGrid()
    {
        for (int i = 1; i < LastHorizontalGridPos; i++)
        {
            for (int j = 1; j < LastVerticalGridPos; j++)
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
        for (int i = 1; i < LastHorizontalGridPos; i++)
        {
            for (int j = 1; j < LastVerticalGridPos; j++)
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
        var _hitSuccess = enemy.board.LaunchAttack(PlayerVerticalAttackCoord, PlayerHorizontalAttackCoord);
        RefreshBoard(2);
        if (enemy.board.CheckIfDefeated())
        {
            Debug.Log("You win!");
            SceneManager.LoadScene(0);
            return;
        }
        if (_hitSuccess)
        {
            return;
        }
        DoEnemyMove();
    }

    private void DoEnemyMove()
    {
        while (true)
        {
            int[] _enemyAttackCoords = enemy.DoAnAttack();
            var _hitSuccess = player.board.LaunchAttack(_enemyAttackCoords[0], _enemyAttackCoords[1]);
            RefreshBoard(1);
            if (player.board.CheckIfDefeated())
            {
                Debug.Log("You lose!");
                SceneManager.LoadScene(0);
                return;
            }

            if (!_hitSuccess)
            {
                break;
            }
        }

        turnCount++;
    }

    public void PlayButton()
    {
        enemy.CurrentDifficulty = DifficultyIndex;
        enemy.board.PopulateBoard(player.ChooseShipsConfiguration(configurationIndex), Convert.ToInt32(BoardVerticalSize) + 2, Convert.ToInt32(BoardHorizontalSize) + 2);
        player.board.PopulateBoard(player.ChooseShipsConfiguration(configurationIndex), Convert.ToInt32(BoardVerticalSize) + 2, Convert.ToInt32(BoardHorizontalSize) + 2);
        LastHorizontalGridPos = player.board.LastHorizontalGridPos;
        LastVerticalGridPos = player.board.LastVerticalGridPos;
        enemy.PlayerBoardGrid = player.board.GeneratedBoard;
        RefreshBoard();
    }

    /// <param name="whichBoard">1 - player board, 2 - enemy board, else: both boards</param>
    public void RefreshBoard(int whichBoard = 0)
    {
        if (whichBoard == 1)
        {
            ClearBoard(playerBoardParent);
            PrintPlayerGrid();
            ShowTurn();
        }
        if (whichBoard == 2)
        {
            ClearBoard(enemyBoardParent);
            PrintEnemyGrid();
            ShowTurn();
        }
        else
        {
            ClearBoard(playerBoardParent);
            ClearBoard(enemyBoardParent);
            PrintPlayerGrid();
            PrintEnemyGrid();
            ShowTurn();
        }
    }

    private void ClearBoard(GameObject gridParent)
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