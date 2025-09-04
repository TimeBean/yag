namespace YetAnotherGame.Components;

public class Entity : IObject
{
    public char Glyph { get; set; }
    public Position Position { get; set; }
    public bool CanPassThrough { get; set; }

    public Entity(char glyph, Position position)
    {
        Glyph = glyph;
        Position = position;
        CanPassThrough = false;
    }
    
    public void MoveBy(Position deltaPosition)
    {
        Position += deltaPosition;
    }
}