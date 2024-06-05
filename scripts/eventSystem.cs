using Godot;
using System;

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

        AddUserSignal("LevelCompleted");
        GD.Print("Signal created");
    }
}