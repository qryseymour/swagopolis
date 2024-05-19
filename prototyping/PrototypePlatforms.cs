using Godot;
using System;

public partial class PrototypePlatforms : StaticBody2D
{
	private Polygon2D polygon2D = null;
    private CollisionPolygon2D collisionPolygon2D = null;
    public override void _Ready()
    {
        // This creates a visible environment through the CP2D and the child P2D without needing to manually edit the P2D every time we want to update things.
        collisionPolygon2D = GetNode<CollisionPolygon2D>("CollisionPolygon2D");
        polygon2D = GetNode<Polygon2D>("CollisionPolygon2D/Polygon2D");
        polygon2D.Polygon = collisionPolygon2D.Polygon;
    }
}
