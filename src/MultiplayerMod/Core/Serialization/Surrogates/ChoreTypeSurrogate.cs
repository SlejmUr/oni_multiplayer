using System.Runtime.Serialization;

namespace MultiplayerMod.Core.Serialization.Surrogates;

internal class ChoreTypeSurrogate : ISerializationSurrogate, ISurrogateType
{
    public Type Type => typeof(ChoreType);

    public void GetObjectData(object obj, SerializationInfo info, StreamingContext context)
    {
        var choreType = (ChoreType) obj;
        info.AddValue("id", choreType.Id);
    }

    public object SetObjectData(
        object obj,
        SerializationInfo info,
        StreamingContext context,
        ISurrogateSelector selector
    )
    {
        var id = info.GetString("id");
        return Db.Get().ChoreTypes.Get(id);
    }
}
