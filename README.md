# QuickClash
QuickClash Revit Add-in for Clash Detection


>Tool to fix and coordinate as fast as possible your BIM model, make clash detection inside Revit without the necessity of using another software. The best solution for 3D coordination in very tied spaces like Mechanical Rooms or areas with little spaces and a high level of coordination.
>
>
>
> Include:
> + ![image](/Resources/architech-working-(1).png) Start Clash : Start the execution of the Addin. What happens when you run it is that all the Clash Parameters are created: "Clash", "Clash Solved", "Element ID", etc; along with a view called "COORD" to start using the Clash Detection tool on it.
> + ![image](/Resources/architech-working-(1).png) Default Values : Return to the initial default values of the clash parameters of the Elements of the active view.
> + ![image](/Resources/pipes-angles-(1).png) Quick Clash : Run the clash detection command to find the elements that are clashing with each other in the active view. It only applies to the elements of the current model.
> + ![image](/Resources/3d-(1).png) Section Box View : With this button, we create a copy of the active 3D view and apply a Section Box to the selected elements.
> + ![image](/Resources/broom-(1).png) Filter Clash : With this button, we apply the clash filters "CLASH YES FILTER" and "CLASH NO FILTER" on the active view. If the filters do not exist, it creates them.

## Author: LAOS

## Steps to try it in your own Revit:

#### 1. Clone it or Download it in your machine.

![image](/Resources/clone-repo.png)

#### 2. if you download it then Unzip it the files.

#### 3. Open "QuickClash.sln" file with Visual Studio 2019.

#### 4. Build solution.

![image](/Resources/build-solution.png)

#### 5. After built the solution, in the project path enter to the folder "\bin\Debug" and Copy "QuickClash.dll" and "QuickClash.addin" files into this path: C:\Users\username\AppData\Roaming\Autodesk\Revit\Addins\2021

#### 6. Now you can open Revit and try it in the tab "Quick Clash"


General Usage Instructions*
Workflow of product and how it fits into user’s workflow.



Installation/Uninstallation*
Explanation of where the product is installed and instructions on how to uninstall (1000 characters). Default text should be correct for products packaged based on Autodesk Apps Framework.



Support Information*
How, when and where users can contact someone for product support. 1000 characters. Minimal HTML formatting.



Additional Information
Anything else including links to supporting files. Also put additional specifications here (2000 characters). Minimal HTML formatting.



Known Issues
Description which will be displayed for this item on the product’s page (1000 characters). Minimal HTML formatting.
