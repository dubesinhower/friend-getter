using UnityEngine;
using System.Collections;

public class WorldController : MonoBehaviour {

    public Sprite floorSprite;

    World world;
	// Use this for initialization
	void Start () {
        world = new World();        

        for (int x = 0; x < world.Width; x++)
        {
            for (int y = 0; y < world.Height; y++)
            {
                Tile tile_data = world.GetTileAt(x, y);
                
                GameObject tile_go = new GameObject();
                tile_go.name = "Tile_" + x + "_" + y;
                tile_go.transform.position = new Vector3(tile_data.X, tile_data.Y, 0);

                tile_go.AddComponent<SpriteRenderer>();

                //replace with events https://www.youtube.com/watch?v=DWtbNWMrOTM&list=PLbghT7MmckI4_VM5q3va043FgAwRim6yX&index=3
                tile_data.RegisterTileTypeChangedCallback((tile) => { OnTileTypeChanged(tile, tile_go); });
            }
        }

        world.RandomizeTiles();
    }

    float randomizeTileTimer = 2f;
	
	// Update is called once per frame
	void Update () {
        randomizeTileTimer -= Time.deltaTime;

        if(randomizeTileTimer < 0)
        {
            world.RandomizeTiles();
            randomizeTileTimer = 2f;
        }
	}

    void OnTileTypeChanged(Tile tile_data, GameObject tile_go)
    {
        SpriteRenderer tile_sr = tile_go.GetComponent<SpriteRenderer>();

        if (tile_data.Type == Tile.TileType.Ground)
        {
            tile_sr.sprite = floorSprite;
        }
        else if(tile_data.Type == Tile.TileType.Empty)
        {
            tile_sr.sprite = null;
        }
        else
        {
            Debug.LogError("OnTileTypeChanged - Unrecognized tile type.");
        }
    }
}
