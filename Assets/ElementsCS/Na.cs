using UnityEngine;
public class Na : Element,Metal
{
    public Na() : base("Na",new VisualPixel( new Color(0.9f, 0.9f, 0.9f),0.2f,1,0), null,new PhysicParameters(23, kelvin + 883, kelvin + 97, 0.742f, 5f,142f,28,0), new(0.96842f))
    {
    }
}
