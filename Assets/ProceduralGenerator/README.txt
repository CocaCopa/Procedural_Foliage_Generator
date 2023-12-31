Jump to sections using Ctl+F
1. Introduction
2. How It Works
3. Getting Started
4. Customize Your Foliage Generator
5. Inspector Buttons
6. Frequently Asked Questions (FAQ)

--------------------------------------------------------------------------------------------
--------------------------------------- Introduction ---------------------------------------
--------------------------------------------------------------------------------------------
Welcome to the Procedural Foliage Generator, your go-to tool for effortlessly adding natural
elements like trees, grass, and more to your Unity projects. Whether you're creating an
open-world game, a dense forest environment, or simply looking to spruce up your scenes
with greenery, our Procedural Foliage Generator is here to help.

Discover a user-friendly solution for generating realistic foliage in your Unity projects.
This package simplifies the process, allowing you to bring lush, natural environments to
life without the complexity.

--------------------------------------------------------------------------------------------------------------
------------------------------------------------ How It Works ------------------------------------------------
--------------------------------------------------------------------------------------------------------------
----------------------------------------------- Parent foliage -----------------------------------------------
1. Generation area:
   - When you click the 'Generate' button, the tool will cast raycasts at random positions, from the collider's
     upper face downward to the collider's lower face.
   - The number of casts depends on the value set in the 'intensity' field.

2. Search for valid ground:
   - Initially, all raycasts will search for layers where foliage cannot spawn. If such a layer is detected,
     that raycast's position will be completely ignored.
   - If no invalid layers are found, the tool will proceed by casting a new set of raycasts, this time searching
     for valid ground. It will save the positions where valid ground is found.

3. Validity of position:
   - Once the tool has the position for the foliage, it will spawn a foliage object randomly selected from the
     'primitives' list.
   - It will then generate a virtual circle around that foliage with a random radius between the
     'minParentBlankArea' and 'maxParentBlankArea' values.
   - Every other potential parent position, will be checked to ensure it's outside of any of the previously
     generated parent circles. If it is, the tool will spawn another parent foliage at that position.

------------------------------------------------ Child foliage ------------------------------------------------
1. Generation area:
   - For each parent, a virtual ring torus is generated around it. The outer radius of the torus is defined by
     the 'spreadDistance' value, and the inner radius is defined by the 'distanceFromParent' value.
   - Raycasts will only be allowed to save their calculated positions if they fall inside that torus.

2. Search for valid ground:
   Similar to the procedure for parent foliage, child foliage follows the same process.

3. Validity of position:
   - Similar to parent foliage, all child foliage generates a virtual circle around them to prevent other
     child foliage from spawning inside. The radius of this circle is determined by the 'minChildBlankArea'
     and 'maxChildBlankArea' values.
   - Depending on the value set for the 'parentDistanceMode' enum, child foliage will need to maintain the
     specified distance from all parents or just their own parent. (For more details on the
     'parentDistanceMode' enum, see the 'Customize Your Foliage Generator' section.)

---------------------------------------------------------------------------------------------
-------------------------------------- Getting Started --------------------------------------
---------------------------------------------------------------------------------------------
1. Adding the ProceduralSpawner to Your Scene:

   Locate the 'ProceduralSpawner' prefab in your Unity project assets.
   Drag and drop the 'ProceduralSpawner' prefab into your Unity scene.
   Configuring the Spawner:

2. Select the 'ProceduralSpawner' in your Hierarchy or Project panel.

   In the Inspector, you'll find the following fields:

   1. Primitives: Assign the parent objects (primitives) you want to use for generating foliage.
   2. Children: Assign the child objects you want to use for generating foliage.
      Each time a parent or child is set to spawn, the system will randomly select an object 
      from their respective lists.

3. Customizing Your Foliage:

   With your primitives and children assigned, you're ready to customize your foliage generation.

------------------------------------------------------------------------------------------------
------------------------------- Customize Your Foliage Generator -------------------------------
------------------------------------------------------------------------------------------------
1.  ----------------------------------- Foliage Holder Name: -----------------------------------
    Specify the name of the parent GameObject that will hold all the generated foliage. This
    allows you to keep your Hierarchy organized and easily locate the generated foliage.

