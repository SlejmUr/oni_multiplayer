namespace MultiplayerMod.Extensions;

/// <summary>
/// 
/// </summary>
public static class ImmigrantScreenExtensions
{

    private const int delayMS = 1;
    private const int maxWaitMS = 50;
    /// <summary>
    /// 
    /// </summary>
    /// <param name="instance"></param>
    /// <returns></returns>
    public static async Task<List<ITelepadDeliverable>> WaitForAllDeliverablesReady(ImmigrantScreen instance)
    {
        var currentDelay = 0;
        while (currentDelay < maxWaitMS)
        {
            var readyDeliverables = instance.containers?.Select(
                container => container switch
                {
                    CharacterContainer characterContainer => (ITelepadDeliverable)characterContainer.stats,
                    CarePackageContainer packageContainer => packageContainer.carePackageInstanceData,
                    _ => null
                }
            ).Where(deliverable => deliverable != null).ToList();
            if (readyDeliverables != null && readyDeliverables.Count == instance.containers?.Count)
                return readyDeliverables;

            await Task.Delay(delayMS);
            currentDelay += delayMS;
        }
        return null;
    }
}
