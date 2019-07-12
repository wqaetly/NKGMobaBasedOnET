using System;
using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

public class ColliderWindow : EditorWindow
{
	public static ColliderWindow window;
	[MenuItem("Window/Collider Editor")]
	private static void Init ()
	{
		window = (ColliderWindow)EditorWindow.GetWindow (typeof(ColliderWindow), false, "Colliders");
		EditorApplication.modifierKeysChanged += window.Repaint;
		modeSelected = EditorPrefs.GetInt ("Collider Mode Selected", 0);
		typeSelected = EditorPrefs.GetInt ("Collider Type Selected", 0);
		compound = EditorPrefs.GetBool ("Compound Collider Mode Enabled", false);
	}
	
	private static void UpdateWindow ()
	{
		if (window == null) {
			Init ();
		}
		window.Repaint ();
	}
	
	
	private struct ColliderType
	{
		public string name;
		public Type type;

		public ColliderType (string name, Type type)
		{
			this.name = name;
			this.type = type;
		}
	}

	
	#region Static Data
	
	static ColliderType[] colliderTypes = {
		new ColliderType ("Sphere", typeof(SphereCollider)),
		new ColliderType ("Capsule", typeof(CapsuleCollider)),
		new ColliderType ("Box", typeof(BoxCollider)),
		new ColliderType ("Wheel", typeof(WheelCollider)),
		new ColliderType ("Mesh", typeof(MeshCollider))
	};
	static int typeSelected;
	static int modeSelected;
	static bool compound = false;
	
	#endregion
	
	
	#region Properties

	public static string ColliderName {
		get { return colliderTypes[typeSelected].name + " " + ColliderModeName; }
	}
	
	public static string ColliderModeName {
		get { return (modeSelected == 0 ? "Collider" : "Trigger") + (Plural ? "s" : ""); }
	}

	public static bool Plural {
		get { return Selection.gameObjects.Length > 1; }
	}

	public static string ColliderModeNamePlural {
		get { return (modeSelected == 0 ? "Collider" : "Trigger") + "s"; }
	}

	public static string ColliderNamePlural {
		get { return colliderTypes[typeSelected].name + " " + ColliderModeNamePlural; }
	}

	#endregion

	
	
	#region Unity events
	
	private void OnSelectionChange ()
	{
		UpdateWindow ();
	}


	private void OnGUI ()
	{
		GUILayoutOption[] options = { GUILayout.MinWidth (170f), GUILayout.MaxWidth (Mathf.Infinity) };
		GUILayout.BeginVertical (options);
		GUILayout.Space (2f);
		GUILayout.Label ("Collider Type", EditorStyles.boldLabel);
		ColliderModeToolbar ();
		GUILayout.Space (2);
		ColliderTypeToolbar ();
		GUILayout.Label ("Operations", EditorStyles.boldLabel);
		if (Selection.gameObjects.Length <= 0) {
			GUI.enabled = false;
		}
		AddCollidersButton ();
		RemoveCollidersButton ();
		GUI.enabled = true;
		CompoundColliderToggle ();
		GUILayout.EndVertical ();
	}
	
	
	#endregion


	private static bool IsCompoundCollider (GameObject go)
	{
		// Must have only a transform component and a collider component, nothing more
		if (go.GetComponent<Collider> () != null && go.GetComponents<Component> ().Length <= 2)
			return true;
		else
			return false;
	}


	private static Collider[] GetCompoundColliders (GameObject go)
	{
		// Get all collider types
		return GetCompoundColliders (go, typeof(Collider));
	}

	private static Collider[] GetCompoundColliders (GameObject go, Type type)
	{
		// Return only objects that have a transform component and a collider. This means that they're functioning as compound colliders.
		List<Collider> colliderList = new List<Collider> ();
		foreach (Collider coll in go.GetComponentsInChildren (type)) {
			// Grab only compound colliders that are direct descendents of the given game object
			if (coll.gameObject.GetComponents<Component> ().Length <= 2 && coll.transform.parent == go.transform) {
				colliderList.Add (coll);
			}
		}
		return colliderList.ToArray ();
	}


	private static Collider[] GetColliders (GameObject[] gameObjects)
	{
		return GetColliders (gameObjects, typeof(Collider));
	}

	private static Collider[] GetColliders (GameObject[] gameObjects, Type type)
	{
		List<Collider> colliders = new List<Collider> ();
		foreach (GameObject go in gameObjects) {
			Collider collider = (Collider)go.GetComponent (type);
			if (collider != null) {
				colliders.Add (collider);
			}
		}
		return colliders.ToArray ();
	}


