using UnityEngine;

public class PerlinNoise : IHeightReshaper
{
    int octaveCount;
    float persistance;
    float lacunarity;
    float scale;
    Vector2 offset;
    public PerlinNoise(PerlinAttributes perlinAttributes)
    {
        this.octaveCount = perlinAttributes.octaveCount;
        this.persistance = perlinAttributes.persistance;
        this.lacunarity = perlinAttributes.lacunarity;
        this.scale = perlinAttributes.scale;
        this.offset = perlinAttributes.offset;
    }

    public void Reshape(MeshIterator it, int xWidth, int zDepth)
    {
        for(int i = 0; i < xWidth+1; i++)
            for(int j = 0; j < zDepth+1; j++)
            {
                float amplitude = 1;
                float frequency = 1;
                float height = 0;

                for(int o = 0; o<octaveCount; o++)
                {
                    float sampleX = i*frequency/scale + offset.x;
                    float sampleY = j*frequency/scale + offset.y;
                    float noise =  Mathf.PerlinNoise(sampleX, sampleY)*2 - 1;
                    height += noise*amplitude;
                    amplitude *= persistance;
                    frequency *= lacunarity;
                }
                it[i,j].vertex = new Vector3(i, height*20, j);
            }
    }
}
