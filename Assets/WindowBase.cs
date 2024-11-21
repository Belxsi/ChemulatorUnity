using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using com.cyborgAssets.inspectorButtonPro;
using System;
using UnityEngine.EventSystems;
using UnityEngine.Events;
public class WindowBase : Window
{
    public override void Load()
    {
        if (elements.Count == 0)
        {
            foreach (var t in name_elements)
                elements.Add(Element.CreateElement(t.Value));
            foreach (var t in ChemistryReaction.AllElements)
            {

                if (t.GetInterface(typegroup)!=null)
                {
                    if (!name_elements.ContainsKey(t.Name))
                    {
                        elements.Add(Element.CreateElement(t.Name));
                    }
                }
            }
        }
    }
    public void InitUI()
    {
        foreach(var e in elements)
        {
            var t = Instantiate(prefab_EB, content).GetComponent<ElementButton>();
            t.Init(this);
            t.element=e;
        }
    }

    void Start()
    {
        Load();
        InitUI();
    }


    void Update()
    {

    }
}
public abstract class Window : MonoBehaviour, RectButton
{
    public Rect rect;
    public RectTransform me;
    public Dictionary<string,string> name_elements = new();
    public List<Element> elements = new();
    public GameObject prefab_EB;
    public Transform content;
    public string typegroup;
    public Vector2 offset;
    public static bool drag;
    [ProButton]
    public void LoadRect()
    {
        me.localPosition = new(rect.x, rect.y);
        me.sizeDelta = rect.size;

    }
    [ProButton]
    public void SaveRect()
    {
        rect = me.rect;
    }
    public void Close()
    {
        gameObject.SetActive(false);
    }
    public void Open()
    {
        gameObject.SetActive(true);
    }
    public void BeginDrag()
    {
        offset = transform.localPosition - Input.mousePosition;
        drag = true;
    }
    public void Drag()
    {

        transform.localPosition =(Vector2)Input.mousePosition+offset;
    }
    public void EndDrag()
    {
        offset = transform.localPosition - Input.mousePosition;
        drag = false;
    }
    public abstract void Load();
    
  
}
public interface RectButton
{
    
    public abstract void SaveRect();

    
    public abstract void LoadRect();
}
