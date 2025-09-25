using System;
using UnityEngine;

public class TestTerrain : MonoBehaviour
{
    [SerializeField] private float raiseAmount = 0.05f;
    
    public Terrain terrain;
    
    private Camera cam;

    private void Start()
    {
        terrain.terrainData = Instantiate(terrain.terrainData);
        cam = Camera.main;
    }

    private void Update()
    {
        if (!Input.GetMouseButton(0)) return;
        
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        if (!Physics.Raycast(ray, out RaycastHit hit)) return;
        
        TerrainData terrainTerrainData = terrain.terrainData;
        
        Vector3 terrainPos = hit.point - terrain.transform.position;
        Vector3 mapCoord = new(terrainPos.x / terrainTerrainData.size.x, 0, terrainPos.z / terrainTerrainData.size.z);

        int mapX = (int)(mapCoord.x * terrainTerrainData.heightmapResolution);
        int mapZ = (int)(mapCoord.z * terrainTerrainData.heightmapResolution);

        float[,] heights = terrainTerrainData.GetHeights(mapX, mapZ, 1, 1);
        heights[0, 0] += raiseAmount;
        terrainTerrainData.SetHeights(mapX, mapZ, heights);
    }
}
