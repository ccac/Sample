using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using Microsoft.Windows.Design.PropertyEditing;

namespace SilverlightControls.Design.Editos
{
    public class CustomInlineEditor : PropertyValueEditor
    {
        public CustomInlineEditor()
        {
            this.InlineEditorTemplate = EditorDictionary.ColorsComboBox;
        }
    }

    public class CustomCategoryEditor : CategoryEditor
    {
        public override string TargetCategory
        {
            get { return "My Category"; }
        }

        public override DataTemplate EditorTemplate
        {
            get { return EditorDictionary.MyCategoryEditor; }
        }

        public override bool ConsumesProperty(PropertyEntry property)
        {
            return true;
        }

        public override object GetImage(Size desiredSize)
        {
            return null;
        }
    }
}
