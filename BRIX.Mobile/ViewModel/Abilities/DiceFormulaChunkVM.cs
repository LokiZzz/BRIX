using BRIX.Library.DiceValue;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BRIX.Mobile.ViewModel.Abilities
{
    /// <summary>
    /// Позже можно украсить эти кусочки формулы иконками разных типов дайсов
    /// </summary>
    public class DiceFormulaChunkVM
    {
        public Dice Dice { get; set; }

        public int Modifier { get; set; }

        public string ChunkText => Dice != null && Dice.Count > 0
            ? $"{Dice.Count}d{Dice.NumberOfFaces}"
            : $"+{Modifier}";

        public static ObservableCollection<DiceFormulaChunkVM> GetChunks(DicePool dicePool)
        {
            List<DiceFormulaChunkVM> chunks = new();

            if (dicePool != null)
            {
                if (dicePool.Dice.Any())
                {
                    chunks.AddRange(dicePool.Dice.Select(x => new DiceFormulaChunkVM { Dice = x }));
                }

                if (dicePool.Modifier > 0)
                {
                    chunks.Add(new DiceFormulaChunkVM { Modifier = dicePool.Modifier });
                }
            }

            return new(chunks);
        }
    }
}
