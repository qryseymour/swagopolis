using Godot;
using System;

public partial class statusEffectTag : Node, eventResponder {
    protected Node owner;
    protected bool isCharacterEntity = false;

    public override void _Ready()
    {
        owner = GetParent();
        if (GetParent() is characterEntity) {
            isCharacterEntity = true;
        }
    }

    public virtual void preSpawnEvent() { }

    public virtual void postSpawnEvent() { }

    public virtual void preJumpEvent() { }

    public virtual void postJumpEvent() { }

    public virtual void preAbilityEvent() { }

    public virtual void postAbilityEvent() { }

    public virtual void groundedEvent() { }

    public virtual void airborneEvent() { }

    public virtual void preDamageEvent() { }

    public virtual void postDamageEvent() { }

    public virtual void preHealingEvent() { }

    public virtual void postHealingEvent() { }

    public virtual void preStatusEvent() { }

    public virtual void postStatusEvent() { }

    public virtual void preCollectibleEvent() { }

    public virtual void postCollectibleEvent() { }
    
    public virtual void levelCompleted() { }
}