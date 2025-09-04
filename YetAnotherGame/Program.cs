using System.Text;
using YetAnotherGame.Components;

namespace YetAnotherGame;

class Program
{
    private const int MapWidth = 22;
    private const int MapHeight = 10;

    private const int LeftOffset = 10;
    private const int TopOffset = 4;

    static void Main(string[] args)
    {
        var map = InitializeMap(MapWidth, MapHeight);
        var entities = InitializeEntities();


        StartGame(map, entities);
    }

    private static GameObject[,] InitializeMap(int width, int height)
    {
        var map = new GameObject[width, height];

        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (i == 0 || j == 0 || i == width - 1 || j == height - 1)
                {
                    map[i, j] = new GameObject('#', j, i, false);
                }
                else
                {
                    map[i, j] = new GameObject('.', j, i, true);
                }
            }
        }

        return map;
    }

    private static Entity[] InitializeEntities()
    {
        var Entities = new Entity[1];

        Entities[0] = new Entity('@', new Position(3, 3));

        return Entities.ToArray();
    }

    private static void StartGame(GameObject[,] map, Entity[] entities)
    {
        Console.Clear();

        int tick = 0;

        while (true)
        {
            Console.Clear();
            RenderMap(map);
            RenderEntities(entities);
            RenderInfo(tick);

            Console.ReadKey();
            tick++;
        }
    }

    private static void RenderMap(GameObject[,] map)
    {
        var stringBuilder = new StringBuilder();
        
        for (int k = 0; k < TopOffset; k++)
        {
            stringBuilder.AppendLine();
        }

        for (var i = 0; i < map.GetLength(1); i++)
        {
            for (int k = 0; k < LeftOffset; k++)
            {
                stringBuilder.Append(' ');
            }
            
            for (var j = 0; j < map.GetLength(0); j++)
            {
                stringBuilder.Append(map[j, i].Glyph);
            }

            stringBuilder.AppendLine();
        }

        Console.WriteLine(stringBuilder.ToString());
    }

    private static void RenderEntities(Entity[] entities)
    {
        foreach (var entity in entities)
        {
            entity.MoveBy(new Position(1, 0));
            
            Console.SetCursorPosition(entity.Position.X + LeftOffset, entity.Position.Y + TopOffset);
            Console.WriteLine(entity.Glyph);
        }
    }

    private static void RenderInfo(int tick)
    {
        Console.SetCursorPosition(MapWidth + LeftOffset + 3, TopOffset);
        Console.WriteLine($"tick: {tick}");
    }
}