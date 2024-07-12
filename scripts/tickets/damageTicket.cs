using Godot;

/// <summary>
/// Damage tickets are containers of information that are passed
/// down during damage events. They can be defined either by
/// an attribute modifier pack, or a float to then become a
/// an attribute modifier pack. They take a Victim and Attacker
/// as a Node, and if they're character entities, it's noted as
/// a boolean (additionally, there should be no reason for a
/// this to change midway through damage event processing, so
/// they're fitted with private setters.)
/// </summary> 
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
    public float dmg_invulnerFrames;
    public string dmg_invulnerLayer;
    // Rather than constantly evaluate if the victim or attacker
    // is a characterEntity, that is remembered using booleans.
    public damageTicket(Node victim, Node attacker, attributeModifierPack damageValue = null, ElementalTag elementalTag = ElementalTag.None, string ability = "", float invulnerFrames = 0.5f, string invulnerLayer = "0") {
        dmg_victim = victim;
        dmg_attacker = attacker;
        if (dmg_victim is characterEntity) {
            dmg_isVictimCharacter = true;
        }
        if (dmg_attacker is characterEntity) {
            dmg_isAttackerCharacter = true;
        }
        dmg_damageValue = damageValue is not null ? damageValue : new attributeModifierPack(0);
        dmg_ability = ability;
        dmg_elementalTag = elementalTag;
        dmg_invulnerFrames = invulnerFrames;
        dmg_invulnerLayer = invulnerLayer;
    }

    public damageTicket(Node victim, Node attacker, float damageValue, ElementalTag elementalTag = ElementalTag.None, string ability = "", float invulnerFrames = 0.5f, string invulnerLayer = "0") : this(victim, attacker, new attributeModifierPack(damageValue), elementalTag, ability, invulnerFrames, invulnerLayer) { }

    public void printDebugInfo() {
        GD.Print("Valid:            " + valid);
        GD.Print("Damage Attribute: (" + dmg_damageValue.Att_amount + ", " + dmg_damageValue.Att_percentage + ", " + dmg_damageValue.Att_multiplier + ", " + dmg_damageValue.Att_flatamount + ")");
        GD.Print("Victim (" + (dmg_isVictimCharacter ? "T" : "F") + "):       " + dmg_victim.Name);
        GD.Print("Attacker ("  + (dmg_isAttackerCharacter ? "T" : "F") + "):     " + dmg_attacker.Name);
        GD.Print("Elemental Tags:   " + dmg_elementalTag);
        GD.Print("Ability:          " + dmg_ability);
        GD.Print("InvulnerLayer:    " + dmg_invulnerLayer);
    }

    public static explicit operator bool(damageTicket dmg) {
        return dmg.valid;
    }
}
