using Godot;
using System;

public partial class characterEntity : CharacterBody2D {
	// Important Attributes
	[Export]
	public entityMovementData entityMovementData;
	public float jumpCount = 0;
    public bool canJumpMidair = false;



	// Non-Important Attributes
	public attributeModifierPack cutJumpFactor = new attributeModifierPack(0, 0, 0.5f);
	public attributeModifierPack cutJumpVelocity = null;
    public int horizontalMovement = 0;
	public bool isJumping = false;
	protected int startedHoldingRight = 0;
	protected bool wasOnFloor = false;
	protected bool justLeftLedge = false;
	protected Vector2 velocity;
    public Vector2 startingPosition;
	public AnimatedSprite2D animatedSprite2D = null;

	public override void _Ready()
	{
        // Child node initations
		animatedSprite2D = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
        // Signal Event Connections
        // Other stuff
		cutJumpVelocity = entityMovementData.JumpVelocity + cutJumpFactor;
        startingPosition = GlobalPosition;
	}

	public override void _PhysicsProcess(double delta)
    {
        velocity = Velocity;
        controlCharacterPhysics(delta);
        Velocity = velocity;
        MoveAndSlide();
    }

    protected virtual void controlCharacterPhysics(double delta)
    {
        handleGravity(delta);
        handleHorizontalDirection();
        applyAcceleration(delta, horizontalMovement, entityMovementData.Speed.getFinalValue(), entityMovementData.Acceleration.getFinalValue());
        if (horizontalMovement == 0)
        {
            applyFriction(delta, entityMovementData.Speed.getFinalValue(), entityMovementData.Friction.getFinalValue());
        }
		/*
			This line acts as a divider between all internal
			factions that modify just X (as above), and can
			modify X AND Y (as below). This avoids any conflicts
			with having set velocities overriding one another.
			Although better coding practices can resolve this.
		*/
        restoreJumps();
        handleJump();
        updateAnimations();
    }

    protected virtual void handleGravity(double delta)
    {
        applyGravity(delta, entityMovementData.GravityVelocity.getFinalValue());
    }

    protected virtual void handleHorizontalDirection()
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
		// Add the gravity if not on the floor.
		if (!IsOnFloor()) {
			velocity.Y += gravityPow * (float)delta;
		}
	}

	protected virtual void handleJump()
    {
        // Handles Jumping, and the change in velocity when letting go of jump.
        if (Input.IsActionJustPressed("ui_accept"))
        {
            controlJumps();
        }
        if (!IsOnFloor())
        {
            // Good form to put more taxing calculations/checks after the &&'s.
            if (Input.IsActionJustReleased("ui_accept") && Velocity.Y < cutJumpVelocity.getFinalValue())
            {
                applyJump(cutJumpVelocity.getFinalValue());
            }
        }
    }

    protected virtual void controlJumps()
    {
        /* 
            If the player has less than 1 possible jump stored,
            they only make a jump representing the fraction
            of that number.
        */
        if (jumpCount >= 1)
        {
            applyJump(entityMovementData.JumpVelocity.getFinalValue());
            jumpCount--;
        }
        else if (jumpCount > 0)
        {
            applyJump(entityMovementData.JumpVelocity.getFinalValue() * jumpCount);
            jumpCount = 0;
        }
    }

    protected virtual void restoreJumps()
    {
        /*
            If the player is on the floor, they are no longer
            considered jumping and they restore their jump count.
        */
        if (IsOnFloor())
        {
            jumpCount = entityMovementData.AvailableJumps.getFinalValue();
            isJumping = false;
        }
    }

    public virtual void applyJump(float jumpPow = -300) {
		velocity.Y = jumpPow;
        isJumping = true;
	}

	public virtual void applyAcceleration(double delta, int movementDirection, float speedPow = 100, float accelerationPow = 100) {
		if (movementDirection != 0) { 
			velocity.X = Mathf.MoveToward(Velocity.X, movementDirection * speedPow, 
			accelerationPow * 60 * (float)delta);
		} 
	}

	public virtual void applyFriction(double delta, float speedPow = 100, float frictionPow = 100) {
		velocity.X = Mathf.MoveToward(Velocity.X, 0, 
		frictionPow * 60 * (float)delta);
	}

	protected virtual void updateAnimations() {
		if (horizontalMovement != 0) {
			animatedSprite2D.Play("run");
			animatedSprite2D.FlipH = horizontalMovement < 0;
		} else {
			animatedSprite2D.Play("idle");
		}
	}
}