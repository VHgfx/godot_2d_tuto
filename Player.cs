using Godot;
using System;

public partial class Player : Area2D
{
	[Signal]
	public delegate void HitEventHandler();
	
	private void OnBodyEntered(Node2D body)
	{
		Hide();
		EmitSignal(SignalName.Hit);
		GetNode<CollisionShape2D>("CollisionShape2D").SetDeferred(CollisionShape2D.PropertyName.Disabled, true);
	}
	
	public void Start(Vector2 position)
	{
		Position = position;
		Show();
		GetNode<CollisionShape2D>("CollisionShape2D").Disabled = false;
	}
	
	// _Ready : appelé dès qu'un node entre en scène
	public override void _Ready()
	{
		ScreenSize = GetViewportRect().Size;
		// StartPosition = new Vector2(ScreenSize.X * 0.5f, ScreenSize.Y * 0.8f);
		
		Hide(); // Cache le joueur au lancement;
		
		// Start(StartPosition);
	}
	
	// _Process : Appelé à chaque frame
	// double delta : temps passé depuis la dernière frame, permet d'avoir un mouvement indépendant du framerate
	public override void _Process(double delta)
	{
		var velocity = Vector2.Zero; // Vecteur mouvement du joueur
		// Equivalent à new Vector(0,0);
		
		if (Input.IsActionPressed("move_right"))
		{
			velocity.X += 1;
		}
		
		if (Input.IsActionPressed("move_left"))
		{
			velocity.X -= 1;
		}
		
		if (Input.IsActionPressed("move_up"))
		{
			velocity.Y -= 1;
		}
		
		if (Input.IsActionPressed("move_down"))
		{
			velocity.Y += 1;
		}
		
		var animatedSprite2D = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		
		if (velocity.Length() > 0) 
		{
			velocity = velocity.Normalized() * Speed; // Normalize pour éviter mouvement diagonal + rapide
			animatedSprite2D.Play();
		} 
		else 
		{
			animatedSprite2D.Stop();
		}
		
		Position += velocity * (float)delta;
		
		// Clamp() : Restreindre à une certaine portée
		// Clamp(value, min, max)
		Position = new Vector2(
			x: Mathf.Clamp(Position.X, 0, ScreenSize.X),
			y: Mathf.Clamp(Position.Y, 0, ScreenSize.Y)
		);
		
		if (velocity.X != 0)
		{
			animatedSprite2D.Animation = "walk"; // Suivre la casse du nom de l'animation
			animatedSprite2D.FlipV = false;
			animatedSprite2D.FlipH = velocity.X < 0; // Boolean conditioning
		}
		else if (velocity.Y != 0)
		{
			animatedSprite2D.Animation = "up";
			animatedSprite2D.FlipV = velocity.Y > 0; // Boolean conditioning
		}
	}
	
	[Export]
	public int Speed { get; set; } = 400; // Speed of player
	
	public Vector2 ScreenSize; // Size of the game window
	// public Vector2 StartPosition;
}
