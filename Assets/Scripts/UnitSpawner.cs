using UnityEngine;
using UnityEngine.UI;

public class UnitSpawner : MonoBehaviour
{
    public GameObject blueUnitPrefab;
    public GameObject redUnitPrefab;
    public GameObject sliderPrefab; // Reference to the slider prefab
    public Canvas canvas; // Reference to the World Space Canvas

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
            GameObject unit = Instantiate(unitPrefab, spawnPosition, Quaternion.identity);

            // Instantiate the slider
            if (sliderPrefab != null && canvas != null)
            {
                // Instantiate the slider as a child of the canvas
                GameObject slider = Instantiate(sliderPrefab, spawnPosition + new Vector3(0, 2, 0), Quaternion.identity);

                // Set the slider's parent to the canvas
                slider.transform.SetParent(canvas.transform);

                // Adjust slider position relative to the canvas
                slider.transform.localPosition = new Vector3(0, 100, 0); // Adjust as needed
                slider.transform.localScale = new Vector3(1, 1, 1); // Ensure proper scaling

                // Debugging: Log positions
                Debug.Log("Canvas Position: " + canvas.transform.position);
                Debug.Log("Slider Position: " + slider.transform.position);
            }
        }
    }
}