	private static Type SpecificColliderType (Collider collider)
	{
		Type collType = null;
		if (collider as SphereCollider)
			collType = typeof(SphereCollider);
		if (collider as CapsuleCollider)
			collType = typeof(CapsuleCollider);
		if (collider as BoxCollider)
			collType = typeof(BoxCollider);
		if (collider as WheelCollider)
			collType = typeof(WheelCollider);
		if (collider as MeshCollider)
			collType = typeof(MeshCollider);
		return collType;
	}


	private static void RemoveColliders (Collider[] colliders, Type type)
	{
		List<Collider> colliderList = new List<Collider> ();
		foreach (Collider coll in colliders) {
			if (SpecificColliderType (coll) == type) {
				colliderList.Add (coll);
			}
		}
		if (colliderList.Count > 0)
			RemoveColliders (colliderList.ToArray ());
	}

	private static void RemoveColliders (Collider[] colliders)
	{
		Undo.RegisterSceneUndo ("Remove Colliders");
		foreach (Collider coll in colliders) {
			// Ignore children that have already been removed
			if (coll == null) {
				continue;
			}
			// FIXME need to respect HideFlags for "NotEditable"
			GameObject go = coll.gameObject;
			UnityEngine.Object.DestroyImmediate (coll);
			// If the object has been gutted completely then we can destroy it since it was apparently a compound collider
			if (go.GetComponents<Component> ().Length <= 1) {
				UnityEngine.Object.DestroyImmediate (go);
			}
		}
	}


	private static void AddColliders (GameObject[] gameObjects, Type type)
	{
		Undo.RegisterSceneUndo ("Add Colliders");
		foreach (GameObject go in gameObjects) {
			if (go.GetComponent<Collider> () == null)
				go.AddComponent (type);
			// If mode set to compound collider
			if (modeSelected == 1 && go.GetComponent<Collider>()) {
				go.GetComponent<Collider>().isTrigger = true;
			}
		}
	}

	private static void ReplaceColliders (GameObject[] gameObjects, Type type)
	{
		Undo.RegisterSceneUndo ("Replace Colliders");
		foreach (GameObject go in gameObjects) {
			
			Collider coll = go.GetComponent<Collider> ();
			if (coll != null) {
				// FIXME take into account game objects that have a script that requires a collider.
				// May have to use reflection here to swap out the component directly
				UnityEngine.Object.DestroyImmediate (go.GetComponent<Collider> ());
			}
			// Make sure all colliders have been completely destroyed before adding another
			if (go.GetComponent<Collider> () == null) {
				go.AddComponent (type);
				// If mode is set to trigger
				if (modeSelected == 1 && go.GetComponent<Collider>()) {
					go.GetComponent<Collider>().isTrigger = true;
				}
				// Change the game object's name if it is a compound collider
				if (compound) {
					go.transform.name = colliderTypes[typeSelected].name + " Collider";
				}
			}
		}
	}

	private static void ReplaceCompoundColliders (GameObject[] gameObjects, Type type)
	{
		List<GameObject> goList = new List<GameObject> ();
		foreach (GameObject go in gameObjects) {
			foreach (Collider coll in GetCompoundColliders (go)) {
				goList.Add (coll.gameObject);
				// Change compound collider name if the collider type has changed
			}
		}
		ReplaceColliders (goList.ToArray (), type);
	}


	private static GameObject CreateCompoundCollider (Type type)
	{
		return CreateCompoundCollider (type, null);
	}

	private static GameObject CreateCompoundCollider (Type type, Transform parent)
	{
		GameObject newObject = new GameObject (colliderTypes[typeSelected].name + " Collider");
		// If set to trigger mode, make the new collider a trigger
		newObject.AddComponent (type);
		if (modeSelected == 1 && newObject.GetComponent<Collider>()) {
			newObject.GetComponent<Collider>().isTrigger = true;
		}
		Transform newTrans = newObject.transform;
		if (parent != null) {
			// Set position and parent of new GameObject
			newTrans.position = parent.position;
			newTrans.parent = parent;
		}
		return newObject;
	}


	private static GameObject[] AddCompoundColliders (GameObject[] gameObjects, Type type)
	{
		Undo.RegisterSceneUndo ("Add Colliders");
		// List of parents so that multiple of the same collider aren't generated repeatedly under one parent node
		List<Transform> parentList = new List<Transform> ();
		List<GameObject> newObjects = new List<GameObject> ();
		foreach (GameObject go in gameObjects) {
			Transform parentTrans = go.transform.parent;
			
			// Don't create more than one compound collider under a single parent
			if (parentTrans != null && parentList.Contains (parentTrans))
				continue;
			
			// If we're given a compound collider that's not in the root of the hierarchy, create a new compound collider as a sibling
			if (IsCompoundCollider (go) && parentTrans != null) {
				newObjects.Add (CreateCompoundCollider (type, parentTrans));
				parentList.Add (parentTrans);
			// Add a new compound collider as a child of the given game object
			} else {
				newObjects.Add (CreateCompoundCollider (type, go.transform));
			}
		}
		if (newObjects.Count < 1)
			return null;
		else
			return newObjects.ToArray ();
	}


