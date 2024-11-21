
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;
using Vector2 = System.Numerics.Vector2;
using Vector2Int = System.Numerics.Vector2Int;
using TMPro;
using UnityEngine.UI;
public class Simulator : MonoBehaviour
{
    public Physic physic;
    public System.Random random = new();
    public Graphic graphic;
    public Field field;
    public Vector2 startSizeWindow, startSizeImage, delta;
    public int indexes = 0;
    public List<List<Element>> Curlements = new List<List<Element>>();
    public static Simulator me;
    public float threadRate;
    public Gradient gradientTV;
    public Lever IsTeplotv;
    public bool updatepixels;
    public Lever IsCompresser;
    public UnityEngine.Vector2 teplovizorBound;
    public float amplitudeGasRandom, powerGasRandom;
    public float TempAll;
    public float speedMoveTemp, speedMoveTempAll;
    public float entropy;
    public bool diffusion;
    public float DiffusionRate;
    public float reupdateGraphicRate;
    public float KTempReactionGive;
    public bool IsInertedAtmosphere;
    public static bool pause;
    public VisualPixel testvp;
    public bool reupdate;
    public ChemistryReaction chemistryReaction;
    public SimulatorStatistic ss;
    public List<TaskSimulator> tasks = new();
    public TextMeshProUGUI info;
    public void CreateElement(Element element, Vector2Int pos, float Temp = Element.kelvin + 22)
    {
        if (!Field.IsElement(pos))
        {
            element.Temp = Temp;
            Field.SetElement(element, pos);
            OptimalAddCurlements(element);
        }
    }
    public Dictionary<string, int> GetCountElement(List<string> names)
    {
        Dictionary<string, int> result = new();
        foreach (var l in Curlements)
        {
            foreach (var n in names)
            {
                int sum = l.Count(x => x.name == n);
                l.FindAll(x =>
                {
                    if (x is Solvent solvent)
                    {
                        if (solvent.dissolved != null)
                            if (solvent.dissolved.name == n)
                            {
                                sum++;
                            }
                    }
                    return false;

                });
                l.FindAll(x =>
                {
                    sum += x.CompressedElements.Count(y => y.name == n);
                    return false;

                }
                );
                if (!result.TryAdd(n, sum)) result[n] += sum;
            }

        }
        return result;
    }
    public Element CreateElement(string nameclass, Vector2Int pos, float Temp = Element.kelvin + 22)
    {
        Type type = System.Type.GetType(nameclass);
        Element element = (Element)System.Activator.CreateInstance(type);
        if (!Field.IsBound(pos))
        {
            if (!Field.IsElement(pos))
            {
                element.Temp = Temp;
                if (Field.SetElement(element, pos))
                {
                    OptimalAddCurlements(element);
                    return element;
                }
                else
                {
                    Debug.LogError("Что то с сет елементом");
                    return null;
                }
            }
            else
            {

                if (Field.TryGetElement(pos, out Element el))
                {
                    if (!el.IsTag("clear"))
                    {
                        el.CompressedElements.Add(element);
                        return el;
                    }
                    else
                    {
                        Simulator.me.ClearElement(el);

                        return CreateElement(nameclass, pos, Temp);

                    }
                }
                Debug.LogError("Элемент уже есть");
                return null;
            }
        }
        else
        {
            Debug.LogError("Дебил чтоли за границей элементы делать?");
            return null;
        }

    }
    public void SetPause(bool value)
    {
        pause = value;
    }
    public void CreateElement<T>(Vector2Int pos, float Temp = Element.kelvin + 22) where T : Element, new()
    {
        Element element = new T();
        if (!Field.IsElement(pos))
        {
            element.Temp = Temp;
            Field.SetElement(element, pos);
            OptimalAddCurlements(element);
        }
    }
    public void CreateElement<T>(T t, Vector2Int pos, float Temp = Element.kelvin + 22) where T : Element, new()
    {
        Element element = t;
        if (!Field.IsElement(pos))
        {
            element.Temp = Temp;
            Field.SetElement(element, pos);
            OptimalAddCurlements(element);
        }
    }
    public List<Element> MinThread()
    {
        int count = int.MaxValue;
        List<Element> list = new List<Element>();
        foreach (var item in Curlements)
        {
            if (item.Count < count)
            {
                count = item.Count;
                list = item;
            }
        }
        return list;
    }

