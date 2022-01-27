using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
/// <summary>
/// shows vertex points of a UI polygon in editor.
/// </summary>
[CustomEditor(typeof(UIPolygon))]
public class UIPolygonEditor : Editor
{
    private int selectedPoint = 0;
    private (float, float) canvasSize;

    private UIPolygon polygon;
    private List<Vector3> realWorldPoints;

    private bool manualPositioning;
    private bool showUI = false;

    private void OnEnable()
    {
        SceneView.duringSceneGui += this.OnSceneGUI;
        polygon = (UIPolygon)target;
        Canvas canvas =  polygon.gameObject.GetComponentInParent<Canvas>();
        Rect canvasRect = canvas.gameObject.GetComponent<RectTransform>().rect;
        canvasSize = (canvasRect.width, canvasRect.height);

        UpdateRealWorldPos();
    }

    private void OnDisable()
    {
        SceneView.duringSceneGui -= this.OnSceneGUI;
    }

    public void OnSceneGUI(SceneView sceneView)
    {
        if(realWorldPoints.Count > 0 && manualPositioning == false)
        {
            for (int i = 0; i < realWorldPoints.Count; i++)
            {
                if(i == selectedPoint)
                {
                    realWorldPoints[i] = Handles.DoPositionHandle(realWorldPoints[i], Quaternion.identity);
                }

                if(i != selectedPoint)
                {
                    Handles.color = Color.white;
                    if (Handles.Button(realWorldPoints[i], Quaternion.identity, 25f, 25f, Handles.SphereHandleCap))
                    {
                        selectedPoint = i;
                    }
                }
                else
                {
                    Handles.color = Color.red;
                    Handles.SphereHandleCap(0, realWorldPoints[i], Quaternion.identity, 25f, EventType.Repaint);
                }
                Handles.color = Color.white;
                Handles.DrawLine(realWorldPoints[i], realWorldPoints[(i+1) % polygon.Vertices.Count]);
            }
            UpdateVertices();
        }
        else if (manualPositioning)
        {
            for (int i = 0; i < polygon.Vertices.Count; i++)
            {
                Handles.SphereHandleCap(0, PosChange(polygon.Vertices[i]), Quaternion.identity, 25f, EventType.Repaint);

                Handles.DrawLine(PosChange(polygon.Vertices[i]), PosChange(polygon.Vertices[(i + 1) % polygon.Vertices.Count]));
            }
            UpdateRealWorldPos();
        }
    }

    public override void OnInspectorGUI()
    {
        if (GUILayout.Button("Toggle Manual Positioning"))
        {
            manualPositioning = !manualPositioning;

            SceneView.RepaintAll();
        }

        /*if (GUILayout.Button("Toggle Polygon UI"))
        {
            if (!showUI) { SceneView.duringSceneGui += this.OnSceneGUI; }
            else { SceneView.duringSceneGui -= this.OnSceneGUI; }

            showUI = !showUI;
            SceneView.RepaintAll();
        }*/

        base.OnInspectorGUI();
    }


    private void UpdateRealWorldPos()
    {
        realWorldPoints = new List<Vector3>();
        foreach (Vector2 point in polygon.Vertices)
        {
            realWorldPoints.Add(PosChange(point));
        }
    }

    private void UpdateVertices()
    {
        for (int i = 0; i < realWorldPoints.Count; i++)
        {
            polygon.Vertices[i] = new Vector2(realWorldPoints[i].x / canvasSize.Item1, realWorldPoints[i].y / canvasSize.Item2);
        }
    }

    private Vector3 PosChange(Vector2 pos)
    {
        return new Vector3(pos.x * canvasSize.Item1, pos.y * canvasSize.Item2, 0);
    }
}
