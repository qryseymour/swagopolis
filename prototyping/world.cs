using Godot;
using System;
using System.Drawing;

public partial class world : Node2D
{
    [Export]
    public PackedScene nextLevel;
	public override void _Ready()
    {
		RenderingServer.SetDefaultClearColor(Colors.Aqua);
        eventSystem.levelCompletedEventChain += showLevelCompleted;
    }

    public override void _ExitTree()
    {
        eventSystem.levelCompletedEventChain -= showLevelCompleted;
        base._ExitTree();
    }

    private void showLevelCompleted() {
        if (nextLevel is PackedScene) {
            CallDeferred(nameof(changeLevel));
        }
        else {    
            GetNode<ColorRect>("CanvasLayer/LevelCompletedHud").Show();
        }
    }

    private void changeLevel() {
        GetTree().ChangeSceneToPacked(nextLevel);
    }
}
