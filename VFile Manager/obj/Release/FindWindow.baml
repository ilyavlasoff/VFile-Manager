   M S B A M L   `   `   `  ����   VFile Manager    VFile_Manager.FindWindow   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    >d2http://schemas.microsoft.com/expression/blend/2008    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006       � Title$
  Find��$	��650��$	��800��.��� .+���  �� Orientation$ Vertical=�.+�� .+��� $ 
Horizontal=�$��Center=�$��0,30,0,0q�.+��� Find in folders�� $ Vertical=�$��20,0,0,0q�.+��� -    ��Name$ dirsToFindBox��$��50��$	��600��. �/� .T�J� .*�.*���  �� BaselineOffset$ 1Q�$
��Wrap=��� $��Search conditions��$��Center=�$
��20,0q�.���  � ColumnDefinitions �� $
��0.3* ��� $��* � � RowDefinitions �� �� �� .+�	� -   $��byNameRadio��$��By name��$��0��$��0��$��Center=� d� 	IsChecked$
 TrueY�	� -   $��bySizeRadio��$��By size��$��0��$��1��$��Center=�	� -   $��byDateRadio��$��By date:��$��0��$��2��$��Center=��� $ Vertical=�$	��600��$��10,0,0,0q�$��1��$��0��.+��� -    ��Name$ condByNameStr���� $
��Wrap=���Enter the file names separated by commas, the unknown number of characters should be marked with the symbol "*", the single character - "?"�� $ 
Horizontal=�$��10,0,0,0q�$��1��$��1��.+��� -   $��modeSelectionSize�� �� SelectedIndex$ 0��.@��� .��more�� .��less�� .��equals�� .��	between�� -   $��sizeSearchValue��$	��100��$
��10,0q��� -   $��sizeSearchSize��$ 0��.@��� .��KB�� .��MB�� .��GB�� $ 
Horizontal=�$��10q�$��1��$��2��.+��� $ 
Horizontal=�$	��600��$��10,0,0,0q�.+��� $��Center=�from:(  "System.Windows.Controls.DatePicker  -	   $��lowDate��$
��10,0q��� $��Center=�to:  -
   $��highDate��$
��10,0q�� .+��� -   $��sortModeBox��$
��Left=�$��Center=�$��50,10q�$ 0��
������ .@��� .��	By name�� .��	By size�� .��Folders first�� .��Files first�� -   $��startSearchBut��$��50��$��Right=�$��50,10q�
������ .��Find�� -   $��	resultBox��$	��300��$	��750��$��20, 10q�$��Center=���5  -System.Windows.Controls.ItemContainerTemplate�  .R�� .+��� ���� *Info.FullName+