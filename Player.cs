using Godot;
using System;

namespace DodgeTheCreeps;

public sealed partial class Player : Area2D
{
    [Export] private int speed = 400;
    private Vector2 _screenSize;

    public override void _Ready()
    {
        _screenSize = GetViewportRect().Size;
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
    }
}
