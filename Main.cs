using Godot;
using System;

public partial class Main : Node
{
	[Export]
	public PackedScene MobScene { get; set; }
	
	private int _score;

	public void NewGame()
	{
		_score = 0;
		
		var hud = GetNode<HUD>("HUD");
		hud.UpdateScore(_score);
		hud.ShowMessage("Get Ready !");
		
		var player = GetNode<Player>("Player");
		var startPosition = GetNode<Marker2D>("StartPosition");
		player.Start(startPosition.Position);
		
		GetTree().CallGroup("mobs", Node.MethodName.QueueFree);
		GetNode<Timer>("StartTimer").Start();
	}
	
	public void GameOver()
	{
		GetNode<Timer>("MobTimer").Stop();
		GetNode<Timer>("ScoreTimer").Stop();
		GetNode<HUD>("HUD").ShowGameOver();
	}
	
	private void OnScoreTimerTimeout()
	{
		_score++;
		GetNode<HUD>("HUD").UpdateScore(_score);
	}
	
	private void OnStartTimerTimeout()
	{
		GetNode<Timer>("MobTimer").Start();
		GetNode<Timer>("ScoreTimer").Start();
	}
	
	private void OnMobTimerTimeout()
	{
		// Instanciation d'une scène Mob
		Mob mob = MobScene.Instantiate<Mob>();
		
		// Random location sur Path2D
		var mobSpawnLocation = GetNode<PathFollow2D>("MobPath/MobSpawnLocation");
		mobSpawnLocation.ProgressRatio = GD.Randf();
		
		// Direction du mob perpendiculaire à son path
		float direction = mobSpawnLocation.Rotation + Mathf.Pi / 2;
		
		// Localisation mob random
		mob.Position = mobSpawnLocation.Position;
		
		// + Random à la direction
		direction += (float)GD.RandRange(-Mathf.Pi / 4, Mathf.Pi / 4);
		mob.Rotation = direction;
		
		// Choix vélocité
		var velocity = new Vector2((float)GD.RandRange(150.0, 250.0), 0);
		mob.LinearVelocity = velocity.Rotated(direction);
		
		// Spawn mob dans la main scene
		AddChild(mob);
	}
	
	public override void _Ready()
	{
		// NewGame();
	}
}
