   M S B A M L   `   `   `  ����T  PMahApps.Metro, Version=1.1.2.0, Culture=neutral, PublicKeyToken=f4fb5a3c4d1e5b4fV;clr-namespace:MahApps.Metro.Controls;assembly=MahApps.MetroMahApps.Metro.Controls  5   	    CloudUSB   CloudUSB.JoinView   5      HControls;clr-namespace:MahApps.Metro.Controls;assembly=MahApps.Metro  5   	   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35U QWPFToolkit, Version=3.5.40128.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   -   5          � Title$
  Join��5   	   $	��441��5      $	��376��6   $	��441��6!   $	��376��61   (   "MahApps.Metro.Controls.MetroWindow   ShowTitleBar .   � WindowStartupLocation$ CenterScreen=�5       � ShowInTaskbar .     ShowMinButton .     ShowMaxRestoreButton .     WindowTransitionsEnabled .  !   ShowSystemMenuOnRightClick .   � 
ResizeMode$ NoResize=�5	   5   	 � 	Resources	 5   
      MyButton
 �� 
TargetType �� Property �� Value �� 
TargetType �� Triggers P� Property P� Value��%o  &	        �� 5   
   "
 ��6!   .�5      ��  � �� 5      �� 5      " ��6&   .R�5      �� $��border��6!   $��0q�6/   
���   �#��z��6W   ._�5      �� $��Center=�6/   $��Center=�6L   5       5      P� 5       �@J�IsMouseOver . . �5   "   ��  �@J�Opacity$	 0.5Q�6<   5      5      5      5      5       ��5!      �� 5"   
    �� ImageSource$ img/joinUs.png��6   .��5$      � ��5%   
   �� 5&      $ img/joinUs.png��6    � ColumnDefinitions 5)   
   �� 5*      $	��69* �6   �� 5+      $��4* �6   .+�5-   
   �� -   5        #��[  5-      $
��Left=�6I   $	��Top=�6d   $��243,349,0,0q�6|   $��82��6�   $��34��6�    J� RenderTransformOrigin$ 0.553,0.426&�6�   
���������5.      �� 5/      $ img/joinBtn.png��6   51      �� 52   
   -   5        $��	joinIdBox��52   f   *   $MahApps.Metro.Controls.TextBoxHelper   	Watermark$ ID��6      ClearTextButton . $��Center=�6w   $��14(�6�   $
��Left=�6�   $	��258��6�   $��37��6�   $��
57,183,0,0q�6�    �� TextWrapping$
 Wrap=�6�   $	��Top=�6  G� 53   
   -   5        $��	joinPwBox��53   p   $ Password��6    . $��Center=�6�   $��14(�6�   $
��Left=�6�   $��
57,230,0,0q�6�   $	��Top=�6�   $	��258��6�   $��37��6  �� 54   
   -   5        $��joinNameBox��54   h   $
 Name��6    . $��Center=�6{   $��14(�6�   $
��Left=�6�   $��
57,278,0,0q�6�   $
 Wrap=�6�   $	��Top=�6�   $	��258��6  $��37��6  55      56      