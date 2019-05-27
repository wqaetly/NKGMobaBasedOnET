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

    [AttributeUsage(AttributeTargets.All, AllowMultiple = true)]
    public class TitleAttribute: System.Attribute
    {
        public bool Bold;

        public TitleAttribute(string value)
        {
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
    }

    [AttributeUsage(AttributeTargets.All, AllowMultiple = true)]
    public class InfoBoxAttribute: System.Attribute
    {
        public InfoBoxAttribute(string value)
        {
        }
    }

    [AttributeUsage(AttributeTargets.All, AllowMultiple = true)]
    public class TabGroupAttribute: System.Attribute
    {
        public TabGroupAttribute(string value)
        {
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
        public GUIColorAttribute(float r, float g, float b)
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
    public class HideIfAttribute: System.Attribute
    {
        public HideIfAttribute(string value, object m)
        {
        }
    }
}