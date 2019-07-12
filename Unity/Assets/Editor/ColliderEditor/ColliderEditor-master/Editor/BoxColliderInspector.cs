using System;
using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

[CustomEditor(typeof(BoxCollider))]
public class BoxColliderInspector : ColliderEditor {
	
	private BoxCollider t;

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
		t = (BoxCollider)target;
		SetupUndo ();
		SetupHandlesMatrix ();
		
		Handles.color = selectedOutline;
		
		// Center control
		Vector3 tempCenter = MoveHandle (t.center);
		
		// Resize controls
		float xExtent = ScaleHandle (t.center + new Vector3 (t.extents.x, 0, 0)).x;
		float xExtent2 = ScaleHandle (t.center + new Vector3 (-t.extents.x, 0, 0)).x;
		float yExtent = ScaleHandle (t.center + new Vector3 (0, t.extents.y, 0)).y;
		float yExtent2 = ScaleHandle (t.center + new Vector3 (0, -t.extents.y, 0)).y;
		float zExtent = ScaleHandle (t.center + new Vector3 (0, 0, t.extents.z)).z;
		float zExtent2 = ScaleHandle (t.center + new Vector3 (0, 0, -t.extents.z)).z;
		
		Vector3 tempSize = new Vector3 (
			Mathf.Abs (xExtent - xExtent2),
			Mathf.Abs (yExtent - yExtent2),
			Mathf.Abs (zExtent - zExtent2)
			);
		
		t.center = tempCenter;
		t.size = tempSize;
		
		UpdateFill ();
		ReleaseHandlesMatrix ();
	}
	
	
	public override void OnInspectorGUI ()
	{
		t = (BoxCollider)target;
		
		// TODO implement undo
		
		EditorGUILayout.BeginVertical ();
		
		// FIXME whenever a PhysicMaterial asset is dropped in, it creates an instance. Need to reference original, not instance.
		PhysicMaterial material = (PhysicMaterial)EditorGUILayout.ObjectField ("Material", t.material, typeof(PhysicMaterial));
		t.material = material;
		
		t.isTrigger = EditorGUILayout.Toggle ("Is Trigger", t.isTrigger);
		
		t.size = EditorGUILayout.Vector3Field ("Size", t.size);
		
		t.center = EditorGUILayout.Vector3Field ("Center", t.center);
		
		EditorGUILayout.EndVertical ();
	}
	
}
