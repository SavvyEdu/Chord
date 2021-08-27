using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Shapes;

public class Euclid_Segment : MonoBehaviour
{
    public bool editing;

    public List<SegmentData> segments = new List<SegmentData>();
    private SegmentData currentSegment = null;

    public void DrawShapes()
    {
        foreach (var segment in segments)
        {
            Draw.Line(segment.start, segment.end);
        }
    }

    public void DrawEditing()
    {
        if (editing)
        {
            Draw.Line(currentSegment.start, currentSegment.end);
        }
    }

    public void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            editing = true;
            currentSegment = new SegmentData(Euclid.snapPos, Euclid.snapPos);
        }

        if (Input.GetMouseButton(0))
        {
            currentSegment.end = Euclid.snapPos;
        }

        if (Input.GetMouseButtonUp(0))
        {
            editing = false;
            segments.Add(currentSegment);
            Euclid.drawStack.Add(EditMode.Segment, 0);
            currentSegment = null;
        }
    }
}

public class SegmentData
{
    public Vector2 start;
    public Vector2 end;

    public SegmentData(Vector2 s, Vector2 e)
    {
        this.start = s;
        this.end = e;
    }
}

