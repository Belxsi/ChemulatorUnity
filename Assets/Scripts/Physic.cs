using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Vector2 = System.Numerics.Vector2;
using Vector2Int = System.Numerics.Vector2Int;

public class Physic
{
    public static Vector2 gravity = new Vector2(0, 1);
    public static System.Random random = new();
    public class UDLR
    {
        public (bool, Element) up = default, down = default, left = default, right = default;
        public (bool, Cell) cup = default, cdown = default, cleft = default, cright = default;
        public Vector2Int pos;
        public int count = -1;
        public UDLR(Vector2Int pos)
        {
            this.pos = pos;
        }

        public int GetCount()
        {
            if (count == -1)
            {
                int sum = 0;

                if (up.Item1) sum++;
                if (down.Item1) sum++;
                if (left.Item1) sum++;
                if (right.Item1) sum++;
                count = sum;
                return sum;
            }
            else return count;
        }
    }

    public void Update(List<Element> obj)
    {

        foreach (Element el in obj.ToList())
        {
            if (el.IsTag("clear")) continue;
            if (random.NextDouble() < Simulator.me.entropy) continue;
            

            
            if (el.cell != null)
            {
                UDLR udlr = new(el.cell.pos);
                if (el.udlr == null)
                {
                    GetUDLR(el, ref udlr.up, ref udlr.down, ref udlr.left, ref udlr.right);
                    GetCellUDLR(el, ref udlr.cup, ref udlr.cdown, ref udlr.cleft, ref udlr.cright);
                    el.udlr = udlr;
                }
                else
                {
                    GetUDLR(el, ref udlr.up, ref udlr.down, ref udlr.left, ref udlr.right);
                    GetCellUDLR(el, ref udlr.cup, ref udlr.cdown, ref udlr.cleft, ref udlr.cright);
                    el.udlr = udlr;
                }
               if (el.CompressedElements.Count > 0)
                    UnPackCompress(el, udlr);
                el.GetAST(udlr);
                el.PhysicBehaviour(udlr);
                Simulator.me.tasks.Add(new StreamTempTS(el, udlr));
            }
            else
            {

            }

        }
    }
    public void UnPackCompress(Element el, UDLR udlr)
    {
        if (udlr.GetCount() < 4)
        {
            List<Element> removes = new();
            int count = el.CompressedElements.Count;
            void SetU((bool, Cell) u, Element el)
            {

                if (u.Item1)
                    if (u.Item2.element == null)
                    {
                        if (count > 0)
                        {
                            Element compress = el.CompressedElements[count - 1];
                            Simulator.me.tasks.Add(new CreateElementTS(compress.name, u.Item2.pos, compress.CompressedElements, compress.Temp));
                            removes.Add(compress);
                            count--;
                        }
                    }



            }
            SetU(udlr.cup, el);
            SetU(udlr.cdown, el);
            SetU(udlr.cleft, el);
            SetU(udlr.cright, el);
            el.CompressedElements.RemoveAll(x => removes.Contains(x));
        }
        else
        {
            (bool, Element) rand = new (bool, Element)[] { udlr.up, udlr.down, udlr.left, udlr.right }[UnityEngine.Random.Range(0, 4)];
            if (rand.Item1)
                if (!(rand.Item2 is NoElement))
                {
                    Element element = el.CompressedElements.ElementAt(el.CompressedElements.Count - 1);
                    rand.Item2.CompressedElements.Add(element);
                    el.CompressedElements.Remove(element);
                }
        }
    }
    public static void ReMagnet(out Vector2 X, out Vector2 Y, UDLR udlr, Element main)
    {

        X = new();
        Y = new();
        MagnetSet(udlr.up, main, ref X, ref Y);
        MagnetSet(udlr.down, main, ref X, ref Y);
        MagnetSet(udlr.left, main, ref X, ref Y);
        MagnetSet(udlr.right, main, ref X, ref Y);

    }
    public static void Diffusion(UDLR uldr, Element main)
    {
        if (Simulator.me.diffusion)
            if (random.NextDouble() > Simulator.me.DiffusionRate)
                if (!main.replaced)
                    if (main.AStype == AgreegateStateType.Liquid || main.AStype == AgreegateStateType.Gas)
                    {
                        (bool, Element) rand = new (bool, Element)[] { uldr.up, uldr.down, uldr.left, uldr.right }[UnityEngine.Random.Range(0, 4)];
                        if (rand.Item1)
                            if (!(rand.Item2 is NoElement))
                                if (rand.Item2.AStype == AgreegateStateType.Liquid || rand.Item2.AStype == AgreegateStateType.Gas)
                                {
                                    if (!rand.Item2.replaced)
                                        //Simulator.me.tasks.Add(new ReplaceTS(main, rand.Item2));
                                        Element.Replace(main, rand.Item2);
                                }
                    }
    }
    public static void Diffusion(Element other, Element main)
    {
        if (!main.replaced)
            if (main.AStype == AgreegateStateType.Liquid || main.AStype == AgreegateStateType.Gas)
            {

                if (!(other is NoElement))
                    if (other.AStype == AgreegateStateType.Liquid || other.AStype == AgreegateStateType.Gas)
                        if (random.NextDouble() > Mathf.Clamp01(1f / (main.pp.viscosity + 1)) || random.NextDouble() > Mathf.Clamp01(1f / (other.pp.viscosity + 1)))
                        {
                            //Simulator.me.tasks.Add(new ReplaceTS(main, other));
                            Element.Replace(main, other);
                        }
            }
    }
    public static void DensityDissection(UDLR udlr, Element main)
    {
        ReplaceDD(udlr.up, main);
        ReplaceDD(udlr.down, main);
        ReplaceDD(udlr.left, main);
        ReplaceDD(udlr.right, main);
    }
    public static void DensityDissection(bool tr, Element e, Element main)
    {
        if (BreakPointer.me.link != null)
            if (BreakPointer.me.ucode == main.ucode)
            {

            }
        ReplaceDD(tr, e, main);

    }
    public static void SearchDissolved(UDLR udlr, Solvent solvent, Element main)
    {
        if (udlr.up.Item1)
            if (udlr.up.Item2.IsSoluble(main.GetType()))
            {
                Element other = udlr.up.Item2;
                main.CompressedElements.AddRange(other.CompressedElements);
                Simulator.me.tasks.Add(new ClearElementsTS(other));
               
                solvent.dissolved = other;
                return;
            }
        if (udlr.down.Item1)
            if (udlr.down.Item2.IsSoluble(main.GetType()))
            {
                Element other = udlr.down.Item2;
                main.CompressedElements.AddRange(other.CompressedElements);
                Simulator.me.tasks.Add(new ClearElementsTS(other));
                solvent.dissolved = other;
                return;
            }
        if (udlr.left.Item1)
            if (udlr.left.Item2.IsSoluble(main.GetType()))
            {
                Element other = udlr.left.Item2;
                main.CompressedElements.AddRange(other.CompressedElements);
                Simulator.me.tasks.Add(new ClearElementsTS(other));
                solvent.dissolved = other;
                return;
            }
        if (udlr.right.Item1)
            if (udlr.right.Item2.IsSoluble(main.GetType()))
            {
                Element other = udlr.right.Item2;
                main.CompressedElements.AddRange(other.CompressedElements);
                Simulator.me.tasks.Add(new ClearElementsTS(other));
                solvent.dissolved = other;
                return;
            }
    }
    public static void SearchPushDissolved(Element main, UDLR udlr, Solvent solvent)
    {
        if (!udlr.up.Item1)
        {

            Simulator.me.CreateElement(solvent.dissolved.GetType().Name, main.cell.pos + Vector2Int.UnitY, main.Temp);

            solvent.dissolved = null;
            return;
        }
        if (!udlr.down.Item1)
        {
            Simulator.me.CreateElement(solvent.dissolved.GetType().Name, main.cell.pos - Vector2Int.UnitY, main.Temp);
            solvent.dissolved = null;
            return;
        }
        if (!udlr.left.Item1)
        {
            Simulator.me.CreateElement(solvent.dissolved.GetType().Name, main.cell.pos - Vector2Int.UnitX, main.Temp);
            solvent.dissolved = null;
            return;
        }
        if (!udlr.right.Item1)
        {
            Simulator.me.CreateElement(solvent.dissolved.GetType().Name, main.cell.pos + Vector2Int.UnitX, main.Temp);
            solvent.dissolved = null;
            return;
        }
    }
    public static void GetUDLR(Element main, ref (bool, Element) up, ref (bool, Element) down, ref (bool, Element) left, ref (bool, Element) right)
    {
        if (main.cell != null)
            if (Field.TryGetElement(main.cell.pos + Vector2.UnitY, out Element other))
            {
                up = new(true, other);

            }
        if (main.cell != null)
            if (Field.TryGetElement(main.cell.pos + Vector2.UnitX, out Element other))
            {
                right = new(true, other);
            }
        if (main.cell != null)
            if (Field.TryGetElement(main.cell.pos - Vector2.UnitY, out Element other))
            {
                down = new(true, other);
            }
        if (main.cell != null)
            if (Field.TryGetElement(main.cell.pos - Vector2.UnitX, out Element other))
            {
                left = new(true, other);
            }
    }
    public static void GetCellUDLR(Element main, ref (bool, Cell) up, ref (bool, Cell) down, ref (bool, Cell) left, ref (bool, Cell) right)
    {
        if (main.cell != null)
            if (Field.TryGetCell(main.cell.pos + Vector2.UnitY, out Cell other))
            {
                up = new(true, other);

            }
        if (main.cell != null)
            if (Field.TryGetCell(main.cell.pos + Vector2.UnitX, out Cell other))
            {
                right = new(true, other);
            }
        if (main.cell != null)
            if (Field.TryGetCell(main.cell.pos - Vector2.UnitY, out Cell other))
            {
                down = new(true, other);
            }
        if (main.cell != null)
            if (Field.TryGetCell(main.cell.pos - Vector2.UnitX, out Cell other))
            {
                left = new(true, other);
            }
    }
    public static void Gravitation(Element element,float strong=1)
    {
        if (element.AStype == AgreegateStateType.Gas)
            if (element.pp.m < 29)
            {
                element.velocity -= gravity*strong;
                return;
            }
        element.velocity += gravity * strong;
    }
    public static void StreamUSet((bool, Element) u, Element main)
    {
        if (u.Item1)
        {
            Element other = u.Item2;
            
                float sr = (other.Temp + main.Temp) / 2f;
                float speed = Mathf.Sqrt(main.pp.thermal_conductivity * other.pp.thermal_conductivity) / Element.maxTC;
                speed = Mathf.Pow(speed, 1 / Simulator.me.speedMoveTemp);
                other.Temp = Mathf.SmoothStep(other.Temp, sr,speed);        
                main.Temp = Mathf.SmoothStep(main.Temp, sr, speed);


            
           


        }
        if(main.name!="Fire")
        main.Temp = Mathf.Lerp(main.Temp, Simulator.me.TempAll, Simulator.me.speedMoveTempAll);
    }
    public static void ReplaceDD((bool, Element) u, Element main)
    {
        if (!main.replaced)
            if (u.Item1)
            {
                Element other = u.Item2;
                if (!(other is NoElement))
                    if (main.GetDensity() > other.GetDensity())
                    {
                        //Simulator.me.tasks.Add(new ReplaceTS(main, other));
                        Element.Replace(main, other);

                    }
                    else
                    {
                        // Element.Replace(other, main);
                    }

            }
    }
    public static void MagnetSet((bool, Element) u, Element main, ref Vector2 X, ref Vector2 Y)
    {
        if (u.Item1)
            if (main.cell != null)
                if (u.Item2.cell != null)
                {
                    Element other = u.Item2;
                    Vector2 dir = (main.cell.pos - other.cell.pos);
                    X = new(X.X + dir.X, 0);
                    Y = new(0, Y.Y + dir.Y);



                }
    }
    public static void ReplaceDD(bool tr, Element e, Element main)
    {
        if (!main.replaced)
            if (!e.replaced)
                if (tr)
                {
                    Element other = e;
                    if (!(other is NoElement))
                        if (main.GetDensity() > other.GetDensity())
                        {
                            //Simulator.me.tasks.Add(new ReplaceTS(main, other));
                            if (main.cell.pos.Y < other.cell.pos.Y)
                                Element.Replace(main, other);
                        }
                        else
                        {
                            if (other.cell.pos.Y < main.cell.pos.Y)
                                Element.Replace(main, other);
                        }

                }
    }
    public static void AddTempStreamU((bool, Element) u, float t)
    {
        if (u.Item1)
        {
            Element other = u.Item2;


            float buffer = other.Temp + t;


            other.Temp = buffer;



        }
    }
    public static void StreamTemp(Element main, UDLR udlr)
    {
        
        StreamUSet(udlr.up, main);
        StreamUSet(udlr.down, main);
        StreamUSet(udlr.left, main);
        StreamUSet(udlr.right, main);
        
    }
    public static void AddTemperature(Element main, UDLR udlr, float add)
    {
        main.Temp = Mathf.SmoothStep(main.Temp, Simulator.me.TempAll, Simulator.me.speedMoveTempAll);
        AddTempStreamU(udlr.up, add);
        AddTempStreamU(udlr.down, add);
        AddTempStreamU(udlr.left, add);
        AddTempStreamU(udlr.right, add);

    }
    public static void GasX(Element element)
    {

        float x = UnityEngine.Random.Range(-Simulator.me.amplitudeGasRandom, Simulator.me.amplitudeGasRandom);
        // float y = UnityEngine.Random.Range(-Simulator.me.amplitudeGasRandom, Simulator.me.amplitudeGasRandom);
        Vector2Int vel = new Vector2Int((int)x, 0) * Simulator.me.powerGasRandom;
        element.velocity += (Vector2)vel;
    }
    public static void GasY(Element element)
    {

        // float x = UnityEngine.Random.Range(-Simulator.me.amplitudeGasRandom, Simulator.me.amplitudeGasRandom);
        float y = UnityEngine.Random.Range(-Simulator.me.amplitudeGasRandom, Simulator.me.amplitudeGasRandom);
        Vector2Int vel = new Vector2Int(0, (int)y) * Simulator.me.powerGasRandom;
        element.velocity += (Vector2)vel;
    }
    public static void LiquidBehavior(Element element)
    {
        bool left = false, right = false, down = false;
        down = Field.IsBound(element.cell.pos + Vector2Int.UnitY);
        if (!down) down = Field.IsElement(element.cell.pos + Vector2Int.UnitY);
        if (!down) return;
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
    public static RaycastHit Raycast(Vector2 origin, Vector2 velocity, Element me)
    {
        RaycastHit hit = default;
        hit.distance = velocity.Length();
        hit.dir = Vector2.Normalize(velocity);
        Vector2 originPos = Vector2.Zero;
        Vector2 dolit = me.dolit;
        if (Field.TryGetElement(origin + velocity, out Element e))
            hit.finalelement = e;
        for (int i = 1; i <= velocity.Length(); i++)
        {
            Vector2 old_currentPoint = hit.dir * (i - 1) + dolit;
            Vector2 currentPoint = hit.dir * i + dolit;
            originPos = origin + currentPoint;
            if ((Vector2Int)currentPoint == Vector2Int.Zero) continue;
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
                hit.elementCollision = element;
                return hit;
            }

        }
        hit.typeCollision = RaycastHit.Collision.None;
        hit.position = originPos;
        me.dolit = Vector2.Zero;
        me.Glut(originPos);
        return hit;
    }
    public struct RaycastHit
    {
        public float distance;
        public Vector2Int position;
        public Vector2 dir;
        public Collision typeCollision;
        public Element element, elementCollision, finalelement;

        public RaycastHit(float distance, Vector2 position, Collision typeCollision, Element element, Vector2 dir, Element elementCollision, Element finalelement)
        {
            this.distance = distance;
            this.position = position;
            this.typeCollision = typeCollision;
            this.element = element;
            this.elementCollision = elementCollision;
            this.dir = dir;
            this.finalelement = finalelement;
        }

        public enum Collision
        {
            None,
            Bound,
            Element
        }
    }

}

