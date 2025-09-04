namespace YetAnotherGame.Components;

public struct Position
{
    public Position(int x, int y)
    {
        X = x;
        Y = y;
    }
    
    public int X { get; set; }
    public int Y { get; set; }
    
    public override string ToString() => $"({X},{Y})";
    
    public static Position operator +(Position a, Position b)
    {
        return new Position(a.X + b.X, a.Y + b.Y);
    }

    public static Position operator -(Position a, Position b)
    {
        return new Position(a.X - b.X, a.Y - b.Y);
    }
}