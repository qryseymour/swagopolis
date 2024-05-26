using Godot;
using System;

public partial class SpecialTiles : TileMap
{
	private ResourcePreloader resourcePreloader;

	public override void _Ready()
	{
		resourcePreloader = GetNode<ResourcePreloader>("ResourcePreloader");
		foreach (Vector2I cellPos in GetUsedCells(0)) {
			TileData data = GetCellTileData(0, cellPos);
			if (data != null) {
				// TODO: Prevent error
				Resource preloadedObj = resourcePreloader.GetResource(data.GetCustomData("preload").Obj as StringName);
				if (preloadedObj != null) {
					// Instance the new resource, set position, and add child
					SetCell(0, cellPos, -1);
				}
			}
		}
	}
}
