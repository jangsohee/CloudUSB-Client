   M S B A M L   `   `   `  ���� T  PMahApps.Metro, Version=1.1.2.0, Culture=neutral, PublicKeyToken=f4fb5a3c4d1e5b4fV;clr-namespace:MahApps.Metro.Controls;assembly=MahApps.MetroMahApps.Metro.Controls   CloudUSB   CloudUSB.JoinView   HControls;clr-namespace:MahApps.Metro.Controls;assembly=MahApps.Metro  \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     -     � Title$
  Join��$	��441��$	��376��$	��441��$	��376��(   "MahApps.Metro.Controls.MetroWindow   ShowTitleBar .   � WindowStartupLocation$ CenterScreen=� � ShowInTaskbar .     ShowMinButton .     ShowMaxRestoreButton .     WindowTransitionsEnabled .  !   ShowSystemMenuOnRightClick .   � 
ResizeMode$ NoResize=�	 � 	Resources	    MyButton
 �� 
TargetType �� Property �� Value �� 
TargetType �� Triggers P� Property P� Value��%�   &	        �� "
 ��.���  � �� �� " ��.R��� $��border��$��0q�
���   �#��z��._��� $��Center=�$��Center=� P�  �@J�IsMouseOver . . ���  �@J�Opacity$	 0.5Q�����  �� ImageSource$ img/joinUs.png��.��� ���� $ img/joinUs.png�� � ColumnDefinitions �� $	��69* ��� $��4* �.+��� -   #��[  $
��Left=�$	��Top=�$��243,349,0,0q�$��82��$��34�� J� RenderTransformOrigin$ 0.553,0.426&�
����������� $ img/joinBtn.png���� -   $��	joinIdBox��*   $MahApps.Metro.Controls.TextBoxHelper   	Watermark$ ID��   ClearTextButton . $��Center=�$��14(�$
��Left=�$	��258��$��37��$��
57,183,0,0q� �� TextWrapping$
 Wrap=�$	��Top=�G� -   $��	joinPwBox��$ Password�� . $��Center=�$��14(�$
��Left=�$��
57,230,0,0q�$	��Top=�$	��258��$��37���� -   $��joinNameBox��$
 Name�� . $��Center=�$��14(�$
��Left=�$��
57,278,0,0q�$
 Wrap=�$	��Top=�$	��258��$��37��