using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

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
    public GameObject winCanvas;
    public GameObject loseCanvas;

    private int turnCount = 1;
    private const float resultScreenDuration = 2f;

    public string BoardVerticalSize { get; set; }
    public string BoardHorizontalSize { get; set; }

    // Hardcoding the configuration index because it doesn't even mean anything yet
    private const int configurationIndex = 1;

    public int DifficultyIndex { get; set; }
    public int PlayerVerticalAttackCoord { get; set; }
    public int PlayerHorizontalAttackCoord { get; set; }

    public int LastHorizontalGridPos { get; set; }
    public int LastVerticalGridPos { get; set; }

    public void PrintPlayerGrid()
    {
        ClearBoard(playerBoardParent);
        for (int i = 1; i < LastHorizontalGridPos; i++)
        {
            for (int j = 1; j < LastVerticalGridPos; j++)
            {
                GameObject playerShip = Instantiate(playerShipPrefab, new Vector2(i * 60, Screen.height + (-j * 60)), Quaternion.identity, playerBoardParent.transform);

                playerShip.GetComponent<Image>().sprite = spriteList[player.board.BoardFields[j, i].Type];
            }
        }
    }

    public void PrintEnemyGrid()
    {
        ClearBoard(enemyBoardParent);
        for (int i = 1; i < LastHorizontalGridPos; i++)
        {
            for (int j = 1; j < LastVerticalGridPos; j++)
            {
                GameObject enemyShip = Instantiate(enemyShipPrefab, new Vector2((Screen.width / 2) + (i * 60), Screen.height + (-j * 60)), Quaternion.identity, enemyBoardParent.transform);
                enemyShip.GetComponent<ShipFunctionality>().HorCoord = i;
                enemyShip.GetComponent<ShipFunctionality>().VertCoord = j;

                if (enemy.board.BoardFields[j, i].FlagIsActive)
                {
                    enemyShip.GetComponent<Image>().sprite = spriteList[(int)BoardFieldType.PlayerFlag];
                }
                else if (enemy.board.BoardFields[j, i].Type != (int)BoardFieldType.Ship)
                {
                    enemyShip.GetComponent<Image>().sprite = spriteList[enemy.board.BoardFields[j, i].Type];
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
            StartCoroutine(EnemyDefeat());
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
                StartCoroutine(PlayerDefeat());
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
        enemy.PlayerBoardGrid = player.board.BoardFields;
        RefreshBoard();
    }

    /// <param name="whichBoard">1 - player board, 2 - enemy board, else: both boards</param>
    public void RefreshBoard(int whichBoard = 0)
    {
        if (whichBoard == 1)
        {
            PrintPlayerGrid();
            ShowTurn();
        }
        if (whichBoard == 2)
        {
            PrintEnemyGrid();
            ShowTurn();
        }
        else
        {
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

    private IEnumerator PlayerDefeat()
    {
        Debug.Log("You lose!");
        Instantiate(loseCanvas);
        yield return new WaitForSeconds(resultScreenDuration);
        SceneManager.LoadScene(0);
    }

    private IEnumerator EnemyDefeat()
    {
        Debug.Log("You Win!");
        Instantiate(winCanvas);
        yield return new WaitForSeconds(resultScreenDuration);
        SceneManager.LoadScene(0);
    }
}