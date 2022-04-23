using UnityEngine;
 using System.ComponentModel;

public enum HeightAlgorithm
{
    PerlinNoise,
    DiamondSquere
}

[System.Serializable]
public struct PerlinAttributes
{
    public int octaveCount;
    [Range(0,1)]
    public float persistance;
    public float lacunarity;
    public float scale;
    public Vector2 offset;

    public void OnValidate(){
        if(octaveCount<0)
            octaveCount = 0;
        if(lacunarity<1)
            lacunarity = 1;
        if(scale<=0)
            scale = 0.0001f;
    }
}

[System.Serializable]
public struct DiamondSquereAttributes
{
    public float decrease;
    public float initialRandom;

    public void OnValidate(){
        if(decrease<1)
            decrease = 1;
        if(initialRandom<0)
            initialRandom = 0;
    }
}

public class AlgorithmSelector : MonoBehaviour
{
    public IHeightReshaper heightReshaper;
    public HeightAlgorithm algorithm;
    
    public PerlinAttributes perlinAttributes;
    public DiamondSquereAttributes diamondSquereAttributes;
    
    public void Reset()
    {
        switch (algorithm)
        {
            case HeightAlgorithm.PerlinNoise:
                heightReshaper = new PerlinNoise(perlinAttributes);
                break;
            case HeightAlgorithm.DiamondSquere:
                heightReshaper = new DiamondSquere(diamondSquereAttributes);
                break;
            default:
                heightReshaper = new PerlinNoise(perlinAttributes);
                break;
        }
    }

    void Awake()
    {
        Reset();
    }

    void OnValidate()
    {
        perlinAttributes.OnValidate();
        diamondSquereAttributes.OnValidate();
    }
}