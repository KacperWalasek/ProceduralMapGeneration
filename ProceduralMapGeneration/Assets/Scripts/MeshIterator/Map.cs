using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Map 
{
    int meshCountX, meshCountZ, meshSize, xWidth, zDepth;

    public Map(int xWidth, int zDepth, int meshSize, int meshCountX, int meshCountZ)
    {
        this.xWidth = xWidth;
        this.zDepth = zDepth;
        this.meshSize = meshSize;
        this.meshCountX = meshCountX;
        this.meshCountZ = meshCountZ;

        vertices = new Vector3[meshCountX,meshCountZ][];
        colors = new Color[meshCountX,meshCountZ][];
        temperatures = new float[meshCountX,meshCountZ][];
        humidities = new float[meshCountX,meshCountZ][];

        for( int i=0; i< meshCountX; i++)
            for( int j=0; j< meshCountZ; j++)
            {
                (int width,int depth) sizes = getMeshSize(i,j); 
                int tabSize = (sizes.width+1)*(sizes.depth+1);

                vertices[i,j] = new Vector3[tabSize];
                colors[i,j] = new Color[tabSize];
                temperatures[i,j] = new float[tabSize];
                humidities[i,j] = new float[tabSize];
            }
    }

    public (int,int) getMeshSize(int x, int z)
    {
        int width = x==meshCountX-1 && xWidth%meshSize!=0 ? xWidth%meshSize : meshSize; 
        int depth = z==meshCountZ-1 && zDepth%meshSize!=0 ? zDepth%meshSize : meshSize; 
        return (width,depth);
    }

    public Vector3[,][] vertices{ get; private set; }
    public Color[,][] colors{ get; private set; }

    public float[,][] temperatures{ get; private set; }
    public float[,][] humidities{ get; private set; }
}
