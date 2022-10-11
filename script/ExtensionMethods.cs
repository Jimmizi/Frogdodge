using Godot;

public static class ExtensionMethods
{
    public static void Normalise(this ref Vector2 vec)
    {
        vec = vec.Normalized();
    }
}
