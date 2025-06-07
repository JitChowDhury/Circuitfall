using UnityEngine;

public class TowerCrossbow : Tower
{
    [Header("CrossBow Details")]
    [SerializeField] private int damage;
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
            Enemy enemyTarget = null;
            IDamagable damagable = hitInfo.transform.GetComponent<IDamagable>();
            if (damagable != null)
            {
                damagable.TakeDamage(damage);
                enemyTarget = currentEnemy;
            }


            visuals.PlayAttackVFX(gunPoint.position,hitInfo.point,enemyTarget);
            visuals.PlayReloadVFX((attackCooldown));
        }
    }
}
