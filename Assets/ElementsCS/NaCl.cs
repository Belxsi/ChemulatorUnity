using UnityEngine;
public class NaCl : Element,Salt
{

    public NaCl() : base("NaCl", new Color(0.8f, 0.8f, 0.8f),new(){typeof(H2O)},new PhysicParameters(58, kelvin + 1465, kelvin + 800, 2.4f, 5f,7f,50F,-411),new(2.16f))
    {
    }
}
