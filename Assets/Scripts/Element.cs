using System.Collections.Generic;
using UnityEngine;


using System;
using System.Linq;


public abstract class Element
{
    public float E;
    public string name;
    public Cell cell;
    public Vector2 velocity;
    public Vector2 dolit;
    public DirLiqual dirUniXMove = DirLiqual.None;
    public float Temp = kelvin + 22;
    public const float kelvin = 273f,maxTC=5000F;
    public List<System.Type> soluble;
    public Physic.UDLR udlr;
    public AgreegateStateType AStype;
    public Reaction[] reactions;
    public PhysicParameters pp;
    public DensityPhaseStates dps;
    public VisualPixel vp;
    public bool replaced;
    public static int unicalcode;
    public int ucode;
    public List<Element> CompressedElements = new();
    public Reaction decomposition;
    public Dictionary<string,string> tags = new();
    public void AddTags(string tag)
    {
        if (!tags.TryAdd(tag, tag))
        {
            tags[tag] = tag;
        }
    }
    public override string ToString()
    {
        string st="";
        foreach(var t in CompressedElements)
        {
            st += t.ToString()+" ";
        }
        if (st == "") st = "нет";
        return name + " темп:" + Temp + " уникод:" + unicalcode + " сжатые:(" + st + ")";
    }
    public bool IsTag(string tag)
    {
        return tags.ContainsKey(tag);
    }
    public void RemoveTags(string tag)
    {
        if (tags.ContainsKey(tag))
        {
            tags.Remove(tag);
        }
    }
    public void ClearTags()
    {
        tags.Clear();
    }
    protected Element(string name, VisualPixel vp, List<System.Type> soluble,PhysicParameters pp,DensityPhaseStates dps)
    {
        ucode = unicalcode;
        unicalcode++;
        this.pp = pp;
        this.name = name;
        this.vp = vp;
        this.soluble = soluble;
        this.dps = dps;
        InitAST();
        InitReactions();
        
        

    }
    protected Element(string name, Color color, List<System.Type> soluble, PhysicParameters pp, DensityPhaseStates dps)
    {
        ucode = unicalcode;
        unicalcode++;
        this.pp = pp;
        this.name = name;
        this.vp = new(color);
        this.soluble = soluble;
        this.dps = dps;
        InitAST();
        InitReactions();



    }
    public void InitReactions()
    {
        if (!(this is NoElement))
            reactions = ChemistryReaction.LibrarryReactions[name];
    }
    public static string[] GetAllReagents(Reaction[] reactions,Element me)
    {
        List<string> sum=new();
        foreach(var item in reactions)
        {
            foreach (var s in item.Reagents)
                if(!sum.Contains(s))
                sum.Add(s);
        }
        sum.RemoveAll(x => x == me.name);
        return sum.ToArray();
    }
    public float GetDensity()
    {
        return AStype switch
        {
            AgreegateStateType.Solid => dps.solid,
            AgreegateStateType.Liquid => dps.liquid,
            AgreegateStateType.Gas => dps.gas,
            _ => dps.solid,
        };
    }
    public bool IsSoluble(System.Type type)
    {
        if (soluble != null)
        {
            return soluble.Contains(type);
        }
        else return false;
    }
    public AgreegateStateType AgregateTransition(AgreegateStateType type, Physic.UDLR udlr)
    {
        if (AStype != type) { 
            if (type == AgreegateStateType.Gas)
            {
                Physic.AddTemperature(this, udlr, -pp.evaporation_temperature);
            }
            AStype = type;

        }
        return type;
    }
    public void InitAST()
    {
        if (Temp < pp.TempMelt)
        {
            AStype = AgreegateStateType.Solid;
           

        }
        if (Temp >= pp.TempMelt & Temp < pp.TempBoil)
        {

            AStype= AgreegateStateType.Liquid;

        }
        if (Temp >= pp.TempBoil)
        {
            AStype= AgreegateStateType.Gas;
        }
      
    }
    public AgreegateStateType GetAST(Physic.UDLR udlr)
    {
        if (Temp < pp.TempMelt)
        {

            return AgregateTransition(AgreegateStateType.Solid, udlr);

        }
        if (Temp >= pp.TempMelt & Temp < pp.TempBoil)
        {
            
            return AgregateTransition(AgreegateStateType.Liquid, udlr);

        }
        if (Temp >= pp.TempBoil)
        {
            return AgregateTransition(AgreegateStateType.Gas, udlr);
        }
        return default;
    }
    public virtual void PhysicBehaviour(Physic.UDLR udlr)
    {
        if (cell != null)
        {
            Element el = this;
            //Physic.GetUDLR(el, ref udlr.up, ref udlr.down, ref udlr.left, ref udlr.right);
            int countuldr = udlr.GetCount();
            if (udlr.GetCount() != 4)
            {
                el.AgreegateState(udlr);
            }
            else
            {
                Physic.Diffusion(udlr, this);
                if (udlr.up.Item2.velocity.x == 0)
                    if(this.name!=udlr.up.Item2.name||this.AStype!= udlr.up.Item2.AStype)
                    Physic.DensityDissection(udlr.up.Item2 != null,this , udlr.up.Item2);
               // Physic.DensityDissection(udlr.up.Item2 != null, udlr.up.Item2, this );

            }
           
            if (el is Solvent)
            {
                Solvent solvent = (Solvent)el;
                if (solvent.dissolved == null)
                    if (el.AStype== AgreegateStateType.Liquid)
                    {
                        Physic.SearchDissolved(udlr, solvent, el);
                    }
                if (solvent.dissolved != null)
                {

                    if (el.AStype == AgreegateStateType.Gas)
                    {
                        Physic.SearchPushDissolved(el, udlr, solvent);
                    }
                    else
                    {
                        if (ChemistryReaction.IsGoAtmosphereReactionSolvent(this,solvent.dissolved, udlr, out Reaction resultd))
                        {
                            ChemistryReaction.StartReactionSolvent(this,solvent.dissolved, udlr, resultd);
                        }
                    }
                }
            }else
            if (ChemistryReaction.IsGoAtmosphereReaction(this, udlr, out Reaction result))
            {
                ChemistryReaction.StartReaction(this, udlr, result);
            }
            




        }
        else
        {

        }
    }
    public static T CreateElement<T>() where T : Element, new()
    {
        
        return new T();
    }
    public static Element CreateElement(string name)
    {
        Type type = System.Type.GetType(name);
        Element element = (Element)System.Activator.CreateInstance(type);
        return element;
    }
    public virtual void Move(Element element, Physic.UDLR udlr)
    {
        if (element.velocity == Vector2.zero) return;
        Physic.RaycastHit hit = Physic.Raycast(element.cell.pos, element.velocity, element);
        switch (hit.typeCollision)
        {
            case Physic.RaycastHit.Collision.None:
                element.Repos(hit.position);

                break;
            case Physic.RaycastHit.Collision.Bound:
                element.Repos(hit.position);

                break;
            case Physic.RaycastHit.Collision.Element:
                element.Repos(hit.position);
                if (element.velocity.x == 0) 
                    if(element.AStype!= hit.elementCollision.AStype)
                Physic.DensityDissection(hit.elementCollision != null, hit.elementCollision, element);
                break;
        }
    }
    public AgreegateStateType GetASTAll()
    {
        if (Simulator.me.TempAll < pp.TempMelt)
        {
            return AgreegateStateType.Solid;

        }
        if (Simulator.me.TempAll >= pp.TempMelt & Simulator.me.TempAll < pp.TempBoil)
        {
            return AgreegateStateType.Liquid;

        }
        if (Simulator.me.TempAll >= pp.TempBoil)
        {
            return AgreegateStateType.Gas;
        }
        return default;
    }
    public virtual void AgreegateState(Physic.UDLR udlr)
    {
        
        switch (AStype)
        {
            case AgreegateStateType.Solid:
                Physic.Gravitation(this);
                Move(this,udlr);
                velocity = Vector2.zero;


                break;
            case AgreegateStateType.Liquid:
                Physic.Gravitation(this);
                Move(this, udlr);
                velocity = Vector2.zero;
                Physic.LiquidBehavior(this);
                Move(this, udlr);
                velocity = Vector2.zero;

                break;
            case AgreegateStateType.Gas:
                Physic.Gravitation(this,1/9);
                Move(this, udlr);
                velocity = Vector2.zero;
                Physic.GasX(this);
                Move(this, udlr);
                velocity = Vector2.zero;
                Physic.GasY(this);
                Move(this, udlr);
                velocity = Vector2.zero;
                Physic.ReMagnet(out Vector2 X,out Vector2 Y,udlr,this);
                velocity = X;
                Move(this, udlr);
                velocity = Vector2.zero;
                velocity = Y;
                Move(this, udlr);
                velocity = Vector2.zero;
                break;
        }
    }
    public static T Clone<T>() where T : Element, new()
    {
        return new T();
    }
  //  public void Render(Vector2Int point)
  //  {
   //     if (Field.TryGetCell(point, out Cell cell))
          //  Simulator.me.graphic.RenderPixel(point, cell);
  //  }
    public void Glut(Vector2 pos)
    {
        dolit += pos - (Vector2)Graphic.Vector2ToInt(pos);
    }
    public void Repos(Vector2Int newpos)
    {


        bool rendering = false;
        if (newpos != cell.pos) rendering = true;
        Cell old = cell;
        if (Field.TryGetCell(newpos, out Cell newcell))
        {

            cell.element = null;
            cell = newcell;
            cell.element = this;

            if (rendering)
            {
                
                   
                Simulator.me.graphic.RenderPixel(newcell);
                
                Simulator.me.graphic.RenderPixel(old);
            }

        }


    }
    public static void Replace(Element a, Element b)
    {
        
        if (a.cell != null)
            if (b.cell != null)
            {
                Cell a_cell = a.cell;
                Cell b_cell = b.cell;
               
                a.cell = null;
                b.cell = null;
                a.cell = b_cell;
                b.cell = a_cell;
                a_cell.element = b;
                b_cell.element = a;
                Simulator.me.graphic.RenderPixel(a_cell);
                Simulator.me.graphic.RenderPixel(b_cell);

            }

    }
}
public enum DirLiqual
{
    R, L, None
}

