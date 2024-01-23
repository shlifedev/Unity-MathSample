using UnityEngine;

public struct TriangleData
{ 
    public Vector3[] points;
    public float size;
    public Vector3 center;
    public TriangleData(Vector3 a, Vector3 b, Vector3 c)
    { 
        Vector3 c1 = b - a;
        Vector3 c2 = c - a;
        Vector3 cross = Vector3.Cross(c1,c2);
            
        points = new[] { a, b, c }; // 점
        size = cross.magnitude / 2.0f; // 크기 
        center = a + b + c / 3; // 삼각형의 중심점 
    } 
}