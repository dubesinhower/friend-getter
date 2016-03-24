using UnityEngine;
using System.Collections.Generic;
using UnityEngine.EventSystems;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class BuildModeController : MonoBehaviour {
    private TileType _buildModeTile = TileType.Ground;

    public Material GridMaterial;
    private Mesh _gridMesh;

    private static World World
    {
        get { return WorldController.Instance.World; }
    }

    // Use this for initialization
    private void Start()
    {
        BuildGridMesh();
    }

    private void BuildGridMesh()
    {
        GridMaterial.mainTextureScale = new Vector2(World.Width, World.Height);

        //// Generate the mesh data
        var vertices = new Vector3[4];
        var triangles = new int[6];
        var normals = new Vector3[4];
        var uv = new Vector2[4];

        vertices[0] = new Vector3(0, World.Height, 10f);
        vertices[1] = new Vector3(World.Width, World.Height, 10f);
        vertices[2] = new Vector3(0, 0, 10f);
        vertices[3] = new Vector3(World.Width, 0, 10f);

        triangles[0] = 0;
        triangles[1] = 3;
        triangles[2] = 2;
        triangles[3] = 0;
        triangles[4] = 1;
        triangles[5] = 3;

        normals[0] = Vector3.forward;
        normals[1] = Vector3.forward;
        normals[2] = Vector3.forward;
        normals[3] = Vector3.forward;

        uv[0] = new Vector2(0, 1);
        uv[1] = new Vector2(1, 1);
        uv[2] = new Vector2(0, 0);
        uv[3] = new Vector2(1, 0);

        //// Create a new mesh and populate with the data

        _gridMesh = new Mesh
        {
            vertices = vertices,
            triangles = triangles,
            normals = normals,
            uv = uv
        };

        //// Assign our mesh to our filter/renderer
        GetComponent<MeshFilter>().mesh = _gridMesh;
        var mr = GetComponent<MeshRenderer>();
        mr.material = GridMaterial;
    }

    public void SetModeGround()
    {
        _buildModeTile = TileType.Ground;
    }

    public void SetModeErase()
    {
        _buildModeTile = TileType.Empty;
    }

    public void DoBuild(Tile t)
    {
        var oldType = t.Type;
        t.Type = _buildModeTile;

        // TODO: check for update code in Youtube video
        if (t.Type == TileType.Ground || oldType == TileType.Ground)
        {
            var x = t.X;
            var y = t.Y;

            var otherT = World.GetTileAt(x, y+1);
            if (otherT != null && (otherT.Type == t.Type || oldType == TileType.Ground))
            {
                otherT.CbTileChanged(otherT);
            }

            otherT = World.GetTileAt(x+1, y);
            if (otherT != null && (otherT.Type == t.Type || oldType == TileType.Ground))
            {
                otherT.CbTileChanged(otherT);
            }

            otherT = World.GetTileAt(x, y-1);
            if (otherT != null && (otherT.Type == t.Type || oldType == TileType.Ground))
            {
                otherT.CbTileChanged(otherT);
            }

            otherT = World.GetTileAt(x-1, y);
            if (otherT != null && (otherT.Type == t.Type || oldType == TileType.Ground))
            {
                otherT.CbTileChanged(otherT);
            }
        }
    }

}
