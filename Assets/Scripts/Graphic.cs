
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using UnityEngine;
using UnityEngine.UI;
using Vector2 = System.Numerics.Vector2;
using Vector2Int = System.Numerics.Vector2Int;


public class Graphic : MonoBehaviour
{

    // public Color32[][] screen;
    public Dictionary<Vector2Int,Cell> screen = new();
    public List<Pixel> effects=new();
    public List<Pixel> olds;
    public Texture2D exporter;
    public Image ImageBox;
    public Sprite image;
    public float scaleImage;
    public Vector2 startSizeWindow,startSizeImage,delta;
    public TypeSizeScreen TypeSize;

  //  public List<(Vector2Int, Color)> pixels=new List<(Vector2Int, Color)>(),buffer =new();
    public void StartLocal()
    {
      //  screen = new Color32[Field.size.X][];
      //  for(int i=0;i<Field.size.X;i++)
       // {
       //     screen[i] = new Color32[Field.size.Y];
      //  }
        exporter = new Texture2D(Field.size.X, Field.size.Y,TextureFormat.RGBA32,false,true);
        
        exporter.filterMode = FilterMode.Point;
        
       
        for(int x=0;x<Field.size.X;x++)
            for (int y = 0; y < Field.size.Y; y++)
            {
                SetPixel(x,y);
            }
        exporter.Apply();
    }
    public void Update()
    {
        switch (TypeSize) {
            case TypeSizeScreen.Fullscreen:
        transform.localScale = new UnityEngine.Vector3(864f / Field.size.X, 272f / Field.size.Y * (1920f / 1080f), 1) * scaleImage;
                break;
            case TypeSizeScreen.Squad:
                transform.localScale = new UnityEngine.Vector3(272f / Field.size.X, 272f / Field.size.Y * (1920f / 1080f), 1) * scaleImage;
                break;
        }
    }
    /*
    public void RenderPixel(Vector2Int point,Cell cell)
    {
        //  screen[point.X][ point.Y]= cell.GetColorElement();
        SetScreenDict(point.X+""+ point.Y, new(point, cell.GetColorElement()));
        
        //exporter.Apply();
    }
    */
    public void SetScreenDict(Vector2Int s,Cell p)
    {
        if (!screen.ContainsKey(s))
        {
            screen.Add(s, p);
        }
        else
        {
           
                screen[s] = p;
            
            
        }
    }
   
    public void RenderPixel(Vector2Int point)
    {
        Field.TryGetCell(point, out Cell cell);
        // screen[point.X][ point.Y]= cell.GetColorElement();
        if(cell!=null)
            if (cell.element != null)
                if (BreakPointer.me.ucode == cell.element.ucode)
        {

        }
        SetScreenDict(point, cell);
       

    }
    public void RenderPixel(Cell cell)
    {
       
        // screen[point.X][ point.Y]= cell.GetColorElement();
        SetScreenDict(cell.pos, cell);


    }
    /*
    public void RenderPixel(int x, int y)
    {
        Vector2Int point = new(x, y);
        Field.TryGetCell(point, out Cell cell);
        // screen[point.X][ point.Y]= cell.GetColorElement();
        SetScreenDict(point.ToString(),new(point, cell.GetColorElement()));
        //   screen.Apply();
    }
    */
    public void SetPixel(int x, int y)
    {
        Vector2Int point = new(x, y);
        Field.TryGetCell(point, out Cell cell);
        // screen[point.X][ point.Y]= cell.GetColorElement();
        // screen.Add(screen[point.X],[point.Y]);
        SetScreenDict(point, cell);

    }
    public void SetPixel(int x, int y,Color color)
    {
        Vector2Int point = new(x, y);
        effects.Add(new(point, color));

    }
    /*
    public void PaintPixel(Vector2Int point,Color color)
    {
        //  screen[point.X][ point.Y]= color;
        SetScreenDict(point.ToString(),new(point, color));
        //screen.Apply();
    }
    */

