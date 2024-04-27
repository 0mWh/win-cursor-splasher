# Cursor Splasher

[![Build](https://github.com/0mWh/win-cursor-splasher/actions/workflows/dotnet-desktop.yml/badge.svg)](https://github.com/0mWh/win-cursor-splasher/actions/workflows/dotnet-desktop.yml)

Accessibility tool to make it more obvious when the mouse moves long distances.

Application Tabs:
1. Info
   - location: mouse coordinates relative to the top left corner of a rectangle fitting all displays.
   - screen: which screen is the mouse on?
   - relative: mouse coordinates on the current screen.
2. Outline
   - width (pixels): how thick is outline of the circle that appears 
   - radius (pixels): also of the circle that appears
   - duration (seconds): how long does the circle stick around?
3. Updates
   - only mark screen borders:
     - checked: the circle will only appear if the cursor changes screens.
     - unchecked: the circle will appear after the cursor travels enough.
   - distance (pixels): how far does the mouse have to move (during a time window) before a circle appears?
   - timing (seconds): how often to check the cursor position? note that this affects the distance setting above.
4. Credits
   - information that leads here

Several times I've been working on a desktop with many screens and I lost track of which monitor the mouse cursor was on.
On further investigation, I didn't find anyone who did not experience this issue.
