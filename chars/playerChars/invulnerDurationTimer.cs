using Godot;
using System;

public partial class invulnerDurationTimer : Timer
{
    public player playerEntity;
    public override void _Ready()
    {
        Node parent = GetParent();
        if (parent is player) {
            playerEntity = parent as player;
            this.Timeout += () => { 
                playerEntity.removeInvulnerDurationTimerAtLayer(this.Name);
            };
        }
    }
}
