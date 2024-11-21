using UnityEngine;
public class NaOH : Element,Base {
public NaOH() : base("NaOH", new Color(0.9411765f, 0.9411765f,0.9411765f),new() { typeof(H2O)}, new PhysicParameters(40,1676f,596f,1f,5f,0.65f,59F, -496), new(2.13f)){}}