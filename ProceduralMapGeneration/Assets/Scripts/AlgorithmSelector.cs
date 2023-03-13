using UnityEngine;
using System.ComponentModel;
using System.Collections.Generic;
using System;

public enum HeightAlgorithm
{
    PerlinNoise,
    DiamondSquere
}

public enum ClimatAlgorithm
{
    Altitude,RandomCF
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
    public float amplitudeMultipier;

    public void OnValidate(){
        if(octaveCount<0)
            octaveCount = 0;
        if(lacunarity<1)
            lacunarity = 1;
        if(scale<=0)
            scale = 0.0001f;
        if(amplitudeMultipier<=0)
            scale = 0.0001f;
    }
}

[System.Serializable]
public struct DiamondSquereAttributes
{
    public float decrease;
    public float initialRandom;
    public int baseHeight;

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
    public List<IClimatFactor> climatFactors;

    public HeightAlgorithm heighMapAlgorithm;
    public List<ClimatAlgorithm> climatAlgorithms;
    
    public PerlinAttributes perlinAttributes;
    public DiamondSquereAttributes diamondSquereAttributes;
    
    public void Reset()
    {
        switch (heighMapAlgorithm)
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
        foreach(ClimatAlgorithm f in climatAlgorithms)
        {
            switch(f)
            {
                case ClimatAlgorithm.Altitude:
                    climatFactors.Add(new AltitudeCF());
                    break;
                case ClimatAlgorithm.RandomCF:
                    climatFactors.Add(new RandomCF());
                    break;
                default:
                    break;
            }
        }
    }

    void Awake()
    {
        climatFactors = new List<IClimatFactor>();
        Reset();
    }

    void OnValidate()
    {
        perlinAttributes.OnValidate();
        diamondSquereAttributes.OnValidate();
    }
}