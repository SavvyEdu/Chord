using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class PNGExporter : IExporter<Texture2D>
{
    public string FILE_EXTENSION => ".png";

    public bool SaveData(string filePath, Texture2D data)
    {
        string name = $"{filePath}/shot{FILE_EXTENSION}";
        var pngShot = data.EncodeToPNG();

        try
        {
            File.WriteAllBytes(name, pngShot);
            return true;
        }
        catch(Exception e) { return false; }
    }

    public List<Texture2D> LoadAllData(string filePath) => throw new System.NotImplementedException("System cannot load PNG files");
    public Texture2D LoadData(string dataPath) => throw new System.NotImplementedException("System cannot load PNG files");
}
