   M S B A M L   `   `   `  ���� T  PMahApps.Metro, Version=1.1.2.0, Culture=neutral, PublicKeyToken=f4fb5a3c4d1e5b4fV;clr-namespace:MahApps.Metro.Controls;assembly=MahApps.MetroMahApps.Metro.Controls   CloudUSB   CloudUSB.LoginView   HControls;clr-namespace:MahApps.Metro.Controls;assembly=MahApps.Metro  \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     -     � Title$  Login��$	��441��$	��376��$	��441��$	��376��(   "MahApps.Metro.Controls.MetroWindow   ShowTitleBar .   � WindowStartupLocation$ CenterScreen=� � ShowInTaskbar .     ShowMinButton .     ShowMaxRestoreButton .  !   ShowSystemMenuOnRightClick .   � 
ResizeMode$ NoResize=� � 	Resources    MyButton	 �� 
TargetType
 �� Property �� Value �� 
TargetType �� Triggers P� Property P� Value��%�   &	        �� "	 ��.��� 
 � �� �� " ��.R��� $��border��$��0q�
���   �#��z��._��� $��Center=�$��Center=� P�  �@J�IsMouseOver . . ��� 
 �@J�Opacity$	 0.5Q�����  �� ImageSource$ img/loginPage.png��.���  � ColumnDefinitions �� $	��69* ��� $��4* �.+��� -   $��id_box��*   $MahApps.Metro.Controls.TextBoxHelper   	Watermark$ ID��   ClearTextButton . $��Center=�$��15(�$
��Left=�$	��258��$��37��$��
57,198,0,0q� �� TextWrapping$
 Wrap=�$	��Top=� J� RenderTransformOrigin$ 0.5,0.5&�G� -   $��pw_box��$ Password�� . $��Center=�$��15(�$
��Left=�$��
57,245,0,0q�$	��Top=�$	��258��$��37���� -   #��[  $
��Left=�$��177,333,0,0q�$	��Top=�$��68��$��34��$ 0.553,0.426&�
����������� $ img/loginButton.png���� -   #��[  $
��Left=�$��245,333,0,0q�$��82��$��34��$	��Top=�$��0q�$ 
0.579,0.44&����� $ img/signUp.png��