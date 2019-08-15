//#define UNDO_DEBUG

namespace UndoPro 
{
	#if UNITY_EDITOR

	using UnityEngine;
	using UnityEditor;
	using System;
	using System.Reflection;
	using System.Collections.Generic;
	using System.Linq;

	/// <summary>
	/// Manager for UndoPro, enabling an action-based undo workflow integrated into the default Unity Undo system and providing detailed callbacks for the Undo system
	/// </summary>
	[InitializeOnLoad]
	public static class UndoProManager
	{
		public static bool enabled;

		private static UnityEngine.Object dummyObject;

		private static Action<object, object> getRecordsInternalDelegate;

		public static UndoProRecords records;

		public static Action<string[]> OnUndoPerformed;
		public static Action<string[]> OnRedoPerformed;

		public static Action<string[], bool> OnAddUndoRecord;

		#region General 

		[MenuItem("Edit/Reset UndoPro", false, 10)]
		private static void ResetUndoPro()
		{
			CreateRecords();
		}

		[MenuItem ("Edit/Toggle UndoPro", false, 10)]
		private static void ToggleUndoPro () 
		{
			if (!enabled) 
			{
				EnableUndoPro ();
				if (SceneView.lastActiveSceneView != null) 
				{
					SceneView.lastActiveSceneView.ShowNotification (new GUIContent ("Undo Pro Enabled!"));
					SceneView.lastActiveSceneView.Repaint ();
				}
			}
			else 
			{
				DisableUndoPro ();
				if (SceneView.lastActiveSceneView != null)
				{
					SceneView.lastActiveSceneView.ShowNotification (new GUIContent ("Undo Pro Disabled!"));
					SceneView.lastActiveSceneView.Repaint ();
				}
			}
		}

		static UndoProManager ()
		{
			EnableUndoPro ();
		}

		public static void EnableUndoPro () 
		{
			enabled = true;

			// Assure it is subscribed to all necessary events for undo/redo recognition
			Undo.undoRedoPerformed -= UndoRedoPerformed;
			Undo.undoRedoPerformed += UndoRedoPerformed;
			EditorApplication.update -= Update;
			EditorApplication.update += Update;
			EditorApplication.playModeStateChanged -= PlaymodeStateChange;
			EditorApplication.playModeStateChanged += PlaymodeStateChange;

			// Fetch Reflection members for Undo interaction
			Assembly UnityEditorAsssembly = Assembly.GetAssembly (typeof(UnityEditor.Editor));
			Type undoType = UnityEditorAsssembly.GetType ("UnityEditor.Undo");
			MethodInfo getRecordsInternal = undoType.GetMethod ("GetRecordsInternal", BindingFlags.NonPublic | BindingFlags.Static);
			getRecordsInternalDelegate = (Action<object, object>)Delegate.CreateDelegate (typeof(Action<object, object>), getRecordsInternal);

			// Create dummy object
			if (dummyObject == null)
				dummyObject = new Texture2D (8, 8);

			// Setup default undo state and record
			AssureRecords ();
		}

		private static void AssureRecords () 
		{
			if (records == null)
			{
				records = GameObject.FindObjectOfType<UndoProRecords> ();
				if (records == null)
					CreateRecords ();
		#if UNDO_DEBUG
				else Debug.Log ("Found undo records in scene!");
		#endif
			}
			if (records.undoState == null) 
			{
		#if UNDO_DEBUG
				Debug.Log ("UndoState recreated!");
		#endif
				records.undoState = FetchUndoState ();
			}
		}

		private static void CreateRecords () 
		{
		#if UNDO_DEBUG
			Debug.Log ("Creating scene undo records!");
		#endif

			if (records != null)
				UnityEngine.Object.DestroyImmediate (records.gameObject);

			GameObject recordsGO = new GameObject ("UndoProRecords");
		#if !UNDO_DEBUG
			recordsGO.hideFlags = HideFlags.HideInHierarchy | HideFlags.HideInInspector;
		#endif
			records = recordsGO.AddComponent<UndoProRecords> ();
		}

		public static void DisableUndoPro () 
		{
			enabled = false;

			// Unsubscribe from every event
			Undo.undoRedoPerformed -= UndoRedoPerformed;
			EditorApplication.update -= Update;
			EditorApplication.playModeStateChanged -= PlaymodeStateChange;

			// Discard now unused objects
			dummyObject = null;
			getRecordsInternalDelegate = null;
			records = null;
		}

		#endregion

		#region Custom Undo Recording

		private static bool inRecordStack = false;
		private static bool firstInRecordStack = true;

