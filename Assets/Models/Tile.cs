using UnityEngine;
using System.Collections;
using System;

public class Tile  {
    public enum TileType { Empty, Ground }

    TileType type = TileType.Empty;

    Action<Tile> tileTypeChangedCallback;

    public TileType Type
    {
        get
        {
            return type;
        }

        set
        {
            TileType oldType = type;
            type = value;
            if(tileTypeChangedCallback != null && oldType != type)
                tileTypeChangedCallback(this);
        }
    }

    World world;
    int x;
    public int X
    {
        get
        {
            return x;
        }
    }
    int y;
    public int Y
    {
        get
        {
            return y;
        }
    }

    public Tile ( World world, int x, int y)
    {
        this.world = world;
        this.x = x;
        this.y = y;
    }

    // convert to event https://www.youtube.com/watch?v=DWtbNWMrOTM&list=PLbghT7MmckI4_VM5q3va043FgAwRim6yX&index=3
    public void RegisterTileTypeChangedCallback(Action<Tile> callback)
    {
        tileTypeChangedCallback += callback;
    }

    public void UnregisterTileTypeChangedCallback(Action<Tile> callback)
    {
        tileTypeChangedCallback -= callback;
    }
}
