using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class WindowSearcher : Window
{
    public TMP_InputField inputField;
    public List<ElementButton> buis = new();
    public override void Load()
    {
        if (elements.Count == 0)
        {
            foreach (var t in name_elements)
                elements.Add(Element.CreateElement(t.Value));
            foreach (var t in ChemistryReaction.AllElements)
            {

                if(!(t is NoElement))
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
        foreach (var e in elements)
        {
            var t = Instantiate(prefab_EB, content).GetComponent<ElementButton>();
            buis.Add(t);
            t.Init(this);
            t.element = e;
        }
    }

    void Start()
    {
        Load();
        InitUI();
    }
    public void Devisitor(string text)
    {

        foreach (var bui in buis)
        {
            if ( bui.text.text.ToLower().Contains(text.ToLower()))
            {
                bui.gameObject.SetActive(true);

            }
            else
            {
                bui.gameObject.SetActive(false);
            }
        }
    }

    void Update()
    {
        Devisitor(inputField.text);
    }
}
