using System;
using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

[CustomEditor(typeof(WheelCollider))]
public class WheelColliderEditor : ColliderEditor {
	
	private WheelCollider t;
	
	
	public void OnSceneGUI ()
	{
		t = (WheelCollider)target;
		Transform trans = t.transform;
		
		SetupHandlesMatrix ();
		
		Handles.color = selectedOutline;
		
		// Radius
		float scale = ColliderEditorUtilities.LargestComponent (trans.localScale);
		Vector3 control = ScaleHandle (t.center + trans.position + new Vector3 (0, t.radius, 0));
		float tempRadius = ((control - t.center) / scale).y;
		
		// FIXME scaling doesn't work correctly
		
		
		// Center
		Vector3 tempCenter = Handles.FreeMoveHandle (t.center, Quaternion.identity, HandleUtility.GetHandleSize (t.center) * 0.05f, Vector3.zero, Handles.DotCap);
		
		t.radius = tempRadius;
		t.center = tempCenter;
		
		Handles.color = selectedFill;
		Handles.DrawSolidDisc (trans.position + trans.rotation * t.center, trans.rotation * Vector3.left, t.radius);
		
		ReleaseHandlesMatrix ();
	}
	
	
//	public override void OnInspectorGUI ()
//	{
//		t = (WheelCollider)target;
//		
//		// TODO implement undo
//		
//		EditorGUILayout.BeginVertical ();
//		
//		// FIXME whenever a PhysicMaterial asset is dropped in, it creates an instance. Need to reference original, not instance.
//		t.material = (PhysicMaterial)EditorGUILayout.ObjectField ("Material", t.material, typeof(PhysicMaterial));
//		
//		EditorGUILayout.EndVertical ();
//	}
	

}
