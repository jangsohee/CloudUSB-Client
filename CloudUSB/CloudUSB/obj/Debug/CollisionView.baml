   M S B A M L   `   `   `  ����T  PMahApps.Metro, Version=1.1.2.0, Culture=neutral, PublicKeyToken=f4fb5a3c4d1e5b4fV;clr-namespace:MahApps.Metro.Controls;assembly=MahApps.MetroMahApps.Metro.Controls  5   	    CloudUSB   CloudUSB.CollisionView   5      HControls;clr-namespace:MahApps.Metro.Controls;assembly=MahApps.Metro  5   	   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35U QWPFToolkit, Version=3.5.40128.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   -   5          � Title$  동기화 작업��5   	    � Icon$ img/syncButton.jpg��6   (   "MahApps.Metro.Controls.MetroWindow   ShowTitleBar .     WindowTransitionsEnabled .  $	��440��5   	   $	��500��6   $	��440��6"   $	��650��62    � WindowStartupLocation$ CenterScreen=�5       � ShowInTaskbar .     ShowMinButton .     ShowMaxRestoreButton .  !   ShowSystemMenuOnRightClick .  	 � 
ResizeMode$	 NoResize=�5	   I   
 � 	Resources
 5
         MyButton �� 
TargetType �� Property �� Value �� 
TargetType �� Triggers P� Property P� Value��%o  &	        �� 5   
   " ��6!   .�5      ��  � �� 5      �� 5      " ��6&   .R�5      �� $��border��6!   $��0q�6/   
���   �#��z��6W   ._�5      �� $��Center=�6/   $��Center=�6L   5       5      P� 5       �@J�IsMouseOver . . �5   "   ��  �@J�Opacity$	 0.5Q�6<   5      5      5      5      5       .��5!      � ��5"   
   �� 5#       �� ImageSource$ img/syncBackground3.png��6   .+�5&   
   �� -   5        $��ServerFileListBox��5&       �� SelectionMode$ Multiple=�6,   ���	#F9FFFFFF$
��Left=�6\   $	��318��6w   $��	41,83,0,0q�6�   $	��Top=�6�   $	��283��6�   ��5'      �� 5(      " ��6    �� Triggers 5)      P� 5*       �@��
IsSelected . . �5+      ��  � ��
 �  ��6;   5,      5.      50      �� 52   
   -   5        $��LocalFileListBox��52      $ Multiple=�6*   ���	#F9FFFFFF$
��Left=�6Z   $	��318��6u   $��
330,83,0,0q�6�   $	��Top=�6�   $	��283��6�   ��53      �� 54      " ��6    55      P� 56       �@��
IsSelected . . �57      ��  � ��
 �  ��6;   58      5:      5<      �� 5>   
   -   5        #��[  5>   ,   $
��Left=�6N   $��538,405,0,-29q�6i   $	��Top=�6�   $��86��6�   $��35��6�   ���	#00CCCCCC��5?      �� 5@      $ img/saveButton.png��6   5B      5E      5F      