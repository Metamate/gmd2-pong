# gmd2-pong

Source code for session **01 Pong** of the Game Architecture (GAR) course.

The game is built up in steps. Each step is a separate project that builds on the previous
one, following the exercises from the session. Compare two neighbouring steps (e.g. with a
diff tool) to see exactly what changed.

| Step | Exercise | What's new |
| --- | --- | --- |
| `Pong0` | New game | An empty MonoGame project |
| `Pong1` | Hello Pong | Text centred on a 1280×720 window |
| `Pong2` | Virtual resolution | Resolution independent of the window, point filtering |
| `Pong3` | Drawing rectangles | Paddles and ball, and a custom retro font (`arial` → `font`) |
| `Pong4` | Paddle movement | Keyboard input, frame-rate independent movement |
| `Pong5` | Ball movement | Launch the ball, keep the paddles on screen |
| `Pong6` | Encapsulation | `Paddle` and `Ball` classes (Update Method pattern) |
| `Pong7` | Collision detection | AABB bounces off paddles and walls |
| `Pong8` | Scoring | Scores in a bigger font |
| `Pong9` | Serve state | The player who was scored on serves |
| `Pong10` | Victory | A done state when a player reaches 10 points |
| `Pong11` | Audio | Sound effects for hits and scoring |

## Content

All steps share one folder of raw assets (fonts, images, sounds), built by the **content
builder** (MonoGame 3.8.5+):

```text
Content/
├── Assets/                  # The raw assets, shared by all steps
├── Builder/Builder.cs       # The rules for building the assets, in C#
├── BuildContent.targets     # Runs the builder when a game project builds
└── Content.csproj
```

There is no `.mgcb` file and no MGCB Editor. `Builder.cs` decides how each kind of asset is
processed. Each step project imports `BuildContent.targets`, so building a step also builds
its assets into its output folder, where `Content.Load` finds them.

To add an asset, put it in `Content/Assets` and, if no existing rule matches it, add a rule
in `Builder.cs`. Compare the `Content.Load` calls in neighbouring steps to see when each
asset comes into use.

## Running a step

Requires the [.NET 10 SDK](https://dotnet.microsoft.com/download).

```sh
dotnet run --project Pong11
```

Or open `Pong.slnx` and choose the step to run.
