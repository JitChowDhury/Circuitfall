using System;
using System.Collections;
using UnityEngine;

public class CrossbowVisuals : MonoBehaviour
{
    private TowerCrossbow myTower;
    
    [SerializeField] private LineRenderer attackVisuals;
    [SerializeField] private float attackVisualDuration = .1f;

    private void Awake()
    {
        myTower = GetComponent<TowerCrossbow>();
    }

    public void PlayAttackVFX(Vector3 startPoint, Vector3 endPoint)
    {
        StartCoroutine(VFXCoroutine(startPoint, endPoint));
    }

    private IEnumerator VFXCoroutine(Vector3 startPoint, Vector3 endPoint)
    {   myTower.EnableRotation(false);
        
        attackVisuals.enabled = true;    
        attackVisuals.SetPosition(0,startPoint);
        attackVisuals.SetPosition(1,endPoint);
        
        yield return new WaitForSeconds(attackVisualDuration);
        attackVisuals.enabled = false;
        myTower.EnableRotation(true);
    }
}
