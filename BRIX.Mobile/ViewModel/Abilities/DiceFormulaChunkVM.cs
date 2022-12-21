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

        public string ChunkText 
        {
            get
            {
                if (Dice != null && Dice.Count > 0)
                {
                    return $"{Dice.Count}d{Dice.NumberOfFaces}";
                }
                else
                {
                    if (Modifier > 0) return $"+{Modifier}";
                    if (Modifier < 0) return $"—{Math.Abs(Modifier)}";

                    return string.Empty;
                }
            }
        }

        public static ObservableCollection<DiceFormulaChunkVM> GetChunks(DicePool dicePool)
        {
            List<DiceFormulaChunkVM> chunks = new();

            if (dicePool != null)
            {
                if (dicePool.Dice.Any())
                {
                    chunks.AddRange(dicePool.Dice.Select(x => new DiceFormulaChunkVM { Dice = x }));
                }

                if (dicePool.Modifier != 0)
                {
                    chunks.Add(new DiceFormulaChunkVM { Modifier = dicePool.Modifier });
                }
            }

            return new(chunks);
        }
    }
}
