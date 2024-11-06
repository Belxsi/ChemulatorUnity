
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Threading;
using UnityEngine;
using Vector2 = System.Numerics.Vector2;
using Vector2Int = System.Numerics.Vector2Int;
using UniRx;
using System.Collections;

public class Simulator : MonoBehaviour
{
    public Physic physic;
    public Graphic graphic;
    public Field field;
    public Vector2 startSizeWindow, startSizeImage, delta;
    public int indexes=0;
    public List<List<Element>> Curlements = new List<List<Element>>();
    public static Simulator me;
    public float threadRate;
    
    public void CreateElement(Element element, Vector2Int pos) 
    {
        if (!Field.IsElement(pos))
        {
            Field.SetElement(element, pos);
            OptimalAddCurlements(element);
        }
    }
    public List<Element> MinThread()
    {
        int count = int.MaxValue;
        List<Element> list = new List<Element>();
        foreach(var item in Curlements)
        {
            if(item.Count<count)
            {
                count = item.Count;
                list = item;
            }
        }
        return list;
    }
    public void OptimalAddCurlements(Element element)
    {
        MinThread().Add(element);
         
    }
    public void Start()
    {
        me = this;
        field = new Field(864,272);
        graphic.StartLocal();
        //Field.CreateGradient();
        physic = new Physic();
        InitCurlements();
        CreateElement(new H2O(), new Vector2Int(5, 0));
      //  Observable.Start(() => AddTask()
         //   ).ObserveOnMainThread().Subscribe(xs=>Debug.Log(xs));
        
        StartThreadPoolPhysUpdate();

    }
    public void InitCurlements()
    {
        for (int i = 0; i < 12; i++) Curlements.Add(new List<Element>());
    }
    public void StartThreadPoolPhysUpdate()
    {
        IObservable<Unit>[] observables = new IObservable<Unit>[12];
        for (int i = 0; i < 12; i++)
        {

            object I = i;

            observables[i] = Observable.FromCoroutine(AddTaskUpdatePhys);
             //   ).Subscribe(x=>Debug.Log(x));

          
            // WaitCallback callback = new WaitCallback(AddTaskUpdatePhys);
            // ThreadPool.QueueUserWorkItem(callback,i);
        }
        Observable.WhenAll(observables).Subscribe();
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
    private void Update()
    {
       AddTask();
        if (Input.GetKeyUp(KeyCode.Tab))
        {
            foreach(var a in Curlements)
                foreach(var b in a)
                {
                    Debug.Log(b.cell.pos.ToString());
                }
        }
    }
    public IEnumerator AddTaskUpdatePhys()
    {
        int threadcode=indexes;
        indexes++;

        while (true)
        {
            yield return new WaitForSeconds(threadRate);
            UpdateSimulator(threadcode);
            //  yield return Observable.Start(() => graphic.Render()).Subscribe();
            graphic.Render();


        }
    }
    public void UpdateSimulator( object threadcode)
    {
        try
        {
            if (Curlements[(int)threadcode].Count > 0)
                physic.Update(Curlements[(int)threadcode]);
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
        }
        
    }
}
