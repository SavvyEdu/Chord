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

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            showPOI = !showPOI;
        }
        if (Input.GetKeyDown(KeyCode.G))
        {
            showGuides = !showGuides;
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            showFinal = !showFinal;
        }
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

            //Draw Guide Shapes
            if (showGuides)
            {
                Draw.Color = guideColor;
                Draw.Thickness = thicknessGuides;
                ModuleControl.Circles.DrawShapes();
                ModuleControl.Lines.DrawShapes();
            }

            if (showFinal)
            {
                Draw.Color = finalColor;
                Draw.Thickness = thicknessFinal;
                //Draw Final Lines
                ModuleControl.Arcs.DrawShapes();
                ModuleControl.Segments.DrawShapes();
            }

            Draw.Thickness = thicknessGuides;
            //Draw Mouse Pos
            Draw.Disc(ModuleControl.snapPos, 0.1f, Color.white);
            Draw.Ring(ModuleControl.snapPos, 0.1f);

            
            //Draw Editing Lines
            Draw.Color = editColor;
            ModuleControl.Circles.DrawEditing();
            ModuleControl.Lines.DrawEditing();
            ModuleControl.Arcs.DrawEditing();
            ModuleControl.Segments.DrawEditing();

            //Draw POI
            if (showPOI)
            {
                Draw.Color = guideColor;
                ModuleControl.POI.DrawShapes();
            }
        }
    }
}