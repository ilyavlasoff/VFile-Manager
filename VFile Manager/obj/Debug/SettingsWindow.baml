   M S B A M L   `   `   `  ����H  DVFile Manager, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null-clr-namespace:VFile_ManagerVFile_Manager  5   	   "    VFile_Manager.SettingsWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   -localclr-namespace:VFile_Manager     5   	     � Title$  Settings��5   	   $	��450��6   $	��610��6'   .��5	      � .+�5
   
   ��  �� Orientation$ Vertical=�6   $
��20,5q�6,   .+�5      �� $��3q�6   Load start directories as6>   	� 5      -   5        $��usePrev��5   f   $��3,0,3,3q�6   $%��Use previous opened directories��6+    d� 	IsChecked$
 TrueY�6U   6w   	� 5      -   5        $��	usePrHome��5   K   $��3q�6   $ ��Use program home directory��6&   6^   	� 5      -   5        $��useCustomDirs��5   H   $��3q�6   $��Use custom directories��6'   6_   �� 5      $��3q�6   $ 
Horizontal=�6$   {�6=   ��  �� ElementName$ useCustomDirs�� �� Path$ 	IsChecked�.+�5      �� $	��100��6   Left directory:69   �� 5      -   5        $��leftPathTextBox��5   M   $	��400��6   $��20��6&   $
��Left=�62   6f   5      �� 5      $��3q�6   $ 
Horizontal=�6$   {�6=   �� $ useCustomDirs��$ 	IsChecked�.+�5      �� $	��100��6   Right directory:6:   �� 5      -   5        $��rightPathTextBox��5   M   $��20��6   $	��400��6&   $
��Left=�62   6g   5      �� 5      $��3q�6   Custom extensions settings:6@   &   System.Windows.Controls.DataGrid  5      -   5        $��pathGrid��5   0   $	��550��6   $	��180��6#      
IsReadOnly . 5      �� 5      $ 
Horizontal=�6   .+�5       �� -   5        $��addNewExtensionSetting��5    D   $��25��6   $��25��6$   $��0q�60   $
��Left=�6b   $��0,3,3,0q�6}   
������ 
������ .��5!      �� $��/Icons/addicon.png��6   $��20��68   $��20��6C   6Q   5"      �� 5#      -   5        $��editExtensionSetting��5#   D   $��25��6   $��25��6$   $��0q�60   $
��Left=�6`   $��0,3,3,0q�6{   
������ 
������ .��5$      �� $��/Icons/editicon.png��6   $��20��69   $��20��6D   6R   5%      �� 5&      -	   5        $��removeExtensionSetting��5&   D   $��25��6   $��25��6$   $��0q�60   $
��Left=�6b   $��0,3,3,0q�6}   
������ 
������ .��5'      �� $��/Icons/deleteicon.png��6   $��20��6;   $��20��6F   6T   5(      5)      �� 5*      -
   5        $��saveSettings��5*   .   
������ $��80��6]   $��20��6h   $��Right=�6t   .��6�   Save settings6�   5+      5,      5-      