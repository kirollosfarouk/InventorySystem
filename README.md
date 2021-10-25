# InventorySystem

This was an Optimization Test Task for interview process:

- I add the needed variables for the infoPanel and made them update when the selected item changed it was a straight forward process.
- For the High number for patches required to draw the icons for the list I moved all the icons into a sprite sheet.
- For the lag when scrolling I have created a pooling system to reuse item object instead of creating new ones.


Things Need to be addressed

 -I don't like using events to update UI a better solution is to implment a generic message system to seperate classes better
 -Move reading and creating objects from a jason object out of InventoryManager (maybe a helper)
