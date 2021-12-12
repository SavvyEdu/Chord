using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

/// <summary>
/// Saves and Loads Profile data using a Binary Formatter
/// </summary>
public class BinaryExporter : IExporter<LayerData>
{
    public string FILE_EXTENSION => ".dat";

    public bool SaveData(string dataPath, LayerData layerData)
    {
        BinaryFormatter binaryFormatter = new BinaryFormatter();
        FileStream fileStream = File.Open(dataPath, FileMode.OpenOrCreate);

        bool success = false;
        try
        {
            binaryFormatter.Serialize(fileStream, layerData);
            success = true;
        }
        catch (Exception e)
        {
            Debug.Log("Serialization Failed: " + e.Message);
        }
        finally
        {
            //always close the fileStream
            fileStream.Close();
        }
        return success;
    }

    public LayerData LoadData(string dataPath)
    {
        LayerData layerData = null;
        //make sure the file actually exists
        if (File.Exists(dataPath))
        {
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            FileStream fileStream = File.Open(dataPath, FileMode.Open);

            try
            {
                layerData = (LayerData)binaryFormatter.Deserialize(fileStream);
            }
            catch (Exception e)
            {
                Debug.Log("Deserialization Failed: " + e.Message);
            }
            finally
            {
                //always close the fileStream
                fileStream.Close();
            }
        }
        return layerData;
    }

    public List<LayerData> LoadAllData(string filePath)
    {
        List<LayerData> allLayerData = new List<LayerData>();
        //make sure the directory actually exists
        if (Directory.Exists(filePath))
        {
            BinaryFormatter binaryFormatter = new BinaryFormatter();

            string[] allFilePaths = Directory.GetFiles(filePath);
            foreach (string dataPath in allFilePaths)
            {
                //Check file extension
                if (dataPath.Substring(dataPath.Length - FILE_EXTENSION.Length, FILE_EXTENSION.Length).Equals(FILE_EXTENSION))
                {
                    //make sure the file actually exists
                    if (File.Exists(dataPath))
                    {
                        FileStream fileStream = File.Open(dataPath, FileMode.Open);
                        try
                        {
                            LayerData layerData = (LayerData)binaryFormatter.Deserialize(fileStream);
                            allLayerData.Add(layerData);
                        }
                        catch (Exception e)
                        {
                            Debug.Log("Game Deserialization Failed: " + e.Message);
                        }
                        finally
                        {
                            //always close the fileStream
                            fileStream.Close();
                        }
                    }
                }
            }
        }
        return allLayerData;
    }
}
