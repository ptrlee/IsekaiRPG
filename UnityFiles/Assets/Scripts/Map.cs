using System.Diagnostics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{

    public BiomePreset[] biomes;
    public GameObject tilePrefab;

    [Header("Dimensions")]
    public int width = 50;
    public int height = 50;
    public float scale = 1.0f;
    public Vector2 offset;

   [Header("Height Map")]
    public Wave[] heightWaves;
    public float[,] heightMap;

    [Header("Moisture Map")]
    public Wave[] moistureWaves;
    private float[,] moistureMap;

    [Header("Heat Map")]
    public Wave[] heatWaves;
    private float[,] heatMap;
    


    void GenerateMap ()
    {
        // height map
        heightMap = NoiseGenerator.Generate(width, height, scale, heightWaves, offset);
        // moisture map
        moistureMap = NoiseGenerator.Generate(width, height, scale, moistureWaves, offset);
        // heat map
        heatMap = NoiseGenerator.Generate(width, height, scale, heatWaves, offset);
        for(int x = 0; x < width; x+=4)
        {
            for(int y = 0; y < height; y+=4)
            {
               // Debug.Log("Height Map: " + heightMap[x, y]);
                GameObject tile = Instantiate(tilePrefab, new Vector3(x, y, 0), Quaternion.identity);
                tile.GetComponent<SpriteRenderer>().sprite = GetBiome(heightMap[x, y], moistureMap[x, y], heatMap[x, y]).GetTileSprite();
            }
        }
    }

    BiomePreset GetBiome (float height, float moisture, float heat)
    {
        List<BiomeTempData> biomeTemp = new List<BiomeTempData>();
        BiomePreset biomeToReturn = null;
        foreach(BiomePreset biome in biomes)
        {
            if(biome.MatchCondition(height, moisture, heat))
            {
                biomeTemp.Add(new BiomeTempData(biome));                
            }
        }

        float curVal = 0.0f;
        foreach(BiomeTempData biome in biomeTemp)
        {
            if(biomeToReturn == null)
            {
                biomeToReturn = biome.biome;
                curVal = biome.GetDiffValue(height, moisture, heat);
            }
            else
            {
                if(biome.GetDiffValue(height, moisture, heat) < curVal)
                {
                    biomeToReturn = biome.biome;
                    curVal = biome.GetDiffValue(height, moisture, heat);
                }
            }
        }
        if(biomeToReturn == null)
            biomeToReturn = biomes[0];
        return biomeToReturn;
    }


    // Start is called before the first frame update
    void Start()
    {
        GenerateMap();
        loopPrefabs();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void loopPrefabs() {
        GameObject[] gameObjects = GameObject.FindGameObjectsWithTag("prefab");
        foreach (GameObject gameObject in gameObjects) {
            UnityEngine.Debug.Log(gameObject.GetComponent<SpriteRenderer>().sprite.name);
            UnityEngine.Debug.Log(gameObject.GetComponent<Transform>().position);
        }
    }
}

public class BiomeTempData
{
    public BiomePreset biome;
    public BiomeTempData (BiomePreset preset)
    {
        biome = preset;
    }
        
    public float GetDiffValue (float height, float moisture, float heat)
    {
        return (height - biome.minHeight) + (moisture - biome.minMoisture) + (heat - biome.minHeat);
    }
}