using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UnitSpawner : MonoBehaviour
{
    public GameObject blueUnitPrefab;
    public GameObject redUnitPrefab;

    private System.Random random = new System.Random();

    public Canvas canvas; // Reference to your UI canvas

    private GraphicRaycaster graphicRaycaster;
    private EventSystem eventSystem;

    private void Start()
    {
        eventSystem = EventSystem.current;
        graphicRaycaster = canvas.GetComponent<GraphicRaycaster>();
    }


    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // Left click
        {
            if (!IsPointerOverEventButton())
            {
                SpawnUnit(blueUnitPrefab, UnitType.Blue);
            }
        }
        else if (Input.GetMouseButtonDown(1)) // Right click
        {
            if (!IsPointerOverEventButton())
            {
                SpawnUnit(redUnitPrefab, UnitType.Red);
            }
        }
    }

    void SpawnUnit(GameObject unitPrefab, UnitType unitType)
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            Vector3 spawnPosition = hit.point;
            spawnPosition.y = 0f; // Set Y position to 0

            // Instantiate the unit
            GameObject unitObject = Instantiate(unitPrefab, spawnPosition, Quaternion.identity);

            Debug.Log($"{unitType} unit spawned at position: {spawnPosition}");

            // Store the spawn time
            DateTime spawnTime = DateTime.Now;

            // Generate player name and log details
            GeneratePlayerName(unitType, spawnPosition, spawnTime,  unitObject);
        }
        else
        {
            Debug.Log("No hit detected.");
        }
    }

    private void GeneratePlayerName(UnitType unitType, Vector3 spawnPosition, DateTime spawnTime, GameObject unitObject)
    {
        string datePart = DateTime.Now.ToString("dd");
        string uniquePart = random.Next(1, 101).ToString("D2");
        string playerName = $"{unitType}_{datePart}{uniquePart}";

        // Calculate time difference
        TimeSpan timeDifference = DateTime.Now - spawnTime;
        string timeAgo;

        if (timeDifference.TotalMinutes < 1)
        {
            timeAgo = "0 mins ago";
        }
        else if (timeDifference.TotalMinutes < 60)
        {
            timeAgo = $"{(int)timeDifference.TotalMinutes} mins ago";
        }
        else
        {
            timeAgo = $"{(int)timeDifference.TotalHours} hours ago";
        }

        // Generate the message
        string message = $"{playerName} spawned at {spawnPosition} at time: {timeAgo}";

        // Log player name and message
        Debug.Log($"PlayerName: {playerName}");
        Debug.Log(message);

        if(unitObject != null)
        {
            unitObject.GetComponent<Player>().SetPlayerDetails(playerName, unitType);
        }

        // Raise the event
        UnitEvents.RaiseUnitSpawned(playerName, message, timeAgo, unitType);
    }




    bool IsPointerOverEventButton()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        // Define directions to check
        Vector3[] directions = { Vector3.forward, Vector3.back, Vector3.left, Vector3.right, Vector3.up, Vector3.down };

        // Check for collisions in all directions
        foreach (Vector3 direction in directions)
        {
            RaycastHit hit;
            RaycastHit hit2;

            Debug.DrawRay(ray.origin, ray.direction * 1000, Color.red, 1.0f);

            if (Physics.Raycast(ray, out hit))
            {
                // Check if the hit object has the tag "Event Button"
                if (hit.collider.CompareTag("EventButton"))
                {
                    Debug.Log("Hit Event Button");
                    return true;
                }
            }

            // Cast additional rays in different directions
            if (Physics.Raycast(ray.origin, direction, out hit2))
            {
                if (hit2.collider.CompareTag("EventButton"))
                {
                    Debug.Log("Hit Event Button in direction");
                    return true;
                }
            }
        }

        return false;
    }
}
