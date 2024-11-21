using UnityEngine;
using Vector2 = System.Numerics.Vector2;

public class H2O : Element,Acid,Base, Solvent
{
    public H2O() : base("H2O", new VisualPixel(new Color(28/255f, 163 / 255f, 236 / 255f),0.1f,0.3f,0f), null,new PhysicParameters(18, kelvin + 100, kelvin + 0, 0.547f, 5f,0.6F,75,-275), new(0.9f, 1f, 0.9f))
    {
    }
    public override string ToString()
    {
        string s = "";
        if (dissolved != null) {
            s = "["+"растворенное вещество:"+ dissolved.ToString()+ "] ";
        }
        return s + base.ToString();
    }
    public override void Move(Element element,Physic.UDLR udlr)
    {
        if (element.velocity == Vector2.Zero) return;
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
                //Physic.UDLR udlr = new();
               // Physic.GetUDLR(this, ref udlr.up, ref udlr.down, ref udlr.left, ref udlr.right);
                if (udlr.GetCount() == 4)
                    Physic.Diffusion(udlr, this);
               // Physic.Diffusion(hit.elementCollision, this);
                if (element.velocity.X == 0)
                    if(element.AStype!= hit.elementCollision.AStype)
                    Physic.DensityDissection(hit.elementCollision != null, hit.elementCollision, element);
                break;
        }
    }
    public Element dissolved { get; set; }
}
