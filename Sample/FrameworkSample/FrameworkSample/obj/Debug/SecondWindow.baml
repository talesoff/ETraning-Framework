   M S B A M L   `   `   `  ����J  FFrameworkSample, Version=1.0.0.1, Culture=neutral, PublicKeyToken=null1clr-namespace:FrameworkSampleFrameworkSample  5   	   E'clr-namespace:FrameworkSample.ConverterFrameworkSample.Converter  5   	   "    FrameworkSample.SecondWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   /localclr-namespace:FrameworkSample     5   	   =	converter'clr-namespace:FrameworkSample.Converter     5   	     � Title$  Toolbox��5	   	   $	��540��6   $	��320��6&    � 
ResizeMode$ NoResize=�62    � 	Resources 5       �� 
TargetType �� Property �� Value3   -FrameworkSample.Converter.FPSToColorConverter   FPSToColorConverter3   -FrameworkSample.Converter.FloatToIntConverter  FloatToIntConverter��%�   '��       &	  G     &	 Z     �� 5   
   " ��6   .�5      ��  � ��$ 4q�6'   5        5   
   6(     5   
   6(   5      .��5      � -   5         �Name$ 
LayoutRoot��5       � RowDefinitions 5   
   �� 5      $��* �6   �� 5      $
��Auto �6   �� 5      $
��Auto �6   .+�5   
   � $��0��6   .+�5      �� .��5      ��  �� Orientation$ Vertical=�6   $��4q�64   .+�5      �� $	��Run��6   .��5      �� $ Vertical=�6%   .+�5      �� -   5        	 ��Name$	 StartButton��5   O   $��Start��6%   6O   �� 5       -   5        $	 PauseButton��5    O   $��Pause��6%   6O   �� 5!      -   5        $	 ResumeButton��5!   Q   $��Resume��6%   6Q   �� 5"      -   5        $	 
StopButton��5"   M   $
��Stop��6%   6M   �� 5#      $��1��6%   
�������$��1q�6H   �� 5$      -   5        $	 ResetButton��5$   O   $��Reset��6%   6O   5%      5&      �� 5'      -   5        
 ��Name$
 HierarchyGroup��5'      $��	Hierarchy��67   .��5(      �� .+�5)      �� -   5        $��Add Triangle��5)   %   6<   �� 5*      -	   5        $��Remove Triangle��5*   %   6?   5+      5,      �� 5-      $��	Serialize��6   .��5.      �� .+�5/      �� -
   5        $
��Save��5/   %   64   �� 50      -   5        $
��Load��50   %   64   51      52      �� 53      $��UserBehavior��6   .��54      ,� " ,� VerticalScrollBarVisibility$
 Auto=�63   .��55      /�  /� FontSize$ 10(�6+   .T�56   "   J� .*�6,   .*�57   &   �� .�6+   Script/TransformBehavior.cs6H   58   #   59      5:      5;      5<      5=      5>      � 5@   
   $��1��6   $��4q�6   .+�5A      �� $	��Log��6   .��5B      �� -   5         ��Name$ LogListView��5B      $	��120��60   ��6=   �� *	LogList+��5C      �� 5D      " ��6    .�5E      ��  � ��$ Stretch=�6K   5F      ��5H      �� 5I      .R�5J      �  � ToolTip 6#   �� *Time+.+�5K   "   �� $ Vertical=�6-   .+�5L   &   �� $
��Wrap=�6E   ��60   �� *Log+6E   � 5M   &   $��2q�6+   �� 5N   &   $��1��6-   
�������$��1q�6P   5O   #   5P      5Q      5S      5T      5U      � 5V   
   $��2��6   $��4q�6   .+�5W      �� $	��FPS��6   .��5X      �  5Y      �� 5Z      $
��Auto �6(   �� 5[      $��* �6(   .+�5]      � $��0��6   .+�5^      �� -   5         ��Name$ TotalElapsedText��5^   $   $��--:--:--��6>   $
��Left=�6N   $��Center=�6i   �� 5_      -   5        $ FPSText��5_   $   $��--.--��65   $��Right=�6B   $��Center=�6^   5`      � 5a      $��1��6   .+�5b      �� -   5        $ FPSListView��5b   #   $��70��5c   #   $��Bottom=�5d   #   $��Bottom=�6>   ��5e   #   �� *	FPSList+��5f      �� 5g   "   " ��6(   .�5h   &   ��  � ��$ 0q�6H   �� 5i   &    � ��$ 0q�6?   �� 5j   &    � ��$ 0q�6@   5k   #   ��5m      �� 5n   "   .R�5o   &   �� $ 
Horizontal=�61   $��Bottom=�6J   5p   #   ��5r      �� 5s   "   .R�5t   &   � $��0q�6+   $��3��6}   ��66   �� *FPS+ �� 	Converter# [ ��6�   �� *FPS+# [   6�   �� *FPS+.+�5u   *   � 
������ $��3��6H   $	��Top=�6S   5v   '   5w   #   5y      5z      5{      5|      5~      5      5�      