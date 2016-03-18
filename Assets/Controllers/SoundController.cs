using UnityEngine;
using System.Collections;

public class SoundController : MonoBehaviour
{
    public AudioClip TileChangeAudioClip;

    private float _soundCooldown = 0;

	// Use this for initialization
	void Start ()
	{
	    WorldController.Instance.World.RegisterTileChanged( OnTileChanged );
	}

    void Update()
    {
        if(_soundCooldown > 0)
            _soundCooldown -= Time.deltaTime;
    }

    void OnTileChanged(Tile tileData)
    {
        if (_soundCooldown > 0)
            return;

        AudioSource.PlayClipAtPoint(TileChangeAudioClip, Camera.main.transform.position);
        _soundCooldown = .1f;
    }
}
