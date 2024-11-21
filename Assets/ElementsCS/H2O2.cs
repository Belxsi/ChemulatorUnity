using UnityEngine;
public class H2O2 : Element,Oxide {
public H2O2() : base("H2O2", new Color(0.9137255f, 0.945098f,0.945098f),new() { typeof(H2O) }, new PhysicParameters(34,423f,273f,1f,5f,0.6f,2.62f,-187f), new(1.4f)){}}