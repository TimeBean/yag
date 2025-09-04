namespace YetAnotherGame.Components;

public class Entity : IObject
{
    public char Glyph { get; set; }
    public Position Position { get; set; }
    public bool CanPassThrough { get; set; }
    public Position DeltaPosition { get; set; }

    public Entity(char glyph, Position position)
    {
        Glyph = glyph;
        Position = position;
        CanPassThrough = false;
    }

    public void MoveBy(Position deltaPosition, GameObject[,] map, Entity[] entities)
    {
        var newPosition = Position + deltaPosition;

        var xMax = map.GetLength(0);
        var yMax = map.GetLength(1);

        if (newPosition.X < xMax && newPosition.Y < yMax)
        {
            if (map[newPosition.X, newPosition.Y].CanPassThrough)
            {
                bool entityCanPassThrough = true;

                for (var i = 0; i < entities.Length; i++)
                {
                    if (Equals(entities[i].Position, newPosition))
                    {
                        entityCanPassThrough = false;
                        break;
                    }
                }

                if (entityCanPassThrough)
                {
                    Position += deltaPosition;
                }
            }

            DeltaPosition = Position.Zero;
        }
    }

    public void Seek(GameObject[,] map, Entity[] entities)
    {
        var random = new Random();
        var randomValue = random.Next(0, 4);

        Position seekPosition;

        if (randomValue == 0)
        {
            seekPosition = Position.Left;
        }
        else if (randomValue == 1)
        {
            seekPosition = Position.Right;
        }
        else if (randomValue == 2)
        {
            seekPosition = Position.Up;
        }
        else if (randomValue == 3)
        {
            seekPosition = Position.Down;
        }
        else
        {
            throw new ArgumentOutOfRangeException("");
        }

        MoveBy(seekPosition, map, entities);
    }
}