public interface Solvent
{
    public Element dissolved { get; set; }
}
public interface NoElement{
    public string simpleName { get; set; }
}
public interface Metal { }
public interface NoMetal { }
public interface Acid { }
public interface Base { }
public interface Oxide { }
public interface Salt { }
public interface Hydride { }

public class Fire : Element,NoElement
{
    public float time;
    public bool isnull;

    public string simpleName { get; set ; }

    public Fire() : base( "Fire", new VisualPixel(new(1, 163/255f, 2/255f),0.5f,0.5f,0.25f),null,new PhysicParameters(1,0,1,2.4f,5f,maxTC,1,1),new DensityPhaseStates(0))
    {
        time = Time.time;
        simpleName = "Огонь";
    }
    public override void PhysicBehaviour( Physic.UDLR udlr)
    {
        Temp = kelvin + 900;
        if (!isnull) {
            float timelife = Time.time-time;
            if (timelife >0.5f)
            {
                Vector2 pos = cell.pos;
                Simulator.me.tasks.Add(new ClearElementsTS(this));
              
                isnull = true;
                return;
            }
            Element el = this;
           // Physic.GetUDLR(el, ref udlr.up, ref udlr.down, ref udlr.left, ref udlr.right);
          
            el.AgreegateState(udlr);
           
          //  Physic.DensityDissection(udlr, this);
        }
    }
    public override void AgreegateState(Physic.UDLR udlr)
    {
        Physic.Gravitation(this);
        Move(this, udlr);
        velocity = Vector2.zero;
        Physic.GasX(this);
        Move(this, udlr);
        velocity = Vector2.zero;
        Physic.GasY(this);
        Move(this, udlr);
    }
}
public class Bound : Element,NoElement
{

