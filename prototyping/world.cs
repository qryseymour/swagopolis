using Godot;
using System;
using System.Drawing;

public partial class world : Node2D
{
	public override void _Ready()
    {
		RenderingServer.SetDefaultClearColor(Colors.Aqua);
        eventSystem.Instance.Connect("LevelCompleted", new Callable(this, nameof(showLevelCompleted)));
    }

    private void showLevelCompleted() {
        GD.Print("Display the screen");
        GetNode<ColorRect>("CanvasLayer/LevelCompletedHud").Show();
    }
}
