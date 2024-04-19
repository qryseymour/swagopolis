using Godot;
using System;

public partial class player : CharacterBody2D
{
	public AttributeModifierPack speed = new AttributeModifierPack(100);
	public AttributeModifierPack acceleration = new AttributeModifierPack(100);
	public AttributeModifierPack friction = new AttributeModifierPack(100);
	public AttributeModifierPack jumpVelocity = new AttributeModifierPack(-300);
	public AttributeModifierPack cutJumpFactor = new AttributeModifierPack(2);
	private int startedHoldingDirection = 0;
	private int horizontalMovement = 0;
	private Vector2 velocity;

	// Get the gravity from the project settings to be synced with RigidBody nodes.
	public float gravity = ProjectSettings.GetSetting("physics/2d/default_gravity").AsSingle();

	public override void _PhysicsProcess(double delta)
    {
        controlCharacter(delta);
    }

    private void controlCharacter(double delta)
    {
        velocity = Velocity;
        applyGravity(delta, gravity);
        handleJump();
        handleDirection();
        applyAcceleration(delta, horizontalMovement, speed.getFinalValue(), acceleration.getFinalValue());
        if (horizontalMovement == 0)
        {
            applyFriction(delta, speed.getFinalValue(), friction.getFinalValue());
        }
        Velocity = velocity;
        MoveAndSlide();
    }


    private void handleDirection()
    {
        // Get the input direction and handle the movement/deceleration.
        // As good practice, you should replace UI actions with custom gameplay actions.
        // This piece of code is responsible for null-cancelling movement.
        if (startedHoldingDirection >= 0 && Input.IsActionPressed("ui_right"))
        {
            startedHoldingDirection = 1;
            horizontalMovement = Input.IsActionPressed("ui_left") ? -1 : 1;
        }
        else if (startedHoldingDirection <= 0 && Input.IsActionPressed("ui_left"))
        {
            startedHoldingDirection = -1;
            horizontalMovement = Input.IsActionPressed("ui_right") ? 1 : -1;
        }
        else
        {
            horizontalMovement = 0;
            startedHoldingDirection = 0;
        }
    }

    public void applyGravity(double delta, float gravityPow = 98) {
		// Add the gravity.
		if (!IsOnFloor()) {
			velocity.Y += gravityPow * (float)delta;
		}
	}

	public void handleJump() {
		// Handles Jumping, and the change in velocity when letting go of jump.
		if (IsOnFloor()){
			if (Input.IsActionJustPressed("ui_accept")) {
				applyJump(jumpVelocity.getFinalValue());
			}
		}
		else {
			if (Input.IsActionJustReleased("ui_accept") && Velocity.Y < jumpVelocity.getFinalValue() / cutJumpFactor.getFinalValue()) {
				applyJump(jumpVelocity.getFinalValue() / cutJumpFactor.getFinalValue());
			}
		}
	}

	public void applyJump(float jumpPow = -300) {
		velocity.Y = jumpPow;
	}

	public void applyAcceleration(double delta, int movementDirection, float speedPow = 100, float accelerationPow = 100) {
		if (movementDirection != 0) {
			/*
				Distance from acceleration to speed corelates with 
				the effectiveness of delta. This is intended to allow 
				smaller acceleration numbers closer to speed to not
				have any noticable start up, and vice-versa.				
			*/  
			velocity.X = Mathf.MoveToward(Velocity.X, movementDirection * speedPow, 
			accelerationPow * Mathf.Pow((float)delta, Mathf.Clamp(1 - (accelerationPow / speedPow), 0, 1)));
		} 
	}

	public void applyFriction(double delta, float speedPow = 100, float frictionPow = 100) {
		/*
			The same logic as mentioned with acceleration is
			applied here. Friction closer to speed is almost
			negligant, and friction away from speed is closer
			to slipperyness. However, no check is made if
			the horizontal direction is valid, as only acceleration
			needs to know that to know where it needs to go.
		*/
		velocity.X = Mathf.MoveToward(Velocity.X, 0, 
		frictionPow * Mathf.Pow((float)delta, Mathf.Clamp(1 - (frictionPow / speedPow), 0, 1)));
	}
}
