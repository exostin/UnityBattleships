using UnityEngine;
using UnityEngine.UI;

public class GridManager : MonoBehaviour
{
    [SerializeField] private GridLayoutGroup gl;
    private GameManager _gm;

    public void SetGridLayoutConstraint()
    {
        _gm = FindObjectOfType<GameManager>();
        gl.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        gl.constraintCount = _gm.LastVerticalGridPos - 1;
    }
}