using Godot;
using System;

public partial class spikes : Node2D
{
	public void _onEntityEnterArea2D(Area2D area2D) {
		GD.Print("Test area2D");
		CharacterEntity entity = area2D.GetParent() as CharacterEntity;
		if (entity != null) {
			entity.GlobalPosition = entity.startingPosition;
		}
	}
}
