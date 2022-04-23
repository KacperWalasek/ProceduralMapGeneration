using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MeshIterator
{
    int meshCountX, meshCountZ, meshSize, xWidth, zDepth;
    MapPoint[,][] points{ get; set;}

    public Vector3[,][] vertices{ get; private set; }
    public float[,][] temperatures{ get; private set; }
    public float[,][] humidities{ get; private set; }

    public MeshIterator(int xWidth, int zDepth, int meshSize)
    {
        this.xWidth = xWidth;
        this.zDepth = zDepth;
        this.meshSize = meshSize;
        
        meshCountX = (int)Math.Ceiling((double)xWidth/meshSize);
        meshCountZ = (int)Math.Ceiling((double)zDepth/meshSize);

        vertices = new Vector3[meshCountX,meshCountZ][];
        temperatures = new float[meshCountX,meshCountZ][];
        humidities = new float[meshCountX,meshCountZ][];
        points = new MapPoint[meshCountX,meshCountZ][];

        for( int i=0; i< meshCountX; i++)
            for( int j=0; j< meshCountZ; j++)
            {
                (int width,int depth) sizes = getMeshSize(i,j); 
                int tabSize = (sizes.width+1)*(sizes.depth+1);

                vertices[i,j] = new Vector3[tabSize];
                temperatures[i,j] = new float[tabSize];
                humidities[i,j] = new float[tabSize];
                points[i,j] = new MapPoint[tabSize];
                
                for(int p=0; p < tabSize; p++)
                {
                    if(p%(sizes.width+1)==0 && i!=0)
                    {
                        (int,int) prevSizes = getMeshSize(i-1,j);

                        points[i,j][p] = points[i-1,j][(prevSizes.Item1+1)*(int)(Math.Floor((double)p/(sizes.width+1))+1)-1];
                        points[i,j][p].addIndices(i,j,p);
                        continue;
                    }
                    if(p<sizes.width+1 && j!=0)
                    {
                        (int,int) prevSizes = getMeshSize(i,j-1);

                        points[i,j][p] = points[i,j-1][(prevSizes.Item1+1)*prevSizes.Item2+p];
                        points[i,j][p].addIndices(i,j,p);
                        continue;
                    }
                    points[i,j][p] = new MapPoint(vertices, temperatures, humidities);
                    points[i,j][p].addIndices(i,j,p);
                }
            }
    }

    public (int,int) getMeshSize(int x, int z)
    {
        int width = x==meshCountX-1 && xWidth%meshSize!=0 ? xWidth%meshSize : meshSize; 
        int depth = z==meshCountZ-1 && zDepth%meshSize!=0 ? zDepth%meshSize : meshSize; 
        return (width,depth);
    }

    (int,int,int) getIndices(int x, int z)
    {
        int index1 = x==0 ? 0 : (int)Math.Floor((double)(x-1)/meshSize);
        int index2 = z==0 ? 0 : (int)Math.Floor((double)(z-1)/meshSize);

        (int width,int depth) sizes = getMeshSize(index1,index2);  
        int index3part1 = z==0 ? 0 : (((z-1)%meshSize)+1)*(sizes.width+1);
        int index3part2 = x==0 ? 0 : (x-1)%meshSize + 1;

        return (index1,index2,index3part1 + index3part2);
    }

    public MapPoint this[int x,int z]
    {
        get 
        {
            (int,int,int) indices = getIndices(x,z);
            return points[indices.Item1,indices.Item2][indices.Item3]; 
        }
    }

}
