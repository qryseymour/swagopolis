using Godot;
using System;
using System.Drawing;

public partial class world : Node2D
{
	public override void _Ready()
    {
		  RenderingServer.SetDefaultClearColor(Colors.Aqua);
    }
}
