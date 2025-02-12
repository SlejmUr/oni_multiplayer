using MultiplayerMod.Commands.NetCommands;
using MultiplayerMod.Core.Objects;
using MultiplayerMod.Core.Wrappers;

namespace MultiplayerMod.Commands.Chores;

[Serializable]
public class CreateChoreCommand : BaseCommandEvent
{
    public MultiplayerId MultiId;
    public Type ChoreType;
    public object[] Arguments;

    public CreateChoreCommand(MultiplayerId id, Type choreType, object[] arguments)
    {
        MultiId = id;
        ChoreType = choreType;
        Arguments = ArgumentUtils.WrapObjects(ChoreArgumentsWrapper.Wrap(ChoreType, arguments));
    }
}
