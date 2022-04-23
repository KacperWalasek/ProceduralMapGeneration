using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapPoint 
{
    Vector3[,][] vertices;
    float[,][] temperatures;
    float[,][] humidities;
    List<(int,int,int)> indices;
    
    public MapPoint(Vector3[,][] vertices, float[,][] temperatures, float[,][] humidities)
    {
        this.vertices = vertices;
        this.temperatures = temperatures;
        this.humidities = humidities;
        indices = new List<(int,int,int)>();
    }
    public Vector3 vertex
    {
        get => vertices[indices[0].Item1,indices[0].Item2][indices[0].Item3];
        set 
        {
            foreach ((int x,int z,int j) i in indices)
                vertices[i.x,i.z][i.j] = value; 
        }
    }
    public float temperature 
    {
        get => temperatures[indices[0].Item1,indices[0].Item2][indices[0].Item3];
        set 
        {
            foreach ((int x,int z,int j) i in indices)
                temperatures[i.x,i.z][i.j] = value; 
        }
    }
    public float humidity 
    {
        get => humidities[indices[0].Item1,indices[0].Item2][indices[0].Item3];
        set
        {
            foreach ((int x,int z,int j) i in indices)
                humidities[i.x,i.z][i.j] = value; 
        }
    }
    public void addIndices(int meshX, int meshY, int meshIndex)
    {
        this.indices.Add((meshX,meshY,meshIndex));
    }
}
