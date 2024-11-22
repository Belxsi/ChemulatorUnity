using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class MainBrush : InspectorBrush
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public Texture2D tex;
    public Dictionary<string, Texture2D> cash = new();
    public string current;

    public TypeDraw typeDraw = TypeDraw.Draw;
    public TypeBrush typeBrush = TypeBrush.Squad;
    public int sizeBrush = 1;
    public Element observer;
    void Start()
    {
        mouseConnect = true;
        InspectorBrush.main = this;
    }
    public void SetTypeDraw(string td)
    {
        typeDraw = (TypeDraw)Enum.Parse(typeof(TypeDraw), td);
        observer = null;

    }
    // Update is called once per frame
    public void Update()
    {
        if (type != "" & current != type)
        {
            current = type;
            if (cash.TryGetValue(type, out Texture2D cashed))
            {
                Cursor.SetCursor(cashed, new(0, 0), CursorMode.ForceSoftware);
            }
            else
            {
                Element el = Element.CreateElement(type);
                Color color = el.vp.GetOriginalColor();
                Color[] colors = tex.GetPixels();
                List<Color32> necol = new();
                colors.ToList().FindAll(x =>
                {
                    necol.Add(x * color);
                    return false;
                });
                var newtex = new Texture2D(tex.width, tex.height, tex.format, true);
                newtex.filterMode = FilterMode.Point;
                

                newtex.SetPixels32(necol.ToArray());
                newtex.Apply();
                cash.Add(type, newtex);
                Cursor.SetCursor(newtex, new(0, 0), CursorMode.ForceSoftware);
            }
        }
       
          
        
    }
    void FixedUpdate()
    {
        Active();
      

    }
    public delegate void ActionParameter(Vector2Int vek);
    public void ProvedSize(Action<Vector2Int> action)
    {
        switch (typeBrush)
        {
            case TypeBrush.Squad:
                for (int x = -sizeBrush + 1; x < sizeBrush; x++)
                    for (int y = -sizeBrush + 1; y < sizeBrush; y++)
                    {
                        action.Invoke(new Vector2Int(x, y));

                    }

                break;
        }
    }
    public override void Active()
    {
        switch (typeDraw)
        {
            case TypeDraw.Draw:
                sizeBrush = Mathf.Clamp(sizeBrush + Mathf.RoundToInt(Input.mouseScrollDelta.y), 1, int.MaxValue);
                if (mouseConnect)
                    point = new(MouseDraw.result.x, MouseDraw.result.y);

                if ((create) || mouseConnect & (!Window.drag) & MouseDraw.IsDraw)
                {


                    ProvedSize(new(DrawSelect));
                    if (Input.GetKey(KeyCode.Mouse0))
                    {
                        var ped = new PointerEventData(EventSystem.current);
                        ped.position = Input.mousePosition;
                        var t = new List<RaycastResult>();
                        EventSystem.current.RaycastAll(ped, t);
                        if (t.Count > 0)
                            if (t[0].gameObject.CompareTag("Screen"))
                            {
                                ProvedSize(new(Create));

                            }
                    }
                }
                if (mouseConnect & (!Window.drag))
                    if (Input.GetKey(KeyCode.Mouse1))
                    {

                        ProvedSize(new(Clear));

                    }
                break;
            case TypeDraw.Pipetka:
                sizeBrush = 1;
                if (observer != null)
                {
                    simulator.info.text = observer.ToString();
                }
                else
                {
                    simulator.info.text = "";
                }
                if (mouseConnect)
                    point = new(MouseDraw.result.x, MouseDraw.result.y);

                if ((create) || mouseConnect & (!Window.drag) & MouseDraw.IsDraw)
                {


                    ProvedSize(new(DrawSelect));
                    if (Input.GetKey(KeyCode.Mouse0))
                    {
                        var ped = new PointerEventData(EventSystem.current);
                        ped.position = Input.mousePosition;
                        var t = new List<RaycastResult>();
                        EventSystem.current.RaycastAll(ped, t);
                        if (t.Count > 0)
                            if (t[0].gameObject.CompareTag("Screen"))
                            {
                               Element e=  GetPixel();
                               observer = e;
                               

                            }
                    }
                }
                


                break;
        }
    }
}
public enum TypeBrush
{
    Squad, Circle
}
[Serializable]
public enum TypeDraw
{
    Draw, Pipetka
}
