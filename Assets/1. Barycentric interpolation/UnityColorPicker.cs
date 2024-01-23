using System.Collections.Generic;  
using UnityEngine;  
//3개의 포인트와 하나의 선택된 점의 위치를 가지는 객체 
public class UnityColorPicker : MonoBehaviour
{
     
    private const float SIZE = 3.0f;
    [SerializeField] private List<Vertex<Color>> points = new List<Vertex<Color>>();
    [SerializeField] private Vector3 pickPoint = Vector3.zero;
    [SerializeField] private TriangleData rootTriangle;
    [SerializeField] private List<TriangleData> triangles = new List<TriangleData>(capacity: 3);
    [SerializeField] private Color pickColor;
    private void Awake()
    {
        points.Add(new Vertex<Color>(Vector3.up * SIZE, new Color(1,0,0)));
        // Vector.down 은 0,-1,0, Vector3.left는 -1,0,0
        // 이 둘을 더하면 (-1,-1,0) 이 되며 이 값을 *3 곱해 (-3,-3,) 으로 만듭니다.
        points.Add(new Vertex<Color>((Vector3.down+Vector3.left) * SIZE, new Color(0,1,0)));
        // 밑변을 만들기 위해서 위 코드에서 Vector.left에 음수를 취합니다.
        points.Add(new Vertex<Color>((Vector3.down+-Vector3.left) * SIZE, new Color(0,0,1)));
        
        rootTriangle = new TriangleData(points[0].point, points[1].point, points[2].point);
    }

    private void DrawTriangle()
    { 
        for (var index = 0; index < points.Count; index++)
        {
            // 각 위치에 점을 찍습니다.
            var point = points[index];
            Gizmos.color = point.data;
            Gizmos.DrawSphere(point.point, 0.1f); 
            // (선택) 각 위치에 선을 그립니다.
            if (index != points.Count - 1) 
                Gizmos.DrawLine(points[index].point, points[index+1].point); 
            else if (index == points.Count-1) // 마지막 점은 반드시 첫번째 점과 이어집니다.
                Gizmos.DrawLine(points[index].point, points[0].point);  
        }

    }

    private void UpdatePickPoint()
    {
        pickPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        pickPoint.z = 0;
        CalculateTriangle();
        UpdateColor();
    }

    private void CalculateTriangle()
    {
        triangles.Clear(); 
        for (int i = 0; i < points.Count; i++)
        {
            float sizeOfTriangle = 0;
            if (i == points.Count - 1)
            {  
                triangles.Add(new TriangleData(points[^1].point, points[0].point, pickPoint));  
            } 
            else
            {
                triangles.Add(new TriangleData(points[i].point, points[i + 1].point, pickPoint));  
            } 
        } 
    }

    private void UpdateColor()
    {
        var weight1 = triangles[0].size / rootTriangle.size;
        var weight2 = triangles[1].size / rootTriangle.size;
        var weight3 = triangles[2].size / rootTriangle.size;
        
        // 삼각형을 쪼개는 순서로 인해 weight2(두번쨰 쪼갠 삼각형)이 r이 됨.
        var r = 1 * weight2;
        var b = 1 * weight1; 
        var g = 1 * weight3;
        pickColor = new Color(r, g, b);
    }
    
    private void OnDrawGizmos()
    { 
            DrawTriangle(); 
            foreach (var triangle in triangles) 
                DrawTriangleData(triangle); 
            Gizmos.color = pickColor;
            Gizmos.DrawSphere(pickPoint, 0.2f);
    }


    private void DrawTriangleData(TriangleData data)
    {
        Gizmos.color = Color.white + new Color(0, 0, 0, -0.5f);
            for (int j = 0; j < data.points.Length; j++)
            {
                if (j == data.points.Length - 1)
                {
                    Gizmos.DrawLine(data.points[^1], data.points[0]);   
                }
                else
                {
                    Gizmos.DrawLine(data.points[j], data.points[j + 1]); 
                }
            } 
    }  
    
    

    private void Update()
    {
        UpdatePickPoint();
    }
}