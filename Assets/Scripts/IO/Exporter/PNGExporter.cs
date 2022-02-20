using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class PNGExporter : ImageExporter
{
    //transparency calculation: https://gist.github.com/bitbutter/302da1c840b7c93bc789

    public override string FILE_EXTENSION => ".png";

    public override bool SaveData(string filePath, Texture2D data)
    {
        string name = $"{filePath}/shot{FILE_EXTENSION}";
        byte[] pngShot = data.EncodeToPNG();

        try
        {
            File.WriteAllBytes(name, pngShot);
            return true;
        }
        catch(Exception e) { return false; }
    }
}
