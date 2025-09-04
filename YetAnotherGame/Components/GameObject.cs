namespace YetAnotherGame.Components;

public class GameObject : IObject
{
    public char Glyph { get; set; }
    public Position Position { get; set; }
    public bool CanPassThrough { get; set; }

    public GameObject(char glyph, Position position, bool canPassThrough)
    {
        Glyph = glyph;
        Position = position;
        CanPassThrough = canPassThrough;
    }

    public GameObject(char glyph, int positionX, int positionY, bool canPassThrough) : this(glyph, new Position(positionX, positionY), canPassThrough) { }
}