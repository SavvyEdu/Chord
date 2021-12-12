using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LayerData
{
    public string name = $"Layer {LayersData.layers.Count + 1}";
    public bool visible = true;

    public List<ArcData> compassArcs = new List<ArcData>();
    public List<CircleData> circles = new List<CircleData>();
    public List<LineData> lines = new List<LineData>();
    public List<ArcData> arcs = new List<ArcData>();
    public List<SegmentData> segments = new List<SegmentData>();
    public List<PolyLineData> polyLines = new List<PolyLineData>();

    public List<Vector2> poi = new List<Vector2>();
}