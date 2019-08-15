using UnityEngine;
using UnityEditor;
using System;
using System.Collections.Generic;

using UndoPro.SerializableActionHelper;

public class SerializableActionTestWindow : EditorWindow 
{
	[MenuItem ("Window/SerializableAction Test")]
	private static void Open () 
	{
		EditorWindow.GetWindow<SerializableActionTestWindow> ("SerializableAction Test");
	}

	#region Test Methods: Unity

	private void UnityTargettedNormal () 
	{
		Debug.Log ("Unity-Targetted Normal executed!");
	}

	private void UnityTargettedGenericMethodNormal<T> () 
	{
		Debug.Log ("Unity-Targetted GenericMethod<" + typeof(T).Name + "> Normal executed!");
	}

	private static void UnityTargettedStatic () 
	{
		Debug.Log ("Unity-Targetted Static executed!");
	}

	private static void UnityTargettedGenericMethodStatic<T> () 
	{
		Debug.Log ("Unity-Targetted GenericMethod<" + typeof(T).Name + "> Static executed!");
	}

	private static Action getUnityTargettedAnonymous () 
	{
		return new Action (() => Debug.Log ("Unity-Targetted Anonymous executed!"));
	}

	private static Action getUnityTargettedAnonymous<T> () 
	{
		return new Action (() => Debug.Log ("Unity-Targetted GenericMethod<" + typeof(T).Name + "> Anonymous executed!"));
	}

	#endregion

	#region Test Methods: System

	[Serializable]
	public class SystemClass : System.Object
	{
		public void SystemTargettedNormal () 
		{
			Debug.Log ("System-Targetted Normal executed!");
		}

		public void SystemTargettedGenericMethodNormal<T> () 
		{
			Debug.Log ("System-Targetted GenericMethod<" + typeof(T).Name + "> Normal executed!");
		}

		public static void SystemTargettedStatic () 
		{
			Debug.Log ("System-Targetted Static executed!");
		}

		public static void SystemTargettedGenericMethodStatic<T> () 
		{
			Debug.Log ("System-Targetted GenericMethod<" + typeof(T).Name + "> Static executed!");
		}

		public Action getSystemTargettedAnonymous () 
		{
			return new Action (() => Debug.Log ("System-Targetted Anonymous executed!"));
		}

		public Action getSystemTargettedGenericAnonymous<T> () 
		{
			return new Action (() => Debug.Log ("System-Targetted GenericMethod<" + typeof(T).Name + "> Anonymous executed!"));
		}
	}

	#endregion

	#region Test Methods: System (Generic Class)

	[Serializable]
	public class SystemGenericTypeClass<T> : System.Object
	{
		public void SystemTargettedGenericNormal () 
		{
			Debug.Log ("System-Targetted GenericClass<" + typeof(T).Name + "> Normal executed!");
		}

		public static void SystemTargettedGenericStatic () 
		{
			Debug.Log ("System-Targetted GenericClass<" + typeof(T).Name + "> Static executed!");
		}

		public Action getSystemTargettedGenericAnonymous () 
		{
			return new Action (() => Debug.Log ("System-Targetted GenericClass<" + typeof(T).Name + "> Anonymous executed!"));
		}
	}

	#endregion

	public SystemClass systemClass = new SystemClass ();
	public SystemGenericTypeClass<Vector3> systemGenericTypeClass = new SystemGenericTypeClass<Vector3> ();

	public int testInt = 62;

	public SerializableAction unityStaticAction;
	public SerializableAction unityGenericMethodStaticAction;

	public SerializableAction systemStaticAction;
	public SerializableAction systemGenericStaticAction;
	public SerializableAction systemGenericMethodStaticAction;

	public SerializableAction unityNormalAction;
	public SerializableAction unityGenericMethodNormalAction;

	public SerializableAction systemNormalAction;
	public SerializableAction systemGenericNormalAction;
	public SerializableAction systemGenericMethodNormalAction;

	public SerializableAction unityAnonymousAction;
	public SerializableAction unityGenericMethodAnonymousAction;
	public SerializableAction unityLocalVarAnonymousAction;
	public SerializableAction unityClassVarAnonymousAction;

	public SerializableAction systemAnonymousAction;
	public SerializableAction systemGenericAnonymousAction;
	public SerializableAction systemGenericMethodAnonymousAction;


