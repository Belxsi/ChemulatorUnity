
using System.Numerics;
using UnityEngine;
using Vector2 = System.Numerics.Vector2;

public class Cell
{
    public bool inited;
    public Vector2 pos;
    public Element element;
    public Cell(int x, int y, Element element)
    {
        pos = new Vector2(x, y);
        this.element = element;
        element.cell = this;
        inited = true;
    }
    
    public Cell(int x, int y)
    {
        pos = new Vector2(x, y);
        inited = true;
    }
    public Color GetColorElement()
    {
        if (element != null)
        {
            return element.color;
        }
        else
        {
            return Color.black;
        }
    }
}
