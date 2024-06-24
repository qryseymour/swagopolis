using Godot;
using System;
/// <summary>
/// Entity Battle Data - as it says on the tin - is the storage
/// of different attribute modifier packs representing the entity's
/// stats, things such as Health, Attack, and Defense.
/// </summary> 
[GlobalClass]
public partial class entityBattleData : Resource
{
    [ExportGroup("Attributes")]
    [ExportSubgroup("Base Attributes")]
    [Export]
    public attributeModifierPack MaxHealth = new attributeModifierPack(50);
    [Export]
    public attributeModifierPack MaxMana = new attributeModifierPack(50);
    [Export]
    public attributeModifierPack AttackSpeed = new attributeModifierPack(0);
    [Export]
    public attributeModifierPack Defense = new attributeModifierPack(0);
    [Export]
    public attributeModifierPack Speed = new attributeModifierPack(100);
    [Export]
    public attributeModifierPack Luck = new attributeModifierPack(0);
    [ExportSubgroup("Physics Attributes")]
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
    [ExportSubgroup("Two-Way Attributes")]
    [Export]
    public attributeModifierPack ExtraHealingDealt = new attributeModifierPack(0);
    [Export]
    public attributeModifierPack ExtraHealingReceived = new attributeModifierPack(0);
    [Export]
    public attributeModifierPack ExtraDamageDealt = new attributeModifierPack(0);
    [Export]
    public attributeModifierPack ExtraDamageReceived = new attributeModifierPack(0);
    [Export]
    public attributeModifierPack ExtraKnockbackDealt = new attributeModifierPack(0);
    [Export]
    public attributeModifierPack ExtraKnockbackReceived = new attributeModifierPack(0);
    [ExportSubgroup("Projectile Attributes")]
    [Export]
    public attributeModifierPack ExtraProjectileRange = new attributeModifierPack(0);
    [Export]
    public attributeModifierPack ExtraProjectileSpeed = new attributeModifierPack(0);
    [Export]
    public attributeModifierPack ExtraExplosionSize = new attributeModifierPack(0);
    public entityBattleData() : this(null, null, null, null, null, 
    null, null, null, null, null, null, null, null, null, null, 
    null, null, null, null, null) { }
    public entityBattleData(attributeModifierPack maxHealth, attributeModifierPack maxMana, 
    attributeModifierPack attackSpeed, attributeModifierPack defense, 
    attributeModifierPack speed, attributeModifierPack luck, 
    attributeModifierPack acceleration, attributeModifierPack friction, 
    attributeModifierPack jumpVelocity, attributeModifierPack availableJumps, 
    attributeModifierPack gravityVelocity, attributeModifierPack extraHealingDealt, 
    attributeModifierPack extraHealingReceived, attributeModifierPack extraDamageDealt, 
    attributeModifierPack extraDamageReceived, attributeModifierPack extraKnockbackDealt, 
    attributeModifierPack extraKnockbackReceived, attributeModifierPack extraProjectileRange, 
    attributeModifierPack extraProjectileSpeed, attributeModifierPack extraExplosionSize) {
        MaxHealth = maxHealth;
        MaxMana = maxMana;
        AttackSpeed = attackSpeed;
        Defense = defense;
        Speed = speed;
        Luck = luck;

        Acceleration = acceleration;
        Friction = friction;
        JumpVelocity = jumpVelocity;
        AvailableJumps = availableJumps;
        GravityVelocity = gravityVelocity;

        ExtraHealingDealt = extraHealingDealt;
        ExtraHealingReceived = extraHealingReceived;
        ExtraDamageDealt = extraDamageDealt;
        ExtraDamageReceived = extraDamageReceived;
        ExtraKnockbackDealt = extraKnockbackDealt;
        ExtraKnockbackReceived = extraKnockbackReceived;

        ExtraProjectileRange = extraProjectileRange;
        ExtraProjectileSpeed = extraProjectileSpeed;
        ExtraExplosionSize = extraExplosionSize;
    }
}
