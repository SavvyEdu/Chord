using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public enum EditMode
{
    Panning, Select, Compass, Circle, Line, Arc, Segment, PolyLine, None
}

public class ModuleControl : MonoBehaviour
{
    public static EditMode EdittingMode { get; private set; } = EditMode.None;
    public static IModule DrawModule { get; private set; } = null;

    private static float MAX_SNAP_DIST = 0.2f;

    private Vector2 CamWorldSize;
    private Vector2 CamPos = Vector2.zero;
    private Vector2 panViewportOrigin;
    private float orthographicSize = 5;

    public static Rect cameraRect = new Rect();

    public static CompassModule Compass = new CompassModule();
    public static CircleModule Circles = new CircleModule();
    public static LineModule Lines = new LineModule();
    public static POIModule POI = new POIModule();
    public static ArcModule Arcs = new ArcModule();
    public static SegmentModule Segments = new SegmentModule();
    public static PolyLineModule PolyLine = new PolyLineModule();

    public static Vector2 mouseViewportPos; //viewport (0, 1) coords
    public static Vector2 mousePos; //world coords
    public static Vector2 snapPos; //final snap pos

    public static bool canLineLock = false;
    public static Vector2 lineLockRef;

    private void SetEdittingMode(EditMode value)
    {
        //Cancel the current edit
        if (DrawModule != null)
            DrawModule.CancelEditing();

        //Switch draw module
        EdittingMode = value;
        switch (EdittingMode)
        {
            case EditMode.Compass:      DrawModule = Compass;       break;
            case EditMode.Circle:       DrawModule = Circles;       break;
            case EditMode.Line:         DrawModule = Lines;         break;
            case EditMode.Arc:          DrawModule = Arcs;          break;
            case EditMode.Segment:      DrawModule = Segments;      break;
            case EditMode.PolyLine:     DrawModule = PolyLine;      break;
            default:                    DrawModule = null;          break;
        }

        //Update the tooltip
        if(DrawModule != null)
            Tooltip.setMessage?.Invoke(DrawModule.tooltipMessage);

        //Update thr tool settings
        ToolSettings.onToolSelected?.Invoke(DrawModule);
    }

    private void Awake()
    { 
        orthographicSize = Camera.main.orthographicSize;
    }

    private void Update()
    {
        //Get Mouse positions
        mouseViewportPos = Camera.main.ScreenToViewportPoint(Input.mousePosition);
        snapPos = mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
      
        if(canLineLock && (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)))
            snapPos = GetLineLockPos(snapPos);
     
        snapPos = GetSnapPos(snapPos);

        //Scroll and Pan
        ScrollingUpdate();
        PanningUpdate();
        cameraRect.center = Camera.main.transform.position;
        cameraRect.size = CamWorldSize;

