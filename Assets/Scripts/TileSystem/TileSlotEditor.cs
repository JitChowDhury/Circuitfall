using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(TileSlot)),CanEditMultipleObjects]//customizes unity inspector for tileslot component
public class TileSlotEditor : Editor
{
   private GUIStyle centeredStyle;
   
   public override void OnInspectorGUI()//for custom inspector 
   {
      serializedObject.Update();
      base.OnInspectorGUI();

      centeredStyle = new GUIStyle(GUI.skin.label)
      {
          alignment = TextAnchor.MiddleCenter,
          fontStyle = FontStyle.Bold,
          fontSize = 14
      };
      
      float oneButtonWidth = (EditorGUIUtility.currentViewWidth - 25);
      float twoButtonWidth = (EditorGUIUtility.currentViewWidth - 25) / 2;
      float threeButtonWidth = (EditorGUIUtility.currentViewWidth - 25) / 3;
      
      GUILayout.Label("Position and Rotation",centeredStyle);
      
      GUILayout.BeginHorizontal();//Groups buttons horizontally.
      
      if (GUILayout.Button("Rotate Left", GUILayout.Width(twoButtonWidth)))
      {
    
         foreach (var targetTile in targets)
         {
            ((TileSlot)targetTile).RotateTile(-1);
         }
      }
      
      if (GUILayout.Button("Rotate Right", GUILayout.Width(twoButtonWidth)))
      {

         foreach (var targetTile in targets)
         {
            ((TileSlot)targetTile).RotateTile(1);
         }
      }
      
      GUILayout.EndHorizontal();
      
      GUILayout.BeginHorizontal();//Groups buttons horizontally.
      
      if (GUILayout.Button("-.1f in the Y", GUILayout.Width(twoButtonWidth)))
      {
    
         foreach (var targetTile in targets)
         {
            ((TileSlot)targetTile).ADjustY(-1);
         }
      }
      
      if (GUILayout.Button(".1f in the Y", GUILayout.Width(twoButtonWidth)))
      {

         foreach (var targetTile in targets)
         {
            ((TileSlot)targetTile).ADjustY(1);
         }
      }
      
      GUILayout.EndHorizontal();
      
      
      GUILayout.Label("Tile Options",centeredStyle);
      
      GUILayout.BeginHorizontal();//Groups buttons horizontally.
      
      if (GUILayout.Button("Field", GUILayout.Width(twoButtonWidth)))
      {
         GameObject newTile = FindFirstObjectByType<TileSetHolder>().tileField;
         foreach (var targetTile in targets)
         {
               ((TileSlot)targetTile).switchTile(newTile);
         }
      }
      
      if (GUILayout.Button("Road", GUILayout.Width(twoButtonWidth)))
      {
         GameObject newTile = FindFirstObjectByType<TileSetHolder>().tileRoad;
         foreach (var targetTile in targets)
         {
            ((TileSlot)targetTile).switchTile(newTile);
         }
      }
      
      GUILayout.EndHorizontal();
      
      GUILayout.BeginHorizontal();
      
      if (GUILayout.Button("Sideway", GUILayout.Width(oneButtonWidth)))
      {
         GameObject newTile = FindFirstObjectByType<TileSetHolder>().tileSideway;
         foreach (var targetTile in targets)
         {
            ((TileSlot)targetTile).switchTile(newTile);
         }
      }
      GUILayout.EndHorizontal();
      
      GUILayout.Label("Corner Options",centeredStyle);
      
      GUILayout.BeginHorizontal();//Groups buttons horizontally.
      
      if (GUILayout.Button("Inner Corner", GUILayout.Width(twoButtonWidth)))
      {
         GameObject newTile = FindFirstObjectByType<TileSetHolder>().tileInnerCorner;
         foreach (var targetTile in targets)
         {
               ((TileSlot)targetTile).switchTile(newTile);
         }
      }
      
      if (GUILayout.Button("Outer Corner", GUILayout.Width(twoButtonWidth)))
      {
         GameObject newTile = FindFirstObjectByType<TileSetHolder>().tileOuterCorner;
         foreach (var targetTile in targets)
         {
            ((TileSlot)targetTile).switchTile(newTile);
         }
      }
      
      GUILayout.EndHorizontal();
      
      GUILayout.BeginHorizontal();//Groups buttons horizontally.
      
      if (GUILayout.Button("Inner Corner Small", GUILayout.Width(twoButtonWidth)))
      {
         GameObject newTile = FindFirstObjectByType<TileSetHolder>().tileInnerCornerSmall;
         foreach (var targetTile in targets)
         {
            ((TileSlot)targetTile).switchTile(newTile);
         }
      }
      
      if (GUILayout.Button("Outer Corner Small", GUILayout.Width(twoButtonWidth)))
      {
         GameObject newTile = FindFirstObjectByType<TileSetHolder>().tileOuterCornerSmall;
         foreach (var targetTile in targets)
         {
            ((TileSlot)targetTile).switchTile(newTile);
         }
      }
      
      GUILayout.EndHorizontal();
      
      GUILayout.Label("Bridges and Hills",centeredStyle);
      
      GUILayout.BeginHorizontal();//Groups buttons horizontally.
      
      if (GUILayout.Button("Hill 1", GUILayout.Width(threeButtonWidth)))
      {
         GameObject newTile = FindFirstObjectByType<TileSetHolder>().tileHill_1;
         foreach (var targetTile in targets)
         {
            ((TileSlot)targetTile).switchTile(newTile);
         }
      }
      
      if (GUILayout.Button("Hill 2", GUILayout.Width(threeButtonWidth)))
      {
         GameObject newTile = FindFirstObjectByType<TileSetHolder>().tileHill_2;
         foreach (var targetTile in targets)
         {
            ((TileSlot)targetTile).switchTile(newTile);
         }
      }
      if (GUILayout.Button("Hill 3", GUILayout.Width(threeButtonWidth)))
      {
         GameObject newTile = FindFirstObjectByType<TileSetHolder>().tileHill_3;
         foreach (var targetTile in targets)
         {
            ((TileSlot)targetTile).switchTile(newTile);
         }
      }
      
      GUILayout.EndHorizontal();
      
      GUILayout.BeginHorizontal();//Groups buttons horizontally.
      
      if (GUILayout.Button("Bridge With road", GUILayout.Width(threeButtonWidth)))
      {
         GameObject newTile = FindFirstObjectByType<TileSetHolder>().tileBridgeRoad;
         foreach (var targetTile in targets)
         {
            ((TileSlot)targetTile).switchTile(newTile);
         }
      }
      
      if (GUILayout.Button("Bridge with Field", GUILayout.Width(threeButtonWidth)))
      {
         GameObject newTile = FindFirstObjectByType<TileSetHolder>().tileBridgeField;
         foreach (var targetTile in targets)
         {
            ((TileSlot)targetTile).switchTile(newTile);
         }
      }
      if (GUILayout.Button("Bridge Sideway", GUILayout.Width(threeButtonWidth)))
      {
         GameObject newTile = FindFirstObjectByType<TileSetHolder>().tileBridgeSideway;
         foreach (var targetTile in targets)
         {
            ((TileSlot)targetTile).switchTile(newTile);
         }
      }
      
      GUILayout.EndHorizontal();
   }
}

