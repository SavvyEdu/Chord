using UnityEngine;
using Shapes;

public struct IntersectHelper
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="lineA"></param>
    /// <param name="lineB"></param>
    /// <param name="p">intersection</param>
    /// <returns>true when intersecting</returns>
    public static bool TryLineLine(LineData lineA, LineData lineB, out Vector2 p)
    {
        Vector2 p1 = lineA.startPoint, 
                p2 = lineB.startPoint;
        Vector2 d1 = lineA.Diff, 
                d2 = lineB.Diff;

        float dx = p2.x - p1.x;
        float dz = p2.y - p1.y;
        float det = d2.x * d1.y - d2.y * d1.x;
        if (det == 0)
        {
            p = Vector2.zero;
            return false;
        }
        float u = (dz * d2.x - dx * d2.y) / det;
        float v = (dz * d1.x - dx * d1.y) / det;
        p = p1 + u * d1;
        return true;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="line"></param>
    /// <param name="rect"></param>
    /// <param name="p1"></param>
    /// <param name="p2"></param>
    /// <returns>true when intersecting</returns>
    public static bool TryLineRect(LineData line, Rect rect, out Vector2 p1, out Vector2 p2)
    {
        Vector2 p = line.startPoint;
        Vector2 d = line.Diff;        
        
        if (d.x == 0) //vertical line
        {
            p1 = new Vector2(p.x, rect.yMin); 
            p2 = new Vector2(p.x, rect.yMax);
            return p.x > rect.xMin && p.x < rect.xMax;
        }
        else if (d.y == 0) //horizontal line
        {
            p1 = new Vector2(rect.xMin, p.y);
            p2 = new Vector2(rect.xMax, p.y);
            return p.y > rect.yMin && p.y < rect.yMax;
        }
        else //line has a slope! ... ah shit
        {
            //uses slope intercept form to calculate intersection points
            float m = d.y / d.x;
            float leftY = m * (rect.xMin - p.x) + p.y;
            float rightY = m * (rect.xMax - p.x) + p.y;
            float bottomX = (rect.yMin - p.y) / m + p.x;
            float topX = (rect.yMax - p.y) / m + p.x;

            //check for intersection on rect (ignore corners)
            bool onLeft = leftY > rect.yMin && leftY < rect.yMax;
            bool onRight = rightY > rect.yMin && rightY < rect.yMax;
            bool onBottom = bottomX > rect.xMin && bottomX < rect.xMax;
            bool onTop = topX > rect.xMin && topX < rect.xMax;

            //get the  
            p1 = p2 = Vector2.zero;
            //get the first point (for m>0: left/bottom) (for m<0:left/top)
            if (onLeft)
                p1 = new Vector2(rect.xMin, leftY);
            else if (m > 0 && onBottom)
                p1 = new Vector2(bottomX, rect.yMin);
            else if (m < 0 && onTop)
                p1 = new Vector2(topX, rect.yMax);
            else return false;
            //get the second point (for m>0: top/right) (for m<0:bottom/right)
            if (onRight)
                p2 = new Vector2(rect.xMax, rightY);
            else if (m > 0 && onTop)
                p2 = new Vector2(topX, rect.yMax);
            else if (m < 0 && onBottom)
                p2 = new Vector2(bottomX, rect.yMin);
            else return false;
            return true;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="circleA"></param>
    /// <param name="circleB"></param>
    /// <param name="p1">intersection 1 (when return value > 0 )</param>
    /// <param name="p2">intersection 2 (when return value > 1)</param>
    /// <returns>number of intersections</returns>
    public static int TryCircleCircle(CircleData circleA, CircleData circleB, out Vector2 p1, out Vector2 p2)
    {
        p1 = p2 = Vector2.zero;

        float dist = Vector2.Distance(circleA.origin, circleB.origin);
        if (dist > circleA.radius + circleB.radius)
        {
            //Circles are too far apart to be intersecting
            return 0;
        }
        else if (dist < Mathf.Abs(circleA.radius - circleB.radius))
        {
            //Close enough to intersect, but one circle contains the other
            return 0;
        }
        else if (dist == 0 && circleA.radius == circleB.radius)
        {
            //They are the same circle
            return 0;
        }

        // Find a and h.
        float a = (circleA.radius * circleA.radius - circleB.radius * circleB.radius + dist * dist) / (2 * dist);
        float h = Mathf.Sqrt(circleA.radius * circleA.radius - a * a);

        // Find 'centerpoint'
        Vector2 centerpoint = new Vector2(
            circleA.origin.x + a * (circleB.origin.x - circleA.origin.x) / dist,
            circleA.origin.y + a * (circleB.origin.y - circleA.origin.y) / dist);

        p1 = new Vector2(
            centerpoint.x + h * (circleB.origin.y - circleA.origin.y) / dist,
            centerpoint.y - h * (circleB.origin.x - circleA.origin.x) / dist);
        p2 = new Vector2(
           centerpoint.x - h * (circleB.origin.y - circleA.origin.y) / dist,
           centerpoint.y + h * (circleB.origin.x - circleA.origin.x) / dist);

        if (dist == circleA.radius + circleB.radius)
            return 1; //only 1 point of intersection
        return 2;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="line"></param>
    /// <param name="circle"></param>
    /// <param name="p1">intersection 1 (when return value > 0 )</param>
    /// <param name="p2">intersection 2 (when return value > 1)</param>
    /// <returns>number of intersections</returns>
    public static int TryLineCircle(LineData line, CircleData circle, out Vector2 p1, out Vector2 p2)
    {
        p1 = p2 = Vector2.zero;

        Vector2 closestPoint = GetClosetPointOnLine(line, circle.origin);
        float dist = Vector2.Distance(closestPoint, circle.origin);

        if (dist > circle.radius)
        {
            //too far away to intersect
            return 0;
        }
        else if (dist == circle.radius)
        {
            //tangent
            p1 = p2 = closestPoint;
            return 1;
        }

        //using pythagorean theorem (radius is hyp, dist is side)
        //dt is distance along line from closestPoint to circle 
        float dt = Mathf.Sqrt(circle.radius * circle.radius - dist * dist);

        Vector2 offset = line.Diff.normalized * dt;

        p1 = closestPoint + offset;
        p2 = closestPoint - offset;

        return 2;
    }

    /// <summary>
    /// finds the shortest distance to a line from a point
    /// </summary>
    /// <param name="line">target line</param>
    /// <param name="p">projected point</param>
    /// <returns>point on line</returns>
    public static Vector2 GetClosetPointOnLine(LineData line, Vector2 p)
    {
        //percentage along line that circle origin projects onto
        float t = ShapesMath.GetLineSegmentProjectionT(line.startPoint, line.endPoint, p);

        Vector2 closestPoint = line.startPoint + line.Diff * t;

        return closestPoint;
    }

    /// <summary>
    /// Find the intersection of 2 Rays in the XZ plane
    /// </summary>
    /// <param name="p1">point 1</param>
    /// <param name="d1">direction 1</param>
    /// <param name="p2">point 2</param>
    /// <param name="d2">direction 2</param>
    /// <returns>Point of intersection</returns>
    public Vector2 IntersecionOfRays(Vector2 p1, Vector2 d1, Vector2 p2, Vector2 d2)
    {
        float dx = p2.x - p1.x;
        float dz = p2.y - p1.y;
        float det = d2.x * d1.y - d2.y * d1.x;
        if (det == 0) { return Vector3.zero; }
        float u = (dz * d2.x - dx * d2.y) / det;
        float v = (dz * d1.x - dx * d1.y) / det;
        return p1 + u * d1;
    }
}