2.  -------------------------------------- Foliage Layer: --------------------------------------
    Here, you can assign a layer that will be assigned to every generated foliage object. This
    is useful for managing interactions or visibility settings in your project.

3.  --------------------------------------- Foliage Tag: ---------------------------------------
    This field allows you to assign a tag to every generated foliage object. Tags can be used to
    identify and manage objects with similar characteristics, making it easier to work with them
    in your project.

4.  ------------------------------------- Spawn On Layer: --------------------------------------
    Use this field to specify the layer on which the foliage should be spawned. This helps you
    control where the generated foliage appears in your scene, providing flexibility in scene
    design.

5.  ---------------------------------------- Intensity: ----------------------------------------
    The 'Intensity' setting controls the density of foliage in the specified area. Adjust this
    value to achieve the desired level of coverage for your environment.

6.  ----------------------------------- MarginX and MarginZ: -----------------------------------
    These settings enable you to cut out a certain distance on the X and Z axes. This can be
    helpful for creating open areas within your foliage-covered landscape.

7.  ------------------------------------- Spread Distance: -------------------------------------
    The spread distance of children foliage around their parent. Adjust this value to control
    how far apart children foliage objects are placed around their parent.

8.  --------------------- Min Parent Blank Area and Max Parent Blank Area: ----------------------
    These settings determine the minimum and maximum distances from each parent at which other
    parents cannot spawn. Use these values to create diverse and natural-looking layouts.

13. -------------------------- Min Parent Scale and Max Parent Scale: --------------------------
    These settings allow you to define the minimum and maximum scales for parent foliage.
    Adjusting these values gives you control over the size variation within the parent foliage.

9.  ------------------------------------ Number of Children: ------------------------------------
    Specify the maximum number of children a parent can spawn. This setting allows you to control
    the quantity and density of smaller foliage objects around parents.

10. ----------------------------------- Parent Distance Mode: -----------------------------------
    Choose between two modes for child placement. "All Parents" ensures children maintain the
    desired distance from all parents, while "Own Parent" requires children to keep a minimum
    distance only from their own parent.

11. ----------------------------------- Distance From Parent: -----------------------------------
    Set the minimum distance between children and their parent. This ensures that child foliage
    objects are distributed evenly and naturally within the parent's area.

12.  ---------------------- Min Child Blank Area and Max Child Blank Area: -----------------------
    These settings determine the minimum and maximum distances from each child at which other
    children cannot spawn. Use these values to create diverse and natural-looking layouts.

13. ---------------------------- Min Child Scale and Max Child Scale: ----------------------------
    These settings allow you to define the minimum and maximum scales for children foliage.
    Adjusting these values gives you control over the size variation within the child foliage.

-------------------------------------------------------------------------------------------------------------
--------------------------------------------- Inspector Buttons ---------------------------------------------
-------------------------------------------------------------------------------------------------------------
Generate: The tool will generate foliage based on the given values
Delete  : The tool locates and removes a GameObject with the name specified in the 'FoliageHolderName' field.


------------------------------------------------------------------------------------------------
------------------------------- Frequently Asked Questions (FAQ) -------------------------------
------------------------------------------------------------------------------------------------
Q: Why isn't my generator spawning any foliage?
--
A: Ensure that the generator's bounding box, defined by its box collider, encompasses valid
   ground within the dimensions of the box collider.

Q: Can I run the generator in the same area multiple times to spawn different types of foliage?
--
A: Yes, you can. To do so, assign all generated foliage to the same dedicated layer. This allows
   the tool to detect and avoid previously generated foliage. If you assign each generation to
   a different layer, it might cause the tool not to avoid earlier generations correctly.

Q: When I generate foliage in an area that already has existing foliage, the new generation
   overlaps with the previous one. What can I do to avoid this?
--
A: While the generator doesn't require colliders for detecting foliage, this may not apply when
   you want to generate different foliage in an area that's already occupied. For objects like
   trees, collisions usually won't be an issue, as they typically have colliders. However, for
   other foliage like grass or flowers, where you generally don't want collisions, you can follow
   this work-around:

   1. Locate your foliage prefab in your project files.
   2. Attach a collider to it. This will update all scene objects with the collider.
   3. Run the generator in the area.
   4. Remove the added collider from your prefab.