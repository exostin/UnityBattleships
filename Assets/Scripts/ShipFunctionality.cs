using Enums;
using UnityEngine;
using UnityEngine.EventSystems;

public class ShipFunctionality : MonoBehaviour, IPointerClickHandler
{
    private GameManager _gm;
    public int OriginalValue { get; set; }
    public int VertCoord { get; set; }
    public int HorCoord { get; set; }

    public bool FlagState { get; set; } = true;

    private void Start()
    {
        _gm = FindObjectOfType<GameManager>();
    }

    public virtual void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            ToggleFlag();
            _gm.audioMg.PlaySound((int)SoundClips.Flag);
        }
    }

    public void SelectAttackCoordsByPlayer()
    {
        if (_gm.enemy.board.BoardFields[VertCoord, HorCoord].Type != (int)BoardFieldType.Mishit
            && _gm.enemy.board.BoardFields[VertCoord, HorCoord].Type != (int)BoardFieldType.Shipwreck)
        {
            _gm.PlayerVerticalAttackCoord = VertCoord;
            _gm.PlayerHorizontalAttackCoord = HorCoord;
            _gm.MakeTurn();
        }
    }

    public void ToggleFlag()
    {
        _gm.enemy.board.BoardFields[VertCoord, HorCoord].FlagIsActive = !_gm.enemy.board.BoardFields[VertCoord, HorCoord].FlagIsActive;
        _gm.RefreshBoard((int)BoardOwner.Enemy);
    }
}