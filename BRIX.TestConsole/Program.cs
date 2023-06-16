﻿using BRIX.Library.Characters;

Inventory testInventory = new()
{
    Coins = 250,
    Content = new List<InventoryItem>
    {
        new InventoryItem { Name = "Ремень с медной пряжкой", Description = "Пояс украшен металлической фурнитурой различного размера. При желании пряжка отстегивается." },
        new InventoryItem { Name = "Стильные сапоги" },
        new Container
        {
            Name = "Рюкзак",
            Payload = new List<InventoryItem>
            {
                new InventoryItem { Name = "Расчёска" },
                new InventoryItem { Name = "Бутерброд", Count = 5 },
                new InventoryItem { Name = "Кремень и кресало" },
                new InventoryItem { Name = "Мини-палатка" },
                new Container
                {
                    Name = "Шкатулка",
                    Payload = new List<InventoryItem>
                    {
                        new InventoryItem { Name = "Катушка ниток", Count = 10 },
                        new InventoryItem { Name = "Игла", Count = 4 },
                    }
                },
            }
        },
        new Container
        {
            Name = "Поясные сумки",
            Payload = new List<InventoryItem>
            {
                new Consumable { Name = "Металлический шарик", Count = 100, CoinsPrice = 1 },
                new InventoryItem { Name = "Расчёска" },
            }
        },
        new Equipment { Name = "Фамильный меч", Description = "Азот (Azoth) — магический меч великого лекаря (по легендам). Азот — это имя демона, заключённого в кристалл, использованный в эфесе этого оружия.", CoinsPrice = 100 },
    }
};

var contentList = testInventory.Items().Select(x => x.Name).ToList();

Console.ReadLine();