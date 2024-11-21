using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.IO;
using System.Linq;

public class ReactionMakeManager : MonoBehaviour
{
    public TMP_InputField searcher;
    public Transform contents;
    public GameObject prefab_reactionUI;
    public Reaction[] reactions;
    public List<string> formuls=new();
    public List<ReactionUI> ruis=new();
    public static ReactionMakeManager me;
    public void AddFormule()
    {
        string text = searcher.text;
        formuls.Add(text);
        NewRUI(text);
        searcher.text = "";

    }
    public void RemoveFormule()
    {
        string text = searcher.text;
        formuls.RemoveAll(x=>x==text);
        RemoveRUI(text);
        searcher.text = "";
    }
    public void Save()
    {
        List<Reaction> new_reactions = new();
       foreach(var f in formuls)
        {
            new_reactions.Add(StringToReaction(f));
        }
        SaveR(new_reactions.ToArray());

    }
    public void Load()
    {
        InitOldData();
        InitRUI();
    }
    public void NewRUI(string formula)
    {
        ReactionUI rui = Instantiate(prefab_reactionUI, contents).GetComponent<ReactionUI>();
        rui.reaction = formula;
        ruis.Add(rui);
    }
    public void RemoveRUI(string formula)
    {
        List<ReactionUI> removed= ruis.FindAll(x => x.reaction == formula);
        foreach(var r in removed)
        {
            ruis.Remove(r);
            Destroy(r.gameObject);
        }
    }
    public void SaveR(Reaction[] r)
    {
        string path = Application.streamingAssetsPath + "\\" + "Reactions.json";
        // JSON json = new(path);
        // RJclass r = JsonUtility.FromJson<RJclass>(path);
        string contents = "";
        foreach (var t in r)
        {
            contents += JsonUtility.ToJson(t) + '\n';
        }
        File.WriteAllText(path, contents);
    }
    public static Reaction[] LoadR()
    {
        string path = Application.streamingAssetsPath + "\\" + "Reactions.json";
        // JSON json = new(path);
        // RJclass r = JsonUtility.FromJson<RJclass>(path);
        // Reaction[] r = new[] { new Reaction("2Na+2H20=2NaOH+H2", new[] { "Na", "H2O" }, new[] { "NaOH", "H2" }), new Reaction("2Na+2H20=2NaOH+H2", new[] { "Na", "H2O" }, new[] { "NaOH", "H2" }) };
        string[] r = File.ReadAllLines(path);
        List<Reaction> result = new();
        foreach (var t in r)
        {
            result.Add(JsonUtility.FromJson<Reaction>(t));
        }
        return result.ToArray();
    }
    public string KatToString(string[] Kat)
    {
        string sum="";
       
        for(int i=0;i<Kat.Length;i++)
        {
            if (i + 1 < Kat.Length)
            {
                sum += Kat[i] + ",";
            }
            else
            {
                sum += Kat[i];
            }
            
        }
        return sum;

    }
    public void InitOldData()
    {
        reactions = LoadR();
        formuls.Clear();
        foreach (var t in reactions)
        {
            string s="";
            s += t.Formula;
            if(t.TGetKelvin() != 0)
            s += " T="+t.TGetKelvin();
            if (t.Q != 0)
                s += " Q=" + t.Q;
            if (t.MTGetKelvin() != 0)
                s += " MT=" + t.MTGetKelvin();
            if (t.Kat!=null)
                if (t.Kat.Length>0)
                    s += " Kat=[" + KatToString(t.Kat)+"]";
            formuls.Add(s);
        }
    }
    public void SetTextInput(string text)
    {
        searcher.text = text;
    }
    public void DeleteAllRuis()
    {
        foreach (var r in ruis.ToList())
        {
            ruis.Remove(r);
            Destroy(r.gameObject);
        }
    }
    public void InitRUI()
    {
        DeleteAllRuis();
       
        foreach (var t in formuls)
        {
            NewRUI(t);
        }
    }
    void Start()
    {
        me = this;
        Load();
       
        //StringToReaction("2Na+2H2O=2NaOH+H2 T=295");
    }
    public Reaction StringToReaction(string s)
    {
        string[] subject = s.Split(' ', '=');
        string left = subject[0];
        string[] reagents, products;
        List<int> CountR=new(), CountP=new();
        string right = subject[1];
        string T="",Q="",MT="";
        List<string> Kat=null;
        for (int i = 2; i < subject.Length; i++)
        {
            switch (subject[i]) {
                case "T":
                    T = subject[i+1];

                    break;
                case "Q":
                    Q= subject[i + 1];

                    break;
                case "MT":
                    MT = subject[i + 1];

                    break;
                case "Kat":

                    Kat =new List<string>( subject[i+1].Split('[', ']', ','));
                    Kat.RemoveAll(x => x == "");
                    
                    break;
            }
        }
        string nodigitleft = "", nodigitright="";
        if (char.IsLetter(left[0]))
        {
            CountR.Add(1);
        }
        for (int i=0;i<left.Length;i++)
        {

            if (char.IsDigit(left[i]))
                if (i + 1 < left.Length)
                    if (i - 1 >=0)
                       
                        {
                        if (!char.IsLetter(left[i - 1]))
                        {
                            int d = i - 1;
                            string ddd = "";
                            do
                            {
                                d++;
                                ddd += left[d];
                                i = d;
                            }
                            while (char.IsDigit(left[d + 1]) & (d + 1 < left.Length));






                            CountR.Add(int.Parse(ddd));
                            continue;
                        }

                        
                    }
                    else
                    {
                        int d = i - 1;
                        string ddd = "";
                        do
                        {
                            d++;
                            ddd += left[d];

                        }
                        while (char.IsDigit(left[d + 1]) & (d + 1 < left.Length));






                        CountR.Add(int.Parse(ddd));
                        continue;
                    }
           
            if (left[i]=='+')
                 if (i + 1 < left.Length)
                    if(char.IsLetter(left[i + 1]))
                    {
                        CountR.Add(1);
                    }
            nodigitleft += left[i];

        }
        reagents = nodigitleft.Split('+');
        if (char.IsLetter(right[0]))
        {
            CountP.Add(1);
        }
        for (int i = 0; i < right.Length; i++)
        {

            if (char.IsDigit(right[i]))
                if (i + 1 < right.Length)
                    if (i - 1 >= 0)
                        
                    {
                        if (!char.IsLetter(right[i - 1]))
                        {
                            int d = i - 1;
                            string ddd = "";
                            do
                            {
                                d++;
                                ddd += right[d];
                                i = d;

                            }
                            while (char.IsDigit(right[d + 1]) & (d + 1 < right.Length));






                            CountP.Add(int.Parse(ddd));
                            continue;
                        }
                    }
                    else
                    {
                        int d = i - 1;
                        string ddd = "";
                        do
                        {
                            d++;
                            ddd += right[d];
                                i = d;

                            }
                        while (char.IsDigit(right[d + 1]) & (d + 1 < right.Length));






                        CountP.Add(int.Parse(ddd));
                        continue;
                    }

            if (right[i] == '+')
                if (i + 1 < right.Length)
                    if (char.IsLetter(right[i + 1]))
                    {
                        CountP.Add(1);
                    }
            
            nodigitright += right[i];

        }
        products = nodigitright.Split('+');
        Reaction reaction = new(left +"="+ right, reagents, products);
        reaction.CountR = CountR.ToArray();
        reaction.CountP = CountP.ToArray();
        if (T != "") reaction.TSetCells(int.Parse(T));
        if (Q != "") reaction.Q = float.Parse(Q);
        if (MT != "") reaction.MTSetCells(int.Parse(MT));
        if (Kat != null) reaction.Kat = Kat.ToArray();
        return reaction;
    }
    public void Devisitor(string text)
    {
       
        foreach(var rui in ruis){
            if (rui.text.text.Contains(text))
            {
                rui.gameObject.SetActive(true);

            }
            else
            {
                rui.gameObject.SetActive(false);
            }
        }
    }
    void Update()
    {
        Devisitor(searcher.text);
    }
}
