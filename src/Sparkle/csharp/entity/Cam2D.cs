using System.Numerics;
using Raylib_cs;
using Sparkle.csharp.gui;
using Sparkle.csharp.window;

namespace Sparkle.csharp.entity; 

public class Cam2D : Entity {
    
    private Camera2D _camera2D;
    
    public Vector2 Target;
    public CameraFollowMode Mode;
    
    public float MinZoom;
    public float MaxZoom;
    public float ZoomSpeed;

    public float MinFollowSpeed;
    public float MinFollowEffectLength;
    public float FractionFollowSpeed;
    
    /// <summary>
    /// Initializes a 2D camera with the specified parameters, including position, target, camera mode, and zoom level.
    /// </summary>
    /// <param name="position">The initial position of the camera.</param>
    /// <param name="target">The target location the camera should follow.</param>
    /// <param name="mode">The camera follow mode (e.g., follow smoothly or snap to target).</param>
    /// <param name="zoom">The initial zoom level (default is 5).</param>
    public Cam2D(Vector2 position, Vector2 target, CameraFollowMode mode, float zoom = 5) : base(Vector3.Zero) {
        this.Tag = "camera2D";
        this._camera2D = new Camera2D();
        this.Position = position;
        this.Target = target;
        this.Mode = mode;
        this.MinZoom = 0;
        this.MaxZoom = 10;
        this.Zoom = zoom;
        this.ZoomSpeed = 0.1F;
        this.MinFollowSpeed = 30;
        this.MinFollowEffectLength = 10;
        this.FractionFollowSpeed = 0.8F;
    }
    
    /// <summary>
    /// Gets or sets the 2D camera's current position, which is equivalent to the camera's target.
    /// </summary>
    public new Vector2 Position {
        get => this._camera2D.target;
        set => this._camera2D.target = value;
    }
    
    /// <summary>
    /// Gets or sets the rotation of the 2D camera.
    /// </summary>
    public new float Rotation {
        get => this._camera2D.rotation;
        set => this._camera2D.rotation = value;
    }
    
    /// <summary>
    /// Gets or sets the offset applied to the 2D camera's position.
    /// </summary>
    public Vector2 Offset {
        get => this._camera2D.offset;
        set => this._camera2D.offset = value;
    }
    
    /// <summary>
    /// Gets or sets the zoom level of the 2D camera, with optional minimum and maximum limits.
    /// </summary>
    public float Zoom {
        get => this._camera2D.zoom;
        set {
            if (value < this.MinZoom) {
                this._camera2D.zoom = this.MinZoom;
            }
            else if (value > this.MaxZoom) {
                this._camera2D.zoom = this.MaxZoom;
            }
            else {
                this._camera2D.zoom = value;
            }
        }
    }

    protected internal override void Update() {
        base.Update();

        if (GuiManager.ActiveGui == null) {
            this.Zoom -= Input.GetMouseWheelMove() * this.ZoomSpeed;
        }
        
        switch (this.Mode) {
            case CameraFollowMode.Normal:
                this.NormalMovement();
                break;
            
            case CameraFollowMode.Smooth:
                this.SmoothMovement();
                break;
        }
    }
    
    /// <summary>
    /// Sets the camera's position and offset for normal movement mode, centered on the screen's dimensions.
    /// </summary>
    private void NormalMovement() {
        this.Offset = new Vector2(Window.GetScreenWidth() / 2.0F, Window.GetScreenHeight() / 2.0F);
        this.Position = this.Target;
    }
    
    /// <summary>
    /// Sets the camera's position and offset for smooth movement mode, ensuring gradual tracking of the target.
    /// </summary>
    private void SmoothMovement() {
        this.Offset = new Vector2(Window.GetScreenWidth() / 2.0F, Window.GetScreenHeight() / 2.0F);
        Vector2 diff = this.Target - this.Position;
        float length = diff.Length();
        
        if (length > this.MinFollowEffectLength) {
            float speed = Math.Max(this.FractionFollowSpeed * length, this.MinFollowSpeed);
            this.Position += diff * (speed * Time.Delta / length);
        }
    }
    
    /// <summary>
    /// Defines different modes for camera following behavior.
    /// </summary>
    public enum CameraFollowMode {
        Custom,
        Normal,
        Smooth
    }
    
    /// <summary>
    /// Retrieves the 2D transformation matrix representing the current camera settings.
    /// </summary>
    /// <returns>The 2D transformation matrix based on the camera's configuration.</returns>
    public Matrix4x4 GetMatrix2D() {
        return Raylib.GetCameraMatrix2D(this._camera2D);
    }

    /// <summary>
    /// Converts a screen-space 2D position to world-space using the current camera settings.
    /// </summary>
    /// <param name="position">The screen-space position to be converted.</param>
    /// <returns>The world-space representation of the provided position.</returns>
    public Vector2 GetScreenToWorld2D(Vector2 position) {
        return Raylib.GetScreenToWorld2D(position, this._camera2D);
    }
    
    /// <summary>
    /// Converts a world-space 2D position to screen-space using the current camera settings.
    /// </summary>
    /// <param name="position">The world-space position to be converted.</param>
    /// <returns>The screen-space representation of the provided position.</returns>
    public Vector2 GetWorldToScreen2D(Vector2 position) {
        return Raylib.GetWorldToScreen2D(position, this._camera2D);
    }
    
    /// <summary>
    /// Retrieves the internal 2D camera used for rendering.
    /// </summary>
    /// <returns>The Camera2D object representing the current camera settings.</returns>
    internal Camera2D GetCamera2D() {
        return this._camera2D;
    }
    
    /// <summary>
    /// Begins a 2D rendering mode with the current camera settings.
    /// </summary>
    public void BeginMode2D() {
        Raylib.BeginMode2D(this._camera2D);
    }

    /// <summary>
    /// Ends the 2D rendering mode, restoring the default rendering state.
    /// </summary>
    public void EndMode2D() {
        Raylib.EndMode2D();
    }
}