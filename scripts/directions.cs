using System;

public enum Direction { 
    None = 0,
    Up = 1, 
    Right = 1 << 1, 
    Down = 1 << 2, 
    Left = 1 << 3, 
    UpRight = Up & Right,
    DownRight = Down & Right,
    DownLeft = Down & Left,
    UpLeft = Up & Left
}