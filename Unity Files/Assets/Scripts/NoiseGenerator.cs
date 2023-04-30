using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoiseGenerator : MonoBehaviour {
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static float[,] Generate (int width, int height, float scale, Wave[] waves, Vector2 offset) {
        //create noise map
        float[,] noiseMap = new float[width, height];

        //loop through each element in noise map
        for(int x = 0; x < width; x++) {
            for(int y = 0; y < height; ++y) {

                // calculate the sample positions
                float samplePosX = (float)x * scale + offset.x;
                float samplePosY = (float)y * scale + offset.y;
                float normalization = 0.0f;

                // loop through each wave
                foreach(Wave wave in waves) {

                    // sample the perlin noise taking into consideration amplitude and frequency
                    noiseMap[x, y] += wave.amplitude * Mathf.PerlinNoise(samplePosX * wave.frequency + wave.seed, samplePosY * wave.frequency + wave.seed);
                   // Debug.Log("Perlin Noise: " + Mathf.PerlinNoise(samplePosX * wave.frequency + wave.seed, samplePosY * wave.frequency + wave.seed));
                    normalization += wave.amplitude;
                }

                // normalise the value
                noiseMap[x, y] /= normalization;
            }
        }
        
        return noiseMap;
    }

}

[System.Serializable]
public class Wave {

    public float seed;
    public float frequency;
    public float amplitude;

}
