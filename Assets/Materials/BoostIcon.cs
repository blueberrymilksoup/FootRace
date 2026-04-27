using UnityEngine;

public class BoostPadTextureGenerator : MonoBehaviour
{
    void Start()
    {
        int size = 512;
        Texture2D tex = new Texture2D(size, size);

        for (int y = 0; y < size; y++)
        {
            for (int x = 0; x < size; x++)
            {
                // Background — deep blue
                Color col = new Color(0.05f, 0.1f, 0.5f);

                // Outer ring
                float cx = x - size / 2f;
                float cy = y - size / 2f;
                float dist = Mathf.Sqrt(cx * cx + cy * cy);
                if (dist > size * 0.45f && dist < size * 0.5f)
                    col = new Color(0f, 1f, 1f); // Cyan ring

                // Lightning bolt — simple triangle shape
                if (x > size * 0.45f && x < size * 0.55f && y > size * 0.2f && y < size * 0.8f)
                    col = new Color(1f, 1f, 0f); // Yellow bolt

                tex.SetPixel(x, y, col);
            }
        }

        tex.Apply();

        // Apply to this object's material
        GetComponent<Renderer>().material.mainTexture = tex;
    }
}