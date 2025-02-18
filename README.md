# MoveImages
Utility to move images to a different folder based on characteristics like orientation and dimensions

## Examples:

`MoveImages /orientation:landscape c:\data\images\landscape`

Move all the landscape oriented images from the current folder to `c:\data\images\landscape`

`MoveImages /minsize:1024 largeImages`

Move all the images where the largest dimension (width or hight) is larger than 1024 pixels from the current folder to the `largeImages` folder (under current folder)

`MoveImages /megapixels:>2 largeImages`

Move all the images larger than 2 megapixels from the current folder to the `largeImages` folder (under current folder)

### Filters can be combined as well, for example:

`MoveImages /orientation:portrait /megapixels:<1 c:\images\smallPortrait`

Move all the portrait oriented images smaller than 1 megapixel from the current folder to `c:\images\smallPortrait`