using Godot;
using System;

public partial class player : CharacterBody2D
{
	public AttributeModifierPack speed = new AttributeModifierPack(100);
	public AttributeModifierPack acceleration = new AttributeModifierPack(100);
	public AttributeModifierPack friction = new AttributeModifierPack(100);
	public AttributeModifierPack jumpVelocity = new AttributeModifierPack(-300);
	public AttributeModifierPack cutJumpFactor = new AttributeModifierPack(2);
	public AttributeModifierPack bicycleFactor = new AttributeModifierPack(5);
	private AnimatedSprite2D animatedSprite2D = null;
	private int startedHoldingRight = 0;
	private int horizontalMovement = 0;
	private bool isFacingRight = true;
	private Vector2 velocity;

	// Get the gravity from the project settings to be synced with RigidBody nodes.
	public float gravity = ProjectSettings.GetSetting("physics/2d/default_gravity").AsSingle();

	public override void _Ready()
	{
		animatedSprite2D = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
	}

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
		updateAnimations();
		bicycle();
        Velocity = velocity;
        MoveAndSlide();
    }


    private void handleDirection()
    {
        // Get the input direction and handle the movement/deceleration.
        // As good practice, you should replace UI actions with custom gameplay actions.
        // This piece of code is responsible for null-cancelling movement.
        if (startedHoldingRight >= 0 && Input.IsActionPressed("ui_right"))
        {
            startedHoldingRight = 1;
            horizontalMovement = Input.IsActionPressed("ui_left") ? -1 : 1;
        }
        else if (startedHoldingRight <= 0 && Input.IsActionPressed("ui_left"))
        {
            startedHoldingRight = -1;
            horizontalMovement = Input.IsActionPressed("ui_right") ? 1 : -1;
        }
        else
        {
            horizontalMovement = 0;
            startedHoldingRight = 0;
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
			if (Input.IsActionJustPressed("ui_accept") && Velocity.Y < jumpVelocity.getFinalValue() / cutJumpFactor.getFinalValue()) {
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

	public void updateAnimations() {
		if (horizontalMovement != 0) {
			animatedSprite2D.Play("run");
			animatedSprite2D.FlipH = horizontalMovement < 0;
		} else {
			animatedSprite2D.Play("idle");
		}
	}

	public void bicycle() {
		/*
			Bicycling is a form of intentional movement designed
			to prolong the player's stupid and sad little existance.
			By moving constantly left and right whilst descending,
			the player will have their descent slowed down by defined
			margins. More of this mechanic will be improved further
			done the line.
		*/
		if (isFacingRight && horizontalMovement < 0 || !isFacingRight && horizontalMovement > 0) {
			isFacingRight = !isFacingRight;
			if (velocity.Y > 0) {
				velocity.Y /= bicycleFactor.getFinalValue();
			}
		}
	}
}
