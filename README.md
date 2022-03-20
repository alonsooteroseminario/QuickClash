# QuickClash
QuickClash Revit Add-in for Clash Detection


> Software Open Source. Revit Add-in Tool to fix and coordinate as fast as possible your BIM model.
> Include:
> + Start Clash : Start the execution of the Addin. What happens when you run it is that all the Clash Parameters are created: "Clash", "Clash Solved", "Element ID", etc; along with a view called "COORD" to start using the Clash Detection tool on it.
> + Default Values : Return to the initial default values of the clash parameters of the Elements of the active view.
> + Quick Clash : Run the clash detection command to find the elements that are clashing with each other in the active view. It only applies to the elements of the current model.
> + Section Box View : With this button, we create a copy of the active 3D view and apply a Section Box to the selected elements.
> + Filter Clash : With this button, we apply the clash filters "CLASH YES FILTER" and "CLASH NO FILTER" on the active view. If the filters do not exist, it creates them.

## Author: LAOS

## Steps to try it in your own Revit:

### 1. Clone it or Download it in your machine.

### 2. if you downloaded it then Unzip it the files.

### 3. Open "QuickClash.sln" file with Visual Studio 2019.

### 4. Build solution.

### 5. After built the solution, in the project path enter to the folder "\bin\Debug" and Copy QuickClash.dll and QuickClash.addin files into this path: C:\Users\{{username}}\AppData\Roaming\Autodesk\Revit\Addins\2021

### 6. Now you can open Revit and try it in the tab "Quick Clash"