using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
//using System.Windows.Media;
//using System.Windows.Media.Imaging;
using UnityEngine;

public class GIFExporter : AnimationExporter
{
    public override string FILE_EXTENSION => ".png";

    public override bool SaveData(string filePath, Texture2D[] data)
    {
        /* string name = $"{filePath}/anim{FILE_EXTENSION}";

         FileStream stream = new FileStream(name, FileMode.Create);
         GifBitmapEncoder encoder = new GifBitmapEncoder();

         for (int frame = 0; frame < data.Length; frame++)
         {

             byte[] pngShot = data[frame].EncodeToPNG();

             File.WriteAllBytes(name, pngShot);

         }*/
        return true;
    }
}
