using Godot;

public class damageTicket
{
    public bool valid = true;
    public attributeModifierPack dmg_damageValue;
    public Node dmg_victim;
    public bool dmg_isVictimCharacter { get; private set; } = false;
    public Node dmg_attacker;
    public bool dmg_isAttackerCharacter { get; private set; } = false;
    public string dmg_ability;
    public ElementalTag dmg_elementalTag;
    public damageTicket(attributeModifierPack damageValue = null, Node victim = null, Node attacker = null, ElementalTag elementalTag = ElementalTag.None, string ability = "") {
        dmg_damageValue = damageValue != null ? damageValue : new attributeModifierPack(0);
        dmg_victim = victim;
        dmg_attacker = attacker;
        if (dmg_victim == null || dmg_attacker == null) {
            valid = false;
        } 
        else 
        {
            if (dmg_victim is characterEntity) {
                dmg_isVictimCharacter = true;
            }
            if (dmg_attacker is characterEntity) {
                dmg_isAttackerCharacter = true;
            }
            dmg_ability = ability;
            dmg_elementalTag = elementalTag;
        }
    }

    public damageTicket(float damageValue, Node victim = null, Node attacker = null, ElementalTag elementalTag = ElementalTag.None, string ability = "") : this(new attributeModifierPack(damageValue), victim, attacker, elementalTag, ability) { }

    public void printDebugInfo() {
        GD.Print("Valid:            " + valid);
        GD.Print("Damage Attribute: (" + dmg_damageValue.Att_amount + ", " + dmg_damageValue.Att_percentage + ", " + dmg_damageValue.Att_multiplier + ", " + dmg_damageValue.Att_flatamount + ")");
        GD.Print("Victim (" + (dmg_isVictimCharacter ? "T" : "F") + "):       " + dmg_victim.Name);
        GD.Print("Attacker ("  + (dmg_isAttackerCharacter ? "T" : "F") + "):     " + dmg_attacker.Name);
        GD.Print("Elemental Tags:   " + dmg_elementalTag);
        GD.Print("Ability:          " + dmg_ability);
    }

    public static explicit operator bool(damageTicket dmg) {
        return dmg.valid;
    }
}
