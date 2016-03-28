using UnityEngine;
using System.Collections;
using Tiled2Unity;

// http://answers.unity3d.com/questions/501893/calculating-2d-camera-bounds.html
public class ClampCameraToStage : MonoBehaviour
{
    public GameObject Stage;
    private float _minX, _maxX, _minY, _maxY;

	// Use this for initialization
	void Start ()
	{
	    var tiledMap = Stage.GetComponent<TiledMap>();
	    var mapX = tiledMap.NumTilesWide;
	    var mapY = tiledMap.NumTilesHigh;
	    var vertExtent = Camera.main.orthographicSize;
	    var horzExtent = vertExtent * Screen.width / Screen.height;

	    _minX = horzExtent - mapX/2.0f;
	    _maxX = mapX/2.0f - horzExtent;
	    _minY = vertExtent - mapY/2.0f;
	    _maxY = mapY/2.0f - vertExtent;
	}
	
	// Update is called once per frame
	void LateUpdate () {
        var v3 = transform.position;
        v3.x = Mathf.Clamp(v3.x, _minX, _maxX);
        v3.y = Mathf.Clamp(v3.y, _minY, _maxY);
        transform.position = v3;
    }
}
