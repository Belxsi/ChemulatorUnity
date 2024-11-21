using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector2Int = System.Numerics.Vector2Int;
using Vector3 = UnityEngine.Vector3;

public class TestGeneratorBatching : MonoBehaviour
{
    public GameObject game,parent;
    public bool start;
    public Vector2Int Size;
   
    // Update is called once per frame
    public void Generator()
    {
        for(int x=0;x<Size.X;x++)
            for (int y = 0; y < Size.Y; y++)
            {
                Instantiate(game, new Vector3(x, y, 0), Quaternion.identity,parent.transform);
            }
    }
    void Update()
    {
        if (start)
        {
            start = false;
            Generator();
        }
    }
}
