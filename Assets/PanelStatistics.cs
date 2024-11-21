using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class PanelStatistics : MonoBehaviour
{
    public Toggle IsAllElement;
    public TMP_InputField inputField;
    public WindowStatistics ws;
    public bool init_allelements,init_ui;
    // Update is called once per frame
    public void Init()
    {
        if (IsAllElement.isOn)
        {
            if (!init_allelements)
            {
                ws.name_elements.Clear();
                foreach (var t in ChemistryReaction.AllElements)
                {
                    ws.name_elements.Add(t.Name,t.Name);
                }
                init_allelements = true;
            }
        }
        else
        {
            if (init_allelements)
            {
                ClearUI();
                ws.name_elements.Clear();
                
                init_allelements = false;
            }
        }
               
    }
    public void UpdateSUI()
    {
        
        var st = Simulator.me.ss.GetCountElements(ws.name_elements);
        foreach(var sui in ws.suis)
        {
            sui.count = st[sui.Name];
            sui.Init();
        }
    }
    public void IsTrueAllElement()
    {
        Init();
        if(init_allelements)
        InitUI();
    }
    public void InitAddedElement(string element)
    {
        Simulator.me.ss.AddInit(element);
        int st = Simulator.me.ss.GetCountElement(element);
        var oldsui= ws.suis.Find(x => x.Name==element);
        if (oldsui != null)
        {
            ws.suis.Remove(oldsui);
            Destroy(oldsui.gameObject);
        }
        var sui = Instantiate(ws.prefab_EB, ws.content).GetComponent<FieldUIStatistics>();
            sui.count = st;
            sui.Name = element;
            ws.suis.Add(sui);
        
    }
    public void ClearUI()
    {
        init_ui = false;
        ws.suis.FindAll(x =>
        {
            Destroy(x.gameObject);
            return false;
        });
        ws.suis.Clear();
    }
    public void ClearUI(List<string> names)
    {
       List<FieldUIStatistics> list= ws.suis.FindAll(x =>names.Contains(x.Name));
        foreach (var t in list)
        {
            ws.suis.Remove(t);
            Destroy(t.gameObject);
        }
        
    }
    public void InitUI()
    {
        if (!init_ui)
        {
            ClearUI();
            Simulator.me.ss.Init(ws.name_elements);
            var st = Simulator.me.ss.GetCountElements(ws.name_elements);
            foreach (var e in st)
            {

                var sui = Instantiate(ws.prefab_EB, ws.content).GetComponent<FieldUIStatistics>();
                sui.count = e.Value;
                sui.Name = e.Key;
                ws.suis.Add(sui);
            }
            init_ui = true;
        }
    }
    public void AddElements()
    {
        if (!IsAllElement.isOn)
        {
            var m = inputField.text.Split(',', ' ');
            List<string> list = m.ToList();
            list.RemoveAll(x => x == "" || x == " ");
            foreach (var t in list) {
                if (!ws.name_elements.ContainsKey(t))
                    ws.name_elements.Add(t, t);
                    }
            if (!init_ui)
            {
                InitUI();
            }
            else
            {
                foreach (var t in list)
                    InitAddedElement(t);
            }
        }
        
    }
    public void RemoveElements()
    {
        if (!IsAllElement.isOn)
        {
            var m = inputField.text.Split(',', ' ');
            List<string> list = m.ToList(), removed=new() ;
            list.RemoveAll(x => x == "" || x == " ");
            
            foreach (var t in list)
            {
                if (ws.name_elements.ContainsKey(t))
                    removed.Add(t);
                
            }
            
            if (!init_ui)
            {
                InitUI();
            }
            else
            {
                ClearUI(removed);
            }
        }

    }
}
