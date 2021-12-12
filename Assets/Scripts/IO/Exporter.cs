using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// </summary>
/// <typeparam name="T">exported data type</typeparam>
public interface IExporter<T>
{
    /// <summary>
    /// file format
    /// </summary>
    string FILE_EXTENSION { get; }

    /// <summary>
    /// Writes data to the specified file
    /// </summary>
    /// <param name="filePath">file path to write to</param>
    /// <param name="data">profile data to write</param>
    /// <returns>success</returns>
    bool SaveData(string filePath, T data);

    /// <summary>
    /// Read data from the specified file
    /// </summary>
    /// <param name="dataPath">file path to read from</param>
    /// <returns>data read in</returns>
    T LoadData(string dataPath);

    /// <summary>
    /// Read in all the data from the specified directory
    /// </summary>
    /// <param name="filePath">directiory path to look through</param>
    /// <returns>List of data read in</returns>
    List<T> LoadAllData(string filePath);
}   
