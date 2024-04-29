using Godot;
using System;

public partial class player : entity
{
	// Player variable shadowing
	public player() {
		base.speed = new AttributeModifierPack(135);
		base.acceleration = new AttributeModifierPack(131.25f);
		base.friction = new AttributeModifierPack(87.5f);
		base.jumpVelocity = new AttributeModifierPack(-300);
		base.availableJumps = new AttributeModifierPack(2f);
		base.canJumpMidair = true;
	}


	// Non-Important Attributes
	public AttributeModifierPack additionalGravityFactor = new AttributeModifierPack(0, 0, 2f);
	public AttributeModifierPack additionalGravityVelocity = null;
	public AttributeModifierPack bicycleFactor = new AttributeModifierPack(5);

	public override void _Ready()
	{
		// Basic code to override ready with the base class implementation, followed by additional code.
		base._Ready();
		additionalGravityVelocity = gravityVelocity + additionalGravityFactor;
	}

    protected override void controlCharacter(double delta)
    {
		base.controlCharacter(delta);
        bicycle();
    }

    protected override void handleGravity(double delta)
    {
        if (Input.IsActionPressed("ui_down"))
        {
            applyGravity(delta, additionalGravityVelocity.getFinalValue());
        }
        else
        {
            applyGravity(delta, gravityVelocity.getFinalValue());
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
				velocity.Y /= bicycleFactor.getFinalValue();
				GD.Print("Did a bycicle");
			}
		}
	}
}
