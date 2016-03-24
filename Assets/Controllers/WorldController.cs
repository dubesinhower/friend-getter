//=======================================================================
// Copyright Martin "quill18" Glaude 2015.
//		http://quill18.com
//=======================================================================

using UnityEngine;
using System.Collections.Generic;

//public enum WorldMode { Build, Test }

public class WorldController : MonoBehaviour {

	public static WorldController Instance { get; protected set; }

    //public WorldMode Mode { get; protected set; } 

	// The world and tile data
	public World World { get; protected set; }

	// Use this for initialization
	void OnEnable () {
        //Mode = WorldMode.Edit;

        if (Instance != null) {
			Debug.LogError("There should never be two world controllers.");
		}
		Instance = this;

		// Create a world with Empty tiles
		World = new World();

        // Center the camera
        Camera.main.transform.position = new Vector3(World.Width/2f, World.Height/2f, Camera.main.transform.position.z);
	}

    //void OnWorldModeChanged()
    //{
    //    for (int x = 0; x < World.Width; x++)
    //    {
    //        for (int y = 0; y < World.Height; y++)
    //        {
    //            // TODO: Create callback from SpriteController
    //            SetTileBasedOnMode(World.GetTileAt(x, y));
    //        }
    //    }
    //}

    //public
    //    void SetModeEdit()
    //{
    //    Mode = WorldMode.Edit;
    //    OnWorldModeChanged();
    //}

    //public void SetModeTest()
    //{
    //    Mode = WorldMode.Test;
    //    OnWorldModeChanged();
    //}

    /// <summary>
    /// Gets the tile at the unity-space coordinates
    /// </summary>
    /// <returns>The tile at world coordinate.</returns>
    /// <param name="coord">Unity World-Space coordinates.</param>
    public Tile GetTileAtWorldCoord(Vector3 coord)
    {
        int x = Mathf.FloorToInt(coord.x);
        int y = Mathf.FloorToInt(coord.y);

        return World.GetTileAt(x, y);
    }
}
