using Godot;
using System;

public partial class SpecialTiles : TileMap
{
	public override void _Ready()
	{
		// For each of the tiles within the designated layer, retrieve their tile data.
		foreach (Vector2I tilePos in GetUsedCells(0)) {
			TileData data = GetCellTileData(0, tilePos);
			if (data != null) {
				/* 
					Each valid tile has a custom data that points
					to a specific object. During the foreach loop,
					that refered object is instantiated, placed at
					it's tile position, and added as a child to the
					custom object. Then the original tile is... uh,
					aborted.
				*/
				Node2D customObj = ResourceLoader.Load<PackedScene>("res://prototyping/spikes.tscn").Instantiate() as Node2D;
				customObj.Position = MapToLocal(tilePos);
				AddChild(customObj);
				SetCell(0, tilePos, -1);
			}
		}
	}
}
