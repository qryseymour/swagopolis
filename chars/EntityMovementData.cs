using Godot;

[GlobalClass]
public partial class entityMovementData : Resource {
    [ExportGroup("Attributes")]
    [Export]
    public attributeModifierPack Speed = new attributeModifierPack(100);
    [Export]
	public attributeModifierPack Acceleration = new attributeModifierPack(100);
    [Export]
	public attributeModifierPack Friction = new attributeModifierPack(100);
    [Export]
	public attributeModifierPack JumpVelocity = new attributeModifierPack(-300);
    [Export]
	public attributeModifierPack AvailableJumps = new attributeModifierPack(1);
    [Export]
	public attributeModifierPack GravityVelocity = new attributeModifierPack(980);

    public entityMovementData() : this(null, null, null, null, null, null) { }
    public entityMovementData(attributeModifierPack speed, attributeModifierPack acceleration, attributeModifierPack friction, 
    attributeModifierPack jumpVelocity, attributeModifierPack availableJumps, attributeModifierPack gravityVelocity) {
        Speed = speed;
        Acceleration = acceleration;
        Friction = friction;
        JumpVelocity = jumpVelocity;
        AvailableJumps = availableJumps;
        GravityVelocity = gravityVelocity;
    }
}