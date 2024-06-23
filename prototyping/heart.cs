using Godot;
using System;

public partial class heart : Node2D
{
	public void _onEntityEnterArea2D(Area2D area2D) {
		QueueFree();
		int count = GetTree().GetNodesInGroup("Hearts").Count - 1;
		GD.Print(count);
		if (count == 0) {
			eventSystem.startLevelCompletedEvents();
			GD.Print("Level completed");
		}
	}
}
