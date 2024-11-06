using System.Numerics;
using UnityEngine;
using Vector2 = System.Numerics.Vector2;
using Vector2Int = System.Numerics.Vector2Int;

public abstract class Element
{
    public float m, E;
    public string name;
    public Color color;
    public Cell cell;
    public Vector2 velocity;
    public Vector2 dolit;
    public DirLiqual dirUniXMove=DirLiqual.None;
    protected Element(float m, string name, Color color)
    {
        this.m = m;
        this.name = name;
        this.color = color;

    }
    public static T Clone<T>() where T : Element, new()
    {
        return new T();
    }
    public void Render(Vector2Int point)
    {
        if(Field.TryGetCell(point,out Cell cell))
        Simulator.me.graphic.RenderPixel(point,cell);
    }
    public void Glut(Vector2 pos)
    {
        dolit += pos-(Vector2)(Vector2Int)(pos);
    }
    public void Repos(Vector2Int newpos)
    {
        
        
        bool rendering = false;
        if (newpos != cell.pos) rendering = true;
        Vector2Int old=cell.pos;
        if (Field.TryGetCell(newpos, out Cell newcell))
        {
            
            cell.element = null;
            cell = newcell;
            cell.element = this;

            if (rendering)
            {
                Simulator.me.graphic.RenderPixel(newpos);
                Simulator.me.graphic.RenderPixel(old);
            }

        }


    }
    public static void Replace(Element a, Element b)
    {
        Cell a_cell = a.cell;
        Cell b_cell = b.cell;
        a.cell = null;
        b.cell = null;
        a.cell = b_cell;
        b.cell = a_cell;

    }
}
public enum DirLiqual
{
    R,L,None
}
public class BaseElement : Element
{
    public BaseElement(float m, string name, Color color) : base(m, name, color)
    {
    }
}
public class H2O : Element
{
    public H2O() : base(18, "H2O",new Color(183/255F, 247/255F, 250/255F))
    {
        
    }
}
