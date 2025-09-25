using UnityEngine;

public class TestTerrain : MonoBehaviour
{
    [SerializeField] private float height = 1;
    [SerializeField] private int radius = 5;
    [SerializeField] [Range(0, 1)] private float roughness = 0.1f;
    
    public Terrain terrain;
    private TerrainData terrainData;
    private float timer;
    
    private Camera cam;

    private void Start()
    {
        terrain.terrainData = Instantiate(terrain.terrainData);
        terrainData = terrain.terrainData;
        GetComponent<TerrainCollider>().terrainData = terrainData;
        cam = Camera.main;
    }

    private void Update()
    {
        if (!Input.GetMouseButtonDown(0)) return;
        
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        if (!Physics.Raycast(ray, out RaycastHit hit)) return;
        
        RaiseTerrain(hit.point, radius, height / 100);
        
    }

    private void RaiseTerrain(Vector3 worldPos, float raiseRadius, float strength)
    {
        int hmWidth = terrainData.heightmapResolution;
        int hmHeight = terrainData.heightmapResolution;

        int mapX = (int)(worldPos.x / terrainData.size.x * hmWidth);
        int mapZ = (int)(worldPos.z / terrainData.size.z * hmHeight);

        int radiusInSamples = Mathf.RoundToInt(raiseRadius / terrainData.size.x * hmWidth);

        int xStart = Mathf.Clamp(mapX - radiusInSamples, 0, hmWidth - 1);
        int zStart = Mathf.Clamp(mapZ - radiusInSamples, 0, hmHeight - 1);
        int xEnd = Mathf.Clamp(mapX + radiusInSamples, 0, hmWidth - 1);
        int zEnd = Mathf.Clamp(mapZ + radiusInSamples, 0, hmHeight - 1);

        int sizeX = xEnd - xStart;
        int sizeZ = zEnd - zStart;

        float[,] heights = terrainData.GetHeights(xStart, zStart, sizeX, sizeZ);

        for (int x = 0; x < sizeX; x++)
        {
            for (int z = 0; z < sizeZ; z++)
            {
                float dx = (x + xStart - mapX) / (float)radiusInSamples;
                float dz = (z + zStart - mapZ) / (float)radiusInSamples;
                float dist = Mathf.Sqrt(dx * dx + dz * dz);

                if (dist <= 1f)
                {
                    float falloff = Mathf.Cos(dist * Mathf.PI * roughness);

                    heights[z, x] += strength * falloff;
                }
            }
        }

        terrainData.SetHeights(xStart, zStart, heights);
    }
}
