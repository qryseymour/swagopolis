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
    private Dictionary<Node, isolatedVelocity> forceDic_forces = new Dictionary<Node, isolatedVelocity>();

    public forceDictionary(Dictionary<Node, isolatedVelocity> forces = null) {
        if (forces != null) {
            forceDic_forces = forces;
        }
    }

    public forceDictionary(forceDictionary forceDic) {
        forceDic_forces = new Dictionary<Node, isolatedVelocity>(forceDic.forceDic_forces);
    }

    public Vector2 extractAllForcesPerFrame() {
        /*
            This gets all of the applied forces of the dictionary,
            puts them together, subtracts a frame, then returns
            the result as the culimation of all acting forces.
        */
        Vector2 velocity = new Vector2();
        foreach(KeyValuePair<Node, isolatedVelocity> entry in forceDic_forces)
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
        foreach(KeyValuePair<Node, isolatedVelocity> entry in forceDic_forces)
        {
            if (entry.Value.iso_frames > 0) {
                velocity += entry.Value.getVector2();
            } else {
                forceDic_forces.Remove(entry.Key);
            }
        }
        return velocity;
    }

    public bool addVelocity(Node node, isolatedVelocity isoVelocity) {
        bool addedVelocity = false;
        if (!forceDic_forces.ContainsKey(node)) {
            forceDic_forces.Add(node, new isolatedVelocity(isoVelocity));
            addedVelocity = true;
        }
        return addedVelocity;
    }

    public bool modifyVelocity(Node node, isolatedVelocity isoVelocity) {
        bool modifiedVelocity = false;
        if (forceDic_forces.ContainsKey(node)) {
            forceDic_forces[node] = new isolatedVelocity(isoVelocity);
            modifiedVelocity = true;
        }
        return modifiedVelocity;
    }

    public void addOrModifyVelocity(Node node, isolatedVelocity isoVelocity) {
        if (forceDic_forces.ContainsKey(node)) {
            forceDic_forces[node] = new isolatedVelocity(isoVelocity);
        } else {
            forceDic_forces.Add(node, new isolatedVelocity(isoVelocity));
        }
    }

    public bool removeVelocity(Node node) {
        bool removedVelocity = false;
        if (forceDic_forces.ContainsKey(node)) {
            forceDic_forces.Remove(node);
            removedVelocity = true;
        }
        return removedVelocity;
    }





    // Operator Overloads
    public static forceDictionary operator + (forceDictionary left, forceDictionary right) {
        forceDictionary merged = new forceDictionary(left);
        foreach(KeyValuePair<Node, isolatedVelocity> entry in right.forceDic_forces)
        {
            merged.addVelocity(entry.Key, entry.Value);
        }
        return merged;
    }
    
    public static explicit operator Vector2(forceDictionary forceDic) {
        return forceDic.extractAllForcesPerFrame();
    }
}