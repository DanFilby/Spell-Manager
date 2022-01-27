using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectMeshCreator
{
    [Header("General")]
    public MeshSettings settings;
    public Material meshMaterial;

    public GameObject debugObj;
    public Transform debugPoint;
    public float debugRad;
    public Vector3 debugRot;

    [Header("Simple connection")]
    private Vector2Int ScPointsRange = new Vector2Int(20, 50);
    private Vector2Int ScPointVertsRange = new Vector2Int(10, 25);

    private void Start()
    {
    }

    public EffectMeshCreator(MeshSettings _settings)
    {
        settings = _settings;
    }

    /// <summary>
    /// Creates a simple cylinder mesh between two points
    /// </summary>
    public Mesh SimpleBeam(Vector3 _start, Vector3 _end, float _radius)
    {
        Mesh resultMesh = new Mesh();

        int pointCount = RangeCacl(ScPointsRange, settings.meshResolutionPercent);
        int pointVertsCount = RangeCacl(ScPointVertsRange, settings.meshResolutionPercent);

        //finds the rotation of the circles so they point forward
        float yRot = Mathf.Atan2(_end.y - _start.y, _end.x - _start.x) * Mathf.Rad2Deg;
        float xRot = Mathf.Atan2( _end.x - _start.x, _end.z - _start.z) * Mathf.Rad2Deg;
        Vector3 circleRotation = new Vector3(-yRot, xRot ,0);

        List<Vector3> verts = new List<Vector3>(pointCount * pointVertsCount);
        Vector3 stepSize = (_end - _start) / pointCount;

        for (int i = 0; i < pointCount; i++)
        {
            Vector3 pos = _start + (i * stepSize);
            verts.AddRange(circlePoint(pos, circleRotation, _radius, pointVertsCount));
        }

        resultMesh.vertices = verts.ToArray();

        List<int> tris = new List<int>();
        for (int i = 0; i < pointCount; i++)
        {
            int currentRingIndex = i * pointVertsCount;
            int nextRingIndex = ((i + 1) * pointVertsCount) % (pointCount * pointVertsCount);

            for (int j = 0; j < pointVertsCount; j++)
            {
                tris.Add(currentRingIndex + j);
                tris.Add(nextRingIndex + j);
                tris.Add((currentRingIndex + j + 1) % (pointVertsCount * pointCount));

                tris.Add((currentRingIndex + j + 1) % (pointVertsCount * pointCount));
                tris.Add(nextRingIndex + j);
                tris.Add((nextRingIndex + j + 1) % (pointVertsCount * pointCount));
            }
        }

        resultMesh.triangles = tris.ToArray();

        //foreach (var p in verts)
        //{
        //    GameObject.Instantiate(debugObj, p, Quaternion.identity);
        //}
        
        return resultMesh;
    }

    private List<Vector3> circlePoint(Vector3 centrePos, Vector3 orientation, float radius, int vertCount)
    {
        List<Vector3> verts = new List<Vector3>(vertCount);

        //tracks how far along the circles permiter we are
        float stepSize = (2 * Mathf.PI) / vertCount;

        for (int i = 0; i < vertCount; i++)
        {
            //finds position using formula. rotates around centre to change direction of the circle.
            //TODO: maybe there's a fomula to change roation that goes right into the vert coords
            Vector3 vertPos = centrePos + new Vector3(Mathf.Cos(i * stepSize) * radius, Mathf.Sin(i * stepSize) * radius, 0);
            vertPos = Quaternion.Euler(orientation) * (vertPos - centrePos) + centrePos;
            verts.Add(vertPos);
        }

        return verts;
    }
    
    /// <summary>
    /// Finds the number in the given range, percent of the way (lerp)
    /// </summary>
    private int RangeCacl(Vector2Int range, int percent)
    {
        return (int)(range.x + (percent / 100.0) * (range.y - range.x));
    }

    private void RotateAroundPivot(ref Vector3 point, Vector3 pivot, Vector3 rotation)
    {
        point = Quaternion.Euler(rotation) * (point - pivot) + pivot;
    }

}


[System.Serializable]
public struct MeshSettings
{
    //How defined the mesh is how many verts
    public int meshResolutionPercent;

}