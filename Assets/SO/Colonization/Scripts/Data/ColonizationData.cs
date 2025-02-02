
using UnityEngine;

using Leopotam.EcsLite;

namespace SO.Colonization
{
    public class ColonizationData : MonoBehaviour
    {
        public int colonizationProgressMax;

        public static void LandColonizeRequest(
            EcsWorld world,
            EcsPool<RLandColonize> requestPool,
            EcsPackedEntity newOwnerPE,
            EcsPackedEntity landPE)
        {
            //Создаём новую сущность и назначаем ей запрос
            int requestEntity = world.NewEntity();
            ref RLandColonize requestComp = ref requestPool.Add(requestEntity);

            //Заполняем данные запроса
            requestComp = new(
                newOwnerPE,
                landPE);
        }

        internal static void ColoniesOwnerRemoveColony(
            EcsWorld world,
            EcsPool<CAgentColoniesOwner> aColoniesOwnerPool,
            ref CLandColony landColony)
        {
            //Берём владельца колонии
            landColony.ownerPE.Unpack(world, out int aColoniesOwnerEntity);
            ref CAgentColoniesOwner aColoniesOwner = ref aColoniesOwnerPool.Get(aColoniesOwnerEntity);

            //Удаляем данную колонию из его списка
            aColoniesOwner.ownedColonyPEs.Remove(landColony.selfPE);

            //Проверяем, есть ли у владельца ещё колонии
            ColoniesOwnerCheckColonies(
                world,
                aColoniesOwnerPool,
                ref aColoniesOwner);

            //Удаляем владельца из данных колонии
            landColony.ownerPE = new();
        }

        static void ColoniesOwnerCheckColonies(
            EcsWorld world,
            EcsPool<CAgentColoniesOwner> aColoniesOwnerPool,
            ref CAgentColoniesOwner aColoniesOwner)
        {
            //Если в списке владельца нет колоний
            if (aColoniesOwner.ownedColonyPEs.Count == 0)
            {
                //Берём сущность владельца
                aColoniesOwner.selfPE.Unpack(world, out int aColoniesOwnerEntity);

                //Удаляем компонент владения
                aColoniesOwnerPool.Del(aColoniesOwnerEntity);
            }
        }
    }
}
