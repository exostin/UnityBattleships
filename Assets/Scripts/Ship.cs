using System.Collections;
using System.Collections.Generic;
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
    

    public void Attack()
    {
        gm.playerVerticalAttackCoord = VertCoord;
        gm.playerHorizontalAttackCoord = HorCoord;
        gm.Attack();
    }
}
