using Godot;
using System;

public class SplashScreen : Node2D
{
    [Export]
    public float ScaleInSpeed = 1.0f;
    
    [Export]
    public PackedScene GameplayScene;
    
    private TextureRect logo;
    private Vector2 originalScale;
    private Label jpg;
    
    float sfxTimer = 0.0f;
    float scaleTimer = 0.0f;
    float rotTimer = 0.0f;
    float alpha = 0.0f;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        logo = GetNode<TextureRect>("TextureRect");
        jpg = GetNode<Label>("CanvasLayer/Label");
        originalScale = logo.RectScale;
        logo.RectScale = Vector2.Zero;
        jpg.Modulate = new Color(1.0f, 1.0f, 1.0f, 0.0f);
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        if(sfxTimer >= 0.0f)
        {
            sfxTimer += delta;
            if(sfxTimer >= 0.5f)
            {
                GetNode<AudioStreamPlayer>("TextureRect/Rise").Play();
                sfxTimer = -1.0f;
            }
            
            return;
        }
        
        
        scaleTimer += delta * 0.2f * ScaleInSpeed;
        rotTimer += delta * 360.0f * ScaleInSpeed;
        
        if(scaleTimer >= 0.18f)
        {
            alpha += delta * 4.0f;
            alpha = Mathf.Clamp(alpha, 0.0f, 1.0f);
            jpg.Modulate = new Color(1.0f, 1.0f, 1.0f, alpha);
        }
        
        float scalar = Mathf.Clamp(scaleTimer, 0.0f, 0.2f);
        float rot = Mathf.Clamp(rotTimer, 0.0f, 360.0f);
        
        logo.RectScale = new Vector2(scalar, scalar);
        logo.RectRotation = rot;
        
        if(scaleTimer > 0.6f)
        {
            GetTree().ChangeSceneTo(GameplayScene);
        }
    }
}
