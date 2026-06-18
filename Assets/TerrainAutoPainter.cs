using UnityEngine;

[RequireComponent(typeof(Terrain))]
public class TerrainAutoPainter : MonoBehaviour
{
    [Header("Height Settings")]
    [Tooltip("Heights below this will be 100% grass.")]
    public float grassMaxHeight = 20f;
    [Tooltip("Heights above this will be 100% rock.")]
    public float rockMinHeight = 50f;

    [Header("Slope Settings")]
    [Tooltip("If the mountain slope is steeper than this angle, force it to be rock regardless of height.")]
    public float rockMinSlopeAngle = 45f;

    // The ContextMenu attribute is a fantastic Unity trick. 
    // It lets you right-click the script in the Inspector and run this function WITHOUT hitting Play!
    [ContextMenu("Auto Paint Terrain")]
    public void PaintTerrain()
    {
        Terrain terrain = GetComponent<Terrain>();
        TerrainData terrainData = terrain.terrainData;

        // Check if the user has added the layers in the Inspector yet
        if (terrainData.terrainLayers.Length < 2)
        {
            Debug.LogError("Please add at least 2 Terrain Layers to the Terrain inspector first! (Layer 0 = Grass, Layer 1 = Rock)");
            return;
        }

        int alphaWidth = terrainData.alphamapWidth;
        int alphaHeight = terrainData.alphamapHeight;

        // Unity stores terrain paint data (called a "Splatmap") in a 3D float array.
        // [y, x, layerIndex] where the float is a percentage from 0.0 to 1.0
        float[,,] splatmapData = new float[alphaHeight, alphaWidth, terrainData.alphamapLayers];

        for (int y = 0; y < alphaHeight; y++)
        {
            for (int x = 0; x < alphaWidth; x++)
            {
                // Normalize the coordinates (0.0 to 1.0)
                float normX = x * 1.0f / (alphaWidth - 1);
                float normY = y * 1.0f / (alphaHeight - 1);

                // Get the real world height and steepness at this specific point
                float height = terrainData.GetInterpolatedHeight(normX, normY);
                float steepness = terrainData.GetSteepness(normX, normY);

                // Calculate how much rock vs grass we want
                float grassWeight = 0f;
                float rockWeight = 0f;

                if (steepness > rockMinSlopeAngle)
                {
                    // It's a cliff! Gravity would make dirt slide off, so make it solid rock.
                    rockWeight = 1f;
                }
                else
                {
                    // Blend based on height using Mathf.InverseLerp.
                    // This returns a 0 to 1 value representing where our 'height' falls between our min/max thresholds.
                    rockWeight = Mathf.InverseLerp(grassMaxHeight, rockMinHeight, height);
                    grassWeight = 1f - rockWeight;
                }

                // Assign the weights (Assuming Layer 0 is Grass, Layer 1 is Rock)
                splatmapData[y, x, 0] = grassWeight;
                splatmapData[y, x, 1] = rockWeight;

                // Ensure any extra layers (if you add more later) are set to 0 here to prevent array errors
                for (int i = 2; i < terrainData.alphamapLayers; i++)
                {
                    splatmapData[y, x, i] = 0f;
                }
            }
        }

        // Apply the newly calculated paint data back to the terrain
        terrainData.SetAlphamaps(0, 0, splatmapData);
        Debug.Log("Terrain Auto-Painted Successfully!");
    }
}
