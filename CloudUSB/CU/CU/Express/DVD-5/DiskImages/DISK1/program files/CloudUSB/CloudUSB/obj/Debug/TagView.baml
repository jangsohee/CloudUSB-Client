   M S B A M L   `   `   `  ����T  PMahApps.Metro, Version=1.1.2.0, Culture=neutral, PublicKeyToken=f4fb5a3c4d1e5b4fV;clr-namespace:MahApps.Metro.Controls;assembly=MahApps.MetroMahApps.Metro.Controls  5   	    CloudUSB   CloudUSB.TagView   5      HControls;clr-namespace:MahApps.Metro.Controls;assembly=MahApps.Metro  5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   -   5          � Title$	  Tag��5   	    � Icon$ img/tagIcon.png��6   $	��441��5   	   $	��224��6   $	��441��6"   $	��276��62    � WindowStartupLocation$ CenterScreen=�6A    � ShowInTaskbar .  (   "MahApps.Metro.Controls.MetroWindow   ShowMinButton .     ShowMaxRestoreButton .  !   ShowSystemMenuOnRightClick .   � 
ResizeMode$ NoResize=�5	   ,   .��5      � .+�5   
   �� -   5        $��tagName��5   n   *   $MahApps.Metro.Controls.TextBoxHelper   	Watermark$" 태그를 입력해주세요��6   	   ClearTextButton	 . $
��Left=�6~   $��23��6�   $��	10,10,0,0q�6�   
 �� TextWrapping$

 Wrap=�6�   $	��Top=�6�   $	��170��6�   �� 5   
   -   5        $
��Save��5      $
��Left=�6!   $��
185,11,0,0q�6=   $	��Top=�6Q   $��75��6i    J� RenderTransformOrigin$ 
0.853,0.52&�6t   6�   �� 5   
   -   5        $��
tagListBox��5         tagContextMenu � ContextMenu# �   6E   
�������$��10,41,10,10q�6�    � 	Resources 5      ��%v   &	        �� 5      $y�	Collapsed=�65   .@�5      m� -   5        $��태그삭제��5      6-   5      5      5      5      5      