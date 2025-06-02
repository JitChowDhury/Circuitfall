using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Tower : MonoBehaviour
{
    public Transform currentEnemy;

    [Header("Tower Setup")] [SerializeField]
    protected Transform towerHead;

    [SerializeField] protected float rotationSpeed = 10f;
    [SerializeField] protected float attackRange = 2.5f;
    [SerializeField] protected LayerMask whatIsEnemy;
    [SerializeField] protected float attackCooldown = 1;

    private bool canRotate;
    protected float lastTimeAttacked;

    protected virtual void Awake()
    {
    }

    protected virtual void Update()
    {
        if (currentEnemy == null)
        {
            currentEnemy = FindRandomEnemiesWithinTarget();
            return;
        }

        if (CanAttack()) Attack();
        if (Vector3.Distance(currentEnemy.position, transform.position) > attackRange) currentEnemy = null;
        RotateTowardsEnemy();
    }

    protected virtual void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }

    protected bool CanAttack()
    {
        if (Time.time > lastTimeAttacked + attackCooldown)
        {
            lastTimeAttacked = Time.time;
            return true;
        }

        return false;
    }

    protected virtual void Attack()
    {
        Debug.Log("Attack performed at" + Time.time);
    }

    public void EnableRotation(bool enable)
    {
        canRotate = enable;
    }

    protected void RotateTowardsEnemy()
    {
        if (canRotate == false) return;
        if (currentEnemy == null) return;
        //calculate the vector direction from the tower's head to the current enemy
        var directionToEnemy = currentEnemy.position - towerHead.position;
        //create a Quaternion for the rotation towards the direction of enemy
        var lookRotation = Quaternion.LookRotation(directionToEnemy);
        //smoothly rotates the tower head to the enemy
        var rotation = Quaternion.Lerp(towerHead.rotation, lookRotation, rotationSpeed * Time.deltaTime)
            .eulerAngles;
        //converts euler to quaternion for unity to understand easily
        towerHead.rotation = Quaternion.Euler(rotation);
    }

    protected Transform FindRandomEnemiesWithinTarget()
    {
        List<Enemy> possibleTargets = new List<Enemy>();
        Collider[] enemiesAround = Physics.OverlapSphere(transform.position, attackRange, whatIsEnemy);
        foreach (Collider enemy in enemiesAround)
        {
            Enemy newEnemy = enemy.GetComponent<Enemy>();
            possibleTargets.Add(newEnemy);
        }


        Enemy newTarget = GetMostAdvancedenemy(possibleTargets);
        if (newTarget != null)
        {
            return newTarget.transform;
        }

        return null;

    }

    private Enemy GetMostAdvancedenemy(List<Enemy> possibleTargets)
    {
        Enemy mostAdvancedEnemy = null;
        float minRemainingdistance = float.MaxValue;

        foreach (Enemy enemy in possibleTargets)
        {
            float remainingDistance = enemy.DistanceToFinishLine();
            if (remainingDistance < minRemainingdistance)
            {
                minRemainingdistance = remainingDistance;
                mostAdvancedEnemy = enemy;
            }
        }

        return mostAdvancedEnemy;
    }

    protected Vector3 DirectionToEnemyFrom(Transform startPos)
    {
        return (currentEnemy.position - startPos.position).normalized;
    }
}