using UnityEngine;

public interface IHeightReshaper 
{
    void Reshape(MapIterator mesh, int xWidth, int zDepth);
}
