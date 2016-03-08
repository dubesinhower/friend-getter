using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

// http://forum.unity3d.com/threads/something-about-the-multi-object-pooling.288631/
// http://forum.unity3d.com/threads/simple-reusable-object-pool-help-limit-your-instantiations.76851/
public class ObjectPooler : MonoBehaviour {

    public static ObjectPooler Current;
    public List<GameObject> ObjectsToPool;
    public int DefaultPoolSize = 20;
    public bool WillGrow = true;

    private readonly Dictionary<string, List<GameObject>> _objectPools = new Dictionary<string, List<GameObject>>();

    private void Awake()
    {
        Current = this;
    }

    private void Start() {
        CreateObjectPools();
	}

    private void CreateObjectPools()
    {
        foreach(var objType in ObjectsToPool)
        {
            var tempList = new List<GameObject>();
            for(var i = 0; i < DefaultPoolSize; i++)
            {
                var tempObj = Instantiate(objType);
                tempObj.SetActive(false);
                tempList.Add(tempObj);
            }
            _objectPools.Add(objType.name, tempList);
        }
    }

    public GameObject GetPooledObject(string objectName)
    {
        List<GameObject> objectPool;
        if (!_objectPools.TryGetValue(objectName, out objectPool)) return null;
        foreach (var obj in objectPool.Where(obj => !obj.activeInHierarchy))
        {
            return obj;
        }
        if (!WillGrow) return null;
        var objType = GetObjectTypeFromName(objectName);
        if (objType == null) return null;
        var tempObj = Instantiate(objectPool[0]);
        objectPool.Add(tempObj);
        return tempObj;
    }

    private GameObject GetObjectTypeFromName(string objectName)
    {
        return ObjectsToPool.FirstOrDefault(obj => obj.name == objectName);
    }
}
