using UnityEngine;

public class TowerCrossbow : Tower
{
    [Header("CrossBow Details")]
    [SerializeField] private Transform gunPoint;


    protected override void Attack()
    {
        Vector3 direction = DirectionToEnemyFrom(gunPoint);
        towerHead.forward = direction;
        if (Physics.Raycast(gunPoint.position, direction, out RaycastHit hitInfo, Mathf.Infinity))
        {
            Debug.DrawLine(gunPoint.position,hitInfo.point);
        }
    }
}
