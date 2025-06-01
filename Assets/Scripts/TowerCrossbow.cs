using UnityEngine;

public class TowerCrossbow : Tower
{
    [Header("CrossBow Details")]
    [SerializeField] private Transform gunPoint;

    private CrossbowVisuals visuals;

    protected override void Awake()
    {
        base.Awake();
        visuals = GetComponent<CrossbowVisuals>();
    }
    protected override void Attack()
    {
        Vector3 direction = DirectionToEnemyFrom(gunPoint);
        if (Physics.Raycast(gunPoint.position, direction, out RaycastHit hitInfo, Mathf.Infinity))
        {
            towerHead.forward = direction;
            visuals.PlayAttackVFX(gunPoint.position,hitInfo.point);
            visuals.PlayReloadVFX((attackCooldown));
        }
    }
}
