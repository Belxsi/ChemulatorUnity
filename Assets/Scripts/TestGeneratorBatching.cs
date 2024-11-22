using System.Collections;
using System.Collections.Generic;

using UnityEngine;



public class TestGeneratorBatching : MonoBehaviour
{
    public GameObject game,parent;
    public bool start;
    public Vector2Int Size;
   
    // Update is called once per frame
    public void Generator()
    {
        for(int x=0;x<Size.x;x++)
            for (int y = 0; y < Size.y; y++)
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
