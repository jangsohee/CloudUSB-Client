﻿#pragma checksum "..\..\MainWindow.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "B048C3527B6D426C499161E13865CA2C"
//------------------------------------------------------------------------------
// <auto-generated>
//     이 코드는 도구를 사용하여 생성되었습니다.
//     런타임 버전:4.0.30319.34014
//
//     파일 내용을 변경하면 잘못된 동작이 발생할 수 있으며, 코드를 다시 생성하면
//     이러한 변경 내용이 손실됩니다.
// </auto-generated>
//------------------------------------------------------------------------------

using MahApps.Metro.Controls;
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


namespace CloudUSB {
    
    
    /// <summary>
    /// MainWindow
    /// </summary>
    public partial class MainWindow : MahApps.Metro.Controls.MetroWindow, System.Windows.Markup.IComponentConnector {
        
        
        #line 6 "..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal CloudUSB.MainWindow Root;
        
        #line default
        #line hidden
        
        
        #line 110 "..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.GroupBox GroupBox1;
        
        #line default
        #line hidden
        
        
        #line 115 "..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ListView AllFileList;
        
        #line default
        #line hidden
        
        
        #line 149 "..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.GroupBox GroupBox2;
        
        #line default
        #line hidden
        
        
        #line 165 "..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ListView AllFileList2;
        
        #line default
        #line hidden
        
        
        #line 185 "..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ListBox Category_TaglistBox;
        
        #line default
        #line hidden
        
        
        #line 201 "..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox tagSearchBox;
        
        #line default
        #line hidden
        
        
        #line 208 "..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Primitives.ToggleButton toggleButton;
        
        #line default
        #line hidden
        
        
        #line 237 "..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button LoginBtn;
        
