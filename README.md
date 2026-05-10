# A-Star Pathfinding

A C# **.NET 8** console application that demonstrates pathfinding on a fixed ASCII grid. The program expands a search from **A** to **B**, animating exploration with dots and then drawing the reconstructed path with underscores.

## Requirements

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- Optional: Visual Studio 2022 (17+) or Visual Studio Code with the C# extension

## How to run

From the repository root:

```bash
dotnet run --project "A-Star Pathfinding/A-Star Pathfinding.csproj"
```

You can also build with `dotnet build` or open `A-Star Pathfinding.sln` in Visual Studio and run the startup project.

**Note:** The demo uses `Console.SetCursorPosition` to redraw cells in place. Run it in a normal Windows terminal (Command Prompt, PowerShell, or Windows Terminal). Environments that only capture stdout without a cursor may not show the animation correctly.

## Project layout

Every file in this repository:

| File | Purpose |
|------|---------|
| `A-Star Pathfinding.sln` | Visual Studio solution containing the single project **A-Star Pathfinding**. |
| `A-Star Pathfinding/A-Star Pathfinding.csproj` | SDK-style project: executable targeting **net8.0**, nullable reference types and implicit usings enabled. MSBuild `RootNamespace` is `A_Star_Pathfinding`; the C# code uses namespace `AStarPathfinding`. |
| `A-Star Pathfinding/Program.cs` | The whole app: `Location` type, map data, open/closed lists, neighbor generation, distance helpers, main search loop, and console visualization. |
| `.gitignore` | Standard Visual Studio / .NET ignore rules (build outputs, `.vs/`, NuGet artifacts, etc.). |
| `.gitattributes` | `* text=auto` for consistent line endings across platforms. |

There is no separate test project, web host, or additional source beyond `Program.cs`.

## Sample map (as in code)

The initial grid printed by `CreateMap()` is:

```
+------+
|      |
|A X   |
|XXX   |
|  XX  |
| B    |
|      |
+------+
```

Legend:

- `+`, `-`, `|` — walls (not walkable).
- `` (space) — walkable.
- `X` — obstacle (not walkable).
- `A` — start at coordinates **(1, 2)** (column, row in the `map` string array).
- `B` — goal at **(2, 5)**.

## Behavior and algorithm (as implemented)

This section matches the logic in `Program.cs`; it is useful if you are comparing the code to textbook A*.

### Neighbors

`GetWalkableAdjacentSquares` considers **eight** directions (orthogonal and diagonal). A cell is walkable if the map character is a space **`' '`** or the goal **`'B'`** (so the agent can step onto the target). Start `A` is not treated as walkable for expansion onto that cell.

### Scores (`EuclidianDistance` — spelling as in source)

For a candidate neighbor square:

- **G** — Euclidean distance from the **fixed start** `(start.X, start.Y)` to that square, not the cumulative path cost along `Parent` links from start.
- **H** — Euclidean distance from the square to the **goal** `(target.X, target.Y)`.
- **F** — `G + H`.

### Search loop

1. The **open list** begins with the start `Location`.
2. While the open list is non-empty, the algorithm picks a node with **minimum F** (if several share the same F, `First` wins).
3. That node moves to the **closed list**, is drawn as `'.'`, and its walkable neighbors are processed.
4. Search stops when the goal appears in the closed list.

### Open list and re-expansion

If a neighbor is already in the **closed** list, it is skipped. If it is **not** in the open list, it is inserted with the current node as `Parent`. If it is **already** in the open list, the current implementation does **not** update it with a better G or parent (unlike typical A* presentations that relax keys). Keep that in mind when reasoning about optimality or tie-breaking.

### Visualization

- During expansion, each chosen `current` cell is written as **`.`** at its map position, with a **500 ms** delay between steps.
- After the goal is found, the path is drawn by following **`Parent`** pointers from `current` back to the start, writing **`_`** at each step with the same delay.

## Repository

Remote clone URL: `https://github.com/Naso01/A-Star-Pathfinding.git`
