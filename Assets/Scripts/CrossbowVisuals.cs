using System;
using System.Collections;
using UnityEngine;

public class CrossbowVisuals : MonoBehaviour
{
    private TowerCrossbow myTower;
    private Enemy myEnemy;
    
    [SerializeField] private LineRenderer attackVisuals;
    [SerializeField] private float attackVisualDuration = .1f;

    [Header("Glowing Visuals")] 
    [SerializeField] private MeshRenderer meshRenderer;
    private Material material;
    
    [Space]
    private float currentIntensity;
    [SerializeField] private float maxIntensity=150;
    [Space]
    [SerializeField] private Color startColor;
    [SerializeField] private Color endColor;
    
    [Header("Rotor Visuals")]
    [SerializeField] private Transform rotor;
    [SerializeField] private Transform rotorunloaded;
    [SerializeField] private Transform rotorloaded;

    
    [Header("Front Glow String")]
    [SerializeField]   private LineRenderer frontString_L;
    [SerializeField]   private LineRenderer frontString_R;
    [Space]
    [SerializeField] private Transform frontStartPoint_L;
    [SerializeField] private Transform frontStartPoint_R;
    [SerializeField] private Transform frontEndPoint_L;
    [SerializeField] private Transform frontEndPoint_R;
    [Space]
    [Header("Back Glow String")]
    [SerializeField]   private LineRenderer backString_L;
    [SerializeField]   private LineRenderer backString_R;
    [Space]
    [SerializeField] private Transform backStartPoint_L;
    [SerializeField] private Transform backStartPoint_R;
    [SerializeField] private Transform backEndPoint_L;
    [SerializeField] private Transform backEndPoint_R;

    [SerializeField] private LineRenderer[] lineRenderers;
    

    private void Awake()
    {
        myTower = GetComponent<TowerCrossbow>();
        material = new Material(meshRenderer.material);//creates a new instance of the material
        meshRenderer.material = material;//assigns it to material 

        UpdateMaterialsOnLineRenderer();

        StartCoroutine(ChangeEmissionRoutine(1));

    }

    private void UpdateMaterialsOnLineRenderer()
    {
        foreach (var lr in lineRenderers)
        {
            lr.material = material;
        }
    }

    private void Update()
    {
        UpdateEmissionColor();
        UpdateStrings();
        
         UpdateAttackVisualIfNeeded();
    }

    private void UpdateAttackVisualIfNeeded()
    {
        if(attackVisuals.enabled && myEnemy!=null)
            attackVisuals.SetPosition(1,myEnemy.CentrePoint());
    }

    private void UpdateStrings()
    {
        UpdateStringVisual(frontString_R,frontStartPoint_R,frontEndPoint_R);
        UpdateStringVisual(frontString_L,frontStartPoint_L,frontEndPoint_L);        
        
        UpdateStringVisual(backString_R,backStartPoint_R,backEndPoint_R);
        UpdateStringVisual(backString_L,backStartPoint_L,backEndPoint_L);
    }

    public void PlayReloadVFX(float duration)
    {
        float newDuration = duration / 2;
        StartCoroutine(ChangeEmissionRoutine(newDuration));
        StartCoroutine(UpdateRotorPosRoutine(newDuration));
    }
    private void UpdateEmissionColor()
    {
        Color emissionColor = Color.Lerp(startColor, endColor, currentIntensity / maxIntensity);
        emissionColor = emissionColor * Mathf.LinearToGammaSpace(currentIntensity);
        material.SetColor("_EmissionColor",emissionColor);
    }
 
    public void PlayAttackVFX(Vector3 startPoint, Vector3 endPoint,Enemy newEnemy)
    {
        StartCoroutine(VFXCoroutine(startPoint, endPoint,newEnemy));
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

    private IEnumerator UpdateRotorPosRoutine(float duration)
    {
        float startTime = Time.time;
        while (Time.time - startTime < duration)
        {
            float tValue = (Time.time - startTime) / duration;
            rotor.position = Vector3.Lerp(rotorunloaded.position, rotorloaded.position, tValue);
            yield return null;
        }

        rotor.position = rotorloaded.position;
    }
    private IEnumerator VFXCoroutine(Vector3 startPoint, Vector3 endPoint,Enemy newEnemy)
    {   //myTower.EnableRotation(false);
        myEnemy = newEnemy;
        
        attackVisuals.enabled = true;    
        attackVisuals.SetPosition(0,startPoint);
        attackVisuals.SetPosition(1,endPoint);
        
        yield return new WaitForSeconds(attackVisualDuration);
        attackVisuals.enabled = false;
        //myTower.EnableRotation(true);
    }

    private void UpdateStringVisual(LineRenderer lineRenderer, Transform startPoint, Transform endPoint)
    {
        lineRenderer.SetPosition(0,startPoint.position);
        lineRenderer.SetPosition(1,endPoint.position);
    }
}
