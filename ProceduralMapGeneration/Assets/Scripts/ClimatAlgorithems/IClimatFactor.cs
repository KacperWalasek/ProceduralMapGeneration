using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IClimatFactor
{
    void Apply(MapIterator it, int xWidth, int zDepth);    
}
