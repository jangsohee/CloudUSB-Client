   M S B A M L   `   `   `  ���� T  PMahApps.Metro, Version=1.1.2.0, Culture=neutral, PublicKeyToken=f4fb5a3c4d1e5b4fV;clr-namespace:MahApps.Metro.Controls;assembly=MahApps.MetroMahApps.Metro.Controls   CloudUSB   CloudUSB.MainWindow   HControls;clr-namespace:MahApps.Metro.Controls;assembly=MahApps.Metro  \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     -   $
��Root��  � WindowStartupLocation$  CenterScreen=� � Title$ CloudUSB�� � Icon$ .\img\cuIcon.png��$	��520��$	��680��$	��450��$	��600�� � 	Resources    ItemsPanelTemplate1 K� IsItemsHost  MyButton �� 
TargetType �� Property �� Value �� 
TargetType	 �� Triggers
 P� Property P� Value  MyToggleButton��%�  &	        &	      &	 �     �� .R��  . �� " ��.���  � �� �� " ��.R��� $��border��$��0q�
���   �#��z��._��� $��Center=�$��Center=�	 P� 
 �@J�IsMouseOver . . ���  �@J�Opacity$	 0.5Q��� " d�.���  � �� �� " d�.R��� $��border��$��0q�
���   �#��z��._��� $��Center=�$��Center=�	 P� 
 �@J�IsMouseOver . . ���  �@J�Opacity$	 0.5Q���  � ��
 �   �.���  � RowDefinitions �� $��35 ��� $��35 ��� $��35 ��� $��* � � ColumnDefinitions �� $��30 ��� $��90 ��� $��90 ��� $��90 ��� $��90 ��� $��* ��� $��90 ��� $��33 ��� $��33 ��� $��33 ��� $��30 �   showOnToggled �� BasedOn �� Triggers �� Binding �� ElementName �� Path �� Value  hideOnToggled��%2  &	       7��&	 �     7���� " ��8   .���  � y�$ Visible=� ��  �� $ toggleButton��$ 	IsChecked� True.a���  � y�$ 	Collapsed=��� " ��8   .���  � y�$ 	Collapsed=� ��  �� $ toggleButton��$ 	IsChecked� True.a���  � y�$ Visible=�.+��� -    ��Name$ 	GroupBox1��$�� ��$��1��$��3��#��[ $��30,10,30,24q�$��11��.���� -    ��Name$ AllFileList�� J� 	AllowDrop .  �� SelectionMode$ Single=�$��Center=�#���   $��2q�$��Stretch=�$��Disabled=���. |�.  ���	#00000000$��0q�  ItemContextMenu � ContextMenu# �  $��9������ .R�� $��3q� �� $��32 ��� $��* �$��80��.+���  ��Name$	 img��$��0��$��32��$��32������ *
FileIcon+��  �� TextAlignment$ Center=�$��1��$��Center=�$��75��$
��Wrap=����� *
FileName+ ��%  &	       �� $y�Visible=�.@�m� -   $��잘라내기��{�. m� -   $��복사하기��{�. m� -   $��붙여넣기��{�.  m� -   $��삭제하기��{�. m� -   $��이름수정��{�. m� -	   $��태그설정��{�. m� -
   $��태그삭제��{�. �� -   $ 	GroupBox2��$��Category��$��1��$��3��#��[ $��30,10,30,24q�$��11��.���  �� $��35 ��� $��* � �� $��* ��� $	��130 ��� $��55 �.+��� -   $ AllFileList2��
����������	#00000000$��Stretch=�$ Single=�$��Stretch=�#���   $��2q�$��Disabled=���. |�.  $��0q�$��1��$��2������ .R�� $��3q� �� $��32 ��� $��* �$��80��.+��� $	 img��$��0��$��32��$��32������ *
FileIcon+�� $ Center=�$��1��$��Center=�$��75��$
��Wrap=����� *
NickName+�� -   $��Category_TaglistBox��$��Stretch=�$��2q�$��1��$��1��$��2��$��Stretch=�
����� � �� SelectedIndex$ 0������ " �� P� 
 �@��
IsSelected . . ���  � ��
 ��� ��� -    ��Name$ tagSearchBox��$��0��$��1��*   $MahApps.Metro.Controls.TextBoxHelper   	Watermark$ 태그 검색��   ClearTextButton . $��2q�$��Stretch=� �� TextWrapping$
 Wrap=�$��Stretch=�$��Center=��� -   $��0��$��2��$��Search��$��Stretch=�$��Stretch=�$��2q�d� -     d�Name$  toggleButton��$��2��#��[ $��Center=�$��Bottom=�
�������$��24��$��74��|�.  ���� ! �� ImageSource$! img/off.png�� � $��1��$��Center=�$��24��$��74��$��Bottom=����� $! img/default_off.png�� � $��3��$��Center=�$��24��$��74��$��Bottom=����� $! img/category_off.png���� -   $��4��$��Center=�#��[ $��Bottom=�$��24��$��74��
����������� $! img/tag_off.png���� -   " ��Name$" LoginBtn��$��6��#��[ $��Center=�$��Bottom=�$��24��$��74������ $! img/login.png���� -   $" 
backButton��$��9��#��[ $
��Left=�$��1��$��Bottom=�$��25��$��25��$��4,0,4,33q�$��2������ $! img/backButton.png���� -   $��8��$��Center=�#��[ $��Bottom=�$��24��$��24��$��Normal"�
�������$��5,0,4,0q����� $! img/syncButton.jpg���� -   $��9��$��Center=�#��[ $��Bottom=�$��25��$��24��$��Normal"�
�������$��6,0,3,0q����� $! img/system.png���� -   $��7��$��24��$��22��#��[ $��Center=�$��Bottom=�$��5,0,4,1q����� $! 	img/H.png��