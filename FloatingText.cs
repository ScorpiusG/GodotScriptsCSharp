/*
 *  Floating Text by ScorpiusG/AATTVVV
 *  
 *  HOW TO USE
 *  1. Create a scene, with the base being Node3D or Node 2D. Attach this script on it.
 *  2. Skip this step if working on a 3D scene. Otherwise, change this line to use type of Label2D instead:
 *      [Export] private Label3D label;
 *  3. Create either a Label2D or Label3D child node,
 *      then assign it to this script's Label value in the Inspector.
 *  4. Create an AnimationPlayer child node. With it, make an animation.
 *      Assign it to Autoplay on Load. How you animate the text is entirely up to you,
 *      but you're recommended to use it to call the root node's queue_free() function at the end of animation.
 *  5. Save this scene somewhere. Copy its RELATIVE path
 *      and paste it in the constant variable's value near the top of the script:
 *      private const string SCENE_PATH = "paste-your-scene-path-here";
 *  6. In other scripts where needed, call this method onto a variable like this:
 *      var node = FloatingText.Instantiate();
 *      GetTree().Root.GetChild(0).AddChild(node);
 *  7. Under that line, you can set its position and contents like this:
 *      node.GlobalPosition = GlobalPosition;
 *      node.SetLabelText(content, textColor, outlineColor);
 *  8. Both text and outline color values are optional,
 *      but you can set the default values near the top of the script.
 */

using Godot;

namespace Game.UI;

public partial class FloatingText : Node3D
{
    private const string SCENE_PATH = "res://scenes/gameplay/floating_text.tscn";

    private readonly Color defaultTextBaseColor = Colors.White;
    private readonly Color defaultTextOutlineColor = Colors.Black;

    public static FloatingText Instantiate()
    {
        return ResourceLoader.Load<PackedScene>(SCENE_PATH).Instantiate() as FloatingText;
    }

    [Export] private Label3D label;

    public void SetLabelText(string content, Color textColor, Color outlineColor)
    {
        label.Text = content;
        label.Modulate = textColor;
        label.OutlineModulate = outlineColor;
    }
    public void SetLabelText(string content, Color textColor)
    {
        SetLabelText(content, textColor, defaultTextOutlineColor);
    }
    public void SetLabelText(string content)
    {
        SetLabelText(content, defaultTextBaseColor);
    }
}