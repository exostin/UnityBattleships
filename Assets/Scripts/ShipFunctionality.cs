using UnityEngine;
using UnityEngine.EventSystems;

public class ShipFunctionality : MonoBehaviour, IPointerClickHandler
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
        if (gm.enemy.board.BoardFields[VertCoord, HorCoord] != (int)BoardFieldType.Mishit
            && gm.enemy.board.BoardFields[VertCoord, HorCoord] != (int)BoardFieldType.Shipwreck)
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
            gm.enemy.board.BoardFields[VertCoord, HorCoord] = (int)BoardFieldType.PlayerFlag;
        }
        else
        {
            gm.enemy.board.BoardFields[VertCoord, HorCoord] = OriginalValue;
        }
        FlagActive = !FlagActive;
        gm.RefreshBoard(2);
    }
}