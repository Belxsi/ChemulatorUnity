using UnityEngine;
using TMPro;
public class FieldUIStatistics : MonoBehaviour
{
    public int count;
    public string Name;
    public TextMeshProUGUI tmp;
    string result;
    void Awake()
    {
        Init();
    }
    public void Init()
    {
        result =  Name+": "+ count ;
        tmp.text = result;
        
    }
  
    // Update is called once per frame

}
