# Unity Optimize Inventory Test Instructions

For this test, you are to complete and optimize an inventory panel.
Some notes:

- The scene to load is located in `Scenes/Main.unity`.
- The inventory presents a large number (>1000) of items in a Scroll View.
    These are generated from a JSON member.
- Items can be clicked on and are selectable.
- To complete this test you will have to modify both code and Unity UI.
- As you make changes to the UI make sure you consider supportying layout changes across devices.

Tasks to complete:

1. There is an `InfoPanel` that displays the _Name_, _Descripton_, and _Stat_ for an item. 
    Please display the correct information when an item is selected. In addtion, make sure 
    an item is selected when the app first starts. **(Est. 15 minutes.)**

2. If you run the game with `Stats` from the Unity Editor, you will notice the number of Batches is very high. 
    Without reducing the items or data being displayed, decrease the number of batches and
    do your best to increase performance. This project is very simple and it should be able to 
    run above 60fps on a low-end machine while scrolling. **(Est. 2 hours.)**

3. **OPTIONAL** Please use this time to show off any skills whether improving the game visually or adding gameplay.
    This can include:
    - Support UI scaling across devices.
    - Displaying more images in the info.
    - Adding animations, or sliders for your character.
    - ETC...
    **NOTE:** This task is not necessary and should be considered when #1 and #2 above have been completed.