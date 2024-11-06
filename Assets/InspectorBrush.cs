using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Numerics;
using Vector2Int = System.Numerics.Vector2Int;

public class InspectorBrush : MonoBehaviour
{
    public Simulator simulator;
    public bool create,reset;
    public Vector2Int point;

    
    void Update()
    {
        if (create)
        {
            simulator.CreateElement(new H2O(), point);
            create = reset;
        }
    }
}
