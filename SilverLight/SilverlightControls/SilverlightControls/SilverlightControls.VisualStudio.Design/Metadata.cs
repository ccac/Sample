using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Windows.Design;
using Microsoft.Windows.Design.Metadata;

namespace SilverlightControls.VisualStudio.Design
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
                    _customAttributes = new DeveloperMetadataBuilder().CreateTable();
                }
                return _customAttributes;
            }
        }

        private class DeveloperMetadataBuilder : AttributeTableBuilder
        {
            public DeveloperMetadataBuilder()
            {
             //   AddTypeAttributes(typeof(myControl), new ToolboxBrowsableAttribute(false));
            }

            private void AddTypeAttributes(Type type, params Attribute[] attribs)
            {
                base.AddCallback(type, delegate(AttributeCallbackBuilder builder)
                {
                    builder.AddCustomAttributes(attribs);
                });
            }

            private void AddMemberAttributes(Type type, string memberName, params Attribute[] attribs)
            {
                base.AddCallback(type, delegate(AttributeCallbackBuilder builder)
                {
                    builder.AddCustomAttributes(memberName, attribs);
                });
            }
        }
    }
}
