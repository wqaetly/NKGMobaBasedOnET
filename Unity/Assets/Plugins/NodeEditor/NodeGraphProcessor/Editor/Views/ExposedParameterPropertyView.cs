using UnityEngine.UIElements;
using UnityEditor.UIElements;
using UnityEditor;
using UnityEngine;

namespace GraphProcessor
{
	public class ExposedParameterPropertyView : VisualElement
	{
		protected BaseGraphView baseGraphView;

		public ExposedParameter parameter { get; private set; }

		public Toggle     hideInInspector { get; private set; }

		public ExposedParameterPropertyView(BaseGraphView graphView, ExposedParameter param)
		{
			baseGraphView = graphView;
			parameter      = param;

			var settingField = graphView.exposedParameterFactory.GetParameterSettingsField(param, (newValue) => {
				param.settings = newValue as ExposedParameter.Settings;
			});

			var valueField = graphView.exposedParameterFactory.GetParameterValueField(param, (newValue) =>
			{
				param.value = newValue;
				//serializedObject.ApplyModifiedProperties();
				baseGraphView.graph.NotifyExposedParameterValueChanged(param);
			});

			Add(valueField);
			
			Add(settingField);
		}
	}
} 