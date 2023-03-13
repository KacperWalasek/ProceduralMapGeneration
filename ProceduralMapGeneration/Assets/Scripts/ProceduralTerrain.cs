using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ProceduralTerrain : MonoBehaviour
{
    public int xWidth;
    public int zDepth;
    public int meshSize;
    public Material terrainMaterial;

    int[,][] triangles;

    int meshCountX;
    int meshCountZ;
    Transform[,] children;
    Mesh[,] meshes;
    MapIterator it;
    AlgorithmSelector selector;

    void CreateChildren()
    {
        meshes = new Mesh[meshCountX,meshCountZ];
        children = new Transform[meshCountX,meshCountZ];
        for(int i = 0; i<meshCountX; i++)
            for(int j = 0; j<meshCountZ; j++)
            {
                meshes[i,j] = new Mesh();
                GameObject child = new GameObject($"mesh{i}{j}");
                child.AddComponent<MeshFilter>();
                child.AddComponent<MeshRenderer>();
                child.transform.parent = this.transform;
                child.transform.GetComponent<MeshRenderer>().material = terrainMaterial;
                children[i,j] = child.transform;
            }
    }

    void CreateVertices()
    {
        selector.heightReshaper.Reshape(it,xWidth,zDepth);
    }

    void CreateTriangles()
    {
        triangles = new int[meshCountX,meshCountZ][];
        for(int i=0; i<meshCountX; i++)
            for(int j=0; j<meshCountZ; j++)
            {
                (int width,int depth) meshSize = it.map.getMeshSize(i,j);
                triangles[i,j] = new int[meshSize.width * meshSize.depth * 6];
                int trianglecount = 0;
                int vertexcounter = 0;
                for (int z = 0; z < meshSize.depth; z++)
                {
                    for (int x = 0; x < meshSize.width; x++)
                    {
                        triangles[i,j][0 + trianglecount] = vertexcounter + 0;
                        triangles[i,j][1 + trianglecount] = vertexcounter + meshSize.width + 1;
                        triangles[i,j][2 + trianglecount] = vertexcounter + 1;
                        triangles[i,j][3 + trianglecount] = vertexcounter + 1;
                        triangles[i,j][4 + trianglecount] = vertexcounter + meshSize.width + 1;
                        triangles[i,j][5 + trianglecount] = vertexcounter + meshSize.width + 2;
                        vertexcounter++;
                        trianglecount += 6;
                    }
                    vertexcounter++;
                }
            }
    }

    void SetupMeshes()
    {
        for(int i = 0; i<meshCountX; i++)
            for(int j = 0; j<meshCountZ; j++)
            {
                meshes[i,j].Clear();
                meshes[i,j].vertices = it.map.vertices[i,j];
                meshes[i,j].triangles = triangles[i,j];
                meshes[i,j].RecalculateNormals();
                meshes[i,j].SetColors(it.map.colors[i,j]);
                children[i,j].GetComponent<MeshFilter>().mesh = meshes[i,j];
            }
    }

    void SetupClimat()
    {
        foreach(IClimatFactor f in selector.climatFactors)
            f.Apply(it, xWidth, zDepth);
    }

    void SetupBiomes()
    {
        for(int i =0; i<xWidth; i++)
            for(int j=0; j<zDepth; j++)
            {
                if(it[i,j].temperature<0)
                    it[i,j].color = Color.white;
                else
                    if(it[i,j].temperature<15)
                        it[i,j].color = Color.gray;
                    else
                        if(it[i,j].temperature<30)
                            it[i,j].color = Color.green;
                        else
                            it[i,j].color = Color.yellow;
            }               
    }

    public void UpdateMeshes()
    {
        meshCountX = (int)Math.Ceiling((double)xWidth/meshSize);
        meshCountZ = (int)Math.Ceiling((double)zDepth/meshSize);
        selector.Reset();
        foreach (Transform child in children)
            GameObject.Destroy(child.gameObject);
        it = new MapIterator(xWidth,zDepth,meshSize,meshCountX,meshCountZ);
        CreateChildren();
        CreateVertices();
        CreateTriangles();
        SetupClimat();
        SetupBiomes();
        SetupMeshes();
    }
    //////////////////////////////////////////////////////

    void Start()
    {
        selector = GetComponent<AlgorithmSelector>();
        children = new Transform[0,0];
        UpdateMeshes();
    }

    void OnDrawGizmos()
    {
        if (it.map.vertices == null) return;
        for (int x = 0; x < meshCountX; x++)
            for (int z = 0; z < meshCountZ; z++)
                for (int i = 0; i < it.map.vertices[x,z].Length; i++)
                    Gizmos.DrawSphere(it.map.vertices[x,z][i], .5f);
    }

    public void OnValidate(){
        if(meshSize<0)
            meshSize = 0;
        if(meshSize>250)
            meshSize = 250;
        if(xWidth<1)
            xWidth = 1;
        if(zDepth<1)
            zDepth = 1;
    }
}
