# BorderlessForm

Implements a borderless window using Windows Forms that uses normal controls to preserve the functionality of sizable windows such as:

- Move by dragging the title bar
- Size by dragging the borders and corners
- Maximize, minimize, and close buttons
- System menu
- Double click title bar to maximize
- Aero snap

The window decoration controls in the example are simple labels and can be changed to provide a custom look.

Example screenshot:

![Example](screenshot.png "Example screenshot")

Unsupported:

- Border shadow ("glow"). Office 2013 and Visual Studio 2013 implement this using [4 additional top level windows](http://stackoverflow.com/a/15303486) that keep in sync with the main window's size and position. 

