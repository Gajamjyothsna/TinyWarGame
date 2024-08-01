using UnityEngine;

public class UnitSpawner : MonoBehaviour
{
    public GameObject blueUnitPrefab;
    public GameObject redUnitPrefab;

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // Left click
        {
            SpawnUnit(blueUnitPrefab);
        }
        else if (Input.GetMouseButtonDown(1)) // Right click
        {
            SpawnUnit(redUnitPrefab);
        }
    }

    void SpawnUnit(GameObject unitPrefab)
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            Vector3 spawnPosition = hit.point;
            spawnPosition.y = 0f; // Set Y position to 0

            // Instantiate the unit
            Instantiate(unitPrefab, spawnPosition, Quaternion.identity);
        }
    }
}
