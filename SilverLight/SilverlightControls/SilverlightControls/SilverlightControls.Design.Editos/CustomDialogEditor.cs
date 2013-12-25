using Microsoft.Windows.Design.PropertyEditing;

namespace SilverlightControls.Design.Editos
{
    public class CustomDialogEditor : DialogPropertyValueEditor
    {
        public CustomDialogEditor()
        {
            this.InlineEditorTemplate = EditorDictionary.SimpleTextBox;
            this.DialogEditorTemplate = EditorDictionary.HelloWorldListBox;
        }
    }
}