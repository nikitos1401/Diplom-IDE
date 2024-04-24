using System.Collections.Generic;

namespace Diplom.Repository
{
    public static class DataRepository
    {
        public static Dictionary<int, string> GetAreas()
        {
            var areas = new Dictionary<int, string>
            {
                { 0, "Фермерське господарство" },
                { 1, "Теплиця" }
            };

            return areas;
        }

        public static Dictionary<int, string> GetAreaItems(int areaId)
        {
            var areaItems = new Dictionary<int, string>();

            switch (areaId)
            {
                case 0:
                    areaItems.Add(0, "Корови");
                    areaItems.Add(1, "Свині");
                    areaItems.Add(2, "Кози");
                    areaItems.Add(3, "Кури");
                    break;
                case 1:
                    areaItems.Add(0, "Огірки");
                    areaItems.Add(1, "Помідори");
                    areaItems.Add(2, "Перець");
                    areaItems.Add(3, "Кабачки");
                    break;
                default:
                    break;
            }

            return areaItems;
        }
    }
}