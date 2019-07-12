using UnityEngine;
using UnityEditor;
using System.Collections;

public abstract class ColliderEditor : Editor {
	
	protected readonly Color selectedOutline = new Color (0.6f, 1, 0.6f);
	protected readonly Color selectedFill = new Color (0.3f, 2, 0.5f, 0.2f);
	protected readonly Color deselectedOutline = new Color (0.8f, 0.8f, 0.8f);
	protected readonly Color deselectedFill = new Color (1, 1, 1, 0.2f);
	
	private Matrix4x4 oldMatrix;
	
	private static GameObject displayObject;
	private static Material mat;
	
	
	#region Fill Object
	
	protected void CreateFill ()
	{
		// Create geometry
		if (target as BoxCollider) {
			displayObject = GameObject.CreatePrimitive (PrimitiveType.Cube);
		}
		else if (target as SphereCollider) {
			displayObject = GameObject.CreatePrimitive (PrimitiveType.Sphere);
		}
		else if (target as CapsuleCollider) {
			displayObject = GameObject.CreatePrimitive (PrimitiveType.Capsule);
		}
		else {
			return;
		}
		displayObject.hideFlags = HideFlags.HideAndDontSave;
		
		// Create material
		mat = new Material (Shader.Find ("Transparent/Diffuse"));
		mat.hideFlags = HideFlags.HideAndDontSave;
		mat.color = selectedFill;
		displayObject.GetComponent<Renderer>().sharedMaterial = mat;
	}

	protected void ReleaseFill ()
	{
		DestroyImmediate (displayObject);
		DestroyImmediate (mat);
	}
	
	
	protected void UpdateFill ()
	{
		UpdateFill (false);
	}
	
	
	protected void UpdateFill (bool uniformScale)
	{
		
		// FIXME deep parenting hierachies with scaling messes everything up
		
		
		Collider t = (Collider)target;
		Transform fillTrans = displayObject.transform;
		Transform rootTrans = t.transform;
		
		Vector3 center = Vector3.zero;
		Vector3 size = Vector3.one;
		
		if (t as SphereCollider) {
			size = Vector3.one * ((SphereCollider)t).radius * 2;
			center = ((SphereCollider)t).center;
		}
		if (t as BoxCollider) {
			center = ((BoxCollider)t).center;
			size = ((BoxCollider)t).size;
		}
		if (t as CapsuleCollider) {
			center = ((CapsuleCollider)t).center;
		}
		
		fillTrans.position = rootTrans.position + center;
		fillTrans.rotation = Quaternion.identity;
		// Enlarge scale slightly to avoid z-fighting
		fillTrans.localScale = size * 1.001f;
		
		SoftParent (fillTrans, rootTrans);
		
		if (uniformScale) {
			float l = ColliderEditorUtilities.LargestComponent (fillTrans.localScale);
			fillTrans.localScale = new Vector3 (l, l, l);
		}
	}
	
	#endregion
	
	
	#region Handles
	
	protected Vector3 ScaleHandle (Vector3 position)
	{
		return Handles.FreeMoveHandle (position, Quaternion.identity, HandleUtility.GetHandleSize (position) * 0.03f, Vector3.one, Handles.DotCap);
	}
	
	
	protected Vector3 MoveHandle (Vector3 position)
	{
		return Handles.FreeMoveHandle (position, Quaternion.identity, HandleUtility.GetHandleSize (position) * 0.05f, Vector3.one, Handles.DotCap);
	}
	
	
	protected void SetupHandlesMatrix ()
	{
		SetupHandlesMatrix (false);
	}
	
	protected void SetupHandlesMatrix (bool uniformScale)
	{
		oldMatrix = Handles.matrix;
		Transform trans = ((Collider)target).transform;
		
		Matrix4x4 m;
		
//		// Use the largest scale axis for all three axes
//		if (uniformScale == true) {
//			Vector3 pos = new Vector3 (
//				trans.position.x * trans.localScale.x,
//				trans.position.y * trans.localScale.y,
//				trans.position.z * trans.localScale.z
//				);
//			float s = ColliderEditorUtilities.LargestComponent (fillTrans.localScale);
//			m = Matrix4x4.TRS (pos, trans.rotation, new Vector3 (s, s, s));
//		}
//		else {
			m = trans.localToWorldMatrix;
//		}
		
		Handles.matrix = m;
	}
	
	
	protected void ReleaseHandlesMatrix ()
	{
		Handles.matrix = oldMatrix;
	}
	
	#endregion
	
	
	#region Undo
	
	protected void SetupUndo ()
	{
		Undo.SetSnapshotTarget (target, "Modify Collider");
	}
	
	#endregion
	
	
	/// <summary>
	/// Apply a transformation resembling a parenting relationship without setting up an actual parenting hierarchy
	/// </summary>
	private void SoftParent (Transform child, Transform parent)
	{
		Vector3 offset = child.position - parent.position;
		Vector3 scale = parent.localScale;
		
		// TRS
		child.position = parent.rotation * ColliderEditorUtilities.MultiplyComponents (offset, scale) + parent.position;
		child.rotation = parent.rotation; // FIXME need to add the original rotation
		child.localScale = ColliderEditorUtilities.MultiplyComponents (child.localScale, parent.localScale);
	}
	
}
