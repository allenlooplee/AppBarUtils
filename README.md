Introduction
===========

AppBarUtils provides out-of-box app bar behaviors/triggers/actions for Windows Phone SDK 8.0 or higher. They enable you to do command binding (also binding for other properties) for app bar buttons/menu items, make use of the built-in Blend SDK and 3rd party behaviors, and dynamically show different app bars for different pano/pivot items or based on different page states.

Get the toolkit
===========

AppBarUtils is available [via NuGet](http://nuget.org/packages/AppBarUtils). Simply add it to your project through the NuGet extension from within Visual Studio. If you are using Visual Studio Express, you can still download it from here CodePlex and add it to your project manually.

Sample code
===========

```
<!--Behaviors for fixed app bar-->
<i:Interaction.Behaviors>
    <abu:AppBarItemCommand Id="add" Command="{Binding AddCommand}"/>
</i:Interaction.Behaviors>

<i:Interaction.Triggers>
    <abu:AppBarItemTrigger Type="Button" Id="sync" 
        IsEnabled="{Binding HasData}" Text="{Binding SyncButtonDisplayText}">
        <ec:CallMethodAction MethodName="Sync" TargetObject="{Binding}"/>
    </abu:AppBarItemTrigger>
</i:Interaction.Triggers>

<!--Behaviors for dynamic app bar-->
<i:Interaction.Triggers>
    <abu:SelectedPivotItemChangedTrigger>
        <abu:SwitchAppBarAction>
            <abu:AppBar Id="0">
                <abu:AppBar.MenuItems>
                    <abu:AppBarMenuItem Text="clear" Command="{Binding SampleCommand}"/>
                </abu:AppBar.MenuItems>
                <abu:AppBarButton IconUri="{Binding AddButtonIcon}"
                    Text="{Binding AddButtonText}">
                    <ec:NavigateToPageAction TargetPage="/AddPage.xaml"/>
                </abu:AppBarButton>
            </abu:AppBar>
            <abu:AppBar Id="1" Mode="Minimized">
                <abu:AppBar.MenuItems>
                    <abu:AppBarMenuItem Text="help">
                        <ec:NavigateToPageAction TargetPage="/HelpPage.xaml"/>
                    </abu:AppBarMenuItem>
                </abu:AppBar.MenuItems>
            </abu:AppBar>
        </abu:SwitchAppBarAction>
    </abu:SelectedPivotItemChangedTrigger>
</i:Interaction.Triggers>
```
*You can find a fully functional demo in the source code.*
