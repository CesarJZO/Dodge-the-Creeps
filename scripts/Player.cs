using Godot;

namespace DodgeTheCreeps;

public sealed partial class Player : Area2D
{
    [Signal]
    public delegate void HitEventHandler();

    [Export] private bool startHidden;
    [Export] private int speed = 400;
    private Vector2 _screenSize;

    public void Start(Vector2 position)
    {
        Position = position;
        Show();
        GetNode<CollisionShape2D>("CollisionShape2D").Disabled = false;
    }

    public void OnBodyEntered(PhysicsBody2D body)
    {
        // Player disappears after being hit.
        Hide();
        EmitSignal(SignalName.Hit);
        // Must be deferred as we can't change physics properties on a physics callback.
        GetNode<CollisionShape2D>("CollisionShape2D").SetDeferred(CollisionShape2D.PropertyName.Disabled, true);
    }

    public override void _Ready()
    {
        _screenSize = GetViewportRect().Size;
        if (startHidden)
            Hide();
    }

    public override void _Process(double delta)
    {
        var velocity = new Vector2();

        if (Input.IsActionPressed("move_right"))
            velocity.X += 1;
        if (Input.IsActionPressed("move_left"))
            velocity.X -= 1;
        if (Input.IsActionPressed("move_down"))
            velocity.Y += 1;
        if (Input.IsActionPressed("move_up"))
            velocity.Y -= 1;

        var animatedSprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");

        if (velocity.Length() > 0f)
        {
            velocity = velocity.Normalized() * speed;
            animatedSprite.Play();
        }
        else
        {
            animatedSprite.Stop();
        }

        Position += velocity * (float)delta;
        Position = new Vector2(
            x: Mathf.Clamp(Position.X, 0, _screenSize.X),
            y: Mathf.Clamp(Position.Y, 0, _screenSize.Y)
        );

        if (velocity.X != 0f)
        {
            animatedSprite.Animation = "walk";
            animatedSprite.FlipV = velocity.Y > 0f;
            animatedSprite.FlipH = velocity.X < 0f;
        }
        else if (velocity.Y != 0f)
        {
            animatedSprite.Animation = "up";
            animatedSprite.FlipV = velocity.Y > 0f;
        }
    }
}
