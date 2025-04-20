using Godot;
using System;

public partial class Mob : RigidBody2D
{
	public override void _Ready()
	{
		var animatedSprite2D = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		string[] mobTypes = animatedSprite2D.SpriteFrames.GetAnimationNames(); // Récuoération des noms des animations en array
		animatedSprite2D.Play(mobTypes[GD.Randi() % mobTypes.Length]); // GD.Randi() % n : Sélection d'un int random entre 0 et n-1
	}
	
	private void OnVisibleOnScreenNotifier2DScreenExited()
	{
		QueueFree(); // Fonction qui supprime le node à la fin de la frame
	}
}
