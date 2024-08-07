using Godot;
using System;
using System.Collections.Generic;

public partial class player : characterEntity, eventResponder
{
	// Player variable shadowing
	public player() {
		canJumpMidair = true;
	}

	// Important Attributes
	public attributeModifierPack InvulnerbilityFrames = new attributeModifierPack(3);


	// Non-Important Attributes
	public float additionalGravityFactor = 2;
	public float wallSlidingGravityFactor = 0.5f;
	public float bicycleFactor = 5;



	// Fundamental Variables
    public List<String> invulnerDurationTimers = new List<String>();
	protected bool justFacedRight = true;
	public bool isWallSliding = false;
	public bool justOnWall = false;



    // Node references
	public Timer coyoteJumpTimer = null;
	public Timer blinkTimer = null;
	public Area2D hitboxCollision = null;
	public CollisionShape2D hitboxCollisionShape2D = null;

	public override void _Ready()
	{
		// Basic code to override ready with the base class implementation, followed by additional code.
		base._Ready();
		coyoteJumpTimer = GetNode<Timer>("CoyoteJumpTimer");
		blinkTimer = GetNode<Timer>("BlinkTimer");
		hitboxCollision = GetNode<Area2D>("HitboxCollision");
		hitboxCollisionShape2D = hitboxCollision.GetNode<CollisionShape2D>("CollisionShape2D");
		/* 
			When the coyote jump timer expires, if the entity is
			unable to jump midair, is not jumping, and not on the
			floor (doing the dinosaur) (doing ur mom), then the
			entity loses one possible jump they can make.
		*/
        eventSystem.postDamageEventChain += postDamageEvent;
		blinkTimer.Timeout += () => {
			animatedSprite2D.Visible = !animatedSprite2D.Visible;
		};
		
		coyoteJumpTimer.Timeout += () =>
		{
			if (!canJumpMidair && !isJumping && !IsOnFloor())
			{
				jumpCount = jumpCount < 1 ? 0 : jumpCount - 1;
			}
		};
	}

    public override void _ExitTree()
    {
        eventSystem.postDamageEventChain -= postDamageEvent;
        base._ExitTree();
    }

    protected override void controlCharacterPhysics(double delta)
    {
		base.controlCharacterPhysics(delta);
		handleCoyoteJump();
        bicycle();
    }

    protected override void handleGravity(double delta)
    {
		/*
			The player has a choice whilst falling to hold the
			down button and increase their descent downwards.
		*/
		isWallSliding = (!justOnWall || isWallSliding || Velocity.Y > 0) && !IsOnFloor() && IsOnWall() && (Input.IsActionPressed("ui_right") || Input.IsActionPressed("ui_left"));
		justOnWall = IsOnWall();
		float gravity = entityBattleData.GravityVelocity.getFinalValue();
        if (Input.IsActionPressed("ui_down"))
        {
			gravity *= additionalGravityFactor;
        }
        if (isWallSliding)
        {
			gravity *= wallSlidingGravityFactor;
			GD.Print("isWallSliding");
        }
		applyGravity(delta, gravity);
    }
	protected override void restoreJumps()
    {
		// This override of restoreJumps() now also accounts
		// for the coyoteJumpTimer for the restoration.
        if (IsOnFloor() || coyoteJumpTimer.TimeLeft > 0)
        {
            jumpCount = entityBattleData.AvailableJumps.getFinalValue();
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

	protected override void applyJump()
    {
		/*
			This is the implementation for wall-jumps; When
			a wall-jump is made, its by checking the conditions,
			applying the horizontal force, then simply incrementing
			the jump count, before it then processes a regular
			jump. This... may be clever?
		*/
		if (isWallSliding)
		{
			baseVelocity.X = GetWallNormal().X * entityBattleData.Speed.getFinalValue();
			jumpCount++;
		}
		base.applyJump();
    }

    public override void processDamage(damageTicket damage) {
		if (!invulnerDurationTimers.Contains("idt-" + damage.dmg_invulnerLayer))
        {
			createInvulnerDurationTimerAtLayer(damage.dmg_invulnerFrames, damage.dmg_invulnerLayer);
            base.processDamage(damage);
        }
    }

    private void createInvulnerDurationTimerAtLayer(float frames, string name)
    {
		/* 
			For player characters, when they process damage after
			the pre-damage events, there is check on whether a
			state of invulnerability is active on the damage
			ticket's layer. If there isn't, the player creates
			a new timer representing this invulnerability,
			which then has it's properties/attributes modified,
			(using a packedscene is not that necessary just
			for setting the oneshot variable to be true.)
		*/
        invulnerDurationTimer durTimer = new invulnerDurationTimer();
        AddChild(durTimer);
        durTimer.WaitTime = frames;
        durTimer.OneShot = true;
        durTimer.Name = "idt-" + name;
        invulnerDurationTimers.Add(durTimer.Name);
        durTimer.Start();
        blinkTimer.Start();
        hitboxCollisionShape2D.SetDeferred("disabled", true);
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

	public void removeInvulnerDurationTimerAtLayer(String layer) {
		/* 
			Removes an invulnerDurationTimer at the specified
			layer by first removing the string that tracks it's
			existance from the list (it's datatype must be a 
			string in order to compare it with the layer the damage
			ticket is referring to), then it tries to remove it
			should it be found. After all of this, if there are
			no more invulnerDurationTimers, the blinking timer
			stops, indicating there are no more active invulnerability
			frames.
		*/
		invulnerDurationTimers.Remove(layer);
		invulnerDurationTimer idt = GetNode<invulnerDurationTimer>(layer);
		if (idt != null) {
			idt.QueueFree();
		}
		if (invulnerDurationTimers.Count <= 0) {
			blinkTimer.Stop();
			hitboxCollisionShape2D.SetDeferred("disabled", false);
		}
	}

    public override void postDamageEvent(damageTicket dmgTicket)
    {
		// Placeholder for debugging purposes
		GD.Print("Current Health: " + currentHealth);
    }

}
