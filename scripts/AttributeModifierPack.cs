/*
    I would be type-safe and use attributeModType as a type definition 
    if anything in the future comes up that warrents it, but to due 
    incompatibilities with the current Godot version, I decided not
    to risk taking any action.
*/
// using attributeModType = float;

/// <summary>
/// Attributes are an additive amount, additive percentage, multiplicative 
/// amount, and additive flat amount. These are usually applied 
/// to bring the final stat value of whatever is being measured.
/// Attributes perserve the modifiers used in the calculations to
/// prevent any overrides.
/// </summary> 
public class AttributeModifierPack
{
    private float att_amount;
    public float Att_amount { get; set; }
    private float att_percentage;
    public float Att_percentage { get; set; }
    private float att_multiplier;
    public float Att_multiplier { get; set; }
    private float att_flatamount;
    public float Att_flatamount { get; set; }

    // Basic operators
    public AttributeModifierPack(float amount = 0, float percentage = 0, float multiplier = 1, float flatamount = 0) {
        Att_amount = amount;
        Att_percentage = percentage;
        Att_multiplier = multiplier;
        Att_flatamount = flatamount;
    }

    public AttributeModifierPack(AttributeModifierPack att) {
        Att_amount = att.Att_amount;
        Att_percentage = att.Att_percentage;
        Att_multiplier = att.Att_multiplier;
        Att_flatamount = att.Att_flatamount;
    }

    public float getTotalValue() {
        return (Att_amount * (Att_percentage + 1) * Att_multiplier) + Att_flatamount;
    }

    public static bool operator ==(AttributeModifierPack left, AttributeModifierPack right) {
        return (left.Att_amount == right.Att_amount) && (left.Att_percentage == right.Att_percentage) && (left.Att_multiplier == right.Att_multiplier) && (left.Att_flatamount == right.Att_flatamount);
    }

    public static bool operator !=(AttributeModifierPack left, AttributeModifierPack right) {
        return !(left == right);
    }

    public static AttributeModifierPack operator + (AttributeModifierPack left, AttributeModifierPack right)
    {
        return new AttributeModifierPack(left.Att_amount + right.Att_amount, left.Att_percentage + right.Att_percentage, left.Att_multiplier * right.Att_multiplier, left.Att_flatamount + right.Att_flatamount);
    }

    public static AttributeModifierPack operator * (AttributeModifierPack left, float right)
    {
        return new AttributeModifierPack(left.Att_amount * right, left.Att_percentage * right, left.Att_multiplier * right, left.Att_flatamount * right);
    }
    public static AttributeModifierPack operator / (AttributeModifierPack left, float right)
    {
        return new AttributeModifierPack(left.Att_amount / right, left.Att_percentage / right, left.Att_multiplier / right, left.Att_flatamount / right);
    }

    public static explicit operator float(AttributeModifierPack att) {
        return att.getTotalValue();
    }
}
