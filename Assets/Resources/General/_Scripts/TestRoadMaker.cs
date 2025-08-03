using UnityEngine;

public class TestRoadMaker : MonoBehaviour
{
    public Terrain terrain;
    int roadLayerIndex = 1;

   // public float roadWidth;   // metres
    [Range(0, 1)] public 

    TerrainData td;
    int aW, aH, brushRadius;

    void Awake()
    {
        

        
    }

    void Update()
    {
        MarkTerrain();   
    }

    private void MarkTerrain()
    {

        
    }
}
