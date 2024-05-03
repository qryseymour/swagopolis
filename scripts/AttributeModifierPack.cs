using Godot;

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
public partial class AttributeModifierPack : Resource
{
    [ExportGroup("Operations")]
    private float att_amount;
    [Export]
    public float Att_amount { get {
            return att_amount;
    } 
    set {
            att_amount = value;
            calculateFinalValue();
    } }
    private float att_percentage;
    [Export]
    public float Att_percentage { get {
            return att_percentage;
    } 
    set {
            att_percentage = value;
            calculateFinalValue();
    } }
    private float att_multiplier = 1;
    [Export]
    public float Att_multiplier { get {
            return att_multiplier;
    } 
    set {
            att_multiplier = value;
            calculateFinalValue();
    } }
    private float att_flatamount;
    [Export]
    public float Att_flatamount { get {
            return att_flatamount;
    } 
    set {
            att_flatamount = value;
            calculateFinalValue();
    } }
    // att_finalvalue should never be modifiable outside of it's
    // respective calculations, so only getFinalValue() can get it.
    private float att_finalvalue;

    // Basic operators
    public AttributeModifierPack() {
        att_amount = 0;
        att_percentage = 0;
        att_multiplier = 1;
        att_flatamount = 0;
        calculateFinalValue();
    }
    public AttributeModifierPack(float amount = 0, float percentage = 0, float multiplier = 1, float flatamount = 0) {
        att_amount = amount;
        att_percentage = percentage;
        att_multiplier = multiplier;
        att_flatamount = flatamount;
        calculateFinalValue();
    }

    public AttributeModifierPack(AttributeModifierPack att) {
        att_amount = att.Att_amount;
        att_percentage = att.Att_percentage;
        att_multiplier = att.Att_multiplier;
        att_flatamount = att.Att_flatamount;
        calculateFinalValue();
    }

    private void calculateFinalValue() {
        att_finalvalue = (Att_amount * (Att_percentage + 1) * Att_multiplier) + Att_flatamount;
    }

    public float getFinalValue() {
        return att_finalvalue;
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
    public static AttributeModifierPack operator - (AttributeModifierPack left, AttributeModifierPack right)
    {
        return new AttributeModifierPack(left.Att_amount - right.Att_amount, left.Att_percentage - right.Att_percentage, left.Att_multiplier * -right.Att_multiplier, left.Att_flatamount - right.Att_flatamount);
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
        return att.getFinalValue();
    }

    public override bool Equals(object obj)
    {
        if (ReferenceEquals(this, obj))
        {
            return true;
        }

        if (ReferenceEquals(obj, null))
        {
            return false;
        }

        throw new System.NotImplementedException();
    }

    public override int GetHashCode()
    {
        throw new System.NotImplementedException();
    }
}
