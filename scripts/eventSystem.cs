using Godot;
using System;
public delegate void damageProcessor(damageTicket dmgTicket);
public delegate void levelEndProcessor();
public partial class eventSystem : Node
{	
	// Credits: https://github.com/godotengine/godot/issues/82268
	public static eventSystem Instance { get; private set; }
	public override void _EnterTree()
    {
        if (Instance != null)
        {
            GD.PushWarning("Attempted to re-create another instance of signal bus!");
            return;
        }

        Instance = this;
    }
    public static event damageProcessor preDamageEventChain;
    public static void startPreDamageEvents(damageTicket dmgTicket) {
        preDamageEventChain?.Invoke(dmgTicket);
    }
    public static event damageProcessor postDamageEventChain;
    public static void startPostDamageEvents(damageTicket dmgTicket) {
        postDamageEventChain?.Invoke(dmgTicket);
    }
    public static event levelEndProcessor levelCompletedEventChain;
    public static void startLevelCompletedEvents() {
        levelCompletedEventChain?.Invoke();
    }
}