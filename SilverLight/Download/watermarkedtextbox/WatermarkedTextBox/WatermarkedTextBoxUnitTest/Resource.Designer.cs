﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:2.0.50727.1433
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace System.Windows.Controls.Extended.Test {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "2.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Resource {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resource() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("System.Windows.Controls.Extended.Test.Resource", typeof(Resource).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to &lt;toolkit:WatermarkedTextBox xmlns=&apos;http://schemas.microsoft.com/client/2007&apos;
        ///      xmlns:toolkit=&quot;clr-namespace:System.Windows.Controls;assembly=System.Windows.Controls.Extended&quot; Text=&apos;{0}&apos; IsEnabled=&apos;{1}&apos; Watermark=&apos;{2}&apos; /&gt;.
        /// </summary>
        internal static string WTB_CustomXaml {
            get {
                return ResourceManager.GetString("WTB_CustomXaml", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to &lt;toolkit:WatermarkedTextBox xmlns=&apos;http://schemas.microsoft.com/client/2007&apos;
        ///      xmlns:toolkit=&quot;clr-namespace:System.Windows.Controls;assembly=System.Windows.Controls.Extended&quot;&gt;
        ///    &lt;/toolkit:WatermarkedTextBox&gt;.
        /// </summary>
        internal static string WTB_DefaultXaml {
            get {
                return ResourceManager.GetString("WTB_DefaultXaml", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to &lt;ControlTemplate  xmlns=&apos;http://schemas.microsoft.com/client/2007&apos;
        ///           xmlns:x=&apos;http://schemas.microsoft.com/winfx/2006/xaml&apos;
        ///           xmlns:controls=&quot;clr-namespace:System.Windows.Controls;assembly=System.Windows.Controls&quot;&gt;
        /// &lt;Grid Background=&quot;Magenta&quot; x:Name=&quot;RootElement&quot;&gt;
        ///    &lt;Grid.Resources&gt;
        ///      &lt;Storyboard  x:Key=&quot;Normal State&quot;&gt;
        ///        &lt;DoubleAnimation Storyboard.TargetName=&quot;ELEMENT_Content&quot; Storyboard.TargetProperty=&quot;Opacity&quot; To=&quot;1&quot; Duration=&quot;0:0:0.0&quot;/&gt;
        ///      &lt;/Storyboard&gt;
        ///    &lt;/Grid [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string WTB_LimitedTemplate {
            get {
                return ResourceManager.GetString("WTB_LimitedTemplate", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to ToolTip text..
        /// </summary>
        internal static string WTB_ToolTipText {
            get {
                return ResourceManager.GetString("WTB_ToolTipText", resourceCulture);
            }
        }
    }
}
