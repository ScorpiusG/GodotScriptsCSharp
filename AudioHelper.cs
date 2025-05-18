/*
 *  AudioHelper by ScorpiusG/AATTVVV
 * 
 *  HOW TO USE
 *  1. Create a scene, with the base being Node. Attach this script on it.
 *  2. Save this scene and assign it in Autoplay
 *		(or as a child of another Node already assigned into Autoplay).
 *  3. Create AudioStreamPlayer children; then set a unique name for each child,
 *		and its respective sound effect and settings (such as audio bus to "SFX", etc).
 *  4. Then go to other scripts and call either static method:
 *		AudioHelper.PlaySound(childNameHere);
 *		AudioHelper.PlayMusic(childNameHere);
 *	5. If the assigned audio plays music, looping, or long audio clips,
 *		these static methods also exist:
 *		AudioHelper.StopSound(childNameHere);
 *		AudioHelper.StopMusic(childNameHere); // Is also automatically called during PlayMusic
 *	6. You can use these methods to get and set the audio volume of a bus
 *	    in a float value ranging from 0f to 1f:
 *	    GetBusVolumePercent(busName);
 *	    SetBusVolumePercent(busName, volume);
 *  
 *  Example hierarchy:
 *  Node <-- ATTACH THIS SCRIPT HERE!
 *  - AudioStreamPlayer, named "Music1"
 *  - AudioStreamPlayer, named "Music2"
 *  - AudioStreamPlayer, named "Click"
 *  - AudioStreamPlayer, named "Explosion"
 *  - AudioStreamPlayer, named "Coin"
 *  
 *  Example script usage:
 *  // One-time sound effect playback
 *      AudioHelper.PlaySound("Click");
 *      AudioHelper.PlaySound("Explosion");
 *      AudioHelper.PlaySound("Coin");
 *  // Cut off the audio before it finishes
 *	    AudioHelper.StopSound("Explosion");
 *	// Music switching
 *      AudioHelper.PlayMusic("Music1");
 *      AudioHelper.PlayMusic("Music2"); // Stops Music1 and plays Music2
 *      AudioHelper.StopMusic("Music2");
 *  
 *	HOW IT WORKS
 *	    What this script does is get its children (assumed as AudioStreamPlayers)
 *	    on Node Start and stores it for reference (in a Dictionary),
 *	    which is later used by any of the listed methods above.
 *	    
 *	    For expert programmers: If you alter this Node's children during runtime,
 *	    you can call this static method to refresh the reference list.
 *	    AudioHelper.LinkChildren();
 */

using Godot;
using Godot.Collections;

namespace Game.Autoload;

/// <summary>
/// A class that plays audio easily with in-editor and minor scripting setup.
/// Please open the script and read its instructions.
/// </summary>
public partial class AudioHelper : Node
{
	private static AudioHelper instance;

    /// <summary>
    /// If your game starts with the music being Autoplayed,
    /// set this name to match that Audio Stream Player's Name
    /// in the Inspector.
    /// </summary>
    [Export] private string currentMusicNodeName = string.Empty;

    private Dictionary<string, AudioStreamPlayer> data = [];
	private AudioStreamPlayer streamPlayerMusic;

	public override void _Notification(int what)
	{
		if (what == NotificationSceneInstantiated)
		{
			instance = this;
		}
    }

    public override void _Ready()
    {
        LinkChildren();
    }

    public static void LinkChildren()
    {
        instance.data = [];
        var children = instance.GetChildren();
        foreach (var child in children)
        {
            instance.data[child.Name] = child as AudioStreamPlayer;
        }
    }

    public static void PlaySound(string name)
	{
        var node = instance.GetAudioStreamPlayer(name);
		if (node == null)
		{
			GD.PushWarning($"[AudioHelper.PlaySound] The child \"{name}\" does not exist.");
			return;
		}
		node.Play();
    }
    public static void StopSound(string name)
    {
        var node = instance.GetAudioStreamPlayer(name);
        if (node == null)
        {
            GD.PushWarning($"[AudioHelper.StopSound] The child \"{name}\" does not exist.");
            return;
        }
        node.Stop();
    }

    public static void PlayMusic(string name)
    {
        if (!string.IsNullOrEmpty(instance.currentMusicNodeName))
        {
            StopMusic(instance.currentMusicNodeName);
        }
        var node = instance.GetAudioStreamPlayer(name);
        if (node == null)
        {
            GD.PushWarning($"[AudioHelper.PlayMusic] The child \"{name}\" does not exist.");
            return;
        }
        node.Play();
        instance.currentMusicNodeName = name;
    }
    public static void StopMusic(string name)
    {
        var node = instance.GetAudioStreamPlayer(name);
        if (node == null)
        {
            GD.PushWarning($"[AudioHelper.StopMusic] The child \"{name}\" does not exist.");
            return;
        }
        node.Stop();
        instance.currentMusicNodeName = string.Empty;
    }

    private AudioStreamPlayer GetAudioStreamPlayer(string name)
    {
        return !data.TryGetValue(name, out AudioStreamPlayer value) ? null : value;
    }

    public static void SetBusVolumePercent(string busName, float volume)
    {
        var busIndex = AudioServer.GetBusIndex(busName);
        AudioServer.SetBusVolumeDb(busIndex, Mathf.LinearToDb(volume));
    }

    public static float GetBusVolumePercent(string busName)
    {
        var busIndex = AudioServer.GetBusIndex(busName);
        return Mathf.DbToLinear(AudioServer.GetBusVolumeDb(busIndex));
    }
}