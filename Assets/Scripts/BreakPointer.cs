using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakPointer : MonoBehaviour
{
    public string name;
    public System.Numerics.Vector2 pos;
    public static BreakPointer me;
    public Element link;
    public bool getlink;
    public int ucode=-1;
    // Update is called once per frame
    void Start()
    {
        me = this;
    }
    public void Update()
    {
        if (link != null)
        {
            ucode = link.ucode;
            pos = link.cell.pos;
            link.vp.result = Color.red;
        }
    }
}