    public void ClearElement(Element element)
    {
        foreach (var item in Curlements.ToList())
        {

            item.Remove(element);


        }
        if (element.cell != null)
        {
            //element.Render(element.cell.pos);
            Cell cell = element.cell;
            cell.element = null;
            element.cell = null;
        }
    }
    public void OptimalAddCurlements(Element element)
    {
        MinThread().Add(element);

    }
    public void Start()
    {
        me = this;
        field = new Field(128, 128);
        chemistryReaction = new();
        graphic.StartLocal();
        //Field.CreateGradient();
        physic = new Physic();
        InitCurlements();
        // CreateElement(new H2O(), new Vector2Int(0, 0));
        //StartThreadPoolPhysUpdate();
        ss = new();
        StartCoroutine(enumerator());

    }
    public IEnumerator enumerator()
    {
        while (true)
        {
            threadRate =Mathf.SmoothStep(threadRate,  Time.deltaTime,0.25f);
            if (!pause)
            {
                for (int i = 0; i < 12; i++)
                {
                    UpdateSimulator(i);
                }

                
            }
            foreach (var task in tasks.ToArray())
            {
                task.Do();

            }
            tasks.Clear();
            yield return new WaitForSeconds(threadRate);
            if (updatepixels)
                for (int i = 0; i < 12; i++)
                {
                    UpdateRenderSimulator(i);
                }
            AddTask();
            reupdate = false;
            ss.Update();
            // var dt = GetCountElement(new() { "H2", "Na", "NaH" });
            //  foreach(var d in dt)
            // {
            //     Debug.Log(d.Key + ": " + d.Value);
            // }

        }
    }
    public void InitCurlements()
    {
        for (int i = 0; i < 12; i++) Curlements.Add(new List<Element>());
    }
    public void StartThreadPoolPhysUpdate()
    {

        bool work = true;

        List<IObservable<Unit>> observables = new();
        indexes = 0;
        for (int i = 0; i < 12; i++)
        {

            object I = i;

            observables.Add(Observable.FromCoroutine(AddTaskUpdatePhys));
            //   ).Subscribe(x=>Debug.Log(x));


            // WaitCallback callback = new WaitCallback(AddTaskUpdatePhys);
            // ThreadPool.QueueUserWorkItem(callback,i);
        }


        Observable.WhenAll(observables).ObserveOnMainThread().Subscribe(x =>
        {

            StartThreadPoolPhysUpdate();
        });

    }
    public delegate void AddTaskUpdatePhysDel(int threadcode);

    public delegate void UpdateL();
    public void AddTask()
    {


        //Update(threadcode);
        graphic.FinalRender();


        //mainWindow.pixels.Clear();
        //await mainWindow.Dispatcher.BeginInvoke(new UpdateL(mainWindow.UpdateLayout));



    }
    private void FixedUpdate()
    {
        // AddTask();
        if (Input.GetKeyUp(KeyCode.Tab))
        {
            foreach (var a in Curlements)
                foreach (var b in a)
                {
                    Debug.Log(b.cell.pos.ToString());
                }
        }

    }
    public IEnumerator AddTaskUpdatePhys()
    {
        int threadcode = indexes;
        indexes++;


        yield return new WaitForSeconds(threadRate);
        UpdateSimulator(threadcode);
        //  yield return Observable.Start(() => graphic.Render()).Subscribe();

        graphic.Render();





    }
    public void UpdateSimulator(object threadcode)
    {
        try
        {
            if (Curlements[(int)threadcode].Count > 0)
                physic.Update(Curlements[(int)threadcode]);
        }
        catch (Exception e)
        {
            Debug.LogException(e);
        }

    }
    public void UpdateRenderSimulator(object threadcode)
    {
        try
        {
            if (Curlements[(int)threadcode].Count > 0)
            {
                graphic.UpdateRender(Curlements[(int)threadcode]);

            }
        }
        catch (Exception e)
        {
            Debug.LogException(e);
        }

    }

}
public class SimulatorStatistic
{
    public Dictionary<string, int> countsElements=new();
    List<string> AllElements = new();
    public int GetCountElement(string name)
    {
        if (countsElements.TryGetValue(name, out int value))
        {
            return value;
        }
        return 0;
    }
    public Dictionary<string, int> GetCountElements(List<string> names)
    {
        Dictionary<string, int> result = new();
        foreach (var name in names)
            if (countsElements.ContainsKey(name))
            {
                result.Add(name, countsElements[name]);
            }
        return result;
    }
    public Dictionary<string, int> GetCountElements(Dictionary<string, string> names)
    {
        Dictionary<string, int> result = new();
        foreach (var name in names)
            if (countsElements.ContainsKey(name.Key))
            {
                result.Add(name.Key, countsElements[name.Key]);
            }
        return result;
    }
    public SimulatorStatistic()
    {
       
    }

    public void Init(List<string> list)
    {
        AllElements.Clear();
        foreach (var t in list)
        {
            AllElements.Add(t);
        }
    }
    public void Init(Dictionary< string,string> list)
    {
        AllElements.Clear();
        foreach (var t in list)
        {
            AllElements.Add(t.Key);
        }
    }
    public void AddInit(List<string> list)
    {
        
        foreach (var t in list)
        {
            AllElements.Add(t);
        }
    }
    public void AddInit(string l)
    {

        
            AllElements.Add(l);
        
    }
    public void Update()
    {
        if(AllElements.Count>0)
        countsElements = Simulator.me.GetCountElement(AllElements);
    }
}

public abstract class SwitchLeverActivator:MonoBehaviour
{
    public Button button;
    public bool click;
    public void Awake()
    {
        button.onClick.AddListener(Do);
    }
    public void LateUpdate()
    {
        click = false;
    }
    public abstract void Do();
   
}

