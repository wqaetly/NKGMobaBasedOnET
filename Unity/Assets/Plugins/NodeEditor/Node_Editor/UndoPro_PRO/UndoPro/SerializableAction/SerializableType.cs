namespace UndoPro.SerializableActionHelper
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Reflection;
	using System.Runtime.CompilerServices;
	using UnityEngine;

	/// <summary>
	/// Wrapper for System.Type that handles serialization.
	/// Serialized Data contains assembly type name and generic arguments (one level) only.
	/// </summary>
	[System.Serializable]
	public class SerializableType
	{
		public Type _type;
		public Type type
		{
			get
			{
				if (_type == null)
					Deserialize();
				return _type;
			}
		}

		[SerializeField]
		private string typeName;
		[SerializeField]
		private string[] genericTypes;

		public bool isCompilerGenerated { get { return Attribute.GetCustomAttribute (type, typeof(CompilerGeneratedAttribute), false) != null; } }

		public SerializableType (Type Type)
		{
			_type = Type;
			Serialize();
		}

		#region Serialization

		public void Serialize ()
		{
			if (_type == null)
			{
				typeName = String.Empty;
				genericTypes = null;
				return;
			}

			if (_type.IsGenericType)
			{ // Generic type
				typeName = _type.GetGenericTypeDefinition ().AssemblyQualifiedName;
				genericTypes = _type.GetGenericArguments ().Select ((Type t) => t.AssemblyQualifiedName).ToArray ();
			}
			else
			{ // Normal type
				typeName = _type.AssemblyQualifiedName;
				genericTypes = null;
			}
		}

		public void Deserialize ()
		{
			if (String.IsNullOrEmpty (typeName))
				return;

			_type = Type.GetType (typeName);
			if (_type == null)
				throw new Exception ("Could not deserialize type '" + typeName + "'!");

			if (_type.IsGenericTypeDefinition && genericTypes != null && genericTypes.Length > 0)
			{ // Generic type
				Type[] genArgs = new Type[genericTypes.Length];
				for (int i = 0; i < genericTypes.Length; i++)
					genArgs[i] = Type.GetType (genericTypes[i]);

				Type genType = _type.MakeGenericType (genArgs);
				if (genType != null)
					_type = genType;
				else 
					Debug.LogError ("Could not make generic-type definition '" + typeName + "' generic!");
			}
		}

		#endregion
	}
}