using UnityEngine;

public class Ship : MonoBehaviour
{
    private GameManager gm;
    public int VertCoord { get; set; }
    public int HorCoord { get; set; }

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
}