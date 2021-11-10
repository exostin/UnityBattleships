using UnityEngine;
using UnityEngine.EventSystems;

public class ShipFunctionality : MonoBehaviour, IPointerClickHandler
{
    private GameManager gm;
    public int OriginalValue { get; set; }
    public int VertCoord { get; set; }
    public int HorCoord { get; set; }

    public bool FlagState { get; set; } = true;

    private void Start()
    {
        gm = FindObjectOfType<GameManager>();
    }

    public virtual void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            ToggleFlag();
        }
        //if (eventData.button == PointerEventData.InputButton.Left)
        //{
        //    SelectAttackCoordsByPlayer();
        //}
    }

    public void SelectAttackCoordsByPlayer()
    {
        if (gm.enemy.board.BoardFields[VertCoord, HorCoord].Type != (int)BoardFieldType.Mishit
            && gm.enemy.board.BoardFields[VertCoord, HorCoord].Type != (int)BoardFieldType.Shipwreck)
        {
            gm.PlayerVerticalAttackCoord = VertCoord;
            gm.PlayerHorizontalAttackCoord = HorCoord;
            gm.MakeTurn();
        }
    }

    public void ToggleFlag()
    {
        gm.enemy.board.BoardFields[VertCoord, HorCoord].FlagIsActive = !gm.enemy.board.BoardFields[VertCoord, HorCoord].FlagIsActive;
        gm.RefreshBoard(2);
    }
}