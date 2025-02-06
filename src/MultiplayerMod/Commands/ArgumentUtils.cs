using OniMP.Core.Objects.Resolvers;
using OniMP.Core.Serialization.Surrogates;
using OniMP.Extensions;
using System.Reflection;
using UnityEngine;

namespace OniMP.Commands;

/// <summary>
/// Helping argument serializations.
/// </summary>
public static class ArgumentUtils
{
    /// <summary>
    /// Make <paramref name="objects"/> as the best serializable objects
    /// </summary>
    /// <param name="objects"></param>
    /// <returns></returns>
    public static object[] WrapObjects(object[] objects)
    {
        return objects.Select(WrapObject).ToArray();
    }

    /// <summary>
    /// Make serialized <paramref name="objects"/> to original objects
    /// </summary>
    /// <param name="objects"></param>
    /// <returns></returns>
    public static object[] UnWrapObjects(object[] objects)
    {
        return objects.Select(UnWrapObject).ToArray();
    }

    /// <summary>
    /// Make <paramref name="obj"/> as the best serializable object
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    /// <exception cref="Exception">Type is not serializable</exception>
    public static object WrapObject(object obj)
    {
        return obj switch
        {
            null => null,
            GameObject gameObject => gameObject.GetGOResolver(),
            KMonoBehaviour kMonoBehaviour => kMonoBehaviour.GetComponentResolver(),
            Delegate action => new DelegateRef(action.GetType(), WrapObject(action.Target), action.Method),
            FetchOrder2 order2 => new FetchOrder2Ref(order2),
            _ => obj.GetType().IsSerializable || SerializationSurrogates.HasSurrogate(obj.GetType())
                ? obj
                : throw new Exception($"Type {obj.GetType()} is not serializable")
        };
    }

    /// <summary>
    /// Resolve serialized <paramref name="obj"/> to original objects
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    public static object UnWrapObject(object obj) => obj is IResolver reference ? reference.Resolve() : obj;

    // TODO: Mitigate RCE
    [Serializable]
    internal class DelegateRef(
        Type DelegateType,
        object Target,
        MethodInfo MethodInfo
    ) : IResolver
    {
        public object Resolve()
        {
            return Delegate.CreateDelegate(
                DelegateType,
                UnWrapObject(Target),
                MethodInfo
            );
        }
    }

    [Serializable]
    internal class FetchOrder2Ref(
        HashSet<Tag> tags,
        FetchList2Ref list2Ref,
        ComponentResolver<CreatureDeliveryPoint> creatureDeliveryPointReference
    ) : IResolver
    {
        HashSet<Tag> Tags => tags;
        FetchList2Ref List2Ref => list2Ref;
        ComponentResolver<CreatureDeliveryPoint> CreatureDeliveryPointReference => creatureDeliveryPointReference;


        public FetchOrder2Ref(FetchOrder2 fetchOrder2) : this(
            fetchOrder2.Tags,
            fetchOrder2.OnComplete.Target is FetchList2 fetchList2 ? new FetchList2Ref(fetchList2) : null,
            fetchOrder2.OnComplete.Target is CreatureDeliveryPoint creatureDeliveryPoint
                ? creatureDeliveryPoint.GetComponentResolver()
                : null
        )
        { }

        public object Resolve()
        {
            var list = List2Ref?.GetFetchList2();
            var creatureDeliveryPoint = CreatureDeliveryPointReference?.Resolve();

            var fetchOrders = list?.FetchOrders ?? creatureDeliveryPoint?.fetches;
            if (fetchOrders == null)
            {
                return new FetchOrder2(null, Tags, FetchChore.MatchCriteria.MatchTags, null, null, null, 1);
            }
            return fetchOrders.Single(order => order.Tags.SequenceEqual(Tags));
        }

        public virtual bool Equals(FetchOrder2Ref other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;

            return Tags.SequenceEqual(other.Tags) && Equals(List2Ref, other.List2Ref) && Equals(
                CreatureDeliveryPointReference,
                other.CreatureDeliveryPointReference
            );
        }

        public override int GetHashCode()
        {
            var hashCode = Tags.GetHashCode();
            hashCode = hashCode * 397 ^ (List2Ref != null ? List2Ref.GetHashCode() : 0);
            hashCode = hashCode * 397 ^ (CreatureDeliveryPointReference != null
                ? CreatureDeliveryPointReference.GetHashCode()
                : 0);
            return hashCode;
        }
    }

    [Serializable]
    internal class FetchList2Ref(
        ComponentResolver<Storage> StorageReference,
        ChoreType ComplexFabricatorChoreType,
        StateMachineResolver StateMachineReference,
        ComponentResolver<ComplexFabricator> ComplexFabricatorReference,
        ComponentResolver<Constructable> ConstructableReference,
        FilteredStorageRef FilteredStorageReference,
        ComponentResolver<ManualDeliveryKG> ManualDeliveryKGReference
    )
    {

        public FetchList2Ref(FetchList2 fetchList2) : this(
            fetchList2.Destination.GetComponentResolver(),
            fetchList2.choreType,
            fetchList2.OnComplete.Target is StateMachine.Instance smi ? smi.GetSMResolver() : null,
            fetchList2.OnComplete.Target is ComplexFabricator complexFabricator
                ? complexFabricator.GetComponentResolver()
                : null,
            fetchList2.OnComplete.Target is Constructable constructable ? constructable.GetComponentResolver() : null,
            fetchList2.OnComplete.Target is FilteredStorage storage ? new FilteredStorageRef(storage) : null,
            fetchList2.OnComplete.Target is ManualDeliveryKG manualDeliveryKg ? manualDeliveryKg.GetComponentResolver() : null
        )
        { }

        public FetchList2 GetFetchList2()
        {
            var fetchListList = ComplexFabricatorReference?.Resolve().fetchListList ??
                                StateMachineReference?.Resolve()?.dataTable.OfType<FetchList2>().ToList();
            return fetchListList?.Single(
                       fetchList => Equals(fetchList.Destination.GetComponentResolver(), StorageReference) &&
                                    fetchList.choreType == ComplexFabricatorChoreType
                   ) ??
                   ConstructableReference?.Resolve().fetchList ??
                   FilteredStorageReference?.GetFilteredStorage().fetchList ??
                   ManualDeliveryKGReference?.Resolve().fetchList;
        }
    }

    [Serializable]
    internal class FilteredStorageRef(ComponentResolver RootReference)
    {

        private readonly static BindingFlags bindingFlags = BindingFlags.NonPublic | BindingFlags.Instance;

        public FilteredStorageRef(FilteredStorage filteredStorage) : this(
            filteredStorage.root.GetComponentResolver()
        )
        { }

        public FilteredStorage GetFilteredStorage()
        {
            var root = RootReference.Resolve();
            var type = root!.GetType();

            var field = type.GetField("storageFilter", bindingFlags) ??
                        type.GetField("filteredStorage", bindingFlags) ??
                        type.GetField("foodStorageFilter", bindingFlags);
            return (FilteredStorage)field!.GetValue(root);
        }
    }

    [Serializable]
    internal class GameStateMachineFetchListRef;

}