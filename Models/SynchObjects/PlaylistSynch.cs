using ShareInstances.Instances;
using ShareInstances.Exceptions.SynchAreaExceptions;

using System;
using System.Collections.Generic;
using System.IO;
using SynchronizationBlock.Models.SynchObjects.Base;
using Newtonsoft.Json;
using System.Linq;

namespace SynchronizationBlock.Models.SynchObjects;
public class PlaylistSynch
{
    private string output_pathway = Environment.CurrentDirectory + "/ild_music_playlists.json";

    IList<Playlist> playlists = new List<Playlist>();

    public IList<Playlist> Instances => playlists;

    private string path;
    public string Prefix
    {
        get => path;
        
        set
        {
            path = value;
            output_pathway = path + "/ild_music_playlists.json";
        }
    }



    public void AddInstance(Playlist playlist)
    {            
        if(playlists.Where(p => p.Name.Equals(playlist.Name)).Count() > 0)
        {
            return;
        }
        playlists.Add(playlist);
    }
    
    public void EditInstance(Playlist new_playlist)
    {
        try
        {
            Playlist old = playlists.FirstOrDefault<Playlist>(p => p.Id.Equals(new_playlist.Id));

            old.Name = new_playlist.Name;
            old.Description = new_playlist.Description;
            old.AvatarBase64 = new_playlist.AvatarBase64;
            old.RecoverTracks(new_playlist.GetTracks().ToList());
        }
        catch(Exception ex)
        {
            throw new Exception($"Edit Error {ex.Message}");
        }


        // var playlist = playlists.First(a => a.Id.Equals(playlist_update.Id));
        // var index = playlists.IndexOf(playlist);
        // playlists[index] = playlist_update;
    }

    public void RemoveInstance(Playlist playlist)
    {
        if (playlists.Contains(playlist))
            playlists.Remove(playlist);
    }



    public void Serialize()
    {
        try
        {
            string jsonString = JsonConvert.SerializeObject(this.playlists);
            File.WriteAllText(output_pathway, string.Empty);
            File.WriteAllText(output_pathway, jsonString);
        }
        catch (Exception)
        {
            throw;
        }
    }

    public void Deserialize()
    {
        try 
        {
            string jsonString = File.ReadAllText(output_pathway);
            playlists = JsonConvert.DeserializeObject<List<Playlist>>(jsonString);
        }
        catch (FileNotFoundException fileNotFound)
        {

        }
        catch (Exception)
        {
            throw;
        }
    }

    public void Update()
    {
        throw new NotImplementedException();
    }
}