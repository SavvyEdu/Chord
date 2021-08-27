using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum EditMode
{
    Panning, Select, Circle, Line, Arc, Segment, None
}

public class ModuleControl : MonoBehaviour
{
    public static EditMode editMode;

    private static float MAX_SNAP_DIST = 0.2f;

    private Vector2 CamWorldSize;
    private Vector2 CamPos = Vector2.zero;

    private Vector2 panViewportOrigin;
    private float cameraScrollSize = 5;

    public static CircleModule Circles = new CircleModule();
    public static LineModule Lines = new LineModule();
    public static POIModule POI = new POIModule();
    public static ArcModule Arcs = new ArcModule();
    public static SegmentModule Segments = new SegmentModule();

    public static DrawStack drawStack = new DrawStack();

    public static Vector2 mouseViewportPos;
    public static Vector2 mousePos;
    public static Vector2 snapPos;

    public Text editModeText;

    private void Awake()
    { 
        cameraScrollSize = Camera.main.orthographicSize;

        CircleMode();
    }

    private void Update()
    {
        //Update Draw Mode
        if (Input.GetKeyDown(KeyCode.Alpha1))
            CircleMode();
        else if (Input.GetKeyDown(KeyCode.Alpha2))
            LineMode();
        else if (Input.GetKeyDown(KeyCode.Alpha3))
            ArcMode();
        else if (Input.GetKeyDown(KeyCode.Alpha4))
            SegmentMode();

        //Get Mouse positions
        mouseViewportPos = Camera.main.ScreenToViewportPoint(Input.mousePosition);
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        snapPos = GetSnapPos(mousePos);

        ScrollingUpdate();
        PanningUpdate();

        //Undo
        if (Input.GetKeyDown(KeyCode.Z))
        {
            drawStack.Undo();
        }

        Module drawModule = null;
        switch (editMode)
        {
            case EditMode.Circle:   drawModule = Circles;   break;
            case EditMode.Line:     drawModule = Lines;     break;
            case EditMode.Arc:      drawModule = Arcs;      break;
            case EditMode.Segment:  drawModule = Segments;  break; 
            default:                                        return; //skip input controls
        }

        if (Input.GetMouseButtonDown(0))
            drawModule.InputDown();

        if (Input.GetMouseButton(0))
            drawModule.InputPressed();

        drawModule.WhileEditing();

        if (Input.GetMouseButtonUp(0))
            drawModule.InputReleased();
    }

    private void ScrollingUpdate()
    {
        cameraScrollSize = Mathf.Clamp(cameraScrollSize - Input.mouseScrollDelta.y, 1, 20);
        Camera.main.orthographicSize = cameraScrollSize;

        MAX_SNAP_DIST = 0.04f * cameraScrollSize;
        CamWorldSize = new Vector2(Camera.main.orthographicSize * Screen.width / Screen.height, Camera.main.orthographicSize) * 2;
    }

    private void PanningUpdate()
    {
        if (Input.GetMouseButtonDown(2))
        {
            panViewportOrigin = mouseViewportPos;
        }

        if (Input.GetMouseButton(2))
        {
            Vector2 PanViewportOffset = panViewportOrigin - mouseViewportPos;

            float cameraX = CamPos.x + PanViewportOffset.x * CamWorldSize.x;
            float cameraY = CamPos.y + PanViewportOffset.y * CamWorldSize.y;
            Camera.main.transform.position = new Vector3(cameraX, cameraY, -10f);
        }

        if (Input.GetMouseButtonUp(2))
        {
            CamPos = Camera.main.transform.position;
        }
    }


    #region SNAPPING

    public static Vector2 GetSnapPos(Vector2 point)
    {
        //Snap to Points of interest
        if (TryGetPOISnapPos(point, out Vector2 poiSnap))
        {
            return poiSnap;
        }

        //no POI nearby, Snap to Circles or Lines
        if (TryGetLineORCircleSnapPos(point, out Vector2 snap))
        {
            return snap;
        }
        return point;
    }

    private static bool TryGetLineORCircleSnapPos(Vector2 point, out Vector2 closestPoint)
    {
        closestPoint = point; //default to origional point
        float closestDist = MAX_SNAP_DIST;
        bool snapped = false;

        foreach (var line in Lines.lines)
        {
            Vector2 PointOnLine = IntersectHelper.GetClosetPointOnLine(line, point);
            float distToLine = Vector2.Distance(point, PointOnLine);

            if (distToLine < closestDist)
            {
                closestPoint = PointOnLine;
                snapped = true;
            }
        }

        foreach (var circle in Circles.circles)
        {
            Vector2 offset = point - circle.origin;
            float distToRing = Mathf.Abs(offset.magnitude - circle.radius);

            if (distToRing < closestDist)
            {
                closestPoint = circle.origin + (offset.normalized * circle.radius);
                snapped = true;
            }
        }

        return snapped;
    }


    public static bool TryGetLineSnapPos(Vector2 point, out Vector2 closestPoint)
    {
        closestPoint = point; //default to origional point
        float closestDist = MAX_SNAP_DIST;
        bool snapped = false;

        foreach (var line in Lines.lines)
        {
            Vector2 PointOnLine = IntersectHelper.GetClosetPointOnLine(line, point);
            float distToLine = Vector2.Distance(point, PointOnLine);

            if (distToLine < closestDist)
            {
                closestPoint = PointOnLine;
                snapped = true;
            }
        }

        return snapped;
    }

    private static bool TryGetPOISnapPos(Vector2 point, out Vector2 closestPoint)
    {
        closestPoint = point; //default to origional point
        float closestDist = MAX_SNAP_DIST;
        bool snapped = false;

        foreach (var poi in POI.points)
        {
            float distToPoint = Vector2.Distance(point, poi);

            if (distToPoint < closestDist)
            {
                closestPoint = poi;
                snapped = true;
            }
        }

        return snapped;
    }

    private static bool TryGetCircleSnapPos(Vector2 point, out Vector2 closestPoint)
    {
        closestPoint = point; //default to origional point
        float closestDist = MAX_SNAP_DIST;
        bool snapped = false;

        foreach (var circle in Circles.circles)
        {
            Vector2 offset = point - circle.origin;
            float distToRing = Mathf.Abs(offset.magnitude - circle.radius);

            if (distToRing < closestDist)
            {
                closestPoint = circle.origin + (offset.normalized * circle.radius);
                snapped = true;
            }
        }

        return snapped;
    }

    #endregion

    #region MODES
    public void CircleMode()
    {
        editMode = EditMode.Circle;
        editModeText.text = "Circle";
    }

    public void ArcMode()
    {
        editMode = EditMode.Arc;
        editModeText.text = "Arc";
    }

    public void SegmentMode()
    {
        editMode = EditMode.Segment;
        editModeText.text = "Segment";
    }

    public void LineMode()
    {
        editMode = EditMode.Line;
        editModeText.text = "Line";
    }

    public void PanMode()
    {
        editMode = EditMode.Panning;
        editModeText.text = "Panning";
    }

    public void SelectMode()
    {
        editMode = EditMode.Select;
        editModeText.text = "Select";
    }

    public void NoneMode()
    {
        editMode = EditMode.None;
        editModeText.text = "None";
    }
    #endregion
}
