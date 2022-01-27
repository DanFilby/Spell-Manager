using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPolygon : MonoBehaviour
{
    public static Vector2 centre = new Vector2(0.5f, 0.5f);

    //list of verticies and sides
    public List<Vector2> Vertices;
    private List<LineSegment> Sides;

    //max square of this polygon
    private Boundry boundry;

    private void Start()
    {
        SetupPolygon();
    }

    //if wanted to create one through code
    public UIPolygon(List<Vector2> _vertices)
    {
        Vertices = _vertices;
        SetupPolygon();
    }

    /// <summary>
    /// generates the lines clases to represent the sides from the vertices
    /// </summary>
    private void SetupPolygon()
    {
        Sides = new List<LineSegment>();

        for (int i = 0; i < Vertices.Count; i++)
        {
            LineSegment line = new LineSegment(Vertices[i], Vertices[(i + 1) % Vertices.Count]);
            Sides.Add(line);
        }

        boundry = new Boundry(Vertices);
    }


    /// <summary>
    /// returns true if the point is within this polygon
    /// </summary>
    /// <param name="point"> the point of the mouse </param>
    /// <returns></returns>
    public bool WithinPolygon(Vector2 point)
    {
        //not in the polygon if its not in the boundry
        if (!boundry.CheckWithinBoundry(point)) return false;

        //generates a random point to create a random ray from the point to outside the polygon
        Vector2 randomPoint = new Vector2(Random.Range(5, 15), Random.Range(5, 15));
        LineSegment ray = new LineSegment(point, randomPoint);

        //number of times the ray intersects with sides
        int sideIntersectCount = 0;

        //for each side if it intersects with the ray it increments the counter
        for (int i = 0; i < Sides.Count; i++)
        {
            if (Sides[i].IntersectsLineSegment(ray))
            {
                sideIntersectCount++;
            }
        }        
        Debug.Log(sideIntersectCount);

        //if its not even the point is in the polygon
        return sideIntersectCount % 2 != 0;
    }

    /// <summary>
    /// prints all the info of the polygon
    /// </summary>
    public void PrintPolygonInfo()
    {
        for (int i = 0; i < Sides.Count; i++)
        {
            Debug.Log($"point 1: {Vertices[i]} - point2: {Vertices[(i + 1) % Sides.Count]}");
            Debug.Log($"gradient: {Sides[i].gradient}, intercept: {Sides[i].intercept}");
        }

        Debug.Log($"Boundries: bot left - {boundry} top right - {boundry}");
    }
}

/// <summary>
/// holds the boundry of the line, along with the gradient and y-intercept
/// </summary>
struct LineSegment
{
    //max box of the line
    private Boundry boundry;

    //line's properties
    public float gradient;
    public float intercept;

    /// <summary>
    /// create a line from two given points
    /// </summary>
    public LineSegment(Vector2 _point1, Vector2 _point2) : this()
    {
        boundry = new Boundry(_point1, _point2);
        gradient = (float)((_point1.y - _point2.y) / (_point1.x - _point2.x));
        intercept = _point1.y - (gradient * _point1.x);
    }

    /// <summary>
    /// get the y-coordinate of a point from the given x-coordinate
    /// </summary>
    public float CalculateY(float x)
    {
        return (x * gradient) + intercept;
    }
    /// <summary>
    /// get the x-coordinate of a point from the given y-coordinate
    /// </summary>
    public float CalculateX(float y)
    {
        return (y - intercept) / gradient;
    }

    /// <summary>
    /// check whether this and the given line intersect each other and are within both boundries
    /// </summary>
    public bool IntersectsLineSegment(LineSegment line2)
    {
        //lines are parralel
        if (gradient == line2.gradient) return false;

        Vector2 intersectionPoint = new Vector2();

        /* solving for x. using classic y = mx + c. put x's on one side and intercepts on
        other and then divide. then finding y */
        intersectionPoint.x = (line2.intercept - intercept) / (gradient - line2.gradient);
        intersectionPoint.y = CalculateY(intersectionPoint.x);

        //Debug.Log($"line1: y = {gradient}x + {intercept}   line2:y = {line2.gradient}x + {line2.intercept}");
        //Debug.Log($"math: {(line2.intercept - intercept)} / {(gradient - line2.gradient)}");
        //Debug.Log(intersectionPoint);

        return boundry.CheckWithinBoundry(intersectionPoint) && line2.boundry.CheckWithinBoundry(intersectionPoint);
    }
}

/// <summary>
/// the maximum area the given set of points holds
/// </summary>
struct Boundry
{
    //points of he box
    private Vector2 botLeft;
    private Vector2 topRight;

    /// <summary>
    /// creates a boundry for the given points
    /// </summary>
    public Boundry(List<Vector2> Vertices) : this()
    {
        botLeft = Vertices[0];
        topRight = Vertices[0];

        for (int i = 1; i < Vertices.Count; i++)
        {
            float x = Vertices[i].x;
            float y = Vertices[i].y;

            //check for most left and right points
            if (x <= botLeft.x) botLeft.x = x;
            if (x >= topRight.x) topRight.x = x;

            //check for highest and lowest points
            if (y <= botLeft.y) botLeft.y = y;
            if (y >= topRight.y) topRight.y = y;
        }
    }

    /// <summary>
    /// creates a boundry for the given points
    /// </summary>
    public Boundry(Vector2 point1, Vector2 point2) : this()
    {
        //finds lowest, highest and furthest left and right points
        botLeft.x = Mathf.Min(point1.x, point2.x);
        botLeft.y = Mathf.Min(point1.y, point2.y);
        topRight.x = Mathf.Max(point1.x, point2.x);
        topRight.y = Mathf.Max(point1.y, point2.y);
    }

    /// <summary>
    /// Returns true if the given point is within the boundry
    /// </summary>
    public bool CheckWithinBoundry(Vector2 point)
    {
        return point.x >= botLeft.x && point.x <= topRight.x
            && point.y >= botLeft.y && point.y <= topRight.y;
    }
}
