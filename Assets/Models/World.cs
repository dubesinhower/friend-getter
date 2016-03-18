//=======================================================================
// Copyright Martin "quill18" Glaude 2015.
//		http://quill18.com
//=======================================================================

using System;
using UnityEngine;
using System.Collections;
using Random = UnityEngine.Random;

public class World {

	// A two-dimensional array to hold our tile data.
    private Tile[,] _tiles;

	// The tile width of the world.
	public int Width { get; protected set; }

	// The tile height of the world
	public int Height { get; protected set; }

    private Action<Tile> _cbTileChanged;

	/// <summary>
	/// Initializes a new instance of the <see cref="World"/> class.
	/// </summary>
	/// <param name="width">Width in tiles.</param>
	/// <param name="height">Height in tiles.</param>
	public World(int width = 100, int height = 100) {
		Width = width;
		Height = height;

		_tiles = new Tile[Width,Height];

		for (int x = 0; x < Width; x++) {
			for (int y = 0; y < Height; y++) {
				_tiles[x,y] = new Tile(this, x, y);
                _tiles[x, y].RegisterTileChanged( OnTileChanged );
            }
		}

		Debug.Log ("World created with " + (Width*Height) + " tiles.");
	}

	/// <summary>
	/// A function for testing out the system
	/// </summary>
	public void RandomizeTiles() {
		Debug.Log ("RandomizeTiles");
		for (int x = 0; x < Width; x++) {
			for (int y = 0; y < Height; y++) {
				_tiles[x,y].Type = Random.Range(0, 2) == 0 ? TileType.Empty : TileType.Ground;
            }
		}
	}

	/// <summary>
	/// Gets the tile data at x and y.
	/// </summary>
	/// <returns>The <see cref="Tile"/>.</returns>
	/// <param name="x">The x coordinate.</param>
	/// <param name="y">The y coordinate.</param>
	public Tile GetTileAt(int x, int y) {
		if( x > Width || x < 0 || y > Height || y < 0) {
			Debug.LogError("Tile ("+x+","+y+") is out of range.");
			return null;
		}
		return _tiles[x, y];
	}

    public void RegisterTileChanged(Action<Tile> callbackFunc)
    {
        _cbTileChanged += callbackFunc;
    }

    public void UnregisterTileChanged(Action<Tile> callbackFunc)
    {
        _cbTileChanged -= callbackFunc;
    }

    void OnTileChanged(Tile t)
    {
        if (_cbTileChanged == null)
            return;

        _cbTileChanged(t);
    }
}
