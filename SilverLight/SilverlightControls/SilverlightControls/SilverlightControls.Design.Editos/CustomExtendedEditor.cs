using Microsoft.Windows.Design.PropertyEditing;

namespace SilverlightControls.Design.Editos
{
    public class CustomExtendedEditor : ExtendedPropertyValueEditor
    {
        public CustomExtendedEditor()
        {
            this.InlineEditorTemplate = EditorDictionary.SimpleTextBox;
            this.ExtendedEditorTemplate = EditorDictionary.HelloWorldListBox;
        }
    }
}