		/// <summary>
		/// Begin merging multiple singular undo operations into one group that get's treated as one
		/// </summary>
		public static void BeginRecordStack()
		{
			inRecordStack = true;
			firstInRecordStack = true;
			Undo.IncrementCurrentGroup();
		}

		/// <summary>
		/// End merging multiple singular undo operations into one group that get's treated as one
		/// </summary>
		public static void EndRecordStack()
		{
			Undo.FlushUndoRecordObjects();
			Undo.IncrementCurrentGroup();
			inRecordStack = false;
		}

		/// <summary>
		/// Records a custom operation with given label and actions and executes the operation (perform)
		/// </summary>
		public static void RecordOperationAndPerform (Action perform, Action undo, string label, bool mergeBefore = false, bool mergeAfter = false) 
		{
			RecordOperation (new UndoProRecord (perform, undo, label, 0), mergeBefore, mergeAfter);
			if (perform != null)
				perform.Invoke ();
		}

		/// <summary>
		/// Records a custom operation with given label and actions
		/// </summary>
		public static void RecordOperation (Action perform, Action undo, string label, bool mergeBefore = false, bool mergeAfter = false)
		{
			RecordOperation (new UndoProRecord (perform, undo, label, 0), mergeBefore, mergeAfter);
		}

		/// <summary>
		/// Records the given operation
		/// </summary>
		private static void RecordOperation (UndoProRecord operation, bool mergeBefore = false, bool mergeAfter = false)
		{
			// First, make sure the internal records representation is updated
			UpdateUndoRecords ();

			// Make sure this record isn't included in the previous group
			if (!mergeBefore && !inRecordStack)
				Undo.IncrementCurrentGroup ();

			// Create a dummy record with the given label
			if (dummyObject == null)
				dummyObject = new Texture2D (8, 8);
			Undo.RegisterCompleteObjectUndo (dummyObject, operation.label);

			// Make sure future undo records are not included into this group
			if (!mergeAfter && !inRecordStack)
			{
				Undo.FlushUndoRecordObjects();
				Undo.IncrementCurrentGroup();
			}

			// Now get the new Undo state
			records.undoState = FetchUndoState ();

			// Record operation internally
			if (!inRecordStack || firstInRecordStack)
				records.UndoRecordsAdded (1);
			firstInRecordStack = false;
			records.undoProRecords.Add (operation);

			if (OnAddUndoRecord != null)
				OnAddUndoRecord.Invoke (new string[] { operation.label }, true);
		}

		#endregion

		#region Undo/Redo Tracking

		private static bool lastFrameUndoRedoPerformed = false;

		/// <summary>
		/// Checks if new undo records were added
		/// </summary>
		private static void Update ()
		{
			if (!lastFrameUndoRedoPerformed)
			{ // Only handle the case of possible undo addition, but not when an undo or redo was performed
				UpdateUndoRecords ();
			}
			lastFrameUndoRedoPerformed = false;
		}

		private static void PlaymodeStateChange(PlayModeStateChange change)
		{
			if (change == PlayModeStateChange.EnteredEditMode)
				UpdateUndoRecords();
		}

		/// <summary>
		/// Check the current undoState for any added undo records and updates the internal records accordingly
		/// </summary>
		private static void UpdateUndoRecords () 
		{
			AssureRecords ();

			// Get new UndoState
			UndoState prevState = records.undoState;
			records.undoState = FetchUndoState ();
			UndoState newState = records.undoState;

			// Detect additions to the record through comparision of the old and the new UndoState
			if (prevState.undoRecords.Count == newState.undoRecords.Count)
				return; // No undo record was added for sure

			// Fetch new undo records
			int addedUndoCount = newState.undoRecords.Count-prevState.undoRecords.Count;
			if (addedUndoCount < 0)
			{ // This happens only when the undo was erased, for example after switching the scene
#if UNDO_DEBUG
				string[] undosRemoved = prevState.undoRecords.GetRange(prevState.undoRecords.Count + addedUndoCount, -addedUndoCount).ToArray();
				string undoRemLog = "" + (-addedUndoCount) + " undo records removed: ";
				for (int undoCnt = 0; undoCnt < undosRemoved.Length; undoCnt++)
					undoRemLog += undosRemoved[undoCnt] + "; ";
				Debug.Log(undoRemLog);
#endif

				if (newState.undoRecords.Count != 0)
				{ // Attempt to salvage the undo records that are left
					records.UndoRecordsAdded(addedUndoCount);
					records.ClearRedo ();
					Debug.LogWarning ("Cleared Redo because some undos were removed!");
				}
				else
					CreateRecords ();
				return;
			}

			// Update internals
			records.UndoRecordsAdded (addedUndoCount);

			// Callback
			string[] undosAdded = newState.undoRecords.GetRange (newState.undoRecords.Count-addedUndoCount, addedUndoCount).ToArray ();
			if (OnAddUndoRecord != null)
				OnAddUndoRecord.Invoke (undosAdded, newState.redoRecords.Count == 0);

		#if UNDO_DEBUG
			// Debug added undo records
			string undoLog = undosAdded.Length + " undo records added: ";
			for (int undoCnt = 0; undoCnt < undosAdded.Length; undoCnt++)
				undoLog += undosAdded[undoCnt] + "; ";
			Debug.Log (undoLog);
		#endif
		}

