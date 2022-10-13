using Godot;
using System;

public class Player : KinematicBody2D
{
	[Export]
	public int HopDistance = 120;
	
	[Export]
	public float HopSpeed = 1.0f;
    
    [Export]
    public int ScreenBoundaryPadding = 64;
    	
    private AnimatedSprite sprite;
    
	private Vector2 screenSize;
	private Vector2 directionToMouse = new Vector2();
    
    private Vector2[] hopPositions = new Vector2[3];
    
    private bool isHopping = false;
    private bool mousePressed = false;
    private float hopTimer = 0.0f; 
    private float hopRatio = 0.0f;
    private bool firstZoneFinished = false;
    
    private Timer hopTimerNode;
    private bool isHopAvailable = true;
	
	public override void _Ready()
	{
		screenSize = GetViewport().Size;
        sprite = GetNode<AnimatedSprite>("AnimatedSprite");
        
        hopTimerNode = GetNode<Timer>("Cooldown");
		if(hopTimerNode != null)
		{
			hopTimerNode.Connect("timeout", this, nameof(OnCooldownExpire));
		}
        
        sprite.Play("idle");
	}
	
	public override void _Draw()
	{
        // Vector2 vApex = GetHopApexPosition() - Position;
        // Vector2 vDestination = GetHopDestination() - Position;
        
		// DrawLine(new Vector2(0.0f, 0.0f), vApex, Colors.Red);
		// DrawLine(vApex, vDestination, Colors.Green);
	}
	
	public override void _Process(float delta)
	{
		directionToMouse = GetViewport().GetMousePosition() - Position;
		directionToMouse.Normalise();
		
        if(Input.IsMouseButtonPressed((int)Godot.ButtonList.Left))
        {
            if(!isHopping && !mousePressed && isHopAvailable)
            {
                StartHop();
            }
            
            mousePressed = true;
        }
        else
        {
            mousePressed = false;
        }
        
        //Update();
	}
    
    public override void _PhysicsProcess(float delta)
    {
        ProcessHopping(delta);
    }
    
    void ConstrainPositionToScreen(ref Vector2 vPos)
    {
        vPos.x = Mathf.Clamp(vPos.x, ScreenBoundaryPadding, screenSize.x - ScreenBoundaryPadding);
        vPos.y = Mathf.Clamp(vPos.y, ScreenBoundaryPadding, screenSize.y - ScreenBoundaryPadding);
    }
    
    private void StartHop()
    {
        hopPositions[0] = Position;
        hopPositions[1] = GetHopApexPosition();
        hopPositions[2] = GetHopDestination();
        
        ConstrainPositionToScreen(ref hopPositions[1]);
        ConstrainPositionToScreen(ref hopPositions[2]);
        
        isHopping = true;
        hopTimer = 0.0f;
        hopRatio = 0.0f;
        firstZoneFinished = false;
        
        sprite.Play("jump");
        
        hopTimerNode.Start();
        isHopAvailable = false;
    }
    
    private void ProcessHopping(float delta)
    {
        if(isHopping)
        {
            float amountToAdd = delta * HopSpeed;
            hopTimer += amountToAdd;
            hopRatio += amountToAdd * 2;
            
            bool timerHalfwayDone = (hopTimer >= 0.5f);
            
            if(timerHalfwayDone && !firstZoneFinished)
            {
                firstZoneFinished = true;
                hopRatio = 0.0f;
            }
            
            int iFrom = (timerHalfwayDone ? 1 : 0);
            int iTo = iFrom + 1;
            
            Position = hopPositions[iFrom].LinearInterpolate(hopPositions[iTo], hopRatio);
            if(hopTimer >= 1.0f)
            {
                Position = hopPositions[iTo];
                isHopping = false;
                sprite.Play("exhausted");
            }
        }
    }
    
    Vector2 GetHopApexPosition()
    {
        float x = directionToMouse.x;
        float y = directionToMouse.y;
        
        float heightRatio = 1.0f - Mathf.Abs(directionToMouse.y);
        float theta = Mathf.Deg2Rad((directionToMouse.x > 0.0f ? -30.0f : 30.0f) * heightRatio);

        float cs = Mathf.Cos(theta);
        float sn = Mathf.Sin(theta);
        
        float px = x * cs - y * sn; 
        float py = x * sn + y * cs;
        
        Vector2 vDir = new Vector2(px, py);
        vDir.Normalise();
        
        return Position + (vDir * (HopDistance / 2));
    }
    
    Vector2 GetHopDestination()
    {
        return Position + (directionToMouse * HopDistance);
    }
    
    private void OnCooldownExpire()
    {
        isHopAvailable = true;
        sprite.Play("idle");
    }
}
