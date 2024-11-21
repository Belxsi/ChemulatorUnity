
using System;
using UnityEngine;
using UnityEditor;

[Serializable]

public struct Lever 
{

   
    
    public bool value;
    
    public override bool Equals(object obj)
    {
        return base.Equals(obj);
    }
    public override int GetHashCode()
    {
        return base.GetHashCode();
    }
    public void Switch()
    {
        value = !value;
    }
    public static bool operator !(Lever a)
    {
        return !a.value;
    }
    public static bool operator ==(Lever a, Lever b)
    {
        return a.value == b.value;
    }
    public static implicit operator bool(Lever a)
    {
        return a.value;
    }
    public static bool operator !=(Lever a,Lever b)
    {
        return a.value != b.value;
    }
}
