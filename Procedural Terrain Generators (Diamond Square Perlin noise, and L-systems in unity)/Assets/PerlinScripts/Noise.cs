using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class Noise
{

    public enum NormalizeMode { Local, Global };

    public static float[,] GenerateNoiseMap(int mapWidth, int mapHeight, NoiseSettings settings, Vector2 sampleCentre)
    {
        float[,] noiseMap = new float[mapWidth, mapHeight];

        System.Random prng = new System.Random(settings.seed);
        Vector2[] octaveOffsets = new Vector2[settings.octaves];

        float maxPossibleHeight = 0;
        float amplitude = 1;
        float frequency = 1;

        for (int i = 0; i < settings.octaves; i++)
        {
            float offsetX = prng.Next(-100000, 100000) + settings.offset.x + sampleCentre.x;
            float offsetY = prng.Next(-100000, 100000) - settings.offset.y - sampleCentre.y;
            octaveOffsets[i] = new Vector2(offsetX, offsetY);

            maxPossibleHeight += amplitude;
            amplitude *= settings.persistance;
        }
        LSystemNoise system = new LSystemNoise();
        if (settings.AddLSystem || settings.UseLSystemOnly)
            system.GenerateNoise(4);


        float maxLocalNoiseHeight = float.MinValue;
        float minLocalNoiseHeight = float.MaxValue;

        float halfWidth = mapWidth / 2f;
        float halfHeight = mapHeight / 2f;


        for (int y = 0; y < mapHeight; y++)
        {
            for (int x = 0; x < mapWidth; x++)
            {

                amplitude = 1;
                frequency = 1;
                float noiseHeight = 0;

                for (int i = 0; i < settings.octaves; i++)
                {
                    float sampleX;
                    float sampleY;
                    if (settings.AddLSystem)
                    {
                        
                        sampleX = (x - halfWidth + octaveOffsets[i].x + system.GetNextValue()) / settings.scale * frequency;
                        sampleY = (y - halfHeight + octaveOffsets[i].y + system.GetNextValue()) / settings.scale * frequency;

                      
                    }
                    else
                    {
                        sampleX = (x - halfWidth + octaveOffsets[i].x) / settings.scale * frequency;
                        sampleY = (y - halfHeight + octaveOffsets[i].y) / settings.scale * frequency;

                    }
                    float perlinValue;
                    if (!settings.UseLSystemOnly)
                    {
                        perlinValue  = Mathf.PerlinNoise(sampleX, sampleY) * 2 - 1;
                    }
                else
                    {
                        perlinValue = system.GetNextValue();
                    }
                    noiseHeight += perlinValue * amplitude;

                    amplitude *= settings.persistance;
                    frequency *= settings.lacunarity;
                }

                if (noiseHeight > maxLocalNoiseHeight)
                {
                    maxLocalNoiseHeight = noiseHeight;
                }
                if (noiseHeight < minLocalNoiseHeight)
                {
                    minLocalNoiseHeight = noiseHeight;
                }
                noiseMap[x, y] = noiseHeight;

                if (settings.normalizeMode == NormalizeMode.Global)
                {
                    float normalizedHeight = (noiseMap[x, y] + 1) / (maxPossibleHeight / 0.9f);
                    noiseMap[x, y] = Mathf.Clamp(normalizedHeight, 0, int.MaxValue);
                }
            }
        }

        if (settings.normalizeMode == NormalizeMode.Local)
        {
            for (int y = 0; y < mapHeight; y++)
            {
                for (int x = 0; x < mapWidth; x++)
                {
                    noiseMap[x, y] = Mathf.InverseLerp(minLocalNoiseHeight, maxLocalNoiseHeight, noiseMap[x, y]);
                }
            }
        }

        return noiseMap;
    }

}

[System.Serializable]
public class NoiseSettings
{
    public Noise.NormalizeMode normalizeMode;

    public float scale = 50;

    public int octaves = 6;
    [Range(0, 1)]
    public float persistance = .6f;
    public float lacunarity = 2;

    public int seed;
    public Vector2 offset;
    public bool AddLSystem;
    public bool UseLSystemOnly;

    public void ValidateValues()
    {
        scale = Mathf.Max(scale, 0.01f);
        octaves = Mathf.Max(octaves, 1);
        lacunarity = Mathf.Max(lacunarity, 1);
        persistance = Mathf.Clamp01(persistance);
    }
}


public class LSystemNoise
{
    public Dictionary<char, string> rules = new Dictionary<char, string>();
    public int maxIterations;
    public int minIterations;
    public int iterations = 3;
    public string input = "A";
    private string output;
    public string result;
    System.Random prng = new System.Random(1);

    public List<float> generatedNoise = new List<float>();
    int counter;
    public void GenerateNoise(int width)
    {
        rules.Clear();

        rules.Add('A', "BBAABAAABAAAABAABAABAABB");
        //   rules.Add('B', "A");

        output = input;
        for (int i = 0; i < width; i++)
        {
            output = applyRules(output);
        }
        result = output;
        GenerateTerrain(result, width);
        //foreach (var one in generatedNoise)
        //{
        //    Debug.Log(one);
        //}
    }

    public float GetNextValue()
    {

        return generatedNoise[(counter++ % generatedNoise.Count)];
    }
    private void GenerateTerrain(string formula, float width)
    {
        float LastPos = 0;
        float firstValue = 0.1f;
        bool GoUp = false;
        foreach (char c in formula)
        {
            switch (c)
            {
                case 'A':
                    generatedNoise.Add(LastPos);

                    break;

                case 'B':
                    GoUp = !GoUp;
                    firstValue = ((firstValue + 0.4f) % 1f);
                    if (GoUp)
                    {
                        LastPos += firstValue;
                    }
                    else
                    {
                        LastPos -= firstValue;
                    }

                    LastPos = Mathf.Clamp(LastPos, -1f, 1f);

                    break;

                default:
                    break;
            }
        }

    }

    string applyRules(string p_input)
    {
        System.Text.StringBuilder sb = new System.Text.StringBuilder();
        // Loop through characters in the input string
        foreach (char c in p_input)
        {
            // If character matches key in rules, then replace character with rhs of rule
            if (rules.ContainsKey(c))
            {
                sb.Append(rules[c]);
            }
            // If not, keep the character
            else
            {
                sb.Append(c);
            }
        }
        // Return string with rules applied
        return sb.ToString();
    }
}