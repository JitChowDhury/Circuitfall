using System;
using System.Collections;
using UnityEngine;

public class CrossbowVisuals : MonoBehaviour
{
    private TowerCrossbow myTower;
    
    [SerializeField] private LineRenderer attackVisuals;
    [SerializeField] private float attackVisualDuration = .1f;

    [Header("Glowing Visuals")] 
    [SerializeField] private MeshRenderer meshRenderer;
    [SerializeField] private Material material;
    
    [Space]
    [SerializeField] private float currentIntensity;
    [SerializeField] private float maxIntensity=150;
    
    

    private void Awake()
    {
        myTower = GetComponent<TowerCrossbow>();
        material = new Material(meshRenderer.material);//creates a new instance of the material
        meshRenderer.material = material;//assigns it to material 
        StartCoroutine(ChangeEmissionRoutine(3));
    }

    public void PlayAttackVFX(Vector3 startPoint, Vector3 endPoint)
    {
        StartCoroutine(VFXCoroutine(startPoint, endPoint));
    }

    private IEnumerator ChangeEmissionRoutine(float duration)
    {
        float startTime = Time.time;
        float startIntensity = 0;

        while (Time.time - startTime < duration)
        {
            //calculates the proportion of the duration that has elapsed since the start of the couroutine;
            float tValue = (Time.time - startTime) / duration;
            currentIntensity = Mathf.Lerp(startIntensity, maxIntensity, tValue);
            yield return null;
            currentIntensity = maxIntensity;
        }
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
