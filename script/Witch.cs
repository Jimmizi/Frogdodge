using Godot;
using System;

public class Witch : RigidBody2D
{    
    AnimatedSprite animSprite;
    private bool isFadingIn;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        animSprite = GetNode<AnimatedSprite>("AnimatedSprite");
        animSprite.Playing = true;
        animSprite.Animation = "walk";
        animSprite.Modulate = new Color(1.0f, 1.0f, 1.0f, 0.0f);
        
        isFadingIn = true;
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        if(isFadingIn)
        {
            ProcessFadeIn(delta);
        }
    }
    
    private void OnScreenExited()
    {
        QueueFree();
    }
    
    public void SetHoldingFrog(Player frog)
    {
        GetNode<AnimatedSprite>("AnimatedSprite").Animation = "carry";
        LinearVelocity = Vector2.Zero;
        
        frog.Position = Position + GetNode<Position2D>("Position2D").Position;
    }
    
    private void ProcessFadeIn(float delta)
    {
        if(animSprite.Modulate.a < 1.0f)
        {
            animSprite.Modulate = new Color(1.0f, 1.0f, 1.0f, animSprite.Modulate.a + delta);
        }
        else
        {
            isFadingIn = false;
        }
    }
}
