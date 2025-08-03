using System.Collections.Generic;
using UnityEngine;

public class RoadHandler : Singleton<RoadHandler>
{
    [SerializeField] 
    protected override void Awake()
    {
        base.Awake();
    }
}
[System.Serializable] public class Road
{
    public Vector3 startPoint, endPoint;
    public List<Vector3> points;
}