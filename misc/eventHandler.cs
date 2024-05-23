using Godot;
using System;

public partial class eventHandler : Node
{
    [Signal]
    public delegate void InteractableGeometryEventHandler();
}
