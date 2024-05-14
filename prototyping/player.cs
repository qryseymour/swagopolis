using Godot;
using System;

public partial class player : entity
{
	// Player variable shadowing
	public player() {
		base.canJumpMidair = true;
	}


	// Non-Important Attributes
	public float additionalGravityFactor = 2;
	public float bicycleFactor = 5;

	public override void _Ready()
	{
		// Basic code to override ready with the base class implementation, followed by additional code.
		base._Ready();
	}

    protected override void controlCharacterPhysics(double delta)
    {
		base.controlCharacterPhysics(delta);
        bicycle();
    }

    protected override void handleGravity(double delta)
    {
        if (Input.IsActionPressed("ui_down"))
        {
            applyGravity(delta, entityMovementData.GravityVelocity.getFinalValue() * additionalGravityFactor);
        }
        else
        {
            applyGravity(delta, entityMovementData.GravityVelocity.getFinalValue());
        }
    }

	protected void bicycle() {
		/*
			Bicycling is a form of intentional movement designed
			to prolong the player's stupid and sad little existance.
			By moving constantly left and right whilst descending,
			the player will have their descent slowed down by defined
			margins. More of this mechanic will be improved further
			down the line.
		*/
		if (isFacingRight && horizontalMovement < 0 || !isFacingRight && horizontalMovement > 0) {
			isFacingRight = !isFacingRight;
			if (velocity.Y > 0) {
				velocity.Y /= bicycleFactor;
			}
		}
	}
}