	public void OnGUI () 
	{
		// -----

		EditorGUILayout.Space ();

		GUILayout.Label ("STATIC");

		EditorGUILayout.Space ();

		ActionGUI ("Unity-Targetted Static", ref unityStaticAction, SerializableActionTestWindow.UnityTargettedStatic);

		EditorGUILayout.Space ();

		ActionGUI ("Unity-Targetted GenericMethod<Vector3> Static", ref unityGenericMethodStaticAction, SerializableActionTestWindow.UnityTargettedGenericMethodStatic<Vector3>);

		EditorGUILayout.Space ();

		ActionGUI ("System-Targetted Static", ref systemStaticAction, SystemClass.SystemTargettedStatic);

		EditorGUILayout.Space ();

		ActionGUI ("System-Targetted GenericClass<Vector3> Static", ref systemGenericStaticAction, SystemGenericTypeClass<Vector3>.SystemTargettedGenericStatic);

		EditorGUILayout.Space ();

		ActionGUI ("System-Targetted GenericMethod<Vector3> Static", ref systemGenericMethodStaticAction, SystemClass.SystemTargettedGenericMethodStatic<Vector3>);

		// -----

		EditorGUILayout.Space ();

		GUILayout.Label ("NORMAL");

		EditorGUILayout.Space ();

		ActionGUI ("Unity-Targetted Normal", ref unityNormalAction, this.UnityTargettedNormal);

		EditorGUILayout.Space ();

		ActionGUI ("Unity-Targetted GenericMethod<Vector3> Normal", ref unityGenericMethodNormalAction, this.UnityTargettedGenericMethodNormal<Vector3>);

		EditorGUILayout.Space ();

		ActionGUI ("System-Targetted Normal", ref systemNormalAction, systemClass.SystemTargettedNormal);

		EditorGUILayout.Space ();

		ActionGUI ("System-Targetted GenericClass<Vector3> Normal", ref systemGenericNormalAction, systemGenericTypeClass.SystemTargettedGenericNormal);

		EditorGUILayout.Space ();

		ActionGUI ("System-Targetted GenericMethod<Vector3> Normal", ref systemGenericMethodNormalAction, systemClass.SystemTargettedGenericMethodNormal<Vector3>);

		// -----

		EditorGUILayout.Space ();

		GUILayout.Label ("ANONYMOUS");

		EditorGUILayout.Space ();

		ActionGUI ("Unity-Targetted Anyonymous", ref unityAnonymousAction, getUnityTargettedAnonymous ());

		EditorGUILayout.Space ();

		testInt = EditorGUILayout.IntSlider ("Test Int", testInt, 0, 100);
		ActionGUI ("Unity-Targetted ClassVar Anyonymous", ref unityClassVarAnonymousAction, () => Debug.Log ("Unity-Targetted ClassVar Anyonymous executed: " + testInt));
		int localInt = testInt;
		ActionGUI ("Unity-Targetted LocalVar Anyonymous", ref unityLocalVarAnonymousAction, () => Debug.Log ("Unity-Targetted LocalVar Anyonymous executed: " + localInt));

		EditorGUILayout.Space ();

		ActionGUI ("Unity-Targetted GenericMethod<Vector3> Anyonymous", ref unityGenericMethodAnonymousAction, getUnityTargettedAnonymous<Vector3> ());

		EditorGUILayout.Space ();

		ActionGUI ("System-Targetted Anyonymous", ref systemAnonymousAction, systemClass.getSystemTargettedAnonymous ());

		EditorGUILayout.Space ();

		ActionGUI ("System-Targetted GenericClass<Vector3> Anyonymous", ref systemGenericAnonymousAction, systemGenericTypeClass.getSystemTargettedGenericAnonymous ());

		EditorGUILayout.Space ();

		ActionGUI ("System-Targetted GenericMethod<Vector3> Anyonymous", ref systemGenericMethodAnonymousAction, systemClass.getSystemTargettedGenericAnonymous<Vector3> ());

		// -----

		Repaint ();
	}

	private void ActionGUI (string label, ref SerializableAction serializedAction, Action action)
	{
		GUILayout.Label (label + " SerializableAction");
		if (serializedAction != null && !serializedAction.IsValid ())
			serializedAction = null;
		GUILayout.BeginHorizontal ();
		if (GUILayout.Button ("Create (" + (serializedAction != null) + ")")) 
		{
			serializedAction = new SerializableAction (action);
			serializedAction.Invoke ();
		}
		if (GUILayout.Button ("Delete")) 
		{
			serializedAction = null;
		}
		if (GUILayout.Button ("Invoke")) 
		{
			if (serializedAction != null)
				serializedAction.Invoke ();
			else
				Debug.LogError (label + " Action is null!");
		}
		GUILayout.EndHorizontal ();
	}
}
