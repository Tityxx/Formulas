using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tityx.FormulasSystem
{
    /// <summary>
    ///  Расчёт формулы по входным значениям. Поддерживаются:
    ///  Арифметические операции: +-*/
    ///  Степень и остаток от деления: ^%
    ///  Круглые скобки: ()
    ///  Математические функции: sqrt, floor, ceil, round
    ///  Тригонометрические функции: cos, sin, tan
    ///  Константы: pi
    /// </summary>
    /// <typeparam name="T">Поддерживаемые типы: int, float, long, double</typeparam>
    [Serializable]
    public class Formula<T> where T: IComparable
    {
        public string FormulaString => _formula;

        [SerializeField] protected string _formula = "{0}";

        protected bool _useMin;
        protected T _min;
        protected T _max;
        protected bool _useMax;

        public Formula(string formula)
        {
            _formula = formula;
        }

        public void SetMin(T min)
        {
            _useMin = true;
            _min = min;
        }

        public void SetMax(T max)
        {
            _useMax = true;
            _max = max;
        }

        /// <summary>
        /// При ошибке возвращает false, результат равен 0
        /// </summary>
        /// <param name="result"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public virtual bool TryCalculate(out T result, params T[] parameters)
        {
            //NOTE: Замена пустых ключей
            string formattedFormula = _formula;
            for (int i = parameters.Length; i <= 99; i++)
            {
                if (!formattedFormula.Contains($"{{{i}}}"))
                    continue;

                formattedFormula = formattedFormula.Replace($"{{{i}}}", "0");
            }
            var strValues = new string[parameters.Length];
            for (int i = 0; i < parameters.Length; i++)
            {
                strValues[i] = parameters[i].ToString();
                strValues[i] = strValues[i].Replace(',', '.');
            }

            formattedFormula = string.Format(formattedFormula, strValues);
#if UNITY_2022_3_OR_NEWER
            if (!ExpressionEvaluator.Evaluate(formattedFormula, out  result))
                return false;
#else
            if (!float.TryParse(new System.Data.DataTable().Compute(formattedFormula, "").ToString(), out result))
                return false;
#endif

            if (_useMin && result.CompareTo(_min) < 0)
                    result = _min;

            if (_useMax && result.CompareTo(_max) > 0)
                    result = _max;

            return true;
        }
    }
}