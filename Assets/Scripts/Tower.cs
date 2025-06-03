using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Tower : MonoBehaviour
{
    public Enemy currentEnemy;

    [Header("Tower Setup")] 
    [SerializeField] protected Transform towerHead;

    [SerializeField] protected EnemyType enemyPriorityType = EnemyType.None;
    [SerializeField] protected float rotationSpeed = 10f;
    [SerializeField] protected float attackRange = 2.5f;
    [SerializeField] protected LayerMask whatIsEnemy;
    [SerializeField] protected float attackCooldown = 1;

    [Space] [Tooltip("Enabling this allows tower to change target between attack")] [SerializeField]
    private bool dynamicTargetChange;
    private float targetCheckInterval = .1f;
    private float lastTimeCheckedTarget;

    private bool canRotate;
    protected float lastTimeAttacked;

    protected virtual void Awake()
    {
        EnableRotation(true);//enables rotation so that tower head can turn to face enemies
    }

    protected virtual void Update()
    {
        UpdateTargetIfNeeded();
        if (currentEnemy == null)
        {
            currentEnemy = FindEnemyWithinTarget();
            return;
        }

        if (CanAttack()) Attack();
        if (Vector3.Distance(currentEnemy.CentrePoint(), transform.position) > attackRange) currentEnemy = null;
        RotateTowardsEnemy();
    }

    private void UpdateTargetIfNeeded()
    {
        if(dynamicTargetChange==false)return;
        if (Time.time > lastTimeCheckedTarget + targetCheckInterval)
        {
            lastTimeCheckedTarget = Time.time;
            currentEnemy = FindEnemyWithinTarget();
        }
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
        var directionToEnemy = DirectionToEnemyFrom(towerHead);
        //create a Quaternion for the rotation towards the direction of enemy
        var lookRotation = Quaternion.LookRotation(directionToEnemy);
        //smoothly rotates the tower head to the enemy
        var rotation = Quaternion.Lerp(towerHead.rotation, lookRotation, rotationSpeed * Time.deltaTime)
            .eulerAngles;
        //converts euler to quaternion for unity to understand easily
        towerHead.rotation = Quaternion.Euler(rotation);
    }

    protected Enemy  FindEnemyWithinTarget()
    {
        List<Enemy> priorityTarget = new List<Enemy>();//stores enemies of the type specified
        List<Enemy> possibleTargets = new List<Enemy>();//stores all other enemies
        Collider[] enemiesAround = Physics.OverlapSphere(transform.position, attackRange, whatIsEnemy);
        foreach (Collider enemy in enemiesAround)
        {
            Enemy newEnemy = enemy.GetComponent<Enemy>();
            EnemyType newEnemyType = newEnemy.GetEnemyType();
            if (newEnemyType == enemyPriorityType)
            {
                priorityTarget.Add(newEnemy);
            }
            else
            {
                possibleTargets.Add(newEnemy);
            }
        }
        Enemy newTarget = GetMostAdvancedenemy(possibleTargets);
        if (priorityTarget.Count > 0)
        {
            newTarget = GetMostAdvancedenemy(priorityTarget);
        }
        if (newTarget != null)
        {
            return newTarget;
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
        return (currentEnemy.CentrePoint() - startPos.position).normalized;
    }
}