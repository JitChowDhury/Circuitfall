using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TileSlot : MonoBehaviour
{
 private MeshRenderer meshRenderer => GetComponent<MeshRenderer>();
 private MeshFilter meshFilter => GetComponent<MeshFilter>();
 private Collider myCollider => GetComponent<Collider>();
 public void switchTile(GameObject referenceTile)
 {
  gameObject.name = referenceTile.name;
  
  TileSlot newTile = referenceTile.GetComponent<TileSlot>();
  meshFilter.mesh = newTile.GetMesh();//set information
  meshRenderer.material = newTile.GetMaterial();
  
  UpdateCollider(newTile.GetCollider());

  foreach (GameObject obj in GetAllChildren())
  {
   DestroyImmediate(obj);//destroy currents child gameobject
  }

  foreach (GameObject obj in newTile.GetAllChildren())
  {
   Instantiate(obj, transform);//get new child gameobject
  }
  
 }

 public Material GetMaterial() => meshRenderer.sharedMaterial;//get information from tile
 public Mesh GetMesh() => meshFilter.sharedMesh;//get information from tile
 public Collider GetCollider() => myCollider;

 public List<GameObject> GetAllChildren()
 {
  List<GameObject> children = new List<GameObject>();
  foreach (Transform child in transform)//get all the child objects and add it to the list
  {
   children.Add(child.gameObject);
  }

  return children;
 }

 public void UpdateCollider(Collider newCollider)
 {
  DestroyImmediate(myCollider);
  if (newCollider is BoxCollider)
  {
   BoxCollider original = newCollider.GetComponent<BoxCollider>();
   BoxCollider myNewCollider = transform.AddComponent<BoxCollider>();


   myNewCollider.center = original.center;
   myNewCollider.size = original.size;
   
  }

  if (newCollider is MeshCollider)
  {
   MeshCollider original = newCollider.GetComponent<MeshCollider>();
   MeshCollider myNewCollider = transform.AddComponent<MeshCollider>();

   myNewCollider.sharedMesh = original.sharedMesh;
   myNewCollider.convex = original.convex;
  }
 }
}
