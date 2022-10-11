using Godot;
using System;

public class Player : Area2D
{
    [Export]
    public int HopDistance = 120;
    
    [Export]
    public int HopSpeed = 500;
    
    private Vector2 screenSize;
    private Vector2 directionToMouse = new Vector2();
    
    public override void _Ready()
    {
        screenSize = GetViewport().Size;
    }
    
    public override void _Draw()
    {
        DrawLine(new Vector2(0.0f, 0.0f), (directionToMouse * HopDistance), Colors.Black);
    }
    
    public override void _Process(float delta)
    {
        directionToMouse = GetViewport().GetMousePosition() - Position;
        directionToMouse.Normalise();
        
        //Update();
    }
}
