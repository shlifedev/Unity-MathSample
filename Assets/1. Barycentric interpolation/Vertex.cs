
using UnityEngine;

[System.Serializable]
public struct Vertex<T>
{
    public Vertex(Vector3 point, T color)
    {
        this.point = point;
        this.data = color;
    }
    public Vector3 point;
    public T data;
}