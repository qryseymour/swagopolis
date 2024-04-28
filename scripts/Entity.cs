using Godot;
using System;

public partial class entity : CharacterBody2D {
    // Important Attributes
	public AttributeModifierPack speed = new AttributeModifierPack(100);
	public AttributeModifierPack acceleration = new AttributeModifierPack(100);
	public AttributeModifierPack friction = new AttributeModifierPack(100);
	public AttributeModifierPack jumpVelocity = new AttributeModifierPack(-300);
	public AttributeModifierPack availableJumps = new AttributeModifierPack(1);
	// Get the gravity from the project settings to be synced with RigidBody nodes.
	public AttributeModifierPack gravityVelocity = new AttributeModifierPack(ProjectSettings.GetSetting("physics/2d/default_gravity").AsSingle());
	public short jumpCount = 0;
    public bool canJumpMidair = false;



	// Non-Important Attributes
	public AttributeModifierPack cutJumpFactor = new AttributeModifierPack(0, 0, 0.5f);
	public AttributeModifierPack cutJumpVelocity = null;
	public AnimatedSprite2D animatedSprite2D = null;
	public Timer coyoteJumpTimer = null;
	protected int startedHoldingRight = 0;
	protected int horizontalMovement = 0;
	protected bool isFacingRight = true;
	protected bool wasOnFloor = false;
	protected bool justLeftLedge = false;
	protected Vector2 velocity;

	public override void _Ready()
	{
		animatedSprite2D = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		coyoteJumpTimer = GetNode<Timer>("CoyoteJumpTimer");
		cutJumpVelocity = jumpVelocity + cutJumpFactor;
	}

	public override void _PhysicsProcess(double delta)
    {
        velocity = Velocity;
        controlCharacter(delta);
        Velocity = velocity;
        MoveAndSlide();
    }

    protected virtual void controlCharacter(double delta)
    {
        handleGravity(delta);
        handleJump();
        handleDirection();
        applyAcceleration(delta, horizontalMovement, speed.getFinalValue(), acceleration.getFinalValue());
        if (horizontalMovement == 0)
        {
            applyFriction(delta, speed.getFinalValue(), friction.getFinalValue());
        }
        updateAnimations();
    }

    protected virtual void handleGravity(double delta)
    {
        applyGravity(delta, gravityVelocity.getFinalValue());
    }

    protected virtual void handleDirection()
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

    public virtual void applyGravity(double delta, float gravityPow = 98) {
		// Add the gravity.
		if (!IsOnFloor()) {
			velocity.Y += gravityPow * (float)delta;
		}
	}

	protected virtual void handleJump() {
		// Handles Jumping, and the change in velocity when letting go of jump.
		if (IsOnFloor()){
			jumpCount = (short)Math.Floor(availableJumps.getFinalValue());
		}
		if (Input.IsActionJustPressed("ui_accept") && jumpCount > 0) {
			jumpCount--;
			applyJump(jumpVelocity.getFinalValue());
		}
		if (!IsOnFloor()) {
			// Good form to put more taxing calculations/checks after the &&'s.
			if (Input.IsActionJustReleased("ui_accept") && Velocity.Y < cutJumpVelocity.getFinalValue()) {
				applyJump(cutJumpVelocity.getFinalValue());
			}
		}
	}

	public virtual void applyJump(float jumpPow = -300) {
		velocity.Y = jumpPow;
	}

	public virtual void applyAcceleration(double delta, int movementDirection, float speedPow = 100, float accelerationPow = 100) {
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

	public virtual void applyFriction(double delta, float speedPow = 100, float frictionPow = 100) {
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

	protected virtual void updateAnimations() {
		if (horizontalMovement != 0) {
			animatedSprite2D.Play("run");
			animatedSprite2D.FlipH = horizontalMovement < 0;
		} else {
			animatedSprite2D.Play("idle");
		}
	}

	protected virtual void handleCoyoteJump() {
		wasOnFloor = IsOnFloor();
		justLeftLedge = wasOnFloor && !IsOnFloor();
		if (justLeftLedge) {
			coyoteJumpTimer.Start();
		}
	}
}