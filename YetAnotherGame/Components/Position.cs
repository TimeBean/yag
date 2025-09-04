namespace YetAnotherGame.Components;

public record struct Position(int X, int Y)
{
    public override string ToString() => $"({X},{Y})";

    public static Position operator +(Position a, Position b) => new(a.X + b.X, a.Y + b.Y);
    public static Position operator -(Position a, Position b) => new(a.X - b.X, a.Y - b.Y);
    public static Position operator *(Position a, int b) => new(a.X * b, a.Y * b);
    public static Position operator *(int b, Position a) => a * b;
    public static Position operator /(Position a, int b) => new(a.X / b, a.Y / b);
    public static Position operator -(Position a) => new(-a.X, -a.Y);


    public static readonly Position Zero  = new(0, 0);
    public static readonly Position Right = new(1, 0);
    public static readonly Position Left  = new(-1, 0);
    public static readonly Position Up    = new(0, -1);
    public static readonly Position Down  = new(0, 1);
    
    public static implicit operator (int, int)(Position p) => (p.X, p.Y);
    public static implicit operator Position((int x, int y) t) => new(t.x, t.y);
}