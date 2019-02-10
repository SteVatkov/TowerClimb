using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour {

    public float spawnDelay = 0.0001f;

    [SerializeField] private GameObject[] levelPrefabs;

	void Start () {
        SpawnUntilFull();
    }

    private void SpawnFloors()
    {
        foreach (Transform child in transform)
        {
            int floorNumber = Random.Range(0, levelPrefabs.Length);
            GameObject floor = Instantiate(levelPrefabs[floorNumber], child.transform.position, Quaternion.identity) as GameObject;
            floor.transform.parent = child;
        }

    }

    void SpawnUntilFull()
    {
        Transform freePosition = NextFreePosition();
        if (freePosition)
        {
            int floorNumber = Random.Range(0, levelPrefabs.Length);
            GameObject floor = Instantiate(levelPrefabs[floorNumber], freePosition.position, Quaternion.identity) as GameObject;
            floor.transform.parent = freePosition;
        }
        if (NextFreePosition())
        {
            Invoke("SpawnUntilFull", spawnDelay);
        }
    }

    Transform NextFreePosition()
    {
        foreach (Transform childPositionGameObject in transform)
        {
            if (childPositionGameObject.childCount == 0)
            {
                return childPositionGameObject;
            }
        }
        return null;
    }
}
