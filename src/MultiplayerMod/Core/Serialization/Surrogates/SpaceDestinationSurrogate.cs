using System.Runtime.Serialization;

namespace MultiplayerMod.Core.Serialization.Surrogates;

internal class SpaceDestinationSurrogate : ISerializationSurrogate, ISurrogateType
{
    public Type Type => typeof(SpaceDestination);

    public void GetObjectData(object obj, SerializationInfo info, StreamingContext context)
    {
        var destination = (SpaceDestination) obj;
        info.AddValue("id", destination.id);
    }

    public object SetObjectData(
        object obj,
        SerializationInfo info,
        StreamingContext context,
        ISurrogateSelector selector
    )
    {
        var id = info.GetString("id");
        return SpacecraftManager.instance.GetDestination(int.Parse(id));
    }
}
