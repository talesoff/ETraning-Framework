   M S B A M L   `   `   `  ����C  ?GLSample, Version=1.0.0.1, Culture=neutral, PublicKeyToken=null#clr-namespace:GLSampleGLSample  5   	   7 clr-namespace:GLSample.ConverterGLSample.Converter  5   	       GLSample.SecondWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   (localclr-namespace:GLSample     5   	   6	converter clr-namespace:GLSample.Converter     5   	     � Title$  Toolbox��5	   	   $	��480��6   $	��300��6&    � 
ResizeMode$ NoResize=�62    � 	Resources 5       �� 
TargetType �� Property �� Value,   &GLSample.Converter.FPSToColorConverter   FPSToColorConverter,   &GLSample.Converter.FloatToIntConverter  FloatToIntConverter��%�   '��       &	  l     &	      �� 5   
   " ��6   .�5      ��  � ��$	 200��6&   �� 5       � ��$ 4q�6'   5        5   
   6(     5   
   6(   5      .��5      � -   5         �Name$ 
LayoutRoot��5       � RowDefinitions 5   
   �� 5      $��* �6   �� 5      $
��Auto �6   �� 5      $
��Auto �6   .+�5   
   � $��0��6   .+�5      �� .��5      ��  �� Orientation$ Vertical=�6   $��4q�64   .+�5      �� $	��Run��6   .��5      �� $ Vertical=�6%   .+�5      �� -   5        	 ��Name$	 StartButton��5   O   $��Start��6%   6O   �� 5       -   5        $	 PauseButton��5    O   $��Pause��6%   6O   �� 5!      -   5        $	 ResumeButton��5!   Q   $��Resume��6%   6Q   �� 5"      -   5        $	 
StopButton��5"   M   $
��Stop��6%   6M   5#      5$      �� 5%      $��	Serialize��6   .��5&      �� .+�5'      �� -   5        $
��Save��5'   %   64   �� 5(      -   5        $
��Load��5(   %   64   5)      5*      �� 5+      $��UserBehavior��6   .��5,      ,� "
 ,� VerticalScrollBarVisibility$

 Auto=�63   .��5-      /�  /� FontSize$ 10(�6+   .T�5.   "   J� .*�6,   .*�5/   &   �� .�6+   Script/TransformBehavior.cs6H   50   #   51      52      53      54      55      56      � 57   
   $��1��6   $��4q�6   .+�58      �� -   5         ��Name$ TotalElapsedText��58      $��--:--:--��62   $
��Left=�6B   $��Center=�6]   �� 59      -	   5        $ FPSText��59      $��--.--��6)   $��Right=�66   $��Center=�6R   5;      � 5<   
   $��2��6   $��4q�6   .+�5=      �� -
   5         ��Name$ FPSListView��5=      $��70��5>      $��Bottom=�5?      $��Bottom=�62   ��5@      �� *	FPSList+��5A      �� 5B      " ��6   .�5C      ��  � ��$ 0q�6<   �� 5D       � ��$ 0q�63   �� 5E       � ��$ 0q�64   5F      ��5H      �� 5I      .R�5J      �� $ 
Horizontal=�6%   $��Bottom=�6>   5K      ��5M      �� 5N      .R�5O      � $��0q�6   $��3��6q   ��6*   �� *FPS+ �� 	Converter# [ ��6{   �� *FPS+# [   � ToolTip 6�   �� *FPS+.+�5P      � 
������ $��3��6<   $	��Top=�6G   5Q      5R      5T      5U      5V      5W      