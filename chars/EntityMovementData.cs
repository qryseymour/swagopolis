using Godot;

[GlobalClass]
public partial class EntityMovementData : Resource {
    [ExportGroup("Attributes")]
    [Export]
    public AttributeModifierPack Speed = new AttributeModifierPack(100);
    [Export]
	public AttributeModifierPack Acceleration = new AttributeModifierPack(100);
    [Export]
	public AttributeModifierPack Friction = new AttributeModifierPack(100);
    [Export]
	public AttributeModifierPack JumpVelocity = new AttributeModifierPack(-300);
    [Export]
	public AttributeModifierPack AvailableJumps = new AttributeModifierPack(1);
    [Export]
	public AttributeModifierPack GravityVelocity = new AttributeModifierPack(980);

    public EntityMovementData() : this(null, null, null, null, null, null) { }
    public EntityMovementData(AttributeModifierPack speed, AttributeModifierPack acceleration, AttributeModifierPack friction, 
    AttributeModifierPack jumpVelocity, AttributeModifierPack availableJumps, AttributeModifierPack gravityVelocity) {
        Speed = speed;
        Acceleration = acceleration;
        Friction = friction;
        JumpVelocity = jumpVelocity;
        AvailableJumps = availableJumps;
        GravityVelocity = gravityVelocity;
    }
}