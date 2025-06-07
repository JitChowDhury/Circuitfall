using System.Collections.Generic;
using UnityEngine;

public class TileSlot : MonoBehaviour
{
 private MeshRenderer meshRenderer => GetComponent<MeshRenderer>();
 private MeshFilter meshFilter => GetComponent<MeshFilter>();
 public void switchTile(GameObject referenceTile)
 {
  TileSlot newTile = referenceTile.GetComponent<TileSlot>();
  meshFilter.mesh = newTile.GetMesh();//set information
  meshRenderer.material = newTile.GetMaterial();

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

 public List<GameObject> GetAllChildren()
 {
  List<GameObject> children = new List<GameObject>();
  foreach (Transform child in transform)//get all the child objects and add it to the list
  {
   children.Add(child.gameObject);
  }

  return children;
 }
}
