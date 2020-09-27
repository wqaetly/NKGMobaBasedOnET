using ETModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sirenix.OdinInspector
{
    [AttributeUsage(AttributeTargets.All, AllowMultiple = true)]
    public class LabelTextAttribute: System.Attribute
    {
        public LabelTextAttribute(string value)
        {
        }
    }

    [AttributeUsage(AttributeTargets.All, AllowMultiple = true)]
    public class HideLabelAttribute: System.Attribute
    {
        public HideLabelAttribute()
        {
        }
    }

    public sealed class HideIfAttribute: Attribute
    {
        /// <summary>
        /// Name of a bool field, property or function to show or hide the property.
        /// </summary>
        public string MemberName;

        /// <summary>The optional member value.</summary>
        public object Value;

        /// <summary>
        /// Whether or not to slide the property in and out when the state changes.
        /// </summary>
        public bool Animate;

        /// <summary>
        /// Hides a property in the inspector, if the specified member returns true.
        /// </summary>
        /// <param name="memberName">Name of a bool field, property or function to show or hide the property.</param>
        /// <param name="animate">Whether or not to slide the property in and out when the state changes.</param>
        public HideIfAttribute(string memberName, bool animate = true)
        {
            this.MemberName = memberName;
            this.Animate = animate;
        }

        /// <summary>
        /// Hides a property in the inspector, if the specified member returns the specified value.
        /// </summary>
        /// <param name="memberName">Name of member to check the value of.</param>
        /// <param name="optionalValue">The value to check for.</param>
        /// <param name="animate">Whether or not to slide the property in and out when the state changes.</param>
        public HideIfAttribute(string memberName, object optionalValue, bool animate = true)
        {
            this.MemberName = memberName;
            this.Value = optionalValue;
            this.Animate = animate;
        }
    }

    public enum TitleAlignments
    {
        /// <summary>Title and subtitle left aligned.</summary>
        Left,

        /// <summary>Title and subtitle centered aligned.</summary>
        Centered,

        /// <summary>Title and subtitle right aligned.</summary>
        Right,

        /// <summary>Title on the left, subtitle on the right.</summary>
        Split,
    }

    public class TitleAttribute: Attribute
    {
        /// <summary>
        /// The title displayed above the property in the inspector.
        /// </summary>
        public string Title;

        /// <summary>Optional subtitle.</summary>
        public string Subtitle;

        /// <summary>
        /// If <c>true</c> the title will be displayed with a bold font.
        /// </summary>
        public bool Bold;

        /// <summary>
        /// Gets a value indicating whether or not to draw a horizontal line below the title.
        /// </summary>
        public bool HorizontalLine;

        /// <summary>Title alignment.</summary>
        public TitleAlignments TitleAlignment;

        /// <summary>Creates a title above any property in the inspector.</summary>
        /// <param name="title">The title displayed above the property in the inspector.</param>
        /// <param name="subtitle">Optional subtitle</param>
        /// <param name="titleAlignment">Title alignment</param>
        /// <param name="horizontalLine">Horizontal line</param>
        /// <param name="bold">If <c>true</c> the title will be drawn with a bold font.</param>
        public TitleAttribute(
        string title,
        string subtitle = null,
        TitleAlignments titleAlignment = TitleAlignments.Left,
        bool horizontalLine = true,
        bool bold = true)
        {
            this.Title = title ?? "null";
            this.Subtitle = subtitle;
            this.Bold = bold;
            this.TitleAlignment = titleAlignment;
            this.HorizontalLine = horizontalLine;
        }
    }

    [AttributeUsage(AttributeTargets.All, AllowMultiple = true)]
    public class DictionaryDrawerSettings: System.Attribute
    {
        public string KeyLabel;
        public string ValueLabel;

        public DictionaryDrawerSettings()
        {
        }
    }

    [AttributeUsage(AttributeTargets.All, AllowMultiple = true)]
    public class TitleGroup: System.Attribute
    {
        public TitleGroup(string value)
        {
        }
    }

    [AttributeUsage(AttributeTargets.All, AllowMultiple = true)]
    public class LabelWidthAttribute: System.Attribute
    {
        public LabelWidthAttribute(int value)
        {
        }
    }

    [AttributeUsage(AttributeTargets.All, AllowMultiple = true)]
    public class ShowIfAttribute: System.Attribute
    {
        public ShowIfAttribute(string value)
        {
        }

        public ShowIfAttribute(string name, object value)
        {
        }
    }

    public enum InfoMessageType
    {
        /// <summary>Generic message box with no type.</summary>
        None,

        /// <summary>Information message box.</summary>
        Info,

        /// <summary>Warning message box.</summary>
        Warning,

        /// <summary>Error message box.</summary>
        Error,
    }

    public sealed class InfoBoxAttribute: Attribute
    {
        /// <summary>The message to display in the info box.</summary>
        public string Message;

        /// <summary>The type of the message box.</summary>
        public InfoMessageType InfoMessageType;

        /// <summary>
        /// Optional member field, property or function to show and hide the info box.
        /// </summary>
        public string VisibleIf;

        /// <summary>
        /// When <c>true</c> the InfoBox will ignore the GUI.enable flag and always draw as enabled.
        /// </summary>
        public bool GUIAlwaysEnabled;

        /// <summary>Displays an info box above the property.</summary>
        /// <param name="message">The message for the message box. Supports referencing a member string field, property or method by using $.</param>
        /// <param name="infoMessageType">The type of the message box.</param>
        /// <param name="visibleIfMemberName">Name of member bool to show or hide the message box.</param>
        public InfoBoxAttribute(
        string message,
        InfoMessageType infoMessageType = InfoMessageType.Info,
        string visibleIfMemberName = null)
        {
            this.Message = message;
            this.InfoMessageType = infoMessageType;
            this.VisibleIf = visibleIfMemberName;
        }

        /// <summary>Displays an info box above the property.</summary>
        /// <param name="message">The message for the message box. Supports referencing a member string field, property or method by using $.</param>
        /// <param name="visibleIfMemberName">Name of member bool to show or hide the message box.</param>
        public InfoBoxAttribute(string message, string visibleIfMemberName)
        {
            this.Message = message;
            this.InfoMessageType = InfoMessageType.Info;
            this.VisibleIf = visibleIfMemberName;
        }
    }

    public sealed class HideInInspector: Attribute
    {
    }

    [AttributeUsage(AttributeTargets.All, AllowMultiple = true)]
    public class TabGroupAttribute: System.Attribute
    {
        public TabGroupAttribute(string value)
        {
        }
    }

    public class TooltipAttribute: PropertyAttribute
    {
        /// <summary>
        ///   <para>The tooltip text.</para>
        /// </summary>
        public readonly string tooltip;

        /// <summary>
        ///   <para>Specify a tooltip for a field.</para>
        /// </summary>
        /// <param name="tooltip">The tooltip text.</param>
        public TooltipAttribute(string tooltip)
        {
            this.tooltip = tooltip;
        }
    }

    [AttributeUsage(AttributeTargets.All, AllowMultiple = true)]
    public class HideInEditorModeAttribute: System.Attribute
    {
        public HideInEditorModeAttribute()
        {
        }
    }

    [AttributeUsage(AttributeTargets.All, AllowMultiple = true)]
    public class ListDrawerSettingsAttribute: System.Attribute
    {
        public bool ShowItemCount;
        public bool ShowIndexLabels;

        public ListDrawerSettingsAttribute()
        {
        }
    }

    [AttributeUsage(AttributeTargets.All, AllowMultiple = true)]
    public class GUIColorAttribute: System.Attribute
    {
        public GUIColorAttribute(float r, float g, float b, float a = 0)
        {
        }
    }

    [AttributeUsage(AttributeTargets.All, AllowMultiple = true)]
    public class ReadOnlyAttribute: System.Attribute
    {
        public ReadOnlyAttribute()
        {
        }
    }

    [AttributeUsage(AttributeTargets.All, AllowMultiple = true)]
    public class MultiLinePropertyAttribute: System.Attribute
    {
        public MultiLinePropertyAttribute(int value)
        {
        }
    }

    [AttributeUsage(AttributeTargets.All, AllowMultiple = true)]
    public class HorizontalGroupAttribute: System.Attribute
    {
        public HorizontalGroupAttribute()
        {
        }
    }

    [AttributeUsage(AttributeTargets.All, AllowMultiple = true)]
    public class DisableInEditorModeAttribute: System.Attribute
    {
    }

    [AttributeUsage(AttributeTargets.All, AllowMultiple = true)]
    public class EnumToggleButtonsAttribute: System.Attribute
    {
    }

    [AttributeUsage(AttributeTargets.All, AllowMultiple = true)]
    public class MinValueAttribute: System.Attribute
    {
        public MinValueAttribute(int i)
        {
        }
    }

    public class BoxGroupAttribute: PropertyGroupAttribute
    {
        /// <summary>
        /// If <c>true</c> a label for the group will be drawn on top.
        /// </summary>
        public bool ShowLabel;

        /// <summary>
        /// If <c>true</c> the header label will be places in the center of the group header. Otherwise it will be in left side.
        /// </summary>
        public bool CenterLabel;

        /// <summary>Adds the property to the specified box group.</summary>
        /// <param name="group">The box group.</param>
        /// <param name="showLabel">If <c>true</c> a label will be drawn for the group.</param>
        /// <param name="centerLabel">If set to <c>true</c> the header label will be centered.</param>
        /// <param name="order">The order of the group in the inspector.</param>
        public BoxGroupAttribute(string group, bool showLabel = true, bool centerLabel = false, int order = 0)
                : base(group, order)
        {
            this.ShowLabel = showLabel;
            this.CenterLabel = centerLabel;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Sirenix.OdinInspector.BoxGroupAttribute" /> class. Use the other constructor overloads in order to show a header-label on the box group.
        /// </summary>
        public BoxGroupAttribute()
                : this("_DefaultBoxGroup", false, false, 0)
        {
        }

        /// <summary>Combines the box group with another group.</summary>
        /// <param name="other">The other group.</param>
        protected override void CombineValuesWith(PropertyGroupAttribute other)
        {
            BoxGroupAttribute boxGroupAttribute = other as BoxGroupAttribute;
            if (!this.ShowLabel || !boxGroupAttribute.ShowLabel)
            {
                this.ShowLabel = false;
                boxGroupAttribute.ShowLabel = false;
            }

            this.CenterLabel |= boxGroupAttribute.CenterLabel;
        }
    }

    public class ValueDropdownAttribute: Attribute
    {
        /// <summary>
        /// Name of any field, property or method member that implements IList. E.g. arrays or Lists.
        /// </summary>
        public string MemberName;

        /// <summary>
        /// The number of items before enabling search. Default is 10.
        /// </summary>
        public int NumberOfItemsBeforeEnablingSearch;

        /// <summary>False by default.</summary>
        public bool IsUniqueList;

        /// <summary>
        /// True by default. If the ValueDropdown attribute is applied to a list, then disabling this,
        /// will render all child elements normally without using the ValueDropdown. The ValueDropdown will
        /// still show up when you click the add button on the list drawer, unless <see cref="F:Sirenix.OdinInspector.ValueDropdownAttribute.DisableListAddButtonBehaviour" /> is true.
        /// </summary>
        public bool DrawDropdownForListElements;

        /// <summary>False by default.</summary>
        public bool DisableListAddButtonBehaviour;

        /// <summary>
        /// If the ValueDropdown attribute is applied to a list, and <see cref="F:Sirenix.OdinInspector.ValueDropdownAttribute.IsUniqueList" /> is set to true, then enabling this,
        /// will exclude existing values, instead of rendering a checkbox indicating whether the item is already included or not.
        /// </summary>
        public bool ExcludeExistingValuesInList;

        /// <summary>
        /// If the dropdown renders a tree-view, then setting this to true will ensure everything is expanded by default.
        /// </summary>
        public bool ExpandAllMenuItems;

        /// <summary>
        /// If true, instead of replacing the drawer with a wide dropdown-field, the dropdown button will be a little button, drawn next to the other drawer.
        /// </summary>
        public bool AppendNextDrawer;

        /// <summary>
        /// Disables the the GUI for the appended drawer. False by default.
        /// </summary>
        public bool DisableGUIInAppendedDrawer;

        /// <summary>
        /// By default, a single click selects and confirms the selection.
        /// </summary>
        public bool DoubleClickToConfirm;

        /// <summary>By default, the dropdown will create a tree view.</summary>
        public bool FlattenTreeView;

        /// <summary>
        /// Gets or sets the width of the dropdown. Default is zero.
        /// </summary>
        public int DropdownWidth;

        /// <summary>
        /// Gets or sets the height of the dropdown. Default is zero.
        /// </summary>
        public int DropdownHeight;

        /// <summary>
        /// Gets or sets the title for the dropdown. Null by default.
        /// </summary>
        public string DropdownTitle;

        /// <summary>False by default.</summary>
        public bool SortDropdownItems;

        /// <summary>Whether to draw all child properties in a foldout.</summary>
        public bool HideChildProperties;

        /// <summary>Creates a dropdown menu for a property.</summary>
        /// <param name="memberName">Name of any field, property or method member that implements IList. E.g. arrays or Lists.</param>
        public ValueDropdownAttribute(string memberName)
        {
            this.NumberOfItemsBeforeEnablingSearch = 10;
            this.MemberName = memberName;
            this.DrawDropdownForListElements = true;
        }
    }

    public sealed class OnValueChangedAttribute: Attribute
    {
        /// <summary>Name of callback member function.</summary>
        public string MethodName;

        /// <summary>
        /// Whether to invoke the method when a child value of the property is changed.
        /// </summary>
        public bool IncludeChildren;

        /// <summary>
        /// Adds a callback for when the property's value is changed.
        /// </summary>
        /// <param name="methodName">Name of the method.</param>
        /// <param name="includeChildren">Whether to invoke the method when a child value of the property is changed.</param>
        public OnValueChangedAttribute(string methodName, bool includeChildren = false)
        {
            this.MethodName = methodName;
            this.IncludeChildren = includeChildren;
        }
    }

    public class HideReferenceObjectPickerAttribute: Attribute
    {
    }

    public abstract class PropertyGroupAttribute: Attribute
    {
        /// <summary>The ID used to grouping properties together.</summary>
        public string GroupID;

        /// <summary>
        /// The name of the group. This is the last part of the group ID if there is a path, otherwise it is just the group ID.
        /// </summary>
        public string GroupName;

        /// <summary>The order of the group.</summary>
        public int Order;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Sirenix.OdinInspector.PropertyGroupAttribute" /> class.
        /// </summary>
        /// <param name="groupId">The group identifier.</param>
        /// <param name="order">The group order.</param>
        public PropertyGroupAttribute(string groupId, int order)
        {
            this.GroupID = groupId;
            this.Order = order;
            int num = groupId.LastIndexOf('/');
            this.GroupName = num < 0 || num >= groupId.Length? groupId : groupId.Substring(num + 1);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Sirenix.OdinInspector.PropertyGroupAttribute" /> class.
        /// </summary>
        /// <param name="groupId">The group identifier.</param>
        public PropertyGroupAttribute(string groupId)
                : this(groupId, 0)
        {
        }

        /// <summary>
        /// <para>Combines this attribute with another attribute of the same type.
        /// This method invokes the virtual <see cref="M:Sirenix.OdinInspector.PropertyGroupAttribute.CombineValuesWith(Sirenix.OdinInspector.PropertyGroupAttribute)" /> method to invoke custom combine logic.</para>
        /// <para>All group attributes are combined to one attribute used by a single OdinGroupDrawer.</para>
        /// <para>Example: <code>protected override void CombineValuesWith(PropertyGroupAttribute other) { this.Title = this.Title ?? (other as MyGroupAttribute).Title; }</code></para>
        /// </summary>
        /// <param name="other">The attribute to combine with.</param>
        /// <returns>The instance that the method was invoked on.</returns>
        /// <exception cref="T:System.ArgumentNullException">The argument 'other' was null.</exception>
        /// <exception cref="T:System.ArgumentException">
        /// Attributes to combine are not of the same type.
        /// or
        /// PropertyGroupAttributes to combine must have the same group id.
        /// </exception>
        public PropertyGroupAttribute Combine(PropertyGroupAttribute other)
        {
            if (other == null)
                throw new ArgumentNullException(nameof (other));
            if (other.GetType() != this.GetType())
                throw new ArgumentException("Attributes to combine are not of the same type.");
            if (other.GroupID != this.GroupID)
                throw new ArgumentException("PropertyGroupAttributes to combine must have the same group id.");
            if (this.Order == 0)
                this.Order = other.Order;
            else if (other.Order != 0)
                this.Order = Math.Min(this.Order, other.Order);
            this.CombineValuesWith(other);
            return this;
        }

        /// <summary>
        /// <para>Override this method to add custom combine logic to your group attribute. This method determines how your group's parameters combine when spread across multiple attribute declarations in the same class.</para>
        /// <para>Remember, in .NET, member order is not guaranteed, so you never know which order your attributes will be combined in.</para>
        /// </summary>
        /// <param name="other">The attribute to combine with. This parameter is guaranteed to be of the correct attribute type.</param>
        /// <example>
        /// <para>This example shows how <see cref="T:Sirenix.OdinInspector.BoxGroupAttribute" /> attributes are combined.</para>
        /// <code>
        /// protected override void CombineValuesWith(PropertyGroupAttribute other)
        /// {
        ///     // The given attribute parameter is *guaranteed* to be of type BoxGroupAttribute.
        ///     var attr = other as BoxGroupAttribute;
        /// 
        ///     // If this attribute has no label, we the other group's label, thus preserving the label across combines.
        ///     if (this.Label == null)
        ///     {
        ///         this.Label = attr.Label;
        ///     }
        /// 
        ///     // Combine ShowLabel and CenterLabel parameters.
        ///     this.ShowLabel |= attr.ShowLabel;
        ///     this.CenterLabel |= attr.CenterLabel;
        /// }
        /// </code>
        /// </example>
        protected virtual void CombineValuesWith(PropertyGroupAttribute other)
        {
        }
    }

    public sealed class TextAreaAttribute: PropertyAttribute
    {
        /// <summary>
        ///   <para>The minimum amount of lines the text area will use.</para>
        /// </summary>
        public readonly int minLines;

        /// <summary>
        ///   <para>The maximum amount of lines the text area can show before it starts using a scrollbar.</para>
        /// </summary>
        public readonly int maxLines;

        /// <summary>
        ///   <para>Attribute to make a string be edited with a height-flexible and scrollable text area.</para>
        /// </summary>
        /// <param name="minLines">The minimum amount of lines the text area will use.</param>
        /// <param name="maxLines">The maximum amount of lines the text area can show before it starts using a scrollbar.</param>
        public TextAreaAttribute()
        {
            this.minLines = 3;
            this.maxLines = 3;
        }

        /// <summary>
        ///   <para>Attribute to make a string be edited with a height-flexible and scrollable text area.</para>
        /// </summary>
        /// <param name="minLines">The minimum amount of lines the text area will use.</param>
        /// <param name="maxLines">The maximum amount of lines the text area can show before it starts using a scrollbar.</param>
        public TextAreaAttribute(int minLines, int maxLines)
        {
            this.minLines = minLines;
            this.maxLines = maxLines;
        }
    }

    public abstract class PropertyAttribute: Attribute
    {
        /// <summary>
        ///   <para>Optional field to specify the order that multiple DecorationDrawers should be drawn in.</para>
        /// </summary>
        public int order { get; set; }
    }
}