using System;
using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

[CustomEditor(typeof(CapsuleCollider))]
public class CapsuleColliderEditor : ColliderEditor {
	
	private CapsuleCollider t;
	
	private void OnEnable ()
	{
		CreateFill ();
		UpdateFill ();
	}


	private void OnDisable ()
	{
		ReleaseFill ();
	}
	
	
	
	
	public void OnSceneGUI ()
	{
		t = (CapsuleCollider)target;
		Transform trans = t.transform;
		
		Matrix4x4 oldMatrix = Handles.matrix;
		Handles.matrix = trans.localToWorldMatrix;
		
		Handles.color = selectedOutline;
		
		
		// Center control
//		Vector3 tempCenter = Handles.FreeMoveHandle (t.center, Quaternion.identity, HandleUtility.GetHandleSize (t.center) * 0.05f, Vector3.zero, Handles.DotCap);
		
		
		// Resize controls
//		float xExtent = ScaleHandle (t.center + new Vector3 (t.extents.x, 0, 0)).x;
//		float xExtent2 = ScaleHandle (t.center + new Vector3 (-t.extents.x, 0, 0)).x;
//		float yExtent = ScaleHandle (t.center + new Vector3 (0, t.extents.y, 0)).y;
//		float yExtent2 = ScaleHandle (t.center + new Vector3 (0, -t.extents.y, 0)).y;
//		float zExtent = ScaleHandle (t.center + new Vector3 (0, 0, t.extents.z)).z;
//		float zExtent2 = ScaleHandle (t.center + new Vector3 (0, 0, -t.extents.z)).z;
//		
//		Vector3 tempSize = new Vector3 (Mathf.Abs (xExtent - xExtent2), Mathf.Abs (yExtent - yExtent2), Mathf.Abs (zExtent - zExtent2));
//		
//		
//		// Only update if controls have changed
//		if (tempCenter != t.center || tempSize != t.size) {
//			if (lastControl != GUIUtility.hotControl) {
//				// FIXME Undo should be able to be set again once the mouse is released.
//				// Currently, another control must be touched first before RegisterUndo is called again.
//				Undo.RegisterUndo (t, "Undo Modify Collider");
//			}
//			t.center = tempCenter;
//			t.size = tempSize;
//			lastControl = GUIUtility.hotControl;
//		}
		
		
		UpdateFill ();
		
		Handles.matrix = oldMatrix;
	}
	
	
	#region Inspector GUI
	
//	public override void OnInspectorGUI ()
//	{
//		t = (CapsuleCollider)target;	
//		
//		// TODO implement undo
//		
//		EditorGUILayout.BeginVertical ();
//		
//		// FIXME whenever a PhysicMaterial asset is dropped in, it creates an instance. Need to reference original, not instance.
//		PhysicMaterial material = (PhysicMaterial)EditorGUILayout.ObjectField ("Material", t.material, typeof(PhysicMaterial));
//		t.material = material;
//		
//		t.isTrigger = EditorGUILayout.Toggle ("Is Trigger", t.isTrigger);
//		
//		t.radius = EditorGUILayout.FloatField ("Radius", t.radius);
//		if (t.radius < 0)
//			t.radius = 0;
//		
//		t.center = EditorGUILayout.Vector3Field ("Center", t.center);
//		
//		EditorGUILayout.EndVertical ();
//	}
	
	#endregion
	
}
