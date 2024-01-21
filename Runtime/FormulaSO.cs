using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tityx.FormulasSystem
{
    /// <summary>
    /// Обёртка формулы в SO
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class FormulaSO<T> : ScriptableObject where T : IComparable
    {
        [SerializeField] private bool _useMin;
        [SerializeField] private T _min;
        [SerializeField] private bool _useMax;
        [SerializeField] private T _max;
        [SerializeField] private Formula<T> _formula;

        public bool TryCalculate(out T result, params T[] parameters)
        {
            if (_useMin)
                _formula.SetMin(_min);
            if (_useMax)
                _formula.SetMax(_max);

            return _formula.TryCalculate(out result, parameters);
        }
    }
}