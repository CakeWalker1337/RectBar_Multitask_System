﻿#pragma checksum "..\..\LogWindow.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "B3BF3128DAB13F721E665D75364A38E8FD030253"
//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан программой.
//     Исполняемая версия:4.0.30319.42000
//
//     Изменения в этом файле могут привести к неправильной работе и будут потеряны в случае
//     повторной генерации кода.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Shell;


namespace RectBar_Multitask_System {
    
    
    /// <summary>
    /// LogWindow
    /// </summary>
    public partial class LogWindow : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 26 "..\..\LogWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock StatusBlock;
        
        #line default
        #line hidden
        
        
        #line 39 "..\..\LogWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock loginLabel;
        
        #line default
        #line hidden
        
        
        #line 61 "..\..\LogWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox LogBox;
        
        #line default
        #line hidden
        
        
        #line 63 "..\..\LogWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock passLabel;
        
        #line default
        #line hidden
        
        
        #line 85 "..\..\LogWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox PassBox;
        
        #line default
        #line hidden
        
        
        #line 90 "..\..\LogWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.StackPanel spRememberMe;
        
        #line default
        #line hidden
        
        
        #line 92 "..\..\LogWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.CheckBox rememberMeBox;
        
        #line default
        #line hidden
        
        
        #line 110 "..\..\LogWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button LogButton;
        
        #line default
        #line hidden
        
        
        #line 120 "..\..\LogWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button OptionsButton;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/RectBar Multitask System;component/logwindow.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\LogWindow.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            this.StatusBlock = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 2:
            this.loginLabel = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 3:
            this.LogBox = ((System.Windows.Controls.TextBox)(target));
            return;
            case 4:
            this.passLabel = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 5:
            this.PassBox = ((System.Windows.Controls.TextBox)(target));
            return;
            case 6:
            this.spRememberMe = ((System.Windows.Controls.StackPanel)(target));
            return;
            case 7:
            this.rememberMeBox = ((System.Windows.Controls.CheckBox)(target));
            return;
            case 8:
            this.LogButton = ((System.Windows.Controls.Button)(target));
            
            #line 111 "..\..\LogWindow.xaml"
            this.LogButton.Click += new System.Windows.RoutedEventHandler(this.LogButton_Click);
            
            #line default
            #line hidden
            return;
            case 9:
            this.OptionsButton = ((System.Windows.Controls.Button)(target));
            
            #line 121 "..\..\LogWindow.xaml"
            this.OptionsButton.Click += new System.Windows.RoutedEventHandler(this.OptionsButton_Click);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

