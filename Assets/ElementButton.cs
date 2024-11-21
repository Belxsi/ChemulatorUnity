using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ElementButton : MonoBehaviour
{
    public Element element;
    public TextMeshProUGUI text;
    public Image image;
    public Window window;
    public void Init(Window window)
    {
        this.window = window;
    }
    // Update is called once per frame
    void Update()
    {
        if (!(element is NoElement))
        {
            text.text = element.name;
        }
        else text.text = ((NoElement)element).simpleName;
        image.color = element.vp.GetOriginalColor();
    }
    public void SetBrush()
    {

        InspectorBrush.main.type = element.name;
        window.Close();
    }
}
