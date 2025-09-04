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

        int tick = 0;

        while (true)
        {
            Console.Clear();

            RenderMap(map);
            ProcessEntities(entities, map);
            RenderEntities(entities);
            RenderInfo(tick, entities);

            Console.SetCursorPosition(MapWidth + LeftOffset + 1, MapHeight + TopOffset + 1);

            var key = Console.ReadKey();
            HandleInput(key, entities);

            tick++;
        }
    }

    private static void HandleInput(ConsoleKeyInfo key, Entity[] entities)
    {
        if (key.Key == ConsoleKey.Q)
        {
            Environment.Exit(0);
        }
        else if (key.Key == ConsoleKey.LeftArrow)
        {
            entities[0].DeltaPosition = Position.Left;
        }
        else if (key.Key == ConsoleKey.RightArrow)
        {
            entities[0].DeltaPosition = Position.Right;
        }
        else if (key.Key == ConsoleKey.UpArrow)
        {
            entities[0].DeltaPosition = Position.Up;
        }
        else if (key.Key == ConsoleKey.DownArrow)
        {
            entities[0].DeltaPosition = Position.Down;
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
            Console.WriteLine($"{entities[i].Glyph}, {{ {entities[i].Position.X}, {entities[i].Position.Y} }}, {entities[0].CanPassThrough}");
        }
    }
}