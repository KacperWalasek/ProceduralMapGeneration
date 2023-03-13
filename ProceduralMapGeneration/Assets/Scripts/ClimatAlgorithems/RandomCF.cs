using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomCF : IClimatFactor
{
    public void Apply(MapIterator it, int xWidth, int zDepth)
    {
        float frequency = 1;
        float scale = 10;
        Vector2 offset = new Vector2(10,1);
        float multiplyer = 2;
        for(int i =0; i<xWidth; i++)
            for(int j=0; j<zDepth; j++)
            {
                float sampleX = i*frequency/scale + offset.x;
                float sampleY = j*frequency/scale + offset.y;
                float noise =  Mathf.PerlinNoise(sampleX, sampleY)*2 - 1;
                it[i,j].temperature = it[i,j].temperature + noise*multiplyer;
            }
    }
}
