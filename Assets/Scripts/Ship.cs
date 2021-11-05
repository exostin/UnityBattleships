using UnityEngine;
using UnityEngine.EventSystems;

public class Ship : MonoBehaviour, IPointerClickHandler
{
    private GameManager gm;
    public int OriginalValue { get; set; }
    public int VertCoord { get; set; }
    public int HorCoord { get; set; }
    public bool FlagActive { get; set; } = false;

    private void Start()
    {
        gm = FindObjectOfType<GameManager>();
    }

    public void SelectAttackCoordsByPlayer()
    {
        if (gm.enemy.board.GeneratedBoard[VertCoord, HorCoord] != 2 && gm.enemy.board.GeneratedBoard[VertCoord, HorCoord] != 3)
        {
            gm.PlayerVerticalAttackCoord = VertCoord;
            gm.PlayerHorizontalAttackCoord = HorCoord;
            gm.MakeTurn();
        }
    }

    public virtual void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            ToggleFlag();
        }
    }

    public void ToggleFlag()
    {
        if (!FlagActive)
        {
            gm.enemy.board.GeneratedBoard[VertCoord, HorCoord] = 4;
        }
        else
        {
            gm.enemy.board.GeneratedBoard[VertCoord, HorCoord] = OriginalValue;
        }
        FlagActive = !FlagActive;
        gm.RefreshBoard(2);
    }
}