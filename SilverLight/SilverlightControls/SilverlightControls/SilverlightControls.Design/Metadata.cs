using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows;
using Microsoft.Windows.Design;
using Microsoft.Windows.Design.Metadata;
using Microsoft.Windows.Design.PropertyEditing;
using SilverlightControls.Design.Editos;

namespace SilverlightControls.Design
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
                AddTypeAttributes(typeof(myControl),
                    new DescriptionAttribute("I am a control"));

                AddMemberAttributes(typeof(myControl), "MyStringProperty",
                    new DescriptionAttribute("I am a property"),
                    new DisplayNameAttribute("My String Property"),
                    new CategoryAttribute("My Category"),
                    new PropertyOrderAttribute(PropertyOrder.Early),
                    PropertyValueEditor.CreateEditorAttribute(typeof(CustomDialogEditor)));
                // PropertyValueEditor.CreateEditorAttribute(typeof(CustomInlineEditor))
                // PropertyValueEditor.CreateEditorAttribute(typeof(CustomExtendedEditor))

                AddMemberAttributes(typeof(myControl), "MyIntProperty",
                    new CategoryAttribute("My Category"),
                    new PropertyOrderAttribute(PropertyOrder.Late));
                // BrowsableAttribute.No

                AddMemberAttributes(typeof(myControl), "MyObjectProperty",
                    new CategoryAttribute("My Category"),
                    new TypeConverterAttribute(typeof(ExpandableObjectConverter)),
                    new PropertyOrderAttribute(PropertyOrder.CreateAfter(PropertyOrder.Early)));
                // new TypeConverterAttribute(typeof(BooleanConverter))
                // new TypeConverterAttribute(typeof(myConverter))

                AddTypeAttributes(typeof(myOtherControl),
                    new ToolboxBrowsableAttribute(false));


                //AddCustomAttributes(typeof(myControl), "My Category",
                //    PropertyValueEditor.CreateEditorAttribute(typeof(CustomCategoryEditor)));
            }

            public class myTypeConverter : TypeConverter
            {
                public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
                {
                    return true;
                }

                public override TypeConverter.StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
                {
                    return new StandardValuesCollection(new string[] { "Hello", "World", "foo", "bar" });
                }
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


}
// BindingFlags bindingFlags = BindingFlags.NonPublic | BindingFlags.Instance;
//  new CategoryAttribute(myControl.WidthProperty.GetType().GetProperty("Name", bindingFlags).GetValue(myControl.WidthProperty, new object[0]).ToString()),
