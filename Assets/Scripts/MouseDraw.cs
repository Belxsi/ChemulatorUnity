using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class MouseDraw : MonoBehaviour
{
    public Image pixel;
    public Vector2 point;
    public Vector3 pposmin;
    public Vector3 pposmax;
    public Vector2 size;
    public static Vector2Int result;
    public Vector2 notmalize;
    public Transform maxT, minT,Bax,Bin;
    public Vector2 min,max;
    public Rect rect;
    public Canvas canvas;
    public static bool IsDraw;
    void Start()
    {
        
    }
    public void MouseSet()
    {
        var ped = new PointerEventData(EventSystem.current);
        ped.position = Input.mousePosition;
        
        List<RaycastResult> results = new();
        EventSystem.current.RaycastAll(ped, results);
        RaycastResult rr= results.Find(x => x.gameObject.CompareTag("Screen"));
        IsDraw= rr.isValid;
        point = rr.screenPosition; 
        
        Image image = Simulator.me.graphic.ImageBox;
        var t = image.rectTransform;
        Vector2 sizeDelta =new (image.rectTransform.rect.size.x, image.rectTransform.rect.size.y);
        // max = sizeDelta+ (Vector2)image.rectTransform.anchoredPosition;
        max = Camera.main.WorldToScreenPoint(Bax.transform.position);
        maxT.transform.localPosition = max;
        rect = image.rectTransform.rect;
      //  min = (Vector2)image.rectTransform.anchoredPosition;
      min= Camera.main.WorldToScreenPoint(Bin.transform.position);
        minT.transform.localPosition= min;
        Vector2 a = point - min;
        Vector2 b = max - min;

        notmalize =Vector2.one-new Vector2(a.x/b.x,a.y/b.y);
        result = new((int)( Mathf.Clamp01(Mathf.Abs(notmalize.x)) * (float)Field.size.X),(int) (Mathf.Clamp01(Mathf.Abs(notmalize.y)) * (float)Field.size.Y));
       // pixel.transform.localPosition =(Vector2) result;
        //Debug.Log(re);
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        //pos = Input.mousePosition;
        //  pposmin = Camera.main.WorldToScreenPoint(sr.bounds.min);
        // pposmax = Camera.main.WorldToScreenPoint(sr.bounds.max);
        //   size = new((pos.x - pposmin.x) / pposmax.x,1- (pos.y - pposmin.y) / pposmax.y);
        //  result = new(Mathf.Clamp( (int)(size.x * (float)Field.size.X),0, Field.size.X), Mathf.Clamp((int)(size.y * (float)Field.size.Y),0, Field.size.Y));
        MouseSet();
    }
}
