//=======================================================================
// Copyright Martin "quill18" Glaude 2015.
//		http://quill18.com
//=======================================================================

using System;
using UnityEngine;
using System.Collections.Generic;

public class TileSpriteController : MonoBehaviour {
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
                tileGoSpriteRenderer.sprite = null;

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
    private void OnTileChanged(Tile tileData) {
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

        if (tileData.Type == TileType.Ground)
        {
            
            tileGoSpriteRenderer.sprite = GetGroundSprite(tileData);
            tileGo.GetComponent<BoxCollider2D>().enabled = true;
            tileGo.layer = LayerMask.NameToLayer("Ground");
        }
        else if (tileData.Type == TileType.Empty)
        {
            tileGoSpriteRenderer.sprite = null;
            tileGo.GetComponent<BoxCollider2D>().enabled = false;
            tileGo.layer = LayerMask.NameToLayer("Default");
        }
        else {
            Debug.LogError("OnTileChanged - Unrecognized tile type.");
        }
    }

    private Sprite GetGroundSprite(Tile t)
    {
        var tileNorth = GetTileAtWorldCoord(new Vector3(t.X, t.Y+1));
        if (tileNorth != null && tileNorth.Type == TileType.Ground)
        {
            return _tileSprites["grassCenter.png"];
        }
        return _tileSprites["grassMid.png"];
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
