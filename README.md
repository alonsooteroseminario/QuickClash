# QuickClash

#### General Usage Instructions :

QuickClash is a Tool to fix and coordinate your BIM model and make clash detection inside Revit without the necessity of using another software or without having to export any file.
The best solution for 3D coordination in very tied spaces like Mechanical Rooms or areas with little space and a high level of coordination like hospitals.
You first need to start the Add-in workflow by creating the custom instance parameters, after that, you will be able to run the clash detection for everything against everything in every active View and the analysis will be applied only to the Elements in the currently active View.


#### Installation/Uninstallation :

Please close Revit to run the installer. QuickClash installer will create installation files in the following folders: C:\Users\{username}\AppData\Roaming\Autodesk\Revit\Addins\2022 . This folder may change to match your selected Revit version.( Ex. Revit 2019, 2020, 2021)
If you would like to uninstall QuickClash, you only need to go to "Add and remove programs" in Windows and look for NonicaTab. All toolbar scripts and icons set by the user will be lost during uninstallation.

#### Support Information :

Please, contact us with suggestions for improvements or bugs at contact@weclash.xyz or via LinkedIn at https://www.linkedin.com/company/weclash

#### Additional Information :

https://weclash.xyz
https://www.linkedin.com/company/weclash

#### Known Issues :

You need to have modelled almost a pipe and a duct already to be able to start the execution.

QuickClash Revit Add-in for Clash Detection

>
>
> Include:
> + ![image](/Resources/architech-working-(1).png) Start Clash : Start the execution of the Addin. What happens when you run it is that all the Clash Parameters are created: "Clash", "Clash Solved", "Element ID", etc; along with a view called "COORD" to start using the Clash Detection tool on it.
> + ![image](/Resources/architech-working-(1).png) Default Values : Return to the initial default values of the clash parameters of the Elements of the active view.
> + ![image](/Resources/pipes-angles-(1).png) Quick Clash : Run the clash detection command to find the elements that are clashing with each other in the active view. It only applies to the elements of the current model.
> + ![image](/Resources/pipes-angles-(1).png) Quick Clash Links : Run the clash detection command to find the elements that are clashing with the Revit Links loaded, in the Window showed pick wich link your want to exclude and press ok.
> + ![image](/Resources/broom-(1).png) Filter Clash : With this button, we apply the clash filters "CLASH YES FILTER" and "CLASH NO FILTER" on the active view. If the filters do not exist, it creates them.
> + ![image](/Resources/3d-(1).png) Section Box View : With this button, we create a copy of the active 3D view and apply a Section Box to the selected elements.
> + ![image](/Resources/3d-(1).png) Section Box Zone View : With this button, create new 3D Views for all the elements closed to a Grid Location and apply a Section Box to each group of Elements.
> + ![image](/Resources/3d-(1).png) Section Box Elements Views : With this button, create new 3D Views for each elements and apply a Section Box to each Element.

## Author: LAOS

## Steps to try it in your own Revit:

#### 1. Clone it or Download it in your machine.

![image](/Resources/clone-repo.png)

#### 2. if you download it then Unzip it the files.

#### 3. Open "QuickClash.sln" file with Visual Studio.

#### 4. Build solution.

![image](/Resources/build-solution.png)

#### 5. After built the solution, in the project path enter to the folder "\bin\Debug" and Copy "QuickClash.dll" and "QuickClash.addin" files into this path: C:\Users\{username}\AppData\Roaming\Autodesk\Revit\Addins\2022

#### 6. Now you can open Revit and try it in the tab "Quick Clash"
