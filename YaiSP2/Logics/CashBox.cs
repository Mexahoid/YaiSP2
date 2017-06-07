using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Threading;

namespace YaispTwoEight
{
    public class CashBox
    {
        private Collections.MoneyList MoneyBox;
        private bool Active;
        private int[] MoneyStacksCount;

        public CashBox()
        {
            MoneyBox = new Collections.MoneyList();
            Active = false;
        }
        public void FillMoney(int[] Nom)
        {
            MoneyBox = new Collections.MoneyList();
            MoneyBox.FillMoney(Nom);
        }
        public int GetMoneyCount()
        {
            return MoneyBox.ReturnAllCashCount();
        }
        public int[,] GetMoneyArray()
        {
            return MoneyBox.GetMoneyArray();
        }
        public void Deactivate()
        {
            Active = false;
        }
        public void Activate()
        {
            Active = true;
        }
        public bool GetActiveState()
        {
            return Active;
        }
        public bool OrderMoneySum(int Value)
        {   //Коды возврата нельзя выдать, вообще не хватает
            MoneyStacksCount = MoneyBox.ReturnMoneyStackCount();
            int[] DesireStacks = GetValueToNoteConversion(Value);
            int L = MainUnitProcessor.GlobalNominals.Length;
            bool IsAvailable = true;
            for (int i = 0; i < L; i++)
            {
                if (MoneyStacksCount[i] < DesireStacks[i])
                {
                    IsAvailable = false;
                    if (i != L - 1)  //Если i четное - то в следующем номинале добавляем 5, иначе 2
                    {
                        DesireStacks[i + 1] += (i % 2 == 0 || i == 0) ?
                            (5 * (-MoneyStacksCount[i] + DesireStacks[i])) :
                            (2 * (-MoneyStacksCount[i] + DesireStacks[i]));
                        DesireStacks[i] = MoneyStacksCount[i];
                    }
                    else
                        break;
                }
                else
                    IsAvailable = true;
            }
            if (IsAvailable)
            {
                Collections.MoneyStack CurrentStack;
                for (int i = 0; i < L; i++)
                {
                    CurrentStack = MoneyBox.ReturnByIndex(i);
                    CurrentStack.TakeMoneyFromStack(DesireStacks[i]);
                }
                return true;
            }
            return false;
        }
        private int[] GetValueToNoteConversion(int Value)
        {
            int L = MainUnitProcessor.GlobalNominals.Length;
            int[] Counts = new int[L];
            for (int i = 0; i < L; i++)
            {
                Counts[i] = Value / MainUnitProcessor.GlobalNominals[i];
                Value %= MainUnitProcessor.GlobalNominals[i];
            }
            return Counts;
        }
    }
}