    public Bound() : base( "Bound",new VisualPixel(new(0.5f, 0.5f, 0.5f),0,0,0), null,new PhysicParameters(1, float.MaxValue - 1, float.MaxValue, 2.4f, 5f,maxTC,1,1),new(float.MaxValue))
    {
        simpleName = "Емкость";
    }

    public string simpleName { get ; set ; }

    public override void PhysicBehaviour( Physic.UDLR udlr)
    {
        Element el = this;
       // Physic.GetUDLR(el, ref udlr.up, ref udlr.down, ref udlr.left, ref udlr.right);
       
        
      
        
    }
}
[Serializable]
public struct VisualPixel{
    Color color;
    public Color result;
    public float metalice,smoothness,hue;
    public Color GetOriginalColor()
    {
        return color;
    }
    public VisualPixel(Color color, float metalice, float smoothness, float hue)
    {
        this.color = color;
        result = Color.black;
        this.metalice = metalice;
        this.smoothness = smoothness;
        this.hue = hue;
        InitColor();
    }
    public VisualPixel(Color color)
    {
        this.color = color;
        result = Color.black;
        metalice = 0;
        smoothness = 0;
        hue = 0;
        InitColor();
    }
    public void StaticColor()
    {
        System.Random random = Simulator.me.random;
        float mrgb = (float)random.NextDouble(), srgb = (float)random.NextDouble();
        float r = (float)(color.r * (1f - mrgb * Simulator.me.testvp.metalice));
        float g = (float)(color.g * (1f - mrgb * Simulator.me.testvp.metalice));
        float b = (float)(color.b * (1f - mrgb * Simulator.me.testvp.metalice));

        r = r + Mathf.Sin((float)(g * b * (1f - srgb * Simulator.me.testvp.smoothness)))* Simulator.me.testvp.smoothness;
        g = g + Mathf.Sin((float)(r * b * (1f - srgb * Simulator.me.testvp.smoothness)))* Simulator.me.testvp.smoothness;
        b = b + Mathf.Sin((float)(r * g * (1f - srgb * Simulator.me.testvp.smoothness)))* Simulator.me.testvp.smoothness;
        r = (float)(r * (1f - random.NextDouble() * Simulator.me.testvp.hue));
        g = (float)(g * (1f - random.NextDouble() * Simulator.me.testvp.hue));
        b = (float)(b * (1f - random.NextDouble() * Simulator.me.testvp.hue));
        result = new Color(r, g, b);
    }
    public void InitColor()
    {
        System.Random random = Simulator.me.random;
        float mrgb = (float)random.NextDouble(), srgb = (float)random.NextDouble();
        float r = (float)(color.r*( 1f - mrgb * metalice));
        float g = (float)(color.g * (1f - mrgb * metalice));
        float b = (float)(color.b * (1f - mrgb * metalice));

        r = r + Mathf.Sin((float)(g * b * (1f - srgb * smoothness))) * smoothness;
        g = g + Mathf.Sin((float)(r * b * (1f - srgb * smoothness))) * smoothness;
        b = b + Mathf.Sin((float)(r * g * (1f - srgb * smoothness))) * smoothness;
        r = (float)(r * (1f - random.NextDouble() * hue));
        g = (float)(g * (1f - random.NextDouble() * hue));
        b = (float)(b * (1f - random.NextDouble() * hue));
        result = new Color(r, g, b);
    }
}
public enum AgreegateStateType
{
    Solid, Gas, Liquid
}
public struct DensityPhaseStates
{
   public float solid, liquid, gas;

    public DensityPhaseStates(float solid, float liquid, float gas)
    {
        this.solid = solid;
        this.liquid = liquid;
        this.gas = gas;
    }
    public DensityPhaseStates(float baseP)
    {
        this.solid = baseP;
        this.liquid = baseP*0.9F;
        this.gas = baseP * 0.8F;
    }
}
public struct PhysicParameters
{

    public float m;
    public float TempBoil, TempMelt;   
    public float viscosity;
    public float evaporation_temperature;
    public float thermal_conductivity;
    public float heat_capacity;
    public float H;

    public PhysicParameters(float m,float tempBoil, float tempMelt, float viscosity, float evaporation_temperature, float thermal_conductivity,float heat_capacity,float H)
    {
        TempBoil = tempBoil;
        TempMelt = tempMelt;
        this.heat_capacity = heat_capacity;
        this.m = m;
        this.viscosity = viscosity;
        this.evaporation_temperature = evaporation_temperature;
        this.thermal_conductivity = thermal_conductivity;
        this.H = H;
    }
}
