using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace Slate.ActionClips
{

    [Category("Character")]
    [Description("Sets a collection of BlendShapes (an expression), which you can create in the Character Component Inspector of the actor.")]
    public class CharacterExpression : ActorActionClip<Character>
    {

        [SerializeField]
        [HideInInspector]
        private float _length = 1f;
        [SerializeField]
        [HideInInspector]
        private float _blendIn = 0.25f;
        [SerializeField]
        [HideInInspector]
        private float _blendOut = 0.25f;

        [HideInInspector]
        public string expressionName;
        [HideInInspector]
        public string expressionUID;

        [AnimatableParameter(0, 1)]
        public float weight = 1;

        private float originalWeight;
        private BlendShapeGroup expression;

        public override string info {
            get
            {
                var exp = actor != null ? ResolveExpression() : null;
                return string.Format("Expression '{0}'", exp != null ? exp.name : "NONE");
            }
        }

        public override bool isValid {
            get { return actor != null; }
        }

        public override float length {
            get { return _length; }
            set { _length = value; }
        }

        public override float blendIn {
            get { return _blendIn; }
            set { _blendIn = value; }
        }

        public override float blendOut {
            get { return _blendOut; }
            set { _blendOut = value; }
        }

        BlendShapeGroup ResolveExpression() {
            if ( !string.IsNullOrEmpty(expressionUID) ) { return actor.FindExpressionByUID(expressionUID); } else { return actor.FindExpressionByName(expressionName); }
        }

        protected override void OnEnter() {
            expression = ResolveExpression();
            if ( expression != null ) {
                originalWeight = expression.weight;
            }
        }

        protected override void OnUpdate(float deltaTime) {
            if ( expression != null ) {
                var value = Easing.Ease(EaseType.QuadraticInOut, originalWeight, weight, GetClipWeight(deltaTime));
                expression.weight = value;
            }
        }

        protected override void OnReverse() {
            if ( expression != null ) {
                expression.weight = originalWeight;
                expression = null;
            }
        }
    }
}