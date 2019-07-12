using System;
using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

[CustomEditor(typeof(MeshCollider))]
public class MeshColliderInspector : Editor {
	
	private MeshCollider t;
	
	private static GameObject mesh;
	private static MeshFilter mf;
	private static Material mat;
	
//	private Color selectedOutline = new Color (0.6f, 1, 0.6f);
	private Color selectedFill = new Color (0.3f, 2, 0.5f, 0.2f);
//	private Color deselectedOutline = new Color (0.8f, 0.8f, 0.8f);
//	private Color deselectedFill = new Color (1, 1, 1, 0.2f);
	
	
	
	
	private void OnEnable ()
	{
		t = (MeshCollider)target;
		
		// Create cube
		mesh = new GameObject ();
		mesh.hideFlags = HideFlags.HideAndDontSave;
		mesh.AddComponent<MeshRenderer> ();
		mf = mesh.AddComponent<MeshFilter> ();
		
		mf.mesh = t.sharedMesh;
		
		// Create material
		mat = new Material (Shader.Find ("Transparent/Diffuse"));
		mat.hideFlags = HideFlags.HideAndDontSave;
		mat.color = selectedFill;
		mesh.GetComponent<Renderer>().sharedMaterial = mat;
		
		UpdateFillTransform ();
	}
	
	
	private void OnDisable ()
	{
		DestroyImmediate (mesh);
		DestroyImmediate (mat);
	}
	
	
	
	private void UpdateFillTransform ()
	{
		t = (MeshCollider)target;
		
		Transform meshTrans = mesh.transform;
		Transform rootTrans = t.transform;
		
		meshTrans.position = rootTrans.position;
		meshTrans.rotation = rootTrans.rotation;
		meshTrans.localScale = rootTrans.localScale;
	}
	
	
	
	public void OnSceneGUI ()
	{
		UpdateFillTransform ();
	}
	
	
	public override void OnInspectorGUI ()
	{
		t = (MeshCollider)target;
		
		// TODO implement undo
		
		EditorGUILayout.BeginVertical ();
		
		// FIXME whenever a PhysicMaterial asset is dropped in, it creates an instance. Need to reference original, not instance.
		t.material = (PhysicMaterial)EditorGUILayout.ObjectField ("Material", t.material, typeof(PhysicMaterial));
		
		t.isTrigger = EditorGUILayout.Toggle ("Is Trigger", t.isTrigger);
		
		t.smoothSphereCollisions = EditorGUILayout.Toggle ("Smooth Sphere Collisions", t.smoothSphereCollisions);
		
		t.convex = EditorGUILayout.Toggle ("Convex", t.convex);
		
		Mesh newMesh = (Mesh)EditorGUILayout.ObjectField ("Mesh", t.sharedMesh, typeof(Mesh));
		t.sharedMesh = newMesh;
		mf.sharedMesh = newMesh;
		
		EditorGUILayout.EndVertical ();
	}
	
	
	#region GUI
	
	private void MaterialGUI ()
	{
		
	}
	
	
	private void MeshGUI ()
	{
		
	}
	
	
	#endregion

}
