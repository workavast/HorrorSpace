# UI screens

`UiRoot` is the public entry point for screen navigation. Attach it to a uGUI host, assign a
`ScreenCatalog`, and optionally assign a `MonoBehaviour` implementing `IUIScreenFactory`.
Without a custom factory, `UnityScreenFactory` instantiates the screen prefab. The factory
receives the loaded `ScreenBase` prefab and its parent `Transform` and returns the instance.
Each screen prefab must have `ScreenBase` and a `Canvas` on or under its root. The screen ID
comes from the catalog or the caller, not the prefab. `ScreenBase.ScreenId` is assigned before
`Initialize()`; Unity may call `Awake` or the first `OnEnable` before that assignment.
`ScreenIdReference` converts implicitly to its validated string ID; a null reference converts
to an empty string.

```csharp
await uiRoot.PreloadCatalogAsync(createHidden: true);
uiRoot.Set("MainMenu");
uiRoot.Add("Settings", order: 5);
uiRoot.Undo();
uiRoot.Remove("Settings", ScreenReleaseMode.Release);

// A screen can also be supplied without a catalog entry.
uiRoot.Set("Help", helpPrefabReference);
await uiRoot.PreloadAsync(new[] { new ScreenDefinition("Credits", creditsPrefabReference) });
```

Each ID identifies one screen within a `UiRoot`. Different IDs may use the same prefab and have
separate screen instances. Supplying a different prefab for an existing ID throws an error.
`Remove` and `TryGetActive` also accept an ID plus a reference when the caller wants to verify
both values.

`Add` keeps active screens and places requested screens above them by default. `Set` replaces
the active set and starts sorting at zero by default. Passing `order` assigns consecutive
Canvas sorting orders to the requested group. An occupied order logs an error and still
applies the request. `Remove`, `Set`, and `Clear` hide displaced instances by default;
`ScreenReleaseMode.Release` destroys them and releases Addressables handles. `Undo` and
`UndoAsync` restore the previous active set and orders, loading released screens again if
needed. Preload does not enter navigation history.

Use `AddAsync` or `SetAsync` for a screen that has not been loaded. Synchronous methods may
block while Addressables loads on supported platforms. On WebGL, an unloaded screen cannot
be opened synchronously: preload it or use an async method. `PreloadAsync` can retain only
the prefab or also create a hidden instance. If a hidden instance was created with a
custom factory, pass that factory during preloading; an existing instance cannot change
factories until it has been released.

For simple scenes, attach `DefaultScreensLoader`, assign `UiRoot` and screen IDs, and leave
`autoRun` enabled. Disable `autoRun` to call `RunAsync` manually. The SpaceEater scene
instead awaits `PreloadCatalogAsync(true)` in `MainBootstrap` before starting game states;
its host uses `VContainerScreenFactory` with the existing `UiRootScope`.
