   M S B A M L   `   `   `  ����  	Exercise2    Exercise2.GridWindow   5   	   W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	     � Title$  
GridWindow��5   	   $	��450��6   $	��800��6)   .��5	      �  � RowDefinitions 5   
   �� 5      $
��Auto �6   �� 5      $��* �6   .+�5   
   ��  �� Orientation$ 
Horizontal=�6   $	��Top=�6.   $
��Auto��6F   .+�5      �� $��5q�6   $
��Left=�6#   $��40��6>   Row6N   �� 5      -   5         ��Name$ 
rowTextBox��5      $	��0 5q�6*   $��60��67   6D   �� 5      $
��25 5q�6   $
��Left=�6&   $��45��6A   Column6T   �� 5      -   5        $ columnTextBox��5      $	��0 5q�6-   $��60��6:   6G   �� 5      -   5         ��Name$ applyButton��5      $��Center=�6*   $
��25 0q�6G   $��20,0,0,0q�6V   $��10(�6h   $��20��6v   .��6�   Apply6�   5      � 5   
   -   5         �Name$ mainGrid��5      $��1��6!    � ShowGridLines$
 True��6.   $��0 0 0 0q�6C   $��Stretch=�6T   $	��780��6r   $	��390��6~   .+�5      �� -   5        $ greenButton��5      $��Row 0, Column 0��6*   
������6D    5      �� 5      �� 5      �� 5      �� 5       � ColumnDefinitions 5!      �� 5"      �� 5#      �� 5$      �� 5%      5'      5)      5*      