using Godot;
using System;

public partial class statusEffectTag : Node {
    private CharacterEntity owner;

    public override void _Ready()
    {
        if (GetParent() is CharacterEntity) {
            owner = GetParent() as CharacterEntity;
        }
    }
}