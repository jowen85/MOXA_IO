   M S B A M L   `   `   `  ����>  :GPS, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null?$clr-namespace:MOXA_IO.CustomControlsMOXA_IO.CustomControls  5   	   )    #MOXA_IO.CustomControls.SplashScreen   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   6local$clr-namespace:MOXA_IO.CustomControls     5   	   -   5          �Name$  splashScreen��5	   	    � Title$ SplashScreen��5
   	   $	��270��6   $	��480��6+    � WindowStyle$
 None=�68    � 
ResizeMode$ NoResize=�6K    � WindowStartupLocation$ CenterScreen=�6a    � Topmost . .��5      � .+�5   
   �� -   5         ��Name$ imgLogo��5   -   $��0��6   $��0��6   $��UniformToFill=�6>   $0��*/Library;component/Images/SplashScreen.png��6V   �� 5   
   
���   � J� Opacity$	 0.5Q�6!   �� 5   
   -   5        $��
lblVersion��5      $��	[Version]��6"   $��12(�66   
�������$
��Left=�6\   $	��Top=�6w   �� 5   
   $(��"Greatech Integration (M) SDN. BHD.��6   $��12(�6=   
�������$��Right=�6c   $	��Top=�6   �� 5   
   $
��Left=�6   $	��Top=�6+   $��	21,64,0,0q�6C   
�������$��18(�6n   $
��Bold"�6|   $	��450��6�   $��60��6�   ��5      �� 5      -   5        $��lblMachineName��5       �� TextWrapping$
 Wrap=�63   $��[Machine Name]��6G   5      �� 5   
   -   5        $��tbLoadTitle��5      $��12(�6'   
�������$
��Left=�6M   $��Bottom=�6h   $��	21,0,0,70q�6�   � 5   
   -   5        $��progBar��5      $��15��6%   $	��440��61   $��Bottom=�6=   $
��Left=�6X   $��	21,0,0,43q�6s   	 � Maximum$		 100Q�6�   5      5      