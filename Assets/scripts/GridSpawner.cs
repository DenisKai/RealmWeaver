using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GridSpawner : MonoBehaviour
{
    public GameObject floorHandlePrefab;
    public GameObject floorPrefab;
    public GameObject cellPrefab;
    public Vector3 cameraOffset;
    public int rows = 20;
    public int columns = 20;
    public float cellSpacing = 0.002f;
    
    private Transform xrCamera;
    private float cubeSize;
    private float offsetFromHandle = 0.05f;

    void Awake() {
        xrCamera = Camera.main.transform;

        GameObject temp = Instantiate(cellPrefab);
        Renderer renderer = temp.GetComponentInChildren<Renderer>();
        cubeSize = renderer.bounds.size.x + cellSpacing;
        Destroy(temp);
    }

    void Start()
    {
        StartCoroutine(SpawnGrid());
    }

    IEnumerator SpawnGrid() {
        // wait for camera to be active
        while (xrCamera.position == Vector3.zero) {
            xrCamera = Camera.main.transform;
            yield return null;
        }

        var floorhandle = InstantiateFloorHandle(floorHandlePrefab, xrCamera.position);
        var floor = InstantiateFloor(floorPrefab, floorhandle.transform.position);
        floor.transform.SetParent(floorhandle.transform, true);

        Vector3 offSet = floorhandle.transform.position;
        offSet.x -= cubeSize * (columns / 2);

        for (int x = 0; x < columns; x++)
        {
            for (int z = 0; z < rows; z++)
            {
                Vector3 position = new Vector3(x * cubeSize + (cubeSize / 2), 0, z * cubeSize + (cubeSize / 2) + offsetFromHandle);
                var cell = Instantiate(cellPrefab, offSet + position, Quaternion.identity, transform);
                cell.transform.SetParent(floor.transform, true);
            }
        }
    }

    private GameObject InstantiateFloorHandle(GameObject floorhandlePrefab, Vector3 cameraPos) {
        var offset = new Vector3(cameraPos.x, cameraPos.y + cameraOffset.y, cameraPos.z + cameraOffset.z);
        Quaternion rotation = Quaternion.Euler(0, 0, 90);
        return Instantiate(floorhandlePrefab, offset, rotation, transform);
    }

    private GameObject InstantiateFloor(GameObject floorPrefab, Vector3 floorhandlePos) {
        var offset = new Vector3(floorhandlePos.x, floorhandlePos.y, floorhandlePos.z + cubeSize * (columns / 2) + offsetFromHandle);
        var floor = Instantiate(floorPrefab, offset, Quaternion.identity, transform);
        float gridWidth = columns * cubeSize;
        float gridDepth = rows * cubeSize;
        floor.transform.localScale = new Vector3(gridWidth, 0.025f, gridDepth);
        return floor;
    }
}