    private delegate void RenderDel();
    private delegate void RenderPixelDel(Vector2Int point);
    private delegate void PaintPixelDel(Vector2Int point,Color color);



    /*
    public Image ResizeImg(Image b, int nWidth, int nHeight)
    {

        Image result = new Bitmap(nWidth, nHeight);
        using (Graphics g = Graphics.FromImage((Image)result))
        {
            g.InterpolationMode = InterpolationMode.High;
            g.DrawImage(b, 0, 0, nWidth, nHeight);
            g.Dispose();
        }
        return result;

    }
    */
    public void UpdateRender(List<Element> obj)
    {
        foreach (Element el in obj.ToList())
        {
            if (Simulator.me.updatepixels)
                Simulator.me.graphic.RenderPixel(el.cell.pos);
        }
        }
    private readonly object _listLock = new object();
    public void FinalRender()
    {


        //  for (int x = 0; x < Field.size.X; x++)
        // for (int y = 0; y < Field.size.Y; y++)
        //     exporter.SetPixel(x,y,screen[x][y]);
        if (Simulator.me.random.NextDouble() > Simulator.me.reupdateGraphicRate)
            for (int x = 0; x < Field.size.X; x++)
                for (int y = 0; y < Field.size.Y; y++) { 
                   Cell cell= Field.field[x, y];
        
                exporter.SetPixel(x, y, cell.GetColorElement());
        }
        foreach (var item in screen)
        {
            Cell cell = item.Value;
            exporter.SetPixel((int)cell.pos.X,(int)( cell.pos.Y), cell.GetColorElement());
        }
        if (effects.Count > 0)
        {
            foreach (var item in effects)
            {
                exporter.SetPixel(item.x, item.y, item.Color);
            }
            effects.Clear();
        }
            screen.Clear();
        
        exporter.Apply(true,false);
        
        //Graphics.DrawTexture(new(0, 0, Field.size.X, Field.size.Y), exporter);
        image = Sprite.Create(exporter, new(0, 0, Field.size.X, Field.size.Y), new(0.5f, 0.5f));
        ImageBox.sprite = image;
    }
    public void AllRender()
    {


        //  for (int x = 0; x < Field.size.X; x++)
        // for (int y = 0; y < Field.size.Y; y++)
        //     exporter.SetPixel(x,y,screen[x][y]);
        exporter = new Texture2D(Field.size.X, Field.size.Y, TextureFormat.RGBA32, false, true);
        exporter.filterMode = FilterMode.Point;
       
        foreach (var list in Simulator.me.Curlements)
            foreach (var item in list)
            {
            Pixel pixel =new( item.cell.pos,item.cell.GetColorElement());
                olds.Add(pixel);
            exporter.SetPixel(pixel.x, pixel.y, pixel.Color);
        }
      
        screen.Clear();

        exporter.Apply(true, false);

        //Graphics.DrawTexture(new(0, 0, Field.size.X, Field.size.Y), exporter);
        image = Sprite.Create(exporter, new(0, 0, Field.size.X, Field.size.Y), new(0.5f, 0.5f));
        ImageBox.sprite = image;
    }
    public void Render()
    {
        
        
           
         
        
    }
    public class Pixel
    {
        public int x, y;
        public Color Color;

        public Pixel(int x, int y, Color color)
        {
            this.x = x;
            this.y = y;
            Color = color;
        }
        public Pixel(Vector2Int point, Color color)
        {
            this.x = point.X;
            this.y = point.Y;
            Color = color;
        }
    }


        

        
        












        //form.pictureBox1.BackgroundImage = null;


        //e.Graphics.Clear(Color.Black);
        //Thread.Sleep(1);


        //Resize(ResizeImg(new Bitmap( screen), form.ClientSize.Width, form.ClientSize.Height));
        //Resize(screen);










    

   
}
public enum TypeSizeScreen
{
    Fullscreen,Squad,Custom
}
