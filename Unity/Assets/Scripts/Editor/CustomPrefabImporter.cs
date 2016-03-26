using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// Example custom importer:
[Tiled2Unity.CustomTiledImporter]
class CustomPrefabImporter : Tiled2Unity.ICustomTiledImporter
{
    public void HandleCustomProperties(GameObject gameObject,
        IDictionary<string, string> customProperties)
    {
        Debug.Log("Handle custom properties from Tiled map");
        if (customProperties.ContainsKey("Handler"))
        {
            if (customProperties["Handler"] == "OneWayPlatform")
            {
                // Get array of one way platform box colliders
                var boxColliders = gameObject.GetComponentsInChildren<BoxCollider2D>();

                // Add platform effector to each box colliders' game object
                foreach (var collider in boxColliders)
                {
                    var platformEffector = collider.gameObject.AddComponent<PlatformEffector2D>();
                    platformEffector.surfaceArc = 178;
                    platformEffector.useOneWay = true;
                    collider.usedByEffector = true;
                }
            }
        }
    }

    public void CustomizePrefab(GameObject prefab)
    {
        Debug.Log("Customize prefab");
    }
}
