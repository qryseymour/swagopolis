using Godot;
using System;
public delegate void damageProcessor(damageTicket dmgTicket);
public delegate void statusProcessor(statusTicket stsTicket);
public delegate void levelEndProcessor();
/// <summary>
/// The Event System is the autoloaded node that holds and processes 
/// all associated events when they are invoked. There should
/// only ever be one instance of an EventSystem, thus a static
/// variable exists to hold itself and detect if one already
/// exists or not.
/// </summary> 
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
    public static event statusProcessor preStatusEventChain;
    public static void startPreStatusEvents(statusTicket stusTicket) {
        preStatusEventChain?.Invoke(stusTicket);
    }
    public static event statusProcessor postStatusEventChain;
    public static void startPostStatusEvents(statusTicket stusTicket) {
        postStatusEventChain?.Invoke(stusTicket);
    }
    public static event levelEndProcessor levelCompletedEventChain;
    public static void startLevelCompletedEvents() {
        levelCompletedEventChain?.Invoke();
    }
}