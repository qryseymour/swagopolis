using Godot;
using System;

public partial class player : characterEntity
{
	// Player variable shadowing
	public player() {
		canJumpMidair = true;
	}



	// Non-Important Attributes
	public float additionalGravityFactor = 2;
	public float bicycleFactor = 5;



	// Fundamental Variables
	protected bool justFacedRight = true;
	public Timer coyoteJumpTimer = null;

	public override void _Ready()
	{
		// Basic code to override ready with the base class implementation, followed by additional code.
		base._Ready();
		coyoteJumpTimer = GetNode<Timer>("CoyoteJumpTimer");
		/* 
			When the coyote jump timer expires, if the entity is
			unable to jump midair, is not jumping, and not on the
			floor (doing the dinosaur) (doing ur mom), then the
			entity loses one possible jump they can make.
		*/
		coyoteJumpTimer.Timeout += () =>
		{
			if (!canJumpMidair && !isJumping && !IsOnFloor())
			{
				jumpCount = jumpCount < 1 ? 0 : jumpCount - 1;
			}
		};
	}

    protected override void controlCharacterPhysics(double delta)
    {
		base.controlCharacterPhysics(delta);
		handleCoyoteJump();
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
	protected override void restoreJumps()
    {
        if (IsOnFloor() || coyoteJumpTimer.TimeLeft > 0)
        {
            jumpCount = entityMovementData.AvailableJumps.getFinalValue();
            isJumping = false;
        }
    }

	protected virtual void handleCoyoteJump() {
		/*
			This is the line of code that handles when a player
			just left the ledge, then after it the supporting
			variable to keep track of when the player was on
			the floor. I don't know why I'm making dumb comments
			like these.
		*/
		justLeftLedge = wasOnFloor && !IsOnFloor() && baseVelocity.Y >= 0;
		if (justLeftLedge) {
			coyoteJumpTimer.Start();
		}
		wasOnFloor = IsOnFloor();
	}

	protected override void controlJumps()
    {
		/*
			This is the implementation for wall-jumps; When
			a wall-jump is made, its by checking the conditions,
			applying the horizontal force, then simply incrementing
			the jump count, before it then processes a regular
			jump. This... may be clever?
		*/
		if (!IsOnFloor() && IsOnWall())
		{
			baseVelocity.X = GetWallNormal().X * entityMovementData.Speed.getFinalValue();
			jumpCount++;
		}
		base.controlJumps();
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
		if (justFacedRight && horizontalMovement < 0 || !justFacedRight && horizontalMovement > 0) {
			justFacedRight = !justFacedRight;
			if (baseVelocity.Y > 0) {
				baseVelocity.Y /= bicycleFactor;
			}
		}
	}
}
