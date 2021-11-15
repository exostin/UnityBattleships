using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [Header("Prefabs")]
    public GameObject enemyShipPrefab;

    public GameObject playerShipPrefab;

    [Header("Parents")]
    public GameObject playerBoardParent;

    public GameObject enemyBoardParent;

    [Header("Canvases")]
    public GameObject winCanvas;

    public GameObject loseCanvas;

    [Header("UI")]
    public TextMeshProUGUI turnCounterText;

    public Button playButton;
    public Sprite[] spriteList;

    [Header("Other")]
    public AudioManager audioMg;

    public Player player = new Player();
    public Enemy enemy = new Enemy();
    public string BoardVerticalSize { get; set; }
    public string BoardHorizontalSize { get; set; }
    public int LastHorizontalGridPos { get; set; }
    public int LastVerticalGridPos { get; set; }
    public int PlayerVerticalAttackCoord { get; set; }
    public int PlayerHorizontalAttackCoord { get; set; }
    public int DifficultyIndex { get; set; } = (int)DifficultyLevel.Normal;
    public int ShipConfigurationIndex { get; set; } = 1;

    private const int minBoardSize = 2;
    private const int maxBoardSize = 10;
    private const float resultScreenDuration = 2.8f;

    private int turnCount = 1;

    public void PrintPlayerGrid()
    {
        ClearBoard(playerBoardParent);
        for (int i = 1; i < LastHorizontalGridPos; i++)
        {
            for (int j = 1; j < LastVerticalGridPos; j++)
            {
                // No longer needed since using Grid Layout Grid component in Unity
                //GameObject playerShip = Instantiate(playerShipPrefab, new Vector2(i * distanceBetweenTilesMultiplier,
                //    Screen.height + (-j * distanceBetweenTilesMultiplier)), Quaternion.identity, playerBoardParent.transform);

                GameObject playerShip = Instantiate(playerShipPrefab, playerBoardParent.transform);
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
                // No longer needed since using Grid Layout Group component in Unity
                //GameObject enemyShip = Instantiate(enemyShipPrefab, new Vector2((Screen.width / 2) + (i * distanceBetweenTilesMultiplier),
                //    Screen.height + (-j * distanceBetweenTilesMultiplier)), Quaternion.identity, enemyBoardParent.transform);

                GameObject enemyShip = Instantiate(enemyShipPrefab, enemyBoardParent.transform);
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
        if (DifficultyIndex == (int)DifficultyLevel.Dumb)
        {
            enemy.LastPlayerAttackCoords = new int[] { PlayerVerticalAttackCoord, PlayerHorizontalAttackCoord };
        }

        RefreshBoard((int)BoardOwner.Enemy);
        if (enemy.board.CheckIfDefeated())
        {
            StartCoroutine(EnemyDefeat());
            return;
        }
        if (_hitSuccess)
        {
            audioMg.PlaySound((int)SoundClips.Hit);
            return;
        }
        else
        {
            audioMg.PlaySound((int)SoundClips.Miss);
        }
        DoEnemyMove();
    }

    private void DoEnemyMove()
    {
        while (true)
        {
            int[] _enemyAttackCoords = enemy.DoAnAttack();
            var _hitSuccess = player.board.LaunchAttack(_enemyAttackCoords[0], _enemyAttackCoords[1]);
            RefreshBoard((int)BoardOwner.Player);
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
        ShowTurn();
    }

    public void PlayButton()
    {
        enemy.CurrentDifficulty = DifficultyIndex;
        enemy.board.PopulateBoard(player.ChooseShipsConfiguration(ShipConfigurationIndex), Convert.ToInt32(BoardVerticalSize) + 2, Convert.ToInt32(BoardHorizontalSize) + 2);
        player.board.PopulateBoard(player.ChooseShipsConfiguration(ShipConfigurationIndex), Convert.ToInt32(BoardVerticalSize) + 2, Convert.ToInt32(BoardHorizontalSize) + 2);
        LastHorizontalGridPos = player.board.LastHorizontalGridPos;
        LastVerticalGridPos = player.board.LastVerticalGridPos;
        enemy.PlayerBoardGrid = player.board.BoardFields;

        RefreshBoard();
        ShowTurn();
    }

    /// <param name="whichBoard">BoardOwner.Player/BoardOwner.Enemy/nothing to refresh them both</param>
    public void RefreshBoard(int whichBoard = 0)
    {
        if (whichBoard == (int)BoardOwner.Player)
        {
            PrintPlayerGrid();
        }
        if (whichBoard == (int)BoardOwner.Enemy)
        {
            PrintEnemyGrid();
        }
        else
        {
            PrintPlayerGrid();
            PrintEnemyGrid();
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
        Instantiate(loseCanvas);
        audioMg.PlaySound((int)SoundClips.Defeat);
        yield return new WaitForSeconds(resultScreenDuration);
        SceneManager.LoadScene(0);
    }

    private IEnumerator EnemyDefeat()
    {
        Instantiate(winCanvas);
        audioMg.PlaySound((int)SoundClips.Victory);
        yield return new WaitForSeconds(resultScreenDuration);
        SceneManager.LoadScene(0);
    }

    public void LockPlayIfStringEmpty()
    {
        if (BoardHorizontalSize != null && BoardHorizontalSize.Length > 0 &&
            Convert.ToInt32(BoardHorizontalSize) >= minBoardSize && Convert.ToInt32(BoardHorizontalSize) <= maxBoardSize)
        {
            playButton.interactable = true;
        }
        else
        {
            playButton.interactable = false;
        }
    }
}