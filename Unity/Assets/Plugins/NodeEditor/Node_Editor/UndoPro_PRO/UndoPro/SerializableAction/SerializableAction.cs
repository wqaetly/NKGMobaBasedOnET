namespace UndoPro.SerializableActionHelper
{
	using UnityEngine;
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.IO;
	using System.Runtime.Serialization.Formatters.Binary;
	using System.Reflection;

	/// <summary>
	/// Wrapper for a single-cast Action without parameters that handles serialization supporting System- and UnityEngine Objects aswell as anonymous methods to some degree
	/// </summary>
	[Serializable]
	public class SerializableAction
	{
		private Action action;

		[SerializeField]
		private SerializableObject serializedTarget;
		[SerializeField] 
		private SerializableMethodInfo serializedMethod;

		// Accessor Properties
		public Action Action 
		{ 
			get 
			{
				if (action == null)
					action = DeserializeAction ();
				return action;
			}
		}
		public object Target { get { return Action.Target; } }
		public MethodInfo Method { get { return Action.Method; } }

		/// <summary>
		/// Create a new SerializableAction from a non-anonymous action (System or Unity)
		/// </summary>
		public SerializableAction (Action srcAction)
		{
			if (srcAction.GetInvocationList ().Length > 1)
				throw new UnityException ("Cannot create SerializableAction from a multi-cast action!");

			SerializeAction (srcAction);
		}

		#region General

		/// <summary>
		/// Invoke this serialized action
		/// </summary>
		public void Invoke ()
		{
			if (Action != null) 
				Action.Invoke();
		}

		/// <summary>
		/// Returns whether this action is valid
		/// </summary>
		public bool IsValid ()
		{
			if (action == null) 
			{
				try 
				{
					action = DeserializeAction ();
				}
				catch
				{
					return false;
				}
			}
			return true;
		}

		#endregion

		#region Serialization

		/// <summary>
		/// Serializes the given action depending on the type (System or Unity) and stores it into this SerializableAction
		/// </summary>
		private void SerializeAction (Action srcAction)
		{
			action = srcAction;

			//Debug.Log ("Serializing action for method '" + srcAction.Method.Name + "'!"); // TODO: DEBUG REMOVE

			serializedMethod = new SerializableMethodInfo (srcAction.Method);
			serializedTarget = new SerializableObject (action.Target);
		}


		/// <summary>
		/// Deserializes the action depending on the type (System or Unity) and returns it
		/// </summary>
		private Action DeserializeAction () 
		{
			// Target
			object target = serializedTarget.Object;

			// Method
			MethodInfo method = serializedMethod.methodInfo;
			if (method == null)
				throw new DataMisalignedException ("Could not deserialize action method '" + serializedMethod.SignatureName + "'!");

			return Delegate.CreateDelegate (typeof(Action), target, method) as Action;
		}

		#endregion
	}

}