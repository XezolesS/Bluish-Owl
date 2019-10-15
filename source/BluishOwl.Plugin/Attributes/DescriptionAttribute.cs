using System;
using System.Collections.Generic;
using System.Text;

namespace BluishOwl.Plugin
{
    [AttributeUsage(AttributeTargets.Class)]
    public class DescriptionAttribute : Attribute
    {
        public string Text { get; private set; }

        public DescriptionAttribute(string text = null)
        {
            this.Text = text;
        }
    }
}
