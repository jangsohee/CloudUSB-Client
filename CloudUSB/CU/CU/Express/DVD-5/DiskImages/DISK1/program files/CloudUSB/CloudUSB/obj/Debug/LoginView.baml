   M S B A M L   `   `   `  ����T  PMahApps.Metro, Version=1.1.2.0, Culture=neutral, PublicKeyToken=f4fb5a3c4d1e5b4fV;clr-namespace:MahApps.Metro.Controls;assembly=MahApps.MetroMahApps.Metro.Controls  5   	    CloudUSB   CloudUSB.LoginView   5      HControls;clr-namespace:MahApps.Metro.Controls;assembly=MahApps.Metro  5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   -   5          � Title$  Login��5   	   $	��441��5   	   $	��376��6   $	��441��6"   $	��376��62   (   "MahApps.Metro.Controls.MetroWindow   ShowTitleBar .   � WindowStartupLocation$ CenterScreen=�5   	    � ShowInTaskbar .     ShowMinButton .     ShowMaxRestoreButton .  !   ShowSystemMenuOnRightClick .   � 
ResizeMode$ NoResize=�5	   I    � 	Resources 5         MyButton	 �� 
TargetType
 �� Property �� Value �� 
TargetType �� Triggers P� Property P� Value��%o  &	        �� 5   
   "	 ��6!   .�5      �� 
 � �� 5      �� 5      " ��6&   .R�5      �� $��border��6!   $��0q�6/   
���   �#��z��6W   ._�5      �� $��Center=�6/   $��Center=�6L   5       5      P� 5       �@J�IsMouseOver . . �5   "   �� 
 �@J�Opacity$	 0.5Q�6<   5      5      5      5      5!      ��5"      �� 5#   
    �� ImageSource$ img/loginPage.png��6   .��5%      �  � ColumnDefinitions 5&   
   �� 5'      $	��69* �6   �� 5(      $��4* �6   .+�5*   
   �� -   5        $��id_box��5*   �   *   $MahApps.Metro.Controls.TextBoxHelper   	Watermark$ ID��6      ClearTextButton . $��Center=�6g   $��15(�6�   $
��Left=�6�   $	��258��6�   $��37��6�   $��
57,198,0,0q�6�    �� TextWrapping$
 Wrap=�6�   $	��Top=�6    J� RenderTransformOrigin$ 0.5,0.5&�6  G� 5+   
   -   5        $��pw_box��5+   �   $ Password��6    . $��Center=�6p   $��15(�6�   $
��Left=�6�   $��
57,245,0,0q�6�   $	��Top=�6�   $	��258��6�   $��37��6  �� 5,   
   -   5        #��[  5,      $
��Left=�63   $��177,333,0,0q�6N   $	��Top=�6c   $��68��6{   $��34��6�   $ 0.553,0.426&�6�   
���������5-      �� 5.      $ img/loginButton.png��6   50      �� 51   
   -   5        #��[  51      $
��Left=�63   $��245,333,0,0q�6N   $��82��6c   $��34��6n   $	��Top=�6{   $��0q�6�   $ 
0.579,0.44&�6�   ��52      �� 53      $ img/signUp.png��6   55      57      58      