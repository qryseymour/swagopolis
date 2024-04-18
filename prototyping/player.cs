using Godot;
using System;

public partial class player : CharacterBody2D
{
	public AttributeModifierPack speed = new AttributeModifierPack(100);
	public AttributeModifierPack acceleration = new AttributeModifierPack(100);
	public AttributeModifierPack friction = new AttributeModifierPack(100);
	public AttributeModifierPack jumpVelocity = new AttributeModifierPack(-300);
	public AttributeModifierPack cutJumpFactor = new AttributeModifierPack(2);
	private int isHoldingDirection = 0;
	private int horizontalMovement = 0;
	private Vector2 velocity;

	// Get the gravity from the project settings to be synced with RigidBody nodes.
	public float gravity = ProjectSettings.GetSetting("physics/2d/default_gravity").AsSingle();

	public override void _PhysicsProcess(double delta)
	{
		velocity = Velocity;

		applyGravity(gravity, delta);

		handleJump();

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
		applyAcceleration(delta, speed.getFinalValue(), acceleration.getFinalValue());
		applyFriction(delta, speed.getFinalValue(), friction.getFinalValue());

		Velocity = velocity;
		MoveAndSlide();
	}

	public void applyGravity(float gravityPow = 98, double delta = 1) {
		// Add the gravity.
		if (!IsOnFloor()) {
			velocity.Y += gravityPow * (float)delta;
		}
	}

	public void handleJump() {
		// Handles Jumping, and the change in velocity when letting go of jump.
		if (IsOnFloor()){
			if (Input.IsActionJustPressed("ui_accept")) {
				velocity.Y = jumpVelocity.getFinalValue();
			}
		}
		else {
			if (Input.IsActionJustReleased("ui_accept") && Velocity.Y < jumpVelocity.getFinalValue() / cutJumpFactor.getFinalValue()) {
				velocity.Y = jumpVelocity.getFinalValue() / cutJumpFactor.getFinalValue();
			}
		}
	}

	public void applyAcceleration(double delta, float speedPow = 100, float accelerationPow = 100) {
		if (horizontalMovement != 0) {
			/*
				Distance from acceleration to speed corelates with 
				the effectiveness of delta. This is intended to allow 
				smaller acceleration numbers closer to speed to not
				have any noticable start up, and vice-versa.				
			*/  
			velocity.X = Mathf.MoveToward(Velocity.X, horizontalMovement * speedPow, 
			accelerationPow * Mathf.Pow((float)delta, Mathf.Clamp(1 - (accelerationPow / speedPow), 0, 1)));
		} 
	}

	public void applyFriction(double delta, float speedPow = 100, float frictionPow = 100) {
		if (horizontalMovement == 0) {
			/*
				The same logic as mentioned with acceleration is
				applied here. Friction closer to speed is almost
				negligant, and friction away from speed is closer
				to slipperyness.
			*/
			velocity.X = Mathf.MoveToward(Velocity.X, 0, 
			frictionPow * Mathf.Pow((float)delta, Mathf.Clamp(1 - (frictionPow / speedPow), 0, 1)));
		}
	}
}
