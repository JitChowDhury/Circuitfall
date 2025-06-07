using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(TileSlot)),CanEditMultipleObjects]//customizes unity inspector for tileslot component
public class TileSlotEditor : Editor
{
   public override void OnInspectorGUI()//for custom inspector 
   {
      serializedObject.Update();
      base.OnInspectorGUI();
      float buttonWidth = (EditorGUIUtility.currentViewWidth - 25) / 2;
      
      GUILayout.BeginHorizontal();//Groups buttons horizontally.
      
      if (GUILayout.Button("Field", GUILayout.Width(buttonWidth)))
      {
         GameObject newTile = FindFirstObjectByType<TileSetHolder>().tileField;
         foreach (var targetTile in targets)
         {
               ((TileSlot)targetTile).switchTile(newTile);
         }
      }
      
      if (GUILayout.Button("Road", GUILayout.Width(buttonWidth)))
      {
         GameObject newTile = FindFirstObjectByType<TileSetHolder>().tileRoad;
         foreach (var targetTile in targets)
         {
            ((TileSlot)targetTile).switchTile(newTile);
         }
      }
      
      GUILayout.EndHorizontal();
      
      GUILayout.BeginHorizontal();
      
      if (GUILayout.Button("Sideway", GUILayout.Width(buttonWidth*2)))
      {
         GameObject newTile = FindFirstObjectByType<TileSetHolder>().tileSideway;
         foreach (var targetTile in targets)
         {
            ((TileSlot)targetTile).switchTile(newTile);
         }
      }
      GUILayout.EndHorizontal();
   }
}
