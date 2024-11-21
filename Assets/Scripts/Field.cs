﻿using System.Numerics;
using System;

using System.Configuration;
using UnityEngine;
using Random = System.Random;
using Vector2Int = System.Numerics.Vector2Int;

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
        for (int x = 0; x < size.X; x++)
            for (int y = 0; y < size.Y; y++)
            {
                field[x, y] = new Cell(x, y);
            }
    }
    public static bool SetElement(Element element,Vector2Int pos)
    {
        if (!IsBound(pos))
            if (!IsElement(pos))
            {
                element.cell = field[pos.X, pos.Y];
                element.cell.element = element;
                Debug.Log(element.name);
              //  element.Render(element.cell.pos);
                return true;
            }
            return false;
    }
    public static bool IsBound(Vector2Int point)
    {
        return !(point.X>=0&point.X<size.X && point.Y>=0&point.Y<size.Y);
    }
    public static bool TryGetElement(Vector2Int point,out Element element)
    {
        element = null;
        if (!IsBound(point))
        {
            if(field[point.X, point.Y].element != null)
            {
                element = field[point.X, point.Y].element;
                return true;
            }
        }
        return false;
    }
    public static bool IsElement(Vector2Int point)
    {
        
        if (!IsBound(point))
        {
            if (field[point.X, point.Y].element != null)
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
            if (field[point.X, point.Y] != null)
            {
                cell = field[point.X, point.Y];
                return true;
            }
        }
        return false;
    }

   
    public static void Clear()
    {
        field = new Cell[size.X, size.Y];
    }
}