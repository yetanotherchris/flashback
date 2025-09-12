# Copilot Instructions for Flashback

## Project Overview

Flashback is a Supermemo-inspired spaced repetition flashcard app being migrated from MonoTouch iOS to .NET MAUI. The codebase follows a **bridge architecture** that maintains compatibility between legacy patterns and modern cross-platform implementations.

## Architecture & Key Components

### Core Domain (Supermemo Algorithm)
- **`src/Flashback.Core/Domain/QuestionManager.cs`** - Contains the Supermemo algorithm for interval calculation and spaced repetition scoring (1-6 scale)
- **Scoring System**: 1=didn't remember, 2-3=wrong but recognized, 4-5=correct with delay, 6=perfect recall
- **Interval Logic**: First ask = 1 day, second = 6 days, subsequent = previous × easiness factor

### Repository Pattern & Data Access
- **Legacy Bridge**: `src/Flashback.Core/Repository/Repository.cs` provides static singleton access via `Repository.Default`
- **Modern Implementation**: `src/Flashback.Core.Net/Repositories/SqliteRepository.cs` uses `Microsoft.Data.Sqlite` for cross-platform SQLite
- **Key Tables**: `categories` (id, name, inbuilt, active), `questions` (supermemo fields: interval, easinessfactor, nextaskon, etc.)
- **Dependency Injection**: MAUI app registers `IRepository` in `MauiProgram.cs` and bridges to legacy static pattern

### Platform Strategy
- **Current Structure**: Original iOS (`src/Flashback.UI/`) + New MAUI (`src/Flashback.Maui/`)
- **Migration Pattern**: Port iOS controllers → MAUI pages (e.g., `RootController` → `MainPage`, `CategoriesController` → `CategoriesPage`)
- **Build Targets**: Android (working), iOS/Windows/macCatalyst (basic structure, may need platform-specific fixes)

## Development Workflows

### Building & Testing
```powershell
# Build specific projects
dotnet build src/Flashback.Core.Net/Flashback.Core.Net.csproj
dotnet build src/Flashback.Maui/Flashback.Maui.csproj

# Run tests
dotnet test test/Flashback.Tests.VS2010/

# MAUI-specific builds (requires VS 2022+ with MAUI workloads)
dotnet build src/Flashback.Maui/ -f net8.0-android
```

### Database Development
- **Auto-initialization**: Database creates automatically in `FileSystem.AppDataDirectory` on first run
- **Schema**: Tables auto-created by `SqliteRepository.CreateDatabase()`
- **Testing**: Use smoke tests in `Flashback.Core.Net` project (set `SmokeTest=true` to build as executable)

## Project-Specific Patterns

### Question/Category Models
- **Required Properties**: Use `required` keyword for non-nullable fields (`Title`, `Answer`, `Category`)
- **Supermemo Fields**: Always include `EasinessFactor`, `Interval`, `PreviousInterval`, `ResponseQuality`, `AskCount`
- **DateTime Handling**: Store as `Ticks` in SQLite, convert via `new DateTime(reader.GetInt64())`

### Legacy Compatibility
- **Static Repository Access**: `Repository.Default.SaveQuestion(question)` calls should continue working
- **Domain Model Methods**: Keep static methods like `Question.Save()` and `Category.Read()` that wrap repository calls
- **Namespace Preservation**: Don't change `Flashback.Core` namespaces without updating references

### MAUI Migration Guidelines
- **Controller → Page**: Convert iOS `UIViewController` classes to MAUI `ContentPage` with XAML
- **Navigation**: Replace `PushViewController` with MAUI Shell or `Navigation.PushAsync`
- **Dependency Injection**: Use constructor injection in pages for `IRepository` instead of static access
- **MonoTouch References**: Replace `MonoTouch.UIKit` with MAUI controls (`UILabel` → `Label`, `UIButton` → `Button`)

## Common Tasks

### Adding New Question/Category Features
1. Update domain models in `src/Flashback.Core/Domain/`
2. Extend `IRepository` interface if needed
3. Implement in `SqliteRepository` with proper SQL schema updates
4. Bridge to legacy `Repository.Default` pattern
5. Add MAUI UI in `src/Flashback.Maui/`

### Platform-Specific Issues
- **iOS**: May need asset updates and Info.plist configuration
- **Windows**: Ensure proper program entry point in `Platforms/Windows/`
- **Android**: Check `AndroidManifest.xml` for permissions and platform features

### Task Tracking
- **Active Steps**: Follow `agent-todo.md` - mark completed items with ✅
- **Migration Status**: Check README.md for current platform build status and next steps

## Key Files for Context
- `agent-todo.md` - Current migration checklist
- `README.md` - Project status and build instructions  
- `src/Flashback.Core/Domain/` - Core business logic and Supermemo algorithm
- `src/Flashback.Maui/MauiProgram.cs` - Dependency injection and app configuration
- `src/Flashback.Core.Net/Repositories/SqliteRepository.cs` - Modern data access implementation

git add and git commit all files you've changed, with a suitable commit message when you are done. prefix the commit message with "[copilot-{modelname}]" replacing modelname with a short version of the model name, e.g. "gpt5", "claude4".