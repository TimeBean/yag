namespace YetAnotherGame.Components;

public interface IObject
{
    public char Glyph { get; set; }
    public Position Position { get; set; }
    public bool CanPassThrough { get; set; }
}