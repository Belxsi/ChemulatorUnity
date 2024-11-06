using System;
using System.Collections.Generic;
using System.Numerics;
using System.Security.Cryptography.X509Certificates;

public class Physic
{
    public Vector2 gravity = new Vector2(0, 1);
    public Random random = new Random();
    public void Update(List<Element> obj)
    {
        foreach (Element el in obj)
        {
           
            Gravitation(el);
            Move(el);
            el.velocity = Vector2.Zero;
            LiquidBehavior(el);
            Move(el);
            el.velocity = Vector2.Zero;

        }
    }
    public void Gravitation(Element element)
    {
        element.velocity += gravity;
    }
    public void LiquidBehavior(Element element)
    {
        bool left = false, right = false,down=false;
        down = Field.IsBound(element.cell.pos + Vector2Int.UnitY);
        if (!down) down = Field.IsElement(element.cell.pos + Vector2Int.UnitY);
        if(!down) return;
        switch (element.dirUniXMove)
        {
            case DirLiqual.None:
                left = Field.IsBound(element.cell.pos - Vector2Int.UnitX);
                right = Field.IsBound(element.cell.pos + Vector2Int.UnitX);
                if (!left) left = Field.IsElement(element.cell.pos - Vector2Int.UnitX);
                if (!right) right = Field.IsElement(element.cell.pos + Vector2Int.UnitX);
                if (left == right & left == true)
                {
                    return;
                }
                if (left == right & left == false)
                {
                    element.dirUniXMove = new DirLiqual[2] { DirLiqual.R, DirLiqual.L }[random.Next(0, 2)];
                    return;

                }
                if (left != right)
                {
                    if (left)
                        element.dirUniXMove = DirLiqual.R;
                    if (right)
                        element.dirUniXMove = DirLiqual.L;
                    return;

                }
                break;
            case DirLiqual.R:
                
               
                right = Field.IsBound(element.cell.pos + Vector2Int.UnitX);
                if (!right) right = Field.IsElement(element.cell.pos + Vector2Int.UnitX);
                if (right)
                {
                    element.dirUniXMove = DirLiqual.L;
                    break;
                }
                element.velocity += Vector2.UnitX;
                break;
            case DirLiqual.L:
                left = Field.IsBound(element.cell.pos - Vector2Int.UnitX);
                if (!left) left = Field.IsElement(element.cell.pos - Vector2Int.UnitX);
                if (left)
                {
                    element.dirUniXMove = DirLiqual.R;
                    break;
                }
                element.velocity -= Vector2.UnitX;
                break;
        }
    }
    public RaycastHit Raycast(Vector2 origin, Vector2 velocity,Element me)
    {
        RaycastHit hit = default;
        hit.distance = velocity.Length();
        hit.dir = Vector2.Normalize(velocity);
        Vector2 originPos = Vector2.Zero;
        Vector2 dolit=me.dolit;
        for (int i = 1; i <= velocity.Length(); i++)
        {
            Vector2 old_currentPoint = hit.dir * (i - 1) + dolit;
            Vector2 currentPoint = hit.dir * i+dolit;
            originPos = origin + currentPoint;
            if((Vector2Int) currentPoint == Vector2Int.Zero) continue;
            if (Field.IsBound(originPos))
            {
                hit.typeCollision = RaycastHit.Collision.Bound;
                hit.distance = currentPoint.Length();
                hit.position = origin + old_currentPoint;
                me.dolit = Vector2.Zero;
                me.Glut(origin + old_currentPoint);
                
                return hit;
            }
            if (Field.TryGetElement(originPos, out Element element))
            {
                hit.typeCollision = RaycastHit.Collision.Element;
                hit.distance = currentPoint.Length();
                hit.position = origin + old_currentPoint;
                me.dolit = Vector2.Zero;
                me.Glut(origin + old_currentPoint);
                return hit;
            }

        }
        hit.typeCollision = RaycastHit.Collision.None;
        hit.position = originPos;
        me.dolit = Vector2.Zero;
        me.Glut(originPos);
        return hit;
    }

    public void Move(Element element)
    {
        if (element.velocity == Vector2.Zero) return;
        RaycastHit hit = Raycast(element.cell.pos, element.velocity,element);
        switch (hit.typeCollision)
        {
            case RaycastHit.Collision.None:
                element.Repos(hit.position);

                break;
            case RaycastHit.Collision.Bound:
                element.Repos(hit.position);

                break;
            case RaycastHit.Collision.Element:
                element.Repos(hit.position);

                break;
        }
    }
}
public struct RaycastHit
{
    public float distance;
    public Vector2Int position;
    public Vector2 dir;
    public Collision typeCollision;
    public Element element;

    public RaycastHit(float distance, Vector2 position, Collision typeCollision, Element element, Vector2 dir)
    {
        this.distance = distance;
        this.position = position;
        this.typeCollision = typeCollision;
        this.element = element;

        this.dir = dir;
    }

    public enum Collision
    {
        None,
        Bound,
        Element
    }
}
