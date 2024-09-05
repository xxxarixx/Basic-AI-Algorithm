# Farmer Simulation <br>
## Project Overview  <br>
<img src="ReadmeResources/FullScreenShot.jpg" width=50%>
This project aimed to demonstrate the concept of complexity in simplicity and scalability. <br>
It showcases my ability to create efficient and adaptable code, even for complex projects. <br>

### Key Features <br>

#### A Pathfinding:* <br>
* Isolated pathfinding system using assembly definitions <br>
* Customizable grid sizes and center points, large scale friendly <br>

<img src="ReadmeResources/LargeScaleAstar.jpg" width=50%>

* Flexible path types (sharp linear or smooth bezier curve) <br>
Linear:__________________________Bezier curve:<br>
<img src="ReadmeResources/ScreenShotLinear.jpg" align='left'>

<img src="ReadmeResources/ScreenShotBezier.jpg">

* Automatic obstacle mapping for squares and rectangles <br>
<img src="ReadmeResources/AutoMapping.jpg" width=50%>

* Fully documented <br>

<img src="ReadmeResources/AstarDocumented.jpg" width=50%>

#### Camera Controls: <br>
* Boundaries and zooming <br>
* Farmer following <br>
* Drag-to-move <br>
* Perspective and orthographic camera modes <br>
#### Multiple Farmer AI: <br>
* Open-ended AI system for future actor types <br>
* Realistic farmer behavior with limited inventory and work interactions <br>
* State machine-based AI with 8 distinct states that are: <br>
-Deploy crops state => if wanna transition to planting or have maximum capacity of seeds <br>
-Deploy seeds state => if wanna transition to collecting crops or doesnt have seeds and want to plant <br>
-Find empty crop ground state => if wanna plant seed but isn't near planting spot <br>
-Find grown crop => if wanna collect crop but isn't near that crop spot <br>
-Gather crops state => if wanna collect crop that is standing on <br>
-Gather seeds state => if is near chest that giving seeds and wanna collect seeds <br>
-Plant state => if wanna plant seeds and is in empty crop ground place <br>
-Wait for new work => if there is no work to do or something went wrong, safe state <br>
#### UI <br>
* Debugging friendly with text popups
* Ability to change simulation speed in runtime
* Visual showcase of farmers job statistics

#### Technical Details: <br>
Packages: URP, DOTween, GridBox Prototype Materials, TextMeshPro <br>
Custom Features: A* pathfinding, camera controls, multiple farmer AI, UI <br>

####  Future Plans: <br>
Add new types of plants <br>
