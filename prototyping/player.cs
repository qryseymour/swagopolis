using Godot;
using System;

public partial class player : CharacterBody2D
{
	public AttributeModifierPack speed = new AttributeModifierPack(100);
	public AttributeModifierPack acceleration = new AttributeModifierPack(50);
	public AttributeModifierPack jumpVelocity = new AttributeModifierPack(-300);
	private int isHoldingDirection = 0;
	private int horizontalMovement = 0;

	// Get the gravity from the project settings to be synced with RigidBody nodes.
	public float gravity = ProjectSettings.GetSetting("physics/2d/default_gravity").AsSingle();

	public override void _PhysicsProcess(double delta)
	{
		Vector2 velocity = Velocity;

		// Add the gravity.
		if (!IsOnFloor())
			velocity.Y += gravity * (float)delta;

		// Handle Jump.
		if (Input.IsActionJustPressed("ui_accept") && IsOnFloor())
			velocity.Y = jumpVelocity.getFinalValue();

		// Get the input direction and handle the movement/deceleration.
		// As good practice, you should replace UI actions with custom gameplay actions.

		// This piece of code is responsible for null-cancelling movement.
		if (isHoldingDirection >= 0 && Input.IsActionPressed("ui_right"))
		{
			isHoldingDirection = 1;
			horizontalMovement = 1;
			if (Input.IsActionPressed("ui_left"))
			{
				horizontalMovement *= -1;
			}
		}
		else if (isHoldingDirection <= 0 && Input.IsActionPressed("ui_left"))
		{
			isHoldingDirection = -1;
			horizontalMovement = -1;
			if (Input.IsActionPressed("ui_right"))
			{
				horizontalMovement *= -1;
			}
		} else {
			horizontalMovement = 0;
			isHoldingDirection = 0;
		}
		
		if (horizontalMovement != 0) {
			velocity.X = Mathf.MoveToward(Velocity.X, horizontalMovement * speed.getFinalValue(), 
			acceleration.getFinalValue() * Mathf.Pow((float)delta, Mathf.Clamp(1 - (acceleration.getFinalValue() / speed.getFinalValue()), 0, 1)));
		} else {
			velocity.X = Mathf.MoveToward(Velocity.X, 0, speed.getFinalValue());
		}

		Velocity = velocity;
		MoveAndSlide();
	}
}
