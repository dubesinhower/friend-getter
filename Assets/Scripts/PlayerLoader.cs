using UnityEngine;
using System.Collections;

public class PlayerLoader : MonoBehaviour {

    public GameObject PlayerPrefab;
    private static GameObject instance;
    void Awake()
    {
        Instantiate(PlayerPrefab, new Vector3(0, 0, 0), Quaternion.identity);
    }

    // Use this for initialization
    void Start()
    {
        SetPlayerPosition(this.transform.position);
    }

    void SetPlayerPosition(Vector3 position)
    {
        PlayerPrefab.transform.position = position;
    }

    void Update()
    {
        // Doesn't work yet, need to find player and set transform
        if (Input.GetKeyDown(KeyCode.R))
        {
            SetPlayerPosition(this.transform.position);
        }
    }
}
