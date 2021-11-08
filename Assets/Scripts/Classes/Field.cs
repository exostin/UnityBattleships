using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Field : MonoBehaviour
{
    public int Type { get; set; } = (int)BoardFieldType.Empty;
    public int VerticalCoord { get; set; }
    public int HorizontalCoord { get; set; }

    public Field(int fieldType, int vert, int hor)
    {
        Type = fieldType;
        VerticalCoord = vert;
        HorizontalCoord = hor;
    }
}