using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainUpdater : MonoBehaviour
{
    public RenderTexture heightRT;
    Texture2D tex;
    private float[,] heights;
    [SerializeField] Terrain t;
    [SerializeField] Camera cam;

    private void Start()
    {
        tex = new Texture2D(heightRT.width, heightRT.height, TextureFormat.RGB24, false);
        heights = t.terrainData.GetHeights(0, 0, t.terrainData.heightmapWidth, t.terrainData.heightmapHeight);
    }
    
    public void UpdateHeight()
    {
        int maxHeight = heights.GetLength(0);
        int maxLength = heights.GetLength(1);
        Debug.Log($"mh: {maxHeight} / ml {maxLength}");
        int count = 0;

        Rect rectReadPicture = new Rect(0, 0, tex.width, tex.height);

        for (int zCount = 0; zCount < maxHeight; zCount++)
        {
            for (int xCount = 0; xCount < maxLength; xCount++)
            {
                int pixX = Mathf.RoundToInt(xCount / maxLength);
                int pixY = Mathf.RoundToInt(zCount / maxHeight);

                //sample corresponding pixel here
                tex.ReadPixels(rectReadPicture, pixX, pixY, false);

                float p = tex.GetPixel(pixX, pixY).r;
                heights[zCount, xCount] = p * 30f;
                count++;

                if (count > 5000) break;
            }
        }

        t.terrainData.SetHeightsDelayLOD(0, 0, heights);
    }

    public void UpdateHeightMap()
    {
        cam.Render();
        Debug.Log("Update Heightmap");
        RenderTexture.active = heightRT;
        RectInt ri = new RectInt(0,0, 512, 512);
        t.terrainData.CopyActiveRenderTextureToHeightmap(ri, new Vector2Int(0,0), TerrainHeightmapSyncControl.HeightOnly);
        t.terrainData.SyncHeightmap();
    }
 }
