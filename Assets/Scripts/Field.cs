using System.Numerics;
using System;

using System.Configuration;
using UnityEngine;
using Random = System.Random;


public class Field
{
    public static Vector2Int size;
    public static Cell[,] field;
    public Field(int x, int y)
    {
        size = new Vector2Int(x, y);
        Clear();
        Init();
    }
    public static void Init()
    {
        for (int x = 0; x < size.x; x++)
            for (int y = 0; y < size.y; y++)
            {
                field[x, y] = new Cell(x, y);
            }
    }
    public static bool SetElement(Element element,Vector2Int pos)
    {
        if (!IsBound(pos))
            if (!IsElement(pos))
            {
                element.cell = field[pos.x, pos.y];
                element.cell.element = element;
                Debug.Log(element.name);
              //  element.Render(element.cell.pos);
                return true;
            }
            return false;
    }
    public static bool IsBound(Vector2Int point)
    {
        return !(point.x>=0&point.x<size.x && point.y>=0&point.y<size.y);
    }
    public static bool TryGetElement(Vector2Int point,out Element element)
    {
        element = null;
        if (!IsBound(point))
        {
            if(field[point.x, point.y].element != null)
            {
                element = field[point.x, point.y].element;
                return true;
            }
        }
        return false;
    }
    public static bool IsElement(Vector2Int point)
    {
        
        if (!IsBound(point))
        {
            if (field[point.x, point.y].element != null)
            {
               
                return true;
            }
        }
        return false;
    }
    public static bool TryGetCell(Vector2Int point, out Cell cell)
    {
        cell = null;
        if (!IsBound(point))
        {
            if (field[point.x, point.y] != null)
            {
                cell = field[point.x, point.y];
                return true;
            }
        }
        return false;
    }

   
    public static void Clear()
    {
        field = new Cell[size.x, size.y];
    }
}
