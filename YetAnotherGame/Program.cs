using System.Text;
using YetAnotherGame.Components;

namespace YetAnotherGame;

class Program
{
    private const int MapWidth = 44;
    private const int MapHeight = 20;

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

        map[3, 5] = new GameObject('F', 3, 5, false);
        map[5, 7] = new GameObject('F', 5, 7, false);

        return map;
    }

    private static Entity[] InitializeEntities()
    {
        var entities = new[]
        {
            new Entity('@', new Position(3, 3)),
            new Entity('&', new Position(5, 5)),
            new Entity('&', new Position(7, 5))
        };

        return entities.ToArray();
    }

    private static void StartGame(GameObject[,] map, Entity[] entities)
    {
        Console.Clear();

        var tick = 0;

        while (true)
        {
            Console.Clear();

            RenderMap(map);
            ProcessEntities(entities, map);
            RenderEntities(entities);
            RenderInfo(tick, entities);

            Console.SetCursorPosition(MapWidth + LeftOffset + 1, MapHeight + TopOffset + 1);

            var key = Console.ReadKey(true);
            if (HandleInput(key, entities[0]))
                break;
            
            tick++;
        }
    }

    /// <summary>
    /// Processes player's input. Returns true if the application should be terminated otherwise set delta position to player (entities[0]).
    /// </summary>
    private static bool HandleInput(ConsoleKeyInfo key, Entity player)
    {
        if (key.Key == ConsoleKey.Q)
            return true;

        Position? move = key.Key switch
        {
            ConsoleKey.LeftArrow => Position.Left,
            ConsoleKey.RightArrow => Position.Right,
            ConsoleKey.UpArrow => Position.Up,
            ConsoleKey.DownArrow => Position.Down,
            _ => null
        };

        if (move.HasValue)
            player.DeltaPosition = move.Value;

        return false;
    }

    private static void RenderMap(GameObject[,] map)
    {
        var stringBuilder = new StringBuilder();

        for (var k = 0; k < TopOffset; k++)
        {
            stringBuilder.Append('\n');
        }

        for (var i = 0; i < map.GetLength(1); i++)
        {
            for (var k = 0; k < LeftOffset; k++)
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
            Console.SetCursorPosition(entity.Position.X + LeftOffset, entity.Position.Y + TopOffset);
            Console.WriteLine(entity.Glyph);
        }
    }

    private static void ProcessEntities(Entity[] entities, GameObject[,] map)
    {
        var player = entities[0];
        player.MoveBy(player.DeltaPosition, map, entities);

        for (var i = 1; i < entities.Length; i++)
        {
            entities[i].Seek(map, entities);
        }
    }

    private static void RenderInfo(int tick, Entity[] entities)
    {
        var infoLeftOffset = 3;

        Console.SetCursorPosition(MapWidth + LeftOffset + infoLeftOffset, TopOffset);
        Console.WriteLine($"Info:");

        Console.SetCursorPosition(MapWidth + LeftOffset + infoLeftOffset, TopOffset + 2);
        Console.WriteLine($"tick: {tick}");

        Console.SetCursorPosition(MapWidth + LeftOffset + infoLeftOffset, TopOffset + 4);
        Console.WriteLine("movement: arrows");
        Console.SetCursorPosition(MapWidth + LeftOffset + infoLeftOffset, TopOffset + 5);
        Console.WriteLine("exit: q");

        for (var i = 0; i < entities.Length; i++)
        {
            Console.SetCursorPosition(MapWidth + LeftOffset + infoLeftOffset, TopOffset + 6 + i);
            Console.WriteLine(
                $"{entities[i].Glyph}, {{ {entities[i].Position.X}, {entities[i].Position.Y} }}, {entities[0].CanPassThrough}");
        }
    }
}