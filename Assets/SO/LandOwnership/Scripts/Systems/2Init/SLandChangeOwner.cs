
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace SO.LandOwnership
{
    public class SLandChangeOwner : IEcsInitSystem, IEcsRunSystem
    {
        readonly EcsWorldInject world = default;


        readonly EcsPoolInject<CAgentLandOwner> aLandOwnerPool = default;

        readonly EcsPoolInject<CLandOwned> landOwnedPool = default;

        public void Init(IEcsSystems systems)
        {
            //Изменяем владельцев земли
            LandsChangeOwner();
        }
        
        public void Run(IEcsSystems systems)
        {
            //Изменяем владельцев земли
            LandsChangeOwner();
        }

        readonly EcsFilterInject<Inc<RLandChangeOwner>> landChangeOwnerRFilter = default;
        readonly EcsPoolInject<RLandChangeOwner> landChangeOwnerRPool = default;
        void LandsChangeOwner()
        {
            //Для каждого запроса изменения владельца земли
            foreach(int requestEntity in landChangeOwnerRFilter.Value)
            {
                //Берём запрос
                ref RLandChangeOwner requestComp = ref landChangeOwnerRPool.Value.Get(requestEntity);

                //Изменяем владельца земли
                LandChangeOwner(ref requestComp);

                //Удаляем запрос
                landChangeOwnerRPool.Value.Del(requestEntity);
            }
        }

        void LandChangeOwner(
            ref RLandChangeOwner requestComp)
        {
            //Берём сущность земли, владельца которой требуется изменить
            requestComp.landPE.Unpack(world.Value, out int landEntity);

            //Если она уже имеет владельца
            if (landOwnedPool.Value.Has(landEntity) == true)
            {
                //Берём компонент владения
                ref CLandOwned landOwned = ref landOwnedPool.Value.Get(landEntity);

                //Удаляем землю из списка владельца
                LandOwnerRemoveLand(ref landOwned);

                //Если PE нового владельца не пуста
                if (requestComp.newOwnerPE.Unpack(world.Value, out int aLandOwnerEntity))
                {
                    //Заносим землю в список владельца
                    LandOwnerAddLand(
                        requestComp.newOwnerPE,
                        ref landOwned);
                }
                //Иначе
                else
                {
                    //Удаляем компонент владения
                    landOwnedPool.Value.Del(landEntity);
                }
            }
            //Иначе
            else
            {
                //Если PE нового владельца не пуста
                if (requestComp.newOwnerPE.Unpack(world.Value, out int aLandOwnerEntity))
                {
                    //Назначаем земле компонент владения
                    ref CLandOwned landOwned = ref landOwnedPool.Value.Add(landEntity);

                    //Заполняем основные данные компонента
                    landOwned = new(world.Value.PackEntity(landEntity));

                    //Заносим землю в список владельца
                    LandOwnerAddLand(
                        requestComp.newOwnerPE,
                        ref landOwned);
                }
                //Иначе ничего не происходит
            }
        }

        void LandOwnerRemoveLand(
            ref CLandOwned landOwned)
        {
            //Берём текущего владельца
            landOwned.ownerPE.Unpack(world.Value, out int currentOwnerEntity);
            ref CAgentLandOwner aLandOwner = ref aLandOwnerPool.Value.Get(currentOwnerEntity);

            //Удаляем данную землю из его списка
            aLandOwner.ownedLandPEs.Remove(landOwned.selfPE);

            //Проверяем, есть ли у владельца ещё земля
            LandOwnerCheckLand(ref aLandOwner);

            //Удаляем владельца из данных земли
            landOwned.ownerPE = new();
        }

        void LandOwnerAddLand(
            EcsPackedEntity landOwnerPE,
            ref CLandOwned landOwned)
        {
            //Берём сущность владельца
            landOwnerPE.Unpack(world.Value, out int landOwnerEntity);

            //Если он не имеет компонента владения землёй
            if(aLandOwnerPool.Value.Has(landOwnerEntity) == false)
            {
                //Назначаем ему компонент владения 
                ref CAgentLandOwner newALandOwner = ref aLandOwnerPool.Value.Add(landOwnerEntity);

                //Заполняем основные данные компонента
                newALandOwner = new(
                    world.Value.PackEntity(landOwnerEntity));
            }

            //Берём компонент владения землёй
            ref CAgentLandOwner aLandOwner = ref aLandOwnerPool.Value.Get(landOwnerEntity);

            //Заносим землю в его список
            aLandOwner.ownedLandPEs.Add(landOwned.selfPE);

            //Заносим владельца в данные земли
            landOwned.ownerPE = aLandOwner.selfPE;
        }

        void LandOwnerCheckLand(
            ref CAgentLandOwner aLandOwner)
        {
            //Если в списке владельца больше нет земли
            if(aLandOwner.ownedLandPEs.Count == 0)
            {
                //Берём сущность владельца
                aLandOwner.selfPE.Unpack(world.Value, out int aLandOwnerEntity);

                //Удаляем компонент владения
                aLandOwnerPool.Value.Del(aLandOwnerEntity);
            }
        }
    }
}
