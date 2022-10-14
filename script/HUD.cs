using Godot;
using System;

public class HUD : CanvasLayer
{
    [Signal]
    public delegate void StartGamePressed();
    
    public void ShowMessage(string text)
    {
        var msg = GetNode<Label>("Message");
        var msgBg = GetNode<TileMap>("TileMap");
        
        msg.Text = text;
        msg.Show();
        msgBg.Show();
        
        GetNode<Timer>("MessageTimer").Start();
    }
    
    public void UpdateScore(int score)
    {
        GetNode<Label>("TextureRect/ScoreLabel").Text = score.ToString();
    }
    
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        
    }
    
    async public void ShowGameOver()
    {
        ShowMessage("Oh no!");
        
        var msgTimer = GetNode<Timer>("MessageTimer");
        await ToSignal(msgTimer, "timeout");
        
        var msg = GetNode<Label>("Message");
        var msgBg = GetNode<TileMap>("TileMap");
        
        msg.Text = "Avoid the witches!";
        msg.Show();
        msgBg.Show();
        
        await ToSignal(GetTree().CreateTimer(1), "timeout");
        GetNode<TextureButton>("StartButton").Show();
    }
    
    void OnMessageTimerTimeout()
    {
        GetNode<Label>("Message").Hide();
        GetNode<TileMap>("TileMap").Hide();
    }
    
    void OnStartButtonPressed()
    {
        GetNode<TextureButton>("StartButton").Hide();
        Game.PlayUIClick();
        EmitSignal(nameof(StartGamePressed));
    }
}
