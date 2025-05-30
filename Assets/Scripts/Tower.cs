using System;
using UnityEngine;

public class Tower : MonoBehaviour
{
     public Transform currentEnemy;

     [Header("Tower Setup")] 
     [SerializeField] private Transform towerHead;

     [SerializeField] private float rotationSpeed;

     private void Update()
     {
          RotateTowardsEnemy();
     }

     private void RotateTowardsEnemy()
     {
          //calculate the vector direction from the tower's head to the current enemy
          Vector3 directionToEnemy = currentEnemy.position - towerHead.position;
          //create a Quaternion for the rotation towards the direction of enemy
          Quaternion lookRotation = Quaternion.LookRotation(directionToEnemy);
          //smoothly rotates the towerhead to the enemy
          Vector3 rotation = Quaternion.Lerp(towerHead.rotation, lookRotation, rotationSpeed * Time.deltaTime)
               .eulerAngles;
          //converts euler to quaternion for unity to understand easily
          towerHead.rotation=Quaternion.Euler(rotation);
     }
}
