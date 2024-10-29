using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct MeshPoint
{
    public Vector3 position;
    public Vector3 normal;
    public MeshPoint(Vector3 position, Vector3 normal)
    {
        this.position = position;
        this.normal = normal;
    }
}