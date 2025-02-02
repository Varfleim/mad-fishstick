
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace SO.Colonization
{
    public class SLandColonize : IEcsRunSystem
    {
        readonly EcsWorldInject world = default;


        readonly EcsPoolInject<CAgentColoniesOwner> aColoniesOwnerPool = default;

        readonly EcsPoolInject<CLandColony> landColonyPool = default;

        public void Run(IEcsSystems systems)
        {
            //Колонизируем землю
            LandsColonize();
        }

        readonly EcsFilterInject<Inc<RLandColonize>> landColonizeRFilter = default;
        readonly EcsPoolInject<RLandColonize> landColonizeRPool = default;
        void LandsColonize()
        {
            //Для каждого запроса колонизации земли
            foreach (int requestEntity in landColonizeRFilter.Value)
            {
                //Берём запрос
                ref RLandColonize requestComp = ref landColonizeRPool.Value.Get(requestEntity);

                //Колонизируем землю
                LandColonize(ref requestComp);

                //Удаляем запрос
                landColonizeRPool.Value.Del(requestEntity);
            }
        }

        void LandColonize(
            ref RLandColonize requestComp)
        {
            //Берём сущность земли, которую требуется колонизировать
            requestComp.landPE.Unpack(world.Value, out int landEntity);

            //Если она уже имеет колонию
            if(landColonyPool.Value.Has(landEntity) == true)
            {
                //Берём компонент колонии
                ref CLandColony landColony = ref landColonyPool.Value.Get(landEntity);

                //Удаляем колонию из списка владельца
                ColonizationData.ColoniesOwnerRemoveColony(
                    world.Value,
                    aColoniesOwnerPool.Value,
                    ref landColony);

                //Если PE нового владельца не пуста
                if (requestComp.newOwnerPE.Unpack(world.Value, out int aColoniesOwnerEntity))
                {
                    //Заносим колонию в список владельца
                    ColoniesOwnerAddColony(
                        requestComp.newOwnerPE,
                        ref landColony);
                }
                //Иначе
                else
                {
                    //Удаляем компонент колонии
                    landColonyPool.Value.Del(landEntity);
                }
            }
            //Иначе
            else
            {
                //Если PE нового владельца не пуста
                if(requestComp.newOwnerPE.Unpack(world.Value, out int aColoniesOwnerEntity))
                {
                    //Назначаем земле компонент колонии
                    ref CLandColony landColony = ref landColonyPool.Value.Add(landEntity);

                    //Заполняем основные данные компонента
                    landColony = new(world.Value.PackEntity(landEntity));

                    UnityEngine.Debug.LogWarning("Colony created!");

                    //Заносим колонию в список владельца
                    ColoniesOwnerAddColony(
                        requestComp.newOwnerPE,
                        ref landColony);
                }
                //Иначе ничего не происходит
            }
        }

        void ColoniesOwnerAddColony(
            EcsPackedEntity coloniesOwnerPE,
            ref CLandColony landColony)
        {
            //Берём сущность владельца
            coloniesOwnerPE.Unpack(world.Value, out int coloniesOwnerEntity);

            //Если он не имеет компонента владения колониями
            if (aColoniesOwnerPool.Value.Has(coloniesOwnerEntity) == false)
            {
                //Назначаем ему компонент владения 
                ref CAgentColoniesOwner newAColoniesOwner = ref aColoniesOwnerPool.Value.Add(coloniesOwnerEntity);

                //Заполняем основные данные компонента
                newAColoniesOwner = new(
                    world.Value.PackEntity(coloniesOwnerEntity));
            }

            //Берём компонент владения колониями
            ref CAgentColoniesOwner aColoniesOwner = ref aColoniesOwnerPool.Value.Get(coloniesOwnerEntity);

            //Заносим колонию в его список
            aColoniesOwner.ownedColonyPEs.Add(landColony.selfPE);

            //Заносим владельца в данные колонии
            landColony.ownerPE = aColoniesOwner.selfPE;
        }
    }
}
