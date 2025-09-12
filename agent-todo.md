current-prompt.txt

This file lists the active steps the assistant and maintainer should follow. When a step is completed, prefix it with a checked emoji (✅) so future runs will skip it.

Created: 2025-09-12 by assistant

Steps:

0. ✅ Create this file in the repository so the instructions exist and can be followed.

1. ✅ Implement a cross-platform SQLite repository for the MAUI app using sqlite-net (or an equivalent cross-platform SQLite library). The repository should live in the appropriate project (e.g., `Flashback.Core` or `Flashback.Maui`) and provide the same `IRepository` surface expected by the app.

	Note: A cross-platform implementation already exists at `Flashback.Core.Net/Repositories/SqliteRepository.cs` using `Microsoft.Data.Sqlite`.

2. ✅ Wire the repository into dependency injection in `MauiProgram` so the MAUI app uses the cross-platform SQLite repository at runtime.

	Note: `Flashback.Maui/MauiProgram.cs` registers `IRepository` and sets the legacy `Repository.SetInstance(repo)` bridge.

3. ✅ Add any required NuGet package references (e.g., `sqlite-net-pcl`) to the MAUI project files and update project files as necessary.

	Note: `Flashback.Core.Net.csproj` contains a `PackageReference` to `Microsoft.Data.Sqlite` (already present in the repo).

4. ✅ Add a small smoke test or sample usage that verifies the repository can create the DB file, insert a sample question, and read it back.

5. ✅ Run a build for the MAUI project (or a minimal compile) and fix any compile issues introduced by the repository changes.

	Note: Successfully fixed compilation errors including missing resource files, MAUI package references, dependency injection setup, and XAML issues. The Android target now builds successfully. Platform-specific issues remain for iOS, Windows, and macCatalyst but the core repository integration is working correctly.

6. ✅ Update repository README or developer notes describing how to run the MAUI app and any platform-specific setup for the SQLite database.

	Note: Updated README.md with comprehensive documentation covering the cross-platform SQLite implementation, dependency injection setup, automatic database initialization, and build status for different platforms.

7. ✅ Move all source code to "/src" folder". Move all tests to "/test" folder.

	Note: Successfully reorganized the project structure. Moved all source projects (Flashback.Core, Flashback.Core.iPhone, Flashback.Core.Net, Flashback.CsvConverter, Flashback.Maui, Flashback.UI) to the /src folder and test projects (Flashback.Tests.VS2010) to the /test folder. Updated solution files (Flashback.sln and Flashback.VS2010.sln) to reference the new paths, and corrected project references to maintain build compatibility.

Notes / Assumptions:
- If any step requires clarification, annotate the step and continue with the most conservative, low-risk implementation.
- Keep breaking changes minimal; prefer adding new files and wiring rather than refactoring existing public APIs.

If you want me to start executing these steps now, tell me which step to begin with (for example: "start with step 1"), or I can proceed to implement step 1 automatically.
