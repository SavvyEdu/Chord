using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Shapes;

[ExecuteAlways]
public class Euclid_Draw : ImmediateModeShapeDrawer
{
    public Color guideColor;
    public Color finalColor;
    public Color editColor;

    public bool showGuides = true;
    public int thicknessGuides = 2;

    public bool showFinal = true;
    public bool showPOI = true;
    public int thicknessFinal = 2;

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
                Euclid.Circles.DrawShapes();
                Euclid.Lines.DrawShapes();
            }

            if (showFinal)
            {
                Draw.Color = finalColor;
                Draw.Thickness = thicknessFinal;
                //Draw Final Lines
                Euclid.Arcs.DrawShapes();
                Euclid.Segments.DrawShapes();
            }

            //Draw Mouse Pos
            Draw.Disc(Euclid.snapPos, 0.1f, Color.white);
            Draw.Ring(Euclid.snapPos, 0.1f);

            Draw.Thickness = thicknessGuides;
            //Draw Editing Lines
            Draw.Color = editColor;
            Euclid.Circles.DrawEditing();
            Euclid.Lines.DrawEditing();
            Euclid.Arcs.DrawEditing();
            Euclid.Segments.DrawEditing();

            //Draw POI
            if (showPOI)
            {
                Draw.Color = guideColor;
                Euclid.POI.DrawShapes();
            }
        }
    }
}