using Godot;
using System;

/// <summary>
/// Isolated Velocities are independant velocities that hold
/// the frames they last, or if the force they are applying is
/// infinite. Good for things like knockback or constant forces.
/// </summary> 
public class isolatedVelocity
{
    public Vector2 iso_velocity;
    public uint iso_frames = 0;
    public bool iso_endless = false;
    private velocityFormula getVelocityOnFrame = (Vector2 ind_velocity, uint ind_frames) => { return ind_velocity; };
    public isolatedVelocity(Vector2 velocity, uint frames = 0, velocityFormula formula = null) {
        // All velocities received are multiplied by 60 to
        // account for delta.
        iso_velocity = velocity * 60;
        if (frames > 0) {
            iso_frames = frames;
        } else {
            iso_endless = true;
        }
        if (formula != null) {
            getVelocityOnFrame = formula;
        }
    }

    public isolatedVelocity(isolatedVelocity iso) {
        iso_velocity = iso.iso_velocity;
        iso_frames = iso.iso_frames;
        iso_endless = iso.iso_endless;
        getVelocityOnFrame = iso.getVelocityOnFrame;
    }

    public Vector2 getVector2() {
        return getVelocityOnFrame(iso_velocity, iso_frames);
    }





    // Operator Overloads
    public static bool operator ==(isolatedVelocity left, isolatedVelocity right) {
        return (left.iso_velocity == right.iso_velocity) && ((left.iso_frames == right.iso_frames) || (left.iso_endless == right.iso_endless));
    }

    public static bool operator !=(isolatedVelocity left, isolatedVelocity right) {
        return !(left == right);
    }

    public static isolatedVelocity operator + (Vector2 left, isolatedVelocity right) {
        return new isolatedVelocity(left + right.iso_velocity);
    }
    public static isolatedVelocity operator + (isolatedVelocity left, Vector2 right) {
        return new isolatedVelocity(left.iso_velocity + right);
    }

    public static isolatedVelocity operator + (isolatedVelocity left, uint right) {
        return new isolatedVelocity(left.iso_velocity, left.iso_frames + right, left.getVelocityOnFrame);
    }
    public static isolatedVelocity operator - (isolatedVelocity left, uint right) {
        return new isolatedVelocity(left.iso_velocity, left.iso_frames - right, left.getVelocityOnFrame);
    }
    public static isolatedVelocity operator * (isolatedVelocity left, uint right) {
        return new isolatedVelocity(left.iso_velocity, left.iso_frames * right, left.getVelocityOnFrame);
    }

    public static explicit operator Vector2(isolatedVelocity isoVelocity) {
        return isoVelocity.getVelocityOnFrame(isoVelocity.iso_velocity, isoVelocity.iso_frames);
    }

    public override bool Equals(object obj)
    {
        if (ReferenceEquals(this, obj))
        {
            return true;
        }

        if (ReferenceEquals(obj, null))
        {
            return false;
        }

        throw new System.NotImplementedException();
    }

    public override int GetHashCode()
    {
        throw new System.NotImplementedException();
        //Test
    }
}
public delegate Vector2 velocityFormula(Vector2 velocity, uint frames);