using UnityEngine;
using System;

public class DiamondSquere : IHeightReshaper
{
    public float decrease;
    public float initialRandom;

    public DiamondSquere(DiamondSquereAttributes diamondSquereAttributes)
    {
        this.decrease = diamondSquereAttributes.decrease;
        this.initialRandom = diamondSquereAttributes.initialRandom;
    }
    public void Reshape(MeshIterator it, int xWidth, int zDepth)
    {
        int log = (int)Math.Ceiling(Math.Log(Math.Max(xWidth,zDepth),2));
        int mapSize = (int)Math.Pow(2,log) + 1;
        double[,] tmpMap = new double[mapSize,mapSize];
        float h = initialRandom*log;
        for (int sideLength = mapSize - 1; sideLength >= 2; sideLength /= 2, h /= decrease) {
            int halfSide = sideLength / 2;
            for (int x = 0; x < mapSize - 1; x += sideLength) 
                for (int y = 0; y < mapSize - 1; y += sideLength) {
                    
                    double avg = tmpMap[x, y] + //top left
                        tmpMap[x + sideLength, y] +//top right
                        tmpMap[x, y + sideLength] + //lower left
                        tmpMap[x + sideLength, y + sideLength];//lower right
                        avg /= 4.0;
                    tmpMap[
                        x + halfSide, 
                        y + halfSide
                        ] = 
                        avg + 
                        UnityEngine.Random.Range(-h,h);
                }
            for (int x = 0; x < mapSize - 1; x += halfSide) {
                for (int y = (x + halfSide) % sideLength; y < mapSize - 1; y += sideLength) {
                    double avg =
                        tmpMap[(x - halfSide + mapSize) % mapSize, y] + //left of center
                        tmpMap[(x + halfSide) % mapSize, y] + //right of center
                        tmpMap[x, (y + halfSide) % mapSize] + //below center
                        tmpMap[x, (y - halfSide + mapSize) % mapSize]; //above center
                    avg /= 4.0;

                    avg = avg + UnityEngine.Random.Range(-h,h);
                    tmpMap[x, y] = avg;

                    if (x == 0) tmpMap[mapSize - 1, y] = avg;
                    if (y == 0) tmpMap[x, mapSize - 1] = avg;
                }
            }
        }
        for(int i = 0; i < zDepth+1; i++)
            for(int j = 0; j < xWidth+1; j++)
            {
                it[i,j].vertex = new Vector3(i, (float)tmpMap[i,j], j);
            }
    }
}
