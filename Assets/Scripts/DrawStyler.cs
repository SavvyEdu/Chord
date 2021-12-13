using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Shapes;

[ExecuteAlways]
public class DrawStyler : ImmediateModeShapeDrawer
{
    public Color guideColor;
    public Color finalColor;
    public Color editColor;

    public bool showGuides = true;
    public int thicknessGuides = 2;

    public bool showFinal = true;
    public bool showPOI = true;
    public int thicknessFinal = 2;


    public ExportCamera exportCamera;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            thicknessFinal++;
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            thicknessFinal--;
        }
    }

    public override void DrawShapes(Camera cam)
    {
        using (Draw.Command(cam))
        {
            //SETUP
            Draw.LineGeometry = LineGeometry.Flat2D;
            Draw.ThicknessSpace = ThicknessSpace.Pixels;
            Draw.RadiusSpace = ThicknessSpace.Meters;
            Draw.Matrix = transform.localToWorldMatrix;

            Draw.Color = Color.red;
            Draw.Thickness = 1;
            exportCamera.DrawOutline();

            LayerUtil.ForeachVisibleLayer((LayerData layer) =>
            {
                if (showGuides) //Draw Guide Shapes
                {
                    Draw.Color = guideColor;
                    Draw.Thickness = thicknessGuides;
                    ModuleControl.Compass.DrawShapes(layer.compassArcs);
                    ModuleControl.Circles.DrawShapes(layer.circles);
                    ModuleControl.Lines.DrawShapes(layer.lines);
                }

                if (showPOI) //Draw POI
                {
                    Draw.Color = guideColor;
                    foreach (var poi in layer.poi)
                    {
                        Draw.Disc(poi, 0.02f);
                    }
                }

                if (showFinal) //Draw Final Lines
                {
                    Draw.Color = finalColor;
                    Draw.Thickness = thicknessFinal;
                    ModuleControl.Arcs.DrawShapes(layer.arcs);
                    ModuleControl.Segments.DrawShapes(layer.segments);
                    ModuleControl.PolyLine.DrawShapes(layer.polyLines);
                }
            });

            Draw.Thickness = thicknessGuides;
            //Draw Mouse Pos
            Draw.Disc(ModuleControl.snapPos, 0.1f, Color.white);
            Draw.Ring(ModuleControl.snapPos, 0.1f);

            //Draw Editing Lines
            Draw.Color = editColor;

            if(ModuleControl.DrawModule != null)
            {
                ModuleControl.DrawModule.DrawEditing();
            }
        }
    }
}