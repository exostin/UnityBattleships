using UnityEngine;
using UnityEngine.UI;

public class GridManager : MonoBehaviour
{
    private GameManager _gm;
    [SerializeField] private GridLayoutGroup gl;

    public void SetGridLayoutConstraint()
    {
        _gm = FindObjectOfType<GameManager>();
        gl.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        gl.constraintCount = _gm.LastVerticalGridPos - 1;
    }
}