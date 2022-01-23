using System;
using System.Collections;
using Classes;
using Enums;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [Header("Prefabs")]
    [SerializeField] private GameObject enemyShipPrefab;

    [SerializeField] private GameObject playerShipPrefab;

    [Header("Parents")]
    [SerializeField] private GameObject playerBoardParent;

    [SerializeField] private GameObject enemyBoardParent;

    [Header("Canvases")]
    [SerializeField] private GameObject winCanvas;

    [SerializeField] private GameObject loseCanvas;

    [Header("UI")]
    [SerializeField] private TextMeshProUGUI turnCounterText;

    [SerializeField] private Button playButton;
    [SerializeField] private Sprite[] spriteList;

    [Header("Other")]
    public AudioManager audioMg;

    private readonly Player _player = new Player();
    public readonly Enemy enemy = new Enemy();
    public string BoardVerticalSize { get; set; }
    public string BoardHorizontalSize { get; set; }
    private int LastHorizontalGridPos { get; set; }
    public int LastVerticalGridPos { get; private set; }
    public int PlayerVerticalAttackCoord { get; set; }
    public int PlayerHorizontalAttackCoord { get; set; }
    public int DifficultyIndex { get; set; } = (int)DifficultyLevel.Normal;
    private int ShipConfigurationIndex { get; set; } = 1;

    private const int MinBoardSize = 2;
    private const int MaxBoardSize = 10;
    private const float ResultScreenDuration = 2.8f;

    private int _turnCount = 1;

    private void PrintPlayerGrid()
    {
        ClearBoard(playerBoardParent);
        for (int i = 1; i < LastHorizontalGridPos; i++)
        {
            for (int j = 1; j < LastVerticalGridPos; j++)
            {
                GameObject playerShip = Instantiate(playerShipPrefab, playerBoardParent.transform);
                playerShip.GetComponent<Image>().sprite = spriteList[_player.board.BoardFields[j, i].Type];
            }
        }
    }

    private void PrintEnemyGrid()
    {
        ClearBoard(enemyBoardParent);
        for (var i = 1; i < LastHorizontalGridPos; i++)
        {
            for (var j = 1; j < LastVerticalGridPos; j++)
            {
                var enemyShip = Instantiate(enemyShipPrefab, enemyBoardParent.transform);
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
        var hitSuccess = enemy.board.LaunchAttack(PlayerVerticalAttackCoord, PlayerHorizontalAttackCoord);
        if (DifficultyIndex == (int)DifficultyLevel.Dumb)
        {
            enemy.LastPlayerAttackCoords = new[] { PlayerVerticalAttackCoord, PlayerHorizontalAttackCoord };
        }

        RefreshBoard((int)BoardOwner.Enemy);
        if (enemy.board.CheckIfDefeated())
        {
            StartCoroutine(EnemyDefeat());
            return;
        }
        if (hitSuccess)
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
            int[] enemyAttackCoords = enemy.DoAnAttack();
            var hitSuccess = _player.board.LaunchAttack(enemyAttackCoords[0], enemyAttackCoords[1]);
            RefreshBoard((int)BoardOwner.Player);
            if (_player.board.CheckIfDefeated())
            {
                StartCoroutine(PlayerDefeat());
                return;
            }

            if (!hitSuccess)
            {
                break;
            }
        }

        _turnCount++;
        ShowTurn();
    }

    public void PlayButton()
    {
        enemy.CurrentDifficulty = DifficultyIndex;
        enemy.board.PopulateBoard(_player.ChooseShipsConfiguration(ShipConfigurationIndex), Convert.ToInt32(BoardVerticalSize) + 2, Convert.ToInt32(BoardHorizontalSize) + 2);
        _player.board.PopulateBoard(_player.ChooseShipsConfiguration(ShipConfigurationIndex), Convert.ToInt32(BoardVerticalSize) + 2, Convert.ToInt32(BoardHorizontalSize) + 2);
        LastHorizontalGridPos = _player.board.LastHorizontalGridPos;
        LastVerticalGridPos = _player.board.LastVerticalGridPos;
        enemy.PlayerBoardGrid = _player.board.BoardFields;

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

    private void ShowTurn()
    {
        turnCounterText.text = "Turn: " + _turnCount;
    }

    private IEnumerator PlayerDefeat()
    {
        Instantiate(loseCanvas);
        audioMg.PlaySound((int)SoundClips.Defeat);
        yield return new WaitForSeconds(ResultScreenDuration);
        SceneManager.LoadScene(1);
    }

    private IEnumerator EnemyDefeat()
    {
        Instantiate(winCanvas);
        audioMg.PlaySound((int)SoundClips.Victory);
        yield return new WaitForSeconds(ResultScreenDuration);
        SceneManager.LoadScene(1);
    }

    public void LockPlayIfStringEmpty()
    {
        if (!string.IsNullOrEmpty(BoardHorizontalSize) &&
            Convert.ToInt32(BoardHorizontalSize) >= MinBoardSize && Convert.ToInt32(BoardHorizontalSize) <= MaxBoardSize)
        {
            playButton.interactable = true;
        }
        else
        {
            playButton.interactable = false;
        }
    }
}