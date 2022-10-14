using Godot;
using System;

public class Game : Node
{
    private static Game instance;
    
    [Export]
    public PackedScene WitchObject;
    
    [Export]
    public int WitchSpeedMin = 50;
    [Export]
    public int WitchSpeedMax = 100;
    
    private int score;
    private int bestScoreSoFar;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        GD.Randomize();
        instance = this;
    }
    
    private void OnEnemyCollideWithPlayer()
    {
        SetGameOver();
    }
    
    private void OnScoreTimerTimeout()
    {
        ++score;
        
        var hud = GetNode<HUD>("HUD");
        hud.UpdateScore(score);
    }
    
    private void OnWitchTimerTimeout()
    {
        int difficultyLevel = Mathf.FloorToInt(score / 30);
        
        int iNumWitchesToSpawn = 1 + Math.Max(0, difficultyLevel - 1);
        float maxSpeedMod = 0.8f + (0.2f * difficultyLevel);
        
        do
        {
            SpawnWitch(maxSpeedMod);
        } while(--iNumWitchesToSpawn > 0);
    }
    
    private void SpawnWitch(float maxSpeedMod = 1.0f)
    {
        var witch = (Witch)WitchObject.Instance();
        var spawnLoc = GetNode<PathFollow2D>("SpawnPath/PathFollow2D");
        spawnLoc.Offset = GD.Randi();
        
        float dir = spawnLoc.Rotation + (Mathf.Pi/2);
        dir += (float)GD.RandRange(-Mathf.Pi / 4, Mathf.Pi / 4);
        
        
        var velocity = new Vector2((float)GD.RandRange(WitchSpeedMin, WitchSpeedMax * maxSpeedMod), 0);
        witch.LinearVelocity = velocity.Rotated(dir);
        
        var screenSize = GetViewport().Size;
        if(spawnLoc.Position.y > screenSize.y - 10)
        {
            witch.Position = spawnLoc.Position - (witch.LinearVelocity * 2);
        }
        else
        {
            witch.Position = spawnLoc.Position;
        }
        
        GetNode<YSort>("YSort").AddChild(witch);
    }
    
    private void OnGameTimerTimeout()
    {
        GetNode<Timer>("WitchSpawnTimer").Start();
        GetNode<Timer>("ScoreIncrementTimer").Start();
    }
    
    private void SetGameStart()
    {
        GetTree().CallGroup("Witches", "queue_free");
        
        score = 0;
        
        var player = GetNode<Player>("YSort/PlayerFrog");
        var startPos = GetNode<Position2D>("PlayerSpawnPosition");
        
        player.Start(startPos.Position);
        
        GetNode<Timer>("GameDelayTimer").Start();
        
        var hud = GetNode<HUD>("HUD");
        hud.UpdateScore(score);
        hud.ShowMessage("Click to hop!");
    }
    
    private void SetGameOver()
    {
        GetNode<Timer>("WitchSpawnTimer").Stop();
        GetNode<Timer>("ScoreIncrementTimer").Stop();
                
        var hud = GetNode<HUD>("HUD");
        hud.ShowGameOver();
        
        if(score > bestScoreSoFar)
        {
            bestScoreSoFar = score;
            hud.ShowMessage($"New best: {bestScoreSoFar}!");
        }
        else
        {
            hud.ShowMessage($"Best score: {bestScoreSoFar}.");
        }
    }
    
    public static void PlayUIClick()
    {
        instance.GetNode<AudioStreamPlayer>("Sfx_UIClick").Play();
    }
    public static void PlayFrogHop()
    {
        instance.GetNode<AudioStreamPlayer>("Sfx_FrogHop").Play();
    }
    public static void PlayFrogLand()
    {
        instance.GetNode<AudioStreamPlayer>("Sfx_FrogLand").Play();
    }
    public static void PlayFrogPickup()
    {
        instance.GetNode<AudioStreamPlayer>("Sfx_FrogPickup").Play();
    }
}
