   M S B A M L   `   `   `  ����B  >WpfApp2, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null!clr-namespace:WpfApp2WpfApp2  5   	   7 clr-namespace:WpfApp2.PropertiesWpfApp2.Properties  5   	       WpfApp2.MainWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35I EUno.UI, Version=255.255.255.255, Culture=neutral, PublicKeyToken=nullF 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     K GUno.Xaml, Version=255.255.255.255, Culture=neutral, PublicKeyToken=nullR NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089>x,http://schemas.microsoft.com/winfx/2006/xaml       5   	   Dd2http://schemas.microsoft.com/expression/blend/2008       5   	   Nmc;http://schemas.openxmlformats.org/markup-compatibility/2006       5   	   +localclr-namespace:WpfApp2       5   	   ;
Properties clr-namespace:WpfApp2.Properties       5   	   $	��480��5      $	��830��6'     � 	Resources  5	         Storyboard1��%   &	        �� 5
   
   6   5      .��5      � $��Center=�6   $��0,0,0,0q�6(   $	��800��69   $	��Top=�6E    � DataContext 5   
      WpfApp2.ShoppingCart  5       � RowDefinitions 5   
   �� 5      $��30 �6   �� 5      $��30 �6   �� 5      $��30 �6   �� 5      $��30 �6   �� 5      $��30 �6   �� 5      $��30 �6   �� 5       � ColumnDefinitions 5   
   �� 5      �� 5      .+�5   
   �� 
���   �$��4��6$   .��62   Count of files in dir:5      �� 5    
   -   5         ��Name$ TF��5       $��4��6    $��2��6-   ��6=   �� *	ItemCount+ 5!        5"      5$      �� 5%   
   $��3��6   $��0��6   .��6-   Current Dir6:   �� 5&   
   -   5        $ TE��5&      $��3��6    $��1��6-   ��6=   �� *	TargetDir+ 5'        5(      5*      �� 5+   
   .��6   
Enter dir:5-      �� 5.   
   -   5         ��Name$ TC��5.      $��0��6   $��1��6+   $��0,5,10,5q�6;   �� 50   
   $��1��6   .��6   Enter loannum delimiter:52      �� 53   
   -   5        $ TD��53      $��1��6   $��1��6+   $��0,5,10,5q�6;   �� 54   
   $��5��6   .��6   SEARCH loannum :56      �� 57   
   -   5        $ TU��57      $��6��6   $��0��6+    �� AcceptsReturn . $
��Auto��6P    �� TextWrapping$
 Wrap=�6^   $��0,5,10,5q�6r   �� 59   
   -   5        $��submit��59   .   $��2��6   $��0��6   $��2q�6<   .��6X   
View files5;      �� 5<   
   -   5        $��update��5<   .   $��2��6   $��1��6   $��2q�6<   .��6\   UPdate view5>      �� 5?   
   -   5        $��print��5?   .   $��5��6   $��1��6   $��2q�6;   .��6\   Print Manifest5A      �� 5B   
   -	   5        $ TG��5B      $��1��6    $
��Left=�60   $��68��6K   $��
0,-68,-2,0q�6W   $
��Wrap=�6k   $��	TextBlock��6   $	��Top=�6�   $	��550��6�   R� 5C   
   -
   5         R�Name$ TT��5C      $��6��6   $��1��6,   5E      5F      