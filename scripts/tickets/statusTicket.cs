using Godot;

public class statusTicket
{
    public bool valid = true;
    public statusEffectTag stus_statusEffect;
    public Node stus_victim;
    public bool stus_isVictimCharacter { get; private set; } = false;
    public Node stus_inflictor;
    public bool stus_isInflictorCharacter { get; private set; } = false;
    public string stus_ability;
    // Rather than constantly evaluate if the victim or attacker
    // is a characterEntity, that is remembered using booleans.
    public statusTicket(Node victim, Node inflictor, statusEffectTag statusEffect, string ability = "") {
        stus_victim = victim;
        stus_inflictor = inflictor;
        if (stus_victim is characterEntity) {
            stus_isVictimCharacter = true;
        }
        if (stus_inflictor is characterEntity) {
            stus_isInflictorCharacter = true;
        }
        stus_statusEffect = statusEffect;
        stus_ability = ability;
    }

    public static explicit operator bool(statusTicket set) {
        return set.valid;
    }

}