	#region GUI components

	private static void AddCollidersButton ()
	{
		// Setup
		
		string addButtonText = "";
		if (EditorGUI.actionKey)
			addButtonText = "Replace All " + ColliderModeNamePlural;
		else
			addButtonText = "Add " + ColliderName;
		Texture2D plusIcon = (Texture2D)Resources.Load ("Plus", typeof(Texture2D));
		
		// Execute
		
		//string tooltip = "Add the given collider type to the selected object(s). Hold down the command key (Mac) or control key (Windows) to replace colliders already attached to the selected objects.";
		string tooltip = "";
		if (GUILayout.Button (new GUIContent (addButtonText, plusIcon, tooltip))) {
			// Special action - replace with new collider type
			if (EditorGUI.actionKey) {
				// Compound collider
				if (compound) {
					ReplaceCompoundColliders (Selection.gameObjects, colliderTypes[typeSelected].type);
					// If the collider type has changed, then change the name of compound collider object
				} else
					// Single collider
					ReplaceColliders (Selection.gameObjects, colliderTypes[typeSelected].type);
			// Standard action
			} else {
				// Compound collider
				if (compound)
					Selection.objects = AddCompoundColliders (Selection.gameObjects, colliderTypes[typeSelected].type);
				else
					// Single collider
					AddColliders (Selection.gameObjects, colliderTypes[typeSelected].type);
			}
		}
	}


	private static void RemoveCollidersButton ()
	{
		// Setup
		
		string removeButtonText = "Remove ";
		if (EditorGUI.actionKey) {
			removeButtonText += "All " + ColliderModeNamePlural;
		} else {
			if (compound)
				removeButtonText += ColliderNamePlural;
			else
				removeButtonText += ColliderName;
		}
		Texture2D minusIcon = (Texture2D)Resources.Load ("Minus", typeof(Texture2D));
		
		// Execute
		
		//string tooltip = "Remove the given collider type on the selected object(s). Hold down the command key (Mac) or control key (Windows) to remove all collider types.";
		string tooltip = "";
		if (GUILayout.Button (new GUIContent (removeButtonText, minusIcon, tooltip))) {
			// Compound colliders
			if (compound) {
				Undo.RegisterSceneUndo ("Remove Colliders");
				foreach (GameObject go in Selection.gameObjects) {
					Collider[] children;
					// Special action - remove all types
					if (EditorGUI.actionKey)
						children = GetCompoundColliders (go);
					else
						// Standard action - remove one type
						children = GetCompoundColliders (go, colliderTypes[typeSelected].type);
					foreach (Collider coll in children)
						UnityEngine.Object.DestroyImmediate (coll.gameObject);
				}
			}
			// Single collider
			if (EditorGUI.actionKey) {
				// Special action
				RemoveColliders (GetColliders (Selection.gameObjects));
			}
			else {
				// Standard action
				RemoveColliders (GetColliders (Selection.gameObjects), colliderTypes[typeSelected].type);
			}
		}
	}


	private static void ColliderModeToolbar ()
	{
		GUIContent[] modes = { new GUIContent ("Collider", "Standard collider"), new GUIContent ("Trigger", "Collider marked as trigger") };
		modeSelected = GUILayout.Toolbar (modeSelected, modes);
		EditorPrefs.SetInt ("Collider Mode Selected", modeSelected);
	}


	private static void ColliderTypeToolbar ()
	{
		int count = colliderTypes.Length;
		GUIContent[] content = new GUIContent[count];
		for (int i = 0; i < count; i++) {
			// Grab a texture of the appropriate collider type icon
			Texture icon = EditorGUIUtility.ObjectContent (null, colliderTypes[i].type).image;
			string tooltip = colliderTypes[i].name + " Collider";
			content[i] = new GUIContent (icon, tooltip);
		}
		typeSelected = GUILayout.Toolbar (typeSelected, content);
		EditorPrefs.SetInt ("Collider Type Selected", typeSelected);
	}


	private static void CompoundColliderToggle ()
	{
		//string tooltip = "When enabled, compound collider objects will be added or removed from the selected game object. If a compound collider is selected, new colliders will be added as siblings.";
		string tooltip = "";
		compound = GUILayout.Toggle (compound, new GUIContent ("Compound Colliders", tooltip));
		EditorPrefs.SetBool ("Compound Collider Mode Enabled", compound);
	}
	
	#endregion
	
}
