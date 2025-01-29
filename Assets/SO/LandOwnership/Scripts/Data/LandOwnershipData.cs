
using UnityEngine;

using Leopotam.EcsLite;

namespace SO.LandOwnership
{
    public class LandOwnershipData : MonoBehaviour
    {
        public static void LandChangeOwnerRequest(
            EcsWorld world,
            EcsPool<RLandChangeOwner> requestPool,
            EcsPackedEntity newOwnerPE,
            EcsPackedEntity landPE)
        {
            //Создаём новую сущность и назначаем ей запрос
            int requestEntity = world.NewEntity();
            ref RLandChangeOwner requestComp = ref requestPool.Add(requestEntity);

            //Заполняем данные запроса
            requestComp = new(
                newOwnerPE,
                landPE);
        }
    }
}