		private static UndoState FetchUndoState () 
		{
			UndoState newUndoState = new UndoState ();
			getRecordsInternalDelegate.Invoke (newUndoState.undoRecords, newUndoState.redoRecords);
			return newUndoState;
		}

		#endregion

		#region UndoPro Record tracking

		/// <summary>
		/// Callback recognising the type of record, calling the apropriate callback and handling undo pro records
		/// </summary>
		private static void UndoRedoPerformed () 
		{
			lastFrameUndoRedoPerformed = true;
			AssureRecords ();

			// Get new UndoState
			UndoState prevState = records.undoState;
			UndoState newState = records.undoState = FetchUndoState ();

			// Detect undo/redo
			int addedRecordCount;
			int change = DetectStateChange (prevState, newState, out addedRecordCount);
			if (change == 0) // Nothing happend; Only possible if Undo/Redo stack was empty
				return;

			List<UndoProRecord> operatedRecords = records.PerformOperationInternal (change, addedRecordCount);

			if (change < 0)
			{ // UNDO operation
				foreach (UndoProRecord undoRecord in operatedRecords)
				{ // Invoke undo operations
					if (undoRecord.undo != null)
						undoRecord.undo.Invoke ();
				}
				// Callback for whole group
				if (OnUndoPerformed != null)
					OnUndoPerformed.Invoke (operatedRecords.Select ((UndoProRecord record) => record.label).ToArray ());
			}
			else
			{ // REDO operation
				foreach (UndoProRecord redoRecord in operatedRecords)
				{ // Invoke redo operations
					if (redoRecord.perform != null)
						redoRecord.perform.Invoke ();
				}
				// Callback for whole group
				if (OnRedoPerformed != null)
					OnRedoPerformed.Invoke (operatedRecords.Select ((UndoProRecord record) => record.label).ToArray ());
			}
		}

		/// <summary>
		/// Detects an UndoState change as either an undo or redo operation
		/// A positive return value indicates a redo operation, a negative an undo operation;
		/// If it is 0, then the adressed stack (undo/redo) was empty
		/// The absolute value is the number of records that were adressed, means the size of the group
		/// It is possible that the group size changed due to an anomaly, so count of records added by the anomaly is put into addedRecordsCount
		/// </summary>
		private static int DetectStateChange (UndoState prevState, UndoState nextState, out int addedRecordsCount)
		{
			addedRecordsCount = 0;

			int prevUndoCount = prevState.undoRecords.Count, prevRedoCount = prevState.redoRecords.Count;
			int nextUndoCount = nextState.undoRecords.Count, nextRedoCount = nextState.redoRecords.Count;
			int undoChange = nextUndoCount-prevUndoCount, redoChange = nextRedoCount-prevRedoCount;

			// Check if the action is undo or redo
			bool undoAction = undoChange < 0, redoAction = redoChange < 0;
			if ((!redoAction && prevUndoCount == 0) || (!undoAction && prevRedoCount == 0)) // Tried to undo/redo with an empty record stack
				return 0;
			if (!undoAction && !redoAction)
				throw new Exception ("Detected neither redo nor undo operation!");
			int recordChange = undoAction? Math.Abs (undoChange) : Math.Abs (redoChange);

		#if UNDO_DEBUG
			Debug.Log ("Detected " + (undoAction? "UNDO" : "REDO") + " of " + recordChange + " initial records!");
		#endif

			if (redoChange != -undoChange)
			{ // This anomaly happens only for records that trigger other undo/redo operations
				// -> only known case: Reparent unselected object in hierarchy, each iteration (undo/redo) of the issued record a 'Parenting' record gets added ontop
				addedRecordsCount = undoAction? (Math.Abs (redoChange)-Math.Abs (undoChange)) : (Math.Abs (undoChange)-Math.Abs (redoChange));
		#if UNDO_DEBUG
				Debug.LogWarning ("Due to an anomaly a difference of " + addedRecordsCount + " records was created during " + (undoAction? "undo" : "redo") + 
					" where undo change was " + undoChange + " and redo change " + redoChange);
		#endif
			}

			return (undoAction? -recordChange : recordChange); // Return the count of initially changed records
		}

		#endregion
	}

#endif
}