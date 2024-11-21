using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Numerics;
using Vector2Int = System.Numerics.Vector2Int;
using System.Reflection;
using UnityEngine.EventSystems;
public abstract class InspectorBrush : MonoBehaviour
{
    public Simulator simulator;
    public static InspectorBrush main;
    public bool create,reset,mouseConnect;
    public Vector2Int point,offset;
    public float Temp = Element.kelvin + 22;
    public string type;
    public int count;
    public void DrawSelect(Vector2Int offset2)
    {
        if (!Field.IsBound(offset2 + point)) 
        simulator.tasks.Add(new RenderPixel(new( point.X+offset2.X, point.Y+offset2.Y, Color.white)));
    }
    public void Clear(Vector2Int offset2)
    {
        if (Field.TryGetElement(point+ offset2, out Element element))
        {
            simulator.tasks.Add(new ClearElementsTS(element));

        }
    }
    public Element GetPixel()
    {
        Field.TryGetElement(point, out Element element);
        return element;
    }
    public void Create(Vector2Int offset2)
    {
        if (!Field.IsElement(point + offset+offset2))
        {
            simulator.CreateElement(type, point + offset + offset2, Temp);
            if (count <= 0)
            {
                create = reset;

            }
            else count--;
        }
    }
    public abstract void Active();
   
}
