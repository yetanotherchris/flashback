MAUI migration - next steps

Goal: finish migrating the old MonoTouch app to a cross-platform .NET MAUI app.

High-level next steps (prioritised):
✅ 1) Finalise cross-platform data layer
   - Replace the iPhone-only `SqliteRepository` with a cross-platform implementation.
   - Options: `sqlite-net-pcl` (easy ORM) or `Microsoft.Data.Sqlite` (raw SQL). Use `FileSystem.AppDataDirectory` for DB file path per-platform.
   - Register platform repository in `MauiProgram` (per-target DI) so `Repository.SetInstance(...)` uses the real SQLite repo in production.

1a) Move all code to src/ folder and all test to /tests folder.

1b) Ensure the gitignore file is correct for a maui c# app.

2) Port one representative screen to MAUI (template)
   - Port `RootController` -> `MainPage` to establish navigation, resource usage, and a pattern for other screens.
   - Convert custom views (ScoreButton, UnderlineButton, BusyView) into MAUI controls or small handlers.

3) Incrementally port controllers/pages
   - Categories / CategoryHub -> MAUI pages
   - Questions views (list, ask, edit) -> MAUI pages
   - Import/Export and Tips/Help pages (use WebView or embedded HTML resources)

4) Move assets
   - Move images to `Resources\Images` and HTML to `Resources\Raw` (or EmbeddedResources) in the MAUI project and update paths.

5) Platform polish
   - Implement app-badge / launcher links via platform-specific handlers.
   - Add per-platform manifest info (iOS entitlements, Android permissions) in MAUI project.

6) Solution and CI
   - Add `Flashback.Maui` and `Flashback.Core.Net` to the main solution file.
   - Add a CI job that runs `dotnet build` for core and `dotnet build -f:net8.0-windows10.0.22621.0` for MAUI Windows (or run platform builds in separate jobs with proper workloads installed).

Commands to run locally (PowerShell) — requires MAUI workloads / Visual Studio with MAUI:
dotnet build .\Flashback.Core.Net\Flashback.Core.Net.csproj -c Release
dotnet build .\Flashback.Maui\Flashback.Maui.csproj -c Release

If you hit workload errors:
dotnet workload list
dotnet workload install maui

Immediate recommendations (pick one and I will implement it):
- Option A: Implement cross-platform SQLite repository using `sqlite-net-pcl` and wire into `MauiProgram` (I can implement and test the core build). 
- Option B: Port `RootController` and one nav flow to MAUI (I will convert controllers/views to MAUI pages and wire NavigationPage). 

Notes:
- I created `Flashback.Core.Net` (netstandard) and `Flashback.Maui` scaffolds in this branch so we can iterate safely without changing original MonoTouch code.
- Building MAUI requires correct .NET SDK/workloads; I can continue coding even if your environment needs setup — the code will be ready to build once workloads are installed.

Next action I will take after you pick an option: implement the chosen item and run builds/tests for the modified projects.
