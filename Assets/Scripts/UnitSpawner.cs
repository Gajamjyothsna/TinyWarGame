using System;
using UnityEngine;

public class UnitSpawner : MonoBehaviour
{
    public GameObject blueUnitPrefab;
    public GameObject redUnitPrefab;

    private System.Random random = new System.Random();

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // Left click
        {
            SpawnUnit(blueUnitPrefab, "Blue");
        }
        else if (Input.GetMouseButtonDown(1)) // Right click
        {
            SpawnUnit(redUnitPrefab, "Red");
        }
    }

    void SpawnUnit(GameObject unitPrefab, string unitType)
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            Vector3 spawnPosition = hit.point;
            spawnPosition.y = 0f; // Set Y position to 0

            // Instantiate the unit
            GameObject unit = Instantiate(unitPrefab, spawnPosition, Quaternion.identity);
            Debug.Log($"{unitType} unit spawned at position: {spawnPosition}");

            // Store the spawn time
            DateTime spawnTime = DateTime.Now;

            // Generate player name and log details
            GeneratePlayerName(unitType, spawnPosition, spawnTime);
        }
        else
        {
            Debug.Log("No hit detected.");
        }
    }

    private void GeneratePlayerName(string unitType, Vector3 spawnPosition, DateTime spawnTime)
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
    }
}
