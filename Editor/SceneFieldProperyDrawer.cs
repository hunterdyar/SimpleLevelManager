using System;
 using UnityEngine;
 using Object = UnityEngine.Object;
 #if UNITY_EDITOR
 using UnityEditor;
using UnityEngine.SceneManagement;

#endif
 
 namespace Bloops.LevelManager
 {

#if UNITY_EDITOR
     [CustomPropertyDrawer(typeof(SceneField))]
     public class SceneFieldPropertyDrawer : PropertyDrawer
     {
         public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
         {
             EditorGUI.BeginProperty(position, GUIContent.none, property);
             var sceneAsset = property.FindPropertyRelative("sceneAsset");
             var sceneName = property.FindPropertyRelative("sceneName");
             var scenePath = property.FindPropertyRelative("scenePath");
             var buildIndex = property.FindPropertyRelative("buildIndex");

             position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);
             if (sceneAsset != null)
             {
                 EditorGUI.BeginChangeCheck();
                 var value = EditorGUI.ObjectField(position, sceneAsset.objectReferenceValue, typeof(SceneAsset), false);
                 if (EditorGUI.EndChangeCheck())
                 {
                     sceneAsset.objectReferenceValue = value;
                     if (sceneAsset.objectReferenceValue != null)
                     {
                         var scenePathA = AssetDatabase.GetAssetPath(sceneAsset.objectReferenceValue);
                         var assetsIndex = scenePathA.IndexOf("Assets", StringComparison.Ordinal) + 7;
                         var slashIndex = scenePathA.LastIndexOf("/", StringComparison.Ordinal) + 7;
                         var extensionIndex = scenePathA.LastIndexOf(".unity", StringComparison.Ordinal);
                         scenePath.stringValue = scenePathA;
                         sceneName.stringValue = scenePathA.Substring(slashIndex, extensionIndex - slashIndex);
                         //
                         buildIndex.intValue = SceneUtility.GetBuildIndexByScenePath(scenePathA);

                     }
                 }
             }
             EditorGUI.EndProperty();
         }
     }
 #endif
 }