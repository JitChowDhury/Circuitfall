using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
     public Transform currentEnemy;

     [Header("Tower Setup")] 
     [SerializeField] private Transform towerHead;

     [SerializeField] private float rotationSpeed;
     [SerializeField] private float attackRange = 2.2f;
     [SerializeField] private LayerMask whatIsEnemy;
     private void Update()
     {
          if (currentEnemy == null)
          {
              currentEnemy= FindRandomEnemiesWithinTarget();
               return;
          }
          if (Vector3.Distance(currentEnemy.position, transform.position) > attackRange) currentEnemy = null;
          RotateTowardsEnemy();
     }

     private void RotateTowardsEnemy()
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

     private Transform FindRandomEnemiesWithinTarget()
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
     
     private void OnDrawGizmos()
     {
          Gizmos.DrawWireSphere(transform.position,attackRange);
     }
}
