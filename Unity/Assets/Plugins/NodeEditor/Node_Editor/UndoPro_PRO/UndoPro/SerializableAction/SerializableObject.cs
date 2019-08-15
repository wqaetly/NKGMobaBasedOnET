namespace UndoPro.SerializableActionHelper
{
	using UnityEngine;
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using System.IO;
	using System.Runtime.Serialization.Formatters.Binary;
	using System.Reflection;

	/// <summary>
	/// Wrapper for an arbitrary object that handles basic serialization, both System.Object, UnityEngine.Object, and even basic unserializable types (the same way, but one-level only, unserializable members will be default or null if previously null)
	/// </summary>
	[Serializable]
	public class SerializableObject : SerializableObjectOneLevel
	{
		[SerializeField]
		private List<SerializableObjectTwoLevel> manuallySerializedMembers;
		[SerializeField]
		protected List<SerializableObjectTwoLevel> collectionObjects;

		/// <summary>
		/// Create a new SerializableObject from an arbitrary object
		/// </summary>
		public SerializableObject(object srcObject) : base(srcObject) { }
		/// <summary>
		/// Create a new SerializableObject from an arbitrary object with the specified name
		/// </summary>
		public SerializableObject(object srcObject, string name) : base(srcObject, name) { }

		#region Serialization

		/// <summary>
		/// Serializes the given object and stores it into this SerializableObject
		/// </summary>
		protected override void Serialize()
		{
			if (isNullObject = _object == null)
				return;

			base.Serialize(); // Serialized normally

			if (_object.GetType().IsGenericType &&
				typeof(ICollection<>).MakeGenericType(_object.GetType().GetGenericArguments()).IsAssignableFrom(_object.GetType()))
			{
				IEnumerable collection = _object as IEnumerable;
				collectionObjects = new List<SerializableObjectTwoLevel>();
				foreach (object obj in collection)
					collectionObjects.Add(new SerializableObjectTwoLevel(obj));
			}
			else if (typeof(UnityEngine.Object).IsAssignableFrom(_object.GetType())) { }
			else if (_object.GetType().IsSerializable) { }
			else
			{ // Object is unserializable so it will later be recreated from the type, now serialize the serializable field values of the object
				FieldInfo[] fields = objectType.type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
				manuallySerializedMembers = new List<SerializableObjectTwoLevel>();
				foreach (FieldInfo field in fields)
					manuallySerializedMembers.Add(new SerializableObjectTwoLevel(field.GetValue(_object), field.Name));
				//manuallySerializedMembers = fields.Select ((FieldInfo field) => new SerializableObjectOneLevel (field.GetValue (_object), field.Name)).ToList ();
			}
		}

		/// <summary>
		/// Deserializes this SerializableObject
		/// </summary>
		protected override void Deserialize()
		{
			if (isNullObject)
				return;

			base.Deserialize(); // Deserialize normally

			Type type = objectType.type;
			if (type.IsGenericType &&
				typeof(ICollection<>).MakeGenericType(type.GetGenericArguments()).IsAssignableFrom(type))
			{
				if (collectionObjects != null && collectionObjects.Count > 0)
				{ // Add deserialized objects to collection
					MethodInfo add = type.GetMethod("Add");
					foreach (SerializableObjectTwoLevel obj in collectionObjects)
						add.Invoke(_object, new object[] { obj.Object });
				}
			}
			else if (typeof(UnityEngine.Object).IsAssignableFrom(type))
				_object = unityObject;
			else if (type.IsSerializable)
				_object = DeserializeFromString<System.Object>(serializedSystemObject);
			else if (manuallySerializedMembers != null && manuallySerializedMembers.Count > 0)
			{ // This object is an unserializable type, and previously the object was recreated from that type
			  // Now, restore the serialized field values of the object
				FieldInfo[] fields = objectType.type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
				if (fields.Length != manuallySerializedMembers.Count)
					Debug.LogError("Field length and serialized member length doesn't match (" + fields.Length + ":" + manuallySerializedMembers.Count + ") for object " + objectType.type.Name + "!");
				foreach (FieldInfo field in fields)
				{
					SerializableObjectTwoLevel matchObj = manuallySerializedMembers.Find((SerializableObjectTwoLevel obj) => obj.Name == field.Name);
					if (matchObj != null)
					{
						object obj = null;
						if (matchObj.Object == null) { }
						else if (!field.FieldType.IsAssignableFrom(matchObj.Object.GetType()))
							Debug.LogWarning("Deserialized object type " + matchObj.Object.GetType().Name + " is incompatible to field type " + field.FieldType.Name + "!");
						else
							obj = matchObj.Object;
						field.SetValue(Object, obj);
					}
					else
						Debug.LogWarning("Couldn't find a matching serialized field for '" + (field.IsPublic ? "public" : "private") + (field.IsStatic ? " static" : "") + " " + field.FieldType.FullName + "'!");
				}
			}
		}

		#endregion
	}

	/// <summary>
	/// Wrapper for an arbitrary object that handles basic serialization, both System.Object, UnityEngine.Object, and even basic unserializable types (the same way, but one-level only, unserializable members will be default or null if previously null)
	/// </summary>
	[Serializable]
	public class SerializableObjectTwoLevel : SerializableObjectOneLevel
	{
		[SerializeField] 
		private List<SerializableObjectOneLevel> manuallySerializedMembers;
		[SerializeField]
		protected List<SerializableObjectOneLevel> collectionObjects;

		/// <summary>
		/// Create a new SerializableObject from an arbitrary object
		/// </summary>
		public SerializableObjectTwoLevel (object srcObject) : base (srcObject) { }
		/// <summary>
		/// Create a new SerializableObject from an arbitrary object with the specified name
		/// </summary>
		public SerializableObjectTwoLevel (object srcObject, string name) : base(srcObject, name) { }

		#region Serialization

		/// <summary>
		/// Serializes the given object and stores it into this SerializableObject
		/// </summary>
		protected override void Serialize ()
		{
			if (isNullObject = _object == null)
				return;

			base.Serialize (); // Serialized normally

			if (_object.GetType().IsGenericType &&
				typeof(ICollection<>).MakeGenericType(_object.GetType().GetGenericArguments()).IsAssignableFrom(_object.GetType()))
			{
				IEnumerable collection = _object as IEnumerable;
				collectionObjects = new List<SerializableObjectOneLevel>();
				foreach (object obj in collection)
					collectionObjects.Add(new SerializableObjectOneLevel(obj));
			}
			else if (typeof(UnityEngine.Object).IsAssignableFrom(_object.GetType())) { }
			else if (_object.GetType().IsSerializable) { }
			else
			{ // Object is unserializable so it will later be recreated from the type, now serialize the serializable field values of the object
				FieldInfo[] fields = objectType.type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
				manuallySerializedMembers = new List<SerializableObjectOneLevel>();
				foreach (FieldInfo field in fields)
					manuallySerializedMembers.Add(new SerializableObjectOneLevel(field.GetValue(_object), field.Name));
				//manuallySerializedMembers = fields.Select ((FieldInfo field) => new SerializableObjectOneLevel (field.GetValue (_object), field.Name)).ToList ();
			}
		}

		/// <summary>
		/// Deserializes this SerializableObject
		/// </summary>
		protected override void Deserialize ()
		{
			if (isNullObject)
				return;

			base.Deserialize (); // Deserialize normally

			Type type = objectType.type;
			if (type.IsGenericType &&
				typeof(ICollection<>).MakeGenericType(type.GetGenericArguments()).IsAssignableFrom(type))
			{ 
				if (collectionObjects != null && collectionObjects.Count > 0)
				{ // Add deserialized objects to collection
					MethodInfo add = type.GetMethod("Add");
					foreach (SerializableObjectOneLevel obj in collectionObjects)
						add.Invoke(_object, new object[] { obj.Object });
				}
			}
			else if (typeof(UnityEngine.Object).IsAssignableFrom(type))
				_object = unityObject;
			else if (type.IsSerializable)
				_object = DeserializeFromString<System.Object>(serializedSystemObject);
			else if (manuallySerializedMembers != null && manuallySerializedMembers.Count > 0)
			{ // This object is an unserializable type, and previously the object was recreated from that type
				// Now, restore the serialized field values of the object
				FieldInfo[] fields = objectType.type.GetFields (BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
				if (fields.Length != manuallySerializedMembers.Count)
					Debug.LogError ("Field length and serialized member length doesn't match (" + fields.Length + ":" + manuallySerializedMembers.Count + ") for object " + objectType.type.Name + "!");
				foreach (FieldInfo field in fields)
				{
					SerializableObjectOneLevel matchObj = manuallySerializedMembers.Find ((SerializableObjectOneLevel obj) => obj.Name == field.Name);
					if (matchObj != null)
					{
						object obj = null;
						if (matchObj.Object == null) { }
						else if (!field.FieldType.IsAssignableFrom(matchObj.Object.GetType()))
							Debug.LogWarning("Deserialized object type " + matchObj.Object.GetType().Name + " is incompatible to field type " + field.FieldType.Name + "!");
						else
							obj = matchObj.Object;
						field.SetValue(Object, obj);
					}
					else
						Debug.LogWarning("Couldn't find a matching serialized field for '" + (field.IsPublic ? "public" : "private") + (field.IsStatic ? " static" : "") + " " + field.FieldType.FullName + "'!");
				}
			}
		}

		#endregion
	}

	/// <summary>
	/// Wrapper for an arbitrary object that handles basic serialization, both System.Object, UnityEngine.Object; unserializable types will be default or null if previously null;
	/// NO RECOMMENDED TO USE, it is primarily built to support SerializableObject!
	/// </summary>
	[Serializable]
	public class SerializableObjectOneLevel
	{
		[SerializeField]
		public string Name; // Just to identify this object
		protected object _object;
		public object Object
		{
			get
			{
				if (_object == null)
					Deserialize();
				return _object;
			}
		}

		// Serialized Data
		[SerializeField]
		protected bool isNullObject;
		[SerializeField] 
		protected SerializableType objectType;
		[SerializeField] 
		protected UnityEngine.Object unityObject;
		[SerializeField] 
		protected string serializedSystemObject;

		public SerializableObjectOneLevel (object srcObject)
		{
			_object = srcObject;
			Serialize();
		}

		public SerializableObjectOneLevel(object srcObject, string name)
		{
			_object = srcObject;
			Name = name;
			Serialize();
		}

		#region Serialization

		/// <summary>
		/// Serializes the given object and stores it into this SerializableObject
		/// </summary>
		protected virtual void Serialize () 
		{
			if (isNullObject = _object == null)
				return;

			unityObject = null;
			serializedSystemObject = String.Empty;
			objectType = new SerializableType (_object.GetType ());
			
			if (_object.GetType().IsGenericType &&
				typeof(ICollection<>).MakeGenericType(_object.GetType().GetGenericArguments()).IsAssignableFrom(_object.GetType()))
			{ // If levels are free to serialize, then they will get serialized, if not, then not
			}
			else if (typeof(UnityEngine.Object).IsAssignableFrom(_object.GetType()))
			{
				unityObject = (UnityEngine.Object)_object;
			}
			else if (_object.GetType().IsSerializable)
			{
				serializedSystemObject = SerializeToString<System.Object>(_object);
				if (serializedSystemObject == null)
					Debug.LogWarning("Failed to serialize field name " + Name + "!");
			}
			// else default object (and even serializable members) will be restored from the type
		}

		/// <summary>
		/// Deserializes this SerializableObject
		/// </summary>
		protected virtual void Deserialize () 
		{
			_object = null;
			if (isNullObject)
				return;
			if (objectType.type == null)
				throw new Exception("Could not deserialize object as it's type could no be deserialized!");
			Type type = objectType.type;

			if (type.IsGenericType &&
				typeof(ICollection<>).MakeGenericType(type.GetGenericArguments()).IsAssignableFrom(type))
			{ // Collection type, if still more levels free, members will be serialized, if not, then not 
				_object = Activator.CreateInstance(type);
			}
			else if (typeof(UnityEngine.Object).IsAssignableFrom(type))
				_object = unityObject;
			else if (type.IsSerializable)
				_object = DeserializeFromString<System.Object>(serializedSystemObject);
			else 
			{ // Unserializable type, it will be recreated from the type (and even it's serializable members)
				_object = Activator.CreateInstance(type);
			}

			// Not always critical. Can happen if GC or Unity deleted those references
			// if (_object == null)
			//	Debug.LogWarning ("Could not deserialize object of type '" + type.Name + "'!");
		}

		#endregion

		#region Embedded Util

		/// <summary>
		/// Serializes 'value' to a string, using BinaryFormatter
		/// </summary>
		protected static string SerializeToString<T> (T value)
		{
			if (value == null)
				return null;
			try
			{
				using (MemoryStream stream = new MemoryStream())
				{
					new BinaryFormatter().Serialize(stream, value);
					stream.Flush();
					return Convert.ToBase64String(stream.ToArray());
				}
			}
			catch (System.Runtime.Serialization.SerializationException)
			{
				Debug.LogWarning("Failed to serialize " + value.GetType().ToString());
				return null;
			}
		}

		/// <summary>
		/// Deserializes an object of type T from the string 'data'
		/// </summary>
		protected static T DeserializeFromString<T> (string data)
		{
			if (String.IsNullOrEmpty (data))
				return default(T);
			byte[] bytes = Convert.FromBase64String (data);
			using (MemoryStream stream = new MemoryStream(bytes)) 
			{
				return (T)new BinaryFormatter().Deserialize (stream);
			}
		}

		#endregion
	}
}