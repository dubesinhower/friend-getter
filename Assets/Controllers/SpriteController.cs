//=======================================================================
// Copyright Martin "quill18" Glaude 2015.
//		http://quill18.com
//=======================================================================

using System;
using UnityEngine;
using System.Collections.Generic;

public class SpriteController : MonoBehaviour {

	public static WorldController Instance { get; protected set; }

    public WorldMode Mode { get; protected set; } 

    private Dictionary<Tile, GameObject> _tileGameObjectMap;

    private Dictionary<string, Sprite> _tileSprites;

    World World
    {
        get { return WorldController.Instance.World; }
    }

	// Use this for initialization
    private void Start ()
    {
        LoadSprites();

        _tileGameObjectMap = new Dictionary<Tile, GameObject>();

        // Create a GameObject for each of our tiles, so they show visually. (and redunt reduntantly)
        for (int x = 0; x < World.Width; x++)
        {
            for (int y = 0; y < World.Height; y++)
            {
                // Get the tile data
                Tile tileData = World.GetTileAt(x, y);

                // This creates a new GameObject and adds it to our scene.
                GameObject tileGo = new GameObject();

                _tileGameObjectMap.Add(tileData, tileGo);

                tileGo.name = "Tile_" + x + "_" + y;
                tileGo.transform.position = new Vector3(tileData.X, tileData.Y, 0);
                tileGo.transform.SetParent(this.transform, true);

                // Add a sprite renderer, but don't bother setting a sprite
                // because all the tiles are empty right now.
                var tileGoSpriteRenderer = tileGo.AddComponent<SpriteRenderer>();
                tileGoSpriteRenderer.sprite = _tileSprites["grassCenter.png"];
                tileGoSpriteRenderer.color = new Color(1f, 1f, 1f, .1f); // 10% opacity

                // Add a box collider, but don't enable it right now.
                var tileCol = tileGo.AddComponent<BoxCollider2D>();
                tileCol.offset = new Vector2(.5f, .5f);
                tileCol.enabled = false;
            }
        }

        World.RegisterTileChanged(OnTileChanged);
	}

    private void LoadSprites()
    {
        _tileSprites = new Dictionary<string, Sprite>();

        // TODO: this isn't working correctly
        var sprites = Resources.LoadAll<Sprite>("Sprites/Kenney Assets/platformer_spritesheet_complete");
        foreach (var sprite in sprites)
        {
            _tileSprites[sprite.name] = sprite;
        }
    }

    // This function should be called automatically whenever a tile's type gets changed.
    // Only works in edit mode.
    private void OnTileChanged(Tile tileData) {
        SetTileBasedOnMode(tileData);
	}

    private void OnWorldModeChanged()
    {
        for (var x = 0; x < World.Width; x++)
        {
            for (var y = 0; y < World.Height; y++)
            {
                SetTileBasedOnMode(World.GetTileAt(x, y));
            }
        }
    }

    public void SetModeEdit()
    {
        Mode = WorldMode.Edit;
        OnWorldModeChanged();
    }

    public void SetModeTest()
    {
        Mode = WorldMode.Test;
        OnWorldModeChanged();
    }

    private void SetTileBasedOnMode(Tile tileData)
    {
        if (_tileGameObjectMap.ContainsKey(tileData) == false)
        {
            Debug.LogError("_tileGameObjectMap doesn't contain tile data - did you forget to add tile to dictionary or unregister callback?");
            return;
        }

        var tileGo = _tileGameObjectMap[tileData];

        if (tileGo == null)
        {
            Debug.LogError("_tileGameObjectMap's returned GameObject is null - did you forget to add tile to dictionary or unregister callback?");
            return;
        }

        var tileGoSpriteRenderer = tileGo.GetComponent<SpriteRenderer>();

        if (Mode == WorldMode.Edit)
        {
            if (tileData.Type == TileType.Ground)
            {
                tileGoSpriteRenderer.sprite = _tileSprites["grassCenter.png"];
                tileGoSpriteRenderer.color = new Color(1f, 1f, 1f, 1f); // normal opacity
                tileGo.GetComponent<BoxCollider2D>().enabled = true;
                tileGo.layer = LayerMask.NameToLayer("Ground");
            }
            else if (tileData.Type == TileType.Empty)
            {
                tileGoSpriteRenderer.sprite = _tileSprites["grassCenter.png"];
                tileGoSpriteRenderer.color = new Color(1f, 1f, 1f, .1f); // 10% opacity
                tileGo.GetComponent<BoxCollider2D>().enabled = false;
                tileGo.layer = LayerMask.NameToLayer("Default");
            }
            else {
                Debug.LogError("OnTileChanged - Unrecognized tile type.");
            }
        }
    }

    /// <summary>
    /// Gets the tile at the unity-space coordinates
    /// </summary>
    /// <returns>The tile at world coordinate.</returns>
    /// <param name="coord">Unity World-Space coordinates.</param>
    public Tile GetTileAtWorldCoord(Vector3 coord)
    {
        var x = Mathf.FloorToInt(coord.x);
        var y = Mathf.FloorToInt(coord.y);

        return World.GetTileAt(x, y);
    }
}
