using Godot;
using System;
using System.Collections.Generic;

/// <summary>
/// The force dictionary is a way to carry isolated velocity 
/// forces onto an entity. Due to the ambiguity of how velocities
/// are applied and in which order, this dictionary serves to
/// hold the conserved forces into one body. This may be
/// depreciated by better coding practices.
/// </summary> 
public class forceDictionary
{
    private Dictionary<string, isolatedVelocity> forceDic_forces = new Dictionary<string, isolatedVelocity>();

    public forceDictionary(Dictionary<string, isolatedVelocity> forces = null) {
        if (forces != null) {
            forceDic_forces = forces;
        }
    }

    public forceDictionary(forceDictionary forceDic) {
        forceDic_forces = new Dictionary<string, isolatedVelocity>(forceDic.forceDic_forces);
    }

    public Vector2 extractAllForcesPerFrame() {
        /*
            This gets all of the applied forces of the dictionary,
            puts them together, subtracts a frame, then returns
            the result as the culimation of all acting forces.
        */
        Vector2 velocity = new Vector2();
        foreach(KeyValuePair<string, isolatedVelocity> entry in forceDic_forces)
        {
            if (entry.Value.iso_frames > 0) {
                velocity += entry.Value.getVector2();
                if (entry.Value.iso_frames > 0) {
                    entry.Value.iso_frames--;
                }
            } else {
                forceDic_forces.Remove(entry.Key);
            }
        }
        return velocity;
    }

    public Vector2 extractAllForces() {
        Vector2 velocity = new Vector2();
        foreach(KeyValuePair<string, isolatedVelocity> entry in forceDic_forces)
        {
            if (entry.Value.iso_frames > 0) {
                velocity += entry.Value.getVector2();
            } else {
                forceDic_forces.Remove(entry.Key);
            }
        }
        return velocity;
    }

    public bool addVelocity(string str, isolatedVelocity isoVelocity) {
        bool addedVelocity = false;
        if (!forceDic_forces.ContainsKey(str)) {
            forceDic_forces.Add(str, new isolatedVelocity(isoVelocity));
            addedVelocity = true;
        }
        return addedVelocity;
    }

    public bool modifyVelocity(string str, isolatedVelocity isoVelocity) {
        bool modifiedVelocity = false;
        if (forceDic_forces.ContainsKey(str)) {
            forceDic_forces[str] = new isolatedVelocity(isoVelocity);
            modifiedVelocity = true;
        }
        return modifiedVelocity;
    }

    public void addOrModifyVelocity(string str, isolatedVelocity isoVelocity) {
        if (forceDic_forces.ContainsKey(str)) {
            forceDic_forces[str] = new isolatedVelocity(isoVelocity);
        } else {
            forceDic_forces.Add(str, new isolatedVelocity(isoVelocity));
        }
    }

    public bool removeVelocity(string str) {
        bool removedVelocity = false;
        if (forceDic_forces.ContainsKey(str)) {
            forceDic_forces.Remove(str);
            removedVelocity = true;
        }
        return removedVelocity;
    }





    // Operator Overloads
    public static forceDictionary operator + (forceDictionary left, forceDictionary right) {
        forceDictionary merged = new forceDictionary(left);
        foreach(KeyValuePair<string, isolatedVelocity> entry in right.forceDic_forces)
        {
            merged.addVelocity(entry.Key, entry.Value);
        }
        return merged;
    }
    
    public static explicit operator Vector2(forceDictionary forceDic) {
        return forceDic.extractAllForcesPerFrame();
    }
}