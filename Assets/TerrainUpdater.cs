using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainUpdater : MonoBehaviour
{
    public RenderTexture heightRT;
    Texture2D tex;

    [SerializeField] Terrain t;

    private void Start()
    {
        tex = new Texture2D(heightRT.width, heightRT.height, TextureFormat.RGB24, false);
    }
    /*
    public void UpdateHeight()
    {
        float[,] heights = t.terrainData.GetHeights(0, 0, t.terrainData.heightmapWidth, t.terrainData.heightmapHeight);
        int maxHeight = heights.GetLength(0);
        int maxLength = heights.GetLength(1);

        RenderTexture.active = heightRT;
        Rect rectReadPicture = new Rect(0, 0, tex.width, tex.height);
        
        for (int zCount = 0; zCount < maxHeight; zCount++)
        {
            for (int xCount = 0; xCount < maxLength; xCount++)
            {
                //sample corresponding pixel here
                Color32 p = tex.ReadPixels(rectReadPicture, 

                float height = 0.0f;

                height = (hit.point.y - t.transform.position.y) * meshHeightInverse;
                height += shiftHeight;
                heights[zCount, xCount] = height;
            }
        }

        t.terrainData.SetHeights(0, 0, heights);
    }
    */
}
