using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using Microsoft.Expression.Framework.ValueEditors;
using Microsoft.Windows.Design;
using Microsoft.Windows.Design.Metadata;
using Microsoft.Windows.Design.PropertyEditing;

namespace SilverlightControls.Expression.Design
{
    public class MetadataRegistration : IRegisterMetadata
    {
        private static AttributeTable _customAttributes;
        private static bool _initialized;

        public void Register()
        {
            if (!_initialized)
            {
                MetadataStore.AddAttributeTable(CustomAttributes);
                _initialized = true;
            }
        }

        public static AttributeTable CustomAttributes
        {
            get
            {
                if (_customAttributes == null)
                {
                    _customAttributes = new CustomMetadataBuilder().CreateTable();
                }
                return _customAttributes;
            }
        }

        private class CustomMetadataBuilder : AttributeTableBuilder
        {
            public CustomMetadataBuilder()
            {
                //AddMemberAttributes(typeof (myControl), "MyIntProperty",
                //        new EditorBrowsableAttribute(EditorBrowsableState.Advanced));

                AddMemberAttributes(typeof(myControl), "MyIntProperty",
                    new NumberRangesAttribute(null, 1, 100, null, null),
                    new NumberIncrementsAttribute(0.5, 1, 5),
                    new NumberFormatAttribute("0'%'", null, 10));
            }

            private void AddTypeAttributes(Type type, params Attribute[] attribs)
            {
                base.AddCallback(type, builder => builder.AddCustomAttributes(attribs));
            }

            private void AddMemberAttributes(Type type, string memberName, params Attribute[] attribs)
            {
                base.AddCallback(type, builder => builder.AddCustomAttributes(memberName, attribs));
            }
        }
    }


    internal class ItemsControlItemsNewItemFactory : NewItemFactory
    {
        public override object CreateInstance(Type type)
        {
            return Activator.CreateInstance(type);
        }

        public override string GetDisplayName(Type type)
        {
            return type.Name;
        }
    }



}
