using Godot;
using System;

public partial class world : Node2D
{
    private Polygon2D polygon2D = null;
    private CollisionPolygon2D collisionPolygon2D = null;
    public override void _Ready()
    {
        // This creates a visible environment through the CP2D and the child P2D without needing to manually edit the P2D every time we want to update things.
        collisionPolygon2D = GetNode<CollisionPolygon2D>("StaticBody2D/CollisionPolygon2D");
        polygon2D = GetNode<Polygon2D>("StaticBody2D/CollisionPolygon2D/Polygon2D");
        polygon2D.Polygon = collisionPolygon2D.Polygon;
        RenderingServer.SetDefaultClearColor(new Color(0f,0f,0f));
    }
}
