using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Windows;
//using System.Windows.Media;
//using System.Windows.Media.Imaging;
using UnityEngine;

public class GIFExporter : IExporter<Texture2D>
{
    public string FILE_EXTENSION => ".gif";

    public bool SaveData(string filePath, Texture2D data)
    {
        int width = 128;
        int height = width;
        int stride = width / 8;
        byte[] pixels = new byte[height * stride];

        /*
        // Define the image palette
        BitmapPalette myPalette = BitmapPalettes.WebPalette;
        FileStream stream = new FileStream("new.gif", FileMode.Create);
        GifBitmapEncoder encoder = new GifBitmapEncoder();


        BitmapSource image = BitmapSource.Create(
            width,
            height,
            96,
            96,
            PixelFormats.Indexed1,
            myPalette,
            pixels,
            stride);

        encoder.Frames.Add(BitmapFrame.Create(image));
        encoder.Save(stream);
        */
        return true;
    }

    

    public List<Texture2D> LoadAllData(string filePath) => throw new System.NotImplementedException();
    public Texture2D LoadData(string dataPath) => throw new System.NotImplementedException();



}
