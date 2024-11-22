

using UnityEngine;


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
    public Color EffectAS(Element color)
    {


        if(color is Solvent)
        {
            Solvent s = (Solvent)color;
            if (s.dissolved != null)
            {
                return (s.dissolved.vp.result + color.vp.result) / 2;
            }
        }
        if (Simulator.me.reupdate) color.vp.StaticColor();
        return color.vp.result;
      //  return element.AStype switch
      //  {
        //    AgreegateStateType.Solid => color ,
        //    AgreegateStateType.Gas => color ,
      //      _ => color,
      //  };
    }
    public Color GetColorElement()
    {
        if (!Simulator.me.IsTeplotv& !Simulator.me.IsCompresser)
        {
            if (element != null)
            {
                return EffectAS(element);
            }
            else
            {
                return Color.black;
            }
        }
        else
        {
            if (element != null)
            {
                if (Simulator.me.IsTeplotv)
                {
                    float t = (element.Temp - Simulator.me.teplovizorBound.x) / (Simulator.me.teplovizorBound.y - Simulator.me.teplovizorBound.x);
                    return Simulator.me.gradientTV.Evaluate(Mathf.Clamp01(t));
                }
                if (Simulator.me.IsCompresser)
                {
                    float t = element.CompressedElements.Count;
                    return Simulator.me.gradientTV.Evaluate(Mathf.Clamp01(t));
                }
                return Color.black;
            }
            else
            {
                return Color.black;
            }
        }
    }
}
