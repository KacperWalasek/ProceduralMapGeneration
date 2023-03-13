using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AltitudeCF : IClimatFactor
{
    public void Apply(MapIterator it, int xWidth, int zDepth)
    {
        int baseTemp = 30;
        for(int i =0; i<xWidth; i++)
            for(int j=0; j<zDepth; j++)
                it[i,j].temperature = baseTemp-it[i,j].vertex.y/10;
    }
}
