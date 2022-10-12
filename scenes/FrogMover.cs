using Godot;
using System;

public class FrogMover : Timer
{
    public delegate void OnTimerUpdate(float time);
    
    private bool isMoving = false;
    
    public void StartMovement()
    {
        SetProcess(true);
        Start();
    }
    
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        SetProcess(false);
        Stop();
    }
    
    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        
    }
}
