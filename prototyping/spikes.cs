using Godot;
using System;

public partial class spikes : Node2D
{
	public void _onEntityEnterArea2D(Area2D area2D) {
		characterEntity entity = area2D.GetParent() as characterEntity;
		if (entity != null) {
			entity.GlobalPosition = entity.startingPosition;
			entity.damage(5);
		}
	}
}
