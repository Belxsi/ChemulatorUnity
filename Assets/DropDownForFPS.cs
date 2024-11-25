using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using System.Linq;
public class DropDownForFPS : MonoBehaviour
{
    public TMP_Dropdown dropdown;
    public List<GameObject> interfaces=new();
    void Start()
    {
        
    }
    public void IsGetSwitch()
    {
       
                interfaces.FindAll(x => { x.SetActive(false); return false; });
                interfaces[dropdown.value].SetActive(true);
               
        
    }
    
    void Update()
    {
        
    }
}
