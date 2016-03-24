//=======================================================================
// Copyright Martin "quill18" Glaude 2015.
//		http://quill18.com
//=======================================================================

using UnityEngine;
using System.Collections;
using System;

public enum TileType { Empty, Ground, Spikes };

public class Tile {
	private TileType _type = TileType.Empty;
	public TileType Type {
		get { return _type; }
		set {
			var oldType = _type;
			_type = value;

			// Call the callback and let things know we've changed.
			if(CbTileChanged != null && oldType != _type)
				CbTileChanged(this);
		}
	}

	// We need to know the context in which we exist. Probably. Maybe.
	public int X { get; protected set; }
	public int Y { get; protected set; }

	// The function we callback any time our tile's data changes
    public Action<Tile> CbTileChanged { get; protected set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="Tile"/> class.
    /// </summary>
    /// <param name="x">The x coordinate.</param>
    /// <param name="y">The y coordinate.</param>
    public Tile( int x, int y ) {
		this.X = x;
		this.Y = y;
	    this.Type = TileType.Empty;
	}

	/// <summary>
	/// Register a function to be called back when our tile type changes.
	/// </summary>
	public void RegisterTileChanged(Action<Tile> callback) {
		CbTileChanged += callback;
	}
	
	/// <summary>
	/// Unregister a callback.
	/// </summary>
	public void UnegisterTileChanged(Action<Tile> callback) {
		CbTileChanged -= callback;
	}
	
}
