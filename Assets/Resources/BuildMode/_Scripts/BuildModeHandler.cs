using EasyRoads3Dv3;
using System;
using System.Runtime.CompilerServices;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.TerrainUtils;

public class BuildModeHandler : Singleton<BuildModeHandler>
{
    InputActionMap actionMap;
    InputAction buildAction, cancelAction;

    [Header("*** Build Mode ***")] //bm = Build Mode
    [SerializeField] bool isActive;
    [SerializeField] CinemachineCamera bm_Camera;
    [SerializeField] GameObject bm_cameraTarget;
    [SerializeField] Terrain activeTerrain;
    TerrainData terrainData;
    [SerializeField] LayerMask terrainMask = 1 << 6;
    [Space]
    [SerializeField] float bm_cameraMoveSpeed = 5f;
    [SerializeField] float bm_cameraZoomSpeed = 1f;
    [SerializeField] float bm_cameraRotationSpeed = 1f;
    [SerializeField] Vector2 bm_cameraZoomLimits = new Vector2(2, 30);
    [Space]
    [SerializeField] BuildItem_S activeBuildItem;
    [SerializeField] Vector3 activeMousePosition;

    [Header("Road Placement Settings")]
    [SerializeField] float strength = 1f;
    int aW, aH, brushRadius;

    [SerializeField] bool roadStartPointset = false, roadEndPointset = false;
    [SerializeField] Vector3 roadStartPos = Vector3.zero, roadEndPos = Vector3.zero;
    [SerializeField] float segmentLength = 10f;
    [SerializeField] int  pointsPerSegment = 10;
    [SerializeField] bool isRoadValid = false;
    [SerializeField] Color validRoadColor, InvalidRoadColor;
    [SerializeField] float roadEndPointMinDistance = 20f, roadEndPointCurrentDistance;
    [SerializeField] LineRenderer lineRenderer;

    protected override void Awake()
    {
        base.Awake();

        actionMap = InputHandler.Instance.GetInput().FindActionMap("BuildMode");
        buildAction = actionMap.FindAction("Build");
        buildAction.performed += ctx => BuildAction();
        cancelAction = actionMap.FindAction("Cancel");

        SetupTerrain();
        //ActivateBuildMode();
    }

    private void SetupTerrain()
    {
        if (!activeTerrain) activeTerrain = Terrain.activeTerrain;
        terrainData = activeTerrain.terrainData;

        aW = terrainData.alphamapWidth;
        aH = terrainData.alphamapHeight;

        float pixelsPerMetre = (float)aW / terrainData.size.x;
        brushRadius = Mathf.Max(1,
                Mathf.RoundToInt((transform.localScale.x / 2) * pixelsPerMetre));
    }

    public bool IsActive() { return isActive; }

    public void SetActiveBuildItem(BuildItem_S item)
    {
        if (item == null) return;
        activeBuildItem = item;

        ActivateBuildMode();
        Debug.Log($"Active Build Item set to: {activeBuildItem.name}");
    }

    private void ActivateBuildMode()
    {
        InputHandler.Instance.ChangeActionMap("BuildMode");
        print("Build Mode Enabled");

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        isActive = true;
    }
   
    private void Update()
    {

        if (isActive && activeBuildItem != null)
        {
            Vector2 mousePosition = Mouse.current.position.ReadValue();
            Ray ray = Camera.main.ScreenPointToRay(mousePosition);
            if (!Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, terrainMask)) return;

            activeMousePosition = hit.point - activeTerrain.transform.position;

            lineRenderer.enabled = (activeBuildItem.itemType ==
                BuildItemType.Road);

            switch (activeBuildItem.itemType)
            {
                case BuildItemType.Road:
                    lineRenderer.SetPosition(1,activeMousePosition);
                    
                    float pointDistance = Vector3.Distance(roadStartPos, activeMousePosition);
                    if(pointDistance < roadEndPointMinDistance)
                    {
                        isRoadValid = false;
                        lineRenderer.startColor = InvalidRoadColor;
                        lineRenderer.endColor = InvalidRoadColor;
                    }
                    else
                    {
                        isRoadValid = true;
                        lineRenderer.startColor = validRoadColor;
                        lineRenderer.endColor = validRoadColor;
                    }
                    break;

                case BuildItemType.TentSite:

                    break;

                case BuildItemType.TrailerSite:

                    break;

                case BuildItemType.CabinSite:

                    break;
            }
        }
    }
     
    private void BuildAction()
    {
        if (isActive && activeBuildItem != null)
        {
            switch (activeBuildItem.itemType)
            {
                case BuildItemType.Road:
                    RoadPlacement();
                    break;

                case BuildItemType.TentSite:

                    break;

                case BuildItemType.TrailerSite:

                    break;

                case BuildItemType.CabinSite:

                    break;
            }
        }
    }

    private void RoadPlacement()
    {
        if (!roadStartPointset)
        {
            //mouse clicked set point 1
            if (buildAction.IsPressed())
            {
                roadStartPos = activeMousePosition;
                lineRenderer.SetPosition(0, roadStartPos);
                roadStartPointset = true;
            }
        }
        else
        {
            //mouse clicked set point 2
            if (buildAction.IsPressed())
            {
                roadEndPos = activeMousePosition;
                lineRenderer.SetPosition(1, roadEndPos);
                roadEndPointset = true;



            }

        }
    }
    private void ClearRoadPoints()
    {
        roadStartPointset = false;
        roadEndPointset = false;

        roadStartPos = Vector3.zero;
        roadEndPos = Vector3.zero;
    }
    private void BuildRoad()
    {
        Vector3 localPos = transform.position - activeTerrain.transform.position;

        // world → UV
        float u = localPos.x / terrainData.size.x;
        float v = localPos.z / terrainData.size.z;

        // UV → texel float coords (centre‑of‑cell)
        float fx = u * aW - 0.5f;
        float fz = v * aH - 0.5f;

        // UV → alphamap cell indices (nearest lower cell)
        int cx = Mathf.FloorToInt(u * aW);
        int cz = Mathf.FloorToInt(v * aH);

        // Adjust the position to ensure it affects the terrain directly under the transform
        cx = Mathf.RoundToInt(fx);
        cz = Mathf.RoundToInt(fz);

        int startX = Mathf.Clamp(cx - brushRadius, 0, aW - 1);
        int startZ = Mathf.Clamp(cz - brushRadius, 0, aH - 1);
        int sizeX = cx + brushRadius - startX + 1;
        int sizeZ = cz + brushRadius - startZ + 1;
        if (sizeX <= 0 || sizeZ <= 0) return;

        float[,,] alphas = terrainData.GetAlphamaps(startX, startZ, sizeX, sizeZ);
        int layers = alphas.GetLength(2);

        for (int z = 0; z < sizeZ; z++)
            for (int x = 0; x < sizeX; x++)
            {
                // solid stamp centred under cube
                float dist = Vector2.Distance(
                    new Vector2(startX + x, startZ + z),
                    new Vector2(cx, cz));

                float add = (dist <= brushRadius) ? strength : 0f;
                if (add == 0f) continue;

                for (int l = 0; l < layers; l++)
                    alphas[z, x, l] *= (1f - add);

                RoadItem roadItem = activeBuildItem as RoadItem;
                alphas[z, x, roadItem.terrainLayerIndex] += add;
            }

        terrainData.SetAlphamaps(startX, startZ, alphas);
    }


}
