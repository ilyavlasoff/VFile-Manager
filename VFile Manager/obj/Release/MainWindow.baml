   M S B A M L   `   `   `  ���� H  DVFile Manager, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null-clr-namespace:VFile_ManagerVFile_Manager      VFile_Manager.MainWindow   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     -localclr-namespace:VFile_Manager     -     � Title$  	VExplorer��$	��800��$
��1000�� � Icon$ /Icons/icon.png��.���  � RowDefinitions �� $��40 ��� $��30 ��� .+�� $��0��.+���  �� Orientation$ 
Horizontal=�$
��Left=�.+��� -    ��Name$ 	mkfileBut��$��30��$��30��
������  � ToolTip$ 	Make file��$��0q�$
��Left=�$��Center=�$
��10,0q�.���� $��Icons\add_file.png�� �� BitmapScalingMode$
 Fant=� J� SnapsToDevicePixels . �� -   $ mkdirBut��$��30��$��30��
������ $ Make folder��$��0q�$
��Left=�$��Center=�$
��10,0q�.���� $��Icons\add_folder.png��$
 Fant=� . �� -   $ copyBut��$��30��$��30��
������ $ Copy choosed��$��0q�$
��Left=�$��Center=�$
��10,0q�.���� $��Icons\copy.png��$
 Fant=� . �� -   $ movBut��$��30��$��30��
������ $ Move choosed��$��0q�$
��Left=�$��Center=�$
��10,0q�.���� $��Icons\move.png��$
 Fant=� . �� -   $ renBut��$��30��$��30��
������ $ Rename��$��0q�$
��Left=�$��Center=�$
��10,0q�.���� $��Icons\rename.png��$
 Fant=� . �� -   $ delBut��$��30��$��30��
������ $ Delete��$��0q�$
��Left=�$��Center=�$
��10,0q�.���� $��Icons\delete.png��$
 Fant=� . �� -   $ findBut��$��30��$��30��
������ $��0q�$
 Find��$
��Left=�$��Center=�$
��10,0q�.���� $��/Icons/find.png��$
 Fant=� . �� -	   $ compHash��$��30��$��30��
������ $��0q�$ Compare Hash��$
��Left=�$��Center=�$
��10,0q�.���� $��/Icons/hash.png��$
 Fant=� . �� -
   $ cmdHere��$��30��$��30��
������ $��0q�$ Command line��$
��Left=�$��Center=�$
��10,0q�.���� $��/Icons/cmd.png��$
 Fant=� . �� -   $ setBut��$��30��$��30��
������ $��0q�$ Settings��$��Right=�$��Center=�$
��10,0q�.���� $��/Icons/settings.png��$
 Fant=� . � $��1�� � ColumnDefinitions �� �� .+�� $��0�� �� $��40 ��� $��* ��� $
��0.5* ��� $��60 �.+��� -   $ toParentDirButL��$��0��$��25��$��25��$��Center=�
�������$��0q�.���� $��Icons\up.png��$
 Fant=� . �� -   	 ��Name$	 pathRowL��$��1��$��Center=�$
��Left=�$��10,0,0,0q��� -   $��leftSortChooseBox��$��2��$��Center=�$��10,0,0,0q�
 �� SelectedIndex$
 0���� -    ��Name$ rootComboBoxL��$��3��$��10,0,0,0q�$��Center=�� $��1�� �� $��40 ��� $��* ��� $
��0.5* ��� $��60 �.+��� -   $ toParentDirButR��$��0��$��25��$��25��$��Center=�
�������$��0q�.���� $��Icons\up.png��$
 Fant=� . �� -   $	 pathRowR��$��1��$��Center=�$
��Left=�$��10,0,0,0q��� -   $��rightSortChooseBox��$��2��$��Center=�$
 0��$��10,0,0,0q��� -   $ rootComboBoxR��$��3��$��10,0,0,0q�$��Center=�� $��2�� �� �� .+�� $��0�� �� �� $��20 �.+�� $��0��.+��� -   $��leftScrollViewer��.���� -   $��leftView�� J� 	AllowDrop .  %� IsVirtualizing .  �� SelectionMode$ Multiple=�$��Stretch=�$��Stretch=����� .R�$� ���� .R���  ��Name$ leftViewItem��-   $��30��.���  �� $��30 ��� $��* ��� $
��0.2* ��� $
��0.4* ��� $
��0.4* �.+��� $��0��$��25��$��25��$��Center=�$
��Left=�$
 Fant=� . ���� *Info.IconPath+�� $��1��$��Center=�$
��Left=����� *Info.ShortName+�� $��2��$��Center=�$
��Left=����� *Info.Extension+�� $��3��$��Center=�$
��Left=����� *Info.Extensions+�� $��4��$��Center=�$
��Left=����� *	Info.Time+ � ContextMenu �� .@�m� -   $
��Open��m� $
��Move��m� $
��Copy��m� $��Delete��m� $��Information��� -   $��
StatusStrL��$��1��.+��� $ 
Horizontal=�.+��� $
��10,0q�$	��600������ *
FullName+�� $
��10,0q����� *NumberOfFilesInside+�� $
��10,0q����� *NumberOfDirsInside+�� $
��10,0q����� *
CreateTime+� $��1�� �� �� $��20 �.+�� -   $��
StatusStrR��$��1��.+��� $ 
Horizontal=�.+��� $
��10,0q�$	��600������ *
FullName+�� $
��10,0q����� *NumberOfFilesInside+�� $
��10,0q����� *NumberOfDirsInside+�� $
��10,0q����� *
CreateTime+� $��0��.+��� -   $��rightScrollViewer��.���� -   $��	rightView�� .  . $ Multiple=�$��Stretch=�$��Stretch=����� .R�$� ���� .R��� $ rightViewItem��-   $
��Auto��$��30��.���  �� $��30 ��� $��* ��� $
��0.2* ��� $
��0.4* ��� $
��0.4* �.+��� $��0��$��25��$��25��$
 Fant=� . $��Center=�$
��Left=����� *Info.IconPath+�� $��1��$��Center=�$
��Left=����� *Info.ShortName+�� $��2��$��Center=�$
��Left=����� *Info.Extension+�� $��3��$��Center=�$
��Left=����� *Info.Extensions+�� $��4��$��Center=�$
��Left=����� *	Info.Time+ �� .@�m� $
��Open��m� $
��Move��m� $
��Copy��m� $��Delete��m� $��Information��