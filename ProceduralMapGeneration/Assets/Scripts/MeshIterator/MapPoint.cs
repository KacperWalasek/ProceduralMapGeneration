using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapPoint 
{
    Map map;
    List<(int,int,int)> indices;
    
    public MapPoint(Map map)
    {
        this.map = map;
        indices = new List<(int,int,int)>();
    }

    
    public Vector3 vertex
    {
        get => map.vertices[indices[0].Item1,indices[0].Item2][indices[0].Item3];
        set 
        {
            foreach ((int x,int z,int j) i in indices)
                map.vertices[i.x,i.z][i.j] = value; 
        }
    }
    public Color color
    {
        get => map.colors[indices[0].Item1,indices[0].Item2][indices[0].Item3];
        set 
        {
            foreach ((int x,int z,int j) i in indices)
                map.colors[i.x,i.z][i.j] = value; 
        }
    }
    public float temperature 
    {
        get => map.temperatures[indices[0].Item1,indices[0].Item2][indices[0].Item3];
        set 
        {
            foreach ((int x,int z,int j) i in indices)
                map.temperatures[i.x,i.z][i.j] = value; 
        }
    }
    public float humidity 
    {
        get => map.humidities[indices[0].Item1,indices[0].Item2][indices[0].Item3];
        set
        {
            foreach ((int x,int z,int j) i in indices)
                map.humidities[i.x,i.z][i.j] = value; 
        }
    }
    public void addIndices(int meshX, int meshY, int meshIndex)
    {
        this.indices.Add((meshX,meshY,meshIndex));
    }
}
