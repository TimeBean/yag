using YetAnotherGame.Components;

namespace YetAnotherGame.MapGenerator;

public class TerrainRule
{
    public float Min { get; }
    public float Max { get; }
    public Func<int, int, GameObject> Factory { get; }

    public TerrainRule(float min, float max, Func<int, int, GameObject> factory)
    {
        Min = min;
        Max = max;
        Factory = factory;
    }

    public bool Matches(float value) => value >= Min && value < Max;
}