   M S B A M L   `   `   `  ����G  CJSONExplorer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullG(clr-namespace:JSONExplorer.Views.WindowsJSONExplorer.Views.Windows  5   	   .    (JSONExplorer.Views.Windows.SettingWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   :local(clr-namespace:JSONExplorer.Views.Windows     5   	     � Title$  Setter��5	   	   $	��500��5
   	   $	��250��6   $	��400��5   	   $	��300��6    � 
ResizeMode$ CanResizeWithGrip=�5   	    � WindowStyle$
 None=�5   	    � WindowState$ Normal=�5   	    � AllowsTransparency .  � DataContext 5   	   �� *	Setting+   	ViewModel �� Source# [  ��5   	   Z�  � 	Resources 5       �� 
TargetType	 �� Property
 �� Value��%R  '��       '�� p     �� 5   
   " ��6   .�5      �� 	 � ��$
 18(�6)   �� 5      	 � ��$
 Center=�64   5      �� 5   
   " ��6   .�5      �� 	 � ��$
 20(�6)   �� 5      	 � ��
 6+   Z� �� 5      	 � ��
 �	#CC2E38516+   �� 5      	 �@��TextAlignment$
 Center=�6.   5      5      .��5      � -   5         �Name$
 Main��5      ���	#CC2E3851 � RowDefinitions 5    
   �� 5!      $��40 �6   �� 5"      �� 5#      $��25 �6   �� 5$      $��20 �6   .+�5&   
   � $��1��6   $��1��6   $��Center=�6,   $
��5, 0q�6G    � ColumnDefinitions 5'      �� 5(      $��* �6#   �� 5)      $��1* �6#   .+�5,      �� $��0��6   
�������.+�5-      �� .��6   View Font :6%   5.      �� 5/      $��1��6   
�������.+�50      �� ��6   �� *TextBoxFontSize+51      52      � 53   
   $��2��6   $
��5, 0q�6    54      �� 55      �� 56      $	��100 �6#   .+�58      �� -   5         ��Name$ BtnReSet��58      $��1��6'   .��67   重置6;   59      5:      5;      