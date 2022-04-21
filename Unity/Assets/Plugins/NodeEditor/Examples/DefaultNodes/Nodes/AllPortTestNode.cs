using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GraphProcessor;
using System.Linq;
using Sirenix.OdinInspector;

[System.Serializable, NodeMenuItem("Custom/AllPortTestNode")]
public class AllPortTestNode : BaseNode
{
    [Input(name = "input_Boolean")] public bool input_Boolean;
    [Input(name = "input_Int")] public int input_Int;
    [Input(name = "input_long")] public long input_long;
    [Input(name = "input_Float")] public float input_Float;
    [Input(name = "input_Double")] public double input_Double;
    [Input(name = "input_String")] public string input_String;
    [Input(name = "input_Object")] public object input_Object;
    [Input(name = "input_Array"),ShowPortIcon(IconNameMatchedInUSSFile = "Array"), TableMatrix(HorizontalTitle = "input_Array")] public int[,] input_Array = new int[3,3];
    [Input(name = "input_Vector2")] public Vector2 input_Vector2;
    [Input(name = "input_Vector3")] public Vector3 input_Vector3;
    [Input(name = "input_Vector4")] public Vector4 input_Vector4;
    [Input(name = "input_Quaternion")] public Quaternion input_Quaternion;
    [Input(name = "input_Color")] public Color output_Color;
    [Input(name = "input_HashSet"), ShowPortIcon(ShowIcon = true, IconNameMatchedInUSSFile = "Object")] public HashSet<string> output_HashSet;
    
    [Output(name = "output_List"), ShowPortIcon(ShowIcon = true, IconNameMatchedInUSSFile = "Object")] public List<string> output_List;
    [Output(name = "output_Dictionary"), ShowPortIcon(ShowIcon = true, IconNameMatchedInUSSFile = "Object")] public Dictionary<string,string> output_Dictionary;
    [Output(name = "output_IfCondition"), ShowPortIcon(ShowIcon = true, IconNameMatchedInUSSFile = "IfCondition")] public bool output_IfCondition;
    [Output(name = "output_ForEach"), ShowPortIcon(ShowIcon = true, IconNameMatchedInUSSFile = "ForEachLoop")] public bool output_ForEach;
    [Output(name = "output_WhileLoop"), ShowPortIcon(ShowIcon = true, IconNameMatchedInUSSFile = "WhileLoop")] public bool output_WhileLoop;
    [Output(name = "output_Action"), ShowPortIcon(ShowIcon = true, IconNameMatchedInUSSFile = "Action")] public bool output_Action;
    [Output(name = "output_GameObject")] public GameObject output_GameObject;
    [Output(name = "output_KeyCode")] public KeyCode output_KeyCode;
    [Output(name = "output_Material")] public Material output_Material;
    [Output(name = "output_Matrix4x4"), ShowPortIcon(ShowIcon = true, IconNameMatchedInUSSFile = "Matrix")] public Matrix4x4 output_Matrix4x4;
    [Output(name = "output_RaycastHit")] public RaycastHit output_RaycastHit;
    [Output(name = "output_Rigidbody")] public Rigidbody output_Rigidbody;
    [Output(name = "output_Transform")] public Transform output_Transform;
    [Output(name = "output_Texture2D")] public Texture2D output_Texture2D;

    public override string name => "AllPortTestNode";

    protected override void Process()
    {
        //output = input * 42;
    }
}