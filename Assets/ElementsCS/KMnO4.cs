using UnityEngine;

public class KMnO4 : Element,Salt
{
public KMnO4() : base("KMnO4", new VisualPixel(new (10.59F/100, 9.41F / 100, 19.22F / 100),0.33F,-0.5F,-0.5F),new() { typeof(H2O) }, new PhysicParameters(158,100272f,513f,1f,5f,144f,1f,-833f), new(2.7f)){}}