        #line default
        #line hidden
        
        
        #line 244 "..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button backButton;
        
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
            System.Uri resourceLocater = new System.Uri("/CloudUSB;component/mainwindow.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\MainWindow.xaml"
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
            this.Root = ((CloudUSB.MainWindow)(target));
            
            #line 10 "..\..\MainWindow.xaml"
            this.Root.KeyDown += new System.Windows.Input.KeyEventHandler(this.Window_KeyDown);
            
            #line default
            #line hidden
            
            #line 11 "..\..\MainWindow.xaml"
            this.Root.Loaded += new System.Windows.RoutedEventHandler(this.Window_Loaded);
            
            #line default
            #line hidden
            
            #line 12 "..\..\MainWindow.xaml"
            this.Root.ContentRendered += new System.EventHandler(this.Window_Rendered);
            
            #line default
            #line hidden
            
            #line 13 "..\..\MainWindow.xaml"
            this.Root.Closing += new System.ComponentModel.CancelEventHandler(this.Root_Closing);
            
            #line default
            #line hidden
            return;
            case 2:
            this.GroupBox1 = ((System.Windows.Controls.GroupBox)(target));
            return;
            case 3:
            this.AllFileList = ((System.Windows.Controls.ListView)(target));
            
            #line 115 "..\..\MainWindow.xaml"
            this.AllFileList.Drop += new System.Windows.DragEventHandler(this.fileDropEvent);
            
            #line default
            #line hidden
            
            #line 115 "..\..\MainWindow.xaml"
            this.AllFileList.MouseDoubleClick += new System.Windows.Input.MouseButtonEventHandler(this.AllFileList_MouseDoubleClick);
            
            #line default
            #line hidden
            
            #line 117 "..\..\MainWindow.xaml"
            this.AllFileList.PreviewMouseLeftButtonDown += new System.Windows.Input.MouseButtonEventHandler(this.AllFileList_PreviewMouseLeftButtonDown);
            
            #line default
            #line hidden
            
            #line 117 "..\..\MainWindow.xaml"
            this.AllFileList.MouseMove += new System.Windows.Input.MouseEventHandler(this.AllFileList_MouseMove);
            
            #line default
            #line hidden
            
            #line 120 "..\..\MainWindow.xaml"
            this.AllFileList.ContextMenuOpening += new System.Windows.Controls.ContextMenuEventHandler(this.ItemRightClick);
            
            #line default
            #line hidden
            
            #line 120 "..\..\MainWindow.xaml"
            this.AllFileList.MouseDown += new System.Windows.Input.MouseButtonEventHandler(this.listViewBackgroundClick);
            
            #line default
            #line hidden
            return;
            case 4:
            
            #line 135 "..\..\MainWindow.xaml"
            ((System.Windows.Controls.MenuItem)(target)).Click += new System.Windows.RoutedEventHandler(this.ContextMenu_Cut);
            
            #line default
            #line hidden
            return;
            case 5:
            
            #line 136 "..\..\MainWindow.xaml"
            ((System.Windows.Controls.MenuItem)(target)).Click += new System.Windows.RoutedEventHandler(this.ContextMenu_Copy);
            
            #line default
            #line hidden
            return;
            case 6:
            
            #line 137 "..\..\MainWindow.xaml"
            ((System.Windows.Controls.MenuItem)(target)).Click += new System.Windows.RoutedEventHandler(this.ContextMenu_Paste);
            
            #line default
            #line hidden
            return;
            case 7:
            
            #line 138 "..\..\MainWindow.xaml"
            ((System.Windows.Controls.MenuItem)(target)).Click += new System.Windows.RoutedEventHandler(this.ContextMenu_Delete);
            
            #line default
            #line hidden
            return;
            case 8:
            
            #line 139 "..\..\MainWindow.xaml"
            ((System.Windows.Controls.MenuItem)(target)).Click += new System.Windows.RoutedEventHandler(this.ContextMenu_Rename);
            
            #line default
            #line hidden
            return;
            case 9:
            
            #line 140 "..\..\MainWindow.xaml"
            ((System.Windows.Controls.MenuItem)(target)).MouseEnter += new System.Windows.Input.MouseEventHandler(this.ContextMenu_TagList);
            
            #line default
            #line hidden
            
            #line 140 "..\..\MainWindow.xaml"
            ((System.Windows.Controls.MenuItem)(target)).Click += new System.Windows.RoutedEventHandler(this.ContextMenu_TagList);
            
            #line default
            #line hidden
            return;
            case 10:
            
            #line 141 "..\..\MainWindow.xaml"
            ((System.Windows.Controls.MenuItem)(target)).MouseEnter += new System.Windows.Input.MouseEventHandler(this.ContextMenu_TagCut);
            
            #line default
            #line hidden
            return;
            case 11:
            this.GroupBox2 = ((System.Windows.Controls.GroupBox)(target));
            return;
            case 12:
            this.AllFileList2 = ((System.Windows.Controls.ListView)(target));
            
            #line 166 "..\..\MainWindow.xaml"
            this.AllFileList2.MouseDoubleClick += new System.Windows.Input.MouseButtonEventHandler(this.AllFileList2_MouseDoubleClick);
            
            #line default
            #line hidden
            
            #line 171 "..\..\MainWindow.xaml"
            this.AllFileList2.MouseDown += new System.Windows.Input.MouseButtonEventHandler(this.listViewBackgroundClick2);
            
            #line default
            #line hidden
            return;
            case 13:
            this.Category_TaglistBox = ((System.Windows.Controls.ListBox)(target));
            
            #line 187 "..\..\MainWindow.xaml"
            this.Category_TaglistBox.SelectionChanged += new System.Windows.Controls.SelectionChangedEventHandler(this.Category_TaglistBox_SelectionChanged);
            
            #line default
            #line hidden
            
            #line 188 "..\..\MainWindow.xaml"
            this.Category_TaglistBox.Loaded += new System.Windows.RoutedEventHandler(this.Category_TaglistBox_Loaded);
            
            #line default
            #line hidden
            return;
            case 14:
            this.tagSearchBox = ((System.Windows.Controls.TextBox)(target));
            return;
            case 15:
            
            #line 202 "..\..\MainWindow.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.SearchBtn_Click);
            
            #line default
            #line hidden
            return;
            case 16:
            this.toggleButton = ((System.Windows.Controls.Primitives.ToggleButton)(target));
            
            #line 209 "..\..\MainWindow.xaml"
            this.toggleButton.Click += new System.Windows.RoutedEventHandler(this.toggleButton_Click);
            
            #line default
            #line hidden
            return;
            case 17:
            
            #line 230 "..\..\MainWindow.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.TagBtn_Click);
            
            #line default
            #line hidden
            return;
            case 18:
            this.LoginBtn = ((System.Windows.Controls.Button)(target));
            
            #line 238 "..\..\MainWindow.xaml"
            this.LoginBtn.Click += new System.Windows.RoutedEventHandler(this.Login_Click);
            
            #line default
            #line hidden
            return;
            case 19:
            this.backButton = ((System.Windows.Controls.Button)(target));
            
            #line 245 "..\..\MainWindow.xaml"
            this.backButton.Click += new System.Windows.RoutedEventHandler(this.Back_Button_Click);
            
            #line default
            #line hidden
            return;
            case 20:
            
            #line 253 "..\..\MainWindow.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.syncBtn_click);
            
            #line default
            #line hidden
            return;
            case 21:
            
            #line 260 "..\..\MainWindow.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.settingBtn_Click);
            
            #line default
            #line hidden
            return;
            case 22:
            
            #line 265 "..\..\MainWindow.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.History_Click);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}
