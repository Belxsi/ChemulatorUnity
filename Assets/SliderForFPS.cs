using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class SliderForFPS : MonoBehaviour
{
    public TextMeshProUGUI text;
    public Slider slider;
    
    void Start()
    {
        
    }
    public void IsSetValue()
    {
        string s = "";
        if (slider.value >= 121) s = "Без границ";
            text.text = slider.value;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