        //Undo
        if (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl))
        {
            if (Input.GetKeyDown(KeyCode.Z))
                Undo();

            if (Input.GetKeyDown(KeyCode.Y))
                Redo();
        }
        
        //Input to draw module controls
        if (DrawModule != null)
        {
            if (!EventSystem.current.IsPointerOverGameObject())
            {
                //Start draw
                if (Input.GetMouseButtonDown(0))
                    DrawModule.MainInputDown();

                if (Input.GetMouseButtonDown(1))
                    DrawModule.AltInputDown();
            }

            if (DrawModule.editing)
            {
                //Continue draw
                if (Input.GetMouseButton(0))
                    DrawModule.MainInputPressed();

                if (Input.GetMouseButton(1))
                    DrawModule.AltInputPressed();

                //Update draw
                DrawModule.WhileEditing();

                //End draw
                if (Input.GetMouseButtonUp(0))
                    DrawModule.MainInputReleased();

                if (Input.GetMouseButtonUp(1))
                    DrawModule.AltInputReleased();
            }

            //switch line lock on and off
            if(DrawModule.editing && !canLineLock)
                EnableLineLock();
            else if(!DrawModule.editing && canLineLock)
                DisableLineLock();
        }

        Tooltip.setActive?.Invoke(!DrawModule.editing);
    }

    private void ScrollingUpdate()
    {
        //Calculate the new orthographic size
        orthographicSize = Mathf.Clamp(orthographicSize - Input.mouseScrollDelta.y, 1, 20);

        //Zoom at snap position
        float t = (orthographicSize / Camera.main.orthographicSize);
        Vector3 pos = ((Vector2)Camera.main.transform.position * t) + (snapPos * (1 - t)); //lerp to snapPos 
        Camera.main.transform.position = new Vector3(pos.x, pos.y, -10f);

        //set the 
        Camera.main.orthographicSize = orthographicSize;




        //Update snapping based on camera size
        MAX_SNAP_DIST = 0.04f * orthographicSize;

        //Update the world size for panning
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

    public static void EnableLineLock() => EnableLineLock(snapPos);
    public static void EnableLineLock(Vector2 lineLockPos)
    {
        lineLockRef = lineLockPos;
        canLineLock = true;
    }

    public static void DisableLineLock()
    {
        canLineLock = false;
    }

    public static Vector2 GetLineLockPos(Vector2 point)
    {
        float a = 2; //min 1
        Vector2 d = point - lineLockRef;  // diff from lineLock to Point

        //RIGHT / LEFT
        if (d.x > Mathf.Abs(a * d.y) || d.x < -Mathf.Abs(a * d.y))
            return new Vector2(point.x, lineLockRef.y);

        //TOP / BOTTOM
        if (d.y > Mathf.Abs(a * d.x) || point.y < -Mathf.Abs(a * d.x))
            return new Vector2(lineLockRef.x, point.y);

        //not close enought on any axis 
        return point;
    }

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
        Vector2 closest = point; //default to origional point
        float closestDist = MAX_SNAP_DIST;
        bool snapped = false;

        LayerUtil.ForeachVisibleLine((LineData line) =>
        {
            Vector2 PointOnLine = IntersectHelper.GetClosetPointOnLine(line, point);
            float distToLine = Vector2.Distance(point, PointOnLine);

            if (distToLine < closestDist)
            {
                closest = PointOnLine;
                snapped = true;
            }
        });

        LayerUtil.ForeachVisibleCircle((CircleData circle) =>
        {
            Vector2 offset = point - circle.origin;
            float distToRing = Mathf.Abs(offset.magnitude - circle.radius);

            if (distToRing < closestDist)
            {
                closest = circle.origin + (offset.normalized * circle.radius);
                snapped = true;
            }
        });

        closestPoint = closest;
        return snapped;
    }

    public static bool TryGetLineSnapPos(Vector2 point, out Vector2 closestPoint)
    {
        Vector2 closest = point; //default to origional point
        float closestDist = MAX_SNAP_DIST;
        bool snapped = false;

        LayerUtil.ForeachVisibleLine((LineData line) =>
        {
            Vector2 PointOnLine = IntersectHelper.GetClosetPointOnLine(line, point);
            float distToLine = Vector2.Distance(point, PointOnLine);

            if (distToLine < closestDist)
            {
                closest = PointOnLine;
                snapped = true;
            }
        });

        closestPoint = closest;
        return snapped;
    }

    private static bool TryGetPOISnapPos(Vector2 point, out Vector2 closestPoint)
    {
        Vector2 closest = point; //default to origional point
        float closestDist = MAX_SNAP_DIST;
        bool snapped = false;

        LayerUtil.ForeachVisibePOI((Vector2 poi) => 
        {
            float distToPoint = Vector2.Distance(point, poi);

            if (distToPoint < closestDist)
            {
                closest = poi;
                snapped = true;
            }
        });

        closestPoint = closest;
        return snapped;
    }

    private static bool TryGetCircleSnapPos(Vector2 point, out Vector2 closestPoint)
    {
        Vector2 closest = point; //default to origional point
        float closestDist = MAX_SNAP_DIST;
        bool snapped = false;

        LayerUtil.ForeachVisibleCircle((CircleData circle) =>
        {
            Vector2 offset = point - circle.origin;
            float distToRing = Mathf.Abs(offset.magnitude - circle.radius);

            if (distToRing < closestDist)
            {
                closest = circle.origin + (offset.normalized * circle.radius);
                snapped = true;
            }
        });

        closestPoint = closest;
        return snapped;
    }

    #endregion

    #region BUTTTON FUNCTIONS
    public void CompassMode() => SetEdittingMode(EditMode.Compass);
    public void CircleMode() => SetEdittingMode(EditMode.Circle);
    public void ArcMode() => SetEdittingMode(EditMode.Arc);
    public void SegmentMode() => SetEdittingMode(EditMode.Segment);
    public void LineMode() => SetEdittingMode(EditMode.Line);
    public void PolyLineMode() => SetEdittingMode(EditMode.PolyLine);
    public void PanMode() => SetEdittingMode(EditMode.Panning);
    public void SelectMode() => SetEdittingMode(EditMode.Select);
    public void NoneMode() => SetEdittingMode(EditMode.None);
    public void Undo() => CommandHistory.Undo();
    public void Redo() => CommandHistory.Redo();
    #endregion
}
