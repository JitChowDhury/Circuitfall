using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
     public Transform currentEnemy;

     [Header("Tower Setup")] 
     [SerializeField] protected Transform towerHead;

     [SerializeField] protected float rotationSpeed=10f;
     [SerializeField] protected float attackRange = 2.5f;
     [SerializeField] protected LayerMask whatIsEnemy;
     [SerializeField] protected float attackCooldown=1;
     protected float lastTimeAttacked;
     protected virtual void Update()
     {
          
          if (currentEnemy == null)
          {
              currentEnemy= FindRandomEnemiesWithinTarget();
               return;
          }
          if(CanAttack()) Attack();
          if (Vector3.Distance(currentEnemy.position, transform.position) > attackRange) currentEnemy = null;
          RotateTowardsEnemy();
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
     protected void RotateTowardsEnemy()
     {
          if(currentEnemy==null)return;
          //calculate the vector direction from the tower's head to the current enemy
          Vector3 directionToEnemy = currentEnemy.position - towerHead.position;
          //create a Quaternion for the rotation towards the direction of enemy
          Quaternion lookRotation = Quaternion.LookRotation(directionToEnemy);
          //smoothly rotates the tower head to the enemy
          Vector3 rotation = Quaternion.Lerp(towerHead.rotation, lookRotation, rotationSpeed * Time.deltaTime)
               .eulerAngles;
          //converts euler to quaternion for unity to understand easily
          towerHead.rotation=Quaternion.Euler(rotation);
     }

     protected Transform FindRandomEnemiesWithinTarget()
     {
          List<Transform> possibleTargets = new List<Transform>();
          Collider[] enemiesAround = Physics.OverlapSphere(transform.position, attackRange,whatIsEnemy);
          foreach (Collider enemy in enemiesAround)
          {
               possibleTargets.Add(enemy.transform);
          }

          int randomIndex = Random.Range(0, possibleTargets.Count);
          if (possibleTargets.Count <= 0) return null;
          return possibleTargets[randomIndex];

     }
     
     protected virtual void OnDrawGizmos()
     {
          Gizmos.DrawWireSphere(transform.position,attackRange);
     }

     protected Vector3 DirectionToEnemyFrom(Transform startPos)
     {
          return (currentEnemy.position - startPos.position).normalized;
     }
}
