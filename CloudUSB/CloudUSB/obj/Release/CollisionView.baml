   M S B A M L   `   `   `  ���� T  PMahApps.Metro, Version=1.1.2.0, Culture=neutral, PublicKeyToken=f4fb5a3c4d1e5b4fV;clr-namespace:MahApps.Metro.Controls;assembly=MahApps.MetroMahApps.Metro.Controls   CloudUSB   CloudUSB.CollisionView   HControls;clr-namespace:MahApps.Metro.Controls;assembly=MahApps.Metro  \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     -     � Title$  동기화 작업�� � Icon$ img/syncButton.jpg��(   "MahApps.Metro.Controls.MetroWindow   ShowTitleBar .     WindowTransitionsEnabled .  $	��440��$	��500��$	��440��$	��650�� � WindowStartupLocation$ CenterScreen=� � ShowInTaskbar .     ShowMinButton .     ShowMaxRestoreButton .  !   ShowSystemMenuOnRightClick .  	 � 
ResizeMode$	 NoResize=�
 � 	Resources
    MyButton �� 
TargetType �� Property �� Value �� 
TargetType �� Triggers P� Property P� Value��%�   &	        �� " ��.���  � �� �� " ��.R��� $��border��$��0q�
���   �#��z��._��� $��Center=�$��Center=� P�  �@J�IsMouseOver . . ���  �@J�Opacity$	 0.5Q�.��� ����  �� ImageSource$ img/syncBackground3.png��.+��� -   $��ServerFileListBox�� �� SelectionMode$ Multiple=����	#F9FFFFFF$
��Left=�$	��318��$��	41,83,0,0q�$	��Top=�$	��283������ " �� �� Triggers P�  �@��
IsSelected . . ���  � ��
 �  ���� -   $��LocalFileListBox��$ Multiple=����	#F9FFFFFF$
��Left=�$	��318��$��
330,83,0,0q�$	��Top=�$	��283������ " �� P�  �@��
IsSelected . . ���  � ��
 �  ���� -   #��[  $
��Left=�$��538,405,0,-29q�$	��Top=�$��86��$��35�����	#00CCCCCC���� $ img/saveButton.png��