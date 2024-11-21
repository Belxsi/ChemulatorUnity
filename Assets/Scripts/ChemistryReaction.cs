using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

public class Atmospheric_Composition
{
    public Dictionary<string, float> dolyElements = new();
    public Atmospheric_Composition()
    {
        dolyElements = new()
        {
            { "N2", 0.755f },
            { "O2", 0.2315f },
            { "Ar", 0.01292f },
            { "CO2", 0.046f / 100f },
            { "Ne", 0.0014f / 100f },
            { "CH4", 0.000084f / 100f },
            { "He", 0.000073f / 100f },
            { "Kr", 0.003f / 100f },
            { "H2", 0.00008f / 100f },
            { "Xe", 0.00004f / 100f }

        };
    }
    public bool TryGetAtmosphere(string[] reagents,out List<Element> list)
    {
        list = new();
        foreach(var r in reagents)
        {
            if (dolyElements.TryGetValue(r, out float rand))
            {
                if (Simulator.me.random.NextDouble() < rand)
                {
                    list.Add(Element.CreateElement(r));
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
        return true;
    }
    public bool TryGetAtmosphere(string reagents,out Element result)
    {
        result = null;
       
            if (dolyElements.TryGetValue(reagents, out float rand))
            {
                if (Simulator.me.random.NextDouble() < rand)
                {
                    result= Element.CreateElement(reagents);
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        
        return true;
    }
    public bool TryGetAtmosphere(string reagents)
    {
       

        if (dolyElements.TryGetValue(reagents, out float rand))
        {
            if (Simulator.me.random.NextDouble() < rand)
            {
                
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }

        return true;
    }
    public bool InAtmosphere(string name)
    {
       
        if (dolyElements.ContainsKey(name)) {
            
            return true;
        }
        return false;
    }
    public bool InAtmosphere(string name, out Element element)
    {
        element = null;
        if(dolyElements.TryGetValue(name,out float d))
        if (Simulator.me.random.NextDouble() < d)
        {
            element = Element.CreateElement(name);
            return true;
        }
        return false;
    }
}
public class ChemistryReaction
{
    public static List<Reaction> AllReactions;
    public static Dictionary<string, Reaction[]> LibrarryReactions = new();
    public static Atmospheric_Composition Acomposit;
    public static Type[] AllElements;
    


    public static bool IsKat(string[] kat,int dist,Physic.UDLR udlr)
    {
        foreach(var item in kat)
        {
            if (SearchKat(dist, item, udlr))
            {
                return true;
            }
        }
        return false;
    }
    public static bool IsGoReaction(Element el, Physic.UDLR udlr, out Reaction result)
    {
        Reaction[] reactions = el.reactions;
        result = null;
        bool IsR((bool, Element) dl, out Reaction rr)
        {
            rr = null;
            foreach (var r in reactions)
            {
                if (r.CountR.Length == 2)
                {
                    if (dl.Item1)
                        if (!dl.Item2.IsTag("clear"))
                            if (dl.Item2.name != el.name)

                            if (r.Reagents.Contains(dl.Item2.name))
                                if (r.reverse == null)
                                {
                                    if (r.Kat == null || r.Kat.Length == 0)
                                    {
                                        rr = r;
                                        return true;
                                    }
                                    else
                                    {
                                        if (IsKat(r.Kat, 2, udlr))
                                        {

                                            rr = r;
                                            return true;
                                        }
                                    }

                                    
                                }
                                else
                                {
                                    if (el.Temp < r.reverse.TGetKelvin())
                                    {
                                        if (r.Kat == null || r.Kat.Length == 0)
                                        {
                                            rr = r;
                                            return true;
                                        }
                                        else
                                        {
                                            if (IsKat(r.Kat, 2, udlr))
                                            {

                                                rr = r;
                                                return true;
                                            }
                                        }
                                    }
                                }
                }
                else
                {
                    if (el.Temp >= r.TGetKelvin())
                    {
                        if (r.Kat == null || r.Kat.Length == 0)
                        {
                            rr = r;
                            return true;
                        }
                        else
                        {
                            if (IsKat(r.Kat, 2, udlr))
                            {

                                rr = r;
                                return true;
                            }
                        }

                    }
                }
            }
            return false;
        }
        if (IsR(udlr.up, out result)) return true;
        if (IsR(udlr.down, out result)) return true;
        if (IsR(udlr.left, out result)) return true;
        if (IsR(udlr.right, out result)) return true;
        return false;

    }
    public static bool IsGoAtmosphereReaction(Element el, Physic.UDLR udlr, out Reaction result)
    {
        Reaction[] reactions = el.reactions;
        result = null;
        bool IsR((bool, Element) dl, out Reaction rr)
        {
            rr = null;
            foreach (var r in reactions)
            {
                if (r.CountR.Length == 2)
                {
                    if (dl.Item1)
                        if (!dl.Item2.IsTag("clear"))
                            if (dl.Item2.name != el.name)

                            if (r.Reagents.Contains(dl.Item2.name))
                                if (r.reverse == null)
                                {
                                    if (r.Kat == null || r.Kat.Length == 0)
                                    {
                                        rr = r;
                                        return true;
                                    }
                                    else
                                    {
                                        if (IsKat(r.Kat, 2, udlr))
                                        {

                                            rr = r;
                                            return true;
                                        }
                                    }


                                }
                                else
                                {
                                    if (el.Temp < r.reverse.TGetKelvin() & el.Temp < r.MTGetKelvin() || el.Temp < r.reverse.TGetKelvin() & 0== r.MTGetKelvin())
                                    {
                                        if (r.Kat == null || r.Kat.Length == 0)
                                        {
                                            rr = r;
                                            return true;
                                        }
                                        else
                                        {
                                            if (IsKat(r.Kat, 2, udlr))
                                            {

                                                rr = r;
                                                return true;
                                            }
                                        }
                                    }
                                }
                }
                else
                {
                    if (el.Temp >= r.TGetKelvin() & el.Temp < r.MTGetKelvin() || el.Temp >= r.TGetKelvin() & 0 == r.MTGetKelvin())
                    {
                        if (r.Kat == null || r.Kat.Length == 0)
                        {
                            rr = r;
                            return true;
                        }
                        else
                        {
                            if (IsKat(r.Kat, 2, udlr))
                            {

                                rr = r;
                                return true;
                            }
                        }

                    }
                }
            }
            return false;
        }
        bool IsA(string dl, out Reaction rr)
        {
            rr = null;
            foreach (var r in reactions)
            {
                if (r.CountR.Length == 2)
                {
                    

                        if (dl != el.name)

                            if (r.Reagents.Contains(dl))
                                if (r.reverse == null)
                                {
                                    if (r.Kat == null || r.Kat.Length == 0)
                                    {
                                        rr = r;
                                        return true;
                                    }
                                    else
                                    {
                                        if (IsKat(r.Kat, 2, udlr))
                                        {

                                            rr = r;
                                            return true;
                                        }
                                    }


                                }
                                else
                                {
                                    if (el.Temp < r.reverse.TGetKelvin())
                                    {
                                        if (r.Kat == null || r.Kat.Length == 0)
                                        {
                                            rr = r;
                                            return true;
                                        }
                                        else
                                        {
                                            if (IsKat(r.Kat, 2, udlr))
                                            {

                                                rr = r;
                                                return true;
                                            }
                                        }
                                    }
                                }
                }
                else
                {
                    if (el.Temp >= r.TGetKelvin())
                    {
                        if (r.Kat == null || r.Kat.Length == 0)
                        {
                            rr = r;
                            return true;
                        }
                        else
                        {
                            if (IsKat(r.Kat, 2, udlr))
                            {

                                rr = r;
                                return true;
                            }
                        }

                    }
                }
            }
            return false;
        }
        if (IsR(udlr.up, out result)) return true;
        if (IsR(udlr.down, out result)) return true;
        if (IsR(udlr.left, out result)) return true;
        if (IsR(udlr.right, out result)) return true;

        
        if(!Simulator.me.IsInertedAtmosphere)
            if(udlr.GetCount()<4)
        foreach (var item in Element.GetAllReagents(reactions, el))
        {
            if (Acomposit.TryGetAtmosphere(item))
            {
                if (IsA(item, out result)) return true;
            }
           
        }
        return false;

    }
    public static bool IsGoAtmosphereReactionSolvent(Element main,Element sol, Physic.UDLR udlr, out Reaction result)
    {
        if(sol.reactions.Length>0) sol.InitReactions();
        Reaction[] reactions = sol.reactions;
        
        result = null;
        bool IsR((bool, Element) dl, out Reaction rr)
        {
            rr = null;
            foreach (var r in reactions)
            {
                if (r.CountR.Length == 2)
                {
                    if (dl.Item1)
                        if (!dl.Item2.IsTag("clear"))
                            if (dl.Item2.name != sol.name)

                            if (r.Reagents.Contains(dl.Item2.name))
                                if (r.reverse == null)
                                {
                                    if (r.Kat == null || r.Kat.Length == 0)
                                    {
                                        rr = r;
                                        return true;
                                    }
                                    else
                                    {
                                        if (IsKat(r.Kat, 2, udlr))
                                        {

                                            rr = r;
                                            return true;
                                        }
                                    }


                                }
                                else
                                {
                                    if (main.Temp < r.reverse.TGetKelvin() & main.Temp < r.MTGetKelvin() || main.Temp < r.reverse.TGetKelvin() & 0 == r.MTGetKelvin())
                                    {
                                        if (r.Kat == null || r.Kat.Length == 0)
                                        {
                                            rr = r;
                                            return true;
                                        }
                                        else
                                        {
                                            if (IsKat(r.Kat, 2, udlr))
                                            {

                                                rr = r;
                                                return true;
                                            }
                                        }
                                    }
                                }
                }
                else
                {
                    if (main.Temp >= r.TGetKelvin() & main.Temp < r.MTGetKelvin() || main.Temp >= r.TGetKelvin() & 0 == r.MTGetKelvin())
                    {
                        if (r.Kat == null || r.Kat.Length == 0)
                        {
                            rr = r;
                            return true;
                        }
                        else
                        {
                            if (IsKat(r.Kat, 2, udlr))
                            {

                                rr = r;
                                return true;
                            }
                        }

                    }
                }
            }
            return false;
        }
        bool IsA(string dl, out Reaction rr)
        {
            rr = null;
            foreach (var r in reactions)
            {
                if (r.CountR.Length == 2)
                {


                    if (dl != sol.name)

                        if (r.Reagents.Contains(dl))
                            if (r.reverse == null)
                            {
                                if (r.Kat == null || r.Kat.Length == 0)
                                {
                                    rr = r;
                                    return true;
                                }
                                else
                                {
                                    if (IsKat(r.Kat, 2, udlr))
                                    {

                                        rr = r;
                                        return true;
                                    }
                                }


                            }
                            else
                            {
                                if (main.Temp < r.reverse.TGetKelvin())
                                {
                                    if (r.Kat == null || r.Kat.Length == 0)
                                    {
                                        rr = r;
                                        return true;
                                    }
                                    else
                                    {
                                        if (IsKat(r.Kat, 2, udlr))
                                        {

                                            rr = r;
                                            return true;
                                        }
                                    }
                                }
                            }
                }
                else
                {
                    if (main.Temp >= r.TGetKelvin())
                    {
                        if (r.Kat == null || r.Kat.Length == 0)
                        {
                            rr = r;
                            return true;
                        }
                        else
                        {
                            if (IsKat(r.Kat, 2, udlr))
                            {

                                rr = r;
                                return true;
                            }
                        }

                    }
                }
            }
            return false;
        }
        if (IsR(udlr.up, out result)) return true;
        if (IsR(udlr.down, out result)) return true;
        if (IsR(udlr.left, out result)) return true;
        if (IsR(udlr.right, out result)) return true;
        if (!Simulator.me.IsInertedAtmosphere)
            if (udlr.GetCount() < 4)
                foreach (var item in Element.GetAllReagents(reactions, sol))
        {
            if (Acomposit.TryGetAtmosphere(item))
            {
                if (IsA(item, out result)) return true;
            }

        }
        return false;

    }
    public static void SearchReagents(int dist, ref List<Element> backpack, List<string> rule, Dictionary<string, int> count, Physic.UDLR udlr,Reaction reaction)
    {
        dist--;
        List<(bool, Element)> ps = new() { udlr.up, udlr.left, udlr.down, udlr.right };

        List<Physic.UDLR> udlrs = new();
        foreach (var p in ps)
        {
            if (p.Item1)
                if (!p.Item2.IsTag("clear"))
                {
                    if (p.Item2.Temp < reaction.TGetKelvin() || p.Item2.Temp > reaction.MTGetKelvin() & reaction.MTGetKelvin() != 0) return;
                    if (!backpack.Contains(p.Item2))

                    {
                        string finded = rule.Find(x => x == p.Item2.name);


                        if (finded != null & finded != "")
                        {
                            backpack.Add(p.Item2);
                            count[finded]--;
                            if (count[finded] <= 0) rule.Remove(finded);
                            udlrs.Add(p.Item2.udlr);
                        }
                        else
                        {
                            if (p.Item2 is Solvent)
                                if (((Solvent)p.Item2).dissolved != null)
                                {
                                    Element dis = ((Solvent)p.Item2).dissolved;
                                    finded = rule.Find(x => x == dis.name);
                                    if (finded != null & finded != "")
                                    {
                                        p.Item2.AddTags("dis");
                                        backpack.Add(p.Item2);
                                        count[finded]--;
                                        if (count[finded] <= 0) rule.Remove(finded);
                                        udlrs.Add(p.Item2.udlr);
                                    }
                                }
                        }
                    }
                }
        }
        if (dist > 0)
            if (rule.Count > 0)
            {
                foreach (var ud in udlrs)
                    if (ud != null)
                    {
                        SearchReagents(dist, ref backpack, rule, count, ud,reaction);

                    }
            }

    }
    public static bool SearchKat(int dist, string name, Physic.UDLR udlr)
    {
        dist--;
        List<(bool, Element)> ps = new() { udlr.up, udlr.left, udlr.down, udlr.right };
        List<Physic.UDLR> udlrs = new();
        foreach (var p in ps)
        {
            if (p.Item1)
                if (!p.Item2.IsTag("clear"))
                    if (p.Item2.name==name)
                {
                    return true;
                        

                }
                else
                {
                    if (p.Item2 is Solvent)
                        if (((Solvent)p.Item2).dissolved != null)
                            if (((Solvent)p.Item2).dissolved.name == name)
                                return true;
                    udlrs.Add(p.Item2.udlr);
                }
        }
        if (dist > 0)
            {
                foreach (var ud in udlrs)
                    if (ud != null)
                    {
                       return  SearchKat(dist, name, ud);

                    }
            }
        return false;
    }

    public static void StartReaction(Element el, Physic.UDLR udlr, Reaction result)
    {
        List<Element> reagents = new();
        List<Cell> reagentsCell = new();
        List<string> rule = result.Reagents.ToList();
        Dictionary<string, int> count = new();
        List<string> countP = new();
        System.Numerics.Vector2Int point = el.cell.pos;
        if (el.IsTag("clear")) return;
        if (el.Temp < result.TGetKelvin()||el.Temp>result.MTGetKelvin() & result.MTGetKelvin() != 0) return;
        for (int i = 0; i < result.Reagents.Length; i++)
        {
            count.Add(result.Reagents[i], result.CountR[i]);
        }

        for (int i = 0; i < result.Products.Length; i++)
            for (int j = 0; j < result.CountP[i]; j++)
            {
                countP.Add(result.Products[i]);
            }
        reagents.Add(el);
        count[el.name]--;
        if (count[el.name] <= 0) rule.Remove(el.name);
        if (udlr == null) return;
        SearchReagents(2, ref reagents, rule, count, udlr,result);
        if (rule.Count > 0)         
        {
            if(Acomposit.TryGetAtmosphere(rule.ToArray(),out List<Element> atmos) &udlr.GetCount() < 4){
                rule.Clear();
                reagents.AddRange(atmos);
            }
            else
            {
                return;
            }
        }
        
            foreach (var p in reagents)
            {
            if (p.cell != null)
                reagentsCell.Add(p.cell);
                
            }
            List<Element> compress = new();
            foreach (var e in reagents)
            {
                if (e.CompressedElements.Count > 0)
                {
                    compress.AddRange(e.CompressedElements);
                }
                if (e is Solvent)
                {
                    Solvent solvent = (Solvent)e;
                    if (solvent.dissolved != null)
                    {
                        compress.Add(solvent.dissolved);
                    }
                }
            e.AddTags("clear");
                Simulator.me.tasks.Add(new ClearElementsTS(e));

            }

            float Q = result.GetEndQ();
            int cCompress = Mathf.CeilToInt((float)compress.Count / countP.Count);
            List<Element>[] groups = new List<Element>[countP.Count];
            for (int i = 0; i < groups.Length; i++)
            {
                groups[i] = new();
            }
            for (int i = 0; i < compress.Count; i++)
            {
                int index = i / cCompress;
                groups[index].Add(compress[i]);
            }
            List<CreateElementTS> listCreated = new();
            for (int i = 0; i < countP.Count; i++)
            {

                if (i < reagentsCell.Count)
                {
                   
                // float Tend = (Q / (product.pp.heat_capacity)) * Simulator.me.KTempReactionGive + el.Temp;
                float Tend = Q * Simulator.me.KTempReactionGive + el.Temp;
                    CreateElementTS createElementTS = new CreateElementTS(countP[i], reagentsCell[i].pos, groups[i], Tend);
                    listCreated.Add(createElementTS);
                   
                }
                else
                {
                    if (listCreated.Count > 0)
                    {
                        
                    //float Tend = (Q / (product.pp.heat_capacity)) * Simulator.me.KTempReactionGive + el.Temp;
                       float Tend = Q * Simulator.me.KTempReactionGive + el.Temp;
                       
                    CreateElementTS createElementTS = new CreateElementTS(countP[i],point, groups[i], Tend);
                    //“Œ∆≈ —¿ÃŒ≈ —ƒ≈À¿… » ƒÀﬂ –≈¿ ÷»… –¿—“¬Œ–Œ¬


                }
                else
                {
                    
                }
                }



            }
        
    }
    
    public static void StartReactionSolvent(Element main,Element sol, Physic.UDLR udlr, Reaction result)
    {
        List<Element> reagents = new();
        List<Cell> reagentsCell = new();
        List<string> rule = result.Reagents.ToList();
        Dictionary<string, int> count = new();
        List<string> countP = new();
        if (main.IsTag("clear")) return;
        if (sol.IsTag("clear")) return;
        System.Numerics.Vector2Int point = main.cell.pos;
        if (main.Temp < result.TGetKelvin() || main.Temp > result.MTGetKelvin() & result.MTGetKelvin() != 0) return;
        for (int i = 0; i < result.Reagents.Length; i++)
        {
            count.Add(result.Reagents[i], result.CountR[i]);
        }

        for (int i = 0; i < result.Products.Length; i++)
            for (int j = 0; j < result.CountP[i]; j++)
            {
                countP.Add(result.Products[i]);
            }
        reagents.Add(sol);
        count[sol.name]--;
        if (count[sol.name] <= 0) rule.Remove(sol.name);
        if (udlr == null) return;
        SearchReagents(2, ref reagents, rule, count, udlr,result);
        
        if (rule.Count > 0)
           
        {
            if (Acomposit.TryGetAtmosphere(rule.ToArray(), out List<Element> atmos) & udlr.GetCount() < 4)
            {
                rule.Clear();
                reagents.AddRange(atmos);
            }
            else
            {
                return;
            }
        }
        
        foreach (var p in reagents)
        {
            if (p.cell != null&!p.IsTag("dis"))
                reagentsCell.Add(p.cell);

        }
        List<Element> compress = new();
        foreach (var e in reagents)
        {
            if (!e.IsTag("dis"))
            {
                if (e.CompressedElements.Count > 0)
                {
                    compress.AddRange(e.CompressedElements);
                }
                if (e is Solvent)
                {
                    Solvent solvent = (Solvent)e;
                    if (solvent.dissolved != null)
                    {
                        compress.Add(solvent.dissolved);
                    }
                }
                e.AddTags("clear");
                Simulator.me.tasks.Add(new ClearElementsTS(e));
            }
            else
            {
                Solvent solvent1 = (Solvent)e;
                Element dis = solvent1.dissolved;
                if (dis.CompressedElements.Count > 0)
                {
                    //compress.AddRange(dis.CompressedElements);
                }
                if (dis is Solvent)
                {
                    Solvent solvent = (Solvent)dis;
                    if (solvent.dissolved != null)
                    {
                        compress.Add(solvent.dissolved);
                    }
                }

                solvent1.dissolved = null;
            }

        }

        float Q = result.GetEndQ();
        int cCompress = Mathf.CeilToInt((float)compress.Count / countP.Count);
        List<Element>[] groups = new List<Element>[countP.Count];
        for (int i = 0; i < groups.Length; i++)
        {
            groups[i] = new();
        }
        for (int i = 0; i < compress.Count; i++)
        {
            int index = i / cCompress;
            groups[index].Add(compress[i]);
        }
        List<CreateElementTS> listCreated = new();
        for (int i = 0; i < countP.Count; i++)
        {

            if (i < reagentsCell.Count)
            {
                Element product = Element.CreateElement(countP[i]);
                //float Tend = (Q / (product.pp.heat_capacity)) * Simulator.me.KTempReactionGive + main.Temp;
                float Tend = Q * Simulator.me.KTempReactionGive + main.Temp;
                CreateElementTS createElementTS = new CreateElementTS(countP[i], reagentsCell[i].pos, groups[i], Tend);
                listCreated.Add(createElementTS);
                Simulator.me.tasks.Add(createElementTS);
            }
            else
            {
                if (listCreated.Count > 0)
                {
                    Element product = Element.CreateElement(countP[i]);
                   // float Tend = (Q / (product.pp.heat_capacity)) * Simulator.me.KTempReactionGive + main.Temp;
                    float Tend = Q * Simulator.me.KTempReactionGive + main.Temp;
                    product.Temp = Tend;
                    CreateElementTS createElementTS = new CreateElementTS(countP[i], point, groups[i], Tend);


                }
                else
                {
                    Element product = Element.CreateElement(countP[i]);
                  
                    //float Tend = (Q / (product.pp.heat_capacity)) * Simulator.me.KTempReactionGive + main.Temp;
                    float Tend = Q * Simulator.me.KTempReactionGive + main.Temp;
                    product.Temp = Tend;
                    main.CompressedElements.Add(product);
                }
            }



        }
        Solvent s = (Solvent)main;
        s.dissolved = null;

    }
    public bool FindInterface(Type type, object criteria)
    {
        return type.Name == (string)criteria;
    }
    public void InitLibrrary()
    {
        for (int i = 0; i < AllElements.Length; i++)
        {
            string name = AllElements[i].Name;
            Reaction[] reactions = AllReactions.FindAll(x => x.Reagents.Contains(name)).ToArray();
            LibrarryReactions.Add(name, reactions);
        }
    }
    public void GetAllElements()
    {
        Type ourtype = typeof(Element); // ¡‡ÁÓ‚˚È ÚËÔ
        IEnumerable<Type> elist = Assembly.GetAssembly(ourtype).GetTypes().Where(type => type.IsSubclassOf(ourtype));  // using System.Linq
        List<Type> list = elist.ToList();
        /*
        foreach (var l in elist.ToList())
        {
            Type[] types = l.FindInterfaces(FindInterface, "NoElement");
            if (types.Length > 0)
                if (types[0].Name == "NoElement")
                {
                    list.Remove(l);
                }
        }
        */
        foreach (Type itm in list)
        {
            Debug.Log(itm);
        }
        AllElements = list.ToArray();
    }
    public void SearchReverseReaction()
    {
        for(int i = 0; i < AllReactions.Count; i++)
        {
            Reaction reaction = AllReactions[i];
            if (reaction.CountP.Length == 1)
            {
                Reaction reverse= AllReactions.Find(x =>
                {
                    if (x.Reagents.Length == 1)
                    {
                        return x.Reagents.Contains(reaction.Products[0]);


                    }
                    else return false;
                });
                if (reverse != null)
                {
                    reaction.reverse = reverse;
                }
            }
        }
    }
    public ChemistryReaction()
    {
        AllReactions = new List<Reaction>(ReactionMakeManager.LoadR());
        Acomposit = new();
        SearchReverseReaction();
        GetAllElements();
        InitLibrrary();
    }

}
public class Reaction
{
    public string Formula;
    public string[] Reagents;
    public int[] CountR;
    public string[] Products;
    public int[] CountP;
    public float T;

    public float MT;
    public float TGetCells()
    {
        return T;
    }
    public float TGetKelvin()
    {
        return T+273;
    }
    public void TSetCells(float T)
    {
        this.T=T;
    }
    public void TSetKelvin(float T)
    {
        this.T= T + 273;
    }
    public float MTGetCells()
    {
        if (MT == 0) return float.MaxValue;
        return MT;
    }
    public float MTGetKelvin()
    {
        if (MT == 0) return float.MaxValue;
        return MT + 273;
    }
    public void MTSetCells(float MT)
    {
        this.MT = MT;
    }
    public void MTSetKelvin(float MT)
    {
        this.MT = MT + 273;
    }
    public float Q;
    public string[] Kat;
    public Reaction reverse;
    public Reaction(string formula, string[] reagents, string[] products)
    {
        Formula = formula;
        Reagents = reagents;
        Products = products;
    }
    public float SumAndMulti(float[] vs, float[] mn)
    {
        float sum = 0;
        for (int i = 0; i < vs.Length; i++)
        {
            sum += vs[i] * mn[i];
        }
        return sum;
    }
    public float SumAndMulti(float[] vs, int[] mn)
    {
        float sum = 0;
        for (int i = 0; i < vs.Length; i++)
        {
            sum += vs[i] * mn[i];
        }
        return sum;
    }
    public float GetEndQ()
    {
        float[] Aentalpys = new float[Reagents.Length];
        float[] Bentalpys = new float[Products.Length];
        for (int i = 0; i < Aentalpys.Length; i++)
        {
            Aentalpys[i] = Element.CreateElement(Reagents[i]).pp.H;
        }
        for (int i = 0; i < Bentalpys.Length; i++)
        {
            Bentalpys[i] = Element.CreateElement(Products[i]).pp.H;
        }
        float Q = SumAndMulti(Aentalpys, CountR) - SumAndMulti(Bentalpys, CountP);
        return Q;


    }
}
public abstract class TaskSimulator
{
    public abstract void Do();

}
public class CreateElementTS : TaskSimulator
{
    public string nameclass;
    public System.Numerics.Vector2Int pos;
    public float Temp = Element.kelvin + 22;
    public List<Element> compress = new();
    public CreateElementTS(string nameclass, System.Numerics.Vector2Int pos, List<Element> compress, float temp = Element.kelvin + 22)
    {
        this.nameclass = nameclass;
        this.pos = pos;
        Temp = temp;
        this.compress = compress;
        Element el= Simulator.me.CreateElement(nameclass, pos, Temp);
        foreach(var t in compress)
        {
            el.CompressedElements.Add(t);
        }
    }

    public override void Do()
    {
       // Simulator.me.CreateElement(nameclass, pos, Temp).CompressedElements.AddRange(compress);
    }
}
public class ClearElementsTS : TaskSimulator
{
    public Element el;

    public ClearElementsTS(Element el)
    {
        this.el = el;
        Simulator.me.ClearElement(el);
    }

    public override void Do()
    {
        //Simulator.me.ClearElement(el);
    }
}
public class RenderPixel : TaskSimulator
{
    public Graphic.Pixel pixel;

    public RenderPixel(Graphic.Pixel pixel)
    {
        this.pixel = pixel;
    }

    public override void Do()
    {
        Simulator.me.graphic.SetPixel(pixel.x,pixel.y,pixel.Color);
    }
}
public class StreamTempTS : TaskSimulator
{
    public Element el;
    public Physic.UDLR udlr;
    public StreamTempTS(Element el,Physic.UDLR udlr)
    {
        this.el = el;
        this.udlr = udlr;
    }

    public override void Do()
    {
        Physic.StreamTemp(el,udlr);
    }
}
public class ReplaceTS : TaskSimulator
{
    public Element a, b;

    public ReplaceTS(Element a, Element b)
    {
        this.a = a;
        this.b = b;
        a.replaced = true;
        b.replaced = true;
    }

    public override void Do()
    {
        Element.Replace(a, b);
        a.replaced = false;
        b.replaced = false;
    }
}
