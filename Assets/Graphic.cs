
using System;
using System.Collections.Generic;

using System.Numerics;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using UnityEngine;
using Vector2 = System.Numerics.Vector2;
using Vector2Int = System.Numerics.Vector2Int;


public class Graphic : MonoBehaviour
{

    // public Color32[][] screen;
    public List<Pixel> screen = new();
  
    public Texture2D exporter;
    public SpriteRenderer ImageBox;
    public Sprite image;
    public float scaleImage;
    public Vector2 startSizeWindow,startSizeImage,delta;
    public bool resized;
  //  public List<(Vector2Int, Color)> pixels=new List<(Vector2Int, Color)>(),buffer =new();
    public void StartLocal()
    {
      //  screen = new Color32[Field.size.X][];
      //  for(int i=0;i<Field.size.X;i++)
       // {
       //     screen[i] = new Color32[Field.size.Y];
      //  }
        exporter = new Texture2D(Field.size.X, Field.size.Y);
        exporter.filterMode = FilterMode.Point;
        transform.localScale =new UnityEngine.Vector3(1f/ Field.size.X,1f/Field.size.Y,1)*scaleImage;
        for(int x=0;x<Field.size.X;x++)
            for (int y = 0; y < Field.size.Y; y++)
            {
                SetPixel(x,y);
            }
        exporter.Apply();
    }
    public void RenderPixel(Vector2Int point,Cell cell)
    {
        //  screen[point.X][ point.Y]= cell.GetColorElement();
        screen.Add(new(point, cell.GetColorElement()));
        
        //exporter.Apply();
    }
    public void RenderPixel(Vector2Int point)
    {
        Field.TryGetCell(point, out Cell cell);
        // screen[point.X][ point.Y]= cell.GetColorElement();
        screen.Add(new(point, cell.GetColorElement()));

    }
    public void RenderPixel(int x, int y)
    {
        Vector2Int point = new(x, y);
        Field.TryGetCell(point, out Cell cell);
        // screen[point.X][ point.Y]= cell.GetColorElement();
        screen.Add(new(point, cell.GetColorElement()));
        //   screen.Apply();
    }
    public void SetPixel(int x, int y)
    {
        Vector2Int point = new(x, y);
        Field.TryGetCell(point, out Cell cell);
        // screen[point.X][ point.Y]= cell.GetColorElement();
        // screen.Add(screen[point.X],[point.Y]);
        screen.Add(new(point, cell.GetColorElement()));

    }
    public void PaintPixel(Vector2Int point,Color color)
    {
        //  screen[point.X][ point.Y]= color;
        screen.Add(new(point, color));
        //screen.Apply();
    }
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
    private readonly object _listLock = new object();
    public void FinalRender()
    {


        //  for (int x = 0; x < Field.size.X; x++)
        // for (int y = 0; y < Field.size.Y; y++)
        //     exporter.SetPixel(x,y,screen[x][y]);
        for (int i = 0; i < screen.Count; i++)
        {
            Pixel pixel = screen[i];
            exporter.SetPixel(pixel.x,pixel.y,pixel.Color);
        }
        screen.Clear();
        exporter.Apply();
        image = Sprite.Create(exporter, new(0, 0, Field.size.X, Field.size.Y), new(0.5f, 0.5f));
        ImageBox.sprite = image;
    }
    public void Render()
    {
        
        
           
         
        
    }
    public class Pixel
    {
        public int x, y;
        public Color32 Color;

        public Pixel(int x, int y, Color32 color)
        {
            this.x = x;
            this.y = y;
            Color = color;
        }
        public Pixel(Vector2Int point, Color32 color)
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
