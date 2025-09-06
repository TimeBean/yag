using DotnetNoise;
using YetAnotherGame.Components;

namespace YetAnotherGame.MapGenerator;

public class MapGenerator
{
    private readonly int _width;
    private readonly int _height;
    private readonly Random _random;
    private readonly List<TerrainRule> _rules;

    public MapGenerator(int width, int height, int seed)
    {
        _width = width;
        _height = height;
        _random = new Random(seed);

        _rules = new List<TerrainRule>
        {
            // water
            new TerrainRule(0.0f, 0.4f, (x, y) => CreateTile('~', x, y, false)),
            // sand (near water)
            new TerrainRule(0.4f, 0.45f, (x, y) => CreateTile(',', x, y, true)),
            // dirt
            new TerrainRule(0.45f, 0.7f, (x, y) => CreateTile('.', x, y, true)),
            // tree (10% chance in special zone)
            new TerrainRule(0.7f, 1.01f, (x, y) =>
            {
                int r = _random.Next(100);
                return r < 10 ? CreateTile('F', x, y, false) : CreateTile('.', x, y, true);
            })
        };
    }

    public GameObject[,] InitializeMap(int seed)
    {
        var map = new GameObject[_width, _height];
        var noiseMap = GenerateNoiseMap(_width, _height, seed);

        for (int i = 0; i < _width; i++)
        {
            for (int j = 0; j < _height; j++)
            {
                if (IsBorder(i, j))
                {
                    map[i, j] = CreateBoundary(j, i);
                    continue;
                }

                float noise = noiseMap[i, j];
                map[i, j] = CreateByNoise(noise, j, i);
            }
        }

        return map;
    }
    
    private bool IsBorder(int i, int j) => i == 0 || j == 0 || i == _width - 1 || j == _height - 1;

    private static GameObject CreateBoundary(int x, int y) => CreateTile('#', x, y, false);

    private GameObject CreateByNoise(float noiseValue, int x, int y)
    {
        foreach (var rule in _rules)
        {
            if (rule.Matches(noiseValue))
                return rule.Factory(x, y);
        }
        
        throw new Exception("No such tile");
    }

    private static GameObject CreateTile(char symbol, int x, int y, bool isWalkable) => new(symbol, x, y, isWalkable);

    private static float[,] GenerateNoiseMap(int width, int height, int seed)
    {
        var noise = new FastNoise(seed)
        {
            Frequency = 0.045f,
            Octaves = 5,
            Lacunarity = 2.0f,
            Gain = 0.5f,
            UsedNoiseType = FastNoise.NoiseType.Perlin,
        };

        var map = new float[width, height];

        for (var y = 0; y < height; y++)
        for (var x = 0; x < width; x++)
        {
            float fx = x;
            float fy = y;

            var raw = noise.GetNoise(fx, fy, 0.5f);

            var norm = (raw + 1f) * 0.5f;
            map[x, y] = Math.Clamp(norm, 0f, 1f);
        }

        return map;
    }
}