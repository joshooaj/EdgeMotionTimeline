# Edge Motion Timeline Plugin

There is currently no way to render motion detected by an edge device on the
timeline in Smart Client, Web Client, or XProtect Mobile. Only motion sequences
observed by the recording server's built-in motion detection feature will be
rendered on the timeline.

This plugin makes it possible to display "Motion Started Driver" and "Motion
Stopped Driver" events as custom-colored sequences in the timeline when playing
back recorded video in Smart Client.

## Prerequisites

- Set device event retention time
  - In Management Client, open Tools > Options > Alarms and Events, and set the
    event retention for device events to a positive number. Ideally this number
    should be at least as many days as you expect to retain recorded video.
- Set "Additional data" to "Show"
  - In Smart Client, open Settings > Timeline, and change "Additional data" to
    "Show" in order to show plugin-provided timeline sequences.

## Installation

There is currently no installer. To install the plugin manually, build the
solution or download & the ZIP file, and place the contents in C:\Program Files\Milestone\MIPPlugins\EdgeMotionTimeline\
such that the plugin.def file is located at C:\Program Files\Milestone\MIPPlugins\EdgeMotionTimeline\plugin.def.

On the next Smart Client startup, the plugin should be loaded. For debugging,
enable Smart Client logs from Smart Client > Settings > Advanced > Logging. You
will find the most useful log file at C:\ProgramData\Milestone\XProtect Smart Client\MIPLog.txt.
