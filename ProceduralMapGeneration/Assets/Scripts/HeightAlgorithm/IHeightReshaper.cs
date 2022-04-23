using UnityEngine;

public interface IHeightReshaper 
{
    void Reshape(MeshIterator mesh, int xWidth, int zDepth);
}
