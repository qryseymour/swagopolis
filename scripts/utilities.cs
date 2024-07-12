using Godot;

public static class Utilities {
    static bool addStatusEffect(Node victim, Node inflictor, PackedScene packedScene) {
        bool validity = false;
        var statusScene = packedScene.Instantiate();
        if (statusScene != null)
        {
            if (statusScene is statusEffectTag) {
                statusEffectTag statusEffect = statusScene as statusEffectTag;
                statusTicket StatusTicket = new statusTicket(victim, inflictor, statusEffect);
                eventSystem.startPreStatusEvents(StatusTicket);
                validity = StatusTicket.valid;
                if (validity)
                {
                    inflictor.AddChild(statusEffect);
                    statusEffect.setParentAsNewOwner();
                    statusEffect.onStart();
                    eventSystem.startPostStatusEvents(StatusTicket);
                } else {
                    statusEffect.QueueFree();
                }
            } else {
                GD.PushWarning("res://scripts/statuses/" + packedScene + ".tscn was found, but doesn't appear to be a statusEffectTag!");
            }
        } else {
            GD.PushWarning("Could not find res://scripts/statuses/" + packedScene + ".tscn!");
        }
        return validity;
    }
    static bool addStatusEffect(Node victim, Node inflictor, string packedScene) {
        return addStatusEffect(victim, inflictor, ResourceLoader.Load<PackedScene>("res://scripts/statuses/" + packedScene + ".tscn"));
    }
}