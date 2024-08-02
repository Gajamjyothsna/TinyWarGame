using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    public enum State { Idle, Walk, Run, Attack, Die }
    public State currentState;

    public float detectionRadius = 20f;
    public float attackRange = 0.2f;
    public float walkSpeed = 2f;
    public float runSpeed = 4f;
    public float health = 100f;
    public float damage = 10f;
    public float minDistanceBetweenSameUnits = 1f; // Minimum distance to maintain between same units

    private GameObject targetUnit;
    private Animator animator;
    private string playerName;
    private UnitType playerType;
    private DateTime lastDamageTime;
    private Player attacker; // Reference to the player who attacked and caused the death

    void Start()
    {
        currentState = State.Idle;
        animator = GetComponent<Animator>();
        Debug.Log("Player started in Idle state.");
    }

    void Update()
    {
        if (currentState == State.Die)
        {
            // Skip updates if the unit is dead
            return;
        }

        if (targetUnit == null)
        {
            SearchForTarget();
        }

        if (targetUnit != null)
        {
            FaceTarget(); // Ensure the player faces the target every frame

            float distance = Vector3.Distance(transform.position, targetUnit.transform.position);

            if (distance <= attackRange)
            {
                currentState = State.Attack;
            }
            else if (distance <= detectionRadius)
            {
                currentState = State.Run;
            }
            else
            {
                currentState = State.Walk;
                MoveTowardsTarget(walkSpeed); // Move towards the target if it's out of detection radius
            }
        }
        else
        {
            // If no target and unit is in Idle state, continue walking
            if (currentState != State.Die)
            {
                currentState = State.Idle;
                Idle(); // Ensure the unit is idle if no target is present
            }
        }

        switch (currentState)
        {
            case State.Idle:
                Idle();
                break;
            case State.Walk:
                Walk();
                break;
            case State.Run:
                Run();
                break;
            case State.Attack:
                Attack();
                break;
        }
    }

    void Idle()
    {
        animator.SetFloat("moveAmount", 0);
        Debug.Log("State: Idle");
    }

    void Walk()
    {
        animator.SetFloat("moveAmount", 0.25f);
        Debug.Log("State: Walk");
    }

    void Run()
    {
        animator.SetFloat("moveAmount", 0.5f);
        Debug.Log("State: Run");
        MoveTowardsTarget(runSpeed);
    }

    void Attack()
    {
        animator.SetFloat("moveAmount", 0.75f);
        Debug.Log("State: Attack");

        if (targetUnit != null)
        {
            // Call DealDamage method here if the attack animation is complete
            // Use an animation event or coroutine if needed
            //DealDamage();
        }
        else
        {
            currentState = State.Idle;
        }
    }

    void Die()
    {
        animator.SetFloat("moveAmount", 1f);
        Debug.Log("State: Die");

        if (attacker != null)
        {
            // Notify the attacker about the kill (if needed)
            Debug.Log($"{attacker.playerName} killed {playerName}!");

            DateTime now = DateTime.Now;
            TimeSpan timeDifference = now - lastDamageTime;
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

            string attackerName = attacker.playerName;
            string message = $"{playerName} is killed by {attackerName}";


            UnitEvents.RaiseUnitDie(playerName, message, DateTime.Now,playerType );
        }

        // Perform death logic here (e.g., disable unit, play death animation)
        Destroy(gameObject, 0.5f); // Destroy after 0.5 seconds to allow death animation to play
    }

    void SearchForTarget()
    {
        GameObject[] enemyUnits = GameObject.FindGameObjectsWithTag(GetEnemyTag());
        float closestDistance = detectionRadius;
        GameObject closestUnit = null;

        foreach (GameObject enemy in enemyUnits)
        {
            float distance = Vector3.Distance(transform.position, enemy.transform.position);

            if (distance <= detectionRadius)
            {
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestUnit = enemy;
                }
            }
        }

        if (closestUnit != null)
        {
            targetUnit = closestUnit;
            currentState = State.Run;
            Debug.Log($"Target found: {targetUnit.tag}");
        }
        else
        {
            targetUnit = null;
            currentState = State.Idle;
            Debug.Log("No targets found, switching to Idle state.");
        }
    }

    void MoveTowardsTarget(float speed)
    {
        if (targetUnit == null)
        {
            return;
        }

        // Check distance to the target
        float distanceToTarget = Vector3.Distance(transform.position, targetUnit.transform.position);

        if (distanceToTarget > attackRange)
        {
            Vector3 targetPosition = targetUnit.transform.position;
            Vector3 directionToTarget = (targetPosition - transform.position).normalized;
            float distanceToMove = Mathf.Min(speed * Time.deltaTime, distanceToTarget - attackRange);

            // Check for collisions with other units of the same type
            Vector3 newPosition = transform.position + directionToTarget * distanceToMove;
            Collider[] colliders = Physics.OverlapSphere(newPosition, minDistanceBetweenSameUnits);

            bool collisionDetected = false;
            Vector3 avoidanceDirection = Vector3.zero;

            foreach (Collider collider in colliders)
            {
                if (collider.gameObject != gameObject && collider.CompareTag(gameObject.tag))
                {
                    collisionDetected = true;
                    avoidanceDirection += transform.position - collider.transform.position;
                }
            }

            if (collisionDetected)
            {
                newPosition = transform.position + avoidanceDirection.normalized * distanceToMove;
            }
            else
            {
                newPosition = transform.position + directionToTarget * distanceToMove;
            }

            transform.position = newPosition;
        }
    }

    void FaceTarget()
    {
        if (targetUnit != null)
        {
            Vector3 direction = (targetUnit.transform.position - transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f); // Smoothly rotate towards the target
        }
    }

    string GetEnemyTag()
    {
        return gameObject.tag == "RedUnit" ? "BlueUnit" : "RedUnit";
    }

    public void TakeDamage(float damage, Player attacker)
    {
        if (currentState == State.Die)
        {
            return; // Ignore damage if already dead
        }

        this.attacker = attacker; // Set the attacker
        health -= damage;
        health = Mathf.Max(health, 0); // Ensure health does not go below zero

        DateTime now = DateTime.Now;
        TimeSpan timeDifference = now - lastDamageTime;
        lastDamageTime = now;
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

        Debug.Log("TakeDamage: " + gameObject.name + " Health: " + health);
        string message = $"{playerName}'s health is deducted to {health}";
        Debug.LogError("Message: " + message);
        Debug.LogError("Time: " + timeAgo);
        UnitEvents.UpdateUnitHealth(playerName, message, DateTime.Now, playerType);

        if (health <= 0)
        {
            currentState = State.Die;
            Die();
        }
    }

    void OnDrawGizmosSelected()
    {
        if (targetUnit != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, targetUnit.transform.position);
        }
    }

    public void DealDamage()
    {
        if (targetUnit != null && currentState != State.Die)
        {
            targetUnit.GetComponent<Player>().TakeDamage(damage, this); // Pass self as the attacker
            Debug.Log($"{gameObject.tag} unit attacking {targetUnit.tag} unit!");
        }
    }

    public void SetPlayerDetails(string playerName, UnitType unitType)
    {
        this.playerName = playerName;
        this.playerType = unitType;
    }
}
