using Godot;
using System;

public sealed partial class hud : CanvasLayer
{
    [Signal]
    public delegate void StartGameEventHandler();

    [Export]
    private NodePath messageNodePath;
    [Export]
    private NodePath messageTimerNodePath;
    [Export]
    private NodePath startButtonNodePath;
    [Export]
    private NodePath scoreLabelNodePath;

    public void ShowMessage(string text)
    {
        var messageNode = GetNode<Label>(messageNodePath);
        messageNode.Text = text;
        messageNode.Show();

        GetNode<Timer>(messageTimerNodePath).Start();
    }

    public async void ShowGameOver()
    {
        ShowMessage("Game Over");

        var messageTimer = GetNode<Timer>(messageTimerNodePath);
        await ToSignal(messageTimer, Timer.SignalName.Timeout);

        var messageNode = GetNode<Label>(messageNodePath);
        messageNode.Text = "Dodge the\nCreeps!";
        messageNode.Show();

        await ToSignal(GetTree().CreateTimer(1.0), SceneTreeTimer.SignalName.Timeout);
        GetNode<Button>(startButtonNodePath).Show();
    }

    public void UpdateScore(int score)
    {
        GetNode<Label>(scoreLabelNodePath).Text = score.ToString();
    }

    private void OnStartButtonPressed()
    {
        GetNode<Button>(startButtonNodePath).Hide();
        EmitSignal(SignalName.StartGame);
    }

    private void OnMessageTimerTimeout()
    {
        GetNode<Label>(messageNodePath).Hide();
    }
}
