   M S B A M L   `   `   `  ����T  PMahApps.Metro, Version=1.1.2.0, Culture=neutral, PublicKeyToken=f4fb5a3c4d1e5b4fV;clr-namespace:MahApps.Metro.Controls;assembly=MahApps.MetroMahApps.Metro.Controls  5   	    CloudUSB   CloudUSB.CollisionView   5      HControls;clr-namespace:MahApps.Metro.Controls;assembly=MahApps.Metro  5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	     � Title$  동기화 작업��5   	    � Icon$ img/syncButton.jpg��6   $	��440��5   	   $	��500��6   $	��440��6"   $	��650��62    � WindowStartupLocation$ CenterScreen=�5   	    � ShowInTaskbar .  (   "MahApps.Metro.Controls.MetroWindow   ShowMinButton .     ShowMaxRestoreButton .  !   ShowSystemMenuOnRightClick .   � 
ResizeMode$ NoResize=�5   I    � 	Resources 5	         MyButton	 �� 
TargetType
 �� Property �� Value �� 
TargetType �� Triggers P� Property P� Value��%o  &	        �� 5   
   "	 ��6!   .�5      �� 
 � �� 5      �� 5      " ��6&   .R�5      �� $��border��6!   $��0q�6/   
���   �#��z��6W   ._�5      �� $��Center=�6/   $��Center=�6L   5       5      P� 5       �@J�IsMouseOver . . �5   "   �� 
 �@J�Opacity$	 0.5Q�6<   5      5      5      5      5      .��5       � ��5!   
   �� 5"       �� ImageSource$ img/syncBackground3.png��6   .+�5$   
   �� 
������ $
��Left=�6+   $	��318��6F   $��	29,75,0,0q�6S   $	��Top=�6f   $	��242��6~   �� 5%   
   
������ $
��Left=�6+   $	��315��6F   $��
376,75,0,0q�6S   $	��Top=�6g   $	��242��6   �� 5&   
   #��[  6   $
��Left=�64   $��
551,11,0,0q�6O   $	��Top=�6c   $��29��6{   $��29��6�   $��0q�6�   ��5'      �� 5(      $ img/okbtn.png��6   5*      �� 5+   
   #��[  6   $
��Left=�63   $��
593,11,0,0q�6N   $	��Top=�6b   $��28��6z   $��28��6�   $��0q�6�   ��5,      �� 5-      $ img/cancelbtn.png��6   5/      �� 50   
   #��[  6   $
��Left=�63   $��301,168,0,0q�6N   $	��Top=�6c   $��44��6{   $��41��6�    J� RenderTransformOrigin$ 
0.08,0.049&�6�   ���	#00CCCCCC��51      �� 52      $ img/arrow-left.png��6   54      �� 55   
   #��[  6   $
��Left=�63   $��301,214,0,0q�6N   $	��Top=�6c   $��44��6{   $��44��6�   ���	#00CCCCCC��56      �� 57      $ img/arrow-right.png��6   59      5<      5=      