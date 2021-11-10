using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GridManager : MonoBehaviour
{
    private GameManager gm;
    public GridLayoutGroup gl;

    public void SetGridLayoutConstraint()
    {
        gm = FindObjectOfType<GameManager>();
        gl.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        gl.constraintCount = gm.LastVerticalGridPos - 1;
    }
}