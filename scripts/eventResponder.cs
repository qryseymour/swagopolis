public interface eventResponder
{
    void preSpawnEvent();
    void postSpawnEvent();
    void preJumpEvent();
    void postJumpEvent();
    void preAbilityEvent();
    void postAbilityEvent();
    void groundedEvent();
    void airborneEvent();
    void preDamageEvent(damageTicket dmgTicket);
    void postDamageEvent(damageTicket dmgTicket);
    void preHealingEvent();
    void postHealingEvent();
    void preStatusEvent();
    void postStatusEvent();
    void preCollectibleEvent();
    void postCollectibleEvent();
    void levelCompleted();
}
