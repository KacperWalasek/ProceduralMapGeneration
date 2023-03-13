using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MapIterator
{
    int meshCountX, meshCountZ, meshSize, xWidth, zDepth;
    MapPoint[,][] points{ get; set;}
    public Map map {get; private set;}

    public MapIterator(int xWidth, int zDepth, int meshSize, int meshCountX, int meshCountZ)
    {
        this.xWidth = xWidth;
        this.zDepth = zDepth;
        this.meshSize = meshSize;
        this.meshCountX = meshCountX;
        this.meshCountZ = meshCountZ;

        map = new Map(xWidth,zDepth,meshSize, meshCountX, meshCountZ);
        points = new MapPoint[meshCountX,meshCountZ][];

        for( int i=0; i< meshCountX; i++)
            for( int j=0; j< meshCountZ; j++)
            {
                (int width,int depth) sizes = map.getMeshSize(i,j); 
                int tabSize = (sizes.width+1)*(sizes.depth+1);

                points[i,j] = new MapPoint[tabSize];
                
                for(int p=0; p < tabSize; p++)
                {
                    if(p%(sizes.width+1)==0 && i!=0)
                    {
                        (int,int) prevSizes = map.getMeshSize(i-1,j);

                        points[i,j][p] = points[i-1,j][(prevSizes.Item1+1)*(int)(Math.Floor((double)p/(sizes.width+1))+1)-1];
                        points[i,j][p].addIndices(i,j,p);
                        continue;
                    }
                    if(p<sizes.width+1 && j!=0)
                    {
                        (int,int) prevSizes = map.getMeshSize(i,j-1);

                        points[i,j][p] = points[i,j-1][(prevSizes.Item1+1)*prevSizes.Item2+p];
                        points[i,j][p].addIndices(i,j,p);
                        continue;
                    }
                    points[i,j][p] = new MapPoint(map);
                    points[i,j][p].addIndices(i,j,p);
                }
            }
    }

    (int,int,int) getIndices(int x, int z)
    {
        int index1 = x==0 ? 0 : (int)Math.Floor((double)(x-1)/meshSize);
        int index2 = z==0 ? 0 : (int)Math.Floor((double)(z-1)/meshSize);

        (int width,int depth) sizes = map.getMeshSize(index1,index2);  
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
