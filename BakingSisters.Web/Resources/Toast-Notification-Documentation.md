# Toast Notification System Documentation

The BakingSisters.Web application uses MudBlazor's Snackbar service to provide toast notifications throughout the application. This document explains how to use this system in your pages and components.

## Overview

The toast notification system is implemented as a service that wraps MudBlazor's ISnackbar service. It provides a simple, consistent API for showing different types of toast notifications.

## Setup

The toast notification system is already set up at the application level:

1. MudBlazor services are registered in `Program.cs`
2. The `ToastService` is registered as a scoped service in `Program.cs`
3. MudBlazor providers are included in the `MainLayout.razor` file

## Using Toast Notifications

### Step 1: Inject the Service

In any Razor component where you want to show toast notifications, inject the `IToastService`:

```csharp
@inject IToastService ToastService
```

### Step 2: Show Toast Notifications

The `IToastService` provides four methods for showing different types of notifications:

1. Success notifications:
```csharp
ToastService.ShowSuccess("Operation completed successfully");
```

2. Error notifications:
```csharp
ToastService.ShowError("An error occurred");
```

3. Warning notifications:
```csharp
ToastService.ShowWarning("Warning: This action cannot be undone");
```

4. Info notifications:
```csharp
ToastService.ShowInfo("Your session will expire in 5 minutes");
```

### Optional Title

Each method accepts an optional title parameter:

```csharp
ToastService.ShowSuccess("User was created successfully", "Success");
```

## Configuration

The default configuration for toast notifications is set in the `ToastService` constructor:

- Position: Top right corner
- Duration: 5 seconds
- Show close icon: Yes
- Transition durations: 500ms

If you need to change these settings globally, modify the `ToastService.cs` file.

## Example

```csharp
@page "/example"
@inject IToastService ToastService

<button class="btn btn-primary" @onclick="SaveData">Save Data</button>

@code {
    private async Task SaveData()
    {
        try
        {
            // Save data logic here...
            
            // Show success notification
            ToastService.ShowSuccess("Data saved successfully");
        }
        catch (Exception ex)
        {
            // Show error notification
            ToastService.ShowError(ex.Message, "Error");
        }
    }
}
```

## Demo Page

A demonstration page is available at `/toast-demo` where you can see all types of toast notifications in action. 