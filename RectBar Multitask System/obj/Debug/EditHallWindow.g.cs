﻿#pragma checksum "..\..\EditHallWindow.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "16A57BA4FA24C9C00D6FF5550448DBD5859E0485"
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
    /// EditHallWindow
    /// </summary>
    public partial class EditHallWindow : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 18 "..\..\EditHallWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.StackPanel choicePanel;
        
        #line default
        #line hidden
        
        
        #line 28 "..\..\EditHallWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox nameComboBox;
        
        #line default
        #line hidden
        
        
        #line 40 "..\..\EditHallWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox nameBox;
        
        #line default
        #line hidden
        
        
        #line 50 "..\..\EditHallWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button saveButton;
        
        #line default
        #line hidden
        
        
        #line 57 "..\..\EditHallWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button deleteButton;
        
        #line default
        #line hidden
        
        
        #line 65 "..\..\EditHallWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button exitButton;
        
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
            System.Uri resourceLocater = new System.Uri("/RectBar Multitask System;component/edithallwindow.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\EditHallWindow.xaml"
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
            this.choicePanel = ((System.Windows.Controls.StackPanel)(target));
            return;
            case 2:
            this.nameComboBox = ((System.Windows.Controls.ComboBox)(target));
            
            #line 29 "..\..\EditHallWindow.xaml"
            this.nameComboBox.SelectionChanged += new System.Windows.Controls.SelectionChangedEventHandler(this.nameComboBox_SelectionChanged);
            
            #line default
            #line hidden
            return;
            case 3:
            this.nameBox = ((System.Windows.Controls.TextBox)(target));
            return;
            case 4:
            this.saveButton = ((System.Windows.Controls.Button)(target));
            
            #line 51 "..\..\EditHallWindow.xaml"
            this.saveButton.Click += new System.Windows.RoutedEventHandler(this.saveButton_Click);
            
            #line default
            #line hidden
            return;
            case 5:
            this.deleteButton = ((System.Windows.Controls.Button)(target));
            
            #line 59 "..\..\EditHallWindow.xaml"
            this.deleteButton.Click += new System.Windows.RoutedEventHandler(this.deleteButton_Click);
            
            #line default
            #line hidden
            return;
            case 6:
            this.exitButton = ((System.Windows.Controls.Button)(target));
            
            #line 66 "..\..\EditHallWindow.xaml"
            this.exitButton.Click += new System.Windows.RoutedEventHandler(this.exitButton_Click);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

