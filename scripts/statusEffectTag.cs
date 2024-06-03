using Godot;
using System;

public partial class statusEffectTag : Node {
    private characterEntity owner;

    public override void _Ready()
    {
        if (GetParent() is characterEntity) {
            owner = GetParent() as characterEntity;
        }
    }
}