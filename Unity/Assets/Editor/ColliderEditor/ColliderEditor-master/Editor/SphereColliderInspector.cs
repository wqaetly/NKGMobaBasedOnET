using System;
using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

[CustomEditor(typeof(SphereCollider))]
public class SphereColliderInspector : ColliderEditor {
	
	private SphereCollider t;
	
	
	private void OnEnable ()
	{
		CreateFill ();
		UpdateFill (true);
	}


	private void OnDisable ()
	{
		ReleaseFill ();
	}
	
	
	private void OnSceneGUI ()
	{
		t = (SphereCollider)target;
		SetupUndo ();
		SetupHandlesMatrix (true);
		
		Handles.color = new Color (0.6f, 1, 0.6f);
		
		Transform trans = t.transform;
		
		float scale = ColliderEditorUtilities.LargestComponent (trans.localScale);
		
		// Center control
		Vector3 tempCenter = MoveHandle (t.center);
		
		// Radius controls
		float x1 = ScaleHandle (t.center + new Vector3 (t.radius, 0, 0)).x;
		float x2 = ScaleHandle (t.center + new Vector3 (-t.radius, 0, 0)).x;
		float y1 = ScaleHandle (t.center + new Vector3 (0, t.radius, 0)).y;
		float y2 = ScaleHandle (t.center + new Vector3 (0, -t.radius, 0)).y;
		float z1 = ScaleHandle (t.center + new Vector3 (0, 0, t.radius)).z;
		float z2 = ScaleHandle (t.center + new Vector3 (0, 0, -t.radius)).z;
	
		float tempRadius = (
			(x1 - t.center.x) + (y1 - t.center.y) + (z1 - t.center.z) +
			(-x2 + t.center.x) + (-y2 + t.center.y) + (-z2 + t.center.z)
			) / 6;
	
		t.radius = Mathf.Abs (tempRadius);
		t.center = tempCenter;
	
		UpdateFill (true);
		ReleaseHandlesMatrix ();
	}
	
	
	#region Inspector GUI
	
	public override void OnInspectorGUI ()
	{
		t = (SphereCollider)target;
		
		// TODO implement undo
		
		EditorGUILayout.BeginVertical ();
		
		// FIXME whenever a PhysicMaterial asset is dropped in, it creates an instance. Need to reference original, not instance.
		PhysicMaterial material = (PhysicMaterial)EditorGUILayout.ObjectField ("Material", t.material, typeof(PhysicMaterial));
		t.material = material;
		
		t.isTrigger = EditorGUILayout.Toggle ("Is Trigger", t.isTrigger);
		
		t.radius = EditorGUILayout.FloatField ("Radius", t.radius);
		if (t.radius < 0)
			t.radius = 0;
		
		t.center = EditorGUILayout.Vector3Field ("Center", t.center);
		
		EditorGUILayout.EndVertical ();
	}
	
	#endregion
